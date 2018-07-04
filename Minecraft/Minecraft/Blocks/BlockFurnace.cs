using System;

namespace net.minecraft.src
{
	public class BlockFurnace : BlockContainer
	{
		/// <summary>
		/// Is the random generator used by furnace to drop the inventory contents in random directions.
		/// </summary>
		private Random FurnaceRand;

		/// <summary>
		/// True if this is an active furnace, false if idle </summary>
		private readonly bool IsActive;

		/// <summary>
		/// This flag is used to prevent the furnace inventory to be dropped upon block removal, is used internally when the
		/// furnace block changes from idle to active and vice-versa.
		/// </summary>
		private static bool KeepFurnaceInventory = false;

        public BlockFurnace(int par1, bool par2)
            : base(par1, Material.Rock)
		{
			FurnaceRand = new Random();
			IsActive = par2;
			BlockIndexInTexture = 45;
		}

		/// <summary>
		/// Returns the ID of the items to drop on destruction.
		/// </summary>
		public override int IdDropped(int par1, Random par2Random, int par3)
		{
			return Block.StoneOvenIdle.BlockID;
		}

		/// <summary>
		/// Called whenever the block is added into the world. Args: world, x, y, z
		/// </summary>
		public override void OnBlockAdded(World par1World, int par2, int par3, int par4)
		{
			base.OnBlockAdded(par1World, par2, par3, par4);
			SetDefaultDirection(par1World, par2, par3, par4);
		}

		/// <summary>
		/// set a blocks direction
		/// </summary>
		private void SetDefaultDirection(World par1World, int par2, int par3, int par4)
		{
			if (par1World.IsRemote)
			{
				return;
			}

			int i = par1World.GetBlockId(par2, par3, par4 - 1);
			int j = par1World.GetBlockId(par2, par3, par4 + 1);
			int k = par1World.GetBlockId(par2 - 1, par3, par4);
			int l = par1World.GetBlockId(par2 + 1, par3, par4);
			sbyte byte0 = 3;

			if (Block.OpaqueCubeLookup[i] && !Block.OpaqueCubeLookup[j])
			{
				byte0 = 3;
			}

			if (Block.OpaqueCubeLookup[j] && !Block.OpaqueCubeLookup[i])
			{
				byte0 = 2;
			}

			if (Block.OpaqueCubeLookup[k] && !Block.OpaqueCubeLookup[l])
			{
				byte0 = 5;
			}

			if (Block.OpaqueCubeLookup[l] && !Block.OpaqueCubeLookup[k])
			{
				byte0 = 4;
			}

			par1World.SetBlockMetadataWithNotify(par2, par3, par4, byte0);
		}

		/// <summary>
		/// Retrieves the block texture to use based on the display side. Args: iBlockAccess, x, y, z, side
		/// </summary>
		public override int GetBlockTexture(IBlockAccess par1IBlockAccess, int par2, int par3, int par4, int par5)
		{
			if (par5 == 1)
			{
				return BlockIndexInTexture + 17;
			}

			if (par5 == 0)
			{
				return BlockIndexInTexture + 17;
			}

			int i = par1IBlockAccess.GetBlockMetadata(par2, par3, par4);

			if (par5 != i)
			{
				return BlockIndexInTexture;
			}

			if (IsActive)
			{
				return BlockIndexInTexture + 16;
			}
			else
			{
				return BlockIndexInTexture - 1;
			}
		}

		/// <summary>
		/// A randomly called display update to be able to add particles or other items for display
		/// </summary>
		public override void RandomDisplayTick(World par1World, int par2, int par3, int par4, Random par5Random)
		{
			if (!IsActive)
			{
				return;
			}

			int i = par1World.GetBlockMetadata(par2, par3, par4);
			float f = (float)par2 + 0.5F;
			float f1 = (float)par3 + 0.0F + (par5Random.NextFloat() * 6F) / 16F;
			float f2 = (float)par4 + 0.5F;
			float f3 = 0.52F;
			float f4 = par5Random.NextFloat() * 0.6F - 0.3F;

			if (i == 4)
			{
				par1World.SpawnParticle("smoke", f - f3, f1, f2 + f4, 0.0F, 0.0F, 0.0F);
				par1World.SpawnParticle("flame", f - f3, f1, f2 + f4, 0.0F, 0.0F, 0.0F);
			}
			else if (i == 5)
			{
				par1World.SpawnParticle("smoke", f + f3, f1, f2 + f4, 0.0F, 0.0F, 0.0F);
				par1World.SpawnParticle("flame", f + f3, f1, f2 + f4, 0.0F, 0.0F, 0.0F);
			}
			else if (i == 2)
			{
				par1World.SpawnParticle("smoke", f + f4, f1, f2 - f3, 0.0F, 0.0F, 0.0F);
				par1World.SpawnParticle("flame", f + f4, f1, f2 - f3, 0.0F, 0.0F, 0.0F);
			}
			else if (i == 3)
			{
				par1World.SpawnParticle("smoke", f + f4, f1, f2 + f3, 0.0F, 0.0F, 0.0F);
				par1World.SpawnParticle("flame", f + f4, f1, f2 + f3, 0.0F, 0.0F, 0.0F);
			}
		}

		/// <summary>
		/// Returns the block texture based on the side being looked at.  Args: side
		/// </summary>
		public override int GetBlockTextureFromSide(int par1)
		{
			if (par1 == 1)
			{
				return BlockIndexInTexture + 17;
			}

			if (par1 == 0)
			{
				return BlockIndexInTexture + 17;
			}

			if (par1 == 3)
			{
				return BlockIndexInTexture - 1;
			}
			else
			{
				return BlockIndexInTexture;
			}
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

			TileEntityFurnace tileentityfurnace = (TileEntityFurnace)par1World.GetBlockTileEntity(par2, par3, par4);

			if (tileentityfurnace != null)
			{
				par5EntityPlayer.DisplayGUIFurnace(tileentityfurnace);
			}

			return true;
		}

		/// <summary>
		/// Update which block ID the furnace is using depending on whether or not it is burning
		/// </summary>
		public static void UpdateFurnaceBlockState(bool par0, World par1World, int par2, int par3, int par4)
		{
			int i = par1World.GetBlockMetadata(par2, par3, par4);
			TileEntity tileentity = par1World.GetBlockTileEntity(par2, par3, par4);
			KeepFurnaceInventory = true;

			if (par0)
			{
				par1World.SetBlockWithNotify(par2, par3, par4, Block.StoneOvenActive.BlockID);
			}
			else
			{
				par1World.SetBlockWithNotify(par2, par3, par4, Block.StoneOvenIdle.BlockID);
			}

			KeepFurnaceInventory = false;
			par1World.SetBlockMetadataWithNotify(par2, par3, par4, i);

			if (tileentity != null)
			{
				tileentity.Validate();
				par1World.SetBlockTileEntity(par2, par3, par4, tileentity);
			}
		}

		/// <summary>
		/// Returns the TileEntity used by this block.
		/// </summary>
		public override TileEntity GetBlockEntity()
		{
			return new TileEntityFurnace();
		}

		/// <summary>
		/// Called when the block is placed in the world.
		/// </summary>
		public override void OnBlockPlacedBy(World par1World, int par2, int par3, int par4, EntityLiving par5EntityLiving)
		{
			int i = MathHelper2.Floor_double((double)((par5EntityLiving.RotationYaw * 4F) / 360F) + 0.5D) & 3;

			if (i == 0)
			{
				par1World.SetBlockMetadataWithNotify(par2, par3, par4, 2);
			}

			if (i == 1)
			{
				par1World.SetBlockMetadataWithNotify(par2, par3, par4, 5);
			}

			if (i == 2)
			{
				par1World.SetBlockMetadataWithNotify(par2, par3, par4, 3);
			}

			if (i == 3)
			{
				par1World.SetBlockMetadataWithNotify(par2, par3, par4, 4);
			}
		}

		/// <summary>
		/// Called whenever the block is removed.
		/// </summary>
		public override void OnBlockRemoval(World par1World, int par2, int par3, int par4)
		{
			if (!KeepFurnaceInventory)
			{
				TileEntityFurnace tileentityfurnace = (TileEntityFurnace)par1World.GetBlockTileEntity(par2, par3, par4);

				if (tileentityfurnace != null)
				{
					label0:

					for (int i = 0; i < tileentityfurnace.GetSizeInventory(); i++)
					{
						ItemStack itemstack = tileentityfurnace.GetStackInSlot(i);

						if (itemstack == null)
						{
							continue;
						}

						float f = FurnaceRand.NextFloat() * 0.8F + 0.1F;
						float f1 = FurnaceRand.NextFloat() * 0.8F + 0.1F;
						float f2 = FurnaceRand.NextFloat() * 0.8F + 0.1F;

						do
						{
							if (itemstack.StackSize <= 0)
							{
								goto label0;
							}

							int j = FurnaceRand.Next(21) + 10;

							if (j > itemstack.StackSize)
							{
								j = itemstack.StackSize;
							}

							itemstack.StackSize -= j;
							EntityItem entityitem = new EntityItem(par1World, (float)par2 + f, (float)par3 + f1, (float)par4 + f2, new ItemStack(itemstack.ItemID, j, itemstack.GetItemDamage()));

							if (itemstack.HasTagCompound())
							{
								entityitem.ItemStack.SetTagCompound((NBTTagCompound)itemstack.GetTagCompound().Copy());
							}

							float f3 = 0.05F;
							entityitem.MotionX = (float)FurnaceRand.NextGaussian() * f3;
							entityitem.MotionY = (float)FurnaceRand.NextGaussian() * f3 + 0.2F;
							entityitem.MotionZ = (float)FurnaceRand.NextGaussian() * f3;
							par1World.SpawnEntityInWorld(entityitem);
						}
						while (true);
					}
				}
			}

			base.OnBlockRemoval(par1World, par2, par3, par4);
		}
	}
}