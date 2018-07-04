using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class ComponentVillageChurch : ComponentVillage
	{
		private int AverageGroundLevel;

		public ComponentVillageChurch(int par1, Random par2Random, StructureBoundingBox par3StructureBoundingBox, int par4) : base(par1)
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
        public static ComponentVillageChurch FindValidPlacement(List<StructureComponent> par0List, Random par1Random, int par2, int par3, int par4, int par5, int par6)
		{
			StructureBoundingBox structureboundingbox = StructureBoundingBox.GetComponentToAddBoundingBox(par2, par3, par4, 0, 0, 0, 5, 12, 9, par5);

			if (!CanVillageGoDeeper(structureboundingbox) || StructureComponent.FindIntersecting(par0List, structureboundingbox) != null)
			{
				return null;
			}
			else
			{
				return new ComponentVillageChurch(par6, par1Random, structureboundingbox, par5);
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

				BoundingBox.Offset(0, ((AverageGroundLevel - BoundingBox.MaxY) + 12) - 1, 0);
			}

			FillWithBlocks(par1World, par3StructureBoundingBox, 1, 1, 1, 3, 3, 7, 0, 0, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 1, 5, 1, 3, 9, 3, 0, 0, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 1, 0, 0, 3, 0, 8, Block.Cobblestone.BlockID, Block.Cobblestone.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 1, 1, 0, 3, 10, 0, Block.Cobblestone.BlockID, Block.Cobblestone.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 1, 1, 0, 10, 3, Block.Cobblestone.BlockID, Block.Cobblestone.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 4, 1, 1, 4, 10, 3, Block.Cobblestone.BlockID, Block.Cobblestone.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 0, 4, 0, 4, 7, Block.Cobblestone.BlockID, Block.Cobblestone.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 4, 0, 4, 4, 4, 7, Block.Cobblestone.BlockID, Block.Cobblestone.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 1, 1, 8, 3, 4, 8, Block.Cobblestone.BlockID, Block.Cobblestone.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 1, 5, 4, 3, 10, 4, Block.Cobblestone.BlockID, Block.Cobblestone.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 1, 5, 5, 3, 5, 7, Block.Cobblestone.BlockID, Block.Cobblestone.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 9, 0, 4, 9, 4, Block.Cobblestone.BlockID, Block.Cobblestone.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 4, 0, 4, 4, 4, Block.Cobblestone.BlockID, Block.Cobblestone.BlockID, false);
			PlaceBlockAtCurrentPosition(par1World, Block.Cobblestone.BlockID, 0, 0, 11, 2, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Cobblestone.BlockID, 0, 4, 11, 2, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Cobblestone.BlockID, 0, 2, 11, 0, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Cobblestone.BlockID, 0, 2, 11, 4, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Cobblestone.BlockID, 0, 1, 1, 6, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Cobblestone.BlockID, 0, 1, 1, 7, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Cobblestone.BlockID, 0, 2, 1, 7, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Cobblestone.BlockID, 0, 3, 1, 6, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Cobblestone.BlockID, 0, 3, 1, 7, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.StairCompactCobblestone.BlockID, GetMetadataWithOffset(Block.StairCompactCobblestone.BlockID, 3), 1, 1, 5, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.StairCompactCobblestone.BlockID, GetMetadataWithOffset(Block.StairCompactCobblestone.BlockID, 3), 2, 1, 6, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.StairCompactCobblestone.BlockID, GetMetadataWithOffset(Block.StairCompactCobblestone.BlockID, 3), 3, 1, 5, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.StairCompactCobblestone.BlockID, GetMetadataWithOffset(Block.StairCompactCobblestone.BlockID, 1), 1, 2, 7, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.StairCompactCobblestone.BlockID, GetMetadataWithOffset(Block.StairCompactCobblestone.BlockID, 0), 3, 2, 7, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.ThinGlass.BlockID, 0, 0, 2, 2, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.ThinGlass.BlockID, 0, 0, 3, 2, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.ThinGlass.BlockID, 0, 4, 2, 2, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.ThinGlass.BlockID, 0, 4, 3, 2, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.ThinGlass.BlockID, 0, 0, 6, 2, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.ThinGlass.BlockID, 0, 0, 7, 2, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.ThinGlass.BlockID, 0, 4, 6, 2, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.ThinGlass.BlockID, 0, 4, 7, 2, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.ThinGlass.BlockID, 0, 2, 6, 0, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.ThinGlass.BlockID, 0, 2, 7, 0, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.ThinGlass.BlockID, 0, 2, 6, 4, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.ThinGlass.BlockID, 0, 2, 7, 4, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.ThinGlass.BlockID, 0, 0, 3, 6, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.ThinGlass.BlockID, 0, 4, 3, 6, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.ThinGlass.BlockID, 0, 2, 3, 8, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.TorchWood.BlockID, 0, 2, 4, 7, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.TorchWood.BlockID, 0, 1, 4, 6, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.TorchWood.BlockID, 0, 3, 4, 6, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.TorchWood.BlockID, 0, 2, 4, 5, par3StructureBoundingBox);
			int i = GetMetadataWithOffset(Block.Ladder.BlockID, 4);

			for (int j = 1; j <= 9; j++)
			{
				PlaceBlockAtCurrentPosition(par1World, Block.Ladder.BlockID, i, 3, j, 3, par3StructureBoundingBox);
			}

			PlaceBlockAtCurrentPosition(par1World, 0, 0, 2, 1, 0, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, 0, 0, 2, 2, 0, par3StructureBoundingBox);
			PlaceDoorAtCurrentPosition(par1World, par3StructureBoundingBox, par2Random, 2, 1, 0, GetMetadataWithOffset(Block.DoorWood.BlockID, 1));

			if (GetBlockIdAtCurrentPosition(par1World, 2, 0, -1, par3StructureBoundingBox) == 0 && GetBlockIdAtCurrentPosition(par1World, 2, -1, -1, par3StructureBoundingBox) != 0)
			{
				PlaceBlockAtCurrentPosition(par1World, Block.StairCompactCobblestone.BlockID, GetMetadataWithOffset(Block.StairCompactCobblestone.BlockID, 3), 2, 0, -1, par3StructureBoundingBox);
			}

			for (int k = 0; k < 9; k++)
			{
				for (int l = 0; l < 5; l++)
				{
					ClearCurrentPositionBlocksUpwards(par1World, l, 12, k, par3StructureBoundingBox);
					FillCurrentPositionBlocksDownwards(par1World, Block.Cobblestone.BlockID, 0, l, -1, k, par3StructureBoundingBox);
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
			return 2;
		}
	}
}