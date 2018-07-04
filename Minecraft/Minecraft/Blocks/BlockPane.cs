using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class BlockPane : Block
	{
		/// <summary>
		/// Holds the texture index of the side of the pane (the thin lateral side)
		/// </summary>
		private int SideTextureIndex;

		/// <summary>
		/// If this field is true, the pane block drops itself when destroyed (like the iron fences), otherwise, it's just
		/// destroyed (like glass panes)
		/// </summary>
		private readonly bool CanDropItself;

        public BlockPane(int par1, int par2, int par3, Material par4Material, bool par5)
            : base(par1, par2, par4Material)
		{
			SideTextureIndex = par3;
			CanDropItself = par5;
		}

		/// <summary>
		/// Returns the ID of the items to drop on destruction.
		/// </summary>
		public override int IdDropped(int par1, Random par2Random, int par3)
		{
			if (!CanDropItself)
			{
				return 0;
			}
			else
			{
				return base.IdDropped(par1, par2Random, par3);
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
		/// The type of render function that is called for this block
		/// </summary>
		public override int GetRenderType()
		{
			return 18;
		}

		/// <summary>
		/// Returns true if the given side of this block type should be rendered, if the adjacent block is at the given
		/// coordinates.  Args: blockAccess, x, y, z, side
		/// </summary>
		public override bool ShouldSideBeRendered(IBlockAccess par1IBlockAccess, int par2, int par3, int par4, int par5)
		{
			int i = par1IBlockAccess.GetBlockId(par2, par3, par4);

			if (i == BlockID)
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
        public override void GetCollidingBoundingBoxes(World par1World, int par2, int par3, int par4, AxisAlignedBB par5AxisAlignedBB, List<AxisAlignedBB> par6ArrayList)
		{
			bool flag = CanThisPaneConnectToThisBlockID(par1World.GetBlockId(par2, par3, par4 - 1));
			bool flag1 = CanThisPaneConnectToThisBlockID(par1World.GetBlockId(par2, par3, par4 + 1));
			bool flag2 = CanThisPaneConnectToThisBlockID(par1World.GetBlockId(par2 - 1, par3, par4));
			bool flag3 = CanThisPaneConnectToThisBlockID(par1World.GetBlockId(par2 + 1, par3, par4));

			if (flag2 && flag3 || !flag2 && !flag3 && !flag && !flag1)
			{
				SetBlockBounds(0.0F, 0.0F, 0.4375F, 1.0F, 1.0F, 0.5625F);
				base.GetCollidingBoundingBoxes(par1World, par2, par3, par4, par5AxisAlignedBB, par6ArrayList);
			}
			else if (flag2 && !flag3)
			{
				SetBlockBounds(0.0F, 0.0F, 0.4375F, 0.5F, 1.0F, 0.5625F);
				base.GetCollidingBoundingBoxes(par1World, par2, par3, par4, par5AxisAlignedBB, par6ArrayList);
			}
			else if (!flag2 && flag3)
			{
				SetBlockBounds(0.5F, 0.0F, 0.4375F, 1.0F, 1.0F, 0.5625F);
				base.GetCollidingBoundingBoxes(par1World, par2, par3, par4, par5AxisAlignedBB, par6ArrayList);
			}

			if (flag && flag1 || !flag2 && !flag3 && !flag && !flag1)
			{
				SetBlockBounds(0.4375F, 0.0F, 0.0F, 0.5625F, 1.0F, 1.0F);
				base.GetCollidingBoundingBoxes(par1World, par2, par3, par4, par5AxisAlignedBB, par6ArrayList);
			}
			else if (flag && !flag1)
			{
				SetBlockBounds(0.4375F, 0.0F, 0.0F, 0.5625F, 1.0F, 0.5F);
				base.GetCollidingBoundingBoxes(par1World, par2, par3, par4, par5AxisAlignedBB, par6ArrayList);
			}
			else if (!flag && flag1)
			{
				SetBlockBounds(0.4375F, 0.0F, 0.5F, 0.5625F, 1.0F, 1.0F);
				base.GetCollidingBoundingBoxes(par1World, par2, par3, par4, par5AxisAlignedBB, par6ArrayList);
			}
		}

		/// <summary>
		/// Sets the block's bounds for rendering it as an item
		/// </summary>
		public override void SetBlockBoundsForItemRender()
		{
			SetBlockBounds(0.0F, 0.0F, 0.0F, 1.0F, 1.0F, 1.0F);
		}

		/// <summary>
		/// Updates the blocks bounds based on its current state. Args: world, x, y, z
		/// </summary>
		public override void SetBlockBoundsBasedOnState(IBlockAccess par1IBlockAccess, int par2, int par3, int par4)
		{
			float f = 0.4375F;
			float f1 = 0.5625F;
			float f2 = 0.4375F;
			float f3 = 0.5625F;
			bool flag = CanThisPaneConnectToThisBlockID(par1IBlockAccess.GetBlockId(par2, par3, par4 - 1));
			bool flag1 = CanThisPaneConnectToThisBlockID(par1IBlockAccess.GetBlockId(par2, par3, par4 + 1));
			bool flag2 = CanThisPaneConnectToThisBlockID(par1IBlockAccess.GetBlockId(par2 - 1, par3, par4));
			bool flag3 = CanThisPaneConnectToThisBlockID(par1IBlockAccess.GetBlockId(par2 + 1, par3, par4));

			if (flag2 && flag3 || !flag2 && !flag3 && !flag && !flag1)
			{
				f = 0.0F;
				f1 = 1.0F;
			}
			else if (flag2 && !flag3)
			{
				f = 0.0F;
			}
			else if (!flag2 && flag3)
			{
				f1 = 1.0F;
			}

			if (flag && flag1 || !flag2 && !flag3 && !flag && !flag1)
			{
				f2 = 0.0F;
				f3 = 1.0F;
			}
			else if (flag && !flag1)
			{
				f2 = 0.0F;
			}
			else if (!flag && flag1)
			{
				f3 = 1.0F;
			}

			SetBlockBounds(f, 0.0F, f2, f1, 1.0F, f3);
		}

		/// <summary>
		/// Returns the texture index of the thin side of the pane.
		/// </summary>
		public virtual int GetSideTextureIndex()
		{
			return SideTextureIndex;
		}

		/// <summary>
		/// Gets passed in the BlockID of the block adjacent and supposed to return true if its allowed to connect to the
		/// type of BlockID passed in. Args: BlockID
		/// </summary>
		public bool CanThisPaneConnectToThisBlockID(int par1)
		{
			return Block.OpaqueCubeLookup[par1] || par1 == BlockID || par1 == Block.Glass.BlockID;
		}
	}

}