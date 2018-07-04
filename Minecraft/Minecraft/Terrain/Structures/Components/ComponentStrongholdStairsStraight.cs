using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class ComponentStrongholdStairsStraight : ComponentStronghold
	{
		private readonly EnumDoor DoorType;

		public ComponentStrongholdStairsStraight(int par1, Random par2Random, StructureBoundingBox par3StructureBoundingBox, int par4) : base(par1)
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
			GetNextComponentNormal((ComponentStrongholdStairs2)par1StructureComponent, par2List, par3Random, 1, 1);
		}

        public static ComponentStrongholdStairsStraight FindValidPlacement(List<StructureComponent> par0List, Random par1Random, int par2, int par3, int par4, int par5, int par6)
		{
			StructureBoundingBox structureboundingbox = StructureBoundingBox.GetComponentToAddBoundingBox(par2, par3, par4, -1, -7, 0, 5, 11, 8, par5);

			if (!CanStrongholdGoDeeper(structureboundingbox) || StructureComponent.FindIntersecting(par0List, structureboundingbox) != null)
			{
				return null;
			}
			else
			{
				return new ComponentStrongholdStairsStraight(par6, par1Random, structureboundingbox, par5);
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

			FillWithRandomizedBlocks(par1World, par3StructureBoundingBox, 0, 0, 0, 4, 10, 7, true, par2Random, StructureStrongholdPieces.GetStrongholdStones());
			PlaceDoor(par1World, par2Random, par3StructureBoundingBox, DoorType, 1, 7, 0);
			PlaceDoor(par1World, par2Random, par3StructureBoundingBox, EnumDoor.OPENING, 1, 1, 7);
			int i = GetMetadataWithOffset(Block.StairCompactCobblestone.BlockID, 2);

			for (int j = 0; j < 6; j++)
			{
				PlaceBlockAtCurrentPosition(par1World, Block.StairCompactCobblestone.BlockID, i, 1, 6 - j, 1 + j, par3StructureBoundingBox);
				PlaceBlockAtCurrentPosition(par1World, Block.StairCompactCobblestone.BlockID, i, 2, 6 - j, 1 + j, par3StructureBoundingBox);
				PlaceBlockAtCurrentPosition(par1World, Block.StairCompactCobblestone.BlockID, i, 3, 6 - j, 1 + j, par3StructureBoundingBox);

				if (j < 5)
				{
					PlaceBlockAtCurrentPosition(par1World, Block.StoneBrick.BlockID, 0, 1, 5 - j, 1 + j, par3StructureBoundingBox);
					PlaceBlockAtCurrentPosition(par1World, Block.StoneBrick.BlockID, 0, 2, 5 - j, 1 + j, par3StructureBoundingBox);
					PlaceBlockAtCurrentPosition(par1World, Block.StoneBrick.BlockID, 0, 3, 5 - j, 1 + j, par3StructureBoundingBox);
				}
			}

			return true;
		}
	}
}