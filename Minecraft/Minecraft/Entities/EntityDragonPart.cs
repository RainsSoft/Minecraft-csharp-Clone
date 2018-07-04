namespace net.minecraft.src
{

	public class EntityDragonPart : Entity
	{
		/// <summary>
		/// The dragon entity this dragon part belongs to </summary>
		public readonly EntityDragonBase EntityDragonObj;

		/// <summary>
		/// The name of the Dragon Part </summary>
		public readonly string Name;

		public EntityDragonPart(EntityDragonBase par1EntityDragonBase, string par2Str, float par3, float par4) : base(par1EntityDragonBase.WorldObj)
		{
			SetSize(par3, par4);
			EntityDragonObj = par1EntityDragonBase;
			Name = par2Str;
		}

		protected override void EntityInit()
		{
		}

		/// <summary>
		/// (abstract) Protected helper method to read subclass entity data from NBT.
		/// </summary>
        public override void ReadEntityFromNBT(NBTTagCompound nbttagcompound)
		{
		}

		/// <summary>
		/// (abstract) Protected helper method to write subclass entity data to NBT.
		/// </summary>
        public override void WriteEntityToNBT(NBTTagCompound nbttagcompound)
		{
		}

		/// <summary>
		/// Returns true if other Entities should be prevented from moving through this Entity.
		/// </summary>
		public override bool CanBeCollidedWith()
		{
			return true;
		}

		/// <summary>
		/// Called when the entity is attacked.
		/// </summary>
		public override bool AttackEntityFrom(DamageSource par1DamageSource, int par2)
		{
			return EntityDragonObj.AttackEntityFromPart(this, par1DamageSource, par2);
		}

		/// <summary>
		/// Returns true if Entity argument is equal to this Entity
		/// </summary>
		public override bool IsEntityEqual(Entity par1Entity)
		{
			return this == par1Entity || EntityDragonObj == par1Entity;
		}
	}

}