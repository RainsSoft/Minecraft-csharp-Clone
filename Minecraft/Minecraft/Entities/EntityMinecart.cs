using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class EntityMinecart : Entity, IInventory
	{
		private ItemStack[] CargoItems;
		private int Fuel;
		private bool Field_856_i;

		/// <summary>
		/// The type of minecart, 2 for powered, 1 for storage. </summary>
		public int MinecartType;
        public float PushX;
        public float PushZ;
		private static readonly int[][][] Field_855_j = { new int[][] { new int[] { 0, 0, -1 }, new int[] { 0, 0, 1 } }, new int[][] { new int[] { -1, 0, 0 }, new int[] { 1, 0, 0 } }, new int[][] { new int[] { -1, -1, 0 }, new int[] { 1, 0, 0 } }, new int[][] { new int[] { -1, 0, 0 }, new int[] { 1, -1, 0 } }, new int[][] { new int[] { 0, 0, -1 }, new int[] { 0, -1, 1 } }, new int[][] { new int[] { 0, -1, -1 }, new int[] { 0, 0, 1 } }, new int[][] { new int[] { 0, 0, 1 }, new int[] { 1, 0, 0 } }, new int[][] { new int[] { 0, 0, 1 }, new int[] { -1, 0, 0 } }, new int[][] { new int[] { 0, 0, -1 }, new int[] { -1, 0, 0 } }, new int[][] { new int[] { 0, 0, -1 }, new int[] { 1, 0, 0 } } };

		/// <summary>
		/// appears to be the progress of the turn </summary>
		private int TurnProgress;
        private float MinecartX;
        private float MinecartY;
        private float MinecartZ;
        private float MinecartYaw;
        private float MinecartPitch;
        private float VelocityX;
        private float VelocityY;
        private float VelocityZ;

		public EntityMinecart(World par1World) : base(par1World)
		{
			CargoItems = new ItemStack[36];
			Fuel = 0;
			Field_856_i = false;
			PreventEntitySpawning = true;
			SetSize(0.98F, 0.7F);
			YOffset = Height / 2.0F;
		}

		/// <summary>
		/// returns if this entity triggers Block.onEntityWalking on the blocks they walk on. used for spiders and wolves to
		/// prevent them from trampling crops
		/// </summary>
		protected override bool CanTriggerWalking()
		{
			return false;
		}

		protected override void EntityInit()
		{
			DataWatcher.AddObject(16, new byte?((byte)0));
			DataWatcher.AddObject(17, new int?(0));
			DataWatcher.AddObject(18, new int?(1));
			DataWatcher.AddObject(19, new int?(0));
		}

		/// <summary>
		/// Returns a boundingBox used to collide the entity with other entities and blocks. This enables the entity to be
		/// pushable on contact, like boats or minecarts.
		/// </summary>
		public override AxisAlignedBB GetCollisionBox(Entity par1Entity)
		{
			return par1Entity.BoundingBox;
		}

		/// <summary>
		/// returns the bounding box for this entity
		/// </summary>
		public override AxisAlignedBB GetBoundingBox()
		{
			return null;
		}

		/// <summary>
		/// Returns true if this entity should push and be pushed by other entities when colliding.
		/// </summary>
		public override bool CanBePushed()
		{
			return true;
		}

        public EntityMinecart(World par1World, float par2, float par4, float par6, int par8)
            : this(par1World)
		{
			SetPosition(par2, par4 + YOffset, par6);
			MotionX = 0.0F;
			MotionY = 0.0F;
			MotionZ = 0.0F;
			PrevPosX = par2;
			PrevPosY = par4;
			PrevPosZ = par6;
			MinecartType = par8;
		}

		/// <summary>
		/// Returns the Y offset from the entity's position for any entity riding this one.
		/// </summary>
        public override float GetMountedYOffset()
		{
			return Height * 0.0F - 0.30000001192092896F;
		}

		/// <summary>
		/// Called when the entity is attacked.
		/// </summary>
		public override bool AttackEntityFrom(DamageSource par1DamageSource, int par2)
		{
			if (WorldObj.IsRemote || IsDead)
			{
				return true;
			}

			Func_41029_h(-Func_41030_m());
			Func_41028_c(10);
			SetBeenAttacked();
			Func_41024_b(Func_41025_i() + par2 * 10);

			if (Func_41025_i() > 40)
			{
				if (RiddenByEntity != null)
				{
					RiddenByEntity.MountEntity(this);
				}

				SetDead();
				DropItemWithOffset(Item.MinecartEmpty.ShiftedIndex, 1, 0.0F);

				if (MinecartType == 1)
				{
					EntityMinecart entityminecart = this;
					label0:

					for (int i = 0; i < entityminecart.GetSizeInventory(); i++)
					{
						ItemStack itemstack = entityminecart.GetStackInSlot(i);

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
							EntityItem entityitem = new EntityItem(WorldObj, PosX + f, PosY + f1, PosZ + f2, new ItemStack(itemstack.ItemID, j, itemstack.GetItemDamage()));
							float f3 = 0.05F;
							entityitem.MotionX = (float)Rand.NextGaussian() * f3;
							entityitem.MotionY = (float)Rand.NextGaussian() * f3 + 0.2F;
							entityitem.MotionZ = (float)Rand.NextGaussian() * f3;
							WorldObj.SpawnEntityInWorld(entityitem);
						}
						while (true);
					}

					DropItemWithOffset(Block.Chest.BlockID, 1, 0.0F);
				}
				else if (MinecartType == 2)
				{
					DropItemWithOffset(Block.StoneOvenIdle.BlockID, 1, 0.0F);
				}
			}

			return true;
		}

		/// <summary>
		/// Setups the entity to do the hurt animation. Only used by packets in multiplayer.
		/// </summary>
		public override void PerformHurtAnimation()
		{
			Func_41029_h(-Func_41030_m());
			Func_41028_c(10);
			Func_41024_b(Func_41025_i() + Func_41025_i() * 10);
		}

		/// <summary>
		/// Returns true if other Entities should be prevented from moving through this Entity.
		/// </summary>
		public override bool CanBeCollidedWith()
		{
			return !IsDead;
		}

		/// <summary>
		/// Will get destroyed next tick.
		/// </summary>
		public override void SetDead()
		{
			label0:

			for (int i = 0; i < GetSizeInventory(); i++)
			{
				ItemStack itemstack = GetStackInSlot(i);

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
					EntityItem entityitem = new EntityItem(WorldObj, PosX + f, PosY + f1, PosZ + f2, new ItemStack(itemstack.ItemID, j, itemstack.GetItemDamage()));

					if (itemstack.HasTagCompound())
					{
						entityitem.ItemStack.SetTagCompound((NBTTagCompound)itemstack.GetTagCompound().Copy());
					}

					float f3 = 0.05F;
					entityitem.MotionX = (float)Rand.NextGaussian() * f3;
					entityitem.MotionY = (float)Rand.NextGaussian() * f3 + 0.2F;
					entityitem.MotionZ = (float)Rand.NextGaussian() * f3;
					WorldObj.SpawnEntityInWorld(entityitem);
				}
				while (true);
			}

			base.SetDead();
		}

		/// <summary>
		/// Called to update the entity's position/logic.
		/// </summary>
		public override void OnUpdate()
		{
			if (Func_41023_l() > 0)
			{
				Func_41028_c(Func_41023_l() - 1);
			}

			if (Func_41025_i() > 0)
			{
				Func_41024_b(Func_41025_i() - 1);
			}

			if (PosY < -64D)
			{
				Kill();
			}

			if (IsMinecartPowered() && Rand.Next(4) == 0)
			{
				WorldObj.SpawnParticle("largesmoke", PosX, PosY + 0.80000000000000004D, PosZ, 0.0F, 0.0F, 0.0F);
			}

			if (WorldObj.IsRemote)
			{
				if (TurnProgress > 0)
				{
                    float d = PosX + (MinecartX - PosX) / TurnProgress;
                    float d1 = PosY + (MinecartY - PosY) / TurnProgress;
                    float d3 = PosZ + (MinecartZ - PosZ) / TurnProgress;
					double d5;

					for (d5 = MinecartYaw - RotationYaw; d5 < -180D; d5 += 360D)
					{
					}

					for (; d5 >= 180D; d5 -= 360D)
					{
					}

					RotationYaw += (float)d5 / TurnProgress;
					RotationPitch += ((float)MinecartPitch - RotationPitch) / TurnProgress;
					TurnProgress--;
					SetPosition(d, d1, d3);
					SetRotation(RotationYaw, RotationPitch);
				}
				else
				{
					SetPosition(PosX, PosY, PosZ);
					SetRotation(RotationYaw, RotationPitch);
				}

				return;
			}

			PrevPosX = PosX;
			PrevPosY = PosY;
			PrevPosZ = PosZ;
			MotionY -= 0.039999999105930328F;
			int i = MathHelper2.Floor_double(PosX);
			int j = MathHelper2.Floor_double(PosY);
			int k = MathHelper2.Floor_double(PosZ);

			if (BlockRail.IsRailBlockAt(WorldObj, i, j - 1, k))
			{
				j--;
			}

            float d2 = 0.40000000000000002F;
            float d4 = 0.0078125F;
			int l = WorldObj.GetBlockId(i, j, k);

			if (BlockRail.IsRailBlock(l))
			{
				Vec3D vec3d = Func_514_g(PosX, PosY, PosZ);
				int i1 = WorldObj.GetBlockMetadata(i, j, k);
				PosY = j;
				bool flag = false;
				bool flag1 = false;

				if (l == Block.RailPowered.BlockID)
				{
					flag = (i1 & 8) != 0;
					flag1 = !flag;
				}

				if (((BlockRail)Block.BlocksList[l]).IsPowered())
				{
					i1 &= 7;
				}

				if (i1 >= 2 && i1 <= 5)
				{
					PosY = j + 1;
				}

				if (i1 == 2)
				{
					MotionX -= d4;
				}

				if (i1 == 3)
				{
					MotionX += d4;
				}

				if (i1 == 4)
				{
					MotionZ += d4;
				}

				if (i1 == 5)
				{
					MotionZ -= d4;
				}

				int[][] ai = Field_855_j[i1];
                float d9 = ai[1][0] - ai[0][0];
                float d10 = ai[1][2] - ai[0][2];
                float d11 = (float)Math.Sqrt(d9 * d9 + d10 * d10);
				double d12 = MotionX * d9 + MotionZ * d10;

				if (d12 < 0.0F)
				{
					d9 = -d9;
					d10 = -d10;
				}

                float d13 = (float)Math.Sqrt(MotionX * MotionX + MotionZ * MotionZ);
				MotionX = (d13 * d9) / d11;
				MotionZ = (d13 * d10) / d11;

				if (flag1)
				{
					double d16 = Math.Sqrt(MotionX * MotionX + MotionZ * MotionZ);

					if (d16 < 0.029999999999999999D)
					{
						MotionX *= 0.0F;
						MotionY *= 0.0F;
						MotionZ *= 0.0F;
					}
					else
					{
						MotionX *= 0.5F;
						MotionY *= 0.0F;
						MotionZ *= 0.5F;
					}
				}

                float d17 = 0.0F;
                float d18 = i + 0.5F + ai[0][0] * 0.5F;
                float d19 = k + 0.5F + ai[0][2] * 0.5F;
                float d20 = i + 0.5F + ai[1][0] * 0.5F;
                float d21 = k + 0.5F + ai[1][2] * 0.5F;
				d9 = d20 - d18;
				d10 = d21 - d19;

				if (d9 == 0.0F)
				{
					PosX = i + 0.5F;
					d17 = PosZ - k;
				}
				else if (d10 == 0.0F)
				{
					PosZ = k + 0.5F;
					d17 = PosX - i;
				}
				else
				{
                    float d22 = PosX - d18;
                    float d24 = PosZ - d19;
                    float d26 = (d22 * d9 + d24 * d10) * 2F;
					d17 = d26;
				}

				PosX = d18 + d9 * d17;
				PosZ = d19 + d10 * d17;
				SetPosition(PosX, PosY + YOffset, PosZ);
                float d23 = MotionX;
                float d25 = MotionZ;

				if (RiddenByEntity != null)
				{
					d23 *= 0.75F;
					d25 *= 0.75F;
				}

				if (d23 < -d2)
				{
					d23 = -d2;
				}

				if (d23 > d2)
				{
					d23 = d2;
				}

				if (d25 < -d2)
				{
					d25 = -d2;
				}

				if (d25 > d2)
				{
					d25 = d2;
				}

				MoveEntity(d23, 0.0F, d25);

				if (ai[0][1] != 0 && MathHelper2.Floor_double(PosX) - i == ai[0][0] && MathHelper2.Floor_double(PosZ) - k == ai[0][2])
				{
					SetPosition(PosX, PosY + ai[0][1], PosZ);
				}
				else if (ai[1][1] != 0 && MathHelper2.Floor_double(PosX) - i == ai[1][0] && MathHelper2.Floor_double(PosZ) - k == ai[1][2])
				{
					SetPosition(PosX, PosY + ai[1][1], PosZ);
				}

				if (RiddenByEntity != null)
				{
					MotionX *= 0.99699997901916504F;
					MotionY *= 0.0F;
					MotionZ *= 0.99699997901916504F;
				}
				else
				{
					if (MinecartType == 2)
					{
                        float d27 = MathHelper2.Sqrt_double(PushX * PushX + PushZ * PushZ);

						if (d27 > 0.01D)
						{
							PushX /= d27;
							PushZ /= d27;
                            float d29 = 0.040000000000000001F;
							MotionX *= 0.80000001192092896F;
							MotionY *= 0.0F;
							MotionZ *= 0.80000001192092896F;
							MotionX += PushX * d29;
							MotionZ += PushZ * d29;
						}
						else
						{
							MotionX *= 0.89999997615814209F;
							MotionY *= 0.0F;
							MotionZ *= 0.89999997615814209F;
						}
					}

					MotionX *= 0.95999997854232788F;
					MotionY *= 0.0F;
					MotionZ *= 0.95999997854232788F;
				}

				Vec3D vec3d1 = Func_514_g(PosX, PosY, PosZ);

				if (vec3d1 != null && vec3d != null)
				{
                    float d28 = ((float)vec3d.YCoord - (float)vec3d1.YCoord) * 0.050000000000000003F;
                    float d14 = (float)Math.Sqrt(MotionX * MotionX + MotionZ * MotionZ);

					if (d14 > 0.0F)
					{
						MotionX = (MotionX / d14) * (d14 + d28);
						MotionZ = (MotionZ / d14) * (d14 + d28);
					}

                    SetPosition(PosX, (float)vec3d1.YCoord, PosZ);
				}

				int k1 = MathHelper2.Floor_double(PosX);
				int l1 = MathHelper2.Floor_double(PosZ);

				if (k1 != i || l1 != k)
				{
                    float d15 = (float)Math.Sqrt(MotionX * MotionX + MotionZ * MotionZ);
					MotionX = d15 * (k1 - i);
					MotionZ = d15 * (l1 - k);
				}

				if (MinecartType == 2)
				{
                    float d30 = MathHelper2.Sqrt_double(PushX * PushX + PushZ * PushZ);

					if (d30 > 0.01F && MotionX * MotionX + MotionZ * MotionZ > 0.001F)
					{
						PushX /= d30;
						PushZ /= d30;

						if (PushX * MotionX + PushZ * MotionZ < 0.0F)
						{
							PushX = 0.0F;
							PushZ = 0.0F;
						}
						else
						{
							PushX = MotionX;
							PushZ = MotionZ;
						}
					}
				}

				if (flag)
				{
                    float d31 = (float)Math.Sqrt(MotionX * MotionX + MotionZ * MotionZ);

					if (d31 > 0.01D)
					{
                        float d32 = 0.059999999999999998F;
						MotionX += (MotionX / d31) * d32;
						MotionZ += (MotionZ / d31) * d32;
					}
					else if (i1 == 1)
					{
						if (WorldObj.IsBlockNormalCube(i - 1, j, k))
						{
							MotionX = 0.02F;
						}
						else if (WorldObj.IsBlockNormalCube(i + 1, j, k))
						{
							MotionX = -0.02F;
						}
					}
					else if (i1 == 0)
					{
						if (WorldObj.IsBlockNormalCube(i, j, k - 1))
						{
							MotionZ = 0.02F;
						}
						else if (WorldObj.IsBlockNormalCube(i, j, k + 1))
						{
							MotionZ = -0.02F;
						}
					}
				}
			}
			else
			{
				if (MotionX < -d2)
				{
					MotionX = -d2;
				}

				if (MotionX > d2)
				{
					MotionX = d2;
				}

				if (MotionZ < -d2)
				{
					MotionZ = -d2;
				}

				if (MotionZ > d2)
				{
					MotionZ = d2;
				}

				if (OnGround)
				{
					MotionX *= 0.5F;
					MotionY *= 0.5F;
					MotionZ *= 0.5F;
				}

				MoveEntity(MotionX, MotionY, MotionZ);

				if (!OnGround)
				{
					MotionX *= 0.94999998807907104F;
					MotionY *= 0.94999998807907104F;
					MotionZ *= 0.94999998807907104F;
				}
			}

			RotationPitch = 0.0F;
            float d6 = PrevPosX - PosX;
            float d7 = PrevPosZ - PosZ;

			if (d6 * d6 + d7 * d7 > 0.001F)
			{
				RotationYaw = (float)((Math.Atan2(d7, d6) * 180F) / Math.PI);

				if (Field_856_i)
				{
					RotationYaw += 180F;
				}
			}

            float d8;

			for (d8 = RotationYaw - PrevRotationYaw; d8 >= 180F; d8 -= 360F)
			{
			}

			for (; d8 < -180D; d8 += 360F)
			{
			}

			if (d8 < -170D || d8 >= 170D)
			{
				RotationYaw += 180F;
				Field_856_i = !Field_856_i;
			}

			SetRotation(RotationYaw, RotationPitch);
            List<Entity> list = WorldObj.GetEntitiesWithinAABBExcludingEntity(this, BoundingBox.Expand(0.20000000298023224F, 0.0F, 0.20000000298023224F));

			if (list != null && list.Count > 0)
			{
				for (int j1 = 0; j1 < list.Count; j1++)
				{
					Entity entity = list[j1];

					if (entity != RiddenByEntity && entity.CanBePushed() && (entity is EntityMinecart))
					{
						entity.ApplyEntityCollision(this);
					}
				}
			}

			if (RiddenByEntity != null && RiddenByEntity.IsDead)
			{
				if (RiddenByEntity.RidingEntity == this)
				{
					RiddenByEntity.RidingEntity = null;
				}

				RiddenByEntity = null;
			}

			if (Fuel > 0)
			{
				Fuel--;
			}

			if (Fuel <= 0)
			{
				PushX = PushZ = 0.0F;
			}

			SetMinecartPowered(Fuel > 0);
		}

		public virtual Vec3D Func_515_a(double par1, double par3, double par5, double par7)
		{
			int i = MathHelper2.Floor_double(par1);
			int j = MathHelper2.Floor_double(par3);
			int k = MathHelper2.Floor_double(par5);

			if (BlockRail.IsRailBlockAt(WorldObj, i, j - 1, k))
			{
				j--;
			}

			int l = WorldObj.GetBlockId(i, j, k);

			if (BlockRail.IsRailBlock(l))
			{
				int i1 = WorldObj.GetBlockMetadata(i, j, k);

				if (((BlockRail)Block.BlocksList[l]).IsPowered())
				{
					i1 &= 7;
				}

				par3 = j;

				if (i1 >= 2 && i1 <= 5)
				{
					par3 = j + 1;
				}

				int[][] ai = Field_855_j[i1];
                float d = ai[1][0] - ai[0][0];
                float d1 = ai[1][2] - ai[0][2];
                float d2 = (float)Math.Sqrt(d * d + d1 * d1);
				d /= d2;
				d1 /= d2;
				par1 += d * par7;
				par5 += d1 * par7;

				if (ai[0][1] != 0 && MathHelper2.Floor_double(par1) - i == ai[0][0] && MathHelper2.Floor_double(par5) - k == ai[0][2])
				{
					par3 += ai[0][1];
				}
				else if (ai[1][1] != 0 && MathHelper2.Floor_double(par1) - i == ai[1][0] && MathHelper2.Floor_double(par5) - k == ai[1][2])
				{
					par3 += ai[1][1];
				}

				return Func_514_g(par1, par3, par5);
			}
			else
			{
				return null;
			}
		}

		public virtual Vec3D Func_514_g(double par1, double par3, double par5)
		{
			int i = MathHelper2.Floor_double(par1);
			int j = MathHelper2.Floor_double(par3);
			int k = MathHelper2.Floor_double(par5);

			if (BlockRail.IsRailBlockAt(WorldObj, i, j - 1, k))
			{
				j--;
			}

			int l = WorldObj.GetBlockId(i, j, k);

			if (BlockRail.IsRailBlock(l))
			{
				int i1 = WorldObj.GetBlockMetadata(i, j, k);
				par3 = j;

				if (((BlockRail)Block.BlocksList[l]).IsPowered())
				{
					i1 &= 7;
				}

				if (i1 >= 2 && i1 <= 5)
				{
					par3 = j + 1;
				}

				int[][] ai = Field_855_j[i1];
				double d = 0.0F;
				double d1 = (double)i + 0.5D + (double)ai[0][0] * 0.5D;
				double d2 = (double)j + 0.5D + (double)ai[0][1] * 0.5D;
				double d3 = (double)k + 0.5D + (double)ai[0][2] * 0.5D;
				double d4 = (double)i + 0.5D + (double)ai[1][0] * 0.5D;
				double d5 = (double)j + 0.5D + (double)ai[1][1] * 0.5D;
				double d6 = (double)k + 0.5D + (double)ai[1][2] * 0.5D;
				double d7 = d4 - d1;
				double d8 = (d5 - d2) * 2D;
				double d9 = d6 - d3;

				if (d7 == 0.0F)
				{
					par1 = (double)i + 0.5D;
					d = par5 - (double)k;
				}
				else if (d9 == 0.0F)
				{
					par5 = (double)k + 0.5D;
					d = par1 - (double)i;
				}
				else
				{
					double d10 = par1 - d1;
					double d11 = par5 - d3;
					double d12 = (d10 * d7 + d11 * d9) * 2D;
					d = d12;
				}

				par1 = d1 + d7 * d;
				par3 = d2 + d8 * d;
				par5 = d3 + d9 * d;

				if (d8 < 0.0F)
				{
					par3++;
				}

				if (d8 > 0.0F)
				{
					par3 += 0.5D;
				}

				return Vec3D.CreateVector(par1, par3, par5);
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// (abstract) Protected helper method to write subclass entity data to NBT.
		/// </summary>
		public override void WriteEntityToNBT(NBTTagCompound par1NBTTagCompound)
		{
			par1NBTTagCompound.SetInteger("Type", MinecartType);

			if (MinecartType == 2)
			{
				par1NBTTagCompound.SetDouble("PushX", PushX);
				par1NBTTagCompound.SetDouble("PushZ", PushZ);
				par1NBTTagCompound.SetShort("Fuel", (short)Fuel);
			}
			else if (MinecartType == 1)
			{
				NBTTagList nbttaglist = new NBTTagList();

				for (int i = 0; i < CargoItems.Length; i++)
				{
					if (CargoItems[i] != null)
					{
						NBTTagCompound nbttagcompound = new NBTTagCompound();
						nbttagcompound.SetByte("Slot", (byte)i);
						CargoItems[i].WriteToNBT(nbttagcompound);
						nbttaglist.AppendTag(nbttagcompound);
					}
				}

				par1NBTTagCompound.SetTag("Items", nbttaglist);
			}
		}

		/// <summary>
		/// (abstract) Protected helper method to read subclass entity data from NBT.
		/// </summary>
		public override void ReadEntityFromNBT(NBTTagCompound par1NBTTagCompound)
		{
			MinecartType = par1NBTTagCompound.GetInteger("Type");

			if (MinecartType == 2)
			{
                PushX = (float)par1NBTTagCompound.GetDouble("PushX");
                PushZ = (float)par1NBTTagCompound.GetDouble("PushZ");
				Fuel = par1NBTTagCompound.GetShort("Fuel");
			}
			else if (MinecartType == 1)
			{
				NBTTagList nbttaglist = par1NBTTagCompound.GetTagList("Items");
				CargoItems = new ItemStack[GetSizeInventory()];

				for (int i = 0; i < nbttaglist.TagCount(); i++)
				{
					NBTTagCompound nbttagcompound = (NBTTagCompound)nbttaglist.TagAt(i);
					int j = nbttagcompound.GetByte("Slot") & 0xff;

					if (j >= 0 && j < CargoItems.Length)
					{
						CargoItems[j] = ItemStack.LoadItemStackFromNBT(nbttagcompound);
					}
				}
			}
		}

		public override float GetShadowSize()
		{
			return 0.0F;
		}

		/// <summary>
		/// Applies a velocity to each of the entities pushing them away from each other. Args: entity
		/// </summary>
		public override void ApplyEntityCollision(Entity par1Entity)
		{
			if (WorldObj.IsRemote)
			{
				return;
			}

			if (par1Entity == RiddenByEntity)
			{
				return;
			}

			if ((par1Entity is EntityLiving) && !(par1Entity is EntityPlayer) && !(par1Entity is EntityIronGolem) && MinecartType == 0 && MotionX * MotionX + MotionZ * MotionZ > 0.01D && RiddenByEntity == null && par1Entity.RidingEntity == null)
			{
				par1Entity.MountEntity(this);
			}

            float d = par1Entity.PosX - PosX;
            float d1 = par1Entity.PosZ - PosZ;
            float d2 = d * d + d1 * d1;

	//JAVA TO C# CONVERTER TODO TASK: Octal literals cannot be represented in C#:
			if (d2 >= 9.9999997473787516E-005D)
			{
				d2 = MathHelper2.Sqrt_double(d2);
				d /= d2;
				d1 /= d2;
                float d3 = 1.0F / d2;

				if (d3 > 1.0F)
				{
					d3 = 1.0F;
				}

				d *= d3;
				d1 *= d3;
				d *= 0.10000000149011612F;
				d1 *= 0.10000000149011612F;
				d *= 1.0F - EntityCollisionReduction;
				d1 *= 1.0F - EntityCollisionReduction;
				d *= 0.5F;
				d1 *= 0.5F;

				if (par1Entity is EntityMinecart)
				{
					double d4 = par1Entity.PosX - PosX;
					double d5 = par1Entity.PosZ - PosZ;
					Vec3D vec3d = Vec3D.CreateVector(d4, 0.0F, d5).Normalize();
					Vec3D vec3d1 = Vec3D.CreateVector(MathHelper2.Cos((RotationYaw * (float)Math.PI) / 180F), 0.0F, MathHelper2.Sin((RotationYaw * (float)Math.PI) / 180F)).Normalize();
					double d6 = Math.Abs(vec3d.DotProduct(vec3d1));

					if (d6 < 0.80000001192092896D)
					{
						return;
					}

                    float d7 = par1Entity.MotionX + MotionX;
                    float d8 = par1Entity.MotionZ + MotionZ;

					if (((EntityMinecart)par1Entity).MinecartType == 2 && MinecartType != 2)
					{
						MotionX *= 0.20000000298023224F;
						MotionZ *= 0.20000000298023224F;
						AddVelocity(par1Entity.MotionX - d, 0.0F, par1Entity.MotionZ - d1);
						par1Entity.MotionX *= 0.94999998807907104F;
						par1Entity.MotionZ *= 0.94999998807907104F;
					}
					else if (((EntityMinecart)par1Entity).MinecartType != 2 && MinecartType == 2)
					{
						par1Entity.MotionX *= 0.20000000298023224F;
						par1Entity.MotionZ *= 0.20000000298023224F;
						par1Entity.AddVelocity(MotionX + d, 0.0F, MotionZ + d1);
						MotionX *= 0.94999998807907104F;
						MotionZ *= 0.94999998807907104F;
					}
					else
					{
						d7 /= 2F;
						d8 /= 2F;
						MotionX *= 0.20000000298023224F;
						MotionZ *= 0.20000000298023224F;
						AddVelocity(d7 - d, 0.0F, d8 - d1);
						par1Entity.MotionX *= 0.20000000298023224F;
						par1Entity.MotionZ *= 0.20000000298023224F;
						par1Entity.AddVelocity(d7 + d, 0.0F, d8 + d1);
					}
				}
				else
				{
					AddVelocity(-d, 0.0F, -d1);
					par1Entity.AddVelocity(d / 4F, 0.0F, d1 / 4F);
				}
			}
		}

		/// <summary>
		/// Returns the number of slots in the inventory.
		/// </summary>
		public virtual int GetSizeInventory()
		{
			return 27;
		}

		/// <summary>
		/// Returns the stack in slot i
		/// </summary>
		public virtual ItemStack GetStackInSlot(int par1)
		{
			return CargoItems[par1];
		}

		/// <summary>
		/// Decrease the size of the stack in slot (first int arg) by the amount of the second int arg. Returns the new
		/// stack.
		/// </summary>
		public virtual ItemStack DecrStackSize(int par1, int par2)
		{
			if (CargoItems[par1] != null)
			{
				if (CargoItems[par1].StackSize <= par2)
				{
					ItemStack itemstack = CargoItems[par1];
					CargoItems[par1] = null;
					return itemstack;
				}

				ItemStack itemstack1 = CargoItems[par1].SplitStack(par2);

				if (CargoItems[par1].StackSize == 0)
				{
					CargoItems[par1] = null;
				}

				return itemstack1;
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// When some containers are closed they call this on each slot, then drop whatever it returns as an EntityItem -
		/// like when you close a workbench GUI.
		/// </summary>
		public virtual ItemStack GetStackInSlotOnClosing(int par1)
		{
			if (CargoItems[par1] != null)
			{
				ItemStack itemstack = CargoItems[par1];
				CargoItems[par1] = null;
				return itemstack;
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Sets the given item stack to the specified slot in the inventory (can be crafting or armor sections).
		/// </summary>
		public virtual void SetInventorySlotContents(int par1, ItemStack par2ItemStack)
		{
			CargoItems[par1] = par2ItemStack;

			if (par2ItemStack != null && par2ItemStack.StackSize > GetInventoryStackLimit())
			{
				par2ItemStack.StackSize = GetInventoryStackLimit();
			}
		}

		/// <summary>
		/// Returns the name of the inventory.
		/// </summary>
		public virtual string GetInvName()
		{
			return "container.minecart";
		}

		/// <summary>
		/// Returns the maximum stack size for a inventory slot. Seems to always be 64, possibly will be extended. *Isn't
		/// this more of a set than a get?*
		/// </summary>
		public virtual int GetInventoryStackLimit()
		{
			return 64;
		}

		/// <summary>
		/// Called when an the contents of an Inventory change, usually
		/// </summary>
		public virtual void OnInventoryChanged()
		{
		}

		/// <summary>
		/// Called when a player interacts with a mob. e.g. gets milk from a cow, gets into the saddle on a pig.
		/// </summary>
		public override bool Interact(EntityPlayer par1EntityPlayer)
		{
			if (MinecartType == 0)
			{
				if (RiddenByEntity != null && (RiddenByEntity is EntityPlayer) && RiddenByEntity != par1EntityPlayer)
				{
					return true;
				}

				if (!WorldObj.IsRemote)
				{
					par1EntityPlayer.MountEntity(this);
				}
			}
			else if (MinecartType == 1)
			{
				if (!WorldObj.IsRemote)
				{
					par1EntityPlayer.DisplayGUIChest(this);
				}
			}
			else if (MinecartType == 2)
			{
				ItemStack itemstack = par1EntityPlayer.Inventory.GetCurrentItem();

				if (itemstack != null && itemstack.ItemID == Item.Coal.ShiftedIndex)
				{
					if (--itemstack.StackSize == 0)
					{
						par1EntityPlayer.Inventory.SetInventorySlotContents(par1EntityPlayer.Inventory.CurrentItem, null);
					}

					Fuel += 3600;
				}

				PushX = PosX - par1EntityPlayer.PosX;
				PushZ = PosZ - par1EntityPlayer.PosZ;
			}

			return true;
		}

		/// <summary>
		/// Sets the position and rotation. Only difference from the other one is no bounding on the rotation. Args: posX,
		/// posY, posZ, yaw, pitch
		/// </summary>
        public override void SetPositionAndRotation2(float par1, float par3, float par5, float par7, float par8, int par9)
		{
			MinecartX = par1;
			MinecartY = par3;
			MinecartZ = par5;
			MinecartYaw = par7;
			MinecartPitch = par8;
			TurnProgress = par9 + 2;
			MotionX = VelocityX;
			MotionY = VelocityY;
			MotionZ = VelocityZ;
		}

		/// <summary>
		/// Sets the velocity to the args. Args: x, y, z
		/// </summary>
        public override void SetVelocity(float par1, float par3, float par5)
		{
			VelocityX = MotionX = par1;
			VelocityY = MotionY = par3;
			VelocityZ = MotionZ = par5;
		}

		/// <summary>
		/// Do not make give this method the name canInteractWith because it clashes with Container
		/// </summary>
		public virtual bool IsUseableByPlayer(EntityPlayer par1EntityPlayer)
		{
			if (IsDead)
			{
				return false;
			}

			return par1EntityPlayer.GetDistanceSqToEntity(this) <= 64D;
		}

		/// <summary>
		/// Is this minecart powered (Fuel > 0)
		/// </summary>
		protected virtual bool IsMinecartPowered()
		{
			return (DataWatcher.GetWatchableObjectByte(16) & 1) != 0;
		}

		/// <summary>
		/// Set if this minecart is powered (Fuel > 0)
		/// </summary>
		protected virtual void SetMinecartPowered(bool par1)
		{
			if (par1)
			{
				DataWatcher.UpdateObject(16, DataWatcher.GetWatchableObjectByte(16) | 1);
			}
			else
			{
				DataWatcher.UpdateObject(16, DataWatcher.GetWatchableObjectByte(16) & -2);
			}
		}

		public virtual void OpenChest()
		{
		}

		public virtual void CloseChest()
		{
		}

		public virtual void Func_41024_b(int par1)
		{
			DataWatcher.UpdateObject(19, par1);
		}

		public virtual int Func_41025_i()
		{
			return DataWatcher.GetWatchableObjectInt(19);
		}

		public virtual void Func_41028_c(int par1)
		{
			DataWatcher.UpdateObject(17, par1);
		}

		public virtual int Func_41023_l()
		{
			return DataWatcher.GetWatchableObjectInt(17);
		}

		public virtual void Func_41029_h(int par1)
		{
			DataWatcher.UpdateObject(18, par1);
		}

		public virtual int Func_41030_m()
		{
			return DataWatcher.GetWatchableObjectInt(18);
		}
	}
}