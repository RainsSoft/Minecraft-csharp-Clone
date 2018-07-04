using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class ComponentStrongholdRightTurn : ComponentStrongholdLeftTurn
	{
		public ComponentStrongholdRightTurn(int par1, Random par2Random, StructureBoundingBox par3StructureBoundingBox, int par4) : base(par1, par2Random, par3StructureBoundingBox, par4)
		{
		}

		/// <summary>
		/// 'Initiates construction of the Structure Component picked, at the current Location of StructGen'
		/// </summary>
		public override void BuildComponent(StructureComponent par1StructureComponent, List<StructureComponent> par2List, Random par3Random)
		{
			if (CoordBaseMode == 2 || CoordBaseMode == 3)
			{
				GetNextComponentZ((ComponentStrongholdStairs2)par1StructureComponent, par2List, par3Random, 1, 1);
			}
			else
			{
				GetNextComponentX((ComponentStrongholdStairs2)par1StructureComponent, par2List, par3Random, 1, 1);
			}
		}

		/// <summary>
		/// 'second Part of Structure generating, this for example places Spiderwebs, Mob Spawners, it closes Mineshafts at
		/// the end, it adds Fences...'
		/// </summary>
		public override bool AddComponentParts(World par1World, Random par2Random, StructureBoundingBox par3StructureBoundingBox)
		{
			if (IsLiquidInStructureBoundingBox(par1World, par3StructureBoundingBox))
			{
				return false;
			}

			FillWithRandomizedBlocks(par1World, par3StructureBoundingBox, 0, 0, 0, 4, 4, 4, true, par2Random, StructureStrongholdPieces.GetStrongholdStones());
			PlaceDoor(par1World, par2Random, par3StructureBoundingBox, DoorType, 1, 1, 0);

			if (CoordBaseMode == 2 || CoordBaseMode == 3)
			{
				FillWithBlocks(par1World, par3StructureBoundingBox, 4, 1, 1, 4, 3, 3, 0, 0, false);
			}
			else
			{
				FillWithBlocks(par1World, par3StructureBoundingBox, 0, 1, 1, 0, 3, 3, 0, 0, false);
			}

			return true;
		}
	}
}