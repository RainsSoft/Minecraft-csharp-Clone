namespace net.minecraft.src
{
	public class EntityAIRestrictSun : EntityAIBase
	{
		private EntityCreature TheEntity;

		public EntityAIRestrictSun(EntityCreature par1EntityCreature)
		{
			TheEntity = par1EntityCreature;
		}

		/// <summary>
		/// Returns whether the EntityAIBase should begin execution.
		/// </summary>
		public override bool ShouldExecute()
		{
			return TheEntity.WorldObj.IsDaytime();
		}

		/// <summary>
		/// Execute a one shot task or start executing a continuous task
		/// </summary>
		public override void StartExecuting()
		{
			TheEntity.GetNavigator().Func_48680_d(true);
		}

		/// <summary>
		/// Resets the task
		/// </summary>
		public override void ResetTask()
		{
			TheEntity.GetNavigator().Func_48680_d(false);
		}
	}
}