using System;

namespace net.minecraft.src
{
	public class EntitySquid : EntityWaterMob
	{
		public float Field_21089_a;
		public float Field_21088_b;
		public float Field_21087_c;
		public float Field_21086_f;
		public float Field_21085_g;
		public float Field_21084_h;

		/// <summary>
		/// angle of the tentacles in radians </summary>
		public float TentacleAngle;

		/// <summary>
		/// the last calculated angle of the tentacles in radians </summary>
		public float LastTentacleAngle;
		private float RandomMotionSpeed;
		private float Field_21080_l;
		private float Field_21079_m;
		private float RandomMotionVecX;
		private float RandomMotionVecY;
		private float RandomMotionVecZ;

		public EntitySquid(World par1World) : base(par1World)
		{
			Field_21089_a = 0.0F;
			Field_21088_b = 0.0F;
			Field_21087_c = 0.0F;
			Field_21086_f = 0.0F;
			Field_21085_g = 0.0F;
			Field_21084_h = 0.0F;
			TentacleAngle = 0.0F;
			LastTentacleAngle = 0.0F;
			RandomMotionSpeed = 0.0F;
			Field_21080_l = 0.0F;
			Field_21079_m = 0.0F;
			RandomMotionVecX = 0.0F;
			RandomMotionVecY = 0.0F;
			RandomMotionVecZ = 0.0F;
			Texture = "/mob/squid.png";
			SetSize(0.95F, 0.95F);
			Field_21080_l = (1.0F / (Rand.NextFloat() + 1.0F)) * 0.2F;
		}

		public override int GetMaxHealth()
		{
			return 10;
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
		/// Returns the sound this mob makes while it's alive.
		/// </summary>
		protected override string GetLivingSound()
		{
			return null;
		}

		/// <summary>
		/// Returns the sound this mob makes when it is hurt.
		/// </summary>
		protected override string GetHurtSound()
		{
			return null;
		}

		/// <summary>
		/// Returns the sound this mob makes on death.
		/// </summary>
		protected override string GetDeathSound()
		{
			return null;
		}

		/// <summary>
		/// Returns the volume for the sounds this mob makes.
		/// </summary>
		protected override float GetSoundVolume()
		{
			return 0.4F;
		}

		/// <summary>
		/// Returns the item ID for the item the mob drops on death.
		/// </summary>
		protected override int GetDropItemId()
		{
			return 0;
		}

		/// <summary>
		/// Drop 0-2 items of this living's type
		/// </summary>
		protected override void DropFewItems(bool par1, int par2)
		{
			int i = Rand.Next(3 + par2) + 1;

			for (int j = 0; j < i; j++)
			{
				EntityDropItem(new ItemStack(Item.DyePowder, 1, 0), 0.0F);
			}
		}

		/// <summary>
		/// Called when a player interacts with a mob. e.g. gets milk from a cow, gets into the saddle on a pig.
		/// </summary>
		public override bool Interact(EntityPlayer par1EntityPlayer)
		{
			return base.Interact(par1EntityPlayer);
		}

		/// <summary>
		/// Checks if this entity is inside water (if inWater field is true as a result of handleWaterMovement() returning
		/// true)
		/// </summary>
		public override bool IsInWater()
		{
			return WorldObj.HandleMaterialAcceleration(BoundingBox.Expand(0.0F, -0.60000002384185791F, 0.0F), Material.Water, this);
		}

		/// <summary>
		/// Called frequently so the entity can update its state every tick as required. For example, zombies and skeletons
		/// use this to react to sunlight and start to burn.
		/// </summary>
		public override void OnLivingUpdate()
		{
			base.OnLivingUpdate();
			Field_21088_b = Field_21089_a;
			Field_21086_f = Field_21087_c;
			Field_21084_h = Field_21085_g;
			LastTentacleAngle = TentacleAngle;
			Field_21085_g += Field_21080_l;

			if (Field_21085_g > ((float)Math.PI * 2F))
			{
				Field_21085_g -= ((float)Math.PI * 2F);

				if (Rand.Next(10) == 0)
				{
					Field_21080_l = (1.0F / (Rand.NextFloat() + 1.0F)) * 0.2F;
				}
			}

			if (IsInWater())
			{
				if (Field_21085_g < (float)Math.PI)
				{
					float f = Field_21085_g / (float)Math.PI;
					TentacleAngle = MathHelper2.Sin(f * f * (float)Math.PI) * (float)Math.PI * 0.25F;

					if ((double)f > 0.75D)
					{
						RandomMotionSpeed = 1.0F;
						Field_21079_m = 1.0F;
					}
					else
					{
						Field_21079_m = Field_21079_m * 0.8F;
					}
				}
				else
				{
					TentacleAngle = 0.0F;
					RandomMotionSpeed = RandomMotionSpeed * 0.9F;
					Field_21079_m = Field_21079_m * 0.99F;
				}

				if (!WorldObj.IsRemote)
				{
					MotionX = RandomMotionVecX * RandomMotionSpeed;
					MotionY = RandomMotionVecY * RandomMotionSpeed;
					MotionZ = RandomMotionVecZ * RandomMotionSpeed;
				}

				float f1 = MathHelper2.Sqrt_double(MotionX * MotionX + MotionZ * MotionZ);
				RenderYawOffset += ((-(float)Math.Atan2(MotionX, MotionZ) * 180F) / (float)Math.PI - RenderYawOffset) * 0.1F;
				RotationYaw = RenderYawOffset;
				Field_21087_c = Field_21087_c + (float)Math.PI * Field_21079_m * 1.5F;
				Field_21089_a += ((-(float)Math.Atan2(f1, MotionY) * 180F) / (float)Math.PI - Field_21089_a) * 0.1F;
			}
			else
			{
				TentacleAngle = MathHelper2.Abs(MathHelper2.Sin(Field_21085_g)) * (float)Math.PI * 0.25F;

				if (!WorldObj.IsRemote)
				{
					MotionX = 0.0F;
					MotionY -= 0.080000000000000002F;
					MotionY *= 0.98000001907348633F;
					MotionZ = 0.0F;
				}

				Field_21089_a += (-90F - Field_21089_a) * 0.02F;
			}
		}

		/// <summary>
		/// Moves the entity based on the specified heading.  Args: strafe, forward
		/// </summary>
		public override void MoveEntityWithHeading(float par1, float par2)
		{
			MoveEntity(MotionX, MotionY, MotionZ);
		}

        public override void UpdateEntityActionState()
		{
			EntityAge++;

			if (EntityAge > 100)
			{
				RandomMotionVecX = RandomMotionVecY = RandomMotionVecZ = 0.0F;
			}
			else if (Rand.Next(50) == 0 || !InWater || RandomMotionVecX == 0.0F && RandomMotionVecY == 0.0F && RandomMotionVecZ == 0.0F)
			{
				float f = Rand.NextFloat() * (float)Math.PI * 2.0F;
				RandomMotionVecX = MathHelper2.Cos(f) * 0.2F;
				RandomMotionVecY = -0.1F + Rand.NextFloat() * 0.2F;
				RandomMotionVecZ = MathHelper2.Sin(f) * 0.2F;
			}

			DespawnEntity();
		}

		/// <summary>
		/// Checks if the entity's current position is a valid location to spawn this entity.
		/// </summary>
		public override bool GetCanSpawnHere()
		{
			return PosY > 45D && PosY < 63D && base.GetCanSpawnHere();
		}
	}
}