namespace net.minecraft.src
{
	public class ContainerPlayer : Container
	{
		/// <summary>
		/// The crafting matrix inventory. </summary>
		public InventoryCrafting CraftMatrix;
		public IInventory CraftResult;

		/// <summary>
		/// Determines if inventory manipulation should be handled. </summary>
		public bool IsLocalWorld;

		public ContainerPlayer(InventoryPlayer par1InventoryPlayer) : this(par1InventoryPlayer, true)
		{
		}

		public ContainerPlayer(InventoryPlayer par1InventoryPlayer, bool par2)
		{
			CraftMatrix = new InventoryCrafting(this, 2, 2);
			CraftResult = new InventoryCraftResult();
			IsLocalWorld = false;
			IsLocalWorld = par2;
			AddSlot(new SlotCrafting(par1InventoryPlayer.Player, CraftMatrix, CraftResult, 0, 144, 36));

			for (int i = 0; i < 2; i++)
			{
				for (int i1 = 0; i1 < 2; i1++)
				{
					AddSlot(new Slot(CraftMatrix, i1 + i * 2, 88 + i1 * 18, 26 + i * 18));
				}
			}

			for (int j = 0; j < 4; j++)
			{
				int j1 = j;
				AddSlot(new SlotArmor(this, par1InventoryPlayer, par1InventoryPlayer.GetSizeInventory() - 1 - j, 8, 8 + j * 18, j1));
			}

			for (int k = 0; k < 3; k++)
			{
				for (int k1 = 0; k1 < 9; k1++)
				{
					AddSlot(new Slot(par1InventoryPlayer, k1 + (k + 1) * 9, 8 + k1 * 18, 84 + k * 18));
				}
			}

			for (int l = 0; l < 9; l++)
			{
				AddSlot(new Slot(par1InventoryPlayer, l, 8 + l * 18, 142));
			}

			OnCraftMatrixChanged(CraftMatrix);
		}

		/// <summary>
		/// Callback for when the crafting matrix is changed.
		/// </summary>
		public override void OnCraftMatrixChanged(IInventory par1IInventory)
		{
			CraftResult.SetInventorySlotContents(0, CraftingManager.GetInstance().FindMatchingRecipe(CraftMatrix));
		}

		/// <summary>
		/// Callback for when the crafting gui is closed.
		/// </summary>
		public override void OnCraftGuiClosed(EntityPlayer par1EntityPlayer)
		{
			base.OnCraftGuiClosed(par1EntityPlayer);

			for (int i = 0; i < 4; i++)
			{
				ItemStack itemstack = CraftMatrix.GetStackInSlotOnClosing(i);

				if (itemstack != null)
				{
					par1EntityPlayer.DropPlayerItem(itemstack);
				}
			}

			CraftResult.SetInventorySlotContents(0, null);
		}

		public override bool CanInteractWith(EntityPlayer par1EntityPlayer)
		{
			return true;
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

				if (par1 == 0)
				{
					if (!MergeItemStack(itemstack1, 9, 45, true))
					{
						return null;
					}

					slot.Func_48433_a(itemstack1, itemstack);
				}
				else if (par1 >= 9 && par1 < 36)
				{
					if (!MergeItemStack(itemstack1, 36, 45, false))
					{
						return null;
					}
				}
				else if (par1 >= 36 && par1 < 45)
				{
					if (!MergeItemStack(itemstack1, 9, 36, false))
					{
						return null;
					}
				}
				else if (!MergeItemStack(itemstack1, 9, 45, false))
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