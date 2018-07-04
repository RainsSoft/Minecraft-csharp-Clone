namespace net.minecraft.src
{
	public class GenLayerHills : GenLayer
	{
		public GenLayerHills(long par1, GenLayer par3GenLayer) : base(par1)
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

					if (NextInt(3) == 0)
					{
						int l = k;

						if (k == BiomeGenBase.Desert.BiomeID)
						{
							l = BiomeGenBase.DesertHills.BiomeID;
						}
						else if (k == BiomeGenBase.Forest.BiomeID)
						{
							l = BiomeGenBase.ForestHills.BiomeID;
						}
						else if (k == BiomeGenBase.Taiga.BiomeID)
						{
							l = BiomeGenBase.TaigaHills.BiomeID;
						}
						else if (k == BiomeGenBase.Plains.BiomeID)
						{
							l = BiomeGenBase.Forest.BiomeID;
						}
						else if (k == BiomeGenBase.IcePlains.BiomeID)
						{
							l = BiomeGenBase.IceMountains.BiomeID;
						}
						else if (k == BiomeGenBase.Jungle.BiomeID)
						{
							l = BiomeGenBase.JungleHills.BiomeID;
						}

						if (l != k)
						{
							int i1 = ai[j + 1 + ((i + 1) - 1) * (par3 + 2)];
							int j1 = ai[j + 1 + 1 + (i + 1) * (par3 + 2)];
							int k1 = ai[((j + 1) - 1) + (i + 1) * (par3 + 2)];
							int l1 = ai[j + 1 + (i + 1 + 1) * (par3 + 2)];

							if (i1 == k && j1 == k && k1 == k && l1 == k)
							{
								ai1[j + i * par3] = l;
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