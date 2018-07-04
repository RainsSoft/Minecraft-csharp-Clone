using System;

namespace net.minecraft.src
{
	public class BlockButton : Block
	{
        public BlockButton(int par1, int par2)
            : base(par1, par2, Material.Circuits)
		{
			SetTickRandomly(true);
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
		/// How many world ticks before ticking
		/// </summary>
		public override int TickRate()
		{
			return 20;
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
		/// checks to see if you can place this block can be placed on that side of a block: BlockLever overrides
		/// </summary>
		public override bool CanPlaceBlockOnSide(World par1World, int par2, int par3, int par4, int par5)
		{
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

			return par1World.IsBlockNormalCube(par2, par3, par4 + 1);
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

			if (par5 == 2 && par1World.IsBlockNormalCube(par2, par3, par4 + 1))
			{
				i = 4;
			}
			else if (par5 == 3 && par1World.IsBlockNormalCube(par2, par3, par4 - 1))
			{
				i = 3;
			}
			else if (par5 == 4 && par1World.IsBlockNormalCube(par2 + 1, par3, par4))
			{
				i = 2;
			}
			else if (par5 == 5 && par1World.IsBlockNormalCube(par2 - 1, par3, par4))
			{
				i = 1;
			}
			else
			{
				i = GetOrientation(par1World, par2, par3, par4);
			}

			par1World.SetBlockMetadataWithNotify(par2, par3, par4, i + j);
		}

		/// <summary>
		/// Get side which this button is facing.
		/// </summary>
		private int GetOrientation(World par1World, int par2, int par3, int par4)
		{
			if (par1World.IsBlockNormalCube(par2 - 1, par3, par4))
			{
				return 1;
			}

			if (par1World.IsBlockNormalCube(par2 + 1, par3, par4))
			{
				return 2;
			}

			if (par1World.IsBlockNormalCube(par2, par3, par4 - 1))
			{
				return 3;
			}

			return !par1World.IsBlockNormalCube(par2, par3, par4 + 1) ? 1 : 4;
		}

		/// <summary>
		/// Lets the block know when one of its neighbor changes. Doesn't know which neighbor changed (coordinates passed are
		/// their own) Args: x, y, z, neighbor BlockID
		/// </summary>
		public override void OnNeighborBlockChange(World par1World, int par2, int par3, int par4, int par5)
		{
			if (RedundantCanPlaceBlockAt(par1World, par2, par3, par4))
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

				if (flag)
				{
					DropBlockAsItem(par1World, par2, par3, par4, par1World.GetBlockMetadata(par2, par3, par4), 0);
					par1World.SetBlockWithNotify(par2, par3, par4, 0);
				}
			}
		}

		/// <summary>
		/// This method is redundant, check it out...
		/// </summary>
		private bool RedundantCanPlaceBlockAt(World par1World, int par2, int par3, int par4)
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
			int i = par1IBlockAccess.GetBlockMetadata(par2, par3, par4);
			int j = i & 7;
			bool flag = (i & 8) > 0;
			float f = 0.375F;
			float f1 = 0.625F;
			float f2 = 0.1875F;
			float f3 = 0.125F;

			if (flag)
			{
				f3 = 0.0625F;
			}

			if (j == 1)
			{
				SetBlockBounds(0.0F, f, 0.5F - f2, f3, f1, 0.5F + f2);
			}
			else if (j == 2)
			{
				SetBlockBounds(1.0F - f3, f, 0.5F - f2, 1.0F, f1, 0.5F + f2);
			}
			else if (j == 3)
			{
				SetBlockBounds(0.5F - f2, f, 0.0F, 0.5F + f2, f1, f3);
			}
			else if (j == 4)
			{
				SetBlockBounds(0.5F - f2, f, 1.0F - f3, 0.5F + f2, f1, 1.0F);
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
			int i = par1World.GetBlockMetadata(par2, par3, par4);
			int j = i & 7;
			int k = 8 - (i & 8);

			if (k == 0)
			{
				return true;
			}

			par1World.SetBlockMetadataWithNotify(par2, par3, par4, j + k);
			par1World.MarkBlocksDirty(par2, par3, par4, par2, par3, par4);
			par1World.PlaySoundEffect((double)par2 + 0.5D, (double)par3 + 0.5D, (double)par4 + 0.5D, "random.click", 0.3F, 0.6F);
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

			par1World.ScheduleBlockUpdate(par2, par3, par4, BlockID, TickRate());
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

		/// <summary>
		/// Ticks the block if it's been scheduled
		/// </summary>
		public override void UpdateTick(World par1World, int par2, int par3, int par4, Random par5Random)
		{
			if (par1World.IsRemote)
			{
				return;
			}

			int i = par1World.GetBlockMetadata(par2, par3, par4);

			if ((i & 8) == 0)
			{
				return;
			}

			par1World.SetBlockMetadataWithNotify(par2, par3, par4, i & 7);
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

			par1World.PlaySoundEffect((double)par2 + 0.5D, (double)par3 + 0.5D, (double)par4 + 0.5D, "random.click", 0.3F, 0.5F);
			par1World.MarkBlocksDirty(par2, par3, par4, par2, par3, par4);
		}

		/// <summary>
		/// Sets the block's bounds for rendering it as an item
		/// </summary>
		public override void SetBlockBoundsForItemRender()
		{
			float f = 0.1875F;
			float f1 = 0.125F;
			float f2 = 0.125F;
			SetBlockBounds(0.5F - f, 0.5F - f1, 0.5F - f2, 0.5F + f, 0.5F + f1, 0.5F + f2);
		}
	}

}