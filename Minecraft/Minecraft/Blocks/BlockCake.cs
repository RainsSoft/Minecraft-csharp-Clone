using System;

namespace net.minecraft.src
{
	public class BlockCake : Block
	{
        public BlockCake(int par1, int par2)
            : base(par1, par2, Material.Cake)
		{
			SetTickRandomly(true);
		}

		/// <summary>
		/// Updates the blocks bounds based on its current state. Args: world, x, y, z
		/// </summary>
		public override void SetBlockBoundsBasedOnState(IBlockAccess par1IBlockAccess, int par2, int par3, int par4)
		{
			int i = par1IBlockAccess.GetBlockMetadata(par2, par3, par4);
			float f = 0.0625F;
			float f1 = (float)(1 + i * 2) / 16F;
			float f2 = 0.5F;
			SetBlockBounds(f1, 0.0F, f, 1.0F - f, f2, 1.0F - f);
		}

		/// <summary>
		/// Sets the block's bounds for rendering it as an item
		/// </summary>
		public override void SetBlockBoundsForItemRender()
		{
			float f = 0.0625F;
			float f1 = 0.5F;
			SetBlockBounds(f, 0.0F, f, 1.0F - f, f1, 1.0F - f);
		}

		/// <summary>
		/// Returns a bounding box from the pool of bounding boxes (this means this box can change after the pool has been
		/// cleared to be reused)
		/// </summary>
		public override AxisAlignedBB GetCollisionBoundingBoxFromPool(World par1World, int par2, int par3, int par4)
		{
			int i = par1World.GetBlockMetadata(par2, par3, par4);
			float f = 0.0625F;
			float f1 = (float)(1 + i * 2) / 16F;
			float f2 = 0.5F;
			return AxisAlignedBB.GetBoundingBoxFromPool((float)par2 + f1, par3, (float)par4 + f, (float)(par2 + 1) - f, ((float)par3 + f2) - f, (float)(par4 + 1) - f);
		}

		/// <summary>
		/// Returns the bounding box of the wired rectangular prism to render.
		/// </summary>
		public override AxisAlignedBB GetSelectedBoundingBoxFromPool(World par1World, int par2, int par3, int par4)
		{
			int i = par1World.GetBlockMetadata(par2, par3, par4);
			float f = 0.0625F;
			float f1 = (float)(1 + i * 2) / 16F;
			float f2 = 0.5F;
			return AxisAlignedBB.GetBoundingBoxFromPool((float)par2 + f1, par3, (float)par4 + f, (float)(par2 + 1) - f, (float)par3 + f2, (float)(par4 + 1) - f);
		}

		/// <summary>
		/// From the specified side and block metadata retrieves the blocks texture. Args: side, metadata
		/// </summary>
		public override int GetBlockTextureFromSideAndMetadata(int par1, int par2)
		{
			if (par1 == 1)
			{
				return BlockIndexInTexture;
			}

			if (par1 == 0)
			{
				return BlockIndexInTexture + 3;
			}

			if (par2 > 0 && par1 == 4)
			{
				return BlockIndexInTexture + 2;
			}
			else
			{
				return BlockIndexInTexture + 1;
			}
		}

		/// <summary>
		/// Returns the block texture based on the side being looked at.  Args: side
		/// </summary>
		public override int GetBlockTextureFromSide(int par1)
		{
			if (par1 == 1)
			{
				return BlockIndexInTexture;
			}

			if (par1 == 0)
			{
				return BlockIndexInTexture + 3;
			}
			else
			{
				return BlockIndexInTexture + 1;
			}
		}

		/// <summary>
		/// If this block doesn't render as an ordinary block it will return False (examples: signs, buttons, stairs, etc)
		/// </summary>
		public override bool RenderAsNormalBlock()
		{
			return false;
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
		/// Called upon block activation (left or right click on the block.). The three integers represent x,y,z of the
		/// block.
		/// </summary>
		public override bool BlockActivated(World par1World, int par2, int par3, int par4, EntityPlayer par5EntityPlayer)
		{
			EatCakeSlice(par1World, par2, par3, par4, par5EntityPlayer);
			return true;
		}

		/// <summary>
		/// Called when the block is clicked by a player. Args: x, y, z, entityPlayer
		/// </summary>
		public override void OnBlockClicked(World par1World, int par2, int par3, int par4, EntityPlayer par5EntityPlayer)
		{
			EatCakeSlice(par1World, par2, par3, par4, par5EntityPlayer);
		}

		/// <summary>
		/// Heals the player and removes a slice from the cake.
		/// </summary>
		private void EatCakeSlice(World par1World, int par2, int par3, int par4, EntityPlayer par5EntityPlayer)
		{
			if (par5EntityPlayer.CanEat(false))
			{
				par5EntityPlayer.GetFoodStats().AddStats(2, 0.1F);
				int i = par1World.GetBlockMetadata(par2, par3, par4) + 1;

				if (i >= 6)
				{
					par1World.SetBlockWithNotify(par2, par3, par4, 0);
				}
				else
				{
					par1World.SetBlockMetadataWithNotify(par2, par3, par4, i);
					par1World.MarkBlockAsNeedsUpdate(par2, par3, par4);
				}
			}
		}

		/// <summary>
		/// Checks to see if its valid to put this block at the specified coordinates. Args: world, x, y, z
		/// </summary>
		public override bool CanPlaceBlockAt(World par1World, int par2, int par3, int par4)
		{
			if (!base.CanPlaceBlockAt(par1World, par2, par3, par4))
			{
				return false;
			}
			else
			{
				return CanBlockStay(par1World, par2, par3, par4);
			}
		}

		/// <summary>
		/// Lets the block know when one of its neighbor changes. Doesn't know which neighbor changed (coordinates passed are
		/// their own) Args: x, y, z, neighbor BlockID
		/// </summary>
		public override void OnNeighborBlockChange(World par1World, int par2, int par3, int par4, int par5)
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
			return par1World.GetBlockMaterial(par2, par3 - 1, par4).IsSolid();
		}

		/// <summary>
		/// Returns the quantity of items to drop on block destruction.
		/// </summary>
		public override int QuantityDropped(Random par1Random)
		{
			return 0;
		}

		/// <summary>
		/// Returns the ID of the items to drop on destruction.
		/// </summary>
		public override int IdDropped(int par1, Random par2Random, int par3)
		{
			return 0;
		}
	}
}