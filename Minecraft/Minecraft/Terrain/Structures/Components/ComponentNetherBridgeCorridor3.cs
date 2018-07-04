using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class ComponentNetherBridgeCorridor3 : ComponentNetherBridgePiece
	{
		public ComponentNetherBridgeCorridor3(int par1, Random par2Random, StructureBoundingBox par3StructureBoundingBox, int par4) : base(par1)
		{
			CoordBaseMode = par4;
			BoundingBox = par3StructureBoundingBox;
		}

		/// <summary>
		/// 'Initiates construction of the Structure Component picked, at the current Location of StructGen'
		/// </summary>
        public override void BuildComponent(StructureComponent par1StructureComponent, List<StructureComponent> par2List, Random par3Random)
		{
			GetNextComponentNormal((ComponentNetherBridgeStartPiece)par1StructureComponent, par2List, par3Random, 1, 0, true);
		}

		/// <summary>
		/// Creates and returns a new component piece. Or null if it could not find enough room to place it.
		/// </summary>
        public static ComponentNetherBridgeCorridor3 CreateValidComponent(List<StructureComponent> par0List, Random par1Random, int par2, int par3, int par4, int par5, int par6)
		{
			StructureBoundingBox structureboundingbox = StructureBoundingBox.GetComponentToAddBoundingBox(par2, par3, par4, -1, -7, 0, 5, 14, 10, par5);

			if (!IsAboveGround(structureboundingbox) || StructureComponent.FindIntersecting(par0List, structureboundingbox) != null)
			{
				return null;
			}
			else
			{
				return new ComponentNetherBridgeCorridor3(par6, par1Random, structureboundingbox, par5);
			}
		}

		/// <summary>
		/// 'second Part of Structure generating, this for example places Spiderwebs, Mob Spawners, it closes Mineshafts at
		/// the end, it adds Fences...'
		/// </summary>
		public override bool AddComponentParts(World par1World, Random par2Random, StructureBoundingBox par3StructureBoundingBox)
		{
			int i = GetMetadataWithOffset(Block.StairsNetherBrick.BlockID, 2);

			for (int j = 0; j <= 9; j++)
			{
				int k = Math.Max(1, 7 - j);
				int l = Math.Min(Math.Max(k + 5, 14 - j), 13);
				int i1 = j;
				FillWithBlocks(par1World, par3StructureBoundingBox, 0, 0, i1, 4, k, i1, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
				FillWithBlocks(par1World, par3StructureBoundingBox, 1, k + 1, i1, 3, l - 1, i1, 0, 0, false);

				if (j <= 6)
				{
					PlaceBlockAtCurrentPosition(par1World, Block.StairsNetherBrick.BlockID, i, 1, k + 1, i1, par3StructureBoundingBox);
					PlaceBlockAtCurrentPosition(par1World, Block.StairsNetherBrick.BlockID, i, 2, k + 1, i1, par3StructureBoundingBox);
					PlaceBlockAtCurrentPosition(par1World, Block.StairsNetherBrick.BlockID, i, 3, k + 1, i1, par3StructureBoundingBox);
				}

				FillWithBlocks(par1World, par3StructureBoundingBox, 0, l, i1, 4, l, i1, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
				FillWithBlocks(par1World, par3StructureBoundingBox, 0, k + 1, i1, 0, l - 1, i1, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
				FillWithBlocks(par1World, par3StructureBoundingBox, 4, k + 1, i1, 4, l - 1, i1, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);

				if ((j & 1) == 0)
				{
					FillWithBlocks(par1World, par3StructureBoundingBox, 0, k + 2, i1, 0, k + 3, i1, Block.NetherFence.BlockID, Block.NetherFence.BlockID, false);
					FillWithBlocks(par1World, par3StructureBoundingBox, 4, k + 2, i1, 4, k + 3, i1, Block.NetherFence.BlockID, Block.NetherFence.BlockID, false);
				}

				for (int j1 = 0; j1 <= 4; j1++)
				{
					FillCurrentPositionBlocksDownwards(par1World, Block.NetherBrick.BlockID, 0, j1, -1, i1, par3StructureBoundingBox);
				}
			}

			return true;
		}
	}
}