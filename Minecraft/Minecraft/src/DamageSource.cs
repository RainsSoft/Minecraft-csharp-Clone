namespace net.minecraft.src
{

	public class DamageSource
	{
		public static DamageSource InFire = (new DamageSource("inFire")).SetFireDamage();
		public static DamageSource OnFire = (new DamageSource("onFire")).SetDamageBypassesArmor().SetFireDamage();
		public static DamageSource Lava = (new DamageSource("lava")).SetFireDamage();
		public static DamageSource InWall = (new DamageSource("inWall")).SetDamageBypassesArmor();
		public static DamageSource Drown = (new DamageSource("drown")).SetDamageBypassesArmor();
		public static DamageSource Starve = (new DamageSource("starve")).SetDamageBypassesArmor();
		public static DamageSource Cactus = new DamageSource("cactus");
		public static DamageSource Fall = (new DamageSource("fall")).SetDamageBypassesArmor();
		public static DamageSource OutOfWorld = (new DamageSource("outOfWorld")).SetDamageBypassesArmor().SetDamageAllowedInCreativeMode();
		public static DamageSource Generic = (new DamageSource("generic")).SetDamageBypassesArmor();
		public static DamageSource Explosion = new DamageSource("explosion");
		public static DamageSource Magic = (new DamageSource("magic")).SetDamageBypassesArmor();

		/// <summary>
		/// This kind of damage can be blocked or not. </summary>
		private bool isUnblockable_Renamed;
		private bool IsDamageAllowedInCreativeMode;
		private float HungerDamage;

		/// <summary>
		/// This kind of damage is based on fire or not. </summary>
		private bool fireDamage_Renamed;

		/// <summary>
		/// This kind of damage is based on a projectile or not. </summary>
		private bool Projectile;
		public string DamageType;

		public static DamageSource CauseMobDamage(EntityLiving par0EntityLiving)
		{
			return new EntityDamageSource("mob", par0EntityLiving);
		}

		/// <summary>
		/// returns an EntityDamageSource of type player
		/// </summary>
		public static DamageSource CausePlayerDamage(EntityPlayer par0EntityPlayer)
		{
			return new EntityDamageSource("player", par0EntityPlayer);
		}

		/// <summary>
		/// returns EntityDamageSourceIndirect of an arrow
		/// </summary>
		public static DamageSource CauseArrowDamage(EntityArrow par0EntityArrow, Entity par1Entity)
		{
			return (new EntityDamageSourceIndirect("arrow", par0EntityArrow, par1Entity)).SetProjectile();
		}

		/// <summary>
		/// returns EntityDamageSourceIndirect of a fireball
		/// </summary>
		public static DamageSource CauseFireballDamage(EntityFireball par0EntityFireball, Entity par1Entity)
		{
			return (new EntityDamageSourceIndirect("fireball", par0EntityFireball, par1Entity)).SetFireDamage().SetProjectile();
		}

		public static DamageSource CauseThrownDamage(Entity par0Entity, Entity par1Entity)
		{
			return (new EntityDamageSourceIndirect("thrown", par0Entity, par1Entity)).SetProjectile();
		}

		public static DamageSource CauseIndirectMagicDamage(Entity par0Entity, Entity par1Entity)
		{
			return (new EntityDamageSourceIndirect("indirectMagic", par0Entity, par1Entity)).SetDamageBypassesArmor();
		}

		/// <summary>
		/// Returns true if the damage is projectile based.
		/// </summary>
		public virtual bool IsProjectile()
		{
			return Projectile;
		}

		/// <summary>
		/// Define the damage type as projectile based.
		/// </summary>
		public virtual DamageSource SetProjectile()
		{
			Projectile = true;
			return this;
		}

		public virtual bool IsUnblockable()
		{
			return isUnblockable_Renamed;
		}

		/// <summary>
		/// How much satiate(food) is consumed by this DamageSource
		/// </summary>
		public virtual float GetHungerDamage()
		{
			return HungerDamage;
		}

		public virtual bool CanHarmInCreative()
		{
			return IsDamageAllowedInCreativeMode;
		}

		protected DamageSource(string par1Str)
		{
			isUnblockable_Renamed = false;
			IsDamageAllowedInCreativeMode = false;
			HungerDamage = 0.3F;
			DamageType = par1Str;
		}

		public virtual Entity GetSourceOfDamage()
		{
			return GetEntity();
		}

		public virtual Entity GetEntity()
		{
			return null;
		}

		protected virtual DamageSource SetDamageBypassesArmor()
		{
			isUnblockable_Renamed = true;
			HungerDamage = 0.0F;
			return this;
		}

		protected virtual DamageSource SetDamageAllowedInCreativeMode()
		{
			IsDamageAllowedInCreativeMode = true;
			return this;
		}

		/// <summary>
		/// Define the damage type as fire based.
		/// </summary>
		protected virtual DamageSource SetFireDamage()
		{
			fireDamage_Renamed = true;
			return this;
		}

		/// <summary>
		/// Returns true if the damage is fire based.
		/// </summary>
		public virtual bool FireDamage()
		{
			return fireDamage_Renamed;
		}

		/// <summary>
		/// Return the name of damage type.
		/// </summary>
		public virtual string GetDamageType()
		{
			return DamageType;
		}
	}

}