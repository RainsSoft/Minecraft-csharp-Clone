namespace net.minecraft.src
{
	public class EntityAISit : EntityAIBase
	{
		private EntityTameable TheEntity;
		private bool Field_48408_b;

		public EntityAISit(EntityTameable par1EntityTameable)
		{
			Field_48408_b = false;
			TheEntity = par1EntityTameable;
			SetMutexBits(5);
		}

		/// <summary>
		/// Returns whether the EntityAIBase should begin execution.
		/// </summary>
		public override bool ShouldExecute()
		{
			if (!TheEntity.IsTamed())
			{
				return false;
			}

			if (TheEntity.IsInWater())
			{
				return false;
			}

			if (!TheEntity.OnGround)
			{
				return false;
			}

			EntityLiving entityliving = TheEntity.GetOwner();

			if (entityliving == null)
			{
				return true;
			}

			if (TheEntity.GetDistanceSqToEntity(entityliving) < 144D && entityliving.GetAITarget() != null)
			{
				return false;
			}
			else
			{
				return Field_48408_b;
			}
		}

		/// <summary>
		/// Execute a one shot task or start executing a continuous task
		/// </summary>
		public override void StartExecuting()
		{
			TheEntity.GetNavigator().ClearPathEntity();
			TheEntity.Func_48140_f(true);
		}

		/// <summary>
		/// Resets the task
		/// </summary>
		public override void ResetTask()
		{
			TheEntity.Func_48140_f(false);
		}

		public virtual void Func_48407_a(bool par1)
		{
			Field_48408_b = par1;
		}
	}
}