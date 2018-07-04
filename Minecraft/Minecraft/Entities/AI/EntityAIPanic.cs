namespace net.minecraft.src
{
	public class EntityAIPanic : EntityAIBase
	{
		private EntityCreature Field_48316_a;
		private float Field_48314_b;
		private double Field_48315_c;
		private double Field_48312_d;
		private double Field_48313_e;

		public EntityAIPanic(EntityCreature par1EntityCreature, float par2)
		{
			Field_48316_a = par1EntityCreature;
			Field_48314_b = par2;
			SetMutexBits(1);
		}

		/// <summary>
		/// Returns whether the EntityAIBase should begin execution.
		/// </summary>
		public override bool ShouldExecute()
		{
			if (Field_48316_a.GetAITarget() == null)
			{
				return false;
			}

			Vec3D vec3d = RandomPositionGenerator.Func_48622_a(Field_48316_a, 5, 4);

			if (vec3d == null)
			{
				return false;
			}
			else
			{
				Field_48315_c = vec3d.XCoord;
				Field_48312_d = vec3d.YCoord;
				Field_48313_e = vec3d.ZCoord;
				return true;
			}
		}

		/// <summary>
		/// Execute a one shot task or start executing a continuous task
		/// </summary>
		public override void StartExecuting()
		{
			Field_48316_a.GetNavigator().Func_48666_a(Field_48315_c, Field_48312_d, Field_48313_e, Field_48314_b);
		}

		/// <summary>
		/// Returns whether an in-progress EntityAIBase should continue executing
		/// </summary>
		public override bool ContinueExecuting()
		{
			return !Field_48316_a.GetNavigator().NoPath();
		}
	}
}