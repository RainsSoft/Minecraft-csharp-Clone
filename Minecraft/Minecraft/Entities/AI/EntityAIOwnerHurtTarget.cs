namespace net.minecraft.src
{

	public class EntityAIOwnerHurtTarget : EntityAITarget
	{
		EntityTameable Field_48392_a;
		EntityLiving Field_48391_b;

		public EntityAIOwnerHurtTarget(EntityTameable par1EntityTameable) : base(par1EntityTameable, 32F, false)
		{
			Field_48392_a = par1EntityTameable;
			SetMutexBits(1);
		}

		/// <summary>
		/// Returns whether the EntityAIBase should begin execution.
		/// </summary>
		public override bool ShouldExecute()
		{
			if (!Field_48392_a.IsTamed())
			{
				return false;
			}

			EntityLiving entityliving = Field_48392_a.GetOwner();

			if (entityliving == null)
			{
				return false;
			}
			else
			{
				Field_48391_b = entityliving.GetLastAttackingEntity();
				return Func_48376_a(Field_48391_b, false);
			}
		}

		/// <summary>
		/// Execute a one shot task or start executing a continuous task
		/// </summary>
		public override void StartExecuting()
		{
			TaskOwner.SetAttackTarget(Field_48391_b);
			base.StartExecuting();
		}
	}

}