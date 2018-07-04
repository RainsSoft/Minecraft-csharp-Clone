using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class ComponentVillageHouse2 : ComponentVillage
	{
		private static readonly StructurePieceTreasure[] ChestLoot;
		private int AverageGroundLevel;
		private bool HasMadeChest;

		public ComponentVillageHouse2(int par1, Random par2Random, StructureBoundingBox par3StructureBoundingBox, int par4) : base(par1)
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

        public static ComponentVillageHouse2 FindValidPlacement(List<StructureComponent> par0List, Random par1Random, int par2, int par3, int par4, int par5, int par6)
		{
			StructureBoundingBox structureboundingbox = StructureBoundingBox.GetComponentToAddBoundingBox(par2, par3, par4, 0, 0, 0, 10, 6, 7, par5);

			if (!CanVillageGoDeeper(structureboundingbox) || StructureComponent.FindIntersecting(par0List, structureboundingbox) != null)
			{
				return null;
			}
			else
			{
				return new ComponentVillageHouse2(par6, par1Random, structureboundingbox, par5);
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

			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 1, 0, 9, 4, 6, 0, 0, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 0, 0, 9, 0, 6, Block.Cobblestone.BlockID, Block.Cobblestone.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 4, 0, 9, 4, 6, Block.Cobblestone.BlockID, Block.Cobblestone.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 5, 0, 9, 5, 6, Block.StairSingle.BlockID, Block.StairSingle.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 1, 5, 1, 8, 5, 5, 0, 0, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 1, 1, 0, 2, 3, 0, Block.Planks.BlockID, Block.Planks.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 1, 0, 0, 4, 0, Block.Wood.BlockID, Block.Wood.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 3, 1, 0, 3, 4, 0, Block.Wood.BlockID, Block.Wood.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 1, 6, 0, 4, 6, Block.Wood.BlockID, Block.Wood.BlockID, false);
			PlaceBlockAtCurrentPosition(par1World, Block.Planks.BlockID, 0, 3, 3, 1, par3StructureBoundingBox);
			FillWithBlocks(par1World, par3StructureBoundingBox, 3, 1, 2, 3, 3, 2, Block.Planks.BlockID, Block.Planks.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 4, 1, 3, 5, 3, 3, Block.Planks.BlockID, Block.Planks.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 1, 1, 0, 3, 5, Block.Planks.BlockID, Block.Planks.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 1, 1, 6, 5, 3, 6, Block.Planks.BlockID, Block.Planks.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 5, 1, 0, 5, 3, 0, Block.Fence.BlockID, Block.Fence.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 9, 1, 0, 9, 3, 0, Block.Fence.BlockID, Block.Fence.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 6, 1, 4, 9, 4, 6, Block.Cobblestone.BlockID, Block.Cobblestone.BlockID, false);
			PlaceBlockAtCurrentPosition(par1World, Block.LavaMoving.BlockID, 0, 7, 1, 5, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.LavaMoving.BlockID, 0, 8, 1, 5, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.FenceIron.BlockID, 0, 9, 2, 5, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.FenceIron.BlockID, 0, 9, 2, 4, par3StructureBoundingBox);
			FillWithBlocks(par1World, par3StructureBoundingBox, 7, 2, 4, 8, 2, 5, 0, 0, false);
			PlaceBlockAtCurrentPosition(par1World, Block.Cobblestone.BlockID, 0, 6, 1, 3, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.StoneOvenIdle.BlockID, 0, 6, 2, 3, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.StoneOvenIdle.BlockID, 0, 6, 3, 3, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.StairDouble.BlockID, 0, 8, 1, 1, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.ThinGlass.BlockID, 0, 0, 2, 2, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.ThinGlass.BlockID, 0, 0, 2, 4, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.ThinGlass.BlockID, 0, 2, 2, 6, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.ThinGlass.BlockID, 0, 4, 2, 6, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Fence.BlockID, 0, 2, 1, 4, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.PressurePlatePlanks.BlockID, 0, 2, 2, 4, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Planks.BlockID, 0, 1, 1, 5, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.StairCompactPlanks.BlockID, GetMetadataWithOffset(Block.StairCompactPlanks.BlockID, 3), 2, 1, 5, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.StairCompactPlanks.BlockID, GetMetadataWithOffset(Block.StairCompactPlanks.BlockID, 1), 1, 1, 4, par3StructureBoundingBox);

			if (!HasMadeChest)
			{
				int i = GetYWithOffset(1);
				int l = GetXWithOffset(5, 5);
				int j1 = GetZWithOffset(5, 5);

				if (par3StructureBoundingBox.IsVecInside(l, i, j1))
				{
					HasMadeChest = true;
					CreateTreasureChestAtCurrentPosition(par1World, par3StructureBoundingBox, par2Random, 5, 1, 5, ChestLoot, 3 + par2Random.Next(6));
				}
			}

			for (int j = 6; j <= 8; j++)
			{
				if (GetBlockIdAtCurrentPosition(par1World, j, 0, -1, par3StructureBoundingBox) == 0 && GetBlockIdAtCurrentPosition(par1World, j, -1, -1, par3StructureBoundingBox) != 0)
				{
					PlaceBlockAtCurrentPosition(par1World, Block.StairCompactCobblestone.BlockID, GetMetadataWithOffset(Block.StairCompactCobblestone.BlockID, 3), j, 0, -1, par3StructureBoundingBox);
				}
			}

			for (int k = 0; k < 7; k++)
			{
				for (int i1 = 0; i1 < 10; i1++)
				{
					ClearCurrentPositionBlocksUpwards(par1World, i1, 6, k, par3StructureBoundingBox);
					FillCurrentPositionBlocksDownwards(par1World, Block.Cobblestone.BlockID, 0, i1, -1, k, par3StructureBoundingBox);
				}
			}

			SpawnVillagers(par1World, par3StructureBoundingBox, 7, 1, 1, 1);
			return true;
		}

		/// <summary>
		/// Returns the villager type to spawn in this component, based on the number of villagers already spawned.
		/// </summary>
		protected override int GetVillagerType(int par1)
		{
			return 3;
		}

		static ComponentVillageHouse2()
		{
			ChestLoot = (new StructurePieceTreasure[] { new StructurePieceTreasure(Item.Diamond.ShiftedIndex, 0, 1, 3, 3), new StructurePieceTreasure(Item.IngotIron.ShiftedIndex, 0, 1, 5, 10), new StructurePieceTreasure(Item.IngotGold.ShiftedIndex, 0, 1, 3, 5), new StructurePieceTreasure(Item.Bread.ShiftedIndex, 0, 1, 3, 15), new StructurePieceTreasure(Item.AppleRed.ShiftedIndex, 0, 1, 3, 15), new StructurePieceTreasure(Item.PickaxeSteel.ShiftedIndex, 0, 1, 1, 5), new StructurePieceTreasure(Item.SwordSteel.ShiftedIndex, 0, 1, 1, 5), new StructurePieceTreasure(Item.PlateSteel.ShiftedIndex, 0, 1, 1, 5), new StructurePieceTreasure(Item.HelmetSteel.ShiftedIndex, 0, 1, 1, 5), new StructurePieceTreasure(Item.LegsSteel.ShiftedIndex, 0, 1, 1, 5), new StructurePieceTreasure(Item.BootsSteel.ShiftedIndex, 0, 1, 1, 5), new StructurePieceTreasure(Block.Obsidian.BlockID, 0, 3, 7, 5), new StructurePieceTreasure(Block.Sapling.BlockID, 0, 3, 7, 5)
		});
	}
}

}