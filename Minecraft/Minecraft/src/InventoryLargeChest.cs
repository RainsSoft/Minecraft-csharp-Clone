namespace net.minecraft.src
{

	public class InventoryLargeChest : IInventory
	{
		/// <summary>
		/// Name of the chest. </summary>
		private string Name;

		/// <summary>
		/// Inventory object corresponding to double chest upper part </summary>
		private IInventory UpperChest;

		/// <summary>
		/// Inventory object corresponding to double chest lower part </summary>
		private IInventory LowerChest;

		public InventoryLargeChest(string par1Str, IInventory par2IInventory, IInventory par3IInventory)
		{
			Name = par1Str;

			if (par2IInventory == null)
			{
				par2IInventory = par3IInventory;
			}

			if (par3IInventory == null)
			{
				par3IInventory = par2IInventory;
			}

			UpperChest = par2IInventory;
			LowerChest = par3IInventory;
		}

		/// <summary>
		/// Returns the number of slots in the inventory.
		/// </summary>
		public virtual int GetSizeInventory()
		{
			return UpperChest.GetSizeInventory() + LowerChest.GetSizeInventory();
		}

		/// <summary>
		/// Returns the name of the inventory.
		/// </summary>
		public virtual string GetInvName()
		{
			return Name;
		}

		/// <summary>
		/// Returns the stack in slot i
		/// </summary>
		public virtual ItemStack GetStackInSlot(int par1)
		{
			if (par1 >= UpperChest.GetSizeInventory())
			{
				return LowerChest.GetStackInSlot(par1 - UpperChest.GetSizeInventory());
			}
			else
			{
				return UpperChest.GetStackInSlot(par1);
			}
		}

		/// <summary>
		/// Decrease the size of the stack in slot (first int arg) by the amount of the second int arg. Returns the new
		/// stack.
		/// </summary>
		public virtual ItemStack DecrStackSize(int par1, int par2)
		{
			if (par1 >= UpperChest.GetSizeInventory())
			{
				return LowerChest.DecrStackSize(par1 - UpperChest.GetSizeInventory(), par2);
			}
			else
			{
				return UpperChest.DecrStackSize(par1, par2);
			}
		}

		/// <summary>
		/// When some containers are closed they call this on each slot, then drop whatever it returns as an EntityItem -
		/// like when you close a workbench GUI.
		/// </summary>
		public virtual ItemStack GetStackInSlotOnClosing(int par1)
		{
			if (par1 >= UpperChest.GetSizeInventory())
			{
				return LowerChest.GetStackInSlotOnClosing(par1 - UpperChest.GetSizeInventory());
			}
			else
			{
				return UpperChest.GetStackInSlotOnClosing(par1);
			}
		}

		/// <summary>
		/// Sets the given item stack to the specified slot in the inventory (can be crafting or armor sections).
		/// </summary>
		public virtual void SetInventorySlotContents(int par1, ItemStack par2ItemStack)
		{
			if (par1 >= UpperChest.GetSizeInventory())
			{
				LowerChest.SetInventorySlotContents(par1 - UpperChest.GetSizeInventory(), par2ItemStack);
			}
			else
			{
				UpperChest.SetInventorySlotContents(par1, par2ItemStack);
			}
		}

		/// <summary>
		/// Returns the maximum stack size for a inventory slot. Seems to always be 64, possibly will be extended. *Isn't
		/// this more of a set than a get?*
		/// </summary>
		public virtual int GetInventoryStackLimit()
		{
			return UpperChest.GetInventoryStackLimit();
		}

		/// <summary>
		/// Called when an the contents of an Inventory change, usually
		/// </summary>
		public virtual void OnInventoryChanged()
		{
			UpperChest.OnInventoryChanged();
			LowerChest.OnInventoryChanged();
		}

		/// <summary>
		/// Do not make give this method the name canInteractWith because it clashes with Container
		/// </summary>
		public virtual bool IsUseableByPlayer(EntityPlayer par1EntityPlayer)
		{
			return UpperChest.IsUseableByPlayer(par1EntityPlayer) && LowerChest.IsUseableByPlayer(par1EntityPlayer);
		}

		public virtual void OpenChest()
		{
			UpperChest.OpenChest();
			LowerChest.OpenChest();
		}

		public virtual void CloseChest()
		{
			UpperChest.CloseChest();
			LowerChest.CloseChest();
		}
	}

}