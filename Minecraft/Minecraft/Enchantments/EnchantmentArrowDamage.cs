namespace net.minecraft.src
{

	public class EnchantmentArrowDamage : Enchantment
	{
		public EnchantmentArrowDamage(int par1, int par2) : base(par1, par2, EnumEnchantmentType.bow)
		{
			SetName("arrowDamage");
		}

		/// <summary>
		/// Returns the minimal value of enchantability nedded on the enchantment level passed.
		/// </summary>
		public override int GetMinEnchantability(int par1)
		{
			return 1 + (par1 - 1) * 10;
		}

		/// <summary>
		/// Returns the maximum value of enchantability nedded on the enchantment level passed.
		/// </summary>
		public override int GetMaxEnchantability(int par1)
		{
			return GetMinEnchantability(par1) + 15;
		}

		/// <summary>
		/// Returns the maximum level that the enchantment can have.
		/// </summary>
		public override int GetMaxLevel()
		{
			return 5;
		}
	}

}