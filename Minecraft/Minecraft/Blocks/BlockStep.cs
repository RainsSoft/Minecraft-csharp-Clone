using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class BlockStep : Block
	{
		public static readonly string[] BlockStepTypes = { "stone", "sand", "wood", "cobble", "brick", "smoothStoneBrick" };

		/// <summary>
		/// bool used to seperate different states of blocks </summary>
		private bool BlockType;

		public BlockStep(int par1, bool par2) : base(par1, 6, Material.Rock)
		{
			BlockType = par2;

			if (!par2)
			{
				SetBlockBounds(0.0F, 0.0F, 0.0F, 1.0F, 0.5F, 1.0F);
			}
			else
			{
				OpaqueCubeLookup[par1] = true;
			}

			SetLightOpacity(255);
		}

		/// <summary>
		/// Updates the blocks bounds based on its current state. Args: world, x, y, z
		/// </summary>
		public override void SetBlockBoundsBasedOnState(IBlockAccess par1IBlockAccess, int par2, int par3, int par4)
		{
			if (BlockType)
			{
				SetBlockBounds(0.0F, 0.0F, 0.0F, 1.0F, 1.0F, 1.0F);
			}
			else
			{
				bool flag = (par1IBlockAccess.GetBlockMetadata(par2, par3, par4) & 8) != 0;

				if (flag)
				{
					SetBlockBounds(0.0F, 0.5F, 0.0F, 1.0F, 1.0F, 1.0F);
				}
				else
				{
					SetBlockBounds(0.0F, 0.0F, 0.0F, 1.0F, 0.5F, 1.0F);
				}
			}
		}

		/// <summary>
		/// Sets the block's bounds for rendering it as an item
		/// </summary>
		public override void SetBlockBoundsForItemRender()
		{
			if (BlockType)
			{
				SetBlockBounds(0.0F, 0.0F, 0.0F, 1.0F, 1.0F, 1.0F);
			}
			else
			{
				SetBlockBounds(0.0F, 0.0F, 0.0F, 1.0F, 0.5F, 1.0F);
			}
		}

		/// <summary>
		/// Adds to the supplied array any colliding bounding boxes with the passed in bounding box. Args: world, x, y, z,
		/// axisAlignedBB, arrayList
		/// </summary>
        public override void GetCollidingBoundingBoxes(World par1World, int par2, int par3, int par4, AxisAlignedBB par5AxisAlignedBB, List<AxisAlignedBB> par6ArrayList)
		{
			SetBlockBoundsBasedOnState(par1World, par2, par3, par4);
			base.GetCollidingBoundingBoxes(par1World, par2, par3, par4, par5AxisAlignedBB, par6ArrayList);
		}

		/// <summary>
		/// From the specified side and block metadata retrieves the blocks texture. Args: side, metadata
		/// </summary>
		public override int GetBlockTextureFromSideAndMetadata(int par1, int par2)
		{
			int i = par2 & 7;

			if (i == 0)
			{
				return par1 > 1 ? 5 : 6;
			}

			if (i == 1)
			{
				if (par1 == 0)
				{
					return 208;
				}

				return par1 != 1 ? 192 : 176;
			}

			if (i == 2)
			{
				return 4;
			}

			if (i == 3)
			{
				return 16;
			}

			if (i == 4)
			{
				return Block.Brick.BlockIndexInTexture;
			}

			if (i == 5)
			{
				return Block.StoneBrick.BlockIndexInTexture;
			}
			else
			{
				return 6;
			}
		}

		/// <summary>
		/// Returns the block texture based on the side being looked at.  Args: side
		/// </summary>
		public override int GetBlockTextureFromSide(int par1)
		{
			return GetBlockTextureFromSideAndMetadata(par1, 0);
		}

		/// <summary>
		/// Is this block (a) opaque and (b) a full 1m cube?  This determines whether or not to render the shared face of two
		/// adjacent blocks and also whether the player can attach torches, redstone wire, etc to this block.
		/// </summary>
		public override bool IsOpaqueCube()
		{
			return BlockType;
		}

		/// <summary>
		/// Called when a block is placed using an item. Used often for taking the facing and figuring out how to position
		/// the item. Args: x, y, z, facing
		/// </summary>
		public override void OnBlockPlaced(World par1World, int par2, int par3, int par4, int par5)
		{
			if (par5 == 0 && !BlockType)
			{
				int i = par1World.GetBlockMetadata(par2, par3, par4) & 7;
				par1World.SetBlockMetadataWithNotify(par2, par3, par4, i | 8);
			}
		}

		/// <summary>
		/// Returns the ID of the items to drop on destruction.
		/// </summary>
		public override int IdDropped(int par1, Random par2Random, int par3)
		{
			return Block.StairSingle.BlockID;
		}

		/// <summary>
		/// Returns the quantity of items to drop on block destruction.
		/// </summary>
		public override int QuantityDropped(Random par1Random)
		{
			return !BlockType ? 1 : 2;
		}

		/// <summary>
		/// Determines the damage on the item the block drops. Used in cloth and wood.
		/// </summary>
		protected override int DamageDropped(int par1)
		{
			return par1 & 7;
		}

		/// <summary>
		/// If this block doesn't render as an ordinary block it will return False (examples: signs, buttons, stairs, etc)
		/// </summary>
		public override bool RenderAsNormalBlock()
		{
			return BlockType;
		}

		/// <summary>
		/// Returns true if the given side of this block type should be rendered, if the adjacent block is at the given
		/// coordinates.  Args: blockAccess, x, y, z, side
		/// </summary>
		public override bool ShouldSideBeRendered(IBlockAccess par1IBlockAccess, int par2, int par3, int par4, int par5)
		{
			if (BlockType)
			{
				base.ShouldSideBeRendered(par1IBlockAccess, par2, par3, par4, par5);
			}

			if (par5 != 1 && par5 != 0 && !base.ShouldSideBeRendered(par1IBlockAccess, par2, par3, par4, par5))
			{
				return false;
			}

			int i = par2;
			int j = par3;
			int k = par4;
			i += Facing.OffsetsXForSide[Facing.FaceToSide[par5]];
			j += Facing.OffsetsYForSide[Facing.FaceToSide[par5]];
			k += Facing.OffsetsZForSide[Facing.FaceToSide[par5]];
			bool flag = (par1IBlockAccess.GetBlockMetadata(i, j, k) & 8) != 0;

			if (!flag)
			{
				if (par5 == 1)
				{
					return true;
				}

				if (par5 == 0 && base.ShouldSideBeRendered(par1IBlockAccess, par2, par3, par4, par5))
				{
					return true;
				}
				else
				{
					return par1IBlockAccess.GetBlockId(par2, par3, par4) != BlockID || (par1IBlockAccess.GetBlockMetadata(par2, par3, par4) & 8) != 0;
				}
			}

			if (par5 == 0)
			{
				return true;
			}

			if (par5 == 1 && base.ShouldSideBeRendered(par1IBlockAccess, par2, par3, par4, par5))
			{
				return true;
			}
			else
			{
				return par1IBlockAccess.GetBlockId(par2, par3, par4) != BlockID || (par1IBlockAccess.GetBlockMetadata(par2, par3, par4) & 8) == 0;
			}
		}

		/// <summary>
		/// Returns an item stack containing a single instance of the current block type. 'i' is the block's subtype/damage
		/// and is ignored for blocks which do not support subtypes. Blocks which cannot be harvested should return null.
		/// </summary>
		protected override ItemStack CreateStackedBlock(int par1)
		{
			return new ItemStack(Block.StairSingle.BlockID, 1, par1 & 7);
		}
	}

}