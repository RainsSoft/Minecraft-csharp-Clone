using System.Collections.Generic;

namespace net.minecraft.src
{
	public class EntityAIHurtByTarget : EntityAITarget
	{
		bool Field_48395_a;

		public EntityAIHurtByTarget(EntityLiving par1EntityLiving, bool par2) : base(par1EntityLiving, 16F, false)
		{
			Field_48395_a = par2;
			SetMutexBits(1);
		}

		/// <summary>
		/// Returns whether the EntityAIBase should begin execution.
		/// </summary>
		public override bool ShouldExecute()
		{
			return Func_48376_a(TaskOwner.GetAITarget(), true);
		}

		/// <summary>
		/// Execute a one shot task or start executing a continuous task
		/// </summary>
		public override void StartExecuting()
		{
			TaskOwner.SetAttackTarget(TaskOwner.GetAITarget());

			if (Field_48395_a)
			{
				List<Entity> list = TaskOwner.WorldObj.GetEntitiesWithinAABB(TaskOwner.GetType(), AxisAlignedBB.GetBoundingBoxFromPool(TaskOwner.PosX, TaskOwner.PosY, TaskOwner.PosZ, TaskOwner.PosX + 1, TaskOwner.PosY + 1, TaskOwner.PosZ + 1).Expand(Field_48379_d, 4, Field_48379_d));
				IEnumerator<Entity> iterator = list.GetEnumerator();

				do
				{
					if (!iterator.MoveNext())
					{
						break;
					}

					Entity entity = iterator.Current;
					EntityLiving entityliving = (EntityLiving)entity;

					if (TaskOwner != entityliving && entityliving.GetAttackTarget() == null)
					{
						entityliving.SetAttackTarget(TaskOwner.GetAITarget());
					}
				}
				while (true);
			}

			base.StartExecuting();
		}
	}
}