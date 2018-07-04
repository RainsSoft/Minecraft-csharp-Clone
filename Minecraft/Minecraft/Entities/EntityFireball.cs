using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class EntityFireball : Entity
	{
		private int XTile;
		private int YTile;
		private int ZTile;
		private int InTile;
		private bool InGround;
		public EntityLiving ShootingEntity;
		private int TicksAlive;
		private int TicksInAir;
        public float AccelerationX;
        public float AccelerationY;
        public float AccelerationZ;

		public EntityFireball(World par1World) : base(par1World)
		{
			XTile = -1;
			YTile = -1;
			ZTile = -1;
			InTile = 0;
			InGround = false;
			TicksInAir = 0;
			SetSize(1.0F, 1.0F);
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

        public EntityFireball(World par1World, float par2, float par4, float par6, float par8, float par10, float par12)
            : base(par1World)
		{
			XTile = -1;
			YTile = -1;
			ZTile = -1;
			InTile = 0;
			InGround = false;
			TicksInAir = 0;
			SetSize(1.0F, 1.0F);
			SetLocationAndAngles(par2, par4, par6, RotationYaw, RotationPitch);
			SetPosition(par2, par4, par6);
            float d = MathHelper2.Sqrt_double(par8 * par8 + par10 * par10 + par12 * par12);
			AccelerationX = (par8 / d) * 0.10000000000000001F;
			AccelerationY = (par10 / d) * 0.10000000000000001F;
			AccelerationZ = (par12 / d) * 0.10000000000000001F;
		}

        public EntityFireball(World par1World, EntityLiving par2EntityLiving, float par3, float par5, float par7)
            : base(par1World)
		{
			XTile = -1;
			YTile = -1;
			ZTile = -1;
			InTile = 0;
			InGround = false;
			TicksInAir = 0;
			ShootingEntity = par2EntityLiving;
			SetSize(1.0F, 1.0F);
			SetLocationAndAngles(par2EntityLiving.PosX, par2EntityLiving.PosY, par2EntityLiving.PosZ, par2EntityLiving.RotationYaw, par2EntityLiving.RotationPitch);
			SetPosition(PosX, PosY, PosZ);
			YOffset = 0.0F;
			MotionX = MotionY = MotionZ = 0.0F;
			par3 += Rand.NextGaussian() * 0.40000000000000002F;
			par5 += Rand.NextGaussian() * 0.40000000000000002F;
			par7 += Rand.NextGaussian() * 0.40000000000000002F;
            float d = MathHelper2.Sqrt_double(par3 * par3 + par5 * par5 + par7 * par7);
			AccelerationX = (par3 / d) * 0.10000000000000001F;
			AccelerationY = (par5 / d) * 0.10000000000000001F;
			AccelerationZ = (par7 / d) * 0.10000000000000001F;
		}

		/// <summary>
		/// Called to update the entity's position/logic.
		/// </summary>
		public override void OnUpdate()
		{
			if (!WorldObj.IsRemote && (ShootingEntity != null && ShootingEntity.IsDead || !WorldObj.BlockExists((int)PosX, (int)PosY, (int)PosZ)))
			{
				SetDead();
				return;
			}

			base.OnUpdate();
			SetFire(1);

			if (InGround)
			{
				int i = WorldObj.GetBlockId(XTile, YTile, ZTile);

				if (i != InTile)
				{
					InGround = false;
					MotionX *= Rand.NextFloat() * 0.2F;
					MotionY *= Rand.NextFloat() * 0.2F;
					MotionZ *= Rand.NextFloat() * 0.2F;
					TicksAlive = 0;
					TicksInAir = 0;
				}
				else
				{
					TicksAlive++;

					if (TicksAlive == 600)
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
			double d = 0.0F;

			for (int j = 0; j < list.Count; j++)
			{
				Entity entity1 = list[j];

				if (!entity1.CanBeCollidedWith() || entity1.IsEntityEqual(ShootingEntity) && TicksInAir < 25)
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

				double d1 = vec3d.DistanceTo(movingobjectposition1.HitVec);

				if (d1 < d || d == 0.0F)
				{
					entity = entity1;
					d = d1;
				}
			}

			if (entity != null)
			{
				movingobjectposition = new MovingObjectPosition(entity);
			}

			if (movingobjectposition != null)
			{
				Func_40071_a(movingobjectposition);
			}

			PosX += MotionX;
			PosY += MotionY;
			PosZ += MotionZ;
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
			float f1 = 0.95F;

			if (IsInWater())
			{
				for (int k = 0; k < 4; k++)
				{
					float f3 = 0.25F;
					WorldObj.SpawnParticle("bubble", PosX - MotionX * (double)f3, PosY - MotionY * (double)f3, PosZ - MotionZ * (double)f3, MotionX, MotionY, MotionZ);
				}

				f1 = 0.8F;
			}

			MotionX += AccelerationX;
			MotionY += AccelerationY;
			MotionZ += AccelerationZ;
			MotionX *= f1;
			MotionY *= f1;
			MotionZ *= f1;
			WorldObj.SpawnParticle("smoke", PosX, PosY + 0.5D, PosZ, 0.0F, 0.0F, 0.0F);
			SetPosition(PosX, PosY, PosZ);
		}

		protected virtual void Func_40071_a(MovingObjectPosition par1MovingObjectPosition)
		{
			if (!WorldObj.IsRemote)
			{
				if (par1MovingObjectPosition.EntityHit != null)
				{
					if (!par1MovingObjectPosition.EntityHit.AttackEntityFrom(DamageSource.CauseFireballDamage(this, ShootingEntity), 4))
					{
						;
					}
				}

				WorldObj.NewExplosion(null, PosX, PosY, PosZ, 1.0F, true);
				SetDead();
			}
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
			InGround = par1NBTTagCompound.GetByte("inGround") == 1;
		}

		/// <summary>
		/// Returns true if other Entities should be prevented from moving through this Entity.
		/// </summary>
		public override bool CanBeCollidedWith()
		{
			return true;
		}

		public override float GetCollisionBorderSize()
		{
			return 1.0F;
		}

		/// <summary>
		/// Called when the entity is attacked.
		/// </summary>
		public override bool AttackEntityFrom(DamageSource par1DamageSource, int par2)
		{
			SetBeenAttacked();

			if (par1DamageSource.GetEntity() != null)
			{
				Vec3D vec3d = par1DamageSource.GetEntity().GetLookVec();

				if (vec3d != null)
				{
                    MotionX = (float)vec3d.XCoord;
                    MotionY = (float)vec3d.YCoord;
                    MotionZ = (float)vec3d.ZCoord;
					AccelerationX = MotionX * 0.10000000000000001F;
					AccelerationY = MotionY * 0.10000000000000001F;
					AccelerationZ = MotionZ * 0.10000000000000001F;
				}

				if (par1DamageSource.GetEntity() is EntityLiving)
				{
					ShootingEntity = (EntityLiving)par1DamageSource.GetEntity();
				}

				return true;
			}
			else
			{
				return false;
			}
		}

		public override float GetShadowSize()
		{
			return 0.0F;
		}

		/// <summary>
		/// Gets how bright this entity is.
		/// </summary>
		public override float GetBrightness(float par1)
		{
			return 1.0F;
		}

		public override int GetBrightnessForRender(float par1)
		{
			return 0xf000f0;
		}
	}
}