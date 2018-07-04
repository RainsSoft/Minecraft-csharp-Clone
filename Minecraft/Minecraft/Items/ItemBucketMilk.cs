namespace net.minecraft.src
{

	public class ItemBucketMilk : Item
	{
		public ItemBucketMilk(int par1) : base(par1)
		{
			SetMaxStackSize(1);
		}

		public override ItemStack OnFoodEaten(ItemStack par1ItemStack, World par2World, EntityPlayer par3EntityPlayer)
		{
			par1ItemStack.StackSize--;

			if (!par2World.IsRemote)
			{
				par3EntityPlayer.ClearActivePotions();
			}

			if (par1ItemStack.StackSize <= 0)
			{
				return new ItemStack(Item.BucketEmpty);
			}
			else
			{
				return par1ItemStack;
			}
		}

		/// <summary>
		/// How long it takes to use or consume an item
		/// </summary>
		public override int GetMaxItemUseDuration(ItemStack par1ItemStack)
		{
			return 32;
		}

		/// <summary>
		/// returns the action that specifies what animation to play when the items is being used
		/// </summary>
		public override EnumAction GetItemUseAction(ItemStack par1ItemStack)
		{
			return EnumAction.Drink;
		}

		/// <summary>
		/// Called whenever this item is equipped and the right mouse button is pressed. Args: itemStack, world, entityPlayer
		/// </summary>
		public override ItemStack OnItemRightClick(ItemStack par1ItemStack, World par2World, EntityPlayer par3EntityPlayer)
		{
			par3EntityPlayer.SetItemInUse(par1ItemStack, GetMaxItemUseDuration(par1ItemStack));
			return par1ItemStack;
		}
	}

}