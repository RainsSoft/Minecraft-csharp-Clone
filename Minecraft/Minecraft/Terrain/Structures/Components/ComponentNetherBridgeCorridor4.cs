using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class ComponentNetherBridgeCorridor4 : ComponentNetherBridgePiece
	{
		public ComponentNetherBridgeCorridor4(int par1, Random par2Random, StructureBoundingBox par3StructureBoundingBox, int par4) : base(par1)
		{
			CoordBaseMode = par4;
			BoundingBox = par3StructureBoundingBox;
		}

		/// <summary>
		/// 'Initiates construction of the Structure Component picked, at the current Location of StructGen'
		/// </summary>
        public override void BuildComponent(StructureComponent par1StructureComponent, List<StructureComponent> par2List, Random par3Random)
		{
			sbyte byte0 = 1;

			if (CoordBaseMode == 1 || CoordBaseMode == 2)
			{
				byte0 = 5;
			}

			GetNextComponentX((ComponentNetherBridgeStartPiece)par1StructureComponent, par2List, par3Random, 0, byte0, par3Random.Next(8) > 0);
			GetNextComponentZ((ComponentNetherBridgeStartPiece)par1StructureComponent, par2List, par3Random, 0, byte0, par3Random.Next(8) > 0);
		}

		/// <summary>
		/// Creates and returns a new component piece. Or null if it could not find enough room to place it.
		/// </summary>
        public static ComponentNetherBridgeCorridor4 CreateValidComponent(List<StructureComponent> par0List, Random par1Random, int par2, int par3, int par4, int par5, int par6)
		{
			StructureBoundingBox structureboundingbox = StructureBoundingBox.GetComponentToAddBoundingBox(par2, par3, par4, -3, 0, 0, 9, 7, 9, par5);

			if (!IsAboveGround(structureboundingbox) || StructureComponent.FindIntersecting(par0List, structureboundingbox) != null)
			{
				return null;
			}
			else
			{
				return new ComponentNetherBridgeCorridor4(par6, par1Random, structureboundingbox, par5);
			}
		}

		/// <summary>
		/// 'second Part of Structure generating, this for example places Spiderwebs, Mob Spawners, it closes Mineshafts at
		/// the end, it adds Fences...'
		/// </summary>
		public override bool AddComponentParts(World par1World, Random par2Random, StructureBoundingBox par3StructureBoundingBox)
		{
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 0, 0, 8, 1, 8, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 2, 0, 8, 5, 8, 0, 0, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 6, 0, 8, 6, 5, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 2, 0, 2, 5, 0, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 6, 2, 0, 8, 5, 0, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 1, 3, 0, 1, 4, 0, Block.NetherFence.BlockID, Block.NetherFence.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 7, 3, 0, 7, 4, 0, Block.NetherFence.BlockID, Block.NetherFence.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 2, 4, 8, 2, 8, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 1, 1, 4, 2, 2, 4, 0, 0, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 6, 1, 4, 7, 2, 4, 0, 0, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 3, 8, 8, 3, 8, Block.NetherFence.BlockID, Block.NetherFence.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 3, 6, 0, 3, 7, Block.NetherFence.BlockID, Block.NetherFence.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 8, 3, 6, 8, 3, 7, Block.NetherFence.BlockID, Block.NetherFence.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 3, 4, 0, 5, 5, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 8, 3, 4, 8, 5, 5, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 1, 3, 5, 2, 5, 5, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 6, 3, 5, 7, 5, 5, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 1, 4, 5, 1, 5, 5, Block.NetherFence.BlockID, Block.NetherFence.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 7, 4, 5, 7, 5, 5, Block.NetherFence.BlockID, Block.NetherFence.BlockID, false);

			for (int i = 0; i <= 5; i++)
			{
				for (int j = 0; j <= 8; j++)
				{
					FillCurrentPositionBlocksDownwards(par1World, Block.NetherBrick.BlockID, 0, j, -1, i, par3StructureBoundingBox);
				}
			}

			return true;
		}
	}
}