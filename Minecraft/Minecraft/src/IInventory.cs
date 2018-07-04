namespace net.minecraft.src
{

	public interface IInventory
	{
		/// <summary>
		/// Returns the number of slots in the inventory.
		/// </summary>
		int GetSizeInventory();

		/// <summary>
		/// Returns the stack in slot i
		/// </summary>
		ItemStack GetStackInSlot(int i);

		/// <summary>
		/// Decrease the size of the stack in slot (first int arg) by the amount of the second int arg. Returns the new
		/// stack.
		/// </summary>
		ItemStack DecrStackSize(int i, int j);

		/// <summary>
		/// When some containers are closed they call this on each slot, then drop whatever it returns as an EntityItem -
		/// like when you close a workbench GUI.
		/// </summary>
		ItemStack GetStackInSlotOnClosing(int i);

		/// <summary>
		/// Sets the given item stack to the specified slot in the inventory (can be crafting or armor sections).
		/// </summary>
		void SetInventorySlotContents(int i, ItemStack itemstack);

		/// <summary>
		/// Returns the name of the inventory.
		/// </summary>
		string GetInvName();

		/// <summary>
		/// Returns the maximum stack size for a inventory slot. Seems to always be 64, possibly will be extended. *Isn't
		/// this more of a set than a get?*
		/// </summary>
		int GetInventoryStackLimit();

		/// <summary>
		/// Called when an the contents of an Inventory change, usually
		/// </summary>
		void OnInventoryChanged();

		/// <summary>
		/// Do not make give this method the name canInteractWith because it clashes with Container
		/// </summary>
		bool IsUseableByPlayer(EntityPlayer entityplayer);

		void OpenChest();

		void CloseChest();
	}

}