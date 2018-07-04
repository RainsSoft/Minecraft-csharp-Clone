namespace net.minecraft.src
{
	public class EntityAIMoveTowardsTarget : EntityAIBase
	{
		private EntityCreature TheEntity;
		private EntityLiving TargetEntity;
		private double MovePosX;
		private double MovePosY;
		private double MovePosZ;
		private float Field_48330_f;
		private float Field_48331_g;

		public EntityAIMoveTowardsTarget(EntityCreature par1EntityCreature, float par2, float par3)
		{
			TheEntity = par1EntityCreature;
			Field_48330_f = par2;
			Field_48331_g = par3;
			SetMutexBits(1);
		}

		/// <summary>
		/// Returns whether the EntityAIBase should begin execution.
		/// </summary>
		public override bool ShouldExecute()
		{
			TargetEntity = TheEntity.GetAttackTarget();

			if (TargetEntity == null)
			{
				return false;
			}

			if (TargetEntity.GetDistanceSqToEntity(TheEntity) > (double)(Field_48331_g * Field_48331_g))
			{
				return false;
			}

			Vec3D vec3d = RandomPositionGenerator.Func_48620_a(TheEntity, 16, 7, Vec3D.CreateVector(TargetEntity.PosX, TargetEntity.PosY, TargetEntity.PosZ));

			if (vec3d == null)
			{
				return false;
			}
			else
			{
				MovePosX = vec3d.XCoord;
				MovePosY = vec3d.YCoord;
				MovePosZ = vec3d.ZCoord;
				return true;
			}
		}

		/// <summary>
		/// Returns whether an in-progress EntityAIBase should continue executing
		/// </summary>
		public override bool ContinueExecuting()
		{
			return !TheEntity.GetNavigator().NoPath() && TargetEntity.IsEntityAlive() && TargetEntity.GetDistanceSqToEntity(TheEntity) < (double)(Field_48331_g * Field_48331_g);
		}

		/// <summary>
		/// Resets the task
		/// </summary>
		public override void ResetTask()
		{
			TargetEntity = null;
		}

		/// <summary>
		/// Execute a one shot task or start executing a continuous task
		/// </summary>
		public override void StartExecuting()
		{
			TheEntity.GetNavigator().Func_48666_a(MovePosX, MovePosY, MovePosZ, Field_48330_f);
		}
	}
}