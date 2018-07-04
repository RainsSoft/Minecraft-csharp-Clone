using System;

namespace net.minecraft.src
{
	public class EntityAIWatchClosest : EntityAIBase
	{
		private EntityLiving Field_46105_a;

		/// <summary>
		/// The closest entity which is being watched by this one. </summary>
		private Entity ClosestEntity;
		private float Field_46101_d;
		private int Field_46102_e;
		private float Field_48294_e;
		private Type Field_48293_f;

		public EntityAIWatchClosest(EntityLiving par1EntityLiving, Type par2Class, float par3)
		{
			Field_46105_a = par1EntityLiving;
			Field_48293_f = par2Class;
			Field_46101_d = par3;
			Field_48294_e = 0.02F;
			SetMutexBits(2);
		}

		public EntityAIWatchClosest(EntityLiving par1EntityLiving, Type par2Class, float par3, float par4)
		{
			Field_46105_a = par1EntityLiving;
			Field_48293_f = par2Class;
			Field_46101_d = par3;
			Field_48294_e = par4;
			SetMutexBits(2);
		}

		/// <summary>
		/// Returns whether the EntityAIBase should begin execution.
		/// </summary>
		public override bool ShouldExecute()
		{
			if (Field_46105_a.GetRNG().NextFloat() >= Field_48294_e)
			{
				return false;
			}

			if (Field_48293_f == (typeof(net.minecraft.src.EntityPlayer)))
			{
				ClosestEntity = Field_46105_a.WorldObj.GetClosestPlayerToEntity(Field_46105_a, Field_46101_d);
			}
			else
			{
				ClosestEntity = Field_46105_a.WorldObj.FindNearestEntityWithinAABB(Field_48293_f, Field_46105_a.BoundingBox.Expand(Field_46101_d, 3, Field_46101_d), Field_46105_a);
			}

			return ClosestEntity != null;
		}

		/// <summary>
		/// Returns whether an in-progress EntityAIBase should continue executing
		/// </summary>
		public override bool ContinueExecuting()
		{
			if (!ClosestEntity.IsEntityAlive())
			{
				return false;
			}

			if (Field_46105_a.GetDistanceSqToEntity(ClosestEntity) > (double)(Field_46101_d * Field_46101_d))
			{
				return false;
			}
			else
			{
				return Field_46102_e > 0;
			}
		}

		/// <summary>
		/// Execute a one shot task or start executing a continuous task
		/// </summary>
		public override void StartExecuting()
		{
			Field_46102_e = 40 + Field_46105_a.GetRNG().Next(40);
		}

		/// <summary>
		/// Resets the task
		/// </summary>
		public override void ResetTask()
		{
			ClosestEntity = null;
		}

		/// <summary>
		/// Updates the task
		/// </summary>
		public override void UpdateTask()
		{
			Field_46105_a.GetLookHelper().SetLookPosition(ClosestEntity.PosX, ClosestEntity.PosY + (double)ClosestEntity.GetEyeHeight(), ClosestEntity.PosZ, 10F, Field_46105_a.GetVerticalFaceSpeed());
			Field_46102_e--;
		}
	}
}