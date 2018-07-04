using System;

namespace net.minecraft.src
{
	public sealed class StructureStrongholdPieceWeight2 : StructureStrongholdPieceWeight
	{
		public StructureStrongholdPieceWeight2(Type par1Class, int par2, int par3) : base(par1Class, par2, par3)
		{
		}

		public override bool CanSpawnMoreStructuresOfType(int par1)
		{
			return base.CanSpawnMoreStructuresOfType(par1) && par1 > 4;
		}
	}
}