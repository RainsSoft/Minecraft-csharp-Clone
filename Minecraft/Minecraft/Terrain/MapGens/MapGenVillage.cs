using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class MapGenVillage : MapGenStructure
	{
		/// <summary>
		/// A list of all the biomes villages can spawn in. </summary>
        public static List<BiomeGenBase> VillageSpawnBiomes;

		/// <summary>
		/// World terrain type, 0 for normal, 1 for flat map </summary>
		private readonly int TerrainType;

		public MapGenVillage(int par1)
		{
			TerrainType = par1;
		}

		protected override bool CanSpawnStructureAtCoords(int par1, int par2)
		{
			sbyte byte0 = 32;
			sbyte byte1 = 8;
			int i = par1;
			int j = par2;

			if (par1 < 0)
			{
				par1 -= byte0 - 1;
			}

			if (par2 < 0)
			{
				par2 -= byte0 - 1;
			}

			int k = par1 / byte0;
			int l = par2 / byte0;
			Random random = WorldObj.SetRandomSeed(k, l, 0x9e7f70);
			k *= byte0;
			l *= byte0;
			k += random.Next(byte0 - byte1);
			l += random.Next(byte0 - byte1);
			par1 = i;
			par2 = j;

			if (par1 == k && par2 == l)
			{
				bool flag = WorldObj.GetWorldChunkManager().AreBiomesViable(par1 * 16 + 8, par2 * 16 + 8, 0, VillageSpawnBiomes);

				if (flag)
				{
					return true;
				}
			}

			return false;
		}

		protected override StructureStart GetStructureStart(int par1, int par2)
		{
			return new StructureVillageStart(WorldObj, Rand, par1, par2, TerrainType);
		}

		static MapGenVillage()
		{
			VillageSpawnBiomes = new List<BiomeGenBase> { BiomeGenBase.Plains, BiomeGenBase.Desert };
		}
	}
}