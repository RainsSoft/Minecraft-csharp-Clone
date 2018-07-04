using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class BlockPistonExtension : Block
	{
		/// <summary>
		/// The texture for the 'head' of the piston. Sticky or normal. </summary>
		private int HeadTexture;

		public BlockPistonExtension(int par1, int par2) : base(par1, par2, Material.Piston)
		{
			HeadTexture = -1;
			SetStepSound(SoundStoneFootstep);
			SetHardness(0.5F);
		}

		public virtual void SetHeadTexture(int par1)
		{
			HeadTexture = par1;
		}

		public virtual void ClearHeadTexture()
		{
			HeadTexture = -1;
		}

		/// <summary>
		/// Called whenever the block is removed.
		/// </summary>
		public override void OnBlockRemoval(World par1World, int par2, int par3, int par4)
		{
			base.OnBlockRemoval(par1World, par2, par3, par4);
			int i = par1World.GetBlockMetadata(par2, par3, par4);
			int k = Facing.FaceToSide[GetDirectionMeta(i)];
			par2 += Facing.OffsetsXForSide[k];
			par3 += Facing.OffsetsYForSide[k];
			par4 += Facing.OffsetsZForSide[k];
			int l = par1World.GetBlockId(par2, par3, par4);

			if (l == Block.PistonBase.BlockID || l == Block.PistonStickyBase.BlockID)
			{
				int j = par1World.GetBlockMetadata(par2, par3, par4);

				if (BlockPistonBase.IsExtended(j))
				{
					Block.BlocksList[l].DropBlockAsItem(par1World, par2, par3, par4, j, 0);
					par1World.SetBlockWithNotify(par2, par3, par4, 0);
				}
			}
		}

		/// <summary>
		/// From the specified side and block metadata retrieves the blocks texture. Args: side, metadata
		/// </summary>
		public override int GetBlockTextureFromSideAndMetadata(int par1, int par2)
		{
			int i = GetDirectionMeta(par2);

			if (par1 == i)
			{
				if (HeadTexture >= 0)
				{
					return HeadTexture;
				}

				if ((par2 & 8) != 0)
				{
					return BlockIndexInTexture - 1;
				}
				else
				{
					return BlockIndexInTexture;
				}
			}

			return par1 != Facing.FaceToSide[i] ? 108 : 107;
		}

		/// <summary>
		/// The type of render function that is called for this block
		/// </summary>
		public override int GetRenderType()
		{
			return 17;
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
		/// Checks to see if its valid to put this block at the specified coordinates. Args: world, x, y, z
		/// </summary>
		public override bool CanPlaceBlockAt(World par1World, int par2, int par3, int i)
		{
			return false;
		}

		/// <summary>
		/// checks to see if you can place this block can be placed on that side of a block: BlockLever overrides
		/// </summary>
		public override bool CanPlaceBlockOnSide(World par1World, int par2, int par3, int i, int j)
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
		/// Adds to the supplied array any colliding bounding boxes with the passed in bounding box. Args: world, x, y, z,
		/// axisAlignedBB, arrayList
		/// </summary>
        public override void GetCollidingBoundingBoxes(World par1World, int par2, int par3, int par4, AxisAlignedBB par5AxisAlignedBB, List<AxisAlignedBB> par6ArrayList)
		{
			int i = par1World.GetBlockMetadata(par2, par3, par4);

			switch (GetDirectionMeta(i))
			{
				case 0:
					SetBlockBounds(0.0F, 0.0F, 0.0F, 1.0F, 0.25F, 1.0F);
					base.GetCollidingBoundingBoxes(par1World, par2, par3, par4, par5AxisAlignedBB, par6ArrayList);
					SetBlockBounds(0.375F, 0.25F, 0.375F, 0.625F, 1.0F, 0.625F);
					base.GetCollidingBoundingBoxes(par1World, par2, par3, par4, par5AxisAlignedBB, par6ArrayList);
					break;

				case 1:
					SetBlockBounds(0.0F, 0.75F, 0.0F, 1.0F, 1.0F, 1.0F);
					base.GetCollidingBoundingBoxes(par1World, par2, par3, par4, par5AxisAlignedBB, par6ArrayList);
					SetBlockBounds(0.375F, 0.0F, 0.375F, 0.625F, 0.75F, 0.625F);
					base.GetCollidingBoundingBoxes(par1World, par2, par3, par4, par5AxisAlignedBB, par6ArrayList);
					break;

				case 2:
					SetBlockBounds(0.0F, 0.0F, 0.0F, 1.0F, 1.0F, 0.25F);
					base.GetCollidingBoundingBoxes(par1World, par2, par3, par4, par5AxisAlignedBB, par6ArrayList);
					SetBlockBounds(0.25F, 0.375F, 0.25F, 0.75F, 0.625F, 1.0F);
					base.GetCollidingBoundingBoxes(par1World, par2, par3, par4, par5AxisAlignedBB, par6ArrayList);
					break;

				case 3:
					SetBlockBounds(0.0F, 0.0F, 0.75F, 1.0F, 1.0F, 1.0F);
					base.GetCollidingBoundingBoxes(par1World, par2, par3, par4, par5AxisAlignedBB, par6ArrayList);
					SetBlockBounds(0.25F, 0.375F, 0.0F, 0.75F, 0.625F, 0.75F);
					base.GetCollidingBoundingBoxes(par1World, par2, par3, par4, par5AxisAlignedBB, par6ArrayList);
					break;

				case 4:
					SetBlockBounds(0.0F, 0.0F, 0.0F, 0.25F, 1.0F, 1.0F);
					base.GetCollidingBoundingBoxes(par1World, par2, par3, par4, par5AxisAlignedBB, par6ArrayList);
					SetBlockBounds(0.375F, 0.25F, 0.25F, 0.625F, 0.75F, 1.0F);
					base.GetCollidingBoundingBoxes(par1World, par2, par3, par4, par5AxisAlignedBB, par6ArrayList);
					break;

				case 5:
					SetBlockBounds(0.75F, 0.0F, 0.0F, 1.0F, 1.0F, 1.0F);
					base.GetCollidingBoundingBoxes(par1World, par2, par3, par4, par5AxisAlignedBB, par6ArrayList);
					SetBlockBounds(0.0F, 0.375F, 0.25F, 0.75F, 0.625F, 0.75F);
					base.GetCollidingBoundingBoxes(par1World, par2, par3, par4, par5AxisAlignedBB, par6ArrayList);
					break;
			}

			SetBlockBounds(0.0F, 0.0F, 0.0F, 1.0F, 1.0F, 1.0F);
		}

		/// <summary>
		/// Updates the blocks bounds based on its current state. Args: world, x, y, z
		/// </summary>
		public override void SetBlockBoundsBasedOnState(IBlockAccess par1IBlockAccess, int par2, int par3, int par4)
		{
			int i = par1IBlockAccess.GetBlockMetadata(par2, par3, par4);

			switch (GetDirectionMeta(i))
			{
				case 0:
					SetBlockBounds(0.0F, 0.0F, 0.0F, 1.0F, 0.25F, 1.0F);
					break;

				case 1:
					SetBlockBounds(0.0F, 0.75F, 0.0F, 1.0F, 1.0F, 1.0F);
					break;

				case 2:
					SetBlockBounds(0.0F, 0.0F, 0.0F, 1.0F, 1.0F, 0.25F);
					break;

				case 3:
					SetBlockBounds(0.0F, 0.0F, 0.75F, 1.0F, 1.0F, 1.0F);
					break;

				case 4:
					SetBlockBounds(0.0F, 0.0F, 0.0F, 0.25F, 1.0F, 1.0F);
					break;

				case 5:
					SetBlockBounds(0.75F, 0.0F, 0.0F, 1.0F, 1.0F, 1.0F);
					break;
			}
		}

		/// <summary>
		/// Lets the block know when one of its neighbor changes. Doesn't know which neighbor changed (coordinates passed are
		/// their own) Args: x, y, z, neighbor BlockID
		/// </summary>
		public override void OnNeighborBlockChange(World par1World, int par2, int par3, int par4, int par5)
		{
			int i = GetDirectionMeta(par1World.GetBlockMetadata(par2, par3, par4));
			int j = par1World.GetBlockId(par2 - Facing.OffsetsXForSide[i], par3 - Facing.OffsetsYForSide[i], par4 - Facing.OffsetsZForSide[i]);

			if (j != Block.PistonBase.BlockID && j != Block.PistonStickyBase.BlockID)
			{
				par1World.SetBlockWithNotify(par2, par3, par4, 0);
			}
			else
			{
				Block.BlocksList[j].OnNeighborBlockChange(par1World, par2 - Facing.OffsetsXForSide[i], par3 - Facing.OffsetsYForSide[i], par4 - Facing.OffsetsZForSide[i], par5);
			}
		}

		public static int GetDirectionMeta(int par0)
		{
			return par0 & 7;
		}
	}
}