using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class ComponentVillageHouse1 : ComponentVillage
	{
		private int AverageGroundLevel;

		public ComponentVillageHouse1(int par1, Random par2Random, StructureBoundingBox par3StructureBoundingBox, int par4) : base(par1)
		{
			AverageGroundLevel = -1;
			CoordBaseMode = par4;
			BoundingBox = par3StructureBoundingBox;
		}

		/// <summary>
		/// 'Initiates construction of the Structure Component picked, at the current Location of StructGen'
		/// </summary>
        public override void BuildComponent(StructureComponent structurecomponent, List<StructureComponent> list, Random random)
		{
		}

		/// <summary>
		/// Trys to find a valid place to put this component.
		/// </summary>
        public static ComponentVillageHouse1 FindValidPlacement(List<StructureComponent> par0List, Random par1Random, int par2, int par3, int par4, int par5, int par6)
		{
			StructureBoundingBox structureboundingbox = StructureBoundingBox.GetComponentToAddBoundingBox(par2, par3, par4, 0, 0, 0, 9, 9, 6, par5);

			if (!CanVillageGoDeeper(structureboundingbox) || StructureComponent.FindIntersecting(par0List, structureboundingbox) != null)
			{
				return null;
			}
			else
			{
				return new ComponentVillageHouse1(par6, par1Random, structureboundingbox, par5);
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

				BoundingBox.Offset(0, ((AverageGroundLevel - BoundingBox.MaxY) + 9) - 1, 0);
			}

			FillWithBlocks(par1World, par3StructureBoundingBox, 1, 1, 1, 7, 5, 4, 0, 0, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 0, 0, 8, 0, 5, Block.Cobblestone.BlockID, Block.Cobblestone.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 5, 0, 8, 5, 5, Block.Cobblestone.BlockID, Block.Cobblestone.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 6, 1, 8, 6, 4, Block.Cobblestone.BlockID, Block.Cobblestone.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 7, 2, 8, 7, 3, Block.Cobblestone.BlockID, Block.Cobblestone.BlockID, false);
			int i = GetMetadataWithOffset(Block.StairCompactPlanks.BlockID, 3);
			int j = GetMetadataWithOffset(Block.StairCompactPlanks.BlockID, 2);

			for (int k = -1; k <= 2; k++)
			{
				for (int i1 = 0; i1 <= 8; i1++)
				{
					PlaceBlockAtCurrentPosition(par1World, Block.StairCompactPlanks.BlockID, i, i1, 6 + k, k, par3StructureBoundingBox);
					PlaceBlockAtCurrentPosition(par1World, Block.StairCompactPlanks.BlockID, j, i1, 6 + k, 5 - k, par3StructureBoundingBox);
				}
			}

			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 1, 0, 0, 1, 5, Block.Cobblestone.BlockID, Block.Cobblestone.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 1, 1, 5, 8, 1, 5, Block.Cobblestone.BlockID, Block.Cobblestone.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 8, 1, 0, 8, 1, 4, Block.Cobblestone.BlockID, Block.Cobblestone.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 2, 1, 0, 7, 1, 0, Block.Cobblestone.BlockID, Block.Cobblestone.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 2, 0, 0, 4, 0, Block.Cobblestone.BlockID, Block.Cobblestone.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 2, 5, 0, 4, 5, Block.Cobblestone.BlockID, Block.Cobblestone.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 8, 2, 5, 8, 4, 5, Block.Cobblestone.BlockID, Block.Cobblestone.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 8, 2, 0, 8, 4, 0, Block.Cobblestone.BlockID, Block.Cobblestone.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 2, 1, 0, 4, 4, Block.Planks.BlockID, Block.Planks.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 1, 2, 5, 7, 4, 5, Block.Planks.BlockID, Block.Planks.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 8, 2, 1, 8, 4, 4, Block.Planks.BlockID, Block.Planks.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 1, 2, 0, 7, 4, 0, Block.Planks.BlockID, Block.Planks.BlockID, false);
			PlaceBlockAtCurrentPosition(par1World, Block.ThinGlass.BlockID, 0, 4, 2, 0, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.ThinGlass.BlockID, 0, 5, 2, 0, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.ThinGlass.BlockID, 0, 6, 2, 0, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.ThinGlass.BlockID, 0, 4, 3, 0, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.ThinGlass.BlockID, 0, 5, 3, 0, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.ThinGlass.BlockID, 0, 6, 3, 0, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.ThinGlass.BlockID, 0, 0, 2, 2, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.ThinGlass.BlockID, 0, 0, 2, 3, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.ThinGlass.BlockID, 0, 0, 3, 2, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.ThinGlass.BlockID, 0, 0, 3, 3, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.ThinGlass.BlockID, 0, 8, 2, 2, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.ThinGlass.BlockID, 0, 8, 2, 3, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.ThinGlass.BlockID, 0, 8, 3, 2, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.ThinGlass.BlockID, 0, 8, 3, 3, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.ThinGlass.BlockID, 0, 2, 2, 5, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.ThinGlass.BlockID, 0, 3, 2, 5, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.ThinGlass.BlockID, 0, 5, 2, 5, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.ThinGlass.BlockID, 0, 6, 2, 5, par3StructureBoundingBox);
			FillWithBlocks(par1World, par3StructureBoundingBox, 1, 4, 1, 7, 4, 1, Block.Planks.BlockID, Block.Planks.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 1, 4, 4, 7, 4, 4, Block.Planks.BlockID, Block.Planks.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 1, 3, 4, 7, 3, 4, Block.BookShelf.BlockID, Block.BookShelf.BlockID, false);
			PlaceBlockAtCurrentPosition(par1World, Block.Planks.BlockID, 0, 7, 1, 4, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.StairCompactPlanks.BlockID, GetMetadataWithOffset(Block.StairCompactPlanks.BlockID, 0), 7, 1, 3, par3StructureBoundingBox);
			int l = GetMetadataWithOffset(Block.StairCompactPlanks.BlockID, 3);
			PlaceBlockAtCurrentPosition(par1World, Block.StairCompactPlanks.BlockID, l, 6, 1, 4, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.StairCompactPlanks.BlockID, l, 5, 1, 4, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.StairCompactPlanks.BlockID, l, 4, 1, 4, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.StairCompactPlanks.BlockID, l, 3, 1, 4, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Fence.BlockID, 0, 6, 1, 3, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.PressurePlatePlanks.BlockID, 0, 6, 2, 3, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Fence.BlockID, 0, 4, 1, 3, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.PressurePlatePlanks.BlockID, 0, 4, 2, 3, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Workbench.BlockID, 0, 7, 1, 1, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, 0, 0, 1, 1, 0, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, 0, 0, 1, 2, 0, par3StructureBoundingBox);
			PlaceDoorAtCurrentPosition(par1World, par3StructureBoundingBox, par2Random, 1, 1, 0, GetMetadataWithOffset(Block.DoorWood.BlockID, 1));

			if (GetBlockIdAtCurrentPosition(par1World, 1, 0, -1, par3StructureBoundingBox) == 0 && GetBlockIdAtCurrentPosition(par1World, 1, -1, -1, par3StructureBoundingBox) != 0)
			{
				PlaceBlockAtCurrentPosition(par1World, Block.StairCompactCobblestone.BlockID, GetMetadataWithOffset(Block.StairCompactCobblestone.BlockID, 3), 1, 0, -1, par3StructureBoundingBox);
			}

			for (int j1 = 0; j1 < 6; j1++)
			{
				for (int k1 = 0; k1 < 9; k1++)
				{
					ClearCurrentPositionBlocksUpwards(par1World, k1, 9, j1, par3StructureBoundingBox);
					FillCurrentPositionBlocksDownwards(par1World, Block.Cobblestone.BlockID, 0, k1, -1, j1, par3StructureBoundingBox);
				}
			}

			SpawnVillagers(par1World, par3StructureBoundingBox, 2, 1, 2, 1);
			return true;
		}

		/// <summary>
		/// Returns the villager type to spawn in this component, based on the number of villagers already spawned.
		/// </summary>
		protected override int GetVillagerType(int par1)
		{
			return 1;
		}
	}

}