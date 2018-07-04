using System;

namespace net.minecraft.src
{
	public class BlockDispenser : BlockContainer
	{
		private Random Random;

        public BlockDispenser(int par1)
            : base(par1, Material.Rock)
		{
			Random = new Random();
			BlockIndexInTexture = 45;
		}

		/// <summary>
		/// How many world ticks before ticking
		/// </summary>
		public override int TickRate()
		{
			return 4;
		}

		/// <summary>
		/// Returns the ID of the items to drop on destruction.
		/// </summary>
		public override int IdDropped(int par1, Random par2Random, int par3)
		{
			return Block.Dispenser.BlockID;
		}

		/// <summary>
		/// Called whenever the block is added into the world. Args: world, x, y, z
		/// </summary>
		public override void OnBlockAdded(World par1World, int par2, int par3, int par4)
		{
			base.OnBlockAdded(par1World, par2, par3, par4);
			SetDispenserDefaultDirection(par1World, par2, par3, par4);
		}

		/// <summary>
		/// sets Dispenser block direction so that the front faces an non-opaque block; chooses west to be direction if all
		/// surrounding blocks are opaque.
		/// </summary>
		private void SetDispenserDefaultDirection(World par1World, int par2, int par3, int par4)
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
			else
			{
				return BlockIndexInTexture + 1;
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
				return BlockIndexInTexture + 1;
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

			TileEntityDispenser tileentitydispenser = (TileEntityDispenser)par1World.GetBlockTileEntity(par2, par3, par4);

			if (tileentitydispenser != null)
			{
				par5EntityPlayer.DisplayGUIDispenser(tileentitydispenser);
			}

			return true;
		}

		/// <summary>
		/// dispenses an item from a randomly selected item stack from the blocks inventory into the game world.
		/// </summary>
		private void DispenseItem(World par1World, int par2, int par3, int par4, Random par5Random)
		{
			int i = par1World.GetBlockMetadata(par2, par3, par4);
			int j = 0;
			int k = 0;

			if (i == 3)
			{
				k = 1;
			}
			else if (i == 2)
			{
				k = -1;
			}
			else if (i == 5)
			{
				j = 1;
			}
			else
			{
				j = -1;
			}

			TileEntityDispenser tileentitydispenser = (TileEntityDispenser)par1World.GetBlockTileEntity(par2, par3, par4);

			if (tileentitydispenser != null)
			{
				ItemStack itemstack = tileentitydispenser.GetRandomStackFromInventory();
                float d = par2 + j * 0.59999999999999998F + 0.5F;
                float d1 = par3 + 0.5F;
                float d2 = par4 + k * 0.59999999999999998F + 0.5F;

				if (itemstack == null)
				{
					par1World.PlayAuxSFX(1001, par2, par3, par4, 0);
				}
				else
				{
					if (itemstack.ItemID == Item.Arrow.ShiftedIndex)
					{
						EntityArrow entityarrow = new EntityArrow(par1World, d, d1, d2);
						entityarrow.SetArrowHeading(j, 0.10000000149011612F, k, 1.1F, 6F);
						entityarrow.DoesArrowBelongToPlayer = true;
						par1World.SpawnEntityInWorld(entityarrow);
						par1World.PlayAuxSFX(1002, par2, par3, par4, 0);
					}
					else if (itemstack.ItemID == Item.Egg.ShiftedIndex)
					{
						EntityEgg entityegg = new EntityEgg(par1World, d, d1, d2);
						entityegg.SetThrowableHeading(j, 0.10000000149011612F, k, 1.1F, 6F);
						par1World.SpawnEntityInWorld(entityegg);
						par1World.PlayAuxSFX(1002, par2, par3, par4, 0);
					}
					else if (itemstack.ItemID == Item.Snowball.ShiftedIndex)
					{
						EntitySnowball entitysnowball = new EntitySnowball(par1World, d, d1, d2);
						entitysnowball.SetThrowableHeading(j, 0.10000000149011612F, k, 1.1F, 6F);
						par1World.SpawnEntityInWorld(entitysnowball);
						par1World.PlayAuxSFX(1002, par2, par3, par4, 0);
					}
					else if (itemstack.ItemID == Item.Potion.ShiftedIndex && ItemPotion.IsSplash(itemstack.GetItemDamage()))
					{
						EntityPotion entitypotion = new EntityPotion(par1World, d, d1, d2, itemstack.GetItemDamage());
						entitypotion.SetThrowableHeading(j, 0.10000000149011612F, k, 1.375F, 3F);
						par1World.SpawnEntityInWorld(entitypotion);
						par1World.PlayAuxSFX(1002, par2, par3, par4, 0);
					}
					else if (itemstack.ItemID == Item.ExpBottle.ShiftedIndex)
					{
						EntityExpBottle entityexpbottle = new EntityExpBottle(par1World, d, d1, d2);
						entityexpbottle.SetThrowableHeading(j, 0.10000000149011612F, k, 1.375F, 3F);
						par1World.SpawnEntityInWorld(entityexpbottle);
						par1World.PlayAuxSFX(1002, par2, par3, par4, 0);
					}
					else if (itemstack.ItemID == Item.MonsterPlacer.ShiftedIndex)
					{
						ItemMonsterPlacer.Func_48440_a(par1World, itemstack.GetItemDamage(), d + j * 0.29999999999999999F, d1 - 0.29999999999999999F, d2 + k * 0.29999999999999999F);
						par1World.PlayAuxSFX(1002, par2, par3, par4, 0);
					}
					else if (itemstack.ItemID == Item.FireballCharge.ShiftedIndex)
					{
						EntitySmallFireball entitysmallfireball = new EntitySmallFireball(par1World, d + j * 0.29999999999999999F, d1, d2 + k * 0.29999999999999999F, j + par5Random.NextGaussian() * 0.050000000000000003F, par5Random.NextGaussian() * 0.050000000000000003F, k + par5Random.NextGaussian() * 0.050000000000000003F);
						par1World.SpawnEntityInWorld(entitysmallfireball);
						par1World.PlayAuxSFX(1009, par2, par3, par4, 0);
					}
					else
					{
						EntityItem entityitem = new EntityItem(par1World, d, d1 - 0.29999999999999999F, d2, itemstack);
                        float d3 = par5Random.NextFloat() * 0.10000000000000001F + 0.20000000000000001F;
						entityitem.MotionX = j * d3;
						entityitem.MotionY = 0.20000000298023224F;
						entityitem.MotionZ = k * d3;
						entityitem.MotionX += par5Random.NextGaussian() * 0.0074999998323619366F * 6;
						entityitem.MotionY += par5Random.NextGaussian() * 0.0074999998323619366F * 6;
						entityitem.MotionZ += par5Random.NextGaussian() * 0.0074999998323619366F * 6;
						par1World.SpawnEntityInWorld(entityitem);
						par1World.PlayAuxSFX(1000, par2, par3, par4, 0);
					}

					par1World.PlayAuxSFX(2000, par2, par3, par4, j + 1 + (k + 1) * 3);
				}
			}
		}

