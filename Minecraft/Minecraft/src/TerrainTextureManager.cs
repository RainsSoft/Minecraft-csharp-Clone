using System;
using System.IO;
using System.Drawing;
using System.Reflection;
namespace net.minecraft.src
{
	public class TerrainTextureManager
	{
		private float[] TexCols;
		private int[] Pixels;
		private int[] ZBuf;
		private int[] WaterBuf;
		private int[] WaterBr;
		private int[] YBuf;
		private int[] Textures;

		public TerrainTextureManager()
		{
			TexCols = new float[768];
			Pixels = new int[17408];
			ZBuf = new int[17408];
			WaterBuf = new int[17408];
			WaterBr = new int[17408];
			YBuf = new int[34];
			Textures = new int[768];

			try
			{
				Bitmap bufferedimage = (Bitmap)Bitmap.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream(("/terrain.png")));
				int[] ai = new int[0x10000];
				//bufferedimage.getRGB(0, 0, 256, 256, ai, 0, 256);

				for (int j = 0; j < 256; j++)
				{
					int k = 0;
					int l = 0;
					int i1 = 0;
					int j1 = (j % 16) * 16;
					int k1 = (j / 16) * 16;
					int l1 = 0;

					for (int i2 = 0; i2 < 16; i2++)
					{
						for (int j2 = 0; j2 < 16; j2++)
						{
							int k2 = ai[j2 + j1 + (i2 + k1) * 256];
							int l2 = k2 >> 24 & 0xff;

							if (l2 > 128)
							{
								k += k2 >> 16 & 0xff;
								l += k2 >> 8 & 0xff;
								i1 += k2 & 0xff;
								l1++;
							}
						}

						if (l1 == 0)
						{
							l1++;
						}

						TexCols[j * 3 + 0] = k / l1;
						TexCols[j * 3 + 1] = l / l1;
						TexCols[j * 3 + 2] = i1 / l1;
					}
				}
			}
			catch (IOException ioexception)
			{
				Console.WriteLine(ioexception.ToString());
				Console.Write(ioexception.StackTrace);
			}

			for (int i = 0; i < 4096; i++)
			{
				if (Block.BlocksList[i] != null)
				{
					Textures[i * 3 + 0] = Block.BlocksList[i].GetBlockTextureFromSide(1);
					Textures[i * 3 + 1] = Block.BlocksList[i].GetBlockTextureFromSide(2);
					Textures[i * 3 + 2] = Block.BlocksList[i].GetBlockTextureFromSide(3);
				}
			}
		}

