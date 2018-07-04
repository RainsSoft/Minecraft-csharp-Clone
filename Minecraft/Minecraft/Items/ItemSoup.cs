namespace net.minecraft.src
{

	public class ItemSoup : ItemFood
	{
		public ItemSoup(int par1, int par2) : base(par1, par2, false)
		{
			SetMaxStackSize(1);
		}

		public override ItemStack OnFoodEaten(ItemStack par1ItemStack, World par2World, EntityPlayer par3EntityPlayer)
		{
			base.OnFoodEaten(par1ItemStack, par2World, par3EntityPlayer);
			return new ItemStack(Item.BowlEmpty);
		}
	}

}