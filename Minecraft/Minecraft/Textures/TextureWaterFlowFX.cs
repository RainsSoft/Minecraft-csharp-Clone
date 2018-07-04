using System;

namespace net.minecraft.src
{
	public class TextureWaterFlowFX : TextureFX
	{
		protected float[] Field_1138_g;
		protected float[] Field_1137_h;
		protected float[] Field_1136_i;
		protected float[] Field_1135_j;
		private int TickCounter;

		public TextureWaterFlowFX() : base(Block.WaterMoving.BlockIndexInTexture + 1)
		{
			Field_1138_g = new float[256];
			Field_1137_h = new float[256];
			Field_1136_i = new float[256];
			Field_1135_j = new float[256];
			TickCounter = 0;
			TileSize = 2;
		}

		public override void OnTick()
		{
			TickCounter++;

			for (int i = 0; i < 16; i++)
			{
				for (int k = 0; k < 16; k++)
				{
					float f = 0.0F;

					for (int j1 = k - 2; j1 <= k; j1++)
					{
						int k1 = i & 0xf;
						int i2 = j1 & 0xf;
						f += Field_1138_g[k1 + i2 * 16];
					}

					Field_1137_h[i + k * 16] = f / 3.2F + Field_1136_i[i + k * 16] * 0.8F;
				}
			}

			for (int j = 0; j < 16; j++)
			{
				for (int l = 0; l < 16; l++)
				{
					Field_1136_i[j + l * 16] += Field_1135_j[j + l * 16] * 0.05F;

					if (Field_1136_i[j + l * 16] < 0.0F)
					{
						Field_1136_i[j + l * 16] = 0.0F;
					}

					Field_1135_j[j + l * 16] -= 0.3F;

					if ((new Random(1)).NextDouble() < 0.20000000000000001D)
					{
						Field_1135_j[j + l * 16] = 0.5F;
					}
				}
			}

			float[] af = Field_1137_h;
			Field_1137_h = Field_1138_g;
			Field_1138_g = af;

			for (int i1 = 0; i1 < 256; i1++)
			{
				float f1 = Field_1138_g[i1 - TickCounter * 16 & 0xff];

				if (f1 > 1.0F)
				{
					f1 = 1.0F;
				}

				if (f1 < 0.0F)
				{
					f1 = 0.0F;
				}

				float f2 = f1 * f1;
				int l1 = (int)(32F + f2 * 32F);
				int j2 = (int)(50F + f2 * 64F);
				int k2 = 255;
				int l2 = (int)(146F + f2 * 50F);

				if (AnaglyphEnabled)
				{
					int i3 = (l1 * 30 + j2 * 59 + k2 * 11) / 100;
					int j3 = (l1 * 30 + j2 * 70) / 100;
					int k3 = (l1 * 30 + k2 * 70) / 100;
					l1 = i3;
					j2 = j3;
					k2 = k3;
				}

				ImageData[i1 * 4 + 0] = (byte)l1;
				ImageData[i1 * 4 + 1] = (byte)j2;
				ImageData[i1 * 4 + 2] = (byte)k2;
				ImageData[i1 * 4 + 3] = (byte)l2;
			}
		}
	}
}