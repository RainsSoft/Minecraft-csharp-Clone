namespace net.minecraft.src
{

	public class EnchantmentData : WeightedRandomChoice
	{
		/// <summary>
		/// Enchantment object associated with this EnchantmentData </summary>
		public readonly Enchantment Enchantmentobj;

		/// <summary>
		/// Enchantment level associated with this EnchantmentData </summary>
		public readonly int EnchantmentLevel;

		public EnchantmentData(Enchantment par1Enchantment, int par2) : base(par1Enchantment.GetWeight())
		{
			Enchantmentobj = par1Enchantment;
			EnchantmentLevel = par2;
		}
	}

}