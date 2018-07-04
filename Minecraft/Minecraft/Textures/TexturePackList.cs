using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace net.minecraft.src
{
	using net.minecraft.src;
    using IOPath = System.IO.Path;

	public class TexturePackList
	{
		/// <summary>
		/// The list of the available texture packs. </summary>
		private List<TexturePackBase> availableTexturePacks;

		/// <summary>
		/// The default texture pack. </summary>
		private TexturePackBase DefaultTexturePack;

		/// <summary>
		/// The TexturePack that will be used. </summary>
		public TexturePackBase SelectedTexturePack;
		private Dictionary<string, TexturePackBase> Field_6538_d;

		/// <summary>
		/// The Minecraft instance used by this TexturePackList </summary>
		private Minecraft Mc;

		/// <summary>
		/// The directory the texture packs will be loaded from. </summary>
		private string TexturePackDir;
		private string CurrentTexturePack;

		public TexturePackList(Minecraft par1Minecraft, string par2File)
		{
            availableTexturePacks = new List<TexturePackBase>();
			DefaultTexturePack = new TexturePackDefault(par1Minecraft);
            Field_6538_d = new Dictionary<string, TexturePackBase>();
			Mc = par1Minecraft;
			TexturePackDir = IOPath.Combine(par2File, "Texturepacks");

			if (Directory.Exists(TexturePackDir))
			{
				if (File.Exists(TexturePackDir))
				{
					File.Delete(TexturePackDir);
					Directory.CreateDirectory(TexturePackDir);
				}
			}
			else
			{
				Directory.CreateDirectory(TexturePackDir);
			}

			CurrentTexturePack = par1Minecraft.GameSettings.Skin;
			UpdateAvaliableTexturePacks();
			SelectedTexturePack.Func_6482_a();
		}

		/// <summary>
		/// Sets the new TexturePack to be used, returning true if it has actually changed, false if nothing changed.
		/// </summary>
		public virtual bool SetTexturePack(TexturePackBase par1TexturePackBase)
		{
			if (par1TexturePackBase == SelectedTexturePack)
			{
				return false;
			}
			else
			{
				SelectedTexturePack.CloseTexturePackFile();
				CurrentTexturePack = par1TexturePackBase.TexturePackFileName;
				SelectedTexturePack = par1TexturePackBase;
				Mc.GameSettings.Skin = CurrentTexturePack;
				Mc.GameSettings.SaveOptions();
				SelectedTexturePack.Func_6482_a();
				return true;
			}
		}

		/// <summary>
		/// check the texture packs the client has installed
		/// </summary>
		public virtual void UpdateAvaliableTexturePacks()
		{
            List<TexturePackBase> arraylist = new List<TexturePackBase>();
			SelectedTexturePack = null;
			arraylist.Add(DefaultTexturePack);

			if (Directory.Exists(TexturePackDir))
			{
				string[] afile = Directory.GetFiles(TexturePackDir);
				string[] afile1 = afile;
				int i = afile1.Length;

				for (int j = 0; j < i; j++)
				{
					FileInfo file = new FileInfo(afile1[j]);

					if (file.Name.ToLower().EndsWith(".zip"))
					{
						string s = (new StringBuilder()).Append(file.Name).Append(":").Append(file.Length).Append(":").Append(file.LastWriteTime).ToString();

						try
						{
							if (!Field_6538_d.ContainsKey(s))
							{
								TexturePackCustom texturepackcustom = new TexturePackCustom(file.FullName);
								texturepackcustom.TexturePackID = s;
								Field_6538_d[s] = texturepackcustom;
								texturepackcustom.Func_6485_a(Mc);
							}

							TexturePackBase texturepackbase1 = Field_6538_d[s];

							if (texturepackbase1.TexturePackFileName.Equals(CurrentTexturePack))
							{
								SelectedTexturePack = texturepackbase1;
							}

							arraylist.Add(texturepackbase1);
						}
						catch (IOException ioexception)
						{
							Console.WriteLine(ioexception.ToString());
							Console.Write(ioexception.StackTrace);
						}

						continue;
					}

					if (!Directory.Exists(file.FullName) || !File.Exists(IOPath.Combine(file.FullName, "pack.txt")))
					{
						continue;
					}

					string s1 = (new StringBuilder()).Append(file.Name).Append(":folder:").Append(file.LastWriteTime).ToString();

					try
					{
						if (!Field_6538_d.ContainsKey(s1))
						{
							TexturePackFolder texturepackfolder = new TexturePackFolder(file.FullName);
							texturepackfolder.TexturePackID = s1;
							Field_6538_d[s1] = texturepackfolder;
							texturepackfolder.Func_6485_a(Mc);
						}

						TexturePackBase texturepackbase2 = Field_6538_d[s1];

						if (texturepackbase2.TexturePackFileName.Equals(CurrentTexturePack))
						{
							SelectedTexturePack = texturepackbase2;
						}

						arraylist.Add(texturepackbase2);
					}
					catch (IOException ioexception1)
					{
						Console.WriteLine(ioexception1.ToString());
						Console.WriteLine();
					}
				}
			}

			if (SelectedTexturePack == null)
			{
				SelectedTexturePack = DefaultTexturePack;
			}

//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the java.util.Collection 'removeAll' method:
            foreach (TexturePackBase t in arraylist)
			    availableTexturePacks.Remove(t);

			TexturePackBase texturepackbase;

			for (IEnumerator<TexturePackBase> iterator = availableTexturePacks.GetEnumerator(); iterator.MoveNext(); Field_6538_d.Remove(texturepackbase.TexturePackID))
			{
				texturepackbase = iterator.Current;
				texturepackbase.UnbindThumbnailTexture(Mc);
			}

			availableTexturePacks = arraylist;
		}

		/// <summary>
		/// Returns a list of the available texture packs.
		/// </summary>
		public virtual List<TexturePackBase> AvailableTexturePacks()
		{
			return availableTexturePacks;
		}
	}
}