namespace net.minecraft.src
{
	public class EntityAILookAtVillager : EntityAIBase
	{
		private EntityIronGolem TheGolem;
		private EntityVillager TheVillager;
		private int Field_48405_c;

		public EntityAILookAtVillager(EntityIronGolem par1EntityIronGolem)
		{
			TheGolem = par1EntityIronGolem;
			SetMutexBits(3);
		}

		/// <summary>
		/// Returns whether the EntityAIBase should begin execution.
		/// </summary>
		public override bool ShouldExecute()
		{
			if (!TheGolem.WorldObj.IsDaytime())
			{
				return false;
			}

			if (TheGolem.GetRNG().Next(8000) != 0)
			{
				return false;
			}
			else
			{
				TheVillager = (EntityVillager)TheGolem.WorldObj.FindNearestEntityWithinAABB(typeof(net.minecraft.src.EntityVillager), TheGolem.BoundingBox.Expand(6, 2, 6), TheGolem);
				return TheVillager != null;
			}
		}

		/// <summary>
		/// Returns whether an in-progress EntityAIBase should continue executing
		/// </summary>
		public override bool ContinueExecuting()
		{
			return Field_48405_c > 0;
		}

		/// <summary>
		/// Execute a one shot task or start executing a continuous task
		/// </summary>
		public override void StartExecuting()
		{
			Field_48405_c = 400;
			TheGolem.Func_48116_a(true);
		}

		/// <summary>
		/// Resets the task
		/// </summary>
		public override void ResetTask()
		{
			TheGolem.Func_48116_a(false);
			TheVillager = null;
		}

		/// <summary>
		/// Updates the task
		/// </summary>
		public override void UpdateTask()
		{
			TheGolem.GetLookHelper().SetLookPositionWithEntity(TheVillager, 30F, 30F);
			Field_48405_c--;
		}
	}
}