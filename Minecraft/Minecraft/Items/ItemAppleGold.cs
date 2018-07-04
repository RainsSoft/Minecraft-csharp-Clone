namespace net.minecraft.src
{
	public class ItemAppleGold : ItemFood
	{
		public ItemAppleGold(int par1, int par2, float par3, bool par4) : base(par1, par2, par3, par4)
		{
		}

		public override bool HasEffect(ItemStack par1ItemStack)
		{
			return true;
		}

		/// <summary>
		/// Return an item rarity from EnumRarity
		/// </summary>
		public override Rarity GetRarity(ItemStack par1ItemStack)
		{
			return Rarity.Epic;
		}
	}
}