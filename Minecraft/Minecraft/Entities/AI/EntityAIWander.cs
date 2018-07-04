namespace net.minecraft.src
{
	public class EntityAIWander : EntityAIBase
	{
		private EntityCreature Entity;
		private double Field_46098_b;
		private double Field_46099_c;
		private double Field_46097_d;
		private float Field_48317_e;

		public EntityAIWander(EntityCreature par1EntityCreature, float par2)
		{
			Entity = par1EntityCreature;
			Field_48317_e = par2;
			SetMutexBits(1);
		}

		/// <summary>
		/// Returns whether the EntityAIBase should begin execution.
		/// </summary>
		public override bool ShouldExecute()
		{
			if (Entity.GetAge() >= 100)
			{
				return false;
			}

			if (Entity.GetRNG().Next(120) != 0)
			{
				return false;
			}

			Vec3D vec3d = RandomPositionGenerator.Func_48622_a(Entity, 10, 7);

			if (vec3d == null)
			{
				return false;
			}
			else
			{
				Field_46098_b = vec3d.XCoord;
				Field_46099_c = vec3d.YCoord;
				Field_46097_d = vec3d.ZCoord;
				return true;
			}
		}

		/// <summary>
		/// Returns whether an in-progress EntityAIBase should continue executing
		/// </summary>
		public override bool ContinueExecuting()
		{
			return !Entity.GetNavigator().NoPath();
		}

		/// <summary>
		/// Execute a one shot task or start executing a continuous task
		/// </summary>
		public override void StartExecuting()
		{
			Entity.GetNavigator().Func_48666_a(Field_46098_b, Field_46099_c, Field_46097_d, Field_48317_e);
		}
	}
}