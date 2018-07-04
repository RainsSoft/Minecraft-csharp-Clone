namespace net.minecraft.src
{
	public class EntityAISwimming : EntityAIBase
	{
		private EntityLiving TheEntity;

		public EntityAISwimming(EntityLiving par1EntityLiving)
		{
			TheEntity = par1EntityLiving;
			SetMutexBits(4);
			par1EntityLiving.GetNavigator().Func_48669_e(true);
		}

		/// <summary>
		/// Returns whether the EntityAIBase should begin execution.
		/// </summary>
		public override bool ShouldExecute()
		{
			return TheEntity.IsInWater() || TheEntity.HandleLavaMovement();
		}

		/// <summary>
		/// Updates the task
		/// </summary>
		public override void UpdateTask()
		{
			if (TheEntity.GetRNG().NextFloat() < 0.8F)
			{
				TheEntity.GetJumpHelper().SetJumping();
			}
		}
	}
}