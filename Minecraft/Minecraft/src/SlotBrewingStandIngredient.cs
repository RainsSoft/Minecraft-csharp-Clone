namespace net.minecraft.src
{
	class SlotBrewingStandIngredient : Slot
	{
		/// <summary>
		/// The brewing stand this slot belongs to. </summary>
		readonly ContainerBrewingStand Container;

		public SlotBrewingStandIngredient(ContainerBrewingStand par1ContainerBrewingStand, IInventory par2IInventory, int par3, int par4, int par5) : base(par2IInventory, par3, par4, par5)
		{
			Container = par1ContainerBrewingStand;
		}

		/// <summary>
		/// Check if the stack is a valid item for this slot. Always true beside for the armor slots.
		/// </summary>
		public override bool IsItemValid(ItemStack par1ItemStack)
		{
			if (par1ItemStack != null)
			{
				return Item.ItemsList[par1ItemStack.ItemID].IsPotionIngredient();
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Returns the maximum stack size for a given slot (usually the same as getInventoryStackLimit(), but 1 in the case
		/// of armor slots)
		/// </summary>
		public override int GetSlotStackLimit()
		{
			return 64;
		}
	}
}