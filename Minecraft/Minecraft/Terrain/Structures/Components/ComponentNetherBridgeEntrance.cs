using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class ComponentNetherBridgeEntrance : ComponentNetherBridgePiece
	{
		public ComponentNetherBridgeEntrance(int par1, Random par2Random, StructureBoundingBox par3StructureBoundingBox, int par4) : base(par1)
		{
			CoordBaseMode = par4;
			BoundingBox = par3StructureBoundingBox;
		}

		/// <summary>
		/// 'Initiates construction of the Structure Component picked, at the current Location of StructGen'
		/// </summary>
        public override void BuildComponent(StructureComponent par1StructureComponent, List<StructureComponent> par2List, Random par3Random)
		{
			GetNextComponentNormal((ComponentNetherBridgeStartPiece)par1StructureComponent, par2List, par3Random, 5, 3, true);
		}

		/// <summary>
		/// Creates and returns a new component piece. Or null if it could not find enough room to place it.
		/// </summary>
        public static ComponentNetherBridgeEntrance CreateValidComponent(List<StructureComponent> par0List, Random par1Random, int par2, int par3, int par4, int par5, int par6)
		{
			StructureBoundingBox structureboundingbox = StructureBoundingBox.GetComponentToAddBoundingBox(par2, par3, par4, -5, -3, 0, 13, 14, 13, par5);

			if (!IsAboveGround(structureboundingbox) || StructureComponent.FindIntersecting(par0List, structureboundingbox) != null)
			{
				return null;
			}
			else
			{
				return new ComponentNetherBridgeEntrance(par6, par1Random, structureboundingbox, par5);
			}
		}

		/// <summary>
		/// 'second Part of Structure generating, this for example places Spiderwebs, Mob Spawners, it closes Mineshafts at
		/// the end, it adds Fences...'
		/// </summary>
		public override bool AddComponentParts(World par1World, Random par2Random, StructureBoundingBox par3StructureBoundingBox)
		{
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 3, 0, 12, 4, 12, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 5, 0, 12, 13, 12, 0, 0, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 5, 0, 1, 12, 12, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 11, 5, 0, 12, 12, 12, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 2, 5, 11, 4, 12, 12, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 8, 5, 11, 10, 12, 12, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 5, 9, 11, 7, 12, 12, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 2, 5, 0, 4, 12, 1, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 8, 5, 0, 10, 12, 1, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 5, 9, 0, 7, 12, 1, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 2, 11, 2, 10, 12, 10, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 5, 8, 0, 7, 8, 0, Block.NetherFence.BlockID, Block.NetherFence.BlockID, false);

			for (int i = 1; i <= 11; i += 2)
			{
				FillWithBlocks(par1World, par3StructureBoundingBox, i, 10, 0, i, 11, 0, Block.NetherFence.BlockID, Block.NetherFence.BlockID, false);
				FillWithBlocks(par1World, par3StructureBoundingBox, i, 10, 12, i, 11, 12, Block.NetherFence.BlockID, Block.NetherFence.BlockID, false);
				FillWithBlocks(par1World, par3StructureBoundingBox, 0, 10, i, 0, 11, i, Block.NetherFence.BlockID, Block.NetherFence.BlockID, false);
				FillWithBlocks(par1World, par3StructureBoundingBox, 12, 10, i, 12, 11, i, Block.NetherFence.BlockID, Block.NetherFence.BlockID, false);
				PlaceBlockAtCurrentPosition(par1World, Block.NetherBrick.BlockID, 0, i, 13, 0, par3StructureBoundingBox);
				PlaceBlockAtCurrentPosition(par1World, Block.NetherBrick.BlockID, 0, i, 13, 12, par3StructureBoundingBox);
				PlaceBlockAtCurrentPosition(par1World, Block.NetherBrick.BlockID, 0, 0, 13, i, par3StructureBoundingBox);
				PlaceBlockAtCurrentPosition(par1World, Block.NetherBrick.BlockID, 0, 12, 13, i, par3StructureBoundingBox);
				PlaceBlockAtCurrentPosition(par1World, Block.NetherFence.BlockID, 0, i + 1, 13, 0, par3StructureBoundingBox);
				PlaceBlockAtCurrentPosition(par1World, Block.NetherFence.BlockID, 0, i + 1, 13, 12, par3StructureBoundingBox);
				PlaceBlockAtCurrentPosition(par1World, Block.NetherFence.BlockID, 0, 0, 13, i + 1, par3StructureBoundingBox);
				PlaceBlockAtCurrentPosition(par1World, Block.NetherFence.BlockID, 0, 12, 13, i + 1, par3StructureBoundingBox);
			}

			PlaceBlockAtCurrentPosition(par1World, Block.NetherFence.BlockID, 0, 0, 13, 0, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.NetherFence.BlockID, 0, 0, 13, 12, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.NetherFence.BlockID, 0, 0, 13, 0, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.NetherFence.BlockID, 0, 12, 13, 0, par3StructureBoundingBox);

			for (int j = 3; j <= 9; j += 2)
			{
				FillWithBlocks(par1World, par3StructureBoundingBox, 1, 7, j, 1, 8, j, Block.NetherFence.BlockID, Block.NetherFence.BlockID, false);
				FillWithBlocks(par1World, par3StructureBoundingBox, 11, 7, j, 11, 8, j, Block.NetherFence.BlockID, Block.NetherFence.BlockID, false);
			}

			FillWithBlocks(par1World, par3StructureBoundingBox, 4, 2, 0, 8, 2, 12, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 2, 4, 12, 2, 8, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 4, 0, 0, 8, 1, 3, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 4, 0, 9, 8, 1, 12, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 0, 4, 3, 1, 8, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 9, 0, 4, 12, 1, 8, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);

			for (int k = 4; k <= 8; k++)
			{
				for (int j1 = 0; j1 <= 2; j1++)
				{
					FillCurrentPositionBlocksDownwards(par1World, Block.NetherBrick.BlockID, 0, k, -1, j1, par3StructureBoundingBox);
					FillCurrentPositionBlocksDownwards(par1World, Block.NetherBrick.BlockID, 0, k, -1, 12 - j1, par3StructureBoundingBox);
				}
			}

			for (int l = 0; l <= 2; l++)
			{
				for (int k1 = 4; k1 <= 8; k1++)
				{
					FillCurrentPositionBlocksDownwards(par1World, Block.NetherBrick.BlockID, 0, l, -1, k1, par3StructureBoundingBox);
					FillCurrentPositionBlocksDownwards(par1World, Block.NetherBrick.BlockID, 0, 12 - l, -1, k1, par3StructureBoundingBox);
				}
			}

			FillWithBlocks(par1World, par3StructureBoundingBox, 5, 5, 5, 7, 5, 7, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 6, 1, 6, 6, 4, 6, 0, 0, false);
			PlaceBlockAtCurrentPosition(par1World, Block.NetherBrick.BlockID, 0, 6, 0, 6, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.LavaMoving.BlockID, 0, 6, 5, 6, par3StructureBoundingBox);
			int i1 = GetXWithOffset(6, 6);
			int l1 = GetYWithOffset(5);
			int i2 = GetZWithOffset(6, 6);

			if (par3StructureBoundingBox.IsVecInside(i1, l1, i2))
			{
				par1World.ScheduledUpdatesAreImmediate = true;
				Block.BlocksList[Block.LavaMoving.BlockID].UpdateTick(par1World, i1, l1, i2, par2Random);
				par1World.ScheduledUpdatesAreImmediate = false;
			}

			return true;
		}
	}
}