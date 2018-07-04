namespace net.minecraft.src
{
	public class ContainerDispenser : Container
	{
		private TileEntityDispenser TileEntityDispenser;

		public ContainerDispenser(IInventory par1IInventory, TileEntityDispenser par2TileEntityDispenser)
		{
			TileEntityDispenser = par2TileEntityDispenser;

			for (int i = 0; i < 3; i++)
			{
				for (int l = 0; l < 3; l++)
				{
					AddSlot(new Slot(par2TileEntityDispenser, l + i * 3, 62 + l * 18, 17 + i * 18));
				}
			}

			for (int j = 0; j < 3; j++)
			{
				for (int i1 = 0; i1 < 9; i1++)
				{
					AddSlot(new Slot(par1IInventory, i1 + j * 9 + 9, 8 + i1 * 18, 84 + j * 18));
				}
			}

			for (int k = 0; k < 9; k++)
			{
				AddSlot(new Slot(par1IInventory, k, 8 + k * 18, 142));
			}
		}

		public override bool CanInteractWith(EntityPlayer par1EntityPlayer)
		{
			return TileEntityDispenser.IsUseableByPlayer(par1EntityPlayer);
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

				if (par1 < 9)
				{
					if (!MergeItemStack(itemstack1, 9, 45, true))
					{
						return null;
					}
				}
				else if (!MergeItemStack(itemstack1, 0, 9, false))
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

				if (itemstack1.StackSize != itemstack.StackSize)
				{
					slot.OnPickupFromSlot(itemstack1);
				}
				else
				{
					return null;
				}
			}

			return itemstack;
		}
	}

}