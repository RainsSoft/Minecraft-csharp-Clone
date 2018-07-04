using System;

namespace net.minecraft.src
{
	public abstract class EntityAIDoorInteract : EntityAIBase
	{
		protected EntityLiving TheEntity;
		protected int EntityPosX;
		protected int EntityPosY;
		protected int EntityPosZ;
		protected BlockDoor TargetDoor;
		bool Field_48319_f;
		float Field_48320_g;
		float Field_48326_h;

		public EntityAIDoorInteract(EntityLiving par1EntityLiving)
		{
			TheEntity = par1EntityLiving;
		}

		/// <summary>
		/// Returns whether the EntityAIBase should begin execution.
		/// </summary>
		public override bool ShouldExecute()
		{
			if (!TheEntity.IsCollidedHorizontally)
			{
				return false;
			}

			PathNavigate pathnavigate = TheEntity.GetNavigator();
			PathEntity pathentity = pathnavigate.GetPath();

			if (pathentity == null || pathentity.IsFinished() || !pathnavigate.Func_48665_b())
			{
				return false;
			}

			for (int i = 0; i < Math.Min(pathentity.GetCurrentPathIndex() + 2, pathentity.GetCurrentPathLength()); i++)
			{
				PathPoint pathpoint = pathentity.GetPathPointFromIndex(i);
				EntityPosX = pathpoint.XCoord;
				EntityPosY = pathpoint.YCoord + 1;
				EntityPosZ = pathpoint.ZCoord;

				if (TheEntity.GetDistanceSq(EntityPosX, TheEntity.PosY, EntityPosZ) > 2.25D)
				{
					continue;
				}

				TargetDoor = Func_48318_a(EntityPosX, EntityPosY, EntityPosZ);

				if (TargetDoor != null)
				{
					return true;
				}
			}

			EntityPosX = MathHelper2.Floor_double(TheEntity.PosX);
			EntityPosY = MathHelper2.Floor_double(TheEntity.PosY + 1.0D);
			EntityPosZ = MathHelper2.Floor_double(TheEntity.PosZ);
			TargetDoor = Func_48318_a(EntityPosX, EntityPosY, EntityPosZ);
			return TargetDoor != null;
		}

		/// <summary>
		/// Returns whether an in-progress EntityAIBase should continue executing
		/// </summary>
		public override bool ContinueExecuting()
		{
			return !Field_48319_f;
		}

		/// <summary>
		/// Execute a one shot task or start executing a continuous task
		/// </summary>
		public override void StartExecuting()
		{
			Field_48319_f = false;
			Field_48320_g = (float)((double)((float)EntityPosX + 0.5F) - TheEntity.PosX);
			Field_48326_h = (float)((double)((float)EntityPosZ + 0.5F) - TheEntity.PosZ);
		}

		/// <summary>
		/// Updates the task
		/// </summary>
		public override void UpdateTask()
		{
			float f = (float)((double)((float)EntityPosX + 0.5F) - TheEntity.PosX);
			float f1 = (float)((double)((float)EntityPosZ + 0.5F) - TheEntity.PosZ);
			float f2 = Field_48320_g * f + Field_48326_h * f1;

			if (f2 < 0.0F)
			{
				Field_48319_f = true;
			}
		}

		private BlockDoor Func_48318_a(int par1, int par2, int par3)
		{
			int i = TheEntity.WorldObj.GetBlockId(par1, par2, par3);

			if (i != Block.DoorWood.BlockID)
			{
				return null;
			}
			else
			{
				BlockDoor blockdoor = (BlockDoor)Block.BlocksList[i];
				return blockdoor;
			}
		}
	}
}