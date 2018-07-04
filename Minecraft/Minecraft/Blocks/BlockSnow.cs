using System;

namespace net.minecraft.src
{
	public class BlockSnow : Block
	{
        public BlockSnow(int par1, int par2)
            : base(par1, par2, Material.Snow)
		{
			SetBlockBounds(0.0F, 0.0F, 0.0F, 1.0F, 0.125F, 1.0F);
			SetTickRandomly(true);
		}

		/// <summary>
		/// Returns a bounding box from the pool of bounding boxes (this means this box can change after the pool has been
		/// cleared to be reused)
		/// </summary>
		public override AxisAlignedBB GetCollisionBoundingBoxFromPool(World par1World, int par2, int par3, int par4)
		{
			int i = par1World.GetBlockMetadata(par2, par3, par4) & 7;

			if (i >= 3)
			{
				return AxisAlignedBB.GetBoundingBoxFromPool(par2 + MinX, par3 + MinY, par4 + MinZ, par2 + MaxX, par3 + 0.5F, par4 + MaxZ);
			}
			else
			{
				return null;
			}
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
		/// Updates the blocks bounds based on its current state. Args: world, x, y, z
		/// </summary>
		public override void SetBlockBoundsBasedOnState(IBlockAccess par1IBlockAccess, int par2, int par3, int par4)
		{
			int i = par1IBlockAccess.GetBlockMetadata(par2, par3, par4) & 7;
			float f = (float)(2 * (1 + i)) / 16F;
			SetBlockBounds(0.0F, 0.0F, 0.0F, 1.0F, f, 1.0F);
		}

		/// <summary>
		/// Checks to see if its valid to put this block at the specified coordinates. Args: world, x, y, z
		/// </summary>
		public override bool CanPlaceBlockAt(World par1World, int par2, int par3, int par4)
		{
			int i = par1World.GetBlockId(par2, par3 - 1, par4);

			if (i == 0 || i != Block.Leaves.BlockID && !Block.BlocksList[i].IsOpaqueCube())
			{
				return false;
			}
			else
			{
				return par1World.GetBlockMaterial(par2, par3 - 1, par4).BlocksMovement();
			}
		}

		/// <summary>
		/// Lets the block know when one of its neighbor changes. Doesn't know which neighbor changed (coordinates passed are
		/// their own) Args: x, y, z, neighbor BlockID
		/// </summary>
		public override void OnNeighborBlockChange(World par1World, int par2, int par3, int par4, int par5)
		{
			CanSnowStay(par1World, par2, par3, par4);
		}

		/// <summary>
		/// Checks if this snow block can stay at this location.
		/// </summary>
		private bool CanSnowStay(World par1World, int par2, int par3, int par4)
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
		/// Called when the player destroys a block with an item that can harvest it. (i, j, k) are the coordinates of the
		/// block and l is the block's subtype/damage.
		/// </summary>
		public override void HarvestBlock(World par1World, EntityPlayer par2EntityPlayer, int par3, int par4, int par5, int par6)
		{
			int i = Item.Snowball.ShiftedIndex;
			float f = 0.7F;
            float d = (par1World.Rand.NextFloat() * f) + (1.0F - f) * 0.5F;
            float d1 = (par1World.Rand.NextFloat() * f) + (1.0F - f) * 0.5F;
            float d2 = (par1World.Rand.NextFloat() * f) + (1.0F - f) * 0.5F;
			EntityItem entityitem = new EntityItem(par1World, par3 + d, par4 + d1, par5 + d2, new ItemStack(i, 1, 0));
			entityitem.DelayBeforeCanPickup = 10;
			par1World.SpawnEntityInWorld(entityitem);
			par1World.SetBlockWithNotify(par3, par4, par5, 0);
			par2EntityPlayer.AddStat(StatList.MineBlockStatArray[BlockID], 1);
		}

		/// <summary>
		/// Returns the ID of the items to drop on destruction.
		/// </summary>
		public override int IdDropped(int par1, Random par2Random, int par3)
		{
			return Item.Snowball.ShiftedIndex;
		}

		/// <summary>
		/// Returns the quantity of items to drop on block destruction.
		/// </summary>
		public override int QuantityDropped(Random par1Random)
		{
			return 0;
		}

		/// <summary>
		/// Ticks the block if it's been scheduled
		/// </summary>
		public override void UpdateTick(World par1World, int par2, int par3, int par4, Random par5Random)
		{
			if (par1World.GetSavedLightValue(SkyBlock.Block, par2, par3, par4) > 11)
			{
				DropBlockAsItem(par1World, par2, par3, par4, par1World.GetBlockMetadata(par2, par3, par4), 0);
				par1World.SetBlockWithNotify(par2, par3, par4, 0);
			}
		}

		/// <summary>
		/// Returns true if the given side of this block type should be rendered, if the adjacent block is at the given
		/// coordinates.  Args: blockAccess, x, y, z, side
		/// </summary>
		public override bool ShouldSideBeRendered(IBlockAccess par1IBlockAccess, int par2, int par3, int par4, int par5)
		{
			if (par5 == 1)
			{
				return true;
			}
			else
			{
				return base.ShouldSideBeRendered(par1IBlockAccess, par2, par3, par4, par5);
			}
		}
	}
}