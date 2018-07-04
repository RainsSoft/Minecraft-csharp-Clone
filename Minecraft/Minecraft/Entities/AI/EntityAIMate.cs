using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class EntityAIMate : EntityAIBase
	{
		private EntityAnimal TheAnimal;
		World TheWorld;
		private EntityAnimal TargetMate;
		int Field_48261_b;
		float Field_48262_c;

		public EntityAIMate(EntityAnimal par1EntityAnimal, float par2)
		{
			Field_48261_b = 0;
			TheAnimal = par1EntityAnimal;
			TheWorld = par1EntityAnimal.WorldObj;
			Field_48262_c = par2;
			SetMutexBits(3);
		}

		/// <summary>
		/// Returns whether the EntityAIBase should begin execution.
		/// </summary>
		public override bool ShouldExecute()
		{
			if (!TheAnimal.IsInLove())
			{
				return false;
			}
			else
			{
				TargetMate = Func_48258_h();
				return TargetMate != null;
			}
		}

		/// <summary>
		/// Returns whether an in-progress EntityAIBase should continue executing
		/// </summary>
		public override bool ContinueExecuting()
		{
			return TargetMate.IsEntityAlive() && TargetMate.IsInLove() && Field_48261_b < 60;
		}

		/// <summary>
		/// Resets the task
		/// </summary>
		public override void ResetTask()
		{
			TargetMate = null;
			Field_48261_b = 0;
		}

		/// <summary>
		/// Updates the task
		/// </summary>
		public override void UpdateTask()
		{
			TheAnimal.GetLookHelper().SetLookPositionWithEntity(TargetMate, 10F, TheAnimal.GetVerticalFaceSpeed());
			TheAnimal.GetNavigator().Func_48667_a(TargetMate, Field_48262_c);
			Field_48261_b++;

			if (Field_48261_b == 60)
			{
				Func_48257_i();
			}
		}

		private EntityAnimal Func_48258_h()
		{
			float f = 8F;
			List<Entity> list = TheWorld.GetEntitiesWithinAABB(TheAnimal.GetType(), TheAnimal.BoundingBox.Expand(f, f, f));

			for (IEnumerator<Entity> iterator = list.GetEnumerator(); iterator.MoveNext();)
			{
				Entity entity = iterator.Current;
				EntityAnimal entityanimal = (EntityAnimal)entity;

				if (TheAnimal.Func_48135_b(entityanimal))
				{
					return entityanimal;
				}
			}

			return null;
		}

		private void Func_48257_i()
		{
			EntityAnimal entityanimal = TheAnimal.SpawnBabyAnimal(TargetMate);

			if (entityanimal == null)
			{
				return;
			}

			TheAnimal.SetGrowingAge(6000);
			TargetMate.SetGrowingAge(6000);
			TheAnimal.ResetInLove();
			TargetMate.ResetInLove();
			entityanimal.SetGrowingAge(-24000);
			entityanimal.SetLocationAndAngles(TheAnimal.PosX, TheAnimal.PosY, TheAnimal.PosZ, 0.0F, 0.0F);
			TheWorld.SpawnEntityInWorld(entityanimal);
			Random random = TheAnimal.GetRNG();

			for (int i = 0; i < 7; i++)
			{
				double d = random.NextGaussian() * 0.02D;
				double d1 = random.NextGaussian() * 0.02D;
				double d2 = random.NextGaussian() * 0.02D;
				TheWorld.SpawnParticle("heart", (TheAnimal.PosX + (double)(random.NextFloat() * TheAnimal.Width * 2.0F)) - (double)TheAnimal.Width, TheAnimal.PosY + 0.5D + (double)(random.NextFloat() * TheAnimal.Height), (TheAnimal.PosZ + (double)(random.NextFloat() * TheAnimal.Width * 2.0F)) - (double)TheAnimal.Width, d, d1, d2);
			}
		}
	}
}