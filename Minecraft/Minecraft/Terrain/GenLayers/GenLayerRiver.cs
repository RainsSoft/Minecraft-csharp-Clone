namespace net.minecraft.src
{
	public class GenLayerRiver : GenLayer
	{
		public GenLayerRiver(long par1, GenLayer par3GenLayer) : base(par1)
		{
			base.Parent = par3GenLayer;
		}

		/// <summary>
		/// Returns a list of integer values generated by this layer. These may be interpreted as temperatures, rainfall
		/// amounts, or biomeList[] indices based on the particular GenLayer subclass.
		/// </summary>
		public override int[] GetInts(int par1, int par2, int par3, int par4)
		{
			int i = par1 - 1;
			int j = par2 - 1;
			int k = par3 + 2;
			int l = par4 + 2;
			int[] ai = Parent.GetInts(i, j, k, l);
			int[] ai1 = IntCache.GetIntCache(par3 * par4);

			for (int i1 = 0; i1 < par4; i1++)
			{
				for (int j1 = 0; j1 < par3; j1++)
				{
					int k1 = ai[j1 + 0 + (i1 + 1) * k];
					int l1 = ai[j1 + 2 + (i1 + 1) * k];
					int i2 = ai[j1 + 1 + (i1 + 0) * k];
					int j2 = ai[j1 + 1 + (i1 + 2) * k];
					int k2 = ai[j1 + 1 + (i1 + 1) * k];

					if (k2 == 0 || k1 == 0 || l1 == 0 || i2 == 0 || j2 == 0)
					{
						ai1[j1 + i1 * par3] = BiomeGenBase.River.BiomeID;
						continue;
					}

					if (k2 != k1 || k2 != i2 || k2 != l1 || k2 != j2)
					{
						ai1[j1 + i1 * par3] = BiomeGenBase.River.BiomeID;
					}
					else
					{
						ai1[j1 + i1 * par3] = -1;
					}
				}
			}

			return ai1;
		}
	}
}