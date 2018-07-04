using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IOPath = System.IO.Path;

namespace net.minecraft.src
{
	public class TexturePackFolder : TexturePackBase
	{
		private int Field_48191_e;
		private Texture2D Field_48189_f;
		private string Field_48190_g;

		public TexturePackFolder(string par1File)
		{
			Field_48191_e = -1;
			TexturePackFileName = IOPath.GetFileName(par1File);
			Field_48190_g = par1File;
		}

		private string Func_48188_b(string par1Str)
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
        label0:
            {
                Stream inputstream = null;

                try
                {
                    try
                    {
                        try
                        {
                            inputstream = GetResourceAsStream("pack.txt");
                            StreamReader bufferedreader = new StreamReader(inputstream);
                            FirstDescriptionLine = Func_48188_b(bufferedreader.ReadLine());
                            SecondDescriptionLine = Func_48188_b(bufferedreader.ReadLine());
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
                            inputstream = GetResourceAsStream("pack.png");
                            //Field_48189_f = Texture2D.FromStream(inputstream);
                            inputstream.Close();
                        }
                        catch (Exception exception1)
                        {
                            Console.WriteLine(exception1.ToString());
                            Console.WriteLine();
                        }
                    }
                    catch (Exception exception2)
                    {
                        Console.WriteLine(exception2.ToString());
                        Console.Write(exception2.StackTrace);

                        goto label0;
                    }

                    goto label0;
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
                }
            }
		}

		/// <summary>
		/// Unbinds the thumbnail texture for texture pack screen
		/// </summary>
		public override void UnbindThumbnailTexture(Minecraft par1Minecraft)
		{
			if (Field_48189_f != null)
			{
				par1Minecraft.RenderEngineOld.DeleteTexture(Field_48191_e);
			}

			CloseTexturePackFile();
		}

		/// <summary>
		/// binds the texture corresponding to the pack's thumbnail image
		/// </summary>
		public override void BindThumbnailTexture(Minecraft par1Minecraft)
		{
			if (Field_48189_f != null && Field_48191_e < 0)
			{
				//Field_48191_e = par1Minecraft.RenderEngineOld.AllocateAndSetupTexture(Field_48189_f);
			}

			if (Field_48189_f != null)
			{
				par1Minecraft.RenderEngineOld.BindTexture(Field_48191_e);
			}
			else
			{
				////GL.BindTexture(TextureTarget.Texture2D, par1Minecraft.RenderEngineOld.GetTexture("Minecraft.Resources.gui.unknown_pack.png"));
			}
		}

		public override void Func_6482_a()
		{
		}

		/// <summary>
		/// Closes the zipfile associated to this texture pack. Does nothing for the default texture pack.
		/// </summary>
		public override void CloseTexturePackFile()
		{
		}

		/// <summary>
		/// Gives a texture resource as FileStream.
		/// </summary>
		public override Stream GetResourceAsStream(string par1Str)
		{
            try
            {
                string file = System.IO.Path.Combine(Field_48190_g, par1Str.Substring(1));

                if (File.Exists(file))
                {
                    return new FileStream(file, FileMode.Open);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                Console.WriteLine();
            }

			return Activator.CreateInstance<TexturePackBase>().GetResourceAsStream(par1Str);
		}
	}
}