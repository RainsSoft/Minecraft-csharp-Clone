namespace net.minecraft.src
{
	public class GenLayerBiome : GenLayer
	{
		private BiomeGenBase[] AllowedBiomes;

		public GenLayerBiome(long par1, GenLayer par3GenLayer, WorldType par4WorldType) : base(par1)
		{
			AllowedBiomes = (new BiomeGenBase[] { BiomeGenBase.Desert, BiomeGenBase.Forest, BiomeGenBase.ExtremeHills, BiomeGenBase.Swampland, BiomeGenBase.Plains, BiomeGenBase.Taiga, BiomeGenBase.Jungle });
			Parent = par3GenLayer;

			if (par4WorldType == WorldType.DEFAULT_1_1)
			{
				AllowedBiomes = (new BiomeGenBase[] { BiomeGenBase.Desert, BiomeGenBase.Forest, BiomeGenBase.ExtremeHills, BiomeGenBase.Swampland, BiomeGenBase.Plains, BiomeGenBase.Taiga });
			}
		}

		/// <summary>
		/// Returns a list of integer values generated by this layer. These may be interpreted as temperatures, rainfall
		/// amounts, or biomeList[] indices based on the particular GenLayer subclass.
		/// </summary>
		public override int[] GetInts(int par1, int par2, int par3, int par4)
		{
			int[] ai = Parent.GetInts(par1, par2, par3, par4);
			int[] ai1 = IntCache.GetIntCache(par3 * par4);

			for (int i = 0; i < par4; i++)
			{
				for (int j = 0; j < par3; j++)
				{
					InitChunkSeed(j + par1, i + par2);
					int k = ai[j + i * par3];

					if (k == 0)
					{
						ai1[j + i * par3] = 0;
						continue;
					}

					if (k == BiomeGenBase.MushroomIsland.BiomeID)
					{
						ai1[j + i * par3] = k;
						continue;
					}

					if (k == 1)
					{
						ai1[j + i * par3] = AllowedBiomes[NextInt(AllowedBiomes.Length)].BiomeID;
					}
					else
					{
						ai1[j + i * par3] = BiomeGenBase.IcePlains.BiomeID;
					}
				}
			}

			return ai1;
		}
	}
}