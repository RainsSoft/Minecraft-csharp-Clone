namespace net.minecraft.src
{

	public class PlayerCapabilities
	{
		/// <summary>
		/// Disables player damage. </summary>
		public bool DisableDamage;

		/// <summary>
		/// Sets/indicates whether the player is flying. </summary>
		public bool IsFlying;

		/// <summary>
		/// whether or not to allow the player to fly when they double jump. </summary>
		public bool AllowFlying;

		/// <summary>
		/// Used to determine if creative mode is enabled, and therefore if items should be depleted on usage
		/// </summary>
		public bool IsCreativeMode;

		public PlayerCapabilities()
		{
			DisableDamage = false;
			IsFlying = false;
			AllowFlying = false;
			IsCreativeMode = false;
		}

		public virtual void WriteCapabilitiesToNBT(NBTTagCompound par1NBTTagCompound)
		{
			NBTTagCompound nbttagcompound = new NBTTagCompound();
			nbttagcompound.Setbool("invulnerable", DisableDamage);
			nbttagcompound.Setbool("flying", IsFlying);
			nbttagcompound.Setbool("mayfly", AllowFlying);
			nbttagcompound.Setbool("instabuild", IsCreativeMode);
			par1NBTTagCompound.SetTag("abilities", nbttagcompound);
		}

		public virtual void ReadCapabilitiesFromNBT(NBTTagCompound par1NBTTagCompound)
		{
			if (par1NBTTagCompound.HasKey("abilities"))
			{
				NBTTagCompound nbttagcompound = par1NBTTagCompound.GetCompoundTag("abilities");
				DisableDamage = nbttagcompound.Getbool("invulnerable");
				IsFlying = nbttagcompound.Getbool("flying");
				AllowFlying = nbttagcompound.Getbool("mayfly");
				IsCreativeMode = nbttagcompound.Getbool("instabuild");
			}
		}
	}

}