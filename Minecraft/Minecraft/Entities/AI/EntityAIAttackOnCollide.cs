using System;

namespace net.minecraft.src
{
	public class EntityAIAttackOnCollide : EntityAIBase
	{
		World WorldObj;
		EntityLiving Attacker;
		EntityLiving EntityTarget;
		int Field_46091_d;
		float Field_48266_e;
		bool Field_48264_f;
		PathEntity Field_48265_g;
		Type ClassTarget;
		private int Field_48269_i;

		public EntityAIAttackOnCollide(EntityLiving par1EntityLiving, Type par2Class, float par3, bool par4) : this(par1EntityLiving, par3, par4)
		{
			ClassTarget = par2Class;
		}

		public EntityAIAttackOnCollide(EntityLiving par1EntityLiving, float par2, bool par3)
		{
			Field_46091_d = 0;
			Attacker = par1EntityLiving;
			WorldObj = par1EntityLiving.WorldObj;
			Field_48266_e = par2;
			Field_48264_f = par3;
			SetMutexBits(3);
		}

		/// <summary>
		/// Returns whether the EntityAIBase should begin execution.
		/// </summary>
		public override bool ShouldExecute()
		{
			EntityLiving entityliving = Attacker.GetAttackTarget();

			if (entityliving == null)
			{
				return false;
			}

			if (ClassTarget != null && !ClassTarget.IsAssignableFrom(entityliving.GetType()))
			{
				return false;
			}
			else
			{
				EntityTarget = entityliving;
				Field_48265_g = Attacker.GetNavigator().Func_48679_a(EntityTarget);
				return Field_48265_g != null;
			}
		}

		/// <summary>
		/// Returns whether an in-progress EntityAIBase should continue executing
		/// </summary>
		public override bool ContinueExecuting()
		{
			EntityLiving entityliving = Attacker.GetAttackTarget();

			if (entityliving == null)
			{
				return false;
			}

			if (!EntityTarget.IsEntityAlive())
			{
				return false;
			}

			if (!Field_48264_f)
			{
				return !Attacker.GetNavigator().NoPath();
			}

			return Attacker.IsWithinHomeDistance(MathHelper2.Floor_double(EntityTarget.PosX), MathHelper2.Floor_double(EntityTarget.PosY), MathHelper2.Floor_double(EntityTarget.PosZ));
		}

		/// <summary>
		/// Execute a one shot task or start executing a continuous task
		/// </summary>
		public override void StartExecuting()
		{
			Attacker.GetNavigator().SetPath(Field_48265_g, Field_48266_e);
			Field_48269_i = 0;
		}

		/// <summary>
		/// Resets the task
		/// </summary>
		public override void ResetTask()
		{
			EntityTarget = null;
			Attacker.GetNavigator().ClearPathEntity();
		}

		/// <summary>
		/// Updates the task
		/// </summary>
		public override void UpdateTask()
		{
			Attacker.GetLookHelper().SetLookPositionWithEntity(EntityTarget, 30F, 30F);

			if ((Field_48264_f || Attacker.Func_48090_aM().CanSee(EntityTarget)) && --Field_48269_i <= 0)
			{
				Field_48269_i = 4 + Attacker.GetRNG().Next(7);
				Attacker.GetNavigator().Func_48667_a(EntityTarget, Field_48266_e);
			}

			Field_46091_d = Math.Max(Field_46091_d - 1, 0);
			double d = Attacker.Width * 2.0F * (Attacker.Width * 2.0F);

			if (Attacker.GetDistanceSq(EntityTarget.PosX, EntityTarget.BoundingBox.MinY, EntityTarget.PosZ) > d)
			{
				return;
			}

			if (Field_46091_d > 0)
			{
				return;
			}
			else
			{
				Field_46091_d = 20;
				Attacker.AttackEntityAsMob(EntityTarget);
				return;
			}
		}
	}
}