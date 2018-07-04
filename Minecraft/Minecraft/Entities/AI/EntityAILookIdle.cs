using System;

namespace net.minecraft.src
{
	public class EntityAILookIdle : EntityAIBase
	{
		/// <summary>
		/// The entity that is looking idle. </summary>
		private EntityLiving IdleEntity;

		/// <summary>
		/// X offset to look at </summary>
		private double LookX;

		/// <summary>
		/// Z offset to look at </summary>
		private double LookZ;

		/// <summary>
		/// A decrementing tick that stops the entity from being idle once it reaches 0.
		/// </summary>
		private int IdleTime;

		public EntityAILookIdle(EntityLiving par1EntityLiving)
		{
			IdleTime = 0;
			IdleEntity = par1EntityLiving;
			SetMutexBits(3);
		}

		/// <summary>
		/// Returns whether the EntityAIBase should begin execution.
		/// </summary>
		public override bool ShouldExecute()
		{
			return IdleEntity.GetRNG().NextFloat() < 0.02F;
		}

		/// <summary>
		/// Returns whether an in-progress EntityAIBase should continue executing
		/// </summary>
		public override bool ContinueExecuting()
		{
			return IdleTime >= 0;
		}

		/// <summary>
		/// Execute a one shot task or start executing a continuous task
		/// </summary>
		public override void StartExecuting()
		{
			double d = (Math.PI * 2D) * IdleEntity.GetRNG().NextDouble();
			LookX = Math.Cos(d);
			LookZ = Math.Sin(d);
			IdleTime = 20 + IdleEntity.GetRNG().Next(20);
		}

		/// <summary>
		/// Updates the task
		/// </summary>
		public override void UpdateTask()
		{
			IdleTime--;
			IdleEntity.GetLookHelper().SetLookPosition(IdleEntity.PosX + LookX, IdleEntity.PosY + (double)IdleEntity.GetEyeHeight(), IdleEntity.PosZ + LookZ, 10F, IdleEntity.GetVerticalFaceSpeed());
		}
	}

}