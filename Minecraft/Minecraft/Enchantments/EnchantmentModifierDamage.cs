namespace net.minecraft.src
{
	sealed class EnchantmentModifierDamage : IEnchantmentModifier
	{
		/// <summary>
		/// Used to calculate the damage modifier (extra armor) on enchantments that the player have on equipped armors.
		/// </summary>
		public int DamageModifier;

		/// <summary>
		/// Used as parameter to calculate the damage modifier (extra armor) on enchantments that the player have on equipped
		/// armors.
		/// </summary>
		public DamageSource DamageSource;

		private EnchantmentModifierDamage()
		{
		}

		/// <summary>
		/// Generic method use to calculate modifiers of offensive or defensive enchantment values.
		/// </summary>
		public void CalculateModifier(Enchantment par1Enchantment, int par2)
		{
			DamageModifier += par1Enchantment.CalcModifierDamage(par2, DamageSource);
		}

		public EnchantmentModifierDamage(Empty3 par1Empty3) : this()
		{
		}
	}
}