using System;

namespace net.minecraft.src
{
	public class EntityBlaze : EntityMob
	{
		/// <summary>
		/// Random offset used in floating behaviour </summary>
		private float HeightOffset;

		/// <summary>
		/// ticks until heightOffset is randomized </summary>
		private int HeightOffsetUpdateTime;
		private int Field_40152_d;

		public EntityBlaze(World par1World) : base(par1World)
		{
			HeightOffset = 0.5F;
			Texture = "/mob/fire.png";
			isImmuneToFire_Renamed = true;
			AttackStrength = 6;
			ExperienceValue = 10;
		}

		public override int GetMaxHealth()
		{
			return 20;
		}

		protected override void EntityInit()
		{
			base.EntityInit();
			DataWatcher.AddObject(16, new sbyte?((sbyte)0));
		}

		/// <summary>
		/// Returns the sound this mob makes while it's alive.
		/// </summary>
		protected override string GetLivingSound()
		{
			return "mob.blaze.breathe";
		}

		/// <summary>
		/// Returns the sound this mob makes when it is hurt.
		/// </summary>
		protected override string GetHurtSound()
		{
			return "mob.blaze.hit";
		}

		/// <summary>
		/// Returns the sound this mob makes on death.
		/// </summary>
		protected override string GetDeathSound()
		{
			return "mob.blaze.death";
		}

		/// <summary>
		/// Called when the entity is attacked.
		/// </summary>
		public override bool AttackEntityFrom(DamageSource par1DamageSource, int par2)
		{
			return base.AttackEntityFrom(par1DamageSource, par2);
		}

		/// <summary>
		/// Called when the mob's health reaches 0.
		/// </summary>
		public override void OnDeath(DamageSource par1DamageSource)
		{
			base.OnDeath(par1DamageSource);
		}

		public override int GetBrightnessForRender(float par1)
		{
			return 0xf000f0;
		}

		/// <summary>
		/// Gets how bright this entity is.
		/// </summary>
		public override float GetBrightness(float par1)
		{
			return 1.0F;
		}

		/// <summary>
		/// Called frequently so the entity can update its state every tick as required. For example, zombies and skeletons
		/// use this to react to sunlight and start to burn.
		/// </summary>
		public override void OnLivingUpdate()
		{
			if (!WorldObj.IsRemote)
			{
				if (IsWet())
				{
					AttackEntityFrom(DamageSource.Drown, 1);
				}

				HeightOffsetUpdateTime--;

				if (HeightOffsetUpdateTime <= 0)
				{
					HeightOffsetUpdateTime = 100;
					HeightOffset = 0.5F + (float)Rand.NextGaussian() * 3F;
				}

				if (GetEntityToAttack() != null && GetEntityToAttack().PosY + (double)GetEntityToAttack().GetEyeHeight() > PosY + (double)GetEyeHeight() + (double)HeightOffset)
				{
					MotionY = MotionY + (0.30000001192092896F - MotionY) * 0.30000001192092896F;
				}
			}

			if (Rand.Next(24) == 0)
			{
				WorldObj.PlaySoundEffect(PosX + 0.5D, PosY + 0.5D, PosZ + 0.5D, "fire.fire", 1.0F + Rand.NextFloat(), Rand.NextFloat() * 0.7F + 0.3F);
			}

			if (!OnGround && MotionY < 0.0F)
			{
				MotionY *= 0.59999999999999998F;
			}

			for (int i = 0; i < 2; i++)
			{
				WorldObj.SpawnParticle("largesmoke", PosX + (Rand.NextDouble() - 0.5D) * (double)Width, PosY + Rand.NextDouble() * (double)Height, PosZ + (Rand.NextDouble() - 0.5D) * (double)Width, 0.0F, 0.0F, 0.0F);
			}

			base.OnLivingUpdate();
		}

