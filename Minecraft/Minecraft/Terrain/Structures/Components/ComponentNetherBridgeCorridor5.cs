using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class ComponentNetherBridgeCorridor5 : ComponentNetherBridgePiece
	{
		public ComponentNetherBridgeCorridor5(int par1, Random par2Random, StructureBoundingBox par3StructureBoundingBox, int par4) : base(par1)
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
        public static ComponentNetherBridgeCorridor5 CreateValidComponent(List<StructureComponent> par0List, Random par1Random, int par2, int par3, int par4, int par5, int par6)
		{
			StructureBoundingBox structureboundingbox = StructureBoundingBox.GetComponentToAddBoundingBox(par2, par3, par4, -1, 0, 0, 5, 7, 5, par5);

			if (!IsAboveGround(structureboundingbox) || StructureComponent.FindIntersecting(par0List, structureboundingbox) != null)
			{
				return null;
			}
			else
			{
				return new ComponentNetherBridgeCorridor5(par6, par1Random, structureboundingbox, par5);
			}
		}

		/// <summary>
		/// 'second Part of Structure generating, this for example places Spiderwebs, Mob Spawners, it closes Mineshafts at
		/// the end, it adds Fences...'
		/// </summary>
		public override bool AddComponentParts(World par1World, Random par2Random, StructureBoundingBox par3StructureBoundingBox)
		{
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 0, 0, 4, 1, 4, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 2, 0, 4, 5, 4, 0, 0, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 2, 0, 0, 5, 4, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 4, 2, 0, 4, 5, 4, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 3, 1, 0, 4, 1, Block.NetherFence.BlockID, Block.NetherFence.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 3, 3, 0, 4, 3, Block.NetherFence.BlockID, Block.NetherFence.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 4, 3, 1, 4, 4, 1, Block.NetherFence.BlockID, Block.NetherFence.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 4, 3, 3, 4, 4, 3, Block.NetherFence.BlockID, Block.NetherFence.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 6, 0, 4, 6, 4, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);

			for (int i = 0; i <= 4; i++)
			{
				for (int j = 0; j <= 4; j++)
				{
					FillCurrentPositionBlocksDownwards(par1World, Block.NetherBrick.BlockID, 0, i, -1, j, par3StructureBoundingBox);
				}
			}

			return true;
		}
	}
}