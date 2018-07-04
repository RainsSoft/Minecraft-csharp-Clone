using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class EntityArrow : Entity
	{
		private int XTile;
		private int YTile;
		private int ZTile;
		private int InTile;
		private int InData;
		private bool InGround;
		public bool DoesArrowBelongToPlayer;

		/// <summary>
		/// Seems to be some sort of timer for animating an arrow. </summary>
		public int ArrowShake;

		/// <summary>
		/// The owner of this arrow. </summary>
		public Entity ShootingEntity;
		private int TicksInGround;
		private int TicksInAir;
		private double Damage;
		private int Field_46027_au;

		/// <summary>
		/// Is this arrow a critical hit? (Controls particles and damage) </summary>
		public bool ArrowCritical;

		public EntityArrow(World par1World) : base(par1World)
		{
			XTile = -1;
			YTile = -1;
			ZTile = -1;
			InTile = 0;
			InData = 0;
			InGround = false;
			DoesArrowBelongToPlayer = false;
			ArrowShake = 0;
			TicksInAir = 0;
			Damage = 2D;
			ArrowCritical = false;
			SetSize(0.5F, 0.5F);
		}

        public EntityArrow(World par1World, float par2, float par4, float par6)
            : base(par1World)
		{
			XTile = -1;
			YTile = -1;
			ZTile = -1;
			InTile = 0;
			InData = 0;
			InGround = false;
			DoesArrowBelongToPlayer = false;
			ArrowShake = 0;
			TicksInAir = 0;
			Damage = 2D;
			ArrowCritical = false;
			SetSize(0.5F, 0.5F);
			SetPosition(par2, par4, par6);
			YOffset = 0.0F;
		}

		public EntityArrow(World par1World, EntityLiving par2EntityLiving, EntityLiving par3EntityLiving, float par4, float par5) : base(par1World)
		{
			XTile = -1;
			YTile = -1;
			ZTile = -1;
			InTile = 0;
			InData = 0;
			InGround = false;
			DoesArrowBelongToPlayer = false;
			ArrowShake = 0;
			TicksInAir = 0;
			Damage = 2D;
			ArrowCritical = false;
			ShootingEntity = par2EntityLiving;
			DoesArrowBelongToPlayer = par2EntityLiving is EntityPlayer;
			PosY = (par2EntityLiving.PosY + par2EntityLiving.GetEyeHeight()) - 0.10000000149011612F;
            float d = par3EntityLiving.PosX - par2EntityLiving.PosX;
            float d1 = (par3EntityLiving.PosY + par3EntityLiving.GetEyeHeight()) - 0.69999998807907104F - PosY;
            float d2 = par3EntityLiving.PosZ - par2EntityLiving.PosZ;
            float d3 = MathHelper2.Sqrt_double(d * d + d2 * d2);

	//JAVA TO C# CONVERTER TODO TASK: Octal literals cannot be represented in C#:
			if (d3 < 9.9999999999999995E-008D)
			{
				return;
			}
			else
			{
				float f = (float)((Math.Atan2(d2, d) * 180D) / Math.PI) - 90F;
				float f1 = (float)(-((Math.Atan2(d1, d3) * 180D) / Math.PI));
                float d4 = d / d3;
                float d5 = d2 / d3;
				SetLocationAndAngles(par2EntityLiving.PosX + d4, PosY, par2EntityLiving.PosZ + d5, f, f1);
				YOffset = 0.0F;
				float f2 = (float)d3 * 0.2F;
				SetArrowHeading(d, d1 + f2, d2, par4, par5);
				return;
			}
		}

		public EntityArrow(World par1World, EntityLiving par2EntityLiving, float par3) : base(par1World)
		{
			XTile = -1;
			YTile = -1;
			ZTile = -1;
			InTile = 0;
			InData = 0;
			InGround = false;
			DoesArrowBelongToPlayer = false;
			ArrowShake = 0;
			TicksInAir = 0;
			Damage = 2D;
			ArrowCritical = false;
			ShootingEntity = par2EntityLiving;
			DoesArrowBelongToPlayer = par2EntityLiving is EntityPlayer;
			SetSize(0.5F, 0.5F);
			SetLocationAndAngles(par2EntityLiving.PosX, par2EntityLiving.PosY + par2EntityLiving.GetEyeHeight(), par2EntityLiving.PosZ, par2EntityLiving.RotationYaw, par2EntityLiving.RotationPitch);
			PosX -= MathHelper2.Cos((RotationYaw / 180F) * (float)Math.PI) * 0.16F;
			PosY -= 0.10000000149011612F;
			PosZ -= MathHelper2.Sin((RotationYaw / 180F) * (float)Math.PI) * 0.16F;
			SetPosition(PosX, PosY, PosZ);
			YOffset = 0.0F;
			MotionX = -MathHelper2.Sin((RotationYaw / 180F) * (float)Math.PI) * MathHelper2.Cos((RotationPitch / 180F) * (float)Math.PI);
			MotionZ = MathHelper2.Cos((RotationYaw / 180F) * (float)Math.PI) * MathHelper2.Cos((RotationPitch / 180F) * (float)Math.PI);
			MotionY = -MathHelper2.Sin((RotationPitch / 180F) * (float)Math.PI);
			SetArrowHeading(MotionX, MotionY, MotionZ, par3 * 1.5F, 1.0F);
		}

		protected override void EntityInit()
		{
		}

		/// <summary>
		/// Uses the provided coordinates as a heading and determines the velocity from it with the set force and random
		/// variance. Args: x, y, z, force, forceVariation
		/// </summary>
        public virtual void SetArrowHeading(float par1, float par3, float par5, float par7, float par8)
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
				PrevRotationPitch = RotationPitch;
				PrevRotationYaw = RotationYaw;
				SetLocationAndAngles(PosX, PosY, PosZ, RotationYaw, RotationPitch);
				TicksInGround = 0;
			}
		}

		/// <summary>
		/// Called to update the entity's position/logic.
		/// </summary>
		public override void OnUpdate()
		{
			base.OnUpdate();

			if (PrevRotationPitch == 0.0F && PrevRotationYaw == 0.0F)
			{
				float f = MathHelper2.Sqrt_double(MotionX * MotionX + MotionZ * MotionZ);
				PrevRotationYaw = RotationYaw = (float)((Math.Atan2(MotionX, MotionZ) * 180D) / Math.PI);
				PrevRotationPitch = RotationPitch = (float)((Math.Atan2(MotionY, f) * 180D) / Math.PI);
			}

			int i = WorldObj.GetBlockId(XTile, YTile, ZTile);

			if (i > 0)
			{
				Block.BlocksList[i].SetBlockBoundsBasedOnState(WorldObj, XTile, YTile, ZTile);
				AxisAlignedBB axisalignedbb = Block.BlocksList[i].GetCollisionBoundingBoxFromPool(WorldObj, XTile, YTile, ZTile);

				if (axisalignedbb != null && axisalignedbb.IsVecInside(Vec3D.CreateVector(PosX, PosY, PosZ)))
				{
					InGround = true;
				}
			}

			if (ArrowShake > 0)
			{
				ArrowShake--;
			}

			if (InGround)
			{
				int j = WorldObj.GetBlockId(XTile, YTile, ZTile);
				int k = WorldObj.GetBlockMetadata(XTile, YTile, ZTile);

				if (j != InTile || k != InData)
				{
					InGround = false;
					MotionX *= Rand.NextFloat() * 0.2F;
					MotionY *= Rand.NextFloat() * 0.2F;
					MotionZ *= Rand.NextFloat() * 0.2F;
					TicksInGround = 0;
					TicksInAir = 0;
					return;
				}

				TicksInGround++;

				if (TicksInGround == 1200)
				{
					SetDead();
				}

				return;
			}

			TicksInAir++;
			Vec3D vec3d = Vec3D.CreateVector(PosX, PosY, PosZ);
			Vec3D vec3d1 = Vec3D.CreateVector(PosX + MotionX, PosY + MotionY, PosZ + MotionZ);
			MovingObjectPosition movingobjectposition = WorldObj.RayTraceBlocks_do_do(vec3d, vec3d1, false, true);
			vec3d = Vec3D.CreateVector(PosX, PosY, PosZ);
			vec3d1 = Vec3D.CreateVector(PosX + MotionX, PosY + MotionY, PosZ + MotionZ);

			if (movingobjectposition != null)
			{
				vec3d1 = Vec3D.CreateVector(movingobjectposition.HitVec.XCoord, movingobjectposition.HitVec.YCoord, movingobjectposition.HitVec.ZCoord);
			}

			Entity entity = null;
            List<Entity> list = WorldObj.GetEntitiesWithinAABBExcludingEntity(this, BoundingBox.AddCoord(MotionX, MotionY, MotionZ).Expand(1.0F, 1.0F, 1.0F));
			double d = 0.0F;

			for (int l = 0; l < list.Count; l++)
			{
				Entity entity1 = list[l];

				if (!entity1.CanBeCollidedWith() || entity1 == ShootingEntity && TicksInAir < 5)
				{
					continue;
				}

				float f5 = 0.3F;
				AxisAlignedBB axisalignedbb1 = entity1.BoundingBox.Expand(f5, f5, f5);
				MovingObjectPosition movingobjectposition1 = axisalignedbb1.CalculateIntercept(vec3d, vec3d1);

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
				if (movingobjectposition.EntityHit != null)
				{
					float f1 = MathHelper2.Sqrt_double(MotionX * MotionX + MotionY * MotionY + MotionZ * MotionZ);
					int j1 = (int)Math.Ceiling((double)f1 * Damage);

					if (ArrowCritical)
					{
						j1 += Rand.Next(j1 / 2 + 2);
					}

					DamageSource damagesource = null;

					if (ShootingEntity == null)
					{
						damagesource = DamageSource.CauseArrowDamage(this, this);
					}
					else
					{
						damagesource = DamageSource.CauseArrowDamage(this, ShootingEntity);
					}

					if (IsBurning())
					{
						movingobjectposition.EntityHit.SetFire(5);
					}

					if (movingobjectposition.EntityHit.AttackEntityFrom(damagesource, j1))
					{
						if (movingobjectposition.EntityHit is EntityLiving)
						{
							((EntityLiving)movingobjectposition.EntityHit).ArrowHitTempCounter++;

							if (Field_46027_au > 0)
							{
								float f7 = MathHelper2.Sqrt_double(MotionX * MotionX + MotionZ * MotionZ);

								if (f7 > 0.0F)
								{
									movingobjectposition.EntityHit.AddVelocity((MotionX * Field_46027_au * 0.60000002384185791F) / f7, 0.10000000000000001F, (MotionZ * Field_46027_au * 0.60000002384185791F) / f7);
								}
							}
						}

						WorldObj.PlaySoundAtEntity(this, "random.bowhit", 1.0F, 1.2F / (Rand.NextFloat() * 0.2F + 0.9F));
						SetDead();
					}
					else
					{
						MotionX *= -0.10000000149011612F;
						MotionY *= -0.10000000149011612F;
						MotionZ *= -0.10000000149011612F;
						RotationYaw += 180F;
						PrevRotationYaw += 180F;
						TicksInAir = 0;
					}
				}
				else
				{
					XTile = movingobjectposition.BlockX;
					YTile = movingobjectposition.BlockY;
					ZTile = movingobjectposition.BlockZ;
					InTile = WorldObj.GetBlockId(XTile, YTile, ZTile);
					InData = WorldObj.GetBlockMetadata(XTile, YTile, ZTile);
					MotionX = (float)(movingobjectposition.HitVec.XCoord - PosX);
					MotionY = (float)(movingobjectposition.HitVec.YCoord - PosY);
					MotionZ = (float)(movingobjectposition.HitVec.ZCoord - PosZ);
					float f2 = MathHelper2.Sqrt_double(MotionX * MotionX + MotionY * MotionY + MotionZ * MotionZ);
					PosX -= (MotionX / f2) * 0.05000000074505806F;
					PosY -= (MotionY / f2) * 0.05000000074505806F;
					PosZ -= (MotionZ / f2) * 0.05000000074505806F;
					WorldObj.PlaySoundAtEntity(this, "random.bowhit", 1.0F, 1.2F / (Rand.NextFloat() * 0.2F + 0.9F));
					InGround = true;
					ArrowShake = 7;
					ArrowCritical = false;
				}
			}

			if (ArrowCritical)
			{
				for (int i1 = 0; i1 < 4; i1++)
				{
					WorldObj.SpawnParticle("crit", PosX + (MotionX * (double)i1) / 4D, PosY + (MotionY * (double)i1) / 4D, PosZ + (MotionZ * (double)i1) / 4D, -MotionX, -MotionY + 0.20000000000000001D, -MotionZ);
				}
			}

			PosX += MotionX;
			PosY += MotionY;
			PosZ += MotionZ;
			float f3 = MathHelper2.Sqrt_double(MotionX * MotionX + MotionZ * MotionZ);
			RotationYaw = (float)((Math.Atan2(MotionX, MotionZ) * 180D) / Math.PI);

			for (RotationPitch = (float)((Math.Atan2(MotionY, f3) * 180D) / Math.PI); RotationPitch - PrevRotationPitch < -180F; PrevRotationPitch -= 360F)
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
			float f4 = 0.99F;
			float f6 = 0.05F;

			if (IsInWater())
			{
				for (int k1 = 0; k1 < 4; k1++)
				{
					float f8 = 0.25F;
					WorldObj.SpawnParticle("bubble", PosX - MotionX * (double)f8, PosY - MotionY * (double)f8, PosZ - MotionZ * (double)f8, MotionX, MotionY, MotionZ);
				}

				f4 = 0.8F;
			}

			MotionX *= f4;
			MotionY *= f4;
			MotionZ *= f4;
			MotionY -= f6;
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
			par1NBTTagCompound.SetByte("inData", (byte)InData);
			par1NBTTagCompound.SetByte("shake", (byte)ArrowShake);
			par1NBTTagCompound.SetByte("inGround", (byte)(InGround ? 1 : 0));
			par1NBTTagCompound.Setbool("player", DoesArrowBelongToPlayer);
			par1NBTTagCompound.SetDouble("damage", Damage);
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
			InData = par1NBTTagCompound.GetByte("inData") & 0xff;
			ArrowShake = par1NBTTagCompound.GetByte("shake") & 0xff;
			InGround = par1NBTTagCompound.GetByte("inGround") == 1;
			DoesArrowBelongToPlayer = par1NBTTagCompound.Getbool("player");

			if (par1NBTTagCompound.HasKey("damage"))
			{
				Damage = par1NBTTagCompound.GetDouble("damage");
			}
		}

		/// <summary>
		/// Called by a player entity when they collide with an entity
		/// </summary>
		public override void OnCollideWithPlayer(EntityPlayer par1EntityPlayer)
		{
			if (WorldObj.IsRemote)
			{
				return;
			}

			if (InGround && DoesArrowBelongToPlayer && ArrowShake <= 0 && par1EntityPlayer.Inventory.AddItemStackToInventory(new ItemStack(Item.Arrow, 1)))
			{
				WorldObj.PlaySoundAtEntity(this, "random.pop", 0.2F, ((Rand.NextFloat() - Rand.NextFloat()) * 0.7F + 1.0F) * 2.0F);
				par1EntityPlayer.OnItemPickup(this, 1);
				SetDead();
			}
		}

		public override float GetShadowSize()
		{
			return 0.0F;
		}

		public virtual void SetDamage(double par1)
		{
			Damage = par1;
		}

		public virtual double GetDamage()
		{
			return Damage;
		}

		public virtual void Func_46023_b(int par1)
		{
			Field_46027_au = par1;
		}

		/// <summary>
		/// If returns false, the item will not inflict any damage against entities.
		/// </summary>
		public override bool CanAttackWithItem()
		{
			return false;
		}
	}
}