using System;

namespace net.minecraft.src
{
	public class EntityAIOcelotAttack : EntityAIBase
	{
		World TheWorld;
		EntityLiving TheEntity;
		EntityLiving Field_48362_c;
		int Field_48360_d;

		public EntityAIOcelotAttack(EntityLiving par1EntityLiving)
		{
			Field_48360_d = 0;
			TheEntity = par1EntityLiving;
			TheWorld = par1EntityLiving.WorldObj;
			SetMutexBits(3);
		}

		/// <summary>
		/// Returns whether the EntityAIBase should begin execution.
		/// </summary>
		public override bool ShouldExecute()
		{
			EntityLiving entityliving = TheEntity.GetAttackTarget();

			if (entityliving == null)
			{
				return false;
			}
			else
			{
				Field_48362_c = entityliving;
				return true;
			}
		}

		/// <summary>
		/// Returns whether an in-progress EntityAIBase should continue executing
		/// </summary>
		public override bool ContinueExecuting()
		{
			if (!Field_48362_c.IsEntityAlive())
			{
				return false;
			}

			if (TheEntity.GetDistanceSqToEntity(Field_48362_c) > 225D)
			{
				return false;
			}
			else
			{
				return !TheEntity.GetNavigator().NoPath() || ShouldExecute();
			}
		}

		/// <summary>
		/// Resets the task
		/// </summary>
		public override void ResetTask()
		{
			Field_48362_c = null;
			TheEntity.GetNavigator().ClearPathEntity();
		}

		/// <summary>
		/// Updates the task
		/// </summary>
		public override void UpdateTask()
		{
			TheEntity.GetLookHelper().SetLookPositionWithEntity(Field_48362_c, 30F, 30F);
			double d = TheEntity.Width * 2.0F * (TheEntity.Width * 2.0F);
			double d1 = TheEntity.GetDistanceSq(Field_48362_c.PosX, Field_48362_c.BoundingBox.MinY, Field_48362_c.PosZ);
			float f = 0.23F;

			if (d1 > d && d1 < 16D)
			{
				f = 0.4F;
			}
			else if (d1 < 225D)
			{
				f = 0.18F;
			}

			TheEntity.GetNavigator().Func_48667_a(Field_48362_c, f);
			Field_48360_d = Math.Max(Field_48360_d - 1, 0);

			if (d1 > d)
			{
				return;
			}

			if (Field_48360_d > 0)
			{
				return;
			}
			else
			{
				Field_48360_d = 20;
				TheEntity.AttackEntityAsMob(Field_48362_c);
				return;
			}
		}
	}
}