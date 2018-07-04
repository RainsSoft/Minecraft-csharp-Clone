using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class ComponentVillageHouse3 : ComponentVillage
	{
		private int AverageGroundLevel;

		public ComponentVillageHouse3(int par1, Random par2Random, StructureBoundingBox par3StructureBoundingBox, int par4) : base(par1)
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
        public static ComponentVillageHouse3 FindValidPlacement(List<StructureComponent> par0List, Random par1Random, int par2, int par3, int par4, int par5, int par6)
		{
			StructureBoundingBox structureboundingbox = StructureBoundingBox.GetComponentToAddBoundingBox(par2, par3, par4, 0, 0, 0, 9, 7, 12, par5);

			if (!CanVillageGoDeeper(structureboundingbox) || StructureComponent.FindIntersecting(par0List, structureboundingbox) != null)
			{
				return null;
			}
			else
			{
				return new ComponentVillageHouse3(par6, par1Random, structureboundingbox, par5);
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
			FillWithBlocks(par1World, par3StructureBoundingBox, 2, 0, 5, 8, 0, 10, Block.Planks.BlockID, Block.Planks.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 1, 0, 1, 7, 0, 4, Block.Planks.BlockID, Block.Planks.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 0, 0, 0, 3, 5, Block.Cobblestone.BlockID, Block.Cobblestone.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 8, 0, 0, 8, 3, 10, Block.Cobblestone.BlockID, Block.Cobblestone.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 1, 0, 0, 7, 2, 0, Block.Cobblestone.BlockID, Block.Cobblestone.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 1, 0, 5, 2, 1, 5, Block.Cobblestone.BlockID, Block.Cobblestone.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 2, 0, 6, 2, 3, 10, Block.Cobblestone.BlockID, Block.Cobblestone.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 3, 0, 10, 7, 3, 10, Block.Cobblestone.BlockID, Block.Cobblestone.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 1, 2, 0, 7, 3, 0, Block.Planks.BlockID, Block.Planks.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 1, 2, 5, 2, 3, 5, Block.Planks.BlockID, Block.Planks.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 4, 1, 8, 4, 1, Block.Planks.BlockID, Block.Planks.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 4, 4, 3, 4, 4, Block.Planks.BlockID, Block.Planks.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 5, 2, 8, 5, 3, Block.Planks.BlockID, Block.Planks.BlockID, false);
			PlaceBlockAtCurrentPosition(par1World, Block.Planks.BlockID, 0, 0, 4, 2, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Planks.BlockID, 0, 0, 4, 3, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Planks.BlockID, 0, 8, 4, 2, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Planks.BlockID, 0, 8, 4, 3, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Planks.BlockID, 0, 8, 4, 4, par3StructureBoundingBox);
			int i = GetMetadataWithOffset(Block.StairCompactPlanks.BlockID, 3);
			int j = GetMetadataWithOffset(Block.StairCompactPlanks.BlockID, 2);

			for (int k = -1; k <= 2; k++)
			{
				for (int i1 = 0; i1 <= 8; i1++)
				{
					PlaceBlockAtCurrentPosition(par1World, Block.StairCompactPlanks.BlockID, i, i1, 4 + k, k, par3StructureBoundingBox);

					if ((k > -1 || i1 <= 1) && (k > 0 || i1 <= 3) && (k > 1 || i1 <= 4 || i1 >= 6))
					{
						PlaceBlockAtCurrentPosition(par1World, Block.StairCompactPlanks.BlockID, j, i1, 4 + k, 5 - k, par3StructureBoundingBox);
					}
				}
			}

			FillWithBlocks(par1World, par3StructureBoundingBox, 3, 4, 5, 3, 4, 10, Block.Planks.BlockID, Block.Planks.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 7, 4, 2, 7, 4, 10, Block.Planks.BlockID, Block.Planks.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 4, 5, 4, 4, 5, 10, Block.Planks.BlockID, Block.Planks.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 6, 5, 4, 6, 5, 10, Block.Planks.BlockID, Block.Planks.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 5, 6, 3, 5, 6, 10, Block.Planks.BlockID, Block.Planks.BlockID, false);
			int l = GetMetadataWithOffset(Block.StairCompactPlanks.BlockID, 0);

			for (int j1 = 4; j1 >= 1; j1--)
			{
				PlaceBlockAtCurrentPosition(par1World, Block.Planks.BlockID, 0, j1, 2 + j1, 7 - j1, par3StructureBoundingBox);

				for (int l1 = 8 - j1; l1 <= 10; l1++)
				{
					PlaceBlockAtCurrentPosition(par1World, Block.StairCompactPlanks.BlockID, l, j1, 2 + j1, l1, par3StructureBoundingBox);
				}
			}

			int k1 = GetMetadataWithOffset(Block.StairCompactPlanks.BlockID, 1);
			PlaceBlockAtCurrentPosition(par1World, Block.Planks.BlockID, 0, 6, 6, 3, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Planks.BlockID, 0, 7, 5, 4, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.StairCompactPlanks.BlockID, k1, 6, 6, 4, par3StructureBoundingBox);

			for (int i2 = 6; i2 <= 8; i2++)
			{
				for (int l2 = 5; l2 <= 10; l2++)
				{
					PlaceBlockAtCurrentPosition(par1World, Block.StairCompactPlanks.BlockID, k1, i2, 12 - i2, l2, par3StructureBoundingBox);
				}
			}

			PlaceBlockAtCurrentPosition(par1World, Block.Wood.BlockID, 0, 0, 2, 1, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Wood.BlockID, 0, 0, 2, 4, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.ThinGlass.BlockID, 0, 0, 2, 2, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.ThinGlass.BlockID, 0, 0, 2, 3, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Wood.BlockID, 0, 4, 2, 0, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.ThinGlass.BlockID, 0, 5, 2, 0, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Wood.BlockID, 0, 6, 2, 0, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Wood.BlockID, 0, 8, 2, 1, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.ThinGlass.BlockID, 0, 8, 2, 2, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.ThinGlass.BlockID, 0, 8, 2, 3, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Wood.BlockID, 0, 8, 2, 4, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Planks.BlockID, 0, 8, 2, 5, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Wood.BlockID, 0, 8, 2, 6, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.ThinGlass.BlockID, 0, 8, 2, 7, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.ThinGlass.BlockID, 0, 8, 2, 8, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Wood.BlockID, 0, 8, 2, 9, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Wood.BlockID, 0, 2, 2, 6, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.ThinGlass.BlockID, 0, 2, 2, 7, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.ThinGlass.BlockID, 0, 2, 2, 8, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Wood.BlockID, 0, 2, 2, 9, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Wood.BlockID, 0, 4, 4, 10, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.ThinGlass.BlockID, 0, 5, 4, 10, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Wood.BlockID, 0, 6, 4, 10, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Planks.BlockID, 0, 5, 5, 10, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, 0, 0, 2, 1, 0, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, 0, 0, 2, 2, 0, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.TorchWood.BlockID, 0, 2, 3, 1, par3StructureBoundingBox);
			PlaceDoorAtCurrentPosition(par1World, par3StructureBoundingBox, par2Random, 2, 1, 0, GetMetadataWithOffset(Block.DoorWood.BlockID, 1));
			FillWithBlocks(par1World, par3StructureBoundingBox, 1, 0, -1, 3, 2, -1, 0, 0, false);

			if (GetBlockIdAtCurrentPosition(par1World, 2, 0, -1, par3StructureBoundingBox) == 0 && GetBlockIdAtCurrentPosition(par1World, 2, -1, -1, par3StructureBoundingBox) != 0)
			{
				PlaceBlockAtCurrentPosition(par1World, Block.StairCompactCobblestone.BlockID, GetMetadataWithOffset(Block.StairCompactCobblestone.BlockID, 3), 2, 0, -1, par3StructureBoundingBox);
			}

			for (int j2 = 0; j2 < 5; j2++)
			{
				for (int i3 = 0; i3 < 9; i3++)
				{
					ClearCurrentPositionBlocksUpwards(par1World, i3, 7, j2, par3StructureBoundingBox);
					FillCurrentPositionBlocksDownwards(par1World, Block.Cobblestone.BlockID, 0, i3, -1, j2, par3StructureBoundingBox);
				}
			}

			for (int k2 = 5; k2 < 11; k2++)
			{
				for (int j3 = 2; j3 < 9; j3++)
				{
					ClearCurrentPositionBlocksUpwards(par1World, j3, 7, k2, par3StructureBoundingBox);
					FillCurrentPositionBlocksDownwards(par1World, Block.Cobblestone.BlockID, 0, j3, -1, k2, par3StructureBoundingBox);
				}
			}

			SpawnVillagers(par1World, par3StructureBoundingBox, 4, 1, 2, 2);
			return true;
		}
	}

}