namespace net.minecraft.src
{
	sealed class EnchantmentModifierLiving : IEnchantmentModifier
	{
		/// <summary>
		/// Used to calculate the (magic) extra damage based on enchantments of current equipped player item.
		/// </summary>
		public int LivingModifier;

		/// <summary>
		/// Used as parameter to calculate the (magic) extra damage based on enchantments of current equipped player item.
		/// </summary>
		public EntityLiving EntityLiving;

		private EnchantmentModifierLiving()
		{
		}

		/// <summary>
		/// Generic method use to calculate modifiers of offensive or defensive enchantment values.
		/// </summary>
		public void CalculateModifier(Enchantment par1Enchantment, int par2)
		{
			LivingModifier += par1Enchantment.CalcModifierLiving(par2, EntityLiving);
		}

		public EnchantmentModifierLiving(Empty3 par1Empty3) : this()
		{
		}
	}
}