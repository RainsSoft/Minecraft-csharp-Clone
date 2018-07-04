using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class ComponentVillageWell : ComponentVillage
	{
		private readonly bool Field_35104_a = true;
		private int AverageGroundLevel;

		public ComponentVillageWell(int par1, Random par2Random, int par3, int par4) : base(par1)
		{
			AverageGroundLevel = -1;
			CoordBaseMode = par2Random.Next(4);

			switch (CoordBaseMode)
			{
				case 0:
				case 2:
					BoundingBox = new StructureBoundingBox(par3, 64, par4, (par3 + 6) - 1, 78, (par4 + 6) - 1);
					break;

				default:
					BoundingBox = new StructureBoundingBox(par3, 64, par4, (par3 + 6) - 1, 78, (par4 + 6) - 1);
					break;
			}
		}

		/// <summary>
		/// 'Initiates construction of the Structure Component picked, at the current Location of StructGen'
		/// </summary>
        public override void BuildComponent(StructureComponent par1StructureComponent, List<StructureComponent> par2List, Random par3Random)
		{
			StructureVillagePieces.GetNextStructureComponentVillagePath((ComponentVillageStartPiece)par1StructureComponent, par2List, par3Random, BoundingBox.MinX - 1, BoundingBox.MaxY - 4, BoundingBox.MinZ + 1, 1, GetComponentType());
			StructureVillagePieces.GetNextStructureComponentVillagePath((ComponentVillageStartPiece)par1StructureComponent, par2List, par3Random, BoundingBox.MaxX + 1, BoundingBox.MaxY - 4, BoundingBox.MinZ + 1, 3, GetComponentType());
			StructureVillagePieces.GetNextStructureComponentVillagePath((ComponentVillageStartPiece)par1StructureComponent, par2List, par3Random, BoundingBox.MinX + 1, BoundingBox.MaxY - 4, BoundingBox.MinZ - 1, 2, GetComponentType());
			StructureVillagePieces.GetNextStructureComponentVillagePath((ComponentVillageStartPiece)par1StructureComponent, par2List, par3Random, BoundingBox.MinX + 1, BoundingBox.MaxY - 4, BoundingBox.MaxZ + 1, 0, GetComponentType());
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

				BoundingBox.Offset(0, (AverageGroundLevel - BoundingBox.MaxY) + 3, 0);
			}

			if (!Field_35104_a)
			{
				;
			}

			FillWithBlocks(par1World, par3StructureBoundingBox, 1, 0, 1, 4, 12, 4, Block.Cobblestone.BlockID, Block.WaterMoving.BlockID, false);
			PlaceBlockAtCurrentPosition(par1World, 0, 0, 2, 12, 2, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, 0, 0, 3, 12, 2, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, 0, 0, 2, 12, 3, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, 0, 0, 3, 12, 3, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Fence.BlockID, 0, 1, 13, 1, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Fence.BlockID, 0, 1, 14, 1, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Fence.BlockID, 0, 4, 13, 1, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Fence.BlockID, 0, 4, 14, 1, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Fence.BlockID, 0, 1, 13, 4, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Fence.BlockID, 0, 1, 14, 4, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Fence.BlockID, 0, 4, 13, 4, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Fence.BlockID, 0, 4, 14, 4, par3StructureBoundingBox);
			FillWithBlocks(par1World, par3StructureBoundingBox, 1, 15, 1, 4, 15, 4, Block.Cobblestone.BlockID, Block.Cobblestone.BlockID, false);

			for (int i = 0; i <= 5; i++)
			{
				for (int j = 0; j <= 5; j++)
				{
					if (j == 0 || j == 5 || i == 0 || i == 5)
					{
						PlaceBlockAtCurrentPosition(par1World, Block.Gravel.BlockID, 0, j, 11, i, par3StructureBoundingBox);
						ClearCurrentPositionBlocksUpwards(par1World, j, 12, i, par3StructureBoundingBox);
					}
				}
			}

			return true;
		}
	}
}