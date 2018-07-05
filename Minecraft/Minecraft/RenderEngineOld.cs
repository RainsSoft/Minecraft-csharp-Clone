using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.Xna.Framework;

namespace net.minecraft.src
{
	public class RenderEngineOld
	{
		/// <summary>
		/// Use mipmaps for all bound textures (unused at present) </summary>
		public static bool UseMipmaps = false;
		private Dictionary<string, int> TextureMap;

		/// <summary>
		/// Texture contents map (key: texture name, value: int[] contents) </summary>
		private Dictionary<string, int[]> TextureContentsMap;

		/// <summary>
		/// A mapping from GL texture names (integers) to Bitmap instances </summary>
		private IntHashMap TextureNameToImageMap;

		/// <summary>
		/// An IntBuffer storing 1 int used as scratch space in RenderEngine </summary>
		private Buffer<int> SingleIntBuffer;

		/// <summary>
		/// Stores the image data for the texture. </summary>
		private Buffer<byte> ImageData;
		private List<TextureFX> TextureList;

		/// <summary>
		/// A mapping from image URLs to ThreadDownloadImageData instances </summary>
		private Dictionary<string, ThreadedImageDownloader> UrlToImageDataMap;

		/// <summary>
		/// Reference to the GameSettings object </summary>
		private GameSettings Options;

		/// <summary>
		/// Flag set when a texture should not be repeated </summary>
		public bool ClampTexture;

		/// <summary>
		/// Flag set when a texture should use blurry resizing </summary>
		public bool BlurTexture;

		/// <summary>
		/// Texture pack </summary>
		private TexturePackList TexturePack;

		/// <summary>
		/// Missing texture image </summary>
        private Bitmap MissingTextureImage;
		private int Field_48512_n;

		public RenderEngineOld(TexturePackList par1TexturePackList, GameSettings par2GameSettings)
		{
			TextureMap = new Dictionary<string, int>();
            TextureContentsMap = new Dictionary<string, int[]>();
			TextureNameToImageMap = new IntHashMap();
            SingleIntBuffer = new Buffer<int>(1);// GLAllocation.CreateDirectIntBuffer(1);
			ImageData = new Buffer<byte>(0x1000000);//GLAllocation.CreateDirectByteBuffer(0x1000000);
            TextureList = new List<TextureFX>();
            UrlToImageDataMap = new Dictionary<string, ThreadedImageDownloader>();
			ClampTexture = false;
			BlurTexture = false;
			MissingTextureImage = new Bitmap(64, 64);
			Field_48512_n = 16;
			TexturePack = par1TexturePackList;
			Options = par2GameSettings;
			Graphics g = Graphics.FromImage(MissingTextureImage);
			g.FillRectangle(Brushes.White, 0, 0, 64, 64);
			g.DrawString("missingtex", SystemFonts.DefaultFont, Brushes.Black, 1, 10);
			g.Dispose();
		}

		public virtual int[] GetTextureContents(string par1Str)
		{
			TexturePackBase texturepackbase = TexturePack.SelectedTexturePack;

            if (TextureContentsMap.ContainsKey(par1Str))
                return TextureContentsMap[par1Str];

			try
			{
				int[] ai1 = null;

				if (par1Str.StartsWith("##"))
				{
					ai1 = GetImageContentsAndAllocate(UnwrapImageByColumns(ReadTextureImage(texturepackbase.GetResourceAsStream(par1Str.Substring(2)))));
				}
				else if (par1Str.StartsWith("%clamp%"))
				{
					ClampTexture = true;
					ai1 = GetImageContentsAndAllocate(ReadTextureImage(texturepackbase.GetResourceAsStream(par1Str.Substring(7))));
					ClampTexture = false;
				}
				else if (par1Str.StartsWith("%blur%"))
				{
					BlurTexture = true;
					ClampTexture = true;
					ai1 = GetImageContentsAndAllocate(ReadTextureImage(texturepackbase.GetResourceAsStream(par1Str.Substring(6))));
					ClampTexture = false;
					BlurTexture = false;
				}
				else
				{
					Stream inputstream = texturepackbase.GetResourceAsStream(par1Str);

					if (inputstream == null)
					{
						ai1 = GetImageContentsAndAllocate(MissingTextureImage);
					}
					else
					{
						ai1 = GetImageContentsAndAllocate(ReadTextureImage(inputstream));
					}
				}

				TextureContentsMap.Add(par1Str, ai1);
				return ai1;
			}
			catch (IOException ioexception)
			{
                Utilities.LogException(ioexception);
			}

			int[] ai2 = GetImageContentsAndAllocate(MissingTextureImage);
			TextureContentsMap.Add(par1Str, ai2);
			return ai2;
		}

