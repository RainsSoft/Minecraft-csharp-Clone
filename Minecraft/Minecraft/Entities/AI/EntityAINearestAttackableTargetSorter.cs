using System.Collections.Generic;

namespace net.minecraft.src
{
    public class EntityAINearestAttackableTargetSorter : IComparer<Entity>
	{
		private Entity TheEntity;
		readonly EntityAINearestAttackableTarget Parent;

		public EntityAINearestAttackableTargetSorter(EntityAINearestAttackableTarget par1EntityAINearestAttackableTarget, Entity par2Entity)
		{
			Parent = par1EntityAINearestAttackableTarget;
			TheEntity = par2Entity;
		}

		public virtual int Func_48469_a(Entity par1Entity, Entity par2Entity)
		{
			double d = TheEntity.GetDistanceSqToEntity(par1Entity);
			double d1 = TheEntity.GetDistanceSqToEntity(par2Entity);

			if (d < d1)
			{
				return -1;
			}

			return d <= d1 ? 0 : 1;
		}

        public virtual int Compare(Entity par1Obj, Entity par2Obj)
		{
			return Func_48469_a(par1Obj, par2Obj);
		}
	}
}