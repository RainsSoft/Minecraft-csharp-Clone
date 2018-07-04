using System.Collections.Generic;

namespace net.minecraft.src
{
	public class EntitySenses
	{
		EntityLiving EntityObj;
		List<Entity> CanSeeCachePositive;
        List<Entity> CanSeeCacheNegative;

		public EntitySenses(EntityLiving par1EntityLiving)
		{
            CanSeeCachePositive = new List<Entity>();
            CanSeeCacheNegative = new List<Entity>();
			EntityObj = par1EntityLiving;
		}

		/// <summary>
		/// Clears CanSeeCachePositive and CanSeeCacheNegative.
		/// </summary>
		public virtual void ClearSensingCache()
		{
			CanSeeCachePositive.Clear();
			CanSeeCacheNegative.Clear();
		}

		/// <summary>
		/// Checks, whether 'our' entity can see the entity given as argument (true) or not (false), caching the result.
		/// </summary>
		public virtual bool CanSee(Entity par1Entity)
		{
			if (CanSeeCachePositive.Contains(par1Entity))
			{
				return true;
			}

			if (CanSeeCacheNegative.Contains(par1Entity))
			{
				return false;
			}

			Profiler.StartSection("CanSee");
			bool flag = EntityObj.CanEntityBeSeen(par1Entity);
			Profiler.EndSection();

			if (flag)
			{
				CanSeeCachePositive.Add(par1Entity);
			}
			else
			{
				CanSeeCacheNegative.Add(par1Entity);
			}

			return flag;
		}
	}
}