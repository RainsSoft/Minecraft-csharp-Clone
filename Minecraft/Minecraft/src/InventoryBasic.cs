using System.Collections.Generic;

namespace net.minecraft.src
{
	public class InventoryBasic : IInventory
	{
		private string InventoryTitle;
		private int SlotsCount;
		private ItemStack[] InventoryContents;
		private List<IInvBasic> Field_20073_d;

		public InventoryBasic(string par1Str, int par2)
		{
			InventoryTitle = par1Str;
			SlotsCount = par2;
			InventoryContents = new ItemStack[par2];
		}

		/// <summary>
		/// Returns the stack in slot i
		/// </summary>
		public virtual ItemStack GetStackInSlot(int par1)
		{
			return InventoryContents[par1];
		}

		/// <summary>
		/// Decrease the size of the stack in slot (first int arg) by the amount of the second int arg. Returns the new
		/// stack.
		/// </summary>
		public virtual ItemStack DecrStackSize(int par1, int par2)
		{
			if (InventoryContents[par1] != null)
			{
				if (InventoryContents[par1].StackSize <= par2)
				{
					ItemStack itemstack = InventoryContents[par1];
					InventoryContents[par1] = null;
					OnInventoryChanged();
					return itemstack;
				}

				ItemStack itemstack1 = InventoryContents[par1].SplitStack(par2);

				if (InventoryContents[par1].StackSize == 0)
				{
					InventoryContents[par1] = null;
				}

				OnInventoryChanged();
				return itemstack1;
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// When some containers are closed they call this on each slot, then drop whatever it returns as an EntityItem -
		/// like when you close a workbench GUI.
		/// </summary>
		public virtual ItemStack GetStackInSlotOnClosing(int par1)
		{
			if (InventoryContents[par1] != null)
			{
				ItemStack itemstack = InventoryContents[par1];
				InventoryContents[par1] = null;
				return itemstack;
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
			InventoryContents[par1] = par2ItemStack;

			if (par2ItemStack != null && par2ItemStack.StackSize > GetInventoryStackLimit())
			{
				par2ItemStack.StackSize = GetInventoryStackLimit();
			}

			OnInventoryChanged();
		}

		/// <summary>
		/// Returns the number of slots in the inventory.
		/// </summary>
		public virtual int GetSizeInventory()
		{
			return SlotsCount;
		}

		/// <summary>
		/// Returns the name of the inventory.
		/// </summary>
		public virtual string GetInvName()
		{
			return InventoryTitle;
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
			if (Field_20073_d != null)
			{
				for (int i = 0; i < Field_20073_d.Count; i++)
				{
					Field_20073_d[i].OnInventoryChanged(this);
				}
			}
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