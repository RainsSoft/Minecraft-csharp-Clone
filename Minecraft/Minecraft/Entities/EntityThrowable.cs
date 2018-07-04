using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public abstract class EntityThrowable : Entity
	{
		private int XTile;
		private int YTile;
		private int ZTile;
		private int InTile;
		protected bool InGround;
		public int ThrowableShake;

		/// <summary>
		/// Is the entity that throws this 'thing' (snowball, ender pearl, eye of ender or potion)
		/// </summary>
		protected EntityLiving Thrower;
		private int TicksInGround;
		private int TicksInAir;

		public EntityThrowable(World par1World) : base(par1World)
		{
			XTile = -1;
			YTile = -1;
			ZTile = -1;
			InTile = 0;
			InGround = false;
			ThrowableShake = 0;
			TicksInAir = 0;
			SetSize(0.25F, 0.25F);
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
            float d = BoundingBox.GetAverageEdgeLength() * 4F;
			d *= 64F;
			return par1 < d * d;
		}

		public EntityThrowable(World par1World, EntityLiving par2EntityLiving) : base(par1World)
		{
			XTile = -1;
			YTile = -1;
			ZTile = -1;
			InTile = 0;
			InGround = false;
			ThrowableShake = 0;
			TicksInAir = 0;
			Thrower = par2EntityLiving;
			SetSize(0.25F, 0.25F);
			SetLocationAndAngles(par2EntityLiving.PosX, par2EntityLiving.PosY + par2EntityLiving.GetEyeHeight(), par2EntityLiving.PosZ, par2EntityLiving.RotationYaw, par2EntityLiving.RotationPitch);
			PosX -= MathHelper2.Cos((RotationYaw / 180F) * (float)Math.PI) * 0.16F;
			PosY -= 0.10000000149011612F;
			PosZ -= MathHelper2.Sin((RotationYaw / 180F) * (float)Math.PI) * 0.16F;
			SetPosition(PosX, PosY, PosZ);
			YOffset = 0.0F;
			float f = 0.4F;
			MotionX = -MathHelper2.Sin((RotationYaw / 180F) * (float)Math.PI) * MathHelper2.Cos((RotationPitch / 180F) * (float)Math.PI) * f;
			MotionZ = MathHelper2.Cos((RotationYaw / 180F) * (float)Math.PI) * MathHelper2.Cos((RotationPitch / 180F) * (float)Math.PI) * f;
			MotionY = -MathHelper2.Sin(((RotationPitch + Func_40074_d()) / 180F) * (float)Math.PI) * f;
			SetThrowableHeading(MotionX, MotionY, MotionZ, Func_40077_c(), 1.0F);
		}

        public EntityThrowable(World par1World, float par2, float par4, float par6)
            : base(par1World)
		{
			XTile = -1;
			YTile = -1;
			ZTile = -1;
			InTile = 0;
			InGround = false;
			ThrowableShake = 0;
			TicksInAir = 0;
			TicksInGround = 0;
			SetSize(0.25F, 0.25F);
			SetPosition(par2, par4, par6);
			YOffset = 0.0F;
		}

		protected virtual float Func_40077_c()
		{
			return 1.5F;
		}

		protected virtual float Func_40074_d()
		{
			return 0.0F;
		}

		/// <summary>
		/// Similar to setArrowHeading, it's point the throwable entity to a x, y, z direction.
		/// </summary>
        public virtual void SetThrowableHeading(float par1, float par3, float par5, float par7, float par8)
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
		/// Sets the velocity to the args. Args: x, y, z
		/// </summary>
        public override void SetVelocity(float par1, float par3, float par5)
		{
			MotionX = par1;
			MotionY = par3;
			MotionZ = par5;

			if (PrevRotationPitch == 0.0F && PrevRotationYaw == 0.0F)
			{
				float f = MathHelper2.Sqrt_double(par1 * par1 + par5 * par5);
				PrevRotationYaw = RotationYaw = (float)((Math.Atan2(par1, par5) * 180D) / Math.PI);
				PrevRotationPitch = RotationPitch = (float)((Math.Atan2(par3, f) * 180D) / Math.PI);
			}
		}

		/// <summary>
		/// Called to update the entity's position/logic.
		/// </summary>
		public override void OnUpdate()
		{
			LastTickPosX = PosX;
			LastTickPosY = PosY;
			LastTickPosZ = PosZ;
			base.OnUpdate();

			if (ThrowableShake > 0)
			{
				ThrowableShake--;
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

			if (!WorldObj.IsRemote)
			{
				Entity entity = null;
                List<Entity> list = WorldObj.GetEntitiesWithinAABBExcludingEntity(this, BoundingBox.AddCoord(MotionX, MotionY, MotionZ).Expand(1.0F, 1.0F, 1.0F));
                float d = 0.0F;

				for (int k = 0; k < list.Count; k++)
				{
					Entity entity1 = list[k];

					if (!entity1.CanBeCollidedWith() || entity1 == Thrower && TicksInAir < 5)
					{
						continue;
					}

					float f4 = 0.3F;
					AxisAlignedBB axisalignedbb = entity1.BoundingBox.Expand(f4, f4, f4);
					MovingObjectPosition movingobjectposition1 = axisalignedbb.CalculateIntercept(vec3d, vec3d1);

					if (movingobjectposition1 == null)
					{
						continue;
					}

                    float d1 = (float)vec3d.DistanceTo(movingobjectposition1.HitVec);

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
			}

			if (movingobjectposition != null)
			{
				OnImpact(movingobjectposition);
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
			float f1 = 0.99F;
			float f2 = Func_40075_e();

			if (IsInWater())
			{
				for (int j = 0; j < 4; j++)
				{
					float f3 = 0.25F;
					WorldObj.SpawnParticle("bubble", PosX - MotionX * f3, PosY - MotionY * f3, PosZ - MotionZ * f3, MotionX, MotionY, MotionZ);
				}

				f1 = 0.8F;
			}

			MotionX *= f1;
			MotionY *= f1;
			MotionZ *= f1;
			MotionY -= f2;
			SetPosition(PosX, PosY, PosZ);
		}

		protected virtual float Func_40075_e()
		{
			return 0.03F;
		}

		/// <summary>
		/// Called when the throwable hits a block or entity.
		/// </summary>
		protected abstract void OnImpact(MovingObjectPosition movingobjectposition);

		/// <summary>
		/// (abstract) Protected helper method to write subclass entity data to NBT.
		/// </summary>
		public override void WriteEntityToNBT(NBTTagCompound par1NBTTagCompound)
		{
			par1NBTTagCompound.SetShort("xTile", (short)XTile);
			par1NBTTagCompound.SetShort("yTile", (short)YTile);
			par1NBTTagCompound.SetShort("zTile", (short)ZTile);
			par1NBTTagCompound.SetByte("inTile", (byte)InTile);
			par1NBTTagCompound.SetByte("shake", (byte)ThrowableShake);
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
			ThrowableShake = par1NBTTagCompound.GetByte("shake") & 0xff;
			InGround = par1NBTTagCompound.GetByte("inGround") == 1;
		}

		/// <summary>
		/// Called by a player entity when they collide with an entity
		/// </summary>
		public override void OnCollideWithPlayer(EntityPlayer entityplayer)
		{
		}

		public override float GetShadowSize()
		{
			return 0.0F;
		}
	}
}