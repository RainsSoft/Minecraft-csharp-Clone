using System;

namespace net.minecraft.src
{
	public class EntityEnderEye : Entity
	{
		public int Field_40096_a;
		private double Field_40094_b;
		private double Field_40095_c;
		private double Field_40091_d;
		private int DespawnTimer;
		private bool ShatterOrDrop;

		public EntityEnderEye(World par1World) : base(par1World)
		{
			Field_40096_a = 0;
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
            float d = BoundingBox.GetAverageEdgeLength() * 4;
			d *= 64;
			return par1 < d * d;
		}

        public EntityEnderEye(World par1World, float par2, float par4, float par6)
            : base(par1World)
		{
			Field_40096_a = 0;
			DespawnTimer = 0;
			SetSize(0.25F, 0.25F);
			SetPosition(par2, par4, par6);
			YOffset = 0.0F;
		}

		public virtual void Func_40090_a(double par1, int par3, double par4)
		{
			double d = par1 - PosX;
			double d1 = par4 - PosZ;
			float f = MathHelper2.Sqrt_double(d * d + d1 * d1);

			if (f > 12F)
			{
				Field_40094_b = PosX + (d / (double)f) * 12D;
				Field_40091_d = PosZ + (d1 / (double)f) * 12D;
				Field_40095_c = PosY + 8D;
			}
			else
			{
				Field_40094_b = par1;
				Field_40095_c = par3;
				Field_40091_d = par4;
			}

			DespawnTimer = 0;
			ShatterOrDrop = Rand.Next(5) > 0;
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

			if (!WorldObj.IsRemote)
			{
				double d = Field_40094_b - PosX;
				double d1 = Field_40091_d - PosZ;
				float f2 = (float)Math.Sqrt(d * d + d1 * d1);
				float f3 = (float)Math.Atan2(d1, d);
                float d2 = f + (f2 - f) * 0.0025000000000000001F;

				if (f2 < 1.0F)
				{
					d2 *= 0.80000000000000004F;
					MotionY *= 0.80000000000000004F;
				}

				MotionX = (float)Math.Cos(f3) * d2;
				MotionZ = (float)Math.Sin(f3) * d2;

				if (PosY < Field_40095_c)
				{
					MotionY = MotionY + (1 - MotionY) * 0.014999999664723873F;
				}
				else
				{
					MotionY = MotionY + (-1 - MotionY) * 0.014999999664723873F;
				}
			}

			float f1 = 0.25F;

			if (IsInWater())
			{
				for (int i = 0; i < 4; i++)
				{
					WorldObj.SpawnParticle("bubble", PosX - MotionX * (double)f1, PosY - MotionY * (double)f1, PosZ - MotionZ * (double)f1, MotionX, MotionY, MotionZ);
				}
			}
			else
			{
				WorldObj.SpawnParticle("portal", ((PosX - MotionX * (double)f1) + Rand.NextDouble() * 0.59999999999999998D) - 0.29999999999999999D, PosY - MotionY * (double)f1 - 0.5D, ((PosZ - MotionZ * (double)f1) + Rand.NextDouble() * 0.59999999999999998D) - 0.29999999999999999D, MotionX, MotionY, MotionZ);
			}

			if (!WorldObj.IsRemote)
			{
				SetPosition(PosX, PosY, PosZ);
				DespawnTimer++;

				if (DespawnTimer > 80 && !WorldObj.IsRemote)
				{
					SetDead();

					if (ShatterOrDrop)
					{
						WorldObj.SpawnEntityInWorld(new EntityItem(WorldObj, PosX, PosY, PosZ, new ItemStack(Item.EyeOfEnder)));
					}
					else
					{
						WorldObj.PlayAuxSFX(2003, (int)Math.Round(PosX), (int)Math.Round(PosY), (int)Math.Round(PosZ), 0);
					}
				}
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

		/// <summary>
		/// If returns false, the item will not inflict any damage against entities.
		/// </summary>
		public override bool CanAttackWithItem()
		{
			return false;
		}
	}

}