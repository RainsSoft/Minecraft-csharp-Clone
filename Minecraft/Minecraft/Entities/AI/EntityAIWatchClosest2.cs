using System;

namespace net.minecraft.src
{

	public class EntityAIWatchClosest2 : EntityAIWatchClosest
	{
		public EntityAIWatchClosest2(EntityLiving par1EntityLiving, Type par2Class, float par3) : base(par1EntityLiving, par2Class, par3)
		{
			SetMutexBits(3);
		}

		public EntityAIWatchClosest2(EntityLiving par1EntityLiving, Type par2Class, float par3, float par4) : base(par1EntityLiving, par2Class, par3, par4)
		{
			SetMutexBits(3);
		}
	}

}