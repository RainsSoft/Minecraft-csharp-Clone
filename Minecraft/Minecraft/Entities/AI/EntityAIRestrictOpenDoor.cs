namespace net.minecraft.src
{
	public class EntityAIRestrictOpenDoor : EntityAIBase
	{
		private EntityCreature EntityObj;
		private VillageDoorInfo FrontDoor;

		public EntityAIRestrictOpenDoor(EntityCreature par1EntityCreature)
		{
			EntityObj = par1EntityCreature;
		}

		/// <summary>
		/// Returns whether the EntityAIBase should begin execution.
		/// </summary>
		public override bool ShouldExecute()
		{
			if (EntityObj.WorldObj.IsDaytime())
			{
				return false;
			}

			Village village = EntityObj.WorldObj.VillageCollectionObj.FindNearestVillage(MathHelper2.Floor_double(EntityObj.PosX), MathHelper2.Floor_double(EntityObj.PosY), MathHelper2.Floor_double(EntityObj.PosZ), 16);

			if (village == null)
			{
				return false;
			}

			FrontDoor = village.FindNearestDoor(MathHelper2.Floor_double(EntityObj.PosX), MathHelper2.Floor_double(EntityObj.PosY), MathHelper2.Floor_double(EntityObj.PosZ));

			if (FrontDoor == null)
			{
				return false;
			}
			else
			{
				return (double)FrontDoor.GetInsideDistanceSquare(MathHelper2.Floor_double(EntityObj.PosX), MathHelper2.Floor_double(EntityObj.PosY), MathHelper2.Floor_double(EntityObj.PosZ)) < 2.25D;
			}
		}

		/// <summary>
		/// Returns whether an in-progress EntityAIBase should continue executing
		/// </summary>
		public override bool ContinueExecuting()
		{
			if (EntityObj.WorldObj.IsDaytime())
			{
				return false;
			}
			else
			{
				return !FrontDoor.IsDetachedFromVillageFlag && FrontDoor.IsInside(MathHelper2.Floor_double(EntityObj.PosX), MathHelper2.Floor_double(EntityObj.PosZ));
			}
		}

		/// <summary>
		/// Execute a one shot task or start executing a continuous task
		/// </summary>
		public override void StartExecuting()
		{
			EntityObj.GetNavigator().SetBreakDoors(false);
			EntityObj.GetNavigator().Func_48663_c(false);
		}

		/// <summary>
		/// Resets the task
		/// </summary>
		public override void ResetTask()
		{
			EntityObj.GetNavigator().SetBreakDoors(true);
			EntityObj.GetNavigator().Func_48663_c(true);
			FrontDoor = null;
		}

		/// <summary>
		/// Updates the task
		/// </summary>
		public override void UpdateTask()
		{
			FrontDoor.IncrementDoorOpeningRestrictionCounter();
		}
	}
}