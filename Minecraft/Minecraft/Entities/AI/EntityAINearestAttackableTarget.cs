using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class EntityAINearestAttackableTarget : EntityAITarget
	{
		EntityLiving TargetEntity;
		Type TargetClass;
		int Field_48386_f;
		private EntityAINearestAttackableTargetSorter Field_48387_g;

		public EntityAINearestAttackableTarget(EntityLiving par1EntityLiving, Type par2Class, float par3, int par4, bool par5) : this(par1EntityLiving, par2Class, par3, par4, par5, false)
		{
		}

		public EntityAINearestAttackableTarget(EntityLiving par1EntityLiving, Type par2Class, float par3, int par4, bool par5, bool par6) : base(par1EntityLiving, par3, par5, par6)
		{
			TargetClass = par2Class;
			Field_48379_d = par3;
			Field_48386_f = par4;
			Field_48387_g = new EntityAINearestAttackableTargetSorter(this, par1EntityLiving);
			SetMutexBits(1);
		}

		/// <summary>
		/// Returns whether the EntityAIBase should begin execution.
		/// </summary>
		public override bool ShouldExecute()
		{
			label0:
			{
				if (Field_48386_f > 0 && TaskOwner.GetRNG().Next(Field_48386_f) != 0)
				{
					return false;
				}

				if (TargetClass == (typeof(net.minecraft.src.EntityPlayer)))
				{
					EntityPlayer entityplayer = TaskOwner.WorldObj.GetClosestVulnerablePlayerToEntity(TaskOwner, Field_48379_d);

					if (Func_48376_a(entityplayer, false))
					{
						TargetEntity = entityplayer;
						return true;
					}

					goto label0;
				}

				List<Entity> list = TaskOwner.WorldObj.GetEntitiesWithinAABB(TargetClass, TaskOwner.BoundingBox.Expand(Field_48379_d, 4, Field_48379_d));
				list.Sort(Field_48387_g);
				IEnumerator<Entity> iterator = list.GetEnumerator();
				EntityLiving entityliving;

				do
				{
					if (!iterator.MoveNext())
					{
						goto label0;
					}

					Entity entity = (Entity)iterator.Current;
					entityliving = (EntityLiving)entity;
				}
				while (!Func_48376_a(entityliving, false));

				TargetEntity = entityliving;
				return true;
			}
			return false;
		}

		/// <summary>
		/// Execute a one shot task or start executing a continuous task
		/// </summary>
		public override void StartExecuting()
		{
			TaskOwner.SetAttackTarget(TargetEntity);
			base.StartExecuting();
		}
	}
}