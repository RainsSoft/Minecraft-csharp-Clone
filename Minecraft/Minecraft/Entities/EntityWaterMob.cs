namespace net.minecraft.src
{
	public abstract class EntityWaterMob : EntityCreature
	{
		public EntityWaterMob(World par1World) : base(par1World)
		{
		}

		public override bool CanBreatheUnderwater()
		{
			return true;
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
		/// Checks if the entity's current position is a valid location to spawn this entity.
		/// </summary>
		public override bool GetCanSpawnHere()
		{
			return WorldObj.CheckIfAABBIsClear(BoundingBox);
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
			return true;
		}

		/// <summary>
		/// Get the experience points the entity currently has.
		/// </summary>
		protected override int GetExperiencePoints(EntityPlayer par1EntityPlayer)
		{
			return 1 + WorldObj.Rand.Next(3);
		}
	}
}