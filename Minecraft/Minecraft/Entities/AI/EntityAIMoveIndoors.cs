namespace net.minecraft.src
{
	public class EntityAIMoveIndoors : EntityAIBase
	{
		private EntityCreature EntityObj;
		private VillageDoorInfo DoorInfo;
		private int InsidePosX;
		private int InsidePosZ;

		public EntityAIMoveIndoors(EntityCreature par1EntityCreature)
		{
			InsidePosX = -1;
			InsidePosZ = -1;
			EntityObj = par1EntityCreature;
			SetMutexBits(1);
		}

		/// <summary>
		/// Returns whether the EntityAIBase should begin execution.
		/// </summary>
		public override bool ShouldExecute()
		{
			if (EntityObj.WorldObj.IsDaytime() && !EntityObj.WorldObj.IsRaining() || EntityObj.WorldObj.WorldProvider.HasNoSky)
			{
				return false;
			}

			if (EntityObj.GetRNG().Next(50) != 0)
			{
				return false;
			}

			if (InsidePosX != -1 && EntityObj.GetDistanceSq(InsidePosX, EntityObj.PosY, InsidePosZ) < 4D)
			{
				return false;
			}

			Village village = EntityObj.WorldObj.VillageCollectionObj.FindNearestVillage(MathHelper2.Floor_double(EntityObj.PosX), MathHelper2.Floor_double(EntityObj.PosY), MathHelper2.Floor_double(EntityObj.PosZ), 14);

			if (village == null)
			{
				return false;
			}
			else
			{
				DoorInfo = village.FindNearestDoorUnrestricted(MathHelper2.Floor_double(EntityObj.PosX), MathHelper2.Floor_double(EntityObj.PosY), MathHelper2.Floor_double(EntityObj.PosZ));
				return DoorInfo != null;
			}
		}

		/// <summary>
		/// Returns whether an in-progress EntityAIBase should continue executing
		/// </summary>
		public override bool ContinueExecuting()
		{
			return !EntityObj.GetNavigator().NoPath();
		}

		/// <summary>
		/// Execute a one shot task or start executing a continuous task
		/// </summary>
		public override void StartExecuting()
		{
			InsidePosX = -1;

			if (EntityObj.GetDistanceSq(DoorInfo.GetInsidePosX(), DoorInfo.PosY, DoorInfo.GetInsidePosZ()) > 256D)
			{
				Vec3D vec3d = RandomPositionGenerator.Func_48620_a(EntityObj, 14, 3, Vec3D.CreateVector((double)DoorInfo.GetInsidePosX() + 0.5D, DoorInfo.GetInsidePosY(), (double)DoorInfo.GetInsidePosZ() + 0.5D));

				if (vec3d != null)
				{
					EntityObj.GetNavigator().Func_48666_a(vec3d.XCoord, vec3d.YCoord, vec3d.ZCoord, 0.3F);
				}
			}
			else
			{
				EntityObj.GetNavigator().Func_48666_a((double)DoorInfo.GetInsidePosX() + 0.5D, DoorInfo.GetInsidePosY(), (double)DoorInfo.GetInsidePosZ() + 0.5D, 0.3F);
			}
		}

		/// <summary>
		/// Resets the task
		/// </summary>
		public override void ResetTask()
		{
			InsidePosX = DoorInfo.GetInsidePosX();
			InsidePosZ = DoorInfo.GetInsidePosZ();
			DoorInfo = null;
		}
	}
}