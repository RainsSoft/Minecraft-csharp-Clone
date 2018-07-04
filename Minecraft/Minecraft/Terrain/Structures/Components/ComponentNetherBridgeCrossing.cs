using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class ComponentNetherBridgeCrossing : ComponentNetherBridgePiece
	{
		public ComponentNetherBridgeCrossing(int par1, Random par2Random, StructureBoundingBox par3StructureBoundingBox, int par4) : base(par1)
		{
			CoordBaseMode = par4;
			BoundingBox = par3StructureBoundingBox;
		}

		/// <summary>
		/// 'Initiates construction of the Structure Component picked, at the current Location of StructGen'
		/// </summary>
        public override void BuildComponent(StructureComponent par1StructureComponent, List<StructureComponent> par2List, Random par3Random)
		{
			GetNextComponentNormal((ComponentNetherBridgeStartPiece)par1StructureComponent, par2List, par3Random, 2, 0, false);
			GetNextComponentX((ComponentNetherBridgeStartPiece)par1StructureComponent, par2List, par3Random, 0, 2, false);
			GetNextComponentZ((ComponentNetherBridgeStartPiece)par1StructureComponent, par2List, par3Random, 0, 2, false);
		}

		/// <summary>
		/// Creates and returns a new component piece. Or null if it could not find enough room to place it.
		/// </summary>
        public static ComponentNetherBridgeCrossing CreateValidComponent(List<StructureComponent> par0List, Random par1Random, int par2, int par3, int par4, int par5, int par6)
		{
			StructureBoundingBox structureboundingbox = StructureBoundingBox.GetComponentToAddBoundingBox(par2, par3, par4, -2, 0, 0, 7, 9, 7, par5);

			if (!IsAboveGround(structureboundingbox) || StructureComponent.FindIntersecting(par0List, structureboundingbox) != null)
			{
				return null;
			}
			else
			{
				return new ComponentNetherBridgeCrossing(par6, par1Random, structureboundingbox, par5);
			}
		}

		/// <summary>
		/// 'second Part of Structure generating, this for example places Spiderwebs, Mob Spawners, it closes Mineshafts at
		/// the end, it adds Fences...'
		/// </summary>
		public override bool AddComponentParts(World par1World, Random par2Random, StructureBoundingBox par3StructureBoundingBox)
		{
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 0, 0, 6, 1, 6, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 2, 0, 6, 7, 6, 0, 0, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 2, 0, 1, 6, 0, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 2, 6, 1, 6, 6, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 5, 2, 0, 6, 6, 0, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 5, 2, 6, 6, 6, 6, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 2, 0, 0, 6, 1, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 2, 5, 0, 6, 6, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 6, 2, 0, 6, 6, 1, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 6, 2, 5, 6, 6, 6, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 2, 6, 0, 4, 6, 0, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 2, 5, 0, 4, 5, 0, Block.NetherFence.BlockID, Block.NetherFence.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 2, 6, 6, 4, 6, 6, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 2, 5, 6, 4, 5, 6, Block.NetherFence.BlockID, Block.NetherFence.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 6, 2, 0, 6, 4, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 5, 2, 0, 5, 4, Block.NetherFence.BlockID, Block.NetherFence.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 6, 6, 2, 6, 6, 4, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 6, 5, 2, 6, 5, 4, Block.NetherFence.BlockID, Block.NetherFence.BlockID, false);

			for (int i = 0; i <= 6; i++)
			{
				for (int j = 0; j <= 6; j++)
				{
					FillCurrentPositionBlocksDownwards(par1World, Block.NetherBrick.BlockID, 0, i, -1, j, par3StructureBoundingBox);
				}
			}

			return true;
		}
	}
}