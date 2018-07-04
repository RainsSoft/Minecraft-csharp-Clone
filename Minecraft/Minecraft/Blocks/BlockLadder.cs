using System;

namespace net.minecraft.src
{
	public class BlockLadder : Block
	{
        public BlockLadder(int par1, int par2)
            : base(par1, par2, Material.Circuits)
		{
		}

		/// <summary>
		/// Returns a bounding box from the pool of bounding boxes (this means this box can change after the pool has been
		/// cleared to be reused)
		/// </summary>
		public override AxisAlignedBB GetCollisionBoundingBoxFromPool(World par1World, int par2, int par3, int par4)
		{
			int i = par1World.GetBlockMetadata(par2, par3, par4);
			float f = 0.125F;

			if (i == 2)
			{
				SetBlockBounds(0.0F, 0.0F, 1.0F - f, 1.0F, 1.0F, 1.0F);
			}

			if (i == 3)
			{
				SetBlockBounds(0.0F, 0.0F, 0.0F, 1.0F, 1.0F, f);
			}

			if (i == 4)
			{
				SetBlockBounds(1.0F - f, 0.0F, 0.0F, 1.0F, 1.0F, 1.0F);
			}

			if (i == 5)
			{
				SetBlockBounds(0.0F, 0.0F, 0.0F, f, 1.0F, 1.0F);
			}

			return base.GetCollisionBoundingBoxFromPool(par1World, par2, par3, par4);
		}

		/// <summary>
		/// Returns the bounding box of the wired rectangular prism to render.
		/// </summary>
		public override AxisAlignedBB GetSelectedBoundingBoxFromPool(World par1World, int par2, int par3, int par4)
		{
			int i = par1World.GetBlockMetadata(par2, par3, par4);
			float f = 0.125F;

			if (i == 2)
			{
				SetBlockBounds(0.0F, 0.0F, 1.0F - f, 1.0F, 1.0F, 1.0F);
			}

			if (i == 3)
			{
				SetBlockBounds(0.0F, 0.0F, 0.0F, 1.0F, 1.0F, f);
			}

			if (i == 4)
			{
				SetBlockBounds(1.0F - f, 0.0F, 0.0F, 1.0F, 1.0F, 1.0F);
			}

			if (i == 5)
			{
				SetBlockBounds(0.0F, 0.0F, 0.0F, f, 1.0F, 1.0F);
			}

			return base.GetSelectedBoundingBoxFromPool(par1World, par2, par3, par4);
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
			return 8;
		}

		/// <summary>
		/// Checks to see if its valid to put this block at the specified coordinates. Args: world, x, y, z
		/// </summary>
		public override bool CanPlaceBlockAt(World par1World, int par2, int par3, int par4)
		{
			if (par1World.IsBlockNormalCube(par2 - 1, par3, par4))
			{
				return true;
			}

			if (par1World.IsBlockNormalCube(par2 + 1, par3, par4))
			{
				return true;
			}

			if (par1World.IsBlockNormalCube(par2, par3, par4 - 1))
			{
				return true;
			}

			return par1World.IsBlockNormalCube(par2, par3, par4 + 1);
		}

		/// <summary>
		/// Called when a block is placed using an item. Used often for taking the facing and figuring out how to position
		/// the item. Args: x, y, z, facing
		/// </summary>
		public override void OnBlockPlaced(World par1World, int par2, int par3, int par4, int par5)
		{
			int i = par1World.GetBlockMetadata(par2, par3, par4);

			if ((i == 0 || par5 == 2) && par1World.IsBlockNormalCube(par2, par3, par4 + 1))
			{
				i = 2;
			}

			if ((i == 0 || par5 == 3) && par1World.IsBlockNormalCube(par2, par3, par4 - 1))
			{
				i = 3;
			}

			if ((i == 0 || par5 == 4) && par1World.IsBlockNormalCube(par2 + 1, par3, par4))
			{
				i = 4;
			}

			if ((i == 0 || par5 == 5) && par1World.IsBlockNormalCube(par2 - 1, par3, par4))
			{
				i = 5;
			}

			par1World.SetBlockMetadataWithNotify(par2, par3, par4, i);
		}

		/// <summary>
		/// Lets the block know when one of its neighbor changes. Doesn't know which neighbor changed (coordinates passed are
		/// their own) Args: x, y, z, neighbor BlockID
		/// </summary>
		public override void OnNeighborBlockChange(World par1World, int par2, int par3, int par4, int par5)
		{
			int i = par1World.GetBlockMetadata(par2, par3, par4);
			bool flag = false;

			if (i == 2 && par1World.IsBlockNormalCube(par2, par3, par4 + 1))
			{
				flag = true;
			}

			if (i == 3 && par1World.IsBlockNormalCube(par2, par3, par4 - 1))
			{
				flag = true;
			}

			if (i == 4 && par1World.IsBlockNormalCube(par2 + 1, par3, par4))
			{
				flag = true;
			}

			if (i == 5 && par1World.IsBlockNormalCube(par2 - 1, par3, par4))
			{
				flag = true;
			}

			if (!flag)
			{
				DropBlockAsItem(par1World, par2, par3, par4, i, 0);
				par1World.SetBlockWithNotify(par2, par3, par4, 0);
			}

			base.OnNeighborBlockChange(par1World, par2, par3, par4, par5);
		}

		/// <summary>
		/// Returns the quantity of items to drop on block destruction.
		/// </summary>
		public override int QuantityDropped(Random par1Random)
		{
			return 1;
		}
	}

}