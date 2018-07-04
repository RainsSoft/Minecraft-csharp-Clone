using System.Collections.Generic;

namespace net.minecraft.src
{
	public class EntityAIFollowParent : EntityAIBase
	{
		/// <summary>
		/// The child that is following its parent. </summary>
		EntityAnimal ChildAnimal;
		EntityAnimal ParentAnimal;
		float Field_48248_c;
		private int Field_48246_d;

		public EntityAIFollowParent(EntityAnimal par1EntityAnimal, float par2)
		{
			ChildAnimal = par1EntityAnimal;
			Field_48248_c = par2;
		}

		/// <summary>
		/// Returns whether the EntityAIBase should begin execution.
		/// </summary>
		public override bool ShouldExecute()
		{
			if (ChildAnimal.GetGrowingAge() >= 0)
			{
				return false;
			}

			List<Entity> list = ChildAnimal.WorldObj.GetEntitiesWithinAABB(ChildAnimal.GetType(), ChildAnimal.BoundingBox.Expand(8, 4, 8));
			EntityAnimal entityanimal = null;
			double d = double.MaxValue;
			IEnumerator<Entity> iterator = list.GetEnumerator();

			do
			{
				if (!iterator.MoveNext())
				{
					break;
				}

				Entity entity = iterator.Current;
				EntityAnimal entityanimal1 = (EntityAnimal)entity;

				if (entityanimal1.GetGrowingAge() >= 0)
				{
					double d1 = ChildAnimal.GetDistanceSqToEntity(entityanimal1);

					if (d1 <= d)
					{
						d = d1;
						entityanimal = entityanimal1;
					}
				}
			}
			while (true);

			if (entityanimal == null)
			{
				return false;
			}

			if (d < 9D)
			{
				return false;
			}
			else
			{
				ParentAnimal = entityanimal;
				return true;
			}
		}

		/// <summary>
		/// Returns whether an in-progress EntityAIBase should continue executing
		/// </summary>
		public override bool ContinueExecuting()
		{
			if (!ParentAnimal.IsEntityAlive())
			{
				return false;
			}

			double d = ChildAnimal.GetDistanceSqToEntity(ParentAnimal);
			return d >= 9D && d <= 256D;
		}

		/// <summary>
		/// Execute a one shot task or start executing a continuous task
		/// </summary>
		public override void StartExecuting()
		{
			Field_48246_d = 0;
		}

		/// <summary>
		/// Resets the task
		/// </summary>
		public override void ResetTask()
		{
			ParentAnimal = null;
		}

		/// <summary>
		/// Updates the task
		/// </summary>
		public override void UpdateTask()
		{
			if (--Field_48246_d > 0)
			{
				return;
			}
			else
			{
				Field_48246_d = 10;
				ChildAnimal.GetNavigator().Func_48667_a(ParentAnimal, Field_48248_c);
				return;
			}
		}
	}
}