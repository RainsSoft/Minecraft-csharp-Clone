using System;
using System.Drawing;
using System.IO;
using System.IO.Compression;

namespace net.minecraft.src
{
    using Microsoft.Xna.Framework;
	using net.minecraft.src;

	public class TexturePackCustom : TexturePackBase
	{
		//private ZipFile TexturePackZipFile;

		/// <summary>
		/// The allocated OpenGL texture name for this texture pack, or -1 if it hasn't been allocated yet.
		/// </summary>
		private int TexturePackName;
		private Bitmap TexturePackThumbnail;
		private string TexturePackFile;

		public TexturePackCustom(string par1File)
		{
			TexturePackName = -1;
			TexturePackFileName = System.IO.Path.GetFileName(par1File);
			TexturePackFile = par1File;
		}

		/// <summary>
		/// Truncates the specified string to 34 characters in length and returns it.
		/// </summary>
		private string TruncateString(string par1Str)
		{
			if (par1Str != null && par1Str.Length > 34)
			{
				par1Str = par1Str.Substring(0, 34);
			}

			return par1Str;
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void Func_6485_a(net.minecraft.client.Minecraft par1Minecraft) throws IOException
		public override void Func_6485_a(Minecraft par1Minecraft)
		{
			//ZipFile zipfile = null;
			FileStream inputstream = null;

			try
			{
				//zipfile = new ZipFile(TexturePackFile);

				try
				{
					//inputstream = zipfile.GetFileStream(zipfile.GetEntry("pack.txt"));
					StreamReader bufferedreader = new StreamReader(inputstream);
					FirstDescriptionLine = TruncateString(bufferedreader.ReadLine());
					SecondDescriptionLine = TruncateString(bufferedreader.ReadLine());
					bufferedreader.Close();
					inputstream.Close();
				}
				catch (Exception exception)
                {
                    Console.WriteLine(exception.ToString());
                    Console.WriteLine();
				}

				try
				{
					//inputstream = zipfile.GetFileStream(zipfile.GetEntry("pack.png"));
					//TexturePackThumbnail = ImageIO.Read(inputstream);
					inputstream.Close();
				}
				catch (Exception exception1)
                {
                    Console.WriteLine(exception1.ToString());
                    Console.WriteLine();
				}

				//zipfile.Close();
			}
			catch (Exception exception2)
			{
				Console.WriteLine(exception2.ToString());
				Console.WriteLine();
			}
			finally
			{
				try
				{
					inputstream.Close();
				}
				catch (Exception exception4)
                {
                    Console.WriteLine(exception4.ToString());
                    Console.WriteLine();
				}

				try
				{
					//zipfile.Close();
				}
				catch (Exception exception5)
                {
                    Console.WriteLine(exception5.ToString());
                    Console.WriteLine();
				}
			}
		}

		/// <summary>
		/// Unbinds the thumbnail texture for texture pack screen
		/// </summary>
		public override void UnbindThumbnailTexture(Minecraft par1Minecraft)
		{
			if (TexturePackThumbnail != null)
			{
				par1Minecraft.RenderEngineOld.DeleteTexture(TexturePackName);
			}

			CloseTexturePackFile();
		}

		/// <summary>
		/// binds the texture corresponding to the pack's thumbnail image
		/// </summary>
		public override void BindThumbnailTexture(Minecraft par1Minecraft)
		{
			if (TexturePackThumbnail != null && TexturePackName < 0)
			{
				TexturePackName = par1Minecraft.RenderEngineOld.AllocateAndSetupTexture(TexturePackThumbnail);
			}

			if (TexturePackThumbnail != null)
			{
				par1Minecraft.RenderEngineOld.BindTexture(TexturePackName);
			}
			else
			{
				//GL.BindTexture(TextureTarget.Texture2D, par1Minecraft.RenderEngineOld.GetTexture("/gui/unknown_pack.png"));
			}
		}

		public override void Func_6482_a()
		{
			try
			{
				//TexturePackZipFile = new ZipFile(TexturePackFile);
			}
			catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                Console.WriteLine();
			}
		}

		/// <summary>
		/// Closes the zipfile associated to this texture pack. Does nothing for the default texture pack.
		/// </summary>
		public override void CloseTexturePackFile()
		{
			try
			{
				//TexturePackZipFile.Close();
			}
			catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                Console.WriteLine();
			}

			//TexturePackZipFile = null;
		}

		/// <summary>
		/// Gives a texture resource as FileStream.
		/// </summary>
		public override Stream GetResourceAsStream(string par1Str)
		{
			try
			{/*
				ZipEntry zipentry = TexturePackZipFile.getEntry(par1Str.Substring(1));

				if (zipentry != null)
				{
					return TexturePackZipFile.getFileStream(zipentry);
				}*/
			}
			catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                Console.WriteLine();
			}

            return null;// ((TexturePackBase)Activator.CreateInstance(typeof(TexturePackBase))).GetResourceAsStream(par1Str);
		}
	}
}