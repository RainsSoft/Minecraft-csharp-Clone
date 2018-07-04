namespace net.minecraft.src
{

	public class EntityAIOpenDoor : EntityAIDoorInteract
	{
		bool Field_48328_i;
		int Field_48327_j;

		public EntityAIOpenDoor(EntityLiving par1EntityLiving, bool par2) : base(par1EntityLiving)
		{
			TheEntity = par1EntityLiving;
			Field_48328_i = par2;
		}

		/// <summary>
		/// Returns whether an in-progress EntityAIBase should continue executing
		/// </summary>
		public override bool ContinueExecuting()
		{
			return Field_48328_i && Field_48327_j > 0 && base.ContinueExecuting();
		}

		/// <summary>
		/// Execute a one shot task or start executing a continuous task
		/// </summary>
		public override void StartExecuting()
		{
			Field_48327_j = 20;
			TargetDoor.OnPoweredBlockChange(TheEntity.WorldObj, EntityPosX, EntityPosY, EntityPosZ, true);
		}

		/// <summary>
		/// Resets the task
		/// </summary>
		public override void ResetTask()
		{
			if (Field_48328_i)
			{
				TargetDoor.OnPoweredBlockChange(TheEntity.WorldObj, EntityPosX, EntityPosY, EntityPosZ, false);
			}
		}

		/// <summary>
		/// Updates the task
		/// </summary>
		public override void UpdateTask()
		{
			Field_48327_j--;
			base.UpdateTask();
		}
	}

}