using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class EntityBoat : Entity
	{
		private int BoatPosRotationIncrements;
        private float BoatX;
        private float BoatY;
        private float BoatZ;
        private float BoatYaw;
        private float BoatPitch;
        private float VelocityX;
        private float VelocityY;
        private float VelocityZ;

		public EntityBoat(World par1World) : base(par1World)
		{
			PreventEntitySpawning = true;
			SetSize(1.5F, 0.6F);
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
			return BoundingBox;
		}

		/// <summary>
		/// Returns true if this entity should push and be pushed by other entities when colliding.
		/// </summary>
		public override bool CanBePushed()
		{
			return true;
		}

        public EntityBoat(World par1World, float par2, float par4, float par6)
            : this(par1World)
		{
			SetPosition(par2, par4 + YOffset, par6);
			MotionX = 0.0F;
			MotionY = 0.0F;
			MotionZ = 0.0F;
			PrevPosX = par2;
			PrevPosY = par4;
			PrevPosZ = par6;
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

			SetForwardDirection(-GetForwardDirection());
			SetTimeSinceHit(10);
			SetDamageTaken(GetDamageTaken() + par2 * 10);
			SetBeenAttacked();

			if (GetDamageTaken() > 40)
			{
				if (RiddenByEntity != null)
				{
					RiddenByEntity.MountEntity(this);
				}

				for (int i = 0; i < 3; i++)
				{
					DropItemWithOffset(Block.Planks.BlockID, 1, 0.0F);
				}

				for (int j = 0; j < 2; j++)
				{
					DropItemWithOffset(Item.Stick.ShiftedIndex, 1, 0.0F);
				}

				SetDead();
			}

			return true;
		}

		/// <summary>
		/// Setups the entity to do the hurt animation. Only used by packets in multiplayer.
		/// </summary>
		public override void PerformHurtAnimation()
		{
			SetForwardDirection(-GetForwardDirection());
			SetTimeSinceHit(10);
			SetDamageTaken(GetDamageTaken() * 11);
		}

		/// <summary>
		/// Returns true if other Entities should be prevented from moving through this Entity.
		/// </summary>
		public override bool CanBeCollidedWith()
		{
			return !IsDead;
		}

		/// <summary>
		/// Sets the position and rotation. Only difference from the other one is no bounding on the rotation. Args: posX,
		/// posY, posZ, yaw, pitch
		/// </summary>
        public override void SetPositionAndRotation2(float par1, float par3, float par5, float par7, float par8, int par9)
		{
			BoatX = par1;
			BoatY = par3;
			BoatZ = par5;
			BoatYaw = par7;
			BoatPitch = par8;
			BoatPosRotationIncrements = par9 + 4;
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

			if (GetTimeSinceHit() > 0)
			{
				SetTimeSinceHit(GetTimeSinceHit() - 1);
			}

			if (GetDamageTaken() > 0)
			{
				SetDamageTaken(GetDamageTaken() - 1);
			}

			PrevPosX = PosX;
			PrevPosY = PosY;
			PrevPosZ = PosZ;
			int i = 5;
            float d = 0.0F;

			for (int j = 0; j < i; j++)
			{
                float d2 = (BoundingBox.MinY + ((BoundingBox.MaxY - BoundingBox.MinY) * (j + 0)) / i) - 0.125F;
                float d8 = (BoundingBox.MinY + ((BoundingBox.MaxY - BoundingBox.MinY) * (j + 1)) / i) - 0.125F;
				AxisAlignedBB axisalignedbb = AxisAlignedBB.GetBoundingBoxFromPool(BoundingBox.MinX, d2, BoundingBox.MinZ, BoundingBox.MaxX, d8, BoundingBox.MaxZ);

				if (WorldObj.IsAABBInMaterial(axisalignedbb, Material.Water))
				{
					d += 1.0F / i;
				}
			}

			double d1 = Math.Sqrt(MotionX * MotionX + MotionZ * MotionZ);

			if (d1 > 0.14999999999999999D)
			{
				double d3 = Math.Cos(((double)RotationYaw * Math.PI) / 180D);
				double d9 = Math.Sin(((double)RotationYaw * Math.PI) / 180D);

				for (int i1 = 0; (double)i1 < 1.0D + d1 * 60D; i1++)
				{
					double d16 = Rand.NextFloat() * 2.0F - 1.0F;
					double d19 = (double)(Rand.Next(2) * 2 - 1) * 0.69999999999999996D;

					if (Rand.NextBool())
					{
						double d21 = (PosX - d3 * d16 * 0.80000000000000004D) + d9 * d19;
						double d23 = PosZ - d9 * d16 * 0.80000000000000004D - d3 * d19;
						WorldObj.SpawnParticle("splash", d21, PosY - 0.125D, d23, MotionX, MotionY, MotionZ);
					}
					else
					{
						double d22 = PosX + d3 + d9 * d16 * 0.69999999999999996D;
						double d24 = (PosZ + d9) - d3 * d16 * 0.69999999999999996D;
						WorldObj.SpawnParticle("splash", d22, PosY - 0.125D, d24, MotionX, MotionY, MotionZ);
					}
				}
			}

			if (WorldObj.IsRemote)
			{
				if (BoatPosRotationIncrements > 0)
				{
                    float d4 = PosX + (BoatX - PosX) / BoatPosRotationIncrements;
                    float d10 = PosY + (BoatY - PosY) / BoatPosRotationIncrements;
                    float d13 = PosZ + (BoatZ - PosZ) / BoatPosRotationIncrements;
					double d17;

					for (d17 = BoatYaw - (double)RotationYaw; d17 < -180D; d17 += 360D)
					{
					}

					for (; d17 >= 180D; d17 -= 360D)
					{
					}

					RotationYaw += (float)d17 / BoatPosRotationIncrements;
					RotationPitch += ((float)BoatPitch - RotationPitch) / BoatPosRotationIncrements;
					BoatPosRotationIncrements--;
					SetPosition(d4, d10, d13);
					SetRotation(RotationYaw, RotationPitch);
				}
				else
				{
                    float d5 = PosX + MotionX;
                    float d11 = PosY + MotionY;
                    float d14 = PosZ + MotionZ;
					SetPosition(d5, d11, d14);

					if (OnGround)
					{
						MotionX *= 0.5F;
						MotionY *= 0.5F;
						MotionZ *= 0.5F;
					}

					MotionX *= 0.99000000953674316F;
					MotionY *= 0.94999998807907104F;
					MotionZ *= 0.99000000953674316F;
				}

				return;
			}

			if (d < 1.0F)
			{
                float d6 = d * 2F - 1.0F;
				MotionY += 0.039999999105930328F * d6;
			}
			else
			{
				if (MotionY < 0.0F)
				{
					MotionY /= 2F;
				}

				MotionY += 0.0070000002160668373F;
			}

			if (RiddenByEntity != null)
			{
				MotionX += RiddenByEntity.MotionX * 0.20000000000000001F;
				MotionZ += RiddenByEntity.MotionZ * 0.20000000000000001F;
			}

            float d7 = 0.40000000000000002F;

			if (MotionX < -d7)
			{
				MotionX = -d7;
			}

			if (MotionX > d7)
			{
				MotionX = d7;
			}

			if (MotionZ < -d7)
			{
				MotionZ = -d7;
			}

			if (MotionZ > d7)
			{
				MotionZ = d7;
			}

			if (OnGround)
			{
				MotionX *= 0.5F;
				MotionY *= 0.5F;
				MotionZ *= 0.5F;
			}

			MoveEntity(MotionX, MotionY, MotionZ);

			if (IsCollidedHorizontally && d1 > 0.20000000000000001F)
			{
				if (!WorldObj.IsRemote)
				{
					SetDead();

					for (int k = 0; k < 3; k++)
					{
						DropItemWithOffset(Block.Planks.BlockID, 1, 0.0F);
					}

					for (int l = 0; l < 2; l++)
					{
						DropItemWithOffset(Item.Stick.ShiftedIndex, 1, 0.0F);
					}
				}
			}
			else
			{
				MotionX *= 0.99000000953674316F;
				MotionY *= 0.94999998807907104F;
				MotionZ *= 0.99000000953674316F;
			}

			RotationPitch = 0.0F;
			double d12 = RotationYaw;
			double d15 = PrevPosX - PosX;
			double d18 = PrevPosZ - PosZ;

			if (d15 * d15 + d18 * d18 > 0.001D)
			{
				d12 = (float)((Math.Atan2(d18, d15) * 180D) / Math.PI);
			}

			double d20;

			for (d20 = d12 - (double)RotationYaw; d20 >= 180D; d20 -= 360D)
			{
			}

			for (; d20 < -180D; d20 += 360D)
			{
			}

			if (d20 > 20D)
			{
				d20 = 20D;
			}

			if (d20 < -20D)
			{
				d20 = -20D;
			}

			RotationYaw += (float)d20;
			SetRotation(RotationYaw, RotationPitch);
            List<Entity> list = WorldObj.GetEntitiesWithinAABBExcludingEntity(this, BoundingBox.Expand(0.20000000298023224F, 0.0F, 0.20000000298023224F));

			if (list != null && list.Count > 0)
			{
				for (int j1 = 0; j1 < list.Count; j1++)
				{
					Entity entity = (Entity)list[j1];

					if (entity != RiddenByEntity && entity.CanBePushed() && (entity is EntityBoat))
					{
						entity.ApplyEntityCollision(this);
					}
				}
			}

			for (int k1 = 0; k1 < 4; k1++)
			{
				int l1 = MathHelper2.Floor_double(PosX + ((double)(k1 % 2) - 0.5D) * 0.80000000000000004D);
				int i2 = MathHelper2.Floor_double(PosY);
				int j2 = MathHelper2.Floor_double(PosZ + ((double)(k1 / 2) - 0.5D) * 0.80000000000000004D);

				if (WorldObj.GetBlockId(l1, i2, j2) == Block.Snow.BlockID)
				{
					WorldObj.SetBlockWithNotify(l1, i2, j2, 0);
				}
			}

			if (RiddenByEntity != null && RiddenByEntity.IsDead)
			{
				RiddenByEntity = null;
			}
		}

		public override void UpdateRiderPosition()
		{
			if (RiddenByEntity == null)
			{
				return;
			}
			else
			{
                float d = (float)Math.Cos((RotationYaw * Math.PI) / 180) * 0.40000000000000002F;
                float d1 = (float)Math.Sin((RotationYaw * Math.PI) / 180) * 0.40000000000000002F;
				RiddenByEntity.SetPosition(PosX + d, PosY + GetMountedYOffset() + RiddenByEntity.GetYOffset(), PosZ + d1);
				return;
			}
		}

		/// <summary>
		/// (abstract) Protected helper method to write subclass entity data to NBT.
		/// </summary>
        public override void WriteEntityToNBT(NBTTagCompound nbttagcompound)
		{
		}

		/// <summary>
		/// (abstract) Protected helper method to read subclass entity data from NBT.
		/// </summary>
        public override void ReadEntityFromNBT(NBTTagCompound nbttagcompound)
		{
		}

		public override float GetShadowSize()
		{
			return 0.0F;
		}

		/// <summary>
		/// Called when a player interacts with a mob. e.g. gets milk from a cow, gets into the saddle on a pig.
		/// </summary>
		public override bool Interact(EntityPlayer par1EntityPlayer)
		{
			if (RiddenByEntity != null && (RiddenByEntity is EntityPlayer) && RiddenByEntity != par1EntityPlayer)
			{
				return true;
			}

			if (!WorldObj.IsRemote)
			{
				par1EntityPlayer.MountEntity(this);
			}

			return true;
		}

		/// <summary>
		/// Sets the damage taken from the last hit.
		/// </summary>
		public virtual void SetDamageTaken(int par1)
		{
			DataWatcher.UpdateObject(19, Convert.ToInt32(par1));
		}

		/// <summary>
		/// Gets the damage taken from the last hit.
		/// </summary>
		public virtual int GetDamageTaken()
		{
			return DataWatcher.GetWatchableObjectInt(19);
		}

		/// <summary>
		/// Sets the time to count down from since the last time entity was hit.
		/// </summary>
		public virtual void SetTimeSinceHit(int par1)
		{
			DataWatcher.UpdateObject(17, Convert.ToInt32(par1));
		}

		/// <summary>
		/// Gets the time since the last hit.
		/// </summary>
		public virtual int GetTimeSinceHit()
		{
			return DataWatcher.GetWatchableObjectInt(17);
		}

		/// <summary>
		/// Sets the forward direction of the entity.
		/// </summary>
		public virtual void SetForwardDirection(int par1)
		{
			DataWatcher.UpdateObject(18, Convert.ToInt32(par1));
		}

		/// <summary>
		/// Gets the forward direction of the entity.
		/// </summary>
		public virtual int GetForwardDirection()
		{
			return DataWatcher.GetWatchableObjectInt(18);
		}
	}
}