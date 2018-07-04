using System.Collections.Generic;

namespace net.minecraft.src
{
	public class EntityAIFollowGolem : EntityAIBase
	{
		private EntityVillager TheVillager;
		private EntityIronGolem TheGolem;
		private int Field_48402_c;
		private bool Field_48400_d;

		public EntityAIFollowGolem(EntityVillager par1EntityVillager)
		{
			Field_48400_d = false;
			TheVillager = par1EntityVillager;
			SetMutexBits(3);
		}

		/// <summary>
		/// Returns whether the EntityAIBase should begin execution.
		/// </summary>
		public override bool ShouldExecute()
		{
			if (TheVillager.GetGrowingAge() >= 0)
			{
				return false;
			}

			if (!TheVillager.WorldObj.IsDaytime())
			{
				return false;
			}

			List<Entity> list = TheVillager.WorldObj.GetEntitiesWithinAABB(typeof(net.minecraft.src.EntityIronGolem), TheVillager.BoundingBox.Expand(6, 2, 6));

			if (list.Count == 0)
			{
				return false;
			}

			IEnumerator<Entity> iterator = list.GetEnumerator();

			do
			{
				if (!iterator.MoveNext())
				{
					break;
				}

				Entity entity = iterator.Current;
				EntityIronGolem entityirongolem = (EntityIronGolem)entity;

				if (entityirongolem.Func_48117_D_() <= 0)
				{
					continue;
				}

				TheGolem = entityirongolem;
				break;
			}
			while (true);

			return TheGolem != null;
		}

		/// <summary>
		/// Returns whether an in-progress EntityAIBase should continue executing
		/// </summary>
		public override bool ContinueExecuting()
		{
			return TheGolem.Func_48117_D_() > 0;
		}

		/// <summary>
		/// Execute a one shot task or start executing a continuous task
		/// </summary>
		public override void StartExecuting()
		{
			Field_48402_c = TheVillager.GetRNG().Next(320);
			Field_48400_d = false;
			TheGolem.GetNavigator().ClearPathEntity();
		}

		/// <summary>
		/// Resets the task
		/// </summary>
		public override void ResetTask()
		{
			TheGolem = null;
			TheVillager.GetNavigator().ClearPathEntity();
		}

		/// <summary>
		/// Updates the task
		/// </summary>
		public override void UpdateTask()
		{
			TheVillager.GetLookHelper().SetLookPositionWithEntity(TheGolem, 30F, 30F);

			if (TheGolem.Func_48117_D_() == Field_48402_c)
			{
				TheVillager.GetNavigator().Func_48667_a(TheGolem, 0.15F);
				Field_48400_d = true;
			}

			if (Field_48400_d && TheVillager.GetDistanceSqToEntity(TheGolem) < 4D)
			{
				TheGolem.Func_48116_a(false);
				TheVillager.GetNavigator().ClearPathEntity();
			}
		}
	}
}