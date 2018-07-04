using System;

namespace net.minecraft.src
{
	public class MapGenMineshaft : MapGenStructure
	{
		public MapGenMineshaft()
		{
		}

		protected override bool CanSpawnStructureAtCoords(int par1, int par2)
		{
			return Rand.Next(100) == 0 && Rand.Next(80) < Math.Max(Math.Abs(par1), Math.Abs(par2));
		}

		protected override StructureStart GetStructureStart(int par1, int par2)
		{
			return new StructureMineshaftStart(WorldObj, Rand, par1, par2);
		}
	}

}