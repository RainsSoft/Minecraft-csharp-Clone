namespace net.minecraft.src
{
	public class GenLayerShore : GenLayer
	{
		public GenLayerShore(long par1, GenLayer par3GenLayer) : base(par1)
		{
			Parent = par3GenLayer;
		}

		/// <summary>
		/// Returns a list of integer values generated by this layer. These may be interpreted as temperatures, rainfall
		/// amounts, or biomeList[] indices based on the particular GenLayer subclass.
		/// </summary>
		public override int[] GetInts(int par1, int par2, int par3, int par4)
		{
			int[] ai = Parent.GetInts(par1 - 1, par2 - 1, par3 + 2, par4 + 2);
			int[] ai1 = IntCache.GetIntCache(par3 * par4);

			for (int i = 0; i < par4; i++)
			{
				for (int j = 0; j < par3; j++)
				{
					InitChunkSeed(j + par1, i + par2);
					int k = ai[j + 1 + (i + 1) * (par3 + 2)];

					if (k == BiomeGenBase.MushroomIsland.BiomeID)
					{
						int l = ai[j + 1 + ((i + 1) - 1) * (par3 + 2)];
						int k1 = ai[j + 1 + 1 + (i + 1) * (par3 + 2)];
						int j2 = ai[((j + 1) - 1) + (i + 1) * (par3 + 2)];
						int i3 = ai[j + 1 + (i + 1 + 1) * (par3 + 2)];

						if (l == BiomeGenBase.Ocean.BiomeID || k1 == BiomeGenBase.Ocean.BiomeID || j2 == BiomeGenBase.Ocean.BiomeID || i3 == BiomeGenBase.Ocean.BiomeID)
						{
							ai1[j + i * par3] = BiomeGenBase.MushroomIslandShore.BiomeID;
						}
						else
						{
							ai1[j + i * par3] = k;
						}

						continue;
					}

					if (k != BiomeGenBase.Ocean.BiomeID && k != BiomeGenBase.River.BiomeID && k != BiomeGenBase.Swampland.BiomeID && k != BiomeGenBase.ExtremeHills.BiomeID)
					{
						int i1 = ai[j + 1 + ((i + 1) - 1) * (par3 + 2)];
						int l1 = ai[j + 1 + 1 + (i + 1) * (par3 + 2)];
						int k2 = ai[((j + 1) - 1) + (i + 1) * (par3 + 2)];
						int j3 = ai[j + 1 + (i + 1 + 1) * (par3 + 2)];

						if (i1 == BiomeGenBase.Ocean.BiomeID || l1 == BiomeGenBase.Ocean.BiomeID || k2 == BiomeGenBase.Ocean.BiomeID || j3 == BiomeGenBase.Ocean.BiomeID)
						{
							ai1[j + i * par3] = BiomeGenBase.Beach.BiomeID;
						}
						else
						{
							ai1[j + i * par3] = k;
						}

						continue;
					}

					if (k == BiomeGenBase.ExtremeHills.BiomeID)
					{
						int j1 = ai[j + 1 + ((i + 1) - 1) * (par3 + 2)];
						int i2 = ai[j + 1 + 1 + (i + 1) * (par3 + 2)];
						int l2 = ai[((j + 1) - 1) + (i + 1) * (par3 + 2)];
						int k3 = ai[j + 1 + (i + 1 + 1) * (par3 + 2)];

						if (j1 != BiomeGenBase.ExtremeHills.BiomeID || i2 != BiomeGenBase.ExtremeHills.BiomeID || l2 != BiomeGenBase.ExtremeHills.BiomeID || k3 != BiomeGenBase.ExtremeHills.BiomeID)
						{
							ai1[j + i * par3] = BiomeGenBase.ExtremeHillsEdge.BiomeID;
						}
						else
						{
							ai1[j + i * par3] = k;
						}
					}
					else
					{
						ai1[j + i * par3] = k;
					}
				}
			}

			return ai1;
		}
	}
}