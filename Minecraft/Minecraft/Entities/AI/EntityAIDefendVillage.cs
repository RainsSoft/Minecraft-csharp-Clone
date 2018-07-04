namespace net.minecraft.src
{

	public class EntityAIDefendVillage : EntityAITarget
	{
		EntityIronGolem Irongolem;

		/// <summary>
		/// The aggressor of the iron golem's village which is now the golem's attack target.
		/// </summary>
		EntityLiving VillageAgressorTarget;

		public EntityAIDefendVillage(EntityIronGolem par1EntityIronGolem) : base(par1EntityIronGolem, 16F, false, true)
		{
			Irongolem = par1EntityIronGolem;
			SetMutexBits(1);
		}

		/// <summary>
		/// Returns whether the EntityAIBase should begin execution.
		/// </summary>
		public override bool ShouldExecute()
		{
			Village village = Irongolem.GetVillage();

			if (village == null)
			{
				return false;
			}
			else
			{
				VillageAgressorTarget = village.FindNearestVillageAggressor(Irongolem);
				return Func_48376_a(VillageAgressorTarget, false);
			}
		}

		/// <summary>
		/// Execute a one shot task or start executing a continuous task
		/// </summary>
		public override void StartExecuting()
		{
			Irongolem.SetAttackTarget(VillageAgressorTarget);
			base.StartExecuting();
		}
	}

}