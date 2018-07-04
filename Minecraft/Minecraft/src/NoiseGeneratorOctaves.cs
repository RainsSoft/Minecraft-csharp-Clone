using System;

namespace net.minecraft.src
{
	public class NoiseGeneratorOctaves : NoiseGenerator
	{
		private NoiseGeneratorPerlin[] GeneratorCollection;
		private int Octaves;

		public NoiseGeneratorOctaves(Random par1Random, int par2)
		{
			Octaves = par2;
			GeneratorCollection = new NoiseGeneratorPerlin[par2];

			for (int i = 0; i < par2; i++)
			{
				GeneratorCollection[i] = new NoiseGeneratorPerlin(par1Random);
			}
		}

		public virtual double[] GenerateNoiseOctaves(double[] par1ArrayOfDouble, int par2, int par3, int par4, int par5, int par6, int par7, double par8, double par10, double par12)
		{
			if (par1ArrayOfDouble == null)
			{
				par1ArrayOfDouble = new double[par5 * par6 * par7];
			}
			else
			{
				for (int i = 0; i < par1ArrayOfDouble.Length; i++)
				{
					par1ArrayOfDouble[i] = 0.0F;
				}
			}

			double d = 1.0D;

			for (int j = 0; j < Octaves; j++)
			{
				double d1 = (double)par2 * d * par8;
				double d2 = (double)par3 * d * par10;
				double d3 = (double)par4 * d * par12;
				long l = MathHelper2.Floor_double_long(d1);
				long l1 = MathHelper2.Floor_double_long(d3);
				d1 -= l;
				d3 -= l1;
				l %= 0x1000000L;
				l1 %= 0x1000000L;
				d1 += l;
				d3 += l1;
				GeneratorCollection[j].Func_805_a(par1ArrayOfDouble, d1, d2, d3, par5, par6, par7, par8 * d, par10 * d, par12 * d, d);
				d /= 2D;
			}

			return par1ArrayOfDouble;
		}

		/// <summary>
		/// Bouncer function to the main one with some default arguments.
		/// </summary>
		public virtual double[] GenerateNoiseOctaves(double[] par1ArrayOfDouble, int par2, int par3, int par4, int par5, double par6, double par8, double par10)
		{
			return GenerateNoiseOctaves(par1ArrayOfDouble, par2, 10, par3, par4, 1, par5, par6, 1.0D, par8);
		}
	}

}