		public virtual void Render(IsoImageBuffer par1IsoImageBuffer)
		{
			World world = par1IsoImageBuffer.Level;

			if (world == null)
			{
				par1IsoImageBuffer.NoContent = true;
				par1IsoImageBuffer.Rendered = true;
				return;
			}

			int i = par1IsoImageBuffer.x * 16;
			int j = par1IsoImageBuffer.y * 16;
			int k = i + 16;
			int l = j + 16;
			Chunk chunk = world.GetChunkFromChunkCoords(par1IsoImageBuffer.x, par1IsoImageBuffer.y);

			if (chunk.IsEmpty())
			{
				par1IsoImageBuffer.NoContent = true;
				par1IsoImageBuffer.Rendered = true;
				return;
			}

			par1IsoImageBuffer.NoContent = false;
			JavaHelper.FillArray(ZBuf, 0);
            JavaHelper.FillArray(WaterBuf, 0);
            JavaHelper.FillArray(YBuf, 544);

			for (int i1 = l - 1; i1 >= j; i1--)
			{
				for (int j1 = k - 1; j1 >= i; j1--)
				{
					int k1 = j1 - i;
					int l1 = i1 - j;
					int i2 = k1 + l1;
					bool flag = true;

					for (int j2 = 0; j2 < 128; j2++)
					{
						int k2 = ((l1 - k1 - j2) + 544) - 16;

						if (k2 >= YBuf[i2] && k2 >= YBuf[i2 + 1])
						{
							continue;
						}

						Block block = Block.BlocksList[world.GetBlockId(j1, j2, i1)];

						if (block == null)
						{
							flag = false;
							continue;
						}

						if (block.BlockMaterial == Material.Water)
						{
							int l2 = world.GetBlockId(j1, j2 + 1, i1);

							if (l2 != 0 && Block.BlocksList[l2].BlockMaterial == Material.Water)
							{
								continue;
							}

							float f1 = ((float)j2 / 127F) * 0.6F + 0.4F;
							float f2 = world.GetLightBrightness(j1, j2 + 1, i1) * f1;

							if (k2 < 0 || k2 >= 544)
							{
								continue;
							}

							int i4 = i2 + k2 * 32;

							if (i2 >= 0 && i2 <= 32 && WaterBuf[i4] <= j2)
							{
								WaterBuf[i4] = j2;
								WaterBr[i4] = (int)(f2 * 127F);
							}

							if (i2 >= -1 && i2 <= 31 && WaterBuf[i4 + 1] <= j2)
							{
								WaterBuf[i4 + 1] = j2;
								WaterBr[i4 + 1] = (int)(f2 * 127F);
							}

							flag = false;
							continue;
						}

						if (flag)
						{
							if (k2 < YBuf[i2])
							{
								YBuf[i2] = k2;
							}

							if (k2 < YBuf[i2 + 1])
							{
								YBuf[i2 + 1] = k2;
							}
						}

						float f = ((float)j2 / 127F) * 0.6F + 0.4F;

						if (k2 >= 0 && k2 < 544)
						{
							int i3 = i2 + k2 * 32;
							int k3 = Textures[block.BlockID * 3 + 0];
							float f3 = (world.GetLightBrightness(j1, j2 + 1, i1) * 0.8F + 0.2F) * f;
							int j4 = k3;

							if (i2 >= 0)
							{
								float f5 = f3;

								if (ZBuf[i3] <= j2)
								{
									ZBuf[i3] = j2;
									Pixels[i3] = 0xff00000 | (int)(TexCols[j4 * 3 + 0] * f5) << 16 | (int)(TexCols[j4 * 3 + 1] * f5) << 8 | (int)(TexCols[j4 * 3 + 2] * f5);
								}
							}

							if (i2 < 31)
							{
								float f6 = f3 * 0.9F;

								if (ZBuf[i3 + 1] <= j2)
								{
									ZBuf[i3 + 1] = j2;
									Pixels[i3 + 1] = 0xff00000 | (int)(TexCols[j4 * 3 + 0] * f6) << 16 | (int)(TexCols[j4 * 3 + 1] * f6) << 8 | (int)(TexCols[j4 * 3 + 2] * f6);
								}
							}
						}

						if (k2 < -1 || k2 >= 543)
						{
							continue;
						}

						int j3 = i2 + (k2 + 1) * 32;
						int l3 = Textures[block.BlockID * 3 + 1];
						float f4 = world.GetLightBrightness(j1 - 1, j2, i1) * 0.8F + 0.2F;
						int k4 = Textures[block.BlockID * 3 + 2];
						float f7 = world.GetLightBrightness(j1, j2, i1 + 1) * 0.8F + 0.2F;

						if (i2 >= 0)
						{
							float f8 = f4 * f * 0.6F;

							if (ZBuf[j3] <= j2 - 1)
							{
								ZBuf[j3] = j2 - 1;
								Pixels[j3] = 0xff00000 | (int)(TexCols[l3 * 3 + 0] * f8) << 16 | (int)(TexCols[l3 * 3 + 1] * f8) << 8 | (int)(TexCols[l3 * 3 + 2] * f8);
							}
						}

						if (i2 >= 31)
						{
							continue;
						}

						float f9 = f7 * 0.9F * f * 0.4F;

						if (ZBuf[j3 + 1] <= j2 - 1)
						{
							ZBuf[j3 + 1] = j2 - 1;
							Pixels[j3 + 1] = 0xff00000 | (int)(TexCols[k4 * 3 + 0] * f9) << 16 | (int)(TexCols[k4 * 3 + 1] * f9) << 8 | (int)(TexCols[k4 * 3 + 2] * f9);
						}
					}
				}
			}

			PostProcess();

			if (par1IsoImageBuffer.Image == null)
			{
				par1IsoImageBuffer.Image = new Bitmap(32, 544);
			}

			//par1IsoImageBuffer.Image.setRGB(0, 0, 32, 544, Pixels, 0, 32);
			par1IsoImageBuffer.Rendered = true;
		}

		private void PostProcess()
		{
			for (int i = 0; i < 32; i++)
			{
				for (int j = 0; j < 544; j++)
				{
					int k = i + j * 32;

					if (ZBuf[k] == 0)
					{
						Pixels[k] = 0;
					}

					if (WaterBuf[k] <= ZBuf[k])
					{
						continue;
					}

					int l = Pixels[k] >> 24 & 0xff;
					Pixels[k] = ((Pixels[k] & 0xfefefe) >> 1) + WaterBr[k];

					if (l < 128)
					{
						Pixels[k] = 0x8000000 + WaterBr[k] * 2;
					}
					else
					{
						Pixels[k] |= 0xff00000;
					}
				}
			}
		}
	}
}