namespace net.minecraft.src
{

	interface IEnchantmentModifier
	{
		/// <summary>
		/// Generic method use to calculate modifiers of offensive or defensive enchantment values.
		/// </summary>
		void CalculateModifier(Enchantment enchantment, int i);
	}

}