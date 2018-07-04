using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class EntityFishHook : Entity
	{
		/// <summary>
		/// The tile this entity is on, X position </summary>
		private int XTile;

		/// <summary>
		/// The tile this entity is on, Y position </summary>
		private int YTile;

		/// <summary>
		/// The tile this entity is on, Z position </summary>
		private int ZTile;
		private int InTile;
		private bool InGround;
		public int Shake;
		public EntityPlayer Angler;
		private int TicksInGround;
		private int TicksInAir;

		/// <summary>
		/// the number of ticks remaining until this fish can no longer be caught </summary>
		private int TicksCatchable;

		/// <summary>
		/// The entity that the fishing rod is connected to, if any. When you right click on the fishing rod and the hook
		/// falls on to an entity, this it that entity.
		/// </summary>
		public Entity Bobber;
		private int FishPosRotationIncrements;
        private float FishX;
        private float FishY;
        private float FishZ;
        private float FishYaw;
        private float FishPitch;
        private float VelocityX;
        private float VelocityY;
        private float VelocityZ;

		public EntityFishHook(World par1World) : base(par1World)
		{
			XTile = -1;
			YTile = -1;
			ZTile = -1;
			InTile = 0;
			InGround = false;
			Shake = 0;
			TicksInAir = 0;
			TicksCatchable = 0;
			Bobber = null;
			SetSize(0.25F, 0.25F);
			IgnoreFrustumCheck = true;
		}

        public EntityFishHook(World par1World, float par2, float par4, float par6)
            : this(par1World)
		{
			SetPosition(par2, par4, par6);
			IgnoreFrustumCheck = true;
		}

		public EntityFishHook(World par1World, EntityPlayer par2EntityPlayer) : base(par1World)
		{
			XTile = -1;
			YTile = -1;
			ZTile = -1;
			InTile = 0;
			InGround = false;
			Shake = 0;
			TicksInAir = 0;
			TicksCatchable = 0;
			Bobber = null;
			IgnoreFrustumCheck = true;
			Angler = par2EntityPlayer;
			Angler.FishEntity = this;
			SetSize(0.25F, 0.25F);
			SetLocationAndAngles(par2EntityPlayer.PosX, (par2EntityPlayer.PosY + 1.6200000000000001F) - par2EntityPlayer.YOffset, par2EntityPlayer.PosZ, par2EntityPlayer.RotationYaw, par2EntityPlayer.RotationPitch);
			PosX -= MathHelper2.Cos((RotationYaw / 180F) * (float)Math.PI) * 0.16F;
			PosY -= 0.10000000149011612F;
			PosZ -= MathHelper2.Sin((RotationYaw / 180F) * (float)Math.PI) * 0.16F;
			SetPosition(PosX, PosY, PosZ);
			YOffset = 0.0F;
			float f = 0.4F;
			MotionX = -MathHelper2.Sin((RotationYaw / 180F) * (float)Math.PI) * MathHelper2.Cos((RotationPitch / 180F) * (float)Math.PI) * f;
			MotionZ = MathHelper2.Cos((RotationYaw / 180F) * (float)Math.PI) * MathHelper2.Cos((RotationPitch / 180F) * (float)Math.PI) * f;
			MotionY = -MathHelper2.Sin((RotationPitch / 180F) * (float)Math.PI) * f;
			CalculateVelocity(MotionX, MotionY, MotionZ, 1.5F, 1.0F);
		}

		protected override void EntityInit()
		{
		}

		/// <summary>
		/// Checks if the entity is in range to render by using the past in distance and comparing it to its average edge
		/// length * 64 * renderDistanceWeight Args: distance
		/// </summary>
        public override bool IsInRangeToRenderDist(float par1)
		{
            float d = BoundingBox.GetAverageEdgeLength() * 4;
			d *= 64;
			return par1 < d * d;
		}

        public virtual void CalculateVelocity(float par1, float par3, float par5, float par7, float par8)
		{
			float f = MathHelper2.Sqrt_double(par1 * par1 + par3 * par3 + par5 * par5);
			par1 /= f;
			par3 /= f;
			par5 /= f;
			par1 += Rand.NextGaussian() * 0.0074999998323619366F * par8;
			par3 += Rand.NextGaussian() * 0.0074999998323619366F * par8;
			par5 += Rand.NextGaussian() * 0.0074999998323619366F * par8;
			par1 *= par7;
			par3 *= par7;
			par5 *= par7;
			MotionX = par1;
			MotionY = par3;
			MotionZ = par5;
			float f1 = MathHelper2.Sqrt_double(par1 * par1 + par5 * par5);
			PrevRotationYaw = RotationYaw = (float)((Math.Atan2(par1, par5) * 180D) / Math.PI);
			PrevRotationPitch = RotationPitch = (float)((Math.Atan2(par3, f1) * 180D) / Math.PI);
			TicksInGround = 0;
		}

		/// <summary>
		/// Sets the position and rotation. Only difference from the other one is no bounding on the rotation. Args: posX,
		/// posY, posZ, yaw, pitch
		/// </summary>
        public override void SetPositionAndRotation2(float par1, float par3, float par5, float par7, float par8, int par9)
		{
			FishX = par1;
			FishY = par3;
			FishZ = par5;
			FishYaw = par7;
			FishPitch = par8;
			FishPosRotationIncrements = par9;
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
		/// Called to update the entity's position/logic.
		/// </summary>
		public override void OnUpdate()
		{
			base.OnUpdate();

			if (FishPosRotationIncrements > 0)
			{
                float d = PosX + (FishX - PosX) / FishPosRotationIncrements;
                float d1 = PosY + (FishY - PosY) / FishPosRotationIncrements;
                float d2 = PosZ + (FishZ - PosZ) / FishPosRotationIncrements;
				double d4;

				for (d4 = FishYaw - RotationYaw; d4 < -180D; d4 += 360D)
				{
				}

				for (; d4 >= 180D; d4 -= 360D)
				{
				}

				RotationYaw += (float)d4 / FishPosRotationIncrements;
				RotationPitch += (float)(FishPitch - RotationPitch) / FishPosRotationIncrements;
				FishPosRotationIncrements--;
				SetPosition(d, d1, d2);
				SetRotation(RotationYaw, RotationPitch);
				return;
			}

			if (!WorldObj.IsRemote)
			{
				ItemStack itemstack = Angler.GetCurrentEquippedItem();

				if (Angler.IsDead || !Angler.IsEntityAlive() || itemstack == null || itemstack.GetItem() != Item.FishingRod || GetDistanceSqToEntity(Angler) > 1024D)
				{
					SetDead();
					Angler.FishEntity = null;
					return;
				}

				if (Bobber != null)
				{
					if (Bobber.IsDead)
					{
						Bobber = null;
					}
					else
					{
						PosX = Bobber.PosX;
						PosY = Bobber.BoundingBox.MinY + Bobber.Height * 0.80000000000000004F;
						PosZ = Bobber.PosZ;
						return;
					}
				}
			}

			if (Shake > 0)
			{
				Shake--;
			}

			if (InGround)
			{
				int i = WorldObj.GetBlockId(XTile, YTile, ZTile);

				if (i != InTile)
				{
					InGround = false;
					MotionX *= Rand.NextFloat() * 0.2F;
					MotionY *= Rand.NextFloat() * 0.2F;
					MotionZ *= Rand.NextFloat() * 0.2F;
					TicksInGround = 0;
					TicksInAir = 0;
				}
				else
				{
					TicksInGround++;

					if (TicksInGround == 1200)
					{
						SetDead();
					}

					return;
				}
			}
			else
			{
				TicksInAir++;
			}

			Vec3D vec3d = Vec3D.CreateVector(PosX, PosY, PosZ);
			Vec3D vec3d1 = Vec3D.CreateVector(PosX + MotionX, PosY + MotionY, PosZ + MotionZ);
			MovingObjectPosition movingobjectposition = WorldObj.RayTraceBlocks(vec3d, vec3d1);
			vec3d = Vec3D.CreateVector(PosX, PosY, PosZ);
			vec3d1 = Vec3D.CreateVector(PosX + MotionX, PosY + MotionY, PosZ + MotionZ);

			if (movingobjectposition != null)
			{
				vec3d1 = Vec3D.CreateVector(movingobjectposition.HitVec.XCoord, movingobjectposition.HitVec.YCoord, movingobjectposition.HitVec.ZCoord);
			}

			Entity entity = null;
			List<Entity> list = WorldObj.GetEntitiesWithinAABBExcludingEntity(this, BoundingBox.AddCoord(MotionX, MotionY, MotionZ).Expand(1.0F, 1.0F, 1.0F));
			double d3 = 0.0F;

			for (int j = 0; j < list.Count; j++)
			{
				Entity entity1 = list[j];

				if (!entity1.CanBeCollidedWith() || entity1 == Angler && TicksInAir < 5)
				{
					continue;
				}

				float f2 = 0.3F;
				AxisAlignedBB axisalignedbb = entity1.BoundingBox.Expand(f2, f2, f2);
				MovingObjectPosition movingobjectposition1 = axisalignedbb.CalculateIntercept(vec3d, vec3d1);

				if (movingobjectposition1 == null)
				{
					continue;
				}

				double d6 = vec3d.DistanceTo(movingobjectposition1.HitVec);

				if (d6 < d3 || d3 == 0.0F)
				{
					entity = entity1;
					d3 = d6;
				}
			}

			if (entity != null)
			{
				movingobjectposition = new MovingObjectPosition(entity);
			}

			if (movingobjectposition != null)
			{
				if (movingobjectposition.EntityHit != null)
				{
					if (movingobjectposition.EntityHit.AttackEntityFrom(DamageSource.CauseThrownDamage(this, Angler), 0))
					{
						Bobber = movingobjectposition.EntityHit;
					}
				}
				else
				{
					InGround = true;
				}
			}

			if (InGround)
			{
				return;
			}

			MoveEntity(MotionX, MotionY, MotionZ);
			float f = MathHelper2.Sqrt_double(MotionX * MotionX + MotionZ * MotionZ);
			RotationYaw = (float)((Math.Atan2(MotionX, MotionZ) * 180D) / Math.PI);

			for (RotationPitch = (float)((Math.Atan2(MotionY, f) * 180D) / Math.PI); RotationPitch - PrevRotationPitch < -180F; PrevRotationPitch -= 360F)
			{
			}

			for (; RotationPitch - PrevRotationPitch >= 180F; PrevRotationPitch += 360F)
			{
			}

			for (; RotationYaw - PrevRotationYaw < -180F; PrevRotationYaw -= 360F)
			{
			}

			for (; RotationYaw - PrevRotationYaw >= 180F; PrevRotationYaw += 360F)
			{
			}

			RotationPitch = PrevRotationPitch + (RotationPitch - PrevRotationPitch) * 0.2F;
			RotationYaw = PrevRotationYaw + (RotationYaw - PrevRotationYaw) * 0.2F;
			float f1 = 0.92F;

			if (OnGround || IsCollidedHorizontally)
			{
				f1 = 0.5F;
			}

			int k = 5;
            float d5 = 0.0F;

			for (int l = 0; l < k; l++)
			{
                float d8 = ((BoundingBox.MinY + ((BoundingBox.MaxY - BoundingBox.MinY) * (l + 0)) / k) - 0.125F) + 0.125F;
                float d9 = ((BoundingBox.MinY + ((BoundingBox.MaxY - BoundingBox.MinY) * (l + 1)) / k) - 0.125F) + 0.125F;
				AxisAlignedBB axisalignedbb1 = AxisAlignedBB.GetBoundingBoxFromPool(BoundingBox.MinX, d8, BoundingBox.MinZ, BoundingBox.MaxX, d9, BoundingBox.MaxZ);

				if (WorldObj.IsAABBInMaterial(axisalignedbb1, Material.Water))
				{
					d5 += 1.0F / k;
				}
			}

			if (d5 > 0.0F)
			{
				if (TicksCatchable > 0)
				{
					TicksCatchable--;
				}
				else
				{
					int c = 500;

					if (WorldObj.CanLightningStrikeAt(MathHelper2.Floor_double(PosX), MathHelper2.Floor_double(PosY) + 1, MathHelper2.Floor_double(PosZ)))
					{
						c = 300;
					}

					if (Rand.Next(c) == 0)
					{
						TicksCatchable = Rand.Next(30) + 10;
						MotionY -= 0.20000000298023224F;
						WorldObj.PlaySoundAtEntity(this, "random.splash", 0.25F, 1.0F + (Rand.NextFloat() - Rand.NextFloat()) * 0.4F);
						float f3 = MathHelper2.Floor_double(BoundingBox.MinY);

						for (int i1 = 0; (float)i1 < 1.0F + Width * 20F; i1++)
						{
							float f4 = (Rand.NextFloat() * 2.0F - 1.0F) * Width;
							float f6 = (Rand.NextFloat() * 2.0F - 1.0F) * Width;
							WorldObj.SpawnParticle("bubble", PosX + f4, f3 + 1.0F, PosZ + f6, MotionX, MotionY - (Rand.NextFloat() * 0.2F), MotionZ);
						}

						for (int j1 = 0; (float)j1 < 1.0F + Width * 20F; j1++)
						{
							float f5 = (Rand.NextFloat() * 2.0F - 1.0F) * Width;
							float f7 = (Rand.NextFloat() * 2.0F - 1.0F) * Width;
							WorldObj.SpawnParticle("splash", PosX + f5, f3 + 1.0F, PosZ + f7, MotionX, MotionY, MotionZ);
						}
					}
				}
			}

			if (TicksCatchable > 0)
			{
				MotionY -= (Rand.NextFloat() * Rand.NextFloat() * Rand.NextFloat()) * 0.20000000000000001F;
			}

            float d7 = d5 * 2F - 1.0F;
			MotionY += 0.039999999105930328F * d7;

			if (d5 > 0.0F)
			{
				f1 = (float)((double)f1 * 0.90000000000000002D);
				MotionY *= 0.80000000000000004F;
			}

			MotionX *= f1;
			MotionY *= f1;
			MotionZ *= f1;
			SetPosition(PosX, PosY, PosZ);
		}

		/// <summary>
		/// (abstract) Protected helper method to write subclass entity data to NBT.
		/// </summary>
		public override void WriteEntityToNBT(NBTTagCompound par1NBTTagCompound)
		{
			par1NBTTagCompound.SetShort("xTile", (short)XTile);
			par1NBTTagCompound.SetShort("yTile", (short)YTile);
			par1NBTTagCompound.SetShort("zTile", (short)ZTile);
			par1NBTTagCompound.SetByte("inTile", (byte)InTile);
			par1NBTTagCompound.SetByte("shake", (byte)Shake);
			par1NBTTagCompound.SetByte("inGround", (byte)(InGround ? 1 : 0));
		}

		/// <summary>
		/// (abstract) Protected helper method to read subclass entity data from NBT.
		/// </summary>
		public override void ReadEntityFromNBT(NBTTagCompound par1NBTTagCompound)
		{
			XTile = par1NBTTagCompound.GetShort("xTile");
			YTile = par1NBTTagCompound.GetShort("yTile");
			ZTile = par1NBTTagCompound.GetShort("zTile");
			InTile = par1NBTTagCompound.GetByte("inTile") & 0xff;
			Shake = par1NBTTagCompound.GetByte("shake") & 0xff;
			InGround = par1NBTTagCompound.GetByte("inGround") == 1;
		}

		public override float GetShadowSize()
		{
			return 0.0F;
		}

		public virtual int CatchFish()
		{
			byte byte0 = 0;

			if (Bobber != null)
			{
                float d = Angler.PosX - PosX;
                float d2 = Angler.PosY - PosY;
                float d4 = Angler.PosZ - PosZ;
				double d6 = MathHelper2.Sqrt_double(d * d + d2 * d2 + d4 * d4);
                float d8 = 0.10000000000000001F;
				Bobber.MotionX += d * d8;
				Bobber.MotionY += d2 * d8 + MathHelper2.Sqrt_double(d6) * 0.080000000000000002F;
				Bobber.MotionZ += d4 * d8;
				byte0 = 3;
			}
			else if (TicksCatchable > 0)
			{
				EntityItem entityitem = new EntityItem(WorldObj, PosX, PosY, PosZ, new ItemStack(Item.FishRaw));
                float d1 = Angler.PosX - PosX;
                float d3 = Angler.PosY - PosY;
                float d5 = Angler.PosZ - PosZ;
				double d7 = MathHelper2.Sqrt_double(d1 * d1 + d3 * d3 + d5 * d5);
                float d9 = 0.10000000000000001F;
				entityitem.MotionX = d1 * d9;
				entityitem.MotionY = d3 * d9 + MathHelper2.Sqrt_double(d7) * 0.080000000000000002F;
				entityitem.MotionZ = d5 * d9;
				WorldObj.SpawnEntityInWorld(entityitem);
				Angler.AddStat(StatList.FishCaughtStat, 1);
				byte0 = 1;
			}

			if (InGround)
			{
				byte0 = 2;
			}

			SetDead();
			Angler.FishEntity = null;
			return byte0;
		}
	}
}