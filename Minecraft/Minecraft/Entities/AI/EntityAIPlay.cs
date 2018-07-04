using System.Collections.Generic;

namespace net.minecraft.src
{
	public class EntityAIPlay : EntityAIBase
	{
		private EntityVillager VillagerObj;
		private EntityLiving TargetVillager;
		private float Field_48358_c;
		private int Field_48356_d;

		public EntityAIPlay(EntityVillager par1EntityVillager, float par2)
		{
			VillagerObj = par1EntityVillager;
			Field_48358_c = par2;
			SetMutexBits(1);
		}

		/// <summary>
		/// Returns whether the EntityAIBase should begin execution.
		/// </summary>
		public override bool ShouldExecute()
		{
			if (VillagerObj.GetGrowingAge() >= 0)
			{
				return false;
			}

			if (VillagerObj.GetRNG().Next(400) != 0)
			{
				return false;
			}

			List<Entity> list = VillagerObj.WorldObj.GetEntitiesWithinAABB(typeof(net.minecraft.src.EntityVillager), VillagerObj.BoundingBox.Expand(6, 3, 6));
			double d = double.MaxValue;
			IEnumerator<Entity> iterator = list.GetEnumerator();

			do
			{
				if (!iterator.MoveNext())
				{
					break;
				}

				Entity entity = iterator.Current;

				if (entity != VillagerObj)
				{
					EntityVillager entityvillager = (EntityVillager)entity;

					if (!entityvillager.GetIsPlayingFlag() && entityvillager.GetGrowingAge() < 0)
					{
						double d1 = entityvillager.GetDistanceSqToEntity(VillagerObj);

						if (d1 <= d)
						{
							d = d1;
							TargetVillager = entityvillager;
						}
					}
				}
			}
			while (true);

			if (TargetVillager == null)
			{
				Vec3D vec3d = RandomPositionGenerator.Func_48622_a(VillagerObj, 16, 3);

				if (vec3d == null)
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Returns whether an in-progress EntityAIBase should continue executing
		/// </summary>
		public override bool ContinueExecuting()
		{
			return Field_48356_d > 0;
		}

		/// <summary>
		/// Execute a one shot task or start executing a continuous task
		/// </summary>
		public override void StartExecuting()
		{
			if (TargetVillager != null)
			{
				VillagerObj.SetIsPlayingFlag(true);
			}

			Field_48356_d = 1000;
		}

		/// <summary>
		/// Resets the task
		/// </summary>
		public override void ResetTask()
		{
			VillagerObj.SetIsPlayingFlag(false);
			TargetVillager = null;
		}

		/// <summary>
		/// Updates the task
		/// </summary>
		public override void UpdateTask()
		{
			Field_48356_d--;

			if (TargetVillager != null)
			{
				if (VillagerObj.GetDistanceSqToEntity(TargetVillager) > 4D)
				{
					VillagerObj.GetNavigator().Func_48667_a(TargetVillager, Field_48358_c);
				}
			}
			else if (VillagerObj.GetNavigator().NoPath())
			{
				Vec3D vec3d = RandomPositionGenerator.Func_48622_a(VillagerObj, 16, 3);

				if (vec3d == null)
				{
					return;
				}

				VillagerObj.GetNavigator().Func_48666_a(vec3d.XCoord, vec3d.YCoord, vec3d.ZCoord, Field_48358_c);
			}
		}
	}
}