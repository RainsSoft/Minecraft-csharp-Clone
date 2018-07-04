using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class ComponentVillageWoodHut : ComponentVillage
	{
		private int AverageGroundLevel;
		private readonly bool IsTallHouse;
		private readonly int TablePosition;

		public ComponentVillageWoodHut(int par1, Random par2Random, StructureBoundingBox par3StructureBoundingBox, int par4) : base(par1)
		{
			AverageGroundLevel = -1;
			CoordBaseMode = par4;
			BoundingBox = par3StructureBoundingBox;
			IsTallHouse = par2Random.NextBool();
			TablePosition = par2Random.Next(3);
		}

		/// <summary>
		/// 'Initiates construction of the Structure Component picked, at the current Location of StructGen'
		/// </summary>
        public virtual void BuildComponent(StructureComponent structurecomponent, List<StructureComponent> list, Random random)
		{
		}

        public static ComponentVillageWoodHut FindValidPlacement(List<StructureComponent> par0List, Random par1Random, int par2, int par3, int par4, int par5, int par6)
		{
			StructureBoundingBox structureboundingbox = StructureBoundingBox.GetComponentToAddBoundingBox(par2, par3, par4, 0, 0, 0, 4, 6, 5, par5);

			if (!CanVillageGoDeeper(structureboundingbox) || StructureComponent.FindIntersecting(par0List, structureboundingbox) != null)
			{
				return null;
			}
			else
			{
				return new ComponentVillageWoodHut(par6, par1Random, structureboundingbox, par5);
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

			FillWithBlocks(par1World, par3StructureBoundingBox, 1, 1, 1, 3, 5, 4, 0, 0, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 0, 0, 3, 0, 4, Block.Cobblestone.BlockID, Block.Cobblestone.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 1, 0, 1, 2, 0, 3, Block.Dirt.BlockID, Block.Dirt.BlockID, false);

			if (IsTallHouse)
			{
				FillWithBlocks(par1World, par3StructureBoundingBox, 1, 4, 1, 2, 4, 3, Block.Wood.BlockID, Block.Wood.BlockID, false);
			}
			else
			{
				FillWithBlocks(par1World, par3StructureBoundingBox, 1, 5, 1, 2, 5, 3, Block.Wood.BlockID, Block.Wood.BlockID, false);
			}

			PlaceBlockAtCurrentPosition(par1World, Block.Wood.BlockID, 0, 1, 4, 0, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Wood.BlockID, 0, 2, 4, 0, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Wood.BlockID, 0, 1, 4, 4, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Wood.BlockID, 0, 2, 4, 4, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Wood.BlockID, 0, 0, 4, 1, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Wood.BlockID, 0, 0, 4, 2, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Wood.BlockID, 0, 0, 4, 3, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Wood.BlockID, 0, 3, 4, 1, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Wood.BlockID, 0, 3, 4, 2, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Wood.BlockID, 0, 3, 4, 3, par3StructureBoundingBox);
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 1, 0, 0, 3, 0, Block.Wood.BlockID, Block.Wood.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 3, 1, 0, 3, 3, 0, Block.Wood.BlockID, Block.Wood.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 1, 4, 0, 3, 4, Block.Wood.BlockID, Block.Wood.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 3, 1, 4, 3, 3, 4, Block.Wood.BlockID, Block.Wood.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 1, 1, 0, 3, 3, Block.Planks.BlockID, Block.Planks.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 3, 1, 1, 3, 3, 3, Block.Planks.BlockID, Block.Planks.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 1, 1, 0, 2, 3, 0, Block.Planks.BlockID, Block.Planks.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 1, 1, 4, 2, 3, 4, Block.Planks.BlockID, Block.Planks.BlockID, false);
			PlaceBlockAtCurrentPosition(par1World, Block.ThinGlass.BlockID, 0, 0, 2, 2, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.ThinGlass.BlockID, 0, 3, 2, 2, par3StructureBoundingBox);

			if (TablePosition > 0)
			{
				PlaceBlockAtCurrentPosition(par1World, Block.Fence.BlockID, 0, TablePosition, 1, 3, par3StructureBoundingBox);
				PlaceBlockAtCurrentPosition(par1World, Block.PressurePlatePlanks.BlockID, 0, TablePosition, 2, 3, par3StructureBoundingBox);
			}

			PlaceBlockAtCurrentPosition(par1World, 0, 0, 1, 1, 0, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, 0, 0, 1, 2, 0, par3StructureBoundingBox);
			PlaceDoorAtCurrentPosition(par1World, par3StructureBoundingBox, par2Random, 1, 1, 0, GetMetadataWithOffset(Block.DoorWood.BlockID, 1));

			if (GetBlockIdAtCurrentPosition(par1World, 1, 0, -1, par3StructureBoundingBox) == 0 && GetBlockIdAtCurrentPosition(par1World, 1, -1, -1, par3StructureBoundingBox) != 0)
			{
				PlaceBlockAtCurrentPosition(par1World, Block.StairCompactCobblestone.BlockID, GetMetadataWithOffset(Block.StairCompactCobblestone.BlockID, 3), 1, 0, -1, par3StructureBoundingBox);
			}

			for (int i = 0; i < 5; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					ClearCurrentPositionBlocksUpwards(par1World, j, 6, i, par3StructureBoundingBox);
					FillCurrentPositionBlocksDownwards(par1World, Block.Cobblestone.BlockID, 0, j, -1, i, par3StructureBoundingBox);
				}
			}

			SpawnVillagers(par1World, par3StructureBoundingBox, 1, 1, 2, 1);
			return true;
		}
	}

}