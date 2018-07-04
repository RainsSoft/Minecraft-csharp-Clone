using System;

namespace net.minecraft.src
{
	public class BlockReed : Block
	{
        public BlockReed(int par1, int par2)
            : base(par1, Material.Plants)
		{
			BlockIndexInTexture = par2;
			float f = 0.375F;
			SetBlockBounds(0.5F - f, 0.0F, 0.5F - f, 0.5F + f, 1.0F, 0.5F + f);
			SetTickRandomly(true);
		}

		/// <summary>
		/// Ticks the block if it's been scheduled
		/// </summary>
		public override void UpdateTick(World par1World, int par2, int par3, int par4, Random par5Random)
		{
			if (par1World.IsAirBlock(par2, par3 + 1, par4))
			{
				int i;

				for (i = 1; par1World.GetBlockId(par2, par3 - i, par4) == BlockID; i++)
				{
				}

				if (i < 3)
				{
					int j = par1World.GetBlockMetadata(par2, par3, par4);

					if (j == 15)
					{
						par1World.SetBlockWithNotify(par2, par3 + 1, par4, BlockID);
						par1World.SetBlockMetadataWithNotify(par2, par3, par4, 0);
					}
					else
					{
						par1World.SetBlockMetadataWithNotify(par2, par3, par4, j + 1);
					}
				}
			}
		}

		/// <summary>
		/// Checks to see if its valid to put this block at the specified coordinates. Args: world, x, y, z
		/// </summary>
		public override bool CanPlaceBlockAt(World par1World, int par2, int par3, int par4)
		{
			int i = par1World.GetBlockId(par2, par3 - 1, par4);

			if (i == BlockID)
			{
				return true;
			}

			if (i != Block.Grass.BlockID && i != Block.Dirt.BlockID && i != Block.Sand.BlockID)
			{
				return false;
			}

			if (par1World.GetBlockMaterial(par2 - 1, par3 - 1, par4) == Material.Water)
			{
				return true;
			}

			if (par1World.GetBlockMaterial(par2 + 1, par3 - 1, par4) == Material.Water)
			{
				return true;
			}

			if (par1World.GetBlockMaterial(par2, par3 - 1, par4 - 1) == Material.Water)
			{
				return true;
			}

			return par1World.GetBlockMaterial(par2, par3 - 1, par4 + 1) == Material.Water;
		}

		/// <summary>
		/// Lets the block know when one of its neighbor changes. Doesn't know which neighbor changed (coordinates passed are
		/// their own) Args: x, y, z, neighbor BlockID
		/// </summary>
		public override void OnNeighborBlockChange(World par1World, int par2, int par3, int par4, int par5)
		{
			CheckBlockCoordValid(par1World, par2, par3, par4);
		}

		/// <summary>
		/// Checks if current block pos is valid, if not, breaks the block as dropable item. Used for reed and cactus.
		/// </summary>
		protected void CheckBlockCoordValid(World par1World, int par2, int par3, int par4)
		{
			if (!CanBlockStay(par1World, par2, par3, par4))
			{
				DropBlockAsItem(par1World, par2, par3, par4, par1World.GetBlockMetadata(par2, par3, par4), 0);
				par1World.SetBlockWithNotify(par2, par3, par4, 0);
			}
		}

		/// <summary>
		/// Can this block stay at this position.  Similar to CanPlaceBlockAt except gets checked often with plants.
		/// </summary>
		public override bool CanBlockStay(World par1World, int par2, int par3, int par4)
		{
			return CanPlaceBlockAt(par1World, par2, par3, par4);
		}

		/// <summary>
		/// Returns a bounding box from the pool of bounding boxes (this means this box can change after the pool has been
		/// cleared to be reused)
		/// </summary>
		public override AxisAlignedBB GetCollisionBoundingBoxFromPool(World par1World, int par2, int par3, int i)
		{
			return null;
		}

		/// <summary>
		/// Returns the ID of the items to drop on destruction.
		/// </summary>
		public override int IdDropped(int par1, Random par2Random, int par3)
		{
			return Item.Reed.ShiftedIndex;
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
		/// The type of render function that is called for this block
		/// </summary>
		public override int GetRenderType()
		{
			return 1;
		}
	}
}