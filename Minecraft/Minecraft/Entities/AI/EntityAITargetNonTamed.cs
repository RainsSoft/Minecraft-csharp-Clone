using System;

namespace net.minecraft.src
{

	public class EntityAITargetNonTamed : EntityAINearestAttackableTarget
	{
		private EntityTameable Field_48390_g;

		public EntityAITargetNonTamed(EntityTameable par1EntityTameable, Type par2Class, float par3, int par4, bool par5) : base(par1EntityTameable, par2Class, par3, par4, par5)
		{
			Field_48390_g = par1EntityTameable;
		}

		/// <summary>
		/// Returns whether the EntityAIBase should begin execution.
		/// </summary>
		public override bool ShouldExecute()
		{
			if (Field_48390_g.IsTamed())
			{
				return false;
			}
			else
			{
				return base.ShouldExecute();
			}
		}
	}

}