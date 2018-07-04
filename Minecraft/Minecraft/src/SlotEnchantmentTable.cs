namespace net.minecraft.src
{
	class SlotEnchantmentTable : InventoryBasic
	{
		/// <summary>
		/// The brewing stand this slot belongs to. </summary>
		readonly ContainerEnchantment Container;

		public SlotEnchantmentTable(ContainerEnchantment par1ContainerEnchantment, string par2Str, int par3) : base(par2Str, par3)
		{
			Container = par1ContainerEnchantment;
		}

		/// <summary>
		/// Returns the maximum stack size for a inventory slot. Seems to always be 64, possibly will be extended. *Isn't
		/// this more of a set than a get?*
		/// </summary>
		public override int GetInventoryStackLimit()
		{
			return 1;
		}

		/// <summary>
		/// Called when an the contents of an Inventory change, usually
		/// </summary>
		public override void OnInventoryChanged()
		{
			base.OnInventoryChanged();
			Container.OnCraftMatrixChanged(this);
		}
	}
}