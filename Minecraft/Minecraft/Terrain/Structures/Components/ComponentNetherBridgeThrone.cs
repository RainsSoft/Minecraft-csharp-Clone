using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class ComponentNetherBridgeThrone : ComponentNetherBridgePiece
	{
		private bool HasSpawner;

		public ComponentNetherBridgeThrone(int par1, Random par2Random, StructureBoundingBox par3StructureBoundingBox, int par4) : base(par1)
		{
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
		/// Creates and returns a new component piece. Or null if it could not find enough room to place it.
		/// </summary>
        public static ComponentNetherBridgeThrone CreateValidComponent(List<StructureComponent> par0List, Random par1Random, int par2, int par3, int par4, int par5, int par6)
		{
			StructureBoundingBox structureboundingbox = StructureBoundingBox.GetComponentToAddBoundingBox(par2, par3, par4, -2, 0, 0, 7, 8, 9, par5);

			if (!IsAboveGround(structureboundingbox) || StructureComponent.FindIntersecting(par0List, structureboundingbox) != null)
			{
				return null;
			}
			else
			{
				return new ComponentNetherBridgeThrone(par6, par1Random, structureboundingbox, par5);
			}
		}

		/// <summary>
		/// 'second Part of Structure generating, this for example places Spiderwebs, Mob Spawners, it closes Mineshafts at
		/// the end, it adds Fences...'
		/// </summary>
		public override bool AddComponentParts(World par1World, Random par2Random, StructureBoundingBox par3StructureBoundingBox)
		{
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 2, 0, 6, 7, 7, 0, 0, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 1, 0, 0, 5, 1, 7, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 1, 2, 1, 5, 2, 7, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 1, 3, 2, 5, 3, 7, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 1, 4, 3, 5, 4, 7, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 1, 2, 0, 1, 4, 2, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 5, 2, 0, 5, 4, 2, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 1, 5, 2, 1, 5, 3, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 5, 5, 2, 5, 5, 3, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 5, 3, 0, 5, 8, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 6, 5, 3, 6, 5, 8, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 1, 5, 8, 5, 5, 8, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			PlaceBlockAtCurrentPosition(par1World, Block.NetherFence.BlockID, 0, 1, 6, 3, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.NetherFence.BlockID, 0, 5, 6, 3, par3StructureBoundingBox);
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 6, 3, 0, 6, 8, Block.NetherFence.BlockID, Block.NetherFence.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 6, 6, 3, 6, 6, 8, Block.NetherFence.BlockID, Block.NetherFence.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 1, 6, 8, 5, 7, 8, Block.NetherFence.BlockID, Block.NetherFence.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 2, 8, 8, 4, 8, 8, Block.NetherFence.BlockID, Block.NetherFence.BlockID, false);

			if (!HasSpawner)
			{
				int i = GetYWithOffset(5);
				int k = GetXWithOffset(3, 5);
				int i1 = GetZWithOffset(3, 5);

				if (par3StructureBoundingBox.IsVecInside(k, i, i1))
				{
					HasSpawner = true;
					par1World.SetBlockWithNotify(k, i, i1, Block.MobSpawner.BlockID);
					TileEntityMobSpawner tileentitymobspawner = (TileEntityMobSpawner)par1World.GetBlockTileEntity(k, i, i1);

					if (tileentitymobspawner != null)
					{
						tileentitymobspawner.SetMobID("Blaze");
					}
				}
			}

			for (int j = 0; j <= 6; j++)
			{
				for (int l = 0; l <= 6; l++)
				{
					FillCurrentPositionBlocksDownwards(par1World, Block.NetherBrick.BlockID, 0, j, -1, l, par3StructureBoundingBox);
				}
			}

			return true;
		}
	}
}