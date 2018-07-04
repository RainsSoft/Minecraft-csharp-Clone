namespace net.minecraft.src
{
	public class ContainerChest : Container
	{
		private IInventory LowerChestInventory;
		private int NumRows;

		public ContainerChest(IInventory par1IInventory, IInventory par2IInventory)
		{
			LowerChestInventory = par2IInventory;
			NumRows = par2IInventory.GetSizeInventory() / 9;
			par2IInventory.OpenChest();
			int i = (NumRows - 4) * 18;

			for (int j = 0; j < NumRows; j++)
			{
				for (int i1 = 0; i1 < 9; i1++)
				{
					AddSlot(new Slot(par2IInventory, i1 + j * 9, 8 + i1 * 18, 18 + j * 18));
				}
			}

			for (int k = 0; k < 3; k++)
			{
				for (int j1 = 0; j1 < 9; j1++)
				{
					AddSlot(new Slot(par1IInventory, j1 + k * 9 + 9, 8 + j1 * 18, 103 + k * 18 + i));
				}
			}

			for (int l = 0; l < 9; l++)
			{
				AddSlot(new Slot(par1IInventory, l, 8 + l * 18, 161 + i));
			}
		}

		public override bool CanInteractWith(EntityPlayer par1EntityPlayer)
		{
			return LowerChestInventory.IsUseableByPlayer(par1EntityPlayer);
		}

		/// <summary>
		/// Called to transfer a stack from one inventory to the other eg. when shift clicking.
		/// </summary>
		public override ItemStack TransferStackInSlot(int par1)
		{
			ItemStack itemstack = null;
			Slot slot = (Slot)InventorySlots[par1];

			if (slot != null && slot.GetHasStack())
			{
				ItemStack itemstack1 = slot.GetStack();
				itemstack = itemstack1.Copy();

				if (par1 < NumRows * 9)
				{
					if (!MergeItemStack(itemstack1, NumRows * 9, InventorySlots.Count, true))
					{
						return null;
					}
				}
				else if (!MergeItemStack(itemstack1, 0, NumRows * 9, false))
				{
					return null;
				}

				if (itemstack1.StackSize == 0)
				{
					slot.PutStack(null);
				}
				else
				{
					slot.OnSlotChanged();
				}
			}

			return itemstack;
		}

		/// <summary>
		/// Callback for when the crafting gui is closed.
		/// </summary>
		public override void OnCraftGuiClosed(EntityPlayer par1EntityPlayer)
		{
			base.OnCraftGuiClosed(par1EntityPlayer);
			LowerChestInventory.CloseChest();
		}
	}

}