		/// <summary>
		/// Lets the block know when one of its neighbor changes. Doesn't know which neighbor changed (coordinates passed are
		/// their own) Args: x, y, z, neighbor BlockID
		/// </summary>
		public override void OnNeighborBlockChange(World par1World, int par2, int par3, int par4, int par5)
		{
			if (par5 > 0 && Block.BlocksList[par5].CanProvidePower())
			{
				bool flag = par1World.IsBlockIndirectlyGettingPowered(par2, par3, par4) || par1World.IsBlockIndirectlyGettingPowered(par2, par3 + 1, par4);

				if (flag)
				{
					par1World.ScheduleBlockUpdate(par2, par3, par4, BlockID, TickRate());
				}
			}
		}

		/// <summary>
		/// Ticks the block if it's been scheduled
		/// </summary>
		public override void UpdateTick(World par1World, int par2, int par3, int par4, Random par5Random)
		{
			if (!par1World.IsRemote && (par1World.IsBlockIndirectlyGettingPowered(par2, par3, par4) || par1World.IsBlockIndirectlyGettingPowered(par2, par3 + 1, par4)))
			{
				DispenseItem(par1World, par2, par3, par4, par5Random);
			}
		}

		/// <summary>
		/// Returns the TileEntity used by this block.
		/// </summary>
		public override TileEntity GetBlockEntity()
		{
			return new TileEntityDispenser();
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
			TileEntityDispenser tileentitydispenser = (TileEntityDispenser)par1World.GetBlockTileEntity(par2, par3, par4);

			if (tileentitydispenser != null)
			{
				label0:

				for (int i = 0; i < tileentitydispenser.GetSizeInventory(); i++)
				{
					ItemStack itemstack = tileentitydispenser.GetStackInSlot(i);

					if (itemstack == null)
					{
						continue;
					}

					float f = Random.NextFloat() * 0.8F + 0.1F;
					float f1 = Random.NextFloat() * 0.8F + 0.1F;
					float f2 = Random.NextFloat() * 0.8F + 0.1F;

					do
					{
						if (itemstack.StackSize <= 0)
						{
							goto label0;
						}

						int j = Random.Next(21) + 10;

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
						entityitem.MotionX = (float)Random.NextGaussian() * f3;
						entityitem.MotionY = (float)Random.NextGaussian() * f3 + 0.2F;
						entityitem.MotionZ = (float)Random.NextGaussian() * f3;
						par1World.SpawnEntityInWorld(entityitem);
					}
					while (true);
				}
			}

			base.OnBlockRemoval(par1World, par2, par3, par4);
		}
	}
}