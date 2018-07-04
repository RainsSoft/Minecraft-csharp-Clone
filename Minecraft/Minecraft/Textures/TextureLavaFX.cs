using System;

namespace net.minecraft.src
{
	public class TextureLavaFX : TextureFX
	{
		protected float[] Field_1147_g;
		protected float[] Field_1146_h;
		protected float[] Field_1145_i;
		protected float[] Field_1144_j;

		public TextureLavaFX() : base(Block.LavaMoving.BlockIndexInTexture)
		{
			Field_1147_g = new float[256];
			Field_1146_h = new float[256];
			Field_1145_i = new float[256];
			Field_1144_j = new float[256];
		}

		public override void OnTick()
		{
			for (int i = 0; i < 16; i++)
			{
				for (int j = 0; j < 16; j++)
				{
					float f = 0.0F;
					int l = (int)(MathHelper2.Sin(((float)j * (float)Math.PI * 2.0F) / 16F) * 1.2F);
					int i1 = (int)(MathHelper2.Sin(((float)i * (float)Math.PI * 2.0F) / 16F) * 1.2F);

					for (int k1 = i - 1; k1 <= i + 1; k1++)
					{
						for (int i2 = j - 1; i2 <= j + 1; i2++)
						{
							int k2 = k1 + l & 0xf;
							int i3 = i2 + i1 & 0xf;
							f += Field_1147_g[k2 + i3 * 16];
						}
					}

					Field_1146_h[i + j * 16] = f / 10F + ((Field_1145_i[(i + 0 & 0xf) + (j + 0 & 0xf) * 16] + Field_1145_i[(i + 1 & 0xf) + (j + 0 & 0xf) * 16] + Field_1145_i[(i + 1 & 0xf) + (j + 1 & 0xf) * 16] + Field_1145_i[(i + 0 & 0xf) + (j + 1 & 0xf) * 16]) / 4F) * 0.8F;
					Field_1145_i[i + j * 16] += Field_1144_j[i + j * 16] * 0.01F;

					if (Field_1145_i[i + j * 16] < 0.0F)
					{
						Field_1145_i[i + j * 16] = 0.0F;
					}

					Field_1144_j[i + j * 16] -= 0.06F;

					if ((new Random(1)).NextDouble() < 0.0050000000000000001D)
					{
						Field_1144_j[i + j * 16] = 1.5F;
					}
				}
			}

			float[] af = Field_1146_h;
			Field_1146_h = Field_1147_g;
			Field_1147_g = af;

			for (int k = 0; k < 256; k++)
			{
				float f1 = Field_1147_g[k] * 2.0F;

				if (f1 > 1.0F)
				{
					f1 = 1.0F;
				}

				if (f1 < 0.0F)
				{
					f1 = 0.0F;
				}

				float f2 = f1;
				int j1 = (int)(f2 * 100F + 155F);
				int l1 = (int)(f2 * f2 * 255F);
				int j2 = (int)(f2 * f2 * f2 * f2 * 128F);

				if (AnaglyphEnabled)
				{
					int l2 = (j1 * 30 + l1 * 59 + j2 * 11) / 100;
					int j3 = (j1 * 30 + l1 * 70) / 100;
					int k3 = (j1 * 30 + j2 * 70) / 100;
					j1 = l2;
					l1 = j3;
					j2 = k3;
				}

				ImageData[k * 4 + 0] = (byte)j1;
				ImageData[k * 4 + 1] = (byte)l1;
				ImageData[k * 4 + 2] = (byte)j2;
                unchecked
                {
                    ImageData[k * 4 + 3] = (byte)-1;
                }
			}
		}
	}
}