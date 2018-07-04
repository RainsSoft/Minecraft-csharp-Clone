namespace net.minecraft.src
{
	public class EntityAIMoveTwardsRestriction : EntityAIBase
	{
		private EntityCreature TheEntity;
		private double MovePosX;
		private double MovePosY;
		private double MovePosZ;
		private float Field_48352_e;

		public EntityAIMoveTwardsRestriction(EntityCreature par1EntityCreature, float par2)
		{
			TheEntity = par1EntityCreature;
			Field_48352_e = par2;
			SetMutexBits(1);
		}

		/// <summary>
		/// Returns whether the EntityAIBase should begin execution.
		/// </summary>
		public override bool ShouldExecute()
		{
			if (TheEntity.IsWithinHomeDistanceCurrentPosition())
			{
				return false;
			}

			ChunkCoordinates chunkcoordinates = TheEntity.GetHomePosition();
			Vec3D vec3d = RandomPositionGenerator.Func_48620_a(TheEntity, 16, 7, Vec3D.CreateVector(chunkcoordinates.PosX, chunkcoordinates.PosY, chunkcoordinates.PosZ));

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
			return !TheEntity.GetNavigator().NoPath();
		}

		/// <summary>
		/// Execute a one shot task or start executing a continuous task
		/// </summary>
		public override void StartExecuting()
		{
			TheEntity.GetNavigator().Func_48666_a(MovePosX, MovePosY, MovePosZ, Field_48352_e);
		}
	}
}