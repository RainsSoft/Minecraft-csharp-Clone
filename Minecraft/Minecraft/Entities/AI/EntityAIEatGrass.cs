using System;

namespace net.minecraft.src
{
	public class EntityAIEatGrass : EntityAIBase
	{
		private EntityLiving TheEntity;
		private World TheWorld;

		/// <summary>
		/// A decrementing tick used for the sheep's head offset and animation. </summary>
		int EatGrassTick;

		public EntityAIEatGrass(EntityLiving par1EntityLiving)
		{
			EatGrassTick = 0;
			TheEntity = par1EntityLiving;
			TheWorld = par1EntityLiving.WorldObj;
			SetMutexBits(7);
		}

		/// <summary>
		/// Returns whether the EntityAIBase should begin execution.
		/// </summary>
		public override bool ShouldExecute()
		{
			if (TheEntity.GetRNG().Next(TheEntity.IsChild() ? 50 : 1000) != 0)
			{
				return false;
			}

			int i = MathHelper2.Floor_double(TheEntity.PosX);
			int j = MathHelper2.Floor_double(TheEntity.PosY);
			int k = MathHelper2.Floor_double(TheEntity.PosZ);

			if (TheWorld.GetBlockId(i, j, k) == Block.TallGrass.BlockID && TheWorld.GetBlockMetadata(i, j, k) == 1)
			{
				return true;
			}

			return TheWorld.GetBlockId(i, j - 1, k) == Block.Grass.BlockID;
		}

		/// <summary>
		/// Execute a one shot task or start executing a continuous task
		/// </summary>
		public override void StartExecuting()
		{
			EatGrassTick = 40;
			TheWorld.SetEntityState(TheEntity, (sbyte)10);
			TheEntity.GetNavigator().ClearPathEntity();
		}

		/// <summary>
		/// Resets the task
		/// </summary>
		public override void ResetTask()
		{
			EatGrassTick = 0;
		}

		/// <summary>
		/// Returns whether an in-progress EntityAIBase should continue executing
		/// </summary>
		public override bool ContinueExecuting()
		{
			return EatGrassTick > 0;
		}

		public virtual int Func_48396_h()
		{
			return EatGrassTick;
		}

		/// <summary>
		/// Updates the task
		/// </summary>
		public override void UpdateTask()
		{
			EatGrassTick = Math.Max(0, EatGrassTick - 1);

			if (EatGrassTick != 4)
			{
				return;
			}

			int i = MathHelper2.Floor_double(TheEntity.PosX);
			int j = MathHelper2.Floor_double(TheEntity.PosY);
			int k = MathHelper2.Floor_double(TheEntity.PosZ);

			if (TheWorld.GetBlockId(i, j, k) == Block.TallGrass.BlockID)
			{
				TheWorld.PlayAuxSFX(2001, i, j, k, Block.TallGrass.BlockID + 4096);
				TheWorld.SetBlockWithNotify(i, j, k, 0);
				TheEntity.EatGrassBonus();
			}
			else if (TheWorld.GetBlockId(i, j - 1, k) == Block.Grass.BlockID)
			{
				TheWorld.PlayAuxSFX(2001, i, j - 1, k, Block.Grass.BlockID);
				TheWorld.SetBlockWithNotify(i, j - 1, k, Block.Dirt.BlockID);
				TheEntity.EatGrassBonus();
			}
		}
	}
}