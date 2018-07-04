using System;

namespace net.minecraft.src
{
	public class EntityAIArrowAttack : EntityAIBase
	{
		World WorldObj;

		/// <summary>
		/// The entity the AI instance has been applied to </summary>
		EntityLiving EntityHost;
		EntityLiving AttackTarget;

		/// <summary>
		/// A decrementing tick that spawns a ranged attack once this value reaches 0. It is then set back to the
		/// maxRangedAttackTime.
		/// </summary>
		int RangedAttackTime;
		float Field_48370_e;
		int Field_48367_f;

		/// <summary>
		/// The ID of this ranged attack AI. This chooses which entity is to be used as a ranged attack.
		/// </summary>
		int RangedAttackID;

		/// <summary>
		/// The maximum time the AI has to wait before peforming another ranged attack.
		/// </summary>
		int MaxRangedAttackTime;

		public EntityAIArrowAttack(EntityLiving par1EntityLiving, float par2, int par3, int par4)
		{
			RangedAttackTime = 0;
			Field_48367_f = 0;
			EntityHost = par1EntityLiving;
			WorldObj = par1EntityLiving.WorldObj;
			Field_48370_e = par2;
			RangedAttackID = par3;
			MaxRangedAttackTime = par4;
			SetMutexBits(3);
		}

		/// <summary>
		/// Returns whether the EntityAIBase should begin execution.
		/// </summary>
		public override bool ShouldExecute()
		{
			EntityLiving entityliving = EntityHost.GetAttackTarget();

			if (entityliving == null)
			{
				return false;
			}
			else
			{
				AttackTarget = entityliving;
				return true;
			}
		}

		/// <summary>
		/// Returns whether an in-progress EntityAIBase should continue executing
		/// </summary>
		public override bool ContinueExecuting()
		{
			return ShouldExecute() || !EntityHost.GetNavigator().NoPath();
		}

		/// <summary>
		/// Resets the task
		/// </summary>
		public override void ResetTask()
		{
			AttackTarget = null;
		}

		/// <summary>
		/// Updates the task
		/// </summary>
		public override void UpdateTask()
		{
			double d = 100D;
			double d1 = EntityHost.GetDistanceSq(AttackTarget.PosX, AttackTarget.BoundingBox.MinY, AttackTarget.PosZ);
			bool flag = EntityHost.Func_48090_aM().CanSee(AttackTarget);

			if (flag)
			{
				Field_48367_f++;
			}
			else
			{
				Field_48367_f = 0;
			}

			if (d1 > d || Field_48367_f < 20)
			{
				EntityHost.GetNavigator().Func_48667_a(AttackTarget, Field_48370_e);
			}
			else
			{
				EntityHost.GetNavigator().ClearPathEntity();
			}

			EntityHost.GetLookHelper().SetLookPositionWithEntity(AttackTarget, 30F, 30F);
			RangedAttackTime = Math.Max(RangedAttackTime - 1, 0);

			if (RangedAttackTime > 0)
			{
				return;
			}

			if (d1 > d || !flag)
			{
				return;
			}
			else
			{
				DoRangedAttack();
				RangedAttackTime = MaxRangedAttackTime;
				return;
			}
		}

		/// <summary>
		/// Performs a ranged attack according to the AI's rangedAttackID.
		/// </summary>
		private void DoRangedAttack()
		{
			if (RangedAttackID == 1)
			{
				EntityArrow entityarrow = new EntityArrow(WorldObj, EntityHost, AttackTarget, 1.6F, 12F);
				WorldObj.PlaySoundAtEntity(EntityHost, "random.bow", 1.0F, 1.0F / (EntityHost.GetRNG().NextFloat() * 0.4F + 0.8F));
				WorldObj.SpawnEntityInWorld(entityarrow);
			}
			else if (RangedAttackID == 2)
			{
				EntitySnowball entitysnowball = new EntitySnowball(WorldObj, EntityHost);
                float d = AttackTarget.PosX - EntityHost.PosX;
                float d1 = (AttackTarget.PosY + AttackTarget.GetEyeHeight()) - 1.1000000238418579F - entitysnowball.PosY;
                float d2 = AttackTarget.PosZ - EntityHost.PosZ;
				float f = MathHelper2.Sqrt_double(d * d + d2 * d2) * 0.2F;
				entitysnowball.SetThrowableHeading(d, d1 + f, d2, 1.6F, 12F);
				WorldObj.PlaySoundAtEntity(EntityHost, "random.bow", 1.0F, 1.0F / (EntityHost.GetRNG().NextFloat() * 0.4F + 0.8F));
				WorldObj.SpawnEntityInWorld(entitysnowball);
			}
		}
	}
}