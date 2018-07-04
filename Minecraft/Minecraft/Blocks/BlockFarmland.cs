using System;

namespace net.minecraft.src
{
	public class BlockFarmland : Block
	{
        public BlockFarmland(int par1)
            : base(par1, Material.Ground)
		{
			BlockIndexInTexture = 87;
			SetTickRandomly(true);
			SetBlockBounds(0.0F, 0.0F, 0.0F, 1.0F, 0.9375F, 1.0F);
			SetLightOpacity(255);
		}

		/// <summary>
		/// Returns a bounding box from the pool of bounding boxes (this means this box can change after the pool has been
		/// cleared to be reused)
		/// </summary>
		public override AxisAlignedBB GetCollisionBoundingBoxFromPool(World par1World, int par2, int par3, int par4)
		{
			return AxisAlignedBB.GetBoundingBoxFromPool(par2 + 0, par3 + 0, par4 + 0, par2 + 1, par3 + 1, par4 + 1);
		}

		/// <summary>
		/// Is this block (a) opaque and (b) a full 1m cube?  This determines whether or not to render the shared face of two
		/// adjacent blocks and also whether the player can attach torches, redstone wire, etc to this block.
		/// </summary>
		public override bool IsOpaqueCube()
		{
			return false;
		}

		/// <summary>
		/// If this block doesn't render as an ordinary block it will return False (examples: signs, buttons, stairs, etc)
		/// </summary>
		public override bool RenderAsNormalBlock()
		{
			return false;
		}

		/// <summary>
		/// From the specified side and block metadata retrieves the blocks texture. Args: side, metadata
		/// </summary>
		public override int GetBlockTextureFromSideAndMetadata(int par1, int par2)
		{
			if (par1 == 1 && par2 > 0)
			{
				return BlockIndexInTexture - 1;
			}

			if (par1 == 1)
			{
				return BlockIndexInTexture;
			}
			else
			{
				return 2;
			}
		}

		/// <summary>
		/// Ticks the block if it's been scheduled
		/// </summary>
		public override void UpdateTick(World par1World, int par2, int par3, int par4, Random par5Random)
		{
			if (IsWaterNearby(par1World, par2, par3, par4) || par1World.CanLightningStrikeAt(par2, par3 + 1, par4))
			{
				par1World.SetBlockMetadataWithNotify(par2, par3, par4, 7);
			}
			else
			{
				int i = par1World.GetBlockMetadata(par2, par3, par4);

				if (i > 0)
				{
					par1World.SetBlockMetadataWithNotify(par2, par3, par4, i - 1);
				}
				else if (!IsCropsNearby(par1World, par2, par3, par4))
				{
					par1World.SetBlockWithNotify(par2, par3, par4, Block.Dirt.BlockID);
				}
			}
		}

		/// <summary>
		/// Block's chance to react to an entity falling on it.
		/// </summary>
		public override void OnFallenUpon(World par1World, int par2, int par3, int par4, Entity par5Entity, float par6)
		{
			if (par1World.Rand.NextFloat() < par6 - 0.5F)
			{
				par1World.SetBlockWithNotify(par2, par3, par4, Block.Dirt.BlockID);
			}
		}

		/// <summary>
		/// returns true if there is at least one cropblock nearby (x-1 to x+1, y+1, z-1 to z+1)
		/// </summary>
		private bool IsCropsNearby(World par1World, int par2, int par3, int par4)
		{
			int i = 0;

			for (int j = par2 - i; j <= par2 + i; j++)
			{
				for (int k = par4 - i; k <= par4 + i; k++)
				{
					int l = par1World.GetBlockId(j, par3 + 1, k);

					if (l == Block.Crops.BlockID || l == Block.MelonStem.BlockID || l == Block.PumpkinStem.BlockID)
					{
						return true;
					}
				}
			}

			return false;
		}

		/// <summary>
		/// returns true if there's water nearby (x-4 to x+4, y to y+1, k-4 to k+4)
		/// </summary>
		private bool IsWaterNearby(World par1World, int par2, int par3, int par4)
		{
			for (int i = par2 - 4; i <= par2 + 4; i++)
			{
				for (int j = par3; j <= par3 + 1; j++)
				{
					for (int k = par4 - 4; k <= par4 + 4; k++)
					{
						if (par1World.GetBlockMaterial(i, j, k) == Material.Water)
						{
							return true;
						}
					}
				}
			}

			return false;
		}

		/// <summary>
		/// Lets the block know when one of its neighbor changes. Doesn't know which neighbor changed (coordinates passed are
		/// their own) Args: x, y, z, neighbor BlockID
		/// </summary>
		public override void OnNeighborBlockChange(World par1World, int par2, int par3, int par4, int par5)
		{
			base.OnNeighborBlockChange(par1World, par2, par3, par4, par5);
			Material material = par1World.GetBlockMaterial(par2, par3 + 1, par4);

			if (material.IsSolid())
			{
				par1World.SetBlockWithNotify(par2, par3, par4, Block.Dirt.BlockID);
			}
		}

		/// <summary>
		/// Returns the ID of the items to drop on destruction.
		/// </summary>
		public override int IdDropped(int par1, Random par2Random, int par3)
		{
			return Block.Dirt.IdDropped(0, par2Random, par3);
		}
	}
}