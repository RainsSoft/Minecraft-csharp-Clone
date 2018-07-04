using System;

namespace net.minecraft.src
{

	public class GenLayerFuzzyZoom : GenLayer
	{
		public GenLayerFuzzyZoom(long par1, GenLayer par3GenLayer) : base(par1)
		{
			base.Parent = par3GenLayer;
		}

		/// <summary>
		/// Returns a list of integer values generated by this layer. These may be interpreted as temperatures, rainfall
		/// amounts, or biomeList[] indices based on the particular GenLayer subclass.
		/// </summary>
		public override int[] GetInts(int par1, int par2, int par3, int par4)
		{
			int i = par1 >> 1;
			int j = par2 >> 1;
			int k = (par3 >> 1) + 3;
			int l = (par4 >> 1) + 3;
			int[] ai = Parent.GetInts(i, j, k, l);
			int[] ai1 = IntCache.GetIntCache(k * 2 * (l * 2));
			int i1 = k << 1;

			for (int j1 = 0; j1 < l - 1; j1++)
			{
				int k1 = j1 << 1;
				int i2 = k1 * i1;
				int j2 = ai[0 + (j1 + 0) * k];
				int k2 = ai[0 + (j1 + 1) * k];

				for (int l2 = 0; l2 < k - 1; l2++)
				{
					InitChunkSeed(l2 + i << 1, j1 + j << 1);
					int i3 = ai[l2 + 1 + (j1 + 0) * k];
					int j3 = ai[l2 + 1 + (j1 + 1) * k];
					ai1[i2] = j2;
					ai1[i2++ + i1] = Choose(j2, k2);
					ai1[i2] = Choose(j2, i3);
					ai1[i2++ + i1] = Choose(j2, i3, k2, j3);
					j2 = i3;
					k2 = j3;
				}
			}

			int[] ai2 = IntCache.GetIntCache(par3 * par4);

			for (int l1 = 0; l1 < par4; l1++)
			{
				Array.Copy(ai1, (l1 + (par2 & 1)) * (k << 1) + (par1 & 1), ai2, l1 * par3, par3);
			}

			return ai2;
		}

		/// <summary>
		/// randomly choose between the two args
		/// </summary>
		protected virtual int Choose(int par1, int par2)
		{
			return NextInt(2) != 0 ? par2 : par1;
		}

		/// <summary>
		/// randomly choose between the four args
		/// </summary>
		protected virtual int Choose(int par1, int par2, int par3, int par4)
		{
			int i = NextInt(4);

			if (i == 0)
			{
				return par1;
			}

			if (i == 1)
			{
				return par2;
			}

			if (i == 2)
			{
				return par3;
			}
			else
			{
				return par4;
			}
		}
	}

}