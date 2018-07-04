using System;

namespace net.minecraft.src
{
	public class NoiseGeneratorPerlin : NoiseGenerator
	{
		private int[] Permutations;
		public double XCoord;
		public double YCoord;
		public double ZCoord;

		public NoiseGeneratorPerlin() : this(new Random())
		{
		}

		public NoiseGeneratorPerlin(Random par1Random)
		{
			Permutations = new int[512];
			XCoord = par1Random.NextDouble() * 256D;
			YCoord = par1Random.NextDouble() * 256D;
			ZCoord = par1Random.NextDouble() * 256D;

			for (int i = 0; i < 256; i++)
			{
				Permutations[i] = i;
			}

			for (int j = 0; j < 256; j++)
			{
				int k = par1Random.Next(256 - j) + j;
				int l = Permutations[j];
				Permutations[j] = Permutations[k];
				Permutations[k] = l;
				Permutations[j + 256] = Permutations[j];
			}
		}

		public double Lerp(double par1, double par3, double par5)
		{
			return par3 + par1 * (par5 - par3);
		}

		public double Func_4110_a(int par1, double par2, double par4)
		{
			int i = par1 & 0xf;
			double d = (double)(1 - ((i & 8) >> 3)) * par2;
			double d1 = i >= 4 ? i != 12 && i != 14 ? par4 : par2 : 0.0F;
			return ((i & 1) != 0 ? - d : d) + ((i & 2) != 0 ? - d1 : d1);
		}

		public double Grad(int par1, double par2, double par4, double par6)
		{
			int i = par1 & 0xf;
			double d = i >= 8 ? par4 : par2;
			double d1 = i >= 4 ? i != 12 && i != 14 ? par6 : par2 : par4;
			return ((i & 1) != 0 ? - d : d) + ((i & 2) != 0 ? - d1 : d1);
		}

		public virtual void Func_805_a(double[] par1ArrayOfDouble, double par2, double par4, double par6, int par8, int par9, int par10, double par11, double par13, double par15, double par17)
		{
			if (par9 == 1)
			{
				bool flag = false;
				bool flag1 = false;
				bool flag2 = false;
				bool flag3 = false;
				double d1 = 0.0F;
				double d3 = 0.0F;
				int k2 = 0;
				double d5 = 1.0D / par17;

				for (int j3 = 0; j3 < par8; j3++)
				{
					double d7 = par2 + (double)j3 * par11 + XCoord;
					int k3 = (int)d7;

					if (d7 < (double)k3)
					{
						k3--;
					}

					int l3 = k3 & 0xff;
					d7 -= k3;
					double d10 = d7 * d7 * d7 * (d7 * (d7 * 6D - 15D) + 10D);

					for (int i4 = 0; i4 < par10; i4++)
					{
						double d12 = par6 + (double)i4 * par15 + ZCoord;
						int k4 = (int)d12;

						if (d12 < (double)k4)
						{
							k4--;
						}

						int i5 = k4 & 0xff;
						d12 -= k4;
						double d14 = d12 * d12 * d12 * (d12 * (d12 * 6D - 15D) + 10D);
						int i = Permutations[l3] + 0;
						int k = Permutations[i] + i5;
						int l = Permutations[l3 + 1] + 0;
						int i1 = Permutations[l] + i5;
						double d2 = Lerp(d10, Func_4110_a(Permutations[k], d7, d12), Grad(Permutations[i1], d7 - 1.0D, 0.0F, d12));
						double d4 = Lerp(d10, Grad(Permutations[k + 1], d7, 0.0F, d12 - 1.0D), Grad(Permutations[i1 + 1], d7 - 1.0D, 0.0F, d12 - 1.0D));
						double d16 = Lerp(d14, d2, d4);
						par1ArrayOfDouble[k2++] += d16 * d5;
					}
				}

				return;
			}

			int j = 0;
			double d = 1.0D / par17;
			int j1 = -1;
			bool flag4 = false;
			bool flag5 = false;
			bool flag6 = false;
			bool flag7 = false;
			bool flag8 = false;
			bool flag9 = false;
			double d6 = 0.0F;
			double d8 = 0.0F;
			double d9 = 0.0F;
			double d11 = 0.0F;

			for (int j4 = 0; j4 < par8; j4++)
			{
				double d13 = par2 + (double)j4 * par11 + XCoord;
				int l4 = (int)d13;

				if (d13 < (double)l4)
				{
					l4--;
				}

				int j5 = l4 & 0xff;
				d13 -= l4;
				double d15 = d13 * d13 * d13 * (d13 * (d13 * 6D - 15D) + 10D);

				for (int k5 = 0; k5 < par10; k5++)
				{
					double d17 = par6 + (double)k5 * par15 + ZCoord;
					int l5 = (int)d17;

					if (d17 < (double)l5)
					{
						l5--;
					}

					int i6 = l5 & 0xff;
					d17 -= l5;
					double d18 = d17 * d17 * d17 * (d17 * (d17 * 6D - 15D) + 10D);

					for (int j6 = 0; j6 < par9; j6++)
					{
						double d19 = par4 + (double)j6 * par13 + YCoord;
						int k6 = (int)d19;

						if (d19 < (double)k6)
						{
							k6--;
						}

						int l6 = k6 & 0xff;
						d19 -= k6;
						double d20 = d19 * d19 * d19 * (d19 * (d19 * 6D - 15D) + 10D);

						if (j6 == 0 || l6 != j1)
						{
							j1 = l6;
							int k1 = Permutations[j5] + l6;
							int l1 = Permutations[k1] + i6;
							int i2 = Permutations[k1 + 1] + i6;
							int j2 = Permutations[j5 + 1] + l6;
							int l2 = Permutations[j2] + i6;
							int i3 = Permutations[j2 + 1] + i6;
							d6 = Lerp(d15, Grad(Permutations[l1], d13, d19, d17), Grad(Permutations[l2], d13 - 1.0D, d19, d17));
							d8 = Lerp(d15, Grad(Permutations[i2], d13, d19 - 1.0D, d17), Grad(Permutations[i3], d13 - 1.0D, d19 - 1.0D, d17));
							d9 = Lerp(d15, Grad(Permutations[l1 + 1], d13, d19, d17 - 1.0D), Grad(Permutations[l2 + 1], d13 - 1.0D, d19, d17 - 1.0D));
							d11 = Lerp(d15, Grad(Permutations[i2 + 1], d13, d19 - 1.0D, d17 - 1.0D), Grad(Permutations[i3 + 1], d13 - 1.0D, d19 - 1.0D, d17 - 1.0D));
						}

						double d21 = Lerp(d20, d6, d8);
						double d22 = Lerp(d20, d9, d11);
						double d23 = Lerp(d18, d21, d22);
						par1ArrayOfDouble[j++] += d23 * d;
					}
				}
			}
		}
	}
}