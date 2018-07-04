namespace net.minecraft.src
{
	public class BlockFence : Block
	{
		public BlockFence(int par1, int par2) : base(par1, par2, Material.Wood)
		{
		}

		public BlockFence(int par1, int par2, Material par3Material) : base(par1, par2, par3Material)
		{
		}

		/// <summary>
		/// Checks to see if its valid to put this block at the specified coordinates. Args: world, x, y, z
		/// </summary>
		public override bool CanPlaceBlockAt(World par1World, int par2, int par3, int par4)
		{
			return base.CanPlaceBlockAt(par1World, par2, par3, par4);
		}

		/// <summary>
		/// Returns a bounding box from the pool of bounding boxes (this means this box can change after the pool has been
		/// cleared to be reused)
		/// </summary>
		public override AxisAlignedBB GetCollisionBoundingBoxFromPool(World par1World, int par2, int par3, int par4)
		{
			bool flag = CanConnectFenceTo(par1World, par2, par3, par4 - 1);
			bool flag1 = CanConnectFenceTo(par1World, par2, par3, par4 + 1);
			bool flag2 = CanConnectFenceTo(par1World, par2 - 1, par3, par4);
			bool flag3 = CanConnectFenceTo(par1World, par2 + 1, par3, par4);
			float f = 0.375F;
			float f1 = 0.625F;
			float f2 = 0.375F;
			float f3 = 0.625F;

			if (flag)
			{
				f2 = 0.0F;
			}

			if (flag1)
			{
				f3 = 1.0F;
			}

			if (flag2)
			{
				f = 0.0F;
			}

			if (flag3)
			{
				f1 = 1.0F;
			}

			return AxisAlignedBB.GetBoundingBoxFromPool((float)par2 + f, par3, (float)par4 + f2, (float)par2 + f1, (float)par3 + 1.5F, (float)par4 + f3);
		}

		/// <summary>
		/// Updates the blocks bounds based on its current state. Args: world, x, y, z
		/// </summary>
		public override void SetBlockBoundsBasedOnState(IBlockAccess par1IBlockAccess, int par2, int par3, int par4)
		{
			bool flag = CanConnectFenceTo(par1IBlockAccess, par2, par3, par4 - 1);
			bool flag1 = CanConnectFenceTo(par1IBlockAccess, par2, par3, par4 + 1);
			bool flag2 = CanConnectFenceTo(par1IBlockAccess, par2 - 1, par3, par4);
			bool flag3 = CanConnectFenceTo(par1IBlockAccess, par2 + 1, par3, par4);
			float f = 0.375F;
			float f1 = 0.625F;
			float f2 = 0.375F;
			float f3 = 0.625F;

			if (flag)
			{
				f2 = 0.0F;
			}

			if (flag1)
			{
				f3 = 1.0F;
			}

			if (flag2)
			{
				f = 0.0F;
			}

			if (flag3)
			{
				f1 = 1.0F;
			}

			SetBlockBounds(f, 0.0F, f2, f1, 1.0F, f3);
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

		public override bool GetBlocksMovement(IBlockAccess par1IBlockAccess, int par2, int par3, int i)
		{
			return false;
		}

		/// <summary>
		/// The type of render function that is called for this block
		/// </summary>
		public override int GetRenderType()
		{
			return 11;
		}

		/// <summary>
		/// Returns true if the specified block can be connected by a fence
		/// </summary>
		public virtual bool CanConnectFenceTo(IBlockAccess par1IBlockAccess, int par2, int par3, int par4)
		{
			int i = par1IBlockAccess.GetBlockId(par2, par3, par4);

			if (i == BlockID || i == Block.FenceGate.BlockID)
			{
				return true;
			}

			Block block = Block.BlocksList[i];

			if (block != null && block.BlockMaterial.IsOpaque() && block.RenderAsNormalBlock())
			{
				return block.BlockMaterial != Material.Pumpkin;
			}
			else
			{
				return false;
			}
		}
	}
}