using System;

namespace net.minecraft.src
{
	public class ItemBucket : Item
	{
		/// <summary>
		/// field for checking if the bucket has been filled. </summary>
		private int IsFull;

		public ItemBucket(int par1, int par2) : base(par1)
		{
			MaxStackSize = 1;
			IsFull = par2;
		}

		/// <summary>
		/// Called whenever this item is equipped and the right mouse button is pressed. Args: itemStack, world, entityPlayer
		/// </summary>
		public override ItemStack OnItemRightClick(ItemStack par1ItemStack, World par2World, EntityPlayer par3EntityPlayer)
		{
			float f = 1.0F;
			double d = par3EntityPlayer.PrevPosX + (par3EntityPlayer.PosX - par3EntityPlayer.PrevPosX) * (double)f;
			double d1 = (par3EntityPlayer.PrevPosY + (par3EntityPlayer.PosY - par3EntityPlayer.PrevPosY) * (double)f + 1.6200000000000001D) - (double)par3EntityPlayer.YOffset;
			double d2 = par3EntityPlayer.PrevPosZ + (par3EntityPlayer.PosZ - par3EntityPlayer.PrevPosZ) * (double)f;
			bool flag = IsFull == 0;
			MovingObjectPosition movingobjectposition = GetMovingObjectPositionFromPlayer(par2World, par3EntityPlayer, flag);

			if (movingobjectposition == null)
			{
				return par1ItemStack;
			}

			if (movingobjectposition.TypeOfHit == EnumMovingObjectType.TILE)
			{
				int i = movingobjectposition.BlockX;
				int j = movingobjectposition.BlockY;
				int k = movingobjectposition.BlockZ;

				if (!par2World.CanMineBlock(par3EntityPlayer, i, j, k))
				{
					return par1ItemStack;
				}

				if (IsFull == 0)
				{
					if (!par3EntityPlayer.CanPlayerEdit(i, j, k))
					{
						return par1ItemStack;
					}

					if (par2World.GetBlockMaterial(i, j, k) == Material.Water && par2World.GetBlockMetadata(i, j, k) == 0)
					{
						par2World.SetBlockWithNotify(i, j, k, 0);

						if (par3EntityPlayer.Capabilities.IsCreativeMode)
						{
							return par1ItemStack;
						}
						else
						{
							return new ItemStack(Item.BucketWater);
						}
					}

					if (par2World.GetBlockMaterial(i, j, k) == Material.Lava && par2World.GetBlockMetadata(i, j, k) == 0)
					{
						par2World.SetBlockWithNotify(i, j, k, 0);

						if (par3EntityPlayer.Capabilities.IsCreativeMode)
						{
							return par1ItemStack;
						}
						else
						{
							return new ItemStack(Item.BucketLava);
						}
					}
				}
				else
				{
					if (IsFull < 0)
					{
						return new ItemStack(Item.BucketEmpty);
					}

					if (movingobjectposition.SideHit == 0)
					{
						j--;
					}

					if (movingobjectposition.SideHit == 1)
					{
						j++;
					}

					if (movingobjectposition.SideHit == 2)
					{
						k--;
					}

					if (movingobjectposition.SideHit == 3)
					{
						k++;
					}

					if (movingobjectposition.SideHit == 4)
					{
						i--;
					}

					if (movingobjectposition.SideHit == 5)
					{
						i++;
					}

					if (!par3EntityPlayer.CanPlayerEdit(i, j, k))
					{
						return par1ItemStack;
					}

					if (par2World.IsAirBlock(i, j, k) || !par2World.GetBlockMaterial(i, j, k).IsSolid())
					{
						if (par2World.WorldProvider.IsHellWorld && IsFull == Block.WaterMoving.BlockID)
						{
							par2World.PlaySoundEffect(d + 0.5D, d1 + 0.5D, d2 + 0.5D, "random.fizz", 0.5F, 2.6F + (par2World.Rand.NextFloat() - par2World.Rand.NextFloat()) * 0.8F);

							for (int l = 0; l < 8; l++)
							{
								par2World.SpawnParticle("largesmoke", (double)i + (new Random(1)).NextDouble(), (double)j + new Random(2).NextDouble(), (double)k + new Random(3).NextDouble(), 0.0F, 0.0F, 0.0F);
							}
						}
						else
						{
							par2World.SetBlockAndMetadataWithNotify(i, j, k, IsFull, 0);
						}

						if (par3EntityPlayer.Capabilities.IsCreativeMode)
						{
							return par1ItemStack;
						}
						else
						{
							return new ItemStack(Item.BucketEmpty);
						}
					}
				}
			}
			else if (IsFull == 0 && (movingobjectposition.EntityHit is EntityCow))
			{
				return new ItemStack(Item.BucketMilk);
			}

			return par1ItemStack;
		}
	}
}