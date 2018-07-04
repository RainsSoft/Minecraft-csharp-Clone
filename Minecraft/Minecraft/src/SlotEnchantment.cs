namespace net.minecraft.src
{
	class SlotEnchantment : Slot
	{
		/// <summary>
		/// The brewing stand this slot belongs to. </summary>
		readonly ContainerEnchantment Container;

		public SlotEnchantment(ContainerEnchantment par1ContainerEnchantment, IInventory par2IInventory, int par3, int par4, int par5) : base(par2IInventory, par3, par4, par5)
		{
			Container = par1ContainerEnchantment;
		}

		/// <summary>
		/// Check if the stack is a valid item for this slot. Always true beside for the armor slots.
		/// </summary>
		public override bool IsItemValid(ItemStack par1ItemStack)
		{
			return true;
		}
	}
}