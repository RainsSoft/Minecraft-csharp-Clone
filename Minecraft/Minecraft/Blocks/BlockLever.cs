namespace net.minecraft.src
{
	public class BlockLever : Block
	{
        public BlockLever(int par1, int par2)
            : base(par1, par2, Material.Circuits)
		{
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
			return 12;
		}

		/// <summary>
		/// checks to see if you can place this block can be placed on that side of a block: BlockLever overrides
		/// </summary>
		public override bool CanPlaceBlockOnSide(World par1World, int par2, int par3, int par4, int par5)
		{
			if (par5 == 1 && par1World.IsBlockNormalCube(par2, par3 - 1, par4))
			{
				return true;
			}

			if (par5 == 2 && par1World.IsBlockNormalCube(par2, par3, par4 + 1))
			{
				return true;
			}

			if (par5 == 3 && par1World.IsBlockNormalCube(par2, par3, par4 - 1))
			{
				return true;
			}

			if (par5 == 4 && par1World.IsBlockNormalCube(par2 + 1, par3, par4))
			{
				return true;
			}

			return par5 == 5 && par1World.IsBlockNormalCube(par2 - 1, par3, par4);
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

			if (par1World.IsBlockNormalCube(par2, par3, par4 + 1))
			{
				return true;
			}

			return par1World.IsBlockNormalCube(par2, par3 - 1, par4);
		}

		/// <summary>
		/// Called when a block is placed using an item. Used often for taking the facing and figuring out how to position
		/// the item. Args: x, y, z, facing
		/// </summary>
		public override void OnBlockPlaced(World par1World, int par2, int par3, int par4, int par5)
		{
			int i = par1World.GetBlockMetadata(par2, par3, par4);
			int j = i & 8;
			i &= 7;
			i = -1;

			if (par5 == 1 && par1World.IsBlockNormalCube(par2, par3 - 1, par4))
			{
				i = 5 + par1World.Rand.Next(2);
			}

			if (par5 == 2 && par1World.IsBlockNormalCube(par2, par3, par4 + 1))
			{
				i = 4;
			}

			if (par5 == 3 && par1World.IsBlockNormalCube(par2, par3, par4 - 1))
			{
				i = 3;
			}

			if (par5 == 4 && par1World.IsBlockNormalCube(par2 + 1, par3, par4))
			{
				i = 2;
			}

			if (par5 == 5 && par1World.IsBlockNormalCube(par2 - 1, par3, par4))
			{
				i = 1;
			}

			if (i == -1)
			{
				DropBlockAsItem(par1World, par2, par3, par4, par1World.GetBlockMetadata(par2, par3, par4), 0);
				par1World.SetBlockWithNotify(par2, par3, par4, 0);
				return;
			}
			else
			{
				par1World.SetBlockMetadataWithNotify(par2, par3, par4, i + j);
				return;
			}
		}

		/// <summary>
		/// Lets the block know when one of its neighbor changes. Doesn't know which neighbor changed (coordinates passed are
		/// their own) Args: x, y, z, neighbor BlockID
		/// </summary>
		public override void OnNeighborBlockChange(World par1World, int par2, int par3, int par4, int par5)
		{
			if (CheckIfAttachedToBlock(par1World, par2, par3, par4))
			{
				int i = par1World.GetBlockMetadata(par2, par3, par4) & 7;
				bool flag = false;

				if (!par1World.IsBlockNormalCube(par2 - 1, par3, par4) && i == 1)
				{
					flag = true;
				}

				if (!par1World.IsBlockNormalCube(par2 + 1, par3, par4) && i == 2)
				{
					flag = true;
				}

				if (!par1World.IsBlockNormalCube(par2, par3, par4 - 1) && i == 3)
				{
					flag = true;
				}

				if (!par1World.IsBlockNormalCube(par2, par3, par4 + 1) && i == 4)
				{
					flag = true;
				}

				if (!par1World.IsBlockNormalCube(par2, par3 - 1, par4) && i == 5)
				{
					flag = true;
				}

				if (!par1World.IsBlockNormalCube(par2, par3 - 1, par4) && i == 6)
				{
					flag = true;
				}

				if (flag)
				{
					DropBlockAsItem(par1World, par2, par3, par4, par1World.GetBlockMetadata(par2, par3, par4), 0);
					par1World.SetBlockWithNotify(par2, par3, par4, 0);
				}
			}
		}

		/// <summary>
		/// Checks if the block is attached to another block. If it is not, it returns false and drops the block as an item.
		/// If it is it returns true.
		/// </summary>
		private bool CheckIfAttachedToBlock(World par1World, int par2, int par3, int par4)
		{
			if (!CanPlaceBlockAt(par1World, par2, par3, par4))
			{
				DropBlockAsItem(par1World, par2, par3, par4, par1World.GetBlockMetadata(par2, par3, par4), 0);
				par1World.SetBlockWithNotify(par2, par3, par4, 0);
				return false;
			}
			else
			{
				return true;
			}
		}

		/// <summary>
		/// Updates the blocks bounds based on its current state. Args: world, x, y, z
		/// </summary>
		public override void SetBlockBoundsBasedOnState(IBlockAccess par1IBlockAccess, int par2, int par3, int par4)
		{
			int i = par1IBlockAccess.GetBlockMetadata(par2, par3, par4) & 7;
			float f = 0.1875F;

			if (i == 1)
			{
				SetBlockBounds(0.0F, 0.2F, 0.5F - f, f * 2.0F, 0.8F, 0.5F + f);
			}
			else if (i == 2)
			{
				SetBlockBounds(1.0F - f * 2.0F, 0.2F, 0.5F - f, 1.0F, 0.8F, 0.5F + f);
			}
			else if (i == 3)
			{
				SetBlockBounds(0.5F - f, 0.2F, 0.0F, 0.5F + f, 0.8F, f * 2.0F);
			}
			else if (i == 4)
			{
				SetBlockBounds(0.5F - f, 0.2F, 1.0F - f * 2.0F, 0.5F + f, 0.8F, 1.0F);
			}
			else
			{
				float f1 = 0.25F;
				SetBlockBounds(0.5F - f1, 0.0F, 0.5F - f1, 0.5F + f1, 0.6F, 0.5F + f1);
			}
		}

		/// <summary>
		/// Called when the block is clicked by a player. Args: x, y, z, entityPlayer
		/// </summary>
		public override void OnBlockClicked(World par1World, int par2, int par3, int par4, EntityPlayer par5EntityPlayer)
		{
			BlockActivated(par1World, par2, par3, par4, par5EntityPlayer);
		}

		/// <summary>
		/// Called upon block activation (left or right click on the block.). The three integers represent x,y,z of the
		/// block.
		/// </summary>
		public override bool BlockActivated(World par1World, int par2, int par3, int par4, EntityPlayer par5EntityPlayer)
		{
			if (par1World.IsRemote)
			{
				return true;
			}

			int i = par1World.GetBlockMetadata(par2, par3, par4);
			int j = i & 7;
			int k = 8 - (i & 8);
			par1World.SetBlockMetadataWithNotify(par2, par3, par4, j + k);
			par1World.MarkBlocksDirty(par2, par3, par4, par2, par3, par4);
			par1World.PlaySoundEffect((double)par2 + 0.5D, (double)par3 + 0.5D, (double)par4 + 0.5D, "random.click", 0.3F, k <= 0 ? 0.5F : 0.6F);
			par1World.NotifyBlocksOfNeighborChange(par2, par3, par4, BlockID);

			if (j == 1)
			{
				par1World.NotifyBlocksOfNeighborChange(par2 - 1, par3, par4, BlockID);
			}
			else if (j == 2)
			{
				par1World.NotifyBlocksOfNeighborChange(par2 + 1, par3, par4, BlockID);
			}
			else if (j == 3)
			{
				par1World.NotifyBlocksOfNeighborChange(par2, par3, par4 - 1, BlockID);
			}
			else if (j == 4)
			{
				par1World.NotifyBlocksOfNeighborChange(par2, par3, par4 + 1, BlockID);
			}
			else
			{
				par1World.NotifyBlocksOfNeighborChange(par2, par3 - 1, par4, BlockID);
			}

			return true;
		}

		/// <summary>
		/// Called whenever the block is removed.
		/// </summary>
		public override void OnBlockRemoval(World par1World, int par2, int par3, int par4)
		{
			int i = par1World.GetBlockMetadata(par2, par3, par4);

			if ((i & 8) > 0)
			{
				par1World.NotifyBlocksOfNeighborChange(par2, par3, par4, BlockID);
				int j = i & 7;

				if (j == 1)
				{
					par1World.NotifyBlocksOfNeighborChange(par2 - 1, par3, par4, BlockID);
				}
				else if (j == 2)
				{
					par1World.NotifyBlocksOfNeighborChange(par2 + 1, par3, par4, BlockID);
				}
				else if (j == 3)
				{
					par1World.NotifyBlocksOfNeighborChange(par2, par3, par4 - 1, BlockID);
				}
				else if (j == 4)
				{
					par1World.NotifyBlocksOfNeighborChange(par2, par3, par4 + 1, BlockID);
				}
				else
				{
					par1World.NotifyBlocksOfNeighborChange(par2, par3 - 1, par4, BlockID);
				}
			}

			base.OnBlockRemoval(par1World, par2, par3, par4);
		}

		/// <summary>
		/// Is this block powering the block on the specified side
		/// </summary>
		public override bool IsPoweringTo(IBlockAccess par1IBlockAccess, int par2, int par3, int par4, int par5)
		{
			return (par1IBlockAccess.GetBlockMetadata(par2, par3, par4) & 8) > 0;
		}

		/// <summary>
		/// Is this block indirectly powering the block on the specified side
		/// </summary>
		public override bool IsIndirectlyPoweringTo(World par1World, int par2, int par3, int par4, int par5)
		{
			int i = par1World.GetBlockMetadata(par2, par3, par4);

			if ((i & 8) == 0)
			{
				return false;
			}

			int j = i & 7;

			if (j == 6 && par5 == 1)
			{
				return true;
			}

			if (j == 5 && par5 == 1)
			{
				return true;
			}

			if (j == 4 && par5 == 2)
			{
				return true;
			}

			if (j == 3 && par5 == 3)
			{
				return true;
			}

			if (j == 2 && par5 == 4)
			{
				return true;
			}

			return j == 1 && par5 == 5;
		}

		/// <summary>
		/// Can this block provide power. Only wire currently seems to have this change based on its state.
		/// </summary>
		public override bool CanProvidePower()
		{
			return true;
		}
	}

}