using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Xml;

using IOPath = System.IO.Path;

namespace net.minecraft.src
{
    public class ResourceDownloader
    {
		/// <summary>
		/// The folder to store the resources in. </summary>
		public string ResourcesFolder;

		/// <summary>
		/// A reference to the Minecraft object. </summary>
		private Minecraft minecraft;

		/// <summary>
		/// Set to true when Minecraft is closing down. </summary>
		private bool closing;

        private Thread thread;

		public ResourceDownloader(string par1File, Minecraft mcInstance)
		{
			closing = false;
			minecraft = mcInstance;
            thread = new Thread(RunThread);
			thread.Name = "Resource download thread";
            thread.Priority = ThreadPriority.BelowNormal;
            thread.IsBackground = true;
			ResourcesFolder = System.IO.Path.Combine(par1File, "Resources/");

			if (!Directory.Exists(ResourcesFolder))
			{
                try
                {
                    Directory.CreateDirectory(ResourcesFolder);
                }
                catch
                {
				    throw new Exception((new StringBuilder()).Append("The working directory could not be created: ").Append(ResourcesFolder).ToString());
                }
			}
			else
			{
				return;
			}
		}

		public void Run()
		{
            thread.Start();
		}

        private void RunThread()
        {
			try
			{
				string url = "http://s3.amazonaws.com/MinecraftResources/";

                XmlDocument document = new XmlDocument();
                document.Load(url);
				XmlNodeList nodelist = document.GetElementsByTagName("Contents");

				for (int i = 0; i < 2; i++)
				{
					for (int j = 0; j < nodelist.Count; j++)
					{
						XmlNode node = nodelist.Item(j);

						if (node.NodeType != (XmlNodeType)1)
						{
							continue;
						}

                        XmlElement element = (XmlElement)node;
                        string s = element.GetElementsByTagName("Key").Item(0).ChildNodes.Item(0).Value;
                        long l = Convert.ToInt64(element.GetElementsByTagName("Size").Item(0).ChildNodes.Item(0).Value);

						if (l <= 0L)
						{
							continue;
						}

						DownloadAndInstallResource(url, s, l, i);

						if (closing)
						{
							return;
						}
					}
				}
			}
			catch (Exception exception)
			{
				LoadResource(ResourcesFolder, "");
				
                Utilities.LogException(exception);
			}
        }

		/// <summary>
		/// Reloads the resource folder and passes the resources to Minecraft to install.
		/// </summary>
		public void ReloadResources()
		{
			LoadResource(ResourcesFolder, "");
		}

		/// <summary>
		/// Loads a resource and passes it to Minecraft to install.
		/// </summary>
		private void LoadResource(string par1File, string par2Str)
		{
			string[] afile = Directory.GetFiles(par1File);

			for (int i = 0; i < afile.Length; i++)
			{
                string fileName = IOPath.GetFileName(afile[i]);

				if (Directory.Exists(afile[i]))
				{
					LoadResource(afile[i], (new StringBuilder()).Append(par2Str).Append(fileName).Append("/").ToString());
					continue;
				}

                try
                {
                    minecraft.InstallResource((new StringBuilder()).Append(par2Str).Append(fileName).ToString(), afile[i]);
                }
                catch (Exception exception)
                {
                    Utilities.LogException(exception);

                    Console.WriteLine((new StringBuilder()).Append("Failed to add ").Append(par2Str).Append(fileName).ToString());
                }
			}
		}

		/// <summary>
		/// Downloads the resource and saves it to disk then installs it.
		/// </summary>
		private void DownloadAndInstallResource(string url, string par2Str, long par3, int par5)
		{
			try
			{
				int i = par2Str.IndexOf("/");
				string s = par2Str.Substring(0, i);

				if (s.Equals("sound") || s.Equals("newsound"))
				{
					if (par5 != 0)
					{
						return;
					}
				}
				else if (par5 != 1)
				{
					return;
				}

				FileInfo file = new FileInfo(IOPath.Combine(ResourcesFolder, par2Str));

				if (!file.Exists || file.Length != par3)
				{
					file.Directory.Create();
					string s1 = par2Str.Replace(" ", "%20");
                    DownloadResource(IOPath.Combine(url, s1), file.FullName, par3);

					if (closing)
					{
						return;
					}
				}

				minecraft.InstallResource(par2Str, file.FullName);
			}
			catch (Exception exception)
			{
                Utilities.LogException(exception);
			}
		}

		/// <summary>
		/// Downloads the resource and saves it to disk.
		/// </summary>
        private void DownloadResource(string url, string par2File, long par3)
        {
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(url, par2File);
            }
        }

		/// <summary>
		/// Called when Minecraft is closing down.
		/// </summary>
		public void CloseMinecraft()
		{
			closing = true;
		}
	}
}
