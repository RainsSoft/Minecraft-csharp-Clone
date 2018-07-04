namespace net.minecraft.src
{
	public class EnchantmentLootBonus : Enchantment
	{
        public EnchantmentLootBonus(int par1, int par2, EnumEnchantmentType par3EnumEnchantmentType)
            : base(par1, par2, par3EnumEnchantmentType)
		{
			SetName("lootBonus");

			if (par3EnumEnchantmentType == EnumEnchantmentType.digger)
			{
				SetName("lootBonusDigger");
			}
		}

		/// <summary>
		/// Returns the minimal value of enchantability nedded on the enchantment level passed.
		/// </summary>
		public override int GetMinEnchantability(int par1)
		{
			return 20 + (par1 - 1) * 12;
		}

		/// <summary>
		/// Returns the maximum value of enchantability nedded on the enchantment level passed.
		/// </summary>
		public override int GetMaxEnchantability(int par1)
		{
			return base.GetMinEnchantability(par1) + 50;
		}

		/// <summary>
		/// Returns the maximum level that the enchantment can have.
		/// </summary>
		public override int GetMaxLevel()
		{
			return 3;
		}

		/// <summary>
		/// Determines if the enchantment passed can be applyied together with this enchantment.
		/// </summary>
		public override bool CanApplyTogether(Enchantment par1Enchantment)
		{
			return base.CanApplyTogether(par1Enchantment) && par1Enchantment.EffectId != SilkTouch.EffectId;
		}
	}
}