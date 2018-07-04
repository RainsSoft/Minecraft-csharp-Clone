namespace net.minecraft.src
{

	public class EntityDragonBase : EntityLiving
	{
		/// <summary>
		/// The maximum health of the Entity. </summary>
		protected int MaxHealth;

		public EntityDragonBase(World par1World) : base(par1World)
		{
			MaxHealth = 100;
		}

		public override int GetMaxHealth()
		{
			return MaxHealth;
		}

		public virtual bool AttackEntityFromPart(EntityDragonPart par1EntityDragonPart, DamageSource par2DamageSource, int par3)
		{
			return AttackEntityFrom(par2DamageSource, par3);
		}

		/// <summary>
		/// Called when the entity is attacked.
		/// </summary>
		public override bool AttackEntityFrom(DamageSource par1DamageSource, int par2)
		{
			return false;
		}

		/// <summary>
		/// Returns a super of attackEntityFrom in EntityDragonBase, because the normal attackEntityFrom is overriden
		/// </summary>
		protected virtual bool SuperAttackFrom(DamageSource par1DamageSource, int par2)
		{
			return base.AttackEntityFrom(par1DamageSource, par2);
		}
	}

}