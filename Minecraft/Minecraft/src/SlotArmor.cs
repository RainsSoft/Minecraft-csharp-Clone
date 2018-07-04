namespace net.minecraft.src
{
	class SlotArmor : Slot
	{
		/// <summary>
		/// The armor type that can be placed on that slot, it uses the same values of armorType field on ItemArmor.
		/// </summary>
		readonly int ArmorType;

		/// <summary>
		/// The parent class of this clot, ContainerPlayer, SlotArmor is a Anon inner class.
		/// </summary>
		readonly ContainerPlayer Parent;

		public SlotArmor(ContainerPlayer par1ContainerPlayer, IInventory par2IInventory, int par3, int par4, int par5, int par6) : base(par2IInventory, par3, par4, par5)
		{
			Parent = par1ContainerPlayer;
			ArmorType = par6;
		}

		/// <summary>
		/// Returns the maximum stack size for a given slot (usually the same as getInventoryStackLimit(), but 1 in the case
		/// of armor slots)
		/// </summary>
		public override int GetSlotStackLimit()
		{
			return 1;
		}

		/// <summary>
		/// Check if the stack is a valid item for this slot. Always true beside for the armor slots.
		/// </summary>
		public override bool IsItemValid(ItemStack par1ItemStack)
		{
			if (par1ItemStack.GetItem() is ItemArmor)
			{
				return ((ItemArmor)par1ItemStack.GetItem()).ArmorType == ArmorType;
			}

			if (par1ItemStack.GetItem().ShiftedIndex == Block.Pumpkin.BlockID)
			{
				return ArmorType == 0;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Returns the icon index on items.png that is used as background image of the slot.
		/// </summary>
		public override int GetBackgroundIconIndex()
		{
			return 15 + ArmorType * 16;
		}
	}

}