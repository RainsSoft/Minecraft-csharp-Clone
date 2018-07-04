using System;

namespace net.minecraft.src
{
	public class TextureWaterFX : TextureFX
	{
		protected float[] Red;
		protected float[] Green;
		protected float[] Blue;
		protected float[] Alpha;
		private int TickCounter;

		public TextureWaterFX()
            : base(Block.WaterMoving.BlockIndexInTexture)
		{
			Red = new float[256];
			Green = new float[256];
			Blue = new float[256];
			Alpha = new float[256];
			TickCounter = 0;
		}

		public override void OnTick()
		{
			TickCounter++;

			for (int i = 0; i < 16; i++)
			{
				for (int k = 0; k < 16; k++)
				{
					float f = 0.0F;

					for (int j1 = i - 1; j1 <= i + 1; j1++)
					{
						int k1 = j1 & 0xf;
						int i2 = k & 0xf;
						f += Red[k1 + i2 * 16];
					}

					Green[i + k * 16] = f / 3.3F + Blue[i + k * 16] * 0.8F;
				}
			}

			for (int j = 0; j < 16; j++)
			{
				for (int l = 0; l < 16; l++)
				{
					Blue[j + l * 16] += Alpha[j + l * 16] * 0.05F;

					if (Blue[j + l * 16] < 0.0F)
					{
						Blue[j + l * 16] = 0.0F;
					}

					Alpha[j + l * 16] -= 0.1F;

					if ((new Random(1)).NextDouble() < 0.050000000000000003D)
					{
						Alpha[j + l * 16] = 0.5F;
					}
				}
			}

			float[] af = Green;
			Green = Red;
			Red = af;

			for (int i1 = 0; i1 < 256; i1++)
			{
				float f1 = Red[i1];

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