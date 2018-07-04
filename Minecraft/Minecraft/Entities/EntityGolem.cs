namespace net.minecraft.src
{

	public abstract class EntityGolem : EntityCreature
	{
		public EntityGolem(World par1World) : base(par1World)
		{
		}

		/// <summary>
		/// Called when the mob is falling. Calculates and applies fall damage.
		/// </summary>
		protected override void Fall(float f)
		{
		}

		/// <summary>
		/// (abstract) Protected helper method to write subclass entity data to NBT.
		/// </summary>
		public override void WriteEntityToNBT(NBTTagCompound par1NBTTagCompound)
		{
			base.WriteEntityToNBT(par1NBTTagCompound);
		}

		/// <summary>
		/// (abstract) Protected helper method to read subclass entity data from NBT.
		/// </summary>
		public override void ReadEntityFromNBT(NBTTagCompound par1NBTTagCompound)
		{
			base.ReadEntityFromNBT(par1NBTTagCompound);
		}

		/// <summary>
		/// Returns the sound this mob makes while it's alive.
		/// </summary>
		protected override string GetLivingSound()
		{
			return "none";
		}

		/// <summary>
		/// Returns the sound this mob makes when it is hurt.
		/// </summary>
		protected override string GetHurtSound()
		{
			return "none";
		}

		/// <summary>
		/// Returns the sound this mob makes on death.
		/// </summary>
		protected override string GetDeathSound()
		{
			return "none";
		}

		/// <summary>
		/// Get number of ticks, at least during which the living entity will be silent.
		/// </summary>
		public override int GetTalkInterval()
		{
			return 120;
		}

		/// <summary>
		/// Determines if an entity can be despawned, used on idle far away entities
		/// </summary>
		protected override bool CanDespawn()
		{
			return false;
		}
	}

}