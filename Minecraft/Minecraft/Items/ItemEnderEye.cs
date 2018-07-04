namespace net.minecraft.src
{
	public class ItemEnderEye : Item
	{
		public ItemEnderEye(int par1) : base(par1)
		{
		}

		/// <summary>
		/// Callback for item usage. If the item does something special on right clicking, he will have one of those. Return
		/// True if something happen and false if it don't. This is for ITEMS, not BLOCKS !
		/// </summary>
		public override bool OnItemUse(ItemStack par1ItemStack, EntityPlayer par2EntityPlayer, World par3World, int par4, int par5, int par6, int par7)
		{
			int i = par3World.GetBlockId(par4, par5, par6);
			int j = par3World.GetBlockMetadata(par4, par5, par6);

			if (par2EntityPlayer.CanPlayerEdit(par4, par5, par6) && i == Block.EndPortalFrame.BlockID && !BlockEndPortalFrame.IsEnderEyeInserted(j))
			{
				if (par3World.IsRemote)
				{
					return true;
				}

				par3World.SetBlockMetadataWithNotify(par4, par5, par6, j + 4);
				par1ItemStack.StackSize--;

				for (int k = 0; k < 16; k++)
				{
                    float d = par4 + (5F + ItemRand.NextFloat() * 6F) / 16F;
                    float d1 = par5 + 0.8125F;
                    float d2 = par6 + (5F + ItemRand.NextFloat() * 6F) / 16F;
                    float d3 = 0.0F;
                    float d4 = 0.0F;
                    float d5 = 0.0F;
					par3World.SpawnParticle("smoke", d, d1, d2, d3, d4, d5);
				}

				int l = j & 3;
				int i1 = 0;
				int j1 = 0;
				bool flag = false;
				bool flag1 = true;
				int k1 = Direction.EnderEyeMetaToDirection[l];

				for (int l1 = -2; l1 <= 2; l1++)
				{
					int l2 = par4 + Direction.OffsetX[k1] * l1;
					int l3 = par6 + Direction.OffsetZ[k1] * l1;
					int l4 = par3World.GetBlockId(l2, par5, l3);

					if (l4 != Block.EndPortalFrame.BlockID)
					{
						continue;
					}

					int l5 = par3World.GetBlockMetadata(l2, par5, l3);

					if (!BlockEndPortalFrame.IsEnderEyeInserted(l5))
					{
						flag1 = false;
						break;
					}

					if (!flag)
					{
						i1 = l1;
						j1 = l1;
						flag = true;
					}
					else
					{
						j1 = l1;
					}
				}

				if (flag1 && j1 == i1 + 2)
				{
					int i2 = i1;

					do
					{
						if (i2 > j1)
						{
							break;
						}

						int i3 = par4 + Direction.OffsetX[k1] * i2;
						int i4 = par6 + Direction.OffsetZ[k1] * i2;
						i3 += Direction.OffsetX[l] * 4;
						i4 += Direction.OffsetZ[l] * 4;
						int i5 = par3World.GetBlockId(i3, par5, i4);
						int i6 = par3World.GetBlockMetadata(i3, par5, i4);

						if (i5 != Block.EndPortalFrame.BlockID || !BlockEndPortalFrame.IsEnderEyeInserted(i6))
						{
							flag1 = false;
							break;
						}

						i2++;
					}
					while (true);

					label0:

					for (int j2 = i1 - 1; j2 <= j1 + 1; j2 += 4)
					{
						int j3 = 1;

						do
						{
							if (j3 > 3)
							{
								goto label0;
							}

							int j4 = par4 + Direction.OffsetX[k1] * j2;
							int j5 = par6 + Direction.OffsetZ[k1] * j2;
							j4 += Direction.OffsetX[l] * j3;
							j5 += Direction.OffsetZ[l] * j3;
							int j6 = par3World.GetBlockId(j4, par5, j5);
							int k6 = par3World.GetBlockMetadata(j4, par5, j5);

							if (j6 != Block.EndPortalFrame.BlockID || !BlockEndPortalFrame.IsEnderEyeInserted(k6))
							{
								flag1 = false;
								goto label0;
							}

							j3++;
						}
						while (true);
					}

					if (flag1)
					{
						for (int k2 = i1; k2 <= j1; k2++)
						{
							for (int k3 = 1; k3 <= 3; k3++)
							{
								int k4 = par4 + Direction.OffsetX[k1] * k2;
								int k5 = par6 + Direction.OffsetZ[k1] * k2;
								k4 += Direction.OffsetX[l] * k3;
								k5 += Direction.OffsetZ[l] * k3;
								par3World.SetBlockWithNotify(k4, par5, k5, Block.EndPortal.BlockID);
							}
						}
					}
				}

				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Called whenever this item is equipped and the right mouse button is pressed. Args: itemStack, world, entityPlayer
		/// </summary>
		public override ItemStack OnItemRightClick(ItemStack par1ItemStack, World par2World, EntityPlayer par3EntityPlayer)
		{
			MovingObjectPosition movingobjectposition = GetMovingObjectPositionFromPlayer(par2World, par3EntityPlayer, false);

			if (movingobjectposition != null && movingobjectposition.TypeOfHit == EnumMovingObjectType.TILE)
			{
				int i = par2World.GetBlockId(movingobjectposition.BlockX, movingobjectposition.BlockY, movingobjectposition.BlockZ);

				if (i == Block.EndPortalFrame.BlockID)
				{
					return par1ItemStack;
				}
			}

			if (!par2World.IsRemote)
			{
				ChunkPosition chunkposition = par2World.FindClosestStructure("Stronghold", (int)par3EntityPlayer.PosX, (int)par3EntityPlayer.PosY, (int)par3EntityPlayer.PosZ);

				if (chunkposition != null)
				{
					EntityEnderEye entityendereye = new EntityEnderEye(par2World, par3EntityPlayer.PosX, (par3EntityPlayer.PosY + 1.6200000000000001F) - par3EntityPlayer.YOffset, par3EntityPlayer.PosZ);
					entityendereye.Func_40090_a(chunkposition.x, chunkposition.y, chunkposition.z);
					par2World.SpawnEntityInWorld(entityendereye);
					par2World.PlaySoundAtEntity(par3EntityPlayer, "random.bow", 0.5F, 0.4F / (ItemRand.NextFloat() * 0.4F + 0.8F));
					par2World.PlayAuxSFXAtEntity(null, 1002, (int)par3EntityPlayer.PosX, (int)par3EntityPlayer.PosY, (int)par3EntityPlayer.PosZ, 0);

					if (!par3EntityPlayer.Capabilities.IsCreativeMode)
					{
						par1ItemStack.StackSize--;
					}
				}
			}

			return par1ItemStack;
		}
	}
}