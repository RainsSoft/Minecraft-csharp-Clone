namespace net.minecraft.src
{

	public class ItemCoal : Item
	{
		public ItemCoal(int par1) : base(par1)
		{
			SetHasSubtypes(true);
			SetMaxDamage(0);
		}

		public override string GetItemNameIS(ItemStack par1ItemStack)
		{
			if (par1ItemStack.GetItemDamage() == 1)
			{
				return "item.charcoal";
			}
			else
			{
				return "item.coal";
			}
		}
	}

}