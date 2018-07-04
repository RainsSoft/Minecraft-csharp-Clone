using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class BlockCauldron : Block
	{
		public BlockCauldron(int par1) : base(par1, Material.Iron)
		{
			BlockIndexInTexture = 154;
		}

		/// <summary>
		/// From the specified side and block metadata retrieves the blocks texture. Args: side, metadata
		/// </summary>
		public override int GetBlockTextureFromSideAndMetadata(int par1, int par2)
		{
			if (par1 == 1)
			{
				return 138;
			}

			return par1 != 0 ? 154 : 155;
		}

		/// <summary>
		/// Adds to the supplied array any colliding bounding boxes with the passed in bounding box. Args: world, x, y, z,
		/// axisAlignedBB, arrayList
		/// </summary>
		public override void GetCollidingBoundingBoxes(World par1World, int par2, int par3, int par4, AxisAlignedBB par5AxisAlignedBB, List<AxisAlignedBB> par6ArrayList)
		{
			SetBlockBounds(0.0F, 0.0F, 0.0F, 1.0F, 0.3125F, 1.0F);
			base.GetCollidingBoundingBoxes(par1World, par2, par3, par4, par5AxisAlignedBB, par6ArrayList);
			float f = 0.125F;
			SetBlockBounds(0.0F, 0.0F, 0.0F, f, 1.0F, 1.0F);
			base.GetCollidingBoundingBoxes(par1World, par2, par3, par4, par5AxisAlignedBB, par6ArrayList);
			SetBlockBounds(0.0F, 0.0F, 0.0F, 1.0F, 1.0F, f);
			base.GetCollidingBoundingBoxes(par1World, par2, par3, par4, par5AxisAlignedBB, par6ArrayList);
			SetBlockBounds(1.0F - f, 0.0F, 0.0F, 1.0F, 1.0F, 1.0F);
			base.GetCollidingBoundingBoxes(par1World, par2, par3, par4, par5AxisAlignedBB, par6ArrayList);
			SetBlockBounds(0.0F, 0.0F, 1.0F - f, 1.0F, 1.0F, 1.0F);
			base.GetCollidingBoundingBoxes(par1World, par2, par3, par4, par5AxisAlignedBB, par6ArrayList);
			SetBlockBoundsForItemRender();
		}

		/// <summary>
		/// Sets the block's bounds for rendering it as an item
		/// </summary>
		public override void SetBlockBoundsForItemRender()
		{
			SetBlockBounds(0.0F, 0.0F, 0.0F, 1.0F, 1.0F, 1.0F);
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
			return 24;
		}

		/// <summary>
		/// If this block doesn't render as an ordinary block it will return False (examples: signs, buttons, stairs, etc)
		/// </summary>
		public override bool RenderAsNormalBlock()
		{
			return false;
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

			ItemStack itemstack = par5EntityPlayer.Inventory.GetCurrentItem();

			if (itemstack == null)
			{
				return true;
			}

			int i = par1World.GetBlockMetadata(par2, par3, par4);

			if (itemstack.ItemID == Item.BucketWater.ShiftedIndex)
			{
				if (i < 3)
				{
					if (!par5EntityPlayer.Capabilities.IsCreativeMode)
					{
						par5EntityPlayer.Inventory.SetInventorySlotContents(par5EntityPlayer.Inventory.CurrentItem, new ItemStack(Item.BucketEmpty));
					}

					par1World.SetBlockMetadataWithNotify(par2, par3, par4, 3);
				}

				return true;
			}

			if (itemstack.ItemID == Item.GlassBottle.ShiftedIndex && i > 0)
			{
				ItemStack itemstack1 = new ItemStack(Item.Potion, 1, 0);

				if (!par5EntityPlayer.Inventory.AddItemStackToInventory(itemstack1))
				{
					par1World.SpawnEntityInWorld(new EntityItem(par1World, par2 + 0.5F, par3 + 1.5F, par4 + 0.5F, itemstack1));
				}

				itemstack.StackSize--;

				if (itemstack.StackSize <= 0)
				{
					par5EntityPlayer.Inventory.SetInventorySlotContents(par5EntityPlayer.Inventory.CurrentItem, null);
				}

				par1World.SetBlockMetadataWithNotify(par2, par3, par4, i - 1);
			}

			return true;
		}

		/// <summary>
		/// Returns the ID of the items to drop on destruction.
		/// </summary>
		public override int IdDropped(int par1, Random par2Random, int par3)
		{
			return Item.Cauldron.ShiftedIndex;
		}
	}
}