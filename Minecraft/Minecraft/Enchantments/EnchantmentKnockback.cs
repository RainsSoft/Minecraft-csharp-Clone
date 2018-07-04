namespace net.minecraft.src
{
	public class EnchantmentKnockback : Enchantment
	{
        public EnchantmentKnockback(int par1, int par2)
            : base(par1, par2, EnumEnchantmentType.weapon)
		{
			SetName("knockback");
		}

		/// <summary>
		/// Returns the minimal value of enchantability nedded on the enchantment level passed.
		/// </summary>
		public override int GetMinEnchantability(int par1)
		{
			return 5 + 20 * (par1 - 1);
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
			return 2;
		}
	}
}