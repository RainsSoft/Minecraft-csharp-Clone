namespace net.minecraft.src
{

	public class InventoryCrafting : IInventory
	{
		private ItemStack[] StackList;

		/// <summary>
		/// the width of the crafting inventory </summary>
		private int InventoryWidth;

		/// <summary>
		/// Class containing the callbacks for the events on_GUIClosed and on_CraftMaxtrixChanged.
		/// </summary>
		private Container EventHandler;

		public InventoryCrafting(Container par1Container, int par2, int par3)
		{
			int i = par2 * par3;
			StackList = new ItemStack[i];
			EventHandler = par1Container;
			InventoryWidth = par2;
		}

		/// <summary>
		/// Returns the number of slots in the inventory.
		/// </summary>
		public virtual int GetSizeInventory()
		{
			return StackList.Length;
		}

		/// <summary>
		/// Returns the stack in slot i
		/// </summary>
		public virtual ItemStack GetStackInSlot(int par1)
		{
			if (par1 >= GetSizeInventory())
			{
				return null;
			}
			else
			{
				return StackList[par1];
			}
		}

		/// <summary>
		/// Returns the itemstack in the slot specified (Top left is 0, 0). Args: row, column
		/// </summary>
		public virtual ItemStack GetStackInRowAndColumn(int par1, int par2)
		{
			if (par1 < 0 || par1 >= InventoryWidth)
			{
				return null;
			}
			else
			{
				int i = par1 + par2 * InventoryWidth;
				return GetStackInSlot(i);
			}
		}

		/// <summary>
		/// Returns the name of the inventory.
		/// </summary>
		public virtual string GetInvName()
		{
			return "container.crafting";
		}

		/// <summary>
		/// When some containers are closed they call this on each slot, then drop whatever it returns as an EntityItem -
		/// like when you close a workbench GUI.
		/// </summary>
		public virtual ItemStack GetStackInSlotOnClosing(int par1)
		{
			if (StackList[par1] != null)
			{
				ItemStack itemstack = StackList[par1];
				StackList[par1] = null;
				return itemstack;
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Decrease the size of the stack in slot (first int arg) by the amount of the second int arg. Returns the new
		/// stack.
		/// </summary>
		public virtual ItemStack DecrStackSize(int par1, int par2)
		{
			if (StackList[par1] != null)
			{
				if (StackList[par1].StackSize <= par2)
				{
					ItemStack itemstack = StackList[par1];
					StackList[par1] = null;
					EventHandler.OnCraftMatrixChanged(this);
					return itemstack;
				}

				ItemStack itemstack1 = StackList[par1].SplitStack(par2);

				if (StackList[par1].StackSize == 0)
				{
					StackList[par1] = null;
				}

				EventHandler.OnCraftMatrixChanged(this);
				return itemstack1;
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Sets the given item stack to the specified slot in the inventory (can be crafting or armor sections).
		/// </summary>
		public virtual void SetInventorySlotContents(int par1, ItemStack par2ItemStack)
		{
			StackList[par1] = par2ItemStack;
			EventHandler.OnCraftMatrixChanged(this);
		}

		/// <summary>
		/// Returns the maximum stack size for a inventory slot. Seems to always be 64, possibly will be extended. *Isn't
		/// this more of a set than a get?*
		/// </summary>
		public virtual int GetInventoryStackLimit()
		{
			return 64;
		}

		/// <summary>
		/// Called when an the contents of an Inventory change, usually
		/// </summary>
		public virtual void OnInventoryChanged()
		{
		}

		/// <summary>
		/// Do not make give this method the name canInteractWith because it clashes with Container
		/// </summary>
		public virtual bool IsUseableByPlayer(EntityPlayer par1EntityPlayer)
		{
			return true;
		}

		public virtual void OpenChest()
		{
		}

		public virtual void CloseChest()
		{
		}
	}

}