		/// <summary>
		/// Basic mob attack. Default to touch of death in EntityCreature. Overridden by each mob to define their attack.
		/// </summary>
		protected override void AttackEntity(Entity par1Entity, float par2)
		{
			if (AttackTime <= 0 && par2 < 2.0F && par1Entity.BoundingBox.MaxY > BoundingBox.MinY && par1Entity.BoundingBox.MinY < BoundingBox.MaxY)
			{
				AttackTime = 20;
				AttackEntityAsMob(par1Entity);
			}
			else if (par2 < 30F)
			{
                float d = par1Entity.PosX - PosX;
                float d1 = (par1Entity.BoundingBox.MinY + (par1Entity.Height / 2.0F)) - (PosY + (Height / 2.0F));
                float d2 = par1Entity.PosZ - PosZ;

				if (AttackTime == 0)
				{
					Field_40152_d++;

					if (Field_40152_d == 1)
					{
						AttackTime = 60;
						Func_40150_a(true);
					}
					else if (Field_40152_d <= 4)
					{
						AttackTime = 6;
					}
					else
					{
						AttackTime = 100;
						Field_40152_d = 0;
						Func_40150_a(false);
					}

					if (Field_40152_d > 1)
					{
						float f = MathHelper2.Sqrt_float(par2) * 0.5F;
						WorldObj.PlayAuxSFXAtEntity(null, 1009, (int)PosX, (int)PosY, (int)PosZ, 0);

						for (int i = 0; i < 1; i++)
						{
							EntitySmallFireball entitysmallfireball = new EntitySmallFireball(WorldObj, this, d + Rand.NextGaussian() * f, d1, d2 + Rand.NextGaussian() * f);
							entitysmallfireball.PosY = PosY + (Height / 2.0F) + 0.5F;
							WorldObj.SpawnEntityInWorld(entitysmallfireball);
						}
					}
				}

				RotationYaw = (float)((Math.Atan2(d2, d) * 180D) / Math.PI) - 90F;
				HasAttacked = true;
			}
		}

		/// <summary>
		/// Called when the mob is falling. Calculates and applies fall damage.
		/// </summary>
		protected override void Fall(float f)
		{
		}

		/// <summary>
		/// (abstract) Protected helper method to write subclass entity data to NBT.
		/// </summary>
		public override void WriteEntityToNBT(NBTTagCompound par1NBTTagCompound)
		{
			base.WriteEntityToNBT(par1NBTTagCompound);
		}

		/// <summary>
		/// (abstract) Protected helper method to read subclass entity data from NBT.
		/// </summary>
		public override void ReadEntityFromNBT(NBTTagCompound par1NBTTagCompound)
		{
			base.ReadEntityFromNBT(par1NBTTagCompound);
		}

		/// <summary>
		/// Returns the item ID for the item the mob drops on death.
		/// </summary>
		protected override int GetDropItemId()
		{
			return Item.BlazeRod.ShiftedIndex;
		}

		/// <summary>
		/// Returns true if the entity is on fire. Used by render to add the fire effect on rendering.
		/// </summary>
		public override bool IsBurning()
		{
			return Func_40151_ac();
		}

		/// <summary>
		/// Drop 0-2 items of this living's type
		/// </summary>
		protected override void DropFewItems(bool par1, int par2)
		{
			if (par1)
			{
				int i = Rand.Next(2 + par2);

				for (int j = 0; j < i; j++)
				{
					DropItem(Item.BlazeRod.ShiftedIndex, 1);
				}
			}
		}

		public virtual bool Func_40151_ac()
		{
			return (DataWatcher.GetWatchableObjectByte(16) & 1) != 0;
		}

		public virtual void Func_40150_a(bool par1)
		{
			byte byte0 = DataWatcher.GetWatchableObjectByte(16);

			if (par1)
			{
				byte0 |= 1;
			}
			else
			{
				byte0 &= 0xfe;
			}

			DataWatcher.UpdateObject(16, byte0);
		}

		/// <summary>
		/// Checks to make sure the light is not too bright where the mob is spawning
		/// </summary>
		protected override bool IsValidLightLevel()
		{
			return true;
		}
	}
}