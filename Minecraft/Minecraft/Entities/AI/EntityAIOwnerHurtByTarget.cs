namespace net.minecraft.src
{

	public class EntityAIOwnerHurtByTarget : EntityAITarget
	{
		EntityTameable Field_48394_a;
		EntityLiving Field_48393_b;

		public EntityAIOwnerHurtByTarget(EntityTameable par1EntityTameable) : base(par1EntityTameable, 32F, false)
		{
			Field_48394_a = par1EntityTameable;
			SetMutexBits(1);
		}

		/// <summary>
		/// Returns whether the EntityAIBase should begin execution.
		/// </summary>
		public override bool ShouldExecute()
		{
			if (!Field_48394_a.IsTamed())
			{
				return false;
			}

			EntityLiving entityliving = Field_48394_a.GetOwner();

			if (entityliving == null)
			{
				return false;
			}
			else
			{
				Field_48393_b = entityliving.GetAITarget();
				return Func_48376_a(Field_48393_b, false);
			}
		}

		/// <summary>
		/// Execute a one shot task or start executing a continuous task
		/// </summary>
		public override void StartExecuting()
		{
			TaskOwner.SetAttackTarget(Field_48393_b);
			base.StartExecuting();
		}
	}

}