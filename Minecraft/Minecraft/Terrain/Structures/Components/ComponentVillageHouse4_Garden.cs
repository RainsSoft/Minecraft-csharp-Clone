using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class ComponentVillageHouse4_Garden : ComponentVillage
	{
		private int AverageGroundLevel;
		private readonly bool IsRoofAccessible;

		public ComponentVillageHouse4_Garden(int par1, Random par2Random, StructureBoundingBox par3StructureBoundingBox, int par4) : base(par1)
		{
			AverageGroundLevel = -1;
			CoordBaseMode = par4;
			BoundingBox = par3StructureBoundingBox;
			IsRoofAccessible = par2Random.NextBool();
		}

		/// <summary>
		/// 'Initiates construction of the Structure Component picked, at the current Location of StructGen'
		/// </summary>
        public override void BuildComponent(StructureComponent structurecomponent, List<StructureComponent> list, Random random)
		{
		}

        public static ComponentVillageHouse4_Garden FindValidPlacement(List<StructureComponent> par0List, Random par1Random, int par2, int par3, int par4, int par5, int par6)
		{
			StructureBoundingBox structureboundingbox = StructureBoundingBox.GetComponentToAddBoundingBox(par2, par3, par4, 0, 0, 0, 5, 6, 5, par5);

			if (StructureComponent.FindIntersecting(par0List, structureboundingbox) != null)
			{
				return null;
			}
			else
			{
				return new ComponentVillageHouse4_Garden(par6, par1Random, structureboundingbox, par5);
			}
		}

		/// <summary>
		/// 'second Part of Structure generating, this for example places Spiderwebs, Mob Spawners, it closes Mineshafts at
		/// the end, it adds Fences...'
		/// </summary>
		public override bool AddComponentParts(World par1World, Random par2Random, StructureBoundingBox par3StructureBoundingBox)
		{
			if (AverageGroundLevel < 0)
			{
				AverageGroundLevel = GetAverageGroundLevel(par1World, par3StructureBoundingBox);

				if (AverageGroundLevel < 0)
				{
					return true;
				}

				BoundingBox.Offset(0, ((AverageGroundLevel - BoundingBox.MaxY) + 6) - 1, 0);
			}

			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 0, 0, 4, 0, 4, Block.Cobblestone.BlockID, Block.Cobblestone.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 4, 0, 4, 4, 4, Block.Wood.BlockID, Block.Wood.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 1, 4, 1, 3, 4, 3, Block.Planks.BlockID, Block.Planks.BlockID, false);
			PlaceBlockAtCurrentPosition(par1World, Block.Cobblestone.BlockID, 0, 0, 1, 0, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Cobblestone.BlockID, 0, 0, 2, 0, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Cobblestone.BlockID, 0, 0, 3, 0, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Cobblestone.BlockID, 0, 4, 1, 0, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Cobblestone.BlockID, 0, 4, 2, 0, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Cobblestone.BlockID, 0, 4, 3, 0, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Cobblestone.BlockID, 0, 0, 1, 4, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Cobblestone.BlockID, 0, 0, 2, 4, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Cobblestone.BlockID, 0, 0, 3, 4, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Cobblestone.BlockID, 0, 4, 1, 4, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Cobblestone.BlockID, 0, 4, 2, 4, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Cobblestone.BlockID, 0, 4, 3, 4, par3StructureBoundingBox);
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 1, 1, 0, 3, 3, Block.Planks.BlockID, Block.Planks.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 4, 1, 1, 4, 3, 3, Block.Planks.BlockID, Block.Planks.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 1, 1, 4, 3, 3, 4, Block.Planks.BlockID, Block.Planks.BlockID, false);
			PlaceBlockAtCurrentPosition(par1World, Block.ThinGlass.BlockID, 0, 0, 2, 2, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.ThinGlass.BlockID, 0, 2, 2, 4, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.ThinGlass.BlockID, 0, 4, 2, 2, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Planks.BlockID, 0, 1, 1, 0, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Planks.BlockID, 0, 1, 2, 0, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Planks.BlockID, 0, 1, 3, 0, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Planks.BlockID, 0, 2, 3, 0, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Planks.BlockID, 0, 3, 3, 0, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Planks.BlockID, 0, 3, 2, 0, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Planks.BlockID, 0, 3, 1, 0, par3StructureBoundingBox);

			if (GetBlockIdAtCurrentPosition(par1World, 2, 0, -1, par3StructureBoundingBox) == 0 && GetBlockIdAtCurrentPosition(par1World, 2, -1, -1, par3StructureBoundingBox) != 0)
			{
				PlaceBlockAtCurrentPosition(par1World, Block.StairCompactCobblestone.BlockID, GetMetadataWithOffset(Block.StairCompactCobblestone.BlockID, 3), 2, 0, -1, par3StructureBoundingBox);
			}

			FillWithBlocks(par1World, par3StructureBoundingBox, 1, 1, 1, 3, 3, 3, 0, 0, false);

			if (IsRoofAccessible)
			{
				PlaceBlockAtCurrentPosition(par1World, Block.Fence.BlockID, 0, 0, 5, 0, par3StructureBoundingBox);
				PlaceBlockAtCurrentPosition(par1World, Block.Fence.BlockID, 0, 1, 5, 0, par3StructureBoundingBox);
				PlaceBlockAtCurrentPosition(par1World, Block.Fence.BlockID, 0, 2, 5, 0, par3StructureBoundingBox);
				PlaceBlockAtCurrentPosition(par1World, Block.Fence.BlockID, 0, 3, 5, 0, par3StructureBoundingBox);
				PlaceBlockAtCurrentPosition(par1World, Block.Fence.BlockID, 0, 4, 5, 0, par3StructureBoundingBox);
				PlaceBlockAtCurrentPosition(par1World, Block.Fence.BlockID, 0, 0, 5, 4, par3StructureBoundingBox);
				PlaceBlockAtCurrentPosition(par1World, Block.Fence.BlockID, 0, 1, 5, 4, par3StructureBoundingBox);
				PlaceBlockAtCurrentPosition(par1World, Block.Fence.BlockID, 0, 2, 5, 4, par3StructureBoundingBox);
				PlaceBlockAtCurrentPosition(par1World, Block.Fence.BlockID, 0, 3, 5, 4, par3StructureBoundingBox);
				PlaceBlockAtCurrentPosition(par1World, Block.Fence.BlockID, 0, 4, 5, 4, par3StructureBoundingBox);
				PlaceBlockAtCurrentPosition(par1World, Block.Fence.BlockID, 0, 4, 5, 1, par3StructureBoundingBox);
				PlaceBlockAtCurrentPosition(par1World, Block.Fence.BlockID, 0, 4, 5, 2, par3StructureBoundingBox);
				PlaceBlockAtCurrentPosition(par1World, Block.Fence.BlockID, 0, 4, 5, 3, par3StructureBoundingBox);
				PlaceBlockAtCurrentPosition(par1World, Block.Fence.BlockID, 0, 0, 5, 1, par3StructureBoundingBox);
				PlaceBlockAtCurrentPosition(par1World, Block.Fence.BlockID, 0, 0, 5, 2, par3StructureBoundingBox);
				PlaceBlockAtCurrentPosition(par1World, Block.Fence.BlockID, 0, 0, 5, 3, par3StructureBoundingBox);
			}

			if (IsRoofAccessible)
			{
				int i = GetMetadataWithOffset(Block.Ladder.BlockID, 3);
				PlaceBlockAtCurrentPosition(par1World, Block.Ladder.BlockID, i, 3, 1, 3, par3StructureBoundingBox);
				PlaceBlockAtCurrentPosition(par1World, Block.Ladder.BlockID, i, 3, 2, 3, par3StructureBoundingBox);
				PlaceBlockAtCurrentPosition(par1World, Block.Ladder.BlockID, i, 3, 3, 3, par3StructureBoundingBox);
				PlaceBlockAtCurrentPosition(par1World, Block.Ladder.BlockID, i, 3, 4, 3, par3StructureBoundingBox);
			}

			PlaceBlockAtCurrentPosition(par1World, Block.TorchWood.BlockID, 0, 2, 3, 1, par3StructureBoundingBox);

			for (int j = 0; j < 5; j++)
			{
				for (int k = 0; k < 5; k++)
				{
					ClearCurrentPositionBlocksUpwards(par1World, k, 6, j, par3StructureBoundingBox);
					FillCurrentPositionBlocksDownwards(par1World, Block.Cobblestone.BlockID, 0, k, -1, j, par3StructureBoundingBox);
				}
			}

			SpawnVillagers(par1World, par3StructureBoundingBox, 1, 1, 2, 1);
			return true;
		}
	}
}