using System;

namespace net.minecraft.src
{
	public class BlockSign : BlockContainer
	{
		private Type SignEntityClass;

		/// <summary>
		/// Whether this is a freestanding sign or a wall-mounted sign </summary>
		private bool IsFreestanding;

        public BlockSign(int par1, Type par2Class, bool par3)
            : base(par1, Material.Wood)
		{
			IsFreestanding = par3;
			BlockIndexInTexture = 4;
			SignEntityClass = par2Class;
			float f = 0.25F;
			float f1 = 1.0F;
			SetBlockBounds(0.5F - f, 0.0F, 0.5F - f, 0.5F + f, f1, 0.5F + f);
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
		/// Returns the bounding box of the wired rectangular prism to render.
		/// </summary>
		public override AxisAlignedBB GetSelectedBoundingBoxFromPool(World par1World, int par2, int par3, int par4)
		{
			SetBlockBoundsBasedOnState(par1World, par2, par3, par4);
			return base.GetSelectedBoundingBoxFromPool(par1World, par2, par3, par4);
		}

		/// <summary>
		/// Updates the blocks bounds based on its current state. Args: world, x, y, z
		/// </summary>
		public override void SetBlockBoundsBasedOnState(IBlockAccess par1IBlockAccess, int par2, int par3, int par4)
		{
			if (IsFreestanding)
			{
				return;
			}

			int i = par1IBlockAccess.GetBlockMetadata(par2, par3, par4);
			float f = 0.28125F;
			float f1 = 0.78125F;
			float f2 = 0.0F;
			float f3 = 1.0F;
			float f4 = 0.125F;
			SetBlockBounds(0.0F, 0.0F, 0.0F, 1.0F, 1.0F, 1.0F);

			if (i == 2)
			{
				SetBlockBounds(f2, f, 1.0F - f4, f3, f1, 1.0F);
			}

			if (i == 3)
			{
				SetBlockBounds(f2, f, 0.0F, f3, f1, f4);
			}

			if (i == 4)
			{
				SetBlockBounds(1.0F - f4, f, f2, 1.0F, f1, f3);
			}

			if (i == 5)
			{
				SetBlockBounds(0.0F, f, f2, f4, f1, f3);
			}
		}

		/// <summary>
		/// The type of render function that is called for this block
		/// </summary>
		public override int GetRenderType()
		{
			return -1;
		}

		/// <summary>
		/// If this block doesn't render as an ordinary block it will return False (examples: signs, buttons, stairs, etc)
		/// </summary>
		public override bool RenderAsNormalBlock()
		{
			return false;
		}

		public override bool GetBlocksMovement(IBlockAccess par1IBlockAccess, int par2, int par3, int i)
		{
			return true;
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
		/// Returns the TileEntity used by this block.
		/// </summary>
		public override TileEntity GetBlockEntity()
		{
			try
			{
				return (TileEntity)Activator.CreateInstance(SignEntityClass);
			}
			catch (Exception exception)
			{
				throw exception;
			}
		}

		/// <summary>
		/// Returns the ID of the items to drop on destruction.
		/// </summary>
		public override int IdDropped(int par1, Random par2Random, int par3)
		{
			return Item.Sign.ShiftedIndex;
		}

		/// <summary>
		/// Lets the block know when one of its neighbor changes. Doesn't know which neighbor changed (coordinates passed are
		/// their own) Args: x, y, z, neighbor BlockID
		/// </summary>
		public override void OnNeighborBlockChange(World par1World, int par2, int par3, int par4, int par5)
		{
			bool flag = false;

			if (IsFreestanding)
			{
				if (!par1World.GetBlockMaterial(par2, par3 - 1, par4).IsSolid())
				{
					flag = true;
				}
			}
			else
			{
				int i = par1World.GetBlockMetadata(par2, par3, par4);
				flag = true;

				if (i == 2 && par1World.GetBlockMaterial(par2, par3, par4 + 1).IsSolid())
				{
					flag = false;
				}

				if (i == 3 && par1World.GetBlockMaterial(par2, par3, par4 - 1).IsSolid())
				{
					flag = false;
				}

				if (i == 4 && par1World.GetBlockMaterial(par2 + 1, par3, par4).IsSolid())
				{
					flag = false;
				}

				if (i == 5 && par1World.GetBlockMaterial(par2 - 1, par3, par4).IsSolid())
				{
					flag = false;
				}
			}

			if (flag)
			{
				DropBlockAsItem(par1World, par2, par3, par4, par1World.GetBlockMetadata(par2, par3, par4), 0);
				par1World.SetBlockWithNotify(par2, par3, par4, 0);
			}

			base.OnNeighborBlockChange(par1World, par2, par3, par4, par5);
		}
	}
}