        private int[] GetImageContentsAndAllocate(Bitmap par1Bitmap)
		{
			int i = par1Bitmap.Width;
			int j = par1Bitmap.Height;
			int[] ai = new int[i * j];
			//var data = par1Bitmap.LockBits(new Rectangle(0, 0, i, j), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			return ai;
		}

        private int[] GetImageContents(Bitmap par1Bitmap, int[] par2ArrayOfInteger)
		{
			int i = par1Bitmap.Width;
			int j = par1Bitmap.Height;
			//par1Bitmap.getRGB(0, 0, i, j, par2ArrayOfInteger, 0, i);
			return par2ArrayOfInteger;
		}

		public virtual int GetTexture(string par1Str)
		{
			TexturePackBase texturepackbase = TexturePack.SelectedTexturePack;

            if (TextureMap.ContainsKey(par1Str))
                return TextureMap[par1Str];

			try
			{
				SingleIntBuffer.Clear();
				GLAllocation.GenerateTextureNames(SingleIntBuffer);
				int i = SingleIntBuffer.Get(0);

				if (par1Str.StartsWith("##"))
				{
					SetupTexture(UnwrapImageByColumns(ReadTextureImage(texturepackbase.GetResourceAsStream(par1Str.Substring(2)))), i);
				}
				else if (par1Str.StartsWith("%clamp%"))
				{
					ClampTexture = true;
					SetupTexture(ReadTextureImage(texturepackbase.GetResourceAsStream(par1Str.Substring(7))), i);
					ClampTexture = false;
				}
				else if (par1Str.StartsWith("%blur%"))
				{
					BlurTexture = true;
					SetupTexture(ReadTextureImage(texturepackbase.GetResourceAsStream(par1Str.Substring(6))), i);
					BlurTexture = false;
				}
				else if (par1Str.StartsWith("%blurclamp%"))
				{
					BlurTexture = true;
					ClampTexture = true;
					SetupTexture(ReadTextureImage(texturepackbase.GetResourceAsStream(par1Str.Substring(11))), i);
					BlurTexture = false;
					ClampTexture = false;
				}
				else
				{
					Stream inputstream = texturepackbase.GetResourceAsStream(par1Str);

					if (inputstream == null)
					{
						SetupTexture(MissingTextureImage, i);
					}
					else
					{
						SetupTexture(ReadTextureImage(inputstream), i);
					}
				}

				TextureMap.Add(par1Str, i);
				return i;
			}
			catch (Exception exception)
			{
                Utilities.LogException(exception);
			}

			GLAllocation.GenerateTextureNames(SingleIntBuffer);
			int j = SingleIntBuffer.Get(0);
			SetupTexture(MissingTextureImage, j);
			TextureMap.Add(par1Str, j);
			return j;
		}

		/// <summary>
		/// Takes an image with multiple 16-pixel-wide columns and creates a new 16-pixel-wide image where the columns are
		/// stacked vertically
		/// </summary>
        private Bitmap UnwrapImageByColumns(Bitmap par1Bitmap)
		{
			int i = par1Bitmap.Width / 16;
            Bitmap bufferedimage = new Bitmap(16, par1Bitmap.Height * i);
			Graphics g = Graphics.FromImage(bufferedimage);

			for (int j = 0; j < i; j++)
			{
				g.DrawImage(par1Bitmap, -j * 16, j * par1Bitmap.Height);
			}

			g.Dispose();
			return bufferedimage;
		}

		/// <summary>
		/// Copy the supplied image onto a newly-allocated OpenGL texture, returning the allocated texture name
		/// </summary>
        public virtual int AllocateAndSetupTexture(Bitmap par1Bitmap)
		{
			SingleIntBuffer.Clear();
			GLAllocation.GenerateTextureNames(SingleIntBuffer);
			int i = SingleIntBuffer.Get(0);
			SetupTexture(par1Bitmap, i);
			TextureNameToImageMap.AddKey(i, par1Bitmap);
			return i;
		}

		/// <summary>
		/// Copy the supplied image onto the specified OpenGL texture
		/// </summary>
        public virtual void SetupTexture(Bitmap par1Bitmap, int par2)
		{/*
			//GL.BindTexture(TextureTarget.Texture2D, par2);

			if (UseMipmaps)
			{
				//GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.NearestMipmapLinear);
                //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
			}
			else
			{
				//GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
				//GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
			}

			if (BlurTexture)
			{
                //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
			}

			if (ClampTexture)
			{
				//GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Clamp);
				//GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Clamp);
			}
			else
			{
				//GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
				//GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
			}
            */
			int i = par1Bitmap.Width;
			int j = par1Bitmap.Height;
			int[] ai = new int[i * j];
			byte[] abyte0 = new byte[i * j * 4];
			//par1Bitmap.getRGB(0, 0, i, j, ai, 0, i);

			for (int k = 0; k < ai.Length; k++)
			{
				int i1 = ai[k] >> 24 & 0xff;
				int k1 = ai[k] >> 16 & 0xff;
				int i2 = ai[k] >> 8 & 0xff;
				int k2 = ai[k] & 0xff;

				if (Options != null && Options.Anaglyph)
				{
					int i3 = (k1 * 30 + i2 * 59 + k2 * 11) / 100;
					int k3 = (k1 * 30 + i2 * 70) / 100;
					int i4 = (k1 * 30 + k2 * 70) / 100;
					k1 = i3;
					i2 = k3;
					k2 = i4;
				}

				abyte0[k * 4 + 0] = (byte)k1;
				abyte0[k * 4 + 1] = (byte)i2;
				abyte0[k * 4 + 2] = (byte)k2;
				abyte0[k * 4 + 3] = (byte)i1;
			}

			//ImageData.Clear();
			//ImageData.Put(abyte0);
			//ImageData.Position(0).limit(abyte0.Length);
			////GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, i, j, 0, PixelFormat.Rgba, PixelType.UnsignedByte, ImageData);

			if (UseMipmaps)
			{
				for (int l = 1; l <= 4; l++)
				{
					int j1 = i >> l - 1;
					int l1 = i >> l;
					int j2 = j >> l;

					for (int l2 = 0; l2 < l1; l2++)
					{
						for (int j3 = 0; j3 < j2; j3++)
						{/*
							int l3 = ImageData.GetInt((l2 * 2 + 0 + (j3 * 2 + 0) * j1) * 4);
							int j4 = ImageData.GetInt((l2 * 2 + 1 + (j3 * 2 + 0) * j1) * 4);
							int k4 = ImageData.GetInt((l2 * 2 + 1 + (j3 * 2 + 1) * j1) * 4);
							int l4 = ImageData.GetInt((l2 * 2 + 0 + (j3 * 2 + 1) * j1) * 4);
							int i5 = AlphaBlend(AlphaBlend(l3, j4), AlphaBlend(k4, l4));
							ImageData.PutInt((l2 + j3 * l1) * 4, i5);*/
						}
					}

                    ////GL.TexImage2D(TextureTarget.Texture2D, l, PixelInternalFormat.Rgba, l1, j2, 0, PixelFormat.Rgba, PixelType.UnsignedByte, ImageData);
				}
			}
		}

		public virtual void CreateTextureFromBytes(int[] par1ArrayOfInteger, int par2, int par3, int par4)
		{/*
			//GL.BindTexture(TextureTarget.Texture2D, par4);

            if (UseMipmaps)
            {
                //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.NearestMipmapLinear);
                //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            }
            else
            {
                //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
                //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            }

            if (BlurTexture)
            {
                //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            }

            if (ClampTexture)
            {
                //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Clamp);
                //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Clamp);
            }
            else
            {
                //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
                //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            }
            */
			byte[] abyte0 = new byte[par2 * par3 * 4];

			for (int i = 0; i < par1ArrayOfInteger.Length; i++)
			{
				int j = par1ArrayOfInteger[i] >> 24 & 0xff;
				int k = par1ArrayOfInteger[i] >> 16 & 0xff;
				int l = par1ArrayOfInteger[i] >> 8 & 0xff;
				int i1 = par1ArrayOfInteger[i] & 0xff;

				if (Options != null && Options.Anaglyph)
				{
					int j1 = (k * 30 + l * 59 + i1 * 11) / 100;
					int k1 = (k * 30 + l * 70) / 100;
					int l1 = (k * 30 + i1 * 70) / 100;
					k = j1;
					l = k1;
					i1 = l1;
				}

				abyte0[i * 4 + 0] = (byte)k;
				abyte0[i * 4 + 1] = (byte)l;
				abyte0[i * 4 + 2] = (byte)i1;
				abyte0[i * 4 + 3] = (byte)j;
			}
            
			ImageData.Clear();
			ImageData.Put(abyte0);
			ImageData.Position = 0;
            ImageData.Limit(abyte0.Length);
			////GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, par2, par3, PixelFormat.Rgba, PixelType.UnsignedByte, ImageData);
		}

		/// <summary>
		/// Deletes a single GL texture
		/// </summary>
		public virtual void DeleteTexture(int par1)
		{
			TextureNameToImageMap.RemoveObject(par1);
			SingleIntBuffer.Clear();
			SingleIntBuffer.Put(par1);
			SingleIntBuffer.Flip();
			//GL.DeleteTextures(SingleIntBuffer);
		}

		/// <summary>
		/// Takes a URL of a downloadable image and the name of the local image to be used as a fallback.  If the image has
		/// been downloaded, returns the GL texture of the downloaded image, otherwise returns the GL texture of the fallback
		/// image.
		/// </summary>
		public virtual int GetTextureForDownloadableImage(string par1Str, string par2Str)
		{
            ThreadedImageDownloader threaddownloadimagedata = UrlToImageDataMap[par1Str];
            /*
			if (threaddownloadimagedata != null && threaddownloadimagedata.Image != null && !threaddownloadimagedata.TextureSetupComplete)
			{
				if (threaddownloadimagedata.TextureName < 0)
				{
					threaddownloadimagedata.TextureName = AllocateAndSetupTexture(threaddownloadimagedata.Image);
				}
				else
				{
					SetupTexture(threaddownloadimagedata.Image, threaddownloadimagedata.TextureName);
				}

				threaddownloadimagedata.TextureSetupComplete = true;
			}

			if (threaddownloadimagedata == null || threaddownloadimagedata.TextureName < 0)
			{
				if (par2Str == null)
				{
					return -1;
				}
				else
				{
					return GetTexture(par2Str);
				}
			}
			else
			{
				return threaddownloadimagedata.TextureName;
			}*/

            return -1;
		}

		/// <summary>
		/// Return a ThreadDownloadImageData instance for the given URL.  If it does not already exist, it is created and
		/// uses the passed ImageBuffer.  If it does, its reference count is incremented.
		/// </summary>
        public ThreadedImageDownloader ObtainImageData(string par1Str, ImageBuffer par2ImageBuffer)
		{
            ThreadedImageDownloader threaddownloadimagedata = null;

            if (UrlToImageDataMap.ContainsKey(par1Str))
                threaddownloadimagedata = UrlToImageDataMap[par1Str];

			if (threaddownloadimagedata == null)
			{
                UrlToImageDataMap.Add(par1Str, new ThreadedImageDownloader(par1Str, par2ImageBuffer));
			}
			else
			{
				threaddownloadimagedata.ReferenceCount++;
			}

			return threaddownloadimagedata;
		}

		/// <summary>
		/// Decrements the reference count for a given URL, deleting the image data if the reference count hits 0
		/// </summary>
		public void ReleaseImageData(string par1Str)
		{
            //ThreadedImageDownloader threaddownloadimagedata = null;
            /*
            if (UrlToImageDataMap.ContainsKey(par1Str))
                threaddownloadimagedata = UrlToImageDataMap[par1Str];

			if (threaddownloadimagedata != null)
			{
				threaddownloadimagedata.ReferenceCount--;

				if (threaddownloadimagedata.ReferenceCount == 0)
				{
					if (threaddownloadimagedata.TextureName >= 0)
					{
						DeleteTexture(threaddownloadimagedata.TextureName);
					}

					UrlToImageDataMap.Remove(par1Str);
				}
			}*/
		}

		public void RegisterTextureFX(TextureFX par1TextureFX)
		{
			TextureList.Add(par1TextureFX);
			par1TextureFX.OnTick();
		}

		public void UpdateDynamicTextures()
		{
			int i = -1;

			for (int j = 0; j < TextureList.Count; j++)
			{
				TextureFX texturefx = TextureList[j];
				texturefx.AnaglyphEnabled = Options.Anaglyph;
				texturefx.OnTick();
				ImageData.Clear();
				ImageData.Put(texturefx.ImageData);
				ImageData.Position = 0;
                ImageData.Limit(texturefx.ImageData.Length);

				if (texturefx.IconIndex != i)
				{
					texturefx.BindImage(this);
					i = texturefx.IconIndex;
				}

				for (int k = 0; k < texturefx.TileSize; k++)
				{
					for (int l = 0; l < texturefx.TileSize; l++)
					{
						////GL.TexSubImage2D(TextureTarget.Texture2D, 0, (texturefx.IconIndex % 16) * 16 + k * 16, (texturefx.IconIndex / 16) * 16 + l * 16, 16, 16, PixelFormat.Rgba, PixelType.UnsignedByte, ref ImageData);
					}
				}
			}
		}

		/// <summary>
		/// Uses the alpha of the two colors passed in to determine the contributions of each color.  If either of them has
		/// an alpha greater than 0 then the returned alpha is 255 otherwise its zero if they are both zero. Args: color1,
		/// color2
		/// </summary>
		private int AlphaBlend(int par1, int par2)
		{
			int i = (par1 & 0xff00000) >> 24 & 0xff;
			int j = (par2 & 0xff00000) >> 24 & 0xff;
			char c = '\u0377'; if (i + j < 255)
            {
                c = '\0';
                i = 1;
                j = 1;
            }
            else if (i > j)
            {
                i = 255;
                j = 1;
            }
            else
            {
                i = 1;
                j = 255;
            }
            int k = (par1 >> 16 & 0xff) * i;
            int l = (par1 >> 8 & 0xff) * i;
            int i1 = (par1 & 0xff) * i;
            int j1 = (par2 >> 16 & 0xff) * j;
            int k1 = (par2 >> 8 & 0xff) * j;
            int l1 = (par2 & 0xff) * j;
            int i2 = (k + j1) / (i + j);
            int j2 = (l + k1) / (i + j);
            int k2 = (i1 + l1) / (i + j);
            return c << 24 | i2 << 16 | j2 << 8 | k2;
        }
        
		/// <summary>
		/// Call setupTexture on all currently-loaded textures again to account for changes in rendering options
		/// </summary>
        public void RefreshTextures()
        {
            TexturePackBase texturepackbase = TexturePack.SelectedTexturePack;
            int i;
            Bitmap bufferedimage;
            for (IEnumerator<int> iterator = TextureNameToImageMap.GetKeySet().GetEnumerator(); iterator.MoveNext(); SetupTexture(bufferedimage, i))
            {
                i = iterator.Current;
                bufferedimage = (Bitmap)TextureNameToImageMap.Lookup(i);
            }
            for (IEnumerator<ThreadedImageDownloader> iterator1 = UrlToImageDataMap.Values.GetEnumerator(); iterator1.MoveNext(); )
            {
                ThreadedImageDownloader threaddownloadimagedata = iterator1.Current;
                threaddownloadimagedata.TextureSetupComplete = false;
            }
            for (IEnumerator<string> iterator2 = TextureMap.Keys.GetEnumerator(); iterator2.MoveNext(); )
            {
                string s = (string)iterator2.Current;
                try
                {
                    Bitmap bufferedimage1;
                    if (s.StartsWith("##"))
                    {
                        bufferedimage1 = UnwrapImageByColumns(ReadTextureImage(texturepackbase.GetResourceAsStream(s.Substring(2))));
                    }
                    else if (s.StartsWith("%clamp%"))
                    {
                        ClampTexture = true;
                        bufferedimage1 = ReadTextureImage(texturepackbase.GetResourceAsStream(s.Substring(7)));
                    }
                    else if (s.StartsWith("%blur%"))
                    {
                        BlurTexture = true;
                        bufferedimage1 = ReadTextureImage(texturepackbase.GetResourceAsStream(s.Substring(6)));
                    }
                    else if (s.StartsWith("%blurclamp%"))
                    {
                        BlurTexture = true;
                        ClampTexture = true;
                        bufferedimage1 = ReadTextureImage(texturepackbase.GetResourceAsStream(s.Substring(11)));
                    }
                    else
                    {
                        bufferedimage1 = ReadTextureImage(texturepackbase.GetResourceAsStream(s));
                    }
                    int j = TextureMap[s];
                    SetupTexture(bufferedimage1, j);
                    BlurTexture = false;
                    ClampTexture = false;
                }
                catch (IOException ioexception)
                {
                    Utilities.LogException(ioexception);
                }
            }
            for (IEnumerator<string> iterator3 = TextureContentsMap.Keys.GetEnumerator(); iterator3.MoveNext();)
            {
                string s1 = (string)iterator3.Current;
                try
                {
                    Bitmap bufferedimage2;
                    if (s1.StartsWith("##"))
                    {
                        bufferedimage2 = UnwrapImageByColumns(ReadTextureImage(texturepackbase.GetResourceAsStream(s1.Substring(2))));
                    }
                    else if (s1.StartsWith("%clamp%"))
                    {
                        ClampTexture = true;
                        bufferedimage2 = ReadTextureImage(texturepackbase.GetResourceAsStream(s1.Substring(7)));
                    }
                    else if (s1.StartsWith("%blur%"))
                    {
                        BlurTexture = true;
                        bufferedimage2 = ReadTextureImage(texturepackbase.GetResourceAsStream(s1.Substring(6)));
                    }
                    else
                    {
                        bufferedimage2 = ReadTextureImage(texturepackbase.GetResourceAsStream(s1));
                    }
                    GetImageContents(bufferedimage2, (int[])TextureContentsMap[s1]);
                    BlurTexture = false;
                    ClampTexture = false;
                }
                catch (IOException ioexception1)
                {
                    Utilities.LogException(ioexception1);
                }
            }
        }
        
		/// <summary>
		/// Returns a Bitmap read off the provided input stream.  Args: inputStream
		/// </summary>
        private Bitmap ReadTextureImage(Stream par1FileStream)
        {
            try
            {
                Bitmap bufferedimage = (Bitmap)Bitmap.FromStream(par1FileStream);
                par1FileStream.Close();
                return bufferedimage;
            }
            catch (IOException e)
            {
                throw e;
            }
        }
        
        public void BindTexture(int par1)
        {
            if (par1 < 0)
            {
                return;
            }
            else
            {
                ////GL.BindTexture(TextureTarget.Texture2D, par1);
                return;
            }
        }
    }
}