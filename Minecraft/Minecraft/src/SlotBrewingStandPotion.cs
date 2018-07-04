namespace net.minecraft.src
{

	class SlotBrewingStandPotion : Slot
	{
		/// <summary>
		/// The player that has this container open. </summary>
		private EntityPlayer Player;

		/// <summary>
		/// The brewing stand this slot belongs to. </summary>
		readonly ContainerBrewingStand Container;

		public SlotBrewingStandPotion(ContainerBrewingStand par1ContainerBrewingStand, EntityPlayer par2EntityPlayer, IInventory par3IInventory, int par4, int par5, int par6) : base(par3IInventory, par4, par5, par6)
		{
			Container = par1ContainerBrewingStand;
			Player = par2EntityPlayer;
		}

		/// <summary>
		/// Check if the stack is a valid item for this slot. Always true beside for the armor slots.
		/// </summary>
		public override bool IsItemValid(ItemStack par1ItemStack)
		{
			return par1ItemStack != null && (par1ItemStack.ItemID == Item.Potion.ShiftedIndex || par1ItemStack.ItemID == Item.GlassBottle.ShiftedIndex);
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
		/// Called when the player picks up an item from an inventory slot
		/// </summary>
		public override void OnPickupFromSlot(ItemStack par1ItemStack)
		{
			if (par1ItemStack.ItemID == Item.Potion.ShiftedIndex && par1ItemStack.GetItemDamage() > 0)
			{
				Player.AddStat(AchievementList.Potion, 1);
			}

			base.OnPickupFromSlot(par1ItemStack);
		}
	}

}