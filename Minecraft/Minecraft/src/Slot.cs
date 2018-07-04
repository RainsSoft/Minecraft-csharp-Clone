namespace net.minecraft.src
{

	public class Slot
	{
		/// <summary>
		/// The index of the slot in the inventory. </summary>
		private readonly int SlotIndex;

		/// <summary>
		/// The inventory we want to extract a slot from. </summary>
		public readonly IInventory Inventory;

		/// <summary>
		/// the id of the slot(also the index in the inventory arraylist) </summary>
		public int SlotNumber;

		/// <summary>
		/// display position of the inventory slot on the screen x axis </summary>
		public int XDisplayPosition;

		/// <summary>
		/// display position of the inventory slot on the screen y axis </summary>
		public int YDisplayPosition;

		public Slot(IInventory par1IInventory, int par2, int par3, int par4)
		{
			Inventory = par1IInventory;
			SlotIndex = par2;
			XDisplayPosition = par3;
			YDisplayPosition = par4;
		}

		public virtual void Func_48433_a(ItemStack par1ItemStack, ItemStack par2ItemStack)
		{
			if (par1ItemStack == null || par2ItemStack == null)
			{
				return;
			}

			if (par1ItemStack.ItemID != par2ItemStack.ItemID)
			{
				return;
			}

			int i = par2ItemStack.StackSize - par1ItemStack.StackSize;

			if (i > 0)
			{
				Func_48435_a(par1ItemStack, i);
			}
		}

		protected virtual void Func_48435_a(ItemStack itemstack, int i)
		{
		}

		protected virtual void Func_48434_c(ItemStack itemstack)
		{
		}

		/// <summary>
		/// Called when the player picks up an item from an inventory slot
		/// </summary>
		public virtual void OnPickupFromSlot(ItemStack par1ItemStack)
		{
			OnSlotChanged();
		}

		/// <summary>
		/// Check if the stack is a valid item for this slot. Always true beside for the armor slots.
		/// </summary>
		public virtual bool IsItemValid(ItemStack par1ItemStack)
		{
			return true;
		}

		/// <summary>
		/// Helper fnct to get the stack in the slot.
		/// </summary>
		public virtual ItemStack GetStack()
		{
			return Inventory.GetStackInSlot(SlotIndex);
		}

		/// <summary>
		/// Returns if this slot Contains a stack.
		/// </summary>
		public virtual bool GetHasStack()
		{
			return GetStack() != null;
		}

		/// <summary>
		/// Helper fnct to put a stack in the slot.
		/// </summary>
		public virtual void PutStack(ItemStack par1ItemStack)
		{
			Inventory.SetInventorySlotContents(SlotIndex, par1ItemStack);
			OnSlotChanged();
		}

		/// <summary>
		/// Called when the stack in a Slot changes
		/// </summary>
		public virtual void OnSlotChanged()
		{
			Inventory.OnInventoryChanged();
		}

		/// <summary>
		/// Returns the maximum stack size for a given slot (usually the same as getInventoryStackLimit(), but 1 in the case
		/// of armor slots)
		/// </summary>
		public virtual int GetSlotStackLimit()
		{
			return Inventory.GetInventoryStackLimit();
		}

		/// <summary>
		/// Returns the icon index on items.png that is used as background image of the slot.
		/// </summary>
		public virtual int GetBackgroundIconIndex()
		{
			return -1;
		}

		/// <summary>
		/// Decrease the size of the stack in slot (first int arg) by the amount of the second int arg. Returns the new
		/// stack.
		/// </summary>
		public virtual ItemStack DecrStackSize(int par1)
		{
			return Inventory.DecrStackSize(SlotIndex, par1);
		}
	}

}