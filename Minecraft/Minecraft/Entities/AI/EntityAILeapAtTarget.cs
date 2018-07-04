namespace net.minecraft.src
{
	public class EntityAILeapAtTarget : EntityAIBase
	{
		/// <summary>
		/// The entity that is leaping. </summary>
		EntityLiving Leaper;

		/// <summary>
		/// The entity that the leaper is leaping towards. </summary>
		EntityLiving LeapTarget;

		/// <summary>
		/// The entity's motionY after leaping. </summary>
		float LeapMotionY;

		public EntityAILeapAtTarget(EntityLiving par1EntityLiving, float par2)
		{
			Leaper = par1EntityLiving;
			LeapMotionY = par2;
			SetMutexBits(5);
		}

		/// <summary>
		/// Returns whether the EntityAIBase should begin execution.
		/// </summary>
		public override bool ShouldExecute()
		{
			LeapTarget = Leaper.GetAttackTarget();

			if (LeapTarget == null)
			{
				return false;
			}

			double d = Leaper.GetDistanceSqToEntity(LeapTarget);

			if (d < 4D || d > 16D)
			{
				return false;
			}

			if (!Leaper.OnGround)
			{
				return false;
			}

			return Leaper.GetRNG().Next(5) == 0;
		}

		/// <summary>
		/// Returns whether an in-progress EntityAIBase should continue executing
		/// </summary>
		public override bool ContinueExecuting()
		{
			return !Leaper.OnGround;
		}

		/// <summary>
		/// Execute a one shot task or start executing a continuous task
		/// </summary>
		public override void StartExecuting()
		{
            float d = LeapTarget.PosX - Leaper.PosX;
            float d1 = LeapTarget.PosZ - Leaper.PosZ;
			float f = MathHelper2.Sqrt_double(d * d + d1 * d1);
			Leaper.MotionX += (d / f) * 0.5F * 0.80000001192092896F + Leaper.MotionX * 0.20000000298023224F;
			Leaper.MotionZ += (d1 / f) * 0.5F * 0.80000001192092896F + Leaper.MotionZ * 0.20000000298023224F;
			Leaper.MotionY = LeapMotionY;
		}
	}
}