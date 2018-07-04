using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class ComponentVillageHall : ComponentVillage
	{
		private int AverageGroundLevel;

		public ComponentVillageHall(int par1, Random par2Random, StructureBoundingBox par3StructureBoundingBox, int par4) : base(par1)
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
        public static ComponentVillageHall FindValidPlacement(List<StructureComponent> par0List, Random par1Random, int par2, int par3, int par4, int par5, int par6)
		{
			StructureBoundingBox structureboundingbox = StructureBoundingBox.GetComponentToAddBoundingBox(par2, par3, par4, 0, 0, 0, 9, 7, 11, par5);

			if (!CanVillageGoDeeper(structureboundingbox) || StructureComponent.FindIntersecting(par0List, structureboundingbox) != null)
			{
				return null;
			}
			else
			{
				return new ComponentVillageHall(par6, par1Random, structureboundingbox, par5);
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

				BoundingBox.Offset(0, ((AverageGroundLevel - BoundingBox.MaxY) + 7) - 1, 0);
			}

			FillWithBlocks(par1World, par3StructureBoundingBox, 1, 1, 1, 7, 4, 4, 0, 0, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 2, 1, 6, 8, 4, 10, 0, 0, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 2, 0, 6, 8, 0, 10, Block.Dirt.BlockID, Block.Dirt.BlockID, false);
			PlaceBlockAtCurrentPosition(par1World, Block.Cobblestone.BlockID, 0, 6, 0, 6, par3StructureBoundingBox);
			FillWithBlocks(par1World, par3StructureBoundingBox, 2, 1, 6, 2, 1, 10, Block.Fence.BlockID, Block.Fence.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 8, 1, 6, 8, 1, 10, Block.Fence.BlockID, Block.Fence.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 3, 1, 10, 7, 1, 10, Block.Fence.BlockID, Block.Fence.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 1, 0, 1, 7, 0, 4, Block.Planks.BlockID, Block.Planks.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 0, 0, 0, 3, 5, Block.Cobblestone.BlockID, Block.Cobblestone.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 8, 0, 0, 8, 3, 5, Block.Cobblestone.BlockID, Block.Cobblestone.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 1, 0, 0, 7, 1, 0, Block.Cobblestone.BlockID, Block.Cobblestone.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 1, 0, 5, 7, 1, 5, Block.Cobblestone.BlockID, Block.Cobblestone.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 1, 2, 0, 7, 3, 0, Block.Planks.BlockID, Block.Planks.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 1, 2, 5, 7, 3, 5, Block.Planks.BlockID, Block.Planks.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 4, 1, 8, 4, 1, Block.Planks.BlockID, Block.Planks.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 4, 4, 8, 4, 4, Block.Planks.BlockID, Block.Planks.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 5, 2, 8, 5, 3, Block.Planks.BlockID, Block.Planks.BlockID, false);
			PlaceBlockAtCurrentPosition(par1World, Block.Planks.BlockID, 0, 0, 4, 2, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Planks.BlockID, 0, 0, 4, 3, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Planks.BlockID, 0, 8, 4, 2, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Planks.BlockID, 0, 8, 4, 3, par3StructureBoundingBox);
			int i = GetMetadataWithOffset(Block.StairCompactPlanks.BlockID, 3);
			int j = GetMetadataWithOffset(Block.StairCompactPlanks.BlockID, 2);

			for (int k = -1; k <= 2; k++)
			{
				for (int i1 = 0; i1 <= 8; i1++)
				{
					PlaceBlockAtCurrentPosition(par1World, Block.StairCompactPlanks.BlockID, i, i1, 4 + k, k, par3StructureBoundingBox);
					PlaceBlockAtCurrentPosition(par1World, Block.StairCompactPlanks.BlockID, j, i1, 4 + k, 5 - k, par3StructureBoundingBox);
				}
			}

			PlaceBlockAtCurrentPosition(par1World, Block.Wood.BlockID, 0, 0, 2, 1, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Wood.BlockID, 0, 0, 2, 4, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Wood.BlockID, 0, 8, 2, 1, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Wood.BlockID, 0, 8, 2, 4, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.ThinGlass.BlockID, 0, 0, 2, 2, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.ThinGlass.BlockID, 0, 0, 2, 3, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.ThinGlass.BlockID, 0, 8, 2, 2, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.ThinGlass.BlockID, 0, 8, 2, 3, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.ThinGlass.BlockID, 0, 2, 2, 5, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.ThinGlass.BlockID, 0, 3, 2, 5, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.ThinGlass.BlockID, 0, 5, 2, 0, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.ThinGlass.BlockID, 0, 6, 2, 5, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Fence.BlockID, 0, 2, 1, 3, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.PressurePlatePlanks.BlockID, 0, 2, 2, 3, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Planks.BlockID, 0, 1, 1, 4, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.StairCompactPlanks.BlockID, GetMetadataWithOffset(Block.StairCompactPlanks.BlockID, 3), 2, 1, 4, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.StairCompactPlanks.BlockID, GetMetadataWithOffset(Block.StairCompactPlanks.BlockID, 1), 1, 1, 3, par3StructureBoundingBox);
			FillWithBlocks(par1World, par3StructureBoundingBox, 5, 0, 1, 7, 0, 3, Block.StairDouble.BlockID, Block.StairDouble.BlockID, false);
			PlaceBlockAtCurrentPosition(par1World, Block.StairDouble.BlockID, 0, 6, 1, 1, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.StairDouble.BlockID, 0, 6, 1, 2, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, 0, 0, 2, 1, 0, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, 0, 0, 2, 2, 0, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.TorchWood.BlockID, 0, 2, 3, 1, par3StructureBoundingBox);
			PlaceDoorAtCurrentPosition(par1World, par3StructureBoundingBox, par2Random, 2, 1, 0, GetMetadataWithOffset(Block.DoorWood.BlockID, 1));

			if (GetBlockIdAtCurrentPosition(par1World, 2, 0, -1, par3StructureBoundingBox) == 0 && GetBlockIdAtCurrentPosition(par1World, 2, -1, -1, par3StructureBoundingBox) != 0)
			{
				PlaceBlockAtCurrentPosition(par1World, Block.StairCompactCobblestone.BlockID, GetMetadataWithOffset(Block.StairCompactCobblestone.BlockID, 3), 2, 0, -1, par3StructureBoundingBox);
			}

			PlaceBlockAtCurrentPosition(par1World, 0, 0, 6, 1, 5, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, 0, 0, 6, 2, 5, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.TorchWood.BlockID, 0, 6, 3, 4, par3StructureBoundingBox);
			PlaceDoorAtCurrentPosition(par1World, par3StructureBoundingBox, par2Random, 6, 1, 5, GetMetadataWithOffset(Block.DoorWood.BlockID, 1));

			for (int l = 0; l < 5; l++)
			{
				for (int j1 = 0; j1 < 9; j1++)
				{
					ClearCurrentPositionBlocksUpwards(par1World, j1, 7, l, par3StructureBoundingBox);
					FillCurrentPositionBlocksDownwards(par1World, Block.Cobblestone.BlockID, 0, j1, -1, l, par3StructureBoundingBox);
				}
			}

			SpawnVillagers(par1World, par3StructureBoundingBox, 4, 1, 2, 2);
			return true;
		}

		/// <summary>
		/// Returns the villager type to spawn in this component, based on the number of villagers already spawned.
		/// </summary>
		protected override int GetVillagerType(int par1)
		{
			return par1 != 0 ? 0 : 4;
		}
	}
}