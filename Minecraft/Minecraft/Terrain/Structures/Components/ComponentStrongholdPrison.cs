using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class ComponentStrongholdPrison : ComponentStronghold
	{
		protected readonly EnumDoor DoorType;

		public ComponentStrongholdPrison(int par1, Random par2Random, StructureBoundingBox par3StructureBoundingBox, int par4) : base(par1)
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

        public static ComponentStrongholdPrison FindValidPlacement(List<StructureComponent> par0List, Random par1Random, int par2, int par3, int par4, int par5, int par6)
		{
			StructureBoundingBox structureboundingbox = StructureBoundingBox.GetComponentToAddBoundingBox(par2, par3, par4, -1, -1, 0, 9, 5, 11, par5);

			if (!CanStrongholdGoDeeper(structureboundingbox) || StructureComponent.FindIntersecting(par0List, structureboundingbox) != null)
			{
				return null;
			}
			else
			{
				return new ComponentStrongholdPrison(par6, par1Random, structureboundingbox, par5);
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
			else
			{
				FillWithRandomizedBlocks(par1World, par3StructureBoundingBox, 0, 0, 0, 8, 4, 10, true, par2Random, StructureStrongholdPieces.GetStrongholdStones());
				PlaceDoor(par1World, par2Random, par3StructureBoundingBox, DoorType, 1, 1, 0);
				FillWithBlocks(par1World, par3StructureBoundingBox, 1, 1, 10, 3, 3, 10, 0, 0, false);
				FillWithRandomizedBlocks(par1World, par3StructureBoundingBox, 4, 1, 1, 4, 3, 1, false, par2Random, StructureStrongholdPieces.GetStrongholdStones());
				FillWithRandomizedBlocks(par1World, par3StructureBoundingBox, 4, 1, 3, 4, 3, 3, false, par2Random, StructureStrongholdPieces.GetStrongholdStones());
				FillWithRandomizedBlocks(par1World, par3StructureBoundingBox, 4, 1, 7, 4, 3, 7, false, par2Random, StructureStrongholdPieces.GetStrongholdStones());
				FillWithRandomizedBlocks(par1World, par3StructureBoundingBox, 4, 1, 9, 4, 3, 9, false, par2Random, StructureStrongholdPieces.GetStrongholdStones());
				FillWithBlocks(par1World, par3StructureBoundingBox, 4, 1, 4, 4, 3, 6, Block.FenceIron.BlockID, Block.FenceIron.BlockID, false);
				FillWithBlocks(par1World, par3StructureBoundingBox, 5, 1, 5, 7, 3, 5, Block.FenceIron.BlockID, Block.FenceIron.BlockID, false);
				PlaceBlockAtCurrentPosition(par1World, Block.FenceIron.BlockID, 0, 4, 3, 2, par3StructureBoundingBox);
				PlaceBlockAtCurrentPosition(par1World, Block.FenceIron.BlockID, 0, 4, 3, 8, par3StructureBoundingBox);
				PlaceBlockAtCurrentPosition(par1World, Block.DoorSteel.BlockID, GetMetadataWithOffset(Block.DoorSteel.BlockID, 3), 4, 1, 2, par3StructureBoundingBox);
				PlaceBlockAtCurrentPosition(par1World, Block.DoorSteel.BlockID, GetMetadataWithOffset(Block.DoorSteel.BlockID, 3) + 8, 4, 2, 2, par3StructureBoundingBox);
				PlaceBlockAtCurrentPosition(par1World, Block.DoorSteel.BlockID, GetMetadataWithOffset(Block.DoorSteel.BlockID, 3), 4, 1, 8, par3StructureBoundingBox);
				PlaceBlockAtCurrentPosition(par1World, Block.DoorSteel.BlockID, GetMetadataWithOffset(Block.DoorSteel.BlockID, 3) + 8, 4, 2, 8, par3StructureBoundingBox);
				return true;
			}
		}
	}
}