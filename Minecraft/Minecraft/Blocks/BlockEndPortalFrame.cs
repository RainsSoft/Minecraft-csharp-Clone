using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class BlockEndPortalFrame : Block
	{
		public BlockEndPortalFrame(int par1) : base(par1, 159, Material.Glass)
		{
		}

		/// <summary>
		/// From the specified side and block metadata retrieves the blocks texture. Args: side, metadata
		/// </summary>
		public override int GetBlockTextureFromSideAndMetadata(int par1, int par2)
		{
			if (par1 == 1)
			{
				return BlockIndexInTexture - 1;
			}

			if (par1 == 0)
			{
				return BlockIndexInTexture + 16;
			}
			else
			{
				return BlockIndexInTexture;
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
		/// The type of render function that is called for this block
		/// </summary>
		public override int GetRenderType()
		{
			return 26;
		}

		/// <summary>
		/// Sets the block's bounds for rendering it as an item
		/// </summary>
		public override void SetBlockBoundsForItemRender()
		{
			SetBlockBounds(0.0F, 0.0F, 0.0F, 1.0F, 0.8125F, 1.0F);
		}

		/// <summary>
		/// Adds to the supplied array any colliding bounding boxes with the passed in bounding box. Args: world, x, y, z,
		/// axisAlignedBB, arrayList
		/// </summary>
        public override void GetCollidingBoundingBoxes(World par1World, int par2, int par3, int par4, AxisAlignedBB par5AxisAlignedBB, List<AxisAlignedBB> par6ArrayList)
		{
			SetBlockBounds(0.0F, 0.0F, 0.0F, 1.0F, 0.8125F, 1.0F);
			base.GetCollidingBoundingBoxes(par1World, par2, par3, par4, par5AxisAlignedBB, par6ArrayList);
			int i = par1World.GetBlockMetadata(par2, par3, par4);

			if (IsEnderEyeInserted(i))
			{
				SetBlockBounds(0.3125F, 0.8125F, 0.3125F, 0.6875F, 1.0F, 0.6875F);
				base.GetCollidingBoundingBoxes(par1World, par2, par3, par4, par5AxisAlignedBB, par6ArrayList);
			}

			SetBlockBoundsForItemRender();
		}

		/// <summary>
		/// checks if an ender eye has been inserted into the frame block. parameters: metadata
		/// </summary>
		public static bool IsEnderEyeInserted(int par0)
		{
			return (par0 & 4) != 0;
		}

		/// <summary>
		/// Returns the ID of the items to drop on destruction.
		/// </summary>
		public override int IdDropped(int par1, Random par2Random, int par3)
		{
			return 0;
		}

		/// <summary>
		/// Called when the block is placed in the world.
		/// </summary>
		public override void OnBlockPlacedBy(World par1World, int par2, int par3, int par4, EntityLiving par5EntityLiving)
		{
			int i = ((MathHelper2.Floor_double((double)((par5EntityLiving.RotationYaw * 4F) / 360F) + 0.5D) & 3) + 2) % 4;
			par1World.SetBlockMetadataWithNotify(par2, par3, par4, i);
		}
	}

}