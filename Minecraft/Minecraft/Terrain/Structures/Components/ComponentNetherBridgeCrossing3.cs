using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class ComponentNetherBridgeCrossing3 : ComponentNetherBridgePiece
	{
		public ComponentNetherBridgeCrossing3(int par1, Random par2Random, StructureBoundingBox par3StructureBoundingBox, int par4) : base(par1)
		{
			CoordBaseMode = par4;
			BoundingBox = par3StructureBoundingBox;
		}

		protected ComponentNetherBridgeCrossing3(Random par1Random, int par2, int par3) : base(0)
		{
			CoordBaseMode = par1Random.Next(4);

			switch (CoordBaseMode)
			{
				case 0:
				case 2:
					BoundingBox = new StructureBoundingBox(par2, 64, par3, (par2 + 19) - 1, 73, (par3 + 19) - 1);
					break;

				default:
					BoundingBox = new StructureBoundingBox(par2, 64, par3, (par2 + 19) - 1, 73, (par3 + 19) - 1);
					break;
			}
		}

		/// <summary>
		/// 'Initiates construction of the Structure Component picked, at the current Location of StructGen'
		/// </summary>
        public override void BuildComponent(StructureComponent par1StructureComponent, List<StructureComponent> par2List, Random par3Random)
		{
			GetNextComponentNormal((ComponentNetherBridgeStartPiece)par1StructureComponent, par2List, par3Random, 8, 3, false);
			GetNextComponentX((ComponentNetherBridgeStartPiece)par1StructureComponent, par2List, par3Random, 3, 8, false);
			GetNextComponentZ((ComponentNetherBridgeStartPiece)par1StructureComponent, par2List, par3Random, 3, 8, false);
		}

		/// <summary>
		/// Creates and returns a new component piece. Or null if it could not find enough room to place it.
		/// </summary>
        public static ComponentNetherBridgeCrossing3 CreateValidComponent(List<StructureComponent> par0List, Random par1Random, int par2, int par3, int par4, int par5, int par6)
		{
			StructureBoundingBox structureboundingbox = StructureBoundingBox.GetComponentToAddBoundingBox(par2, par3, par4, -8, -3, 0, 19, 10, 19, par5);

			if (!IsAboveGround(structureboundingbox) || StructureComponent.FindIntersecting(par0List, structureboundingbox) != null)
			{
				return null;
			}
			else
			{
				return new ComponentNetherBridgeCrossing3(par6, par1Random, structureboundingbox, par5);
			}
		}

		/// <summary>
		/// 'second Part of Structure generating, this for example places Spiderwebs, Mob Spawners, it closes Mineshafts at
		/// the end, it adds Fences...'
		/// </summary>
		public override bool AddComponentParts(World par1World, Random par2Random, StructureBoundingBox par3StructureBoundingBox)
		{
			FillWithBlocks(par1World, par3StructureBoundingBox, 7, 3, 0, 11, 4, 18, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 3, 7, 18, 4, 11, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 8, 5, 0, 10, 7, 18, 0, 0, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 5, 8, 18, 7, 10, 0, 0, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 7, 5, 0, 7, 5, 7, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 7, 5, 11, 7, 5, 18, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 11, 5, 0, 11, 5, 7, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 11, 5, 11, 11, 5, 18, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 5, 7, 7, 5, 7, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 11, 5, 7, 18, 5, 7, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 5, 11, 7, 5, 11, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 11, 5, 11, 18, 5, 11, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 7, 2, 0, 11, 2, 5, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 7, 2, 13, 11, 2, 18, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 7, 0, 0, 11, 1, 3, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 7, 0, 15, 11, 1, 18, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);

			for (int i = 7; i <= 11; i++)
			{
				for (int k = 0; k <= 2; k++)
				{
					FillCurrentPositionBlocksDownwards(par1World, Block.NetherBrick.BlockID, 0, i, -1, k, par3StructureBoundingBox);
					FillCurrentPositionBlocksDownwards(par1World, Block.NetherBrick.BlockID, 0, i, -1, 18 - k, par3StructureBoundingBox);
				}
			}

			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 2, 7, 5, 2, 11, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 13, 2, 7, 18, 2, 11, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 0, 7, 3, 1, 11, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 15, 0, 7, 18, 1, 11, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);

			for (int j = 0; j <= 2; j++)
			{
				for (int l = 7; l <= 11; l++)
				{
					FillCurrentPositionBlocksDownwards(par1World, Block.NetherBrick.BlockID, 0, j, -1, l, par3StructureBoundingBox);
					FillCurrentPositionBlocksDownwards(par1World, Block.NetherBrick.BlockID, 0, 18 - j, -1, l, par3StructureBoundingBox);
				}
			}

			return true;
		}
	}
}