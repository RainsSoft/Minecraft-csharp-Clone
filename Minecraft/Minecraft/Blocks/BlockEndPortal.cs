using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class BlockEndPortal : BlockContainer
	{
		/// <summary>
		/// true if the enderdragon has been killed - allows end portal blocks to be created in the end
		/// </summary>
		public static bool BossDefeated = false;

        public BlockEndPortal(int par1, Material par2Material)
            : base(par1, 0, par2Material)
		{
			SetLightValue(1.0F);
		}

		/// <summary>
		/// Returns the TileEntity used by this block.
		/// </summary>
		public override TileEntity GetBlockEntity()
		{
			return new TileEntityEndPortal();
		}

		/// <summary>
		/// Updates the blocks bounds based on its current state. Args: world, x, y, z
		/// </summary>
		public override void SetBlockBoundsBasedOnState(IBlockAccess par1IBlockAccess, int par2, int par3, int par4)
		{
			float f = 0.0625F;
			SetBlockBounds(0.0F, 0.0F, 0.0F, 1.0F, f, 1.0F);
		}

		/// <summary>
		/// Returns true if the given side of this block type should be rendered, if the adjacent block is at the given
		/// coordinates.  Args: blockAccess, x, y, z, side
		/// </summary>
		public override bool ShouldSideBeRendered(IBlockAccess par1IBlockAccess, int par2, int par3, int par4, int par5)
		{
			if (par5 != 0)
			{
				return false;
			}
			else
			{
				return base.ShouldSideBeRendered(par1IBlockAccess, par2, par3, par4, par5);
			}
		}

		/// <summary>
		/// Adds to the supplied array any colliding bounding boxes with the passed in bounding box. Args: world, x, y, z,
		/// axisAlignedBB, arrayList
		/// </summary>
        public override void GetCollidingBoundingBoxes(World world, int i, int j, int k, AxisAlignedBB axisalignedbb, List<AxisAlignedBB> arraylist)
		{
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
		/// Returns the quantity of items to drop on block destruction.
		/// </summary>
		public override int QuantityDropped(Random par1Random)
		{
			return 0;
		}

		/// <summary>
		/// Triggered whenever an entity collides with this block (enters into the block). Args: world, x, y, z, entity
		/// </summary>
		public override void OnEntityCollidedWithBlock(World par1World, int par2, int par3, int par4, Entity par5Entity)
		{
			if (par5Entity.RidingEntity == null && par5Entity.RiddenByEntity == null && (par5Entity is EntityPlayer) && !par1World.IsRemote)
			{
				((EntityPlayer)par5Entity).TravelToTheEnd(1);
			}
		}

		/// <summary>
		/// A randomly called display update to be able to add particles or other items for display
		/// </summary>
		public override void RandomDisplayTick(World par1World, int par2, int par3, int par4, Random par5Random)
		{
			double d = (float)par2 + par5Random.NextFloat();
			double d1 = (float)par3 + 0.8F;
			double d2 = (float)par4 + par5Random.NextFloat();
			double d3 = 0.0F;
			double d4 = 0.0F;
			double d5 = 0.0F;
			par1World.SpawnParticle("smoke", d, d1, d2, d3, d4, d5);
		}

		/// <summary>
		/// The type of render function that is called for this block
		/// </summary>
		public override int GetRenderType()
		{
			return -1;
		}

		/// <summary>
		/// Called whenever the block is added into the world. Args: world, x, y, z
		/// </summary>
		public override void OnBlockAdded(World par1World, int par2, int par3, int par4)
		{
			if (BossDefeated)
			{
				return;
			}

			if (par1World.WorldProvider.TheWorldType != 0)
			{
				par1World.SetBlockWithNotify(par2, par3, par4, 0);
				return;
			}
			else
			{
				return;
			}
		}
	}
}