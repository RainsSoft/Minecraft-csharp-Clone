namespace net.minecraft.src
{

	public abstract class EntityFlying : EntityLiving
	{
		public EntityFlying(World par1World) : base(par1World)
		{
		}

		/// <summary>
		/// Called when the mob is falling. Calculates and applies fall damage.
		/// </summary>
		protected override void Fall(float f)
		{
		}

		/// <summary>
		/// Moves the entity based on the specified heading.  Args: strafe, forward
		/// </summary>
		public override void MoveEntityWithHeading(float par1, float par2)
		{
			if (IsInWater())
			{
				MoveFlying(par1, par2, 0.02F);
				MoveEntity(MotionX, MotionY, MotionZ);
				MotionX *= 0.80000001192092896F;
				MotionY *= 0.80000001192092896F;
				MotionZ *= 0.80000001192092896F;
			}
			else if (HandleLavaMovement())
			{
				MoveFlying(par1, par2, 0.02F);
				MoveEntity(MotionX, MotionY, MotionZ);
				MotionX *= 0.5F;
				MotionY *= 0.5F;
				MotionZ *= 0.5F;
			}
			else
			{
				float f = 0.91F;

				if (OnGround)
				{
					f = 0.5460001F;
					int i = WorldObj.GetBlockId(MathHelper2.Floor_double(PosX), MathHelper2.Floor_double(BoundingBox.MinY) - 1, MathHelper2.Floor_double(PosZ));

					if (i > 0)
					{
						f = Block.BlocksList[i].Slipperiness * 0.91F;
					}
				}

				float f1 = 0.1627714F / (f * f * f);
				MoveFlying(par1, par2, OnGround ? 0.1F * f1 : 0.02F);
				f = 0.91F;

				if (OnGround)
				{
					f = 0.5460001F;
					int j = WorldObj.GetBlockId(MathHelper2.Floor_double(PosX), MathHelper2.Floor_double(BoundingBox.MinY) - 1, MathHelper2.Floor_double(PosZ));

					if (j > 0)
					{
						f = Block.BlocksList[j].Slipperiness * 0.91F;
					}
				}

				MoveEntity(MotionX, MotionY, MotionZ);
				MotionX *= f;
				MotionY *= f;
				MotionZ *= f;
			}

			Field_705_Q = Field_704_R;
			double d = PosX - PrevPosX;
			double d1 = PosZ - PrevPosZ;
			float f2 = MathHelper2.Sqrt_double(d * d + d1 * d1) * 4F;

			if (f2 > 1.0F)
			{
				f2 = 1.0F;
			}

			Field_704_R += (f2 - Field_704_R) * 0.4F;
			Field_703_S += Field_704_R;
		}

		/// <summary>
		/// returns true if this entity is by a ladder, false otherwise
		/// </summary>
		public override bool IsOnLadder()
		{
			return false;
		}
	}
}