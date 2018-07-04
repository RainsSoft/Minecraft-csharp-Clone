using System;

namespace net.minecraft.src
{

	public class FoodStats
	{
		/// <summary>
		/// The player's food level. </summary>
		private int FoodLevel;

		/// <summary>
		/// The player's food saturation. </summary>
		private float FoodSaturationLevel;

		/// <summary>
		/// The player's food exhaustion. </summary>
		private float FoodExhaustionLevel;

		/// <summary>
		/// The player's food timer value. </summary>
		private int FoodTimer;
		private int PrevFoodLevel;

		public FoodStats()
		{
			FoodTimer = 0;
			FoodLevel = 20;
			PrevFoodLevel = 20;
			FoodSaturationLevel = 5F;
		}

		/// <summary>
		/// Args: int foodLevel, float foodSaturationModifier
		/// </summary>
		public virtual void AddStats(int par1, float par2)
		{
			FoodLevel = Math.Min(par1 + FoodLevel, 20);
			FoodSaturationLevel = Math.Min(FoodSaturationLevel + (float)par1 * par2 * 2.0F, FoodLevel);
		}

		/// <summary>
		/// Eat some food.
		/// </summary>
		public virtual void AddStats(ItemFood par1ItemFood)
		{
			AddStats(par1ItemFood.GetHealAmount(), par1ItemFood.GetSaturationModifier());
		}

		/// <summary>
		/// Handles the food game logic.
		/// </summary>
		public virtual void OnUpdate(EntityPlayer par1EntityPlayer)
		{
			int i = par1EntityPlayer.WorldObj.DifficultySetting;
			PrevFoodLevel = FoodLevel;

			if (FoodExhaustionLevel > 4F)
			{
				FoodExhaustionLevel -= 4F;

				if (FoodSaturationLevel > 0.0F)
				{
					FoodSaturationLevel = Math.Max(FoodSaturationLevel - 1.0F, 0.0F);
				}
				else if (i > 0)
				{
					FoodLevel = Math.Max(FoodLevel - 1, 0);
				}
			}

			if (FoodLevel >= 18 && par1EntityPlayer.ShouldHeal())
			{
				FoodTimer++;

				if (FoodTimer >= 80)
				{
					par1EntityPlayer.Heal(1);
					FoodTimer = 0;
				}
			}
			else if (FoodLevel <= 0)
			{
				FoodTimer++;

				if (FoodTimer >= 80)
				{
					if (par1EntityPlayer.GetHealth() > 10 || i >= 3 || par1EntityPlayer.GetHealth() > 1 && i >= 2)
					{
						par1EntityPlayer.AttackEntityFrom(DamageSource.Starve, 1);
					}

					FoodTimer = 0;
				}
			}
			else
			{
				FoodTimer = 0;
			}
		}

		/// <summary>
		/// Reads food stats from an NBT object.
		/// </summary>
		public virtual void ReadNBT(NBTTagCompound par1NBTTagCompound)
		{
			if (par1NBTTagCompound.HasKey("foodLevel"))
			{
				FoodLevel = par1NBTTagCompound.GetInteger("foodLevel");
				FoodTimer = par1NBTTagCompound.GetInteger("foodTickTimer");
				FoodSaturationLevel = par1NBTTagCompound.GetFloat("foodSaturationLevel");
				FoodExhaustionLevel = par1NBTTagCompound.GetFloat("foodExhaustionLevel");
			}
		}

		/// <summary>
		/// Writes food stats to an NBT object.
		/// </summary>
		public virtual void WriteNBT(NBTTagCompound par1NBTTagCompound)
		{
			par1NBTTagCompound.SetInteger("foodLevel", FoodLevel);
			par1NBTTagCompound.SetInteger("foodTickTimer", FoodTimer);
			par1NBTTagCompound.SetFloat("foodSaturationLevel", FoodSaturationLevel);
			par1NBTTagCompound.SetFloat("foodExhaustionLevel", FoodExhaustionLevel);
		}

		/// <summary>
		/// Get the player's food level.
		/// </summary>
		public virtual int GetFoodLevel()
		{
			return FoodLevel;
		}

		public virtual int GetPrevFoodLevel()
		{
			return PrevFoodLevel;
		}

		/// <summary>
		/// If foodLevel is not max.
		/// </summary>
		public virtual bool NeedFood()
		{
			return FoodLevel < 20;
		}

		/// <summary>
		/// adds input to foodExhaustionLevel to a max of 40
		/// </summary>
		public virtual void AddExhaustion(float par1)
		{
			FoodExhaustionLevel = Math.Min(FoodExhaustionLevel + par1, 40F);
		}

		/// <summary>
		/// Get the player's food saturation level.
		/// </summary>
		public virtual float GetSaturationLevel()
		{
			return FoodSaturationLevel;
		}

		public virtual void SetFoodLevel(int par1)
		{
			FoodLevel = par1;
		}

		public virtual void SetFoodSaturationLevel(float par1)
		{
			FoodSaturationLevel = par1;
		}
	}

}