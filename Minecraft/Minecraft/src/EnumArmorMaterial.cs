namespace net.minecraft.src
{
	public class ArmorMaterial
	{
		public static ArmorMaterial CLOTH = new ArmorMaterial(5, new int[] { 1, 3, 2, 1 }, 15);

		public static ArmorMaterial CHAIN = new ArmorMaterial(15, new int[] { 2, 5, 4, 1 }, 12);

		public static ArmorMaterial IRON = new ArmorMaterial(15, new int[] { 2, 6, 5, 2 }, 9);

		public static ArmorMaterial GOLD = new ArmorMaterial(7, new int[] { 2, 5, 3, 1 }, 25);

		public static ArmorMaterial DIAMOND = new ArmorMaterial(33, new int[] { 3, 8, 6, 3 }, 10);

		/// <summary>
		/// Holds the maximum damage factor (each piece multiply this by it's own value) of the material, this is the item
		/// damage (how much can absorb before breaks)
		/// </summary>
		public int MaxDamageFactor;

		public int[] DamageReductionAmountArray;

		/// <summary>
		/// Return the enchantability factor of the material </summary>
		public int Enchantability;

		private ArmorMaterial(int par3, int[] par4ArrayOfInteger , int par5)
		{
			MaxDamageFactor = par3;
			DamageReductionAmountArray = par4ArrayOfInteger;
			Enchantability = par5;
		}

        ///<summary>
        /// Returns the durability for a armor slot of for this type.
        ///</summary>
        public int GetDurability(int par1)
        {
            return ItemArmor.GetMaxDamageArray()[par1] * MaxDamageFactor;
        }

        ///<summary>
        /// Return the damage reduction (each 1 point is a half a shield on gui) of the piece index passed (0 = helmet, 1 =
        /// plate, 2 = legs and 3 = boots)
        ///</summary>
        public int GetDamageReductionAmount(int par1)
        {
            return DamageReductionAmountArray[par1];
        }
    }
}