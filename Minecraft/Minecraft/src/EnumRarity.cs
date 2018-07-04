namespace net.minecraft.src
{
	public class Rarity
	{
		public static Rarity Common = new Rarity(15, "Common");

		public static Rarity Uncommon = new Rarity(14, "Uncommon");

        public static Rarity Rare = new Rarity(11, "Rare");

		public static Rarity Epic = new Rarity(13, "Epic");

		/// <summary>
		/// The color given to the name of items with that rarity. </summary>
		public int NameColor;

		public string Field_40532_f;

		private Rarity(int par3, string par4Str)
		{
			NameColor = par3;
			Field_40532_f = par4Str;
		}
	}
}