namespace net.minecraft.src
{
	public class EnchantmentFireAspect : Enchantment
	{
        public EnchantmentFireAspect(int par1, int par2)
            : base(par1, par2, EnumEnchantmentType.weapon)
		{
			SetName("fire");
		}

		/// <summary>
		/// Returns the minimal value of enchantability nedded on the enchantment level passed.
		/// </summary>
		public override int GetMinEnchantability(int par1)
		{
			return 10 + 20 * (par1 - 1);
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