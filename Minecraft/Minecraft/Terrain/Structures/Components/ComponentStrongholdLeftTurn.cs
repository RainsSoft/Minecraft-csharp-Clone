using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class ComponentStrongholdLeftTurn : ComponentStronghold
	{
		protected readonly EnumDoor DoorType;

		public ComponentStrongholdLeftTurn(int par1, Random par2Random, StructureBoundingBox par3StructureBoundingBox, int par4) : base(par1)
		{
			CoordBaseMode = par4;
			DoorType = GetRandomDoor(par2Random);
			BoundingBox = par3StructureBoundingBox;
		}

		/// <summary>
		/// 'Initiates construction of the Structure Component picked, at the current Location of StructGen'
		/// </summary>
        public override void BuildComponent(StructureComponent par1StructureComponent, List<StructureComponent> par2List, Random par3Random)
		{
			if (CoordBaseMode == 2 || CoordBaseMode == 3)
			{
				GetNextComponentX((ComponentStrongholdStairs2)par1StructureComponent, par2List, par3Random, 1, 1);
			}
			else
			{
				GetNextComponentZ((ComponentStrongholdStairs2)par1StructureComponent, par2List, par3Random, 1, 1);
			}
		}

        public static ComponentStrongholdLeftTurn FindValidPlacement(List<StructureComponent> par0List, Random par1Random, int par2, int par3, int par4, int par5, int par6)
		{
			StructureBoundingBox structureboundingbox = StructureBoundingBox.GetComponentToAddBoundingBox(par2, par3, par4, -1, -1, 0, 5, 5, 5, par5);

			if (!CanStrongholdGoDeeper(structureboundingbox) || StructureComponent.FindIntersecting(par0List, structureboundingbox) != null)
			{
				return null;
			}
			else
			{
				return new ComponentStrongholdLeftTurn(par6, par1Random, structureboundingbox, par5);
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
				FillWithBlocks(par1World, par3StructureBoundingBox, 0, 1, 1, 0, 3, 3, 0, 0, false);
			}
			else
			{
				FillWithBlocks(par1World, par3StructureBoundingBox, 4, 1, 1, 4, 3, 3, 0, 0, false);
			}

			return true;
		}
	}
}