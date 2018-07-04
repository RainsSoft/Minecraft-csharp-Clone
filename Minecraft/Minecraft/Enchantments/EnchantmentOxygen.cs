namespace net.minecraft.src
{

	public class EnchantmentOxygen : Enchantment
	{
		public EnchantmentOxygen(int par1, int par2) : base(par1, par2, EnumEnchantmentType.armor_head)
		{
			SetName("oxygen");
		}

		/// <summary>
		/// Returns the minimal value of enchantability nedded on the enchantment level passed.
		/// </summary>
		public override int GetMinEnchantability(int par1)
		{
			return 10 * par1;
		}

		/// <summary>
		/// Returns the maximum value of enchantability nedded on the enchantment level passed.
		/// </summary>
		public override int GetMaxEnchantability(int par1)
		{
			return GetMinEnchantability(par1) + 30;
		}

		/// <summary>
		/// Returns the maximum level that the enchantment can have.
		/// </summary>
		public override int GetMaxLevel()
		{
			return 3;
		}
	}

}