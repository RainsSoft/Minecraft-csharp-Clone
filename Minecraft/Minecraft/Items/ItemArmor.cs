namespace net.minecraft.src
{
	public class ItemArmor : Item
	{
		private static readonly int[] MaxDamageArray = { 11, 16, 15, 13 };

		/// <summary>
		/// Stores the armor type: 0 is helmet, 1 is plate, 2 is legs and 3 is boots
		/// </summary>
		public readonly int ArmorType;

		/// <summary>
		/// Holds the amount of damage that the armor reduces at full durability. </summary>
		public readonly int DamageReduceAmount;

		/// <summary>
		/// Used on RenderPlayer to select the correspondent armor to be rendered on the player: 0 is cloth, 1 is chain, 2 is
		/// iron, 3 is diamond and 4 is gold.
		/// </summary>
		public readonly int RenderIndex;

		/// <summary>
		/// The EnumArmorMaterial used for this ItemArmor </summary>
		private readonly ArmorMaterial Material;

		public ItemArmor(int par1, ArmorMaterial par2EnumArmorMaterial, int par3, int par4) : base(par1)
		{
			Material = par2EnumArmorMaterial;
			ArmorType = par4;
			RenderIndex = par3;
			DamageReduceAmount = par2EnumArmorMaterial.GetDamageReductionAmount(par4);
			SetMaxDamage(par2EnumArmorMaterial.GetDurability(par4));
			MaxStackSize = 1;
		}

		/// <summary>
		/// Return the enchantability factor of the item, most of the time is based on material.
		/// </summary>
		public override int GetItemEnchantability()
		{
			return Material.Enchantability;
		}

		/// <summary>
		/// Returns the 'max damage' factor array for the armor, each piece of armor have a durability factor (that gets
		/// multiplied by armor material factor)
		/// </summary>
		public static int[] GetMaxDamageArray()
		{
			return MaxDamageArray;
		}
	}
}