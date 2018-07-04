namespace net.minecraft.src
{
	public class EntityAIBreakDoor : EntityAIDoorInteract
	{
		private int Field_48329_i;

		public EntityAIBreakDoor(EntityLiving par1EntityLiving) : base(par1EntityLiving)
		{
		}

		/// <summary>
		/// Returns whether the EntityAIBase should begin execution.
		/// </summary>
		public override bool ShouldExecute()
		{
			if (!base.ShouldExecute())
			{
				return false;
			}
			else
			{
				return !TargetDoor.Func_48213_h(TheEntity.WorldObj, EntityPosX, EntityPosY, EntityPosZ);
			}
		}

		/// <summary>
		/// Execute a one shot task or start executing a continuous task
		/// </summary>
		public override void StartExecuting()
		{
			base.StartExecuting();
			Field_48329_i = 240;
		}

		/// <summary>
		/// Returns whether an in-progress EntityAIBase should continue executing
		/// </summary>
		public override bool ContinueExecuting()
		{
			double d = TheEntity.GetDistanceSq(EntityPosX, EntityPosY, EntityPosZ);
			return Field_48329_i >= 0 && !TargetDoor.Func_48213_h(TheEntity.WorldObj, EntityPosX, EntityPosY, EntityPosZ) && d < 4D;
		}

		/// <summary>
		/// Updates the task
		/// </summary>
		public override void UpdateTask()
		{
			base.UpdateTask();

			if (TheEntity.GetRNG().Next(20) == 0)
			{
				TheEntity.WorldObj.PlayAuxSFX(1010, EntityPosX, EntityPosY, EntityPosZ, 0);
			}

			if (--Field_48329_i == 0 && TheEntity.WorldObj.DifficultySetting == 3)
			{
				TheEntity.WorldObj.SetBlockWithNotify(EntityPosX, EntityPosY, EntityPosZ, 0);
				TheEntity.WorldObj.PlayAuxSFX(1012, EntityPosX, EntityPosY, EntityPosZ, 0);
				TheEntity.WorldObj.PlayAuxSFX(2001, EntityPosX, EntityPosY, EntityPosZ, TargetDoor.BlockID);
			}
		}
	}
}