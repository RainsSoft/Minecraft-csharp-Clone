namespace net.minecraft.src
{
	public class EntityAICreeperSwell : EntityAIBase
	{
		/// <summary>
		/// The creeper that is swelling. </summary>
		EntityCreeper SwellingCreeper;

		/// <summary>
		/// The creeper's attack target. This is used for the changing of the creeper's state.
		/// </summary>
		EntityLiving CreeperAttackTarget;

		public EntityAICreeperSwell(EntityCreeper par1EntityCreeper)
		{
			SwellingCreeper = par1EntityCreeper;
			SetMutexBits(1);
		}

		/// <summary>
		/// Returns whether the EntityAIBase should begin execution.
		/// </summary>
		public override bool ShouldExecute()
		{
			EntityLiving entityliving = SwellingCreeper.GetAttackTarget();
			return SwellingCreeper.GetCreeperState() > 0 || entityliving != null && SwellingCreeper.GetDistanceSqToEntity(entityliving) < 9D;
		}

		/// <summary>
		/// Execute a one shot task or start executing a continuous task
		/// </summary>
		public override void StartExecuting()
		{
			SwellingCreeper.GetNavigator().ClearPathEntity();
			CreeperAttackTarget = SwellingCreeper.GetAttackTarget();
		}

		/// <summary>
		/// Resets the task
		/// </summary>
		public override void ResetTask()
		{
			CreeperAttackTarget = null;
		}

		/// <summary>
		/// Updates the task
		/// </summary>
		public override void UpdateTask()
		{
			if (CreeperAttackTarget == null)
			{
				SwellingCreeper.SetCreeperState(-1);
				return;
			}

			if (SwellingCreeper.GetDistanceSqToEntity(CreeperAttackTarget) > 49D)
			{
				SwellingCreeper.SetCreeperState(-1);
				return;
			}

			if (!SwellingCreeper.Func_48090_aM().CanSee(CreeperAttackTarget))
			{
				SwellingCreeper.SetCreeperState(-1);
				return;
			}
			else
			{
				SwellingCreeper.SetCreeperState(1);
				return;
			}
		}
	}
}