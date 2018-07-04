using System;

namespace net.minecraft.src
{
	public class TexturePortalFX : TextureFX
	{
		/// <summary>
		/// Portal tick counter </summary>
		private int PortalTickCounter;
		private sbyte[][] PortalTextureData;

		public TexturePortalFX() : base(Block.Portal.BlockIndexInTexture)
		{
			PortalTickCounter = 0;
			PortalTextureData = JavaHelper.ReturnRectangularArray<sbyte>(32, 1024);
			Random random = new Random(100);

			for (int i = 0; i < 32; i++)
			{
				for (int j = 0; j < 16; j++)
				{
					for (int k = 0; k < 16; k++)
					{
						float f = 0.0F;

						for (int l = 0; l < 2; l++)
						{
							float f1 = (float)(l * 16) * 0.5F;
							float f2 = (float)(l * 16) * 0.5F;
							float f3 = (((float)j - f1) / 16F) * 2.0F;
							float f4 = (((float)k - f2) / 16F) * 2.0F;

							if (f3 < -1F)
							{
								f3 += 2.0F;
							}

							if (f3 >= 1.0F)
							{
								f3 -= 2.0F;
							}

							if (f4 < -1F)
							{
								f4 += 2.0F;
							}

							if (f4 >= 1.0F)
							{
								f4 -= 2.0F;
							}

							float f5 = f3 * f3 + f4 * f4;
							float f6 = (float)Math.Atan2(f4, f3) + ((((float)i / 32F) * (float)Math.PI * 2.0F - f5 * 10F) + (float)(l * 2)) * (float)(l * 2 - 1);
							f6 = (MathHelper2.Sin(f6) + 1.0F) / 2.0F;
							f6 /= f5 + 1.0F;
							f += f6 * 0.5F;
						}

						f += random.NextFloat() * 0.1F;
						int i1 = (int)(f * 100F + 155F);
						int j1 = (int)(f * f * 200F + 55F);
						int k1 = (int)(f * f * f * f * 255F);
						int l1 = (int)(f * 100F + 155F);
						int i2 = k * 16 + j;
						PortalTextureData[i][i2 * 4 + 0] = (sbyte)j1;
						PortalTextureData[i][i2 * 4 + 1] = (sbyte)k1;
						PortalTextureData[i][i2 * 4 + 2] = (sbyte)i1;
						PortalTextureData[i][i2 * 4 + 3] = (sbyte)l1;
					}
				}
			}
		}

		public override void OnTick()
		{
			PortalTickCounter++;
			sbyte[] abyte0 = PortalTextureData[PortalTickCounter & 0x1f];

			for (int i = 0; i < 256; i++)
			{
				int j = abyte0[i * 4 + 0] & 0xff;
				int k = abyte0[i * 4 + 1] & 0xff;
				int l = abyte0[i * 4 + 2] & 0xff;
				int i1 = abyte0[i * 4 + 3] & 0xff;

				if (AnaglyphEnabled)
				{
					int j1 = (j * 30 + k * 59 + l * 11) / 100;
					int k1 = (j * 30 + k * 70) / 100;
					int l1 = (j * 30 + l * 70) / 100;
					j = j1;
					k = k1;
					l = l1;
				}

				ImageData[i * 4 + 0] = (byte)j;
				ImageData[i * 4 + 1] = (byte)k;
				ImageData[i * 4 + 2] = (byte)l;
				ImageData[i * 4 + 3] = (byte)i1;
			}
		}
	}
}