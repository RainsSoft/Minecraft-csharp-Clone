using System.Collections.Generic;

namespace net.minecraft.src
{
	public class MapGenNetherBridge : MapGenStructure
	{
        private List<SpawnListEntry> SpawnList;

		public MapGenNetherBridge()
		{
            SpawnList = new List<SpawnListEntry>();
			SpawnList.Add(new SpawnListEntry(typeof(net.minecraft.src.EntityBlaze), 10, 2, 3));
			SpawnList.Add(new SpawnListEntry(typeof(net.minecraft.src.EntityPigZombie), 10, 4, 4));
			SpawnList.Add(new SpawnListEntry(typeof(net.minecraft.src.EntityMagmaCube), 3, 4, 4));
		}

		public virtual List<SpawnListEntry> GetSpawnList()
		{
			return SpawnList;
		}

		protected override bool CanSpawnStructureAtCoords(int par1, int par2)
		{
			int i = par1 >> 4;
			int j = par2 >> 4;
			Rand.SetSeed((i ^ j << 4) ^ (int)WorldObj.GetSeed());
			Rand.Next();

			if (Rand.Next(3) != 0)
			{
				return false;
			}

			if (par1 != (i << 4) + 4 + Rand.Next(8))
			{
				return false;
			}

			return par2 == (j << 4) + 4 + Rand.Next(8);
		}

		protected override StructureStart GetStructureStart(int par1, int par2)
		{
			return new StructureNetherBridgeStart(WorldObj, Rand, par1, par2);
		}
	}

}