using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class BlockBrewingStand : BlockContainer
	{
		private Random Rand;

		public BlockBrewingStand(int par1) : base(par1, Material.Iron)
		{
			Rand = new Random();
			BlockIndexInTexture = 157;
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
			return 25;
		}

		/// <summary>
		/// Returns the TileEntity used by this block.
		/// </summary>
		public override TileEntity GetBlockEntity()
		{
			return new TileEntityBrewingStand();
		}

		/// <summary>
		/// If this block doesn't render as an ordinary block it will return False (examples: signs, buttons, stairs, etc)
		/// </summary>
		public override bool RenderAsNormalBlock()
		{
			return false;
		}

		/// <summary>
		/// Adds to the supplied array any colliding bounding boxes with the passed in bounding box. Args: world, x, y, z,
		/// axisAlignedBB, arrayList
		/// </summary>
		public override void GetCollidingBoundingBoxes(World par1World, int par2, int par3, int par4, AxisAlignedBB par5AxisAlignedBB, List<AxisAlignedBB> par6ArrayList)
		{
			SetBlockBounds(0.4375F, 0.0F, 0.4375F, 0.5625F, 0.875F, 0.5625F);
			base.GetCollidingBoundingBoxes(par1World, par2, par3, par4, par5AxisAlignedBB, par6ArrayList);
			SetBlockBoundsForItemRender();
			base.GetCollidingBoundingBoxes(par1World, par2, par3, par4, par5AxisAlignedBB, par6ArrayList);
		}

		/// <summary>
		/// Sets the block's bounds for rendering it as an item
		/// </summary>
		public override void SetBlockBoundsForItemRender()
		{
			SetBlockBounds(0.0F, 0.0F, 0.0F, 1.0F, 0.125F, 1.0F);
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

			TileEntityBrewingStand tileentitybrewingstand = (TileEntityBrewingStand)par1World.GetBlockTileEntity(par2, par3, par4);

			if (tileentitybrewingstand != null)
			{
				par5EntityPlayer.DisplayGUIBrewingStand(tileentitybrewingstand);
			}

			return true;
		}

		/// <summary>
		/// A randomly called display update to be able to add particles or other items for display
		/// </summary>
		public override void RandomDisplayTick(World par1World, int par2, int par3, int par4, Random par5Random)
		{
			double d = (float)par2 + 0.4F + par5Random.NextFloat() * 0.2F;
			double d1 = (float)par3 + 0.7F + par5Random.NextFloat() * 0.3F;
			double d2 = (float)par4 + 0.4F + par5Random.NextFloat() * 0.2F;
			par1World.SpawnParticle("smoke", d, d1, d2, 0.0F, 0.0F, 0.0F);
		}

		/// <summary>
		/// Called whenever the block is removed.
		/// </summary>
		public override void OnBlockRemoval(World par1World, int par2, int par3, int par4)
		{
			TileEntity tileentity = par1World.GetBlockTileEntity(par2, par3, par4);

			if (tileentity != null && (tileentity is TileEntityBrewingStand))
			{
				TileEntityBrewingStand tileentitybrewingstand = (TileEntityBrewingStand)tileentity;
				label0:

				for (int i = 0; i < tileentitybrewingstand.GetSizeInventory(); i++)
				{
					ItemStack itemstack = tileentitybrewingstand.GetStackInSlot(i);

					if (itemstack == null)
					{
						continue;
					}

					float f = Rand.NextFloat() * 0.8F + 0.1F;
					float f1 = Rand.NextFloat() * 0.8F + 0.1F;
					float f2 = Rand.NextFloat() * 0.8F + 0.1F;

					do
					{
						if (itemstack.StackSize <= 0)
						{
							goto label0;
						}

						int j = Rand.Next(21) + 10;

						if (j > itemstack.StackSize)
						{
							j = itemstack.StackSize;
						}

						itemstack.StackSize -= j;
						EntityItem entityitem = new EntityItem(par1World, (float)par2 + f, (float)par3 + f1, (float)par4 + f2, new ItemStack(itemstack.ItemID, j, itemstack.GetItemDamage()));
						float f3 = 0.05F;
						entityitem.MotionX = (float)Rand.NextGaussian() * f3;
						entityitem.MotionY = (float)Rand.NextGaussian() * f3 + 0.2F;
						entityitem.MotionZ = (float)Rand.NextGaussian() * f3;
						par1World.SpawnEntityInWorld(entityitem);
					}
					while (true);
				}
			}

			base.OnBlockRemoval(par1World, par2, par3, par4);
		}

		/// <summary>
		/// Returns the ID of the items to drop on destruction.
		/// </summary>
		public override int IdDropped(int par1, Random par2Random, int par3)
		{
			return Item.BrewingStand.ShiftedIndex;
		}
	}
}