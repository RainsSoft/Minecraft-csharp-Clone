using System.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace net.minecraft.src
{
	public class ImageBufferDownload : ImageBuffer
	{
		private int[] ImageData;
		private int ImageWidth;
		private int ImageHeight;

		public ImageBufferDownload()
		{
		}

		public Texture2D ParseUserSkin(Texture2D par1Bitmap)
		{
			if (par1Bitmap == null)
			{
				return null;
			}

			ImageWidth = 64;
			ImageHeight = 32;

            Texture2D bufferedimage = new Texture2D(RenderEngine.Instance.GraphicsDevice, ImageWidth, ImageHeight);
			bufferedimage.GetData<int>(ImageData);

			Func_884_b(0, 0, 32, 16);
			Func_885_a(32, 0, 64, 32);
			Func_884_b(0, 16, 64, 32);
			bool flag = false;

			for (int i = 32; i < 64; i++)
			{
				for (int k = 0; k < 16; k++)
				{
					int i1 = ImageData[i + k * 64];

					if ((i1 >> 24 & 0xff) < 128)
					{
						flag = true;
					}
				}
			}

			if (!flag)
			{
				for (int j = 32; j < 64; j++)
				{
					for (int l = 0; l < 16; l++)
					{
						int j1 = ImageData[j + l * 64];
						bool flag1;

						if ((j1 >> 24 & 0xff) < 128)
						{
							flag1 = true;
						}
					}
				}
			}

			return bufferedimage;
		}

		private void Func_885_a(int par1, int par2, int par3, int par4)
		{
			if (Func_886_c(par1, par2, par3, par4))
			{
				return;
			}

			for (int i = par1; i < par3; i++)
			{
				for (int j = par2; j < par4; j++)
				{
					ImageData[i + j * ImageWidth] &= 0xffffff;
				}
			}
		}

		private void Func_884_b(int par1, int par2, int par3, int par4)
		{
			for (int i = par1; i < par3; i++)
			{
				for (int j = par2; j < par4; j++)
				{
					ImageData[i + j * ImageWidth] |= 0xff00000;
				}
			}
		}

		private bool Func_886_c(int par1, int par2, int par3, int par4)
		{
			for (int i = par1; i < par3; i++)
			{
				for (int j = par2; j < par4; j++)
				{
					int k = ImageData[i + j * ImageWidth];

					if ((k >> 24 & 0xff) < 128)
					{
						return true;
					}
				}
			}

			return false;
		}
	}
}