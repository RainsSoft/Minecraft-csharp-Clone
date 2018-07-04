namespace net.minecraft.src
{
	public class ContainerBrewingStand : Container
	{
		private TileEntityBrewingStand TileBrewingStand;
		private int BrewTime;

		public ContainerBrewingStand(InventoryPlayer par1InventoryPlayer, TileEntityBrewingStand par2TileEntityBrewingStand)
		{
			BrewTime = 0;
			TileBrewingStand = par2TileEntityBrewingStand;
			AddSlot(new SlotBrewingStandPotion(this, par1InventoryPlayer.Player, par2TileEntityBrewingStand, 0, 56, 46));
			AddSlot(new SlotBrewingStandPotion(this, par1InventoryPlayer.Player, par2TileEntityBrewingStand, 1, 79, 53));
			AddSlot(new SlotBrewingStandPotion(this, par1InventoryPlayer.Player, par2TileEntityBrewingStand, 2, 102, 46));
			AddSlot(new SlotBrewingStandIngredient(this, par2TileEntityBrewingStand, 3, 79, 17));

			for (int i = 0; i < 3; i++)
			{
				for (int k = 0; k < 9; k++)
				{
					AddSlot(new Slot(par1InventoryPlayer, k + i * 9 + 9, 8 + k * 18, 84 + i * 18));
				}
			}

			for (int j = 0; j < 9; j++)
			{
				AddSlot(new Slot(par1InventoryPlayer, j, 8 + j * 18, 142));
			}
		}

		/// <summary>
		/// Updates crafting matrix; called from onCraftMatrixChanged. Args: none
		/// </summary>
		public override void UpdateCraftingResults()
		{
			base.UpdateCraftingResults();

			for (int i = 0; i < Crafters.Count; i++)
			{
				ICrafting icrafting = (ICrafting)Crafters[i];

				if (BrewTime != TileBrewingStand.GetBrewTime())
				{
					icrafting.UpdateCraftingInventoryInfo(this, 0, TileBrewingStand.GetBrewTime());
				}
			}

			BrewTime = TileBrewingStand.GetBrewTime();
		}

		public override void UpdateProgressBar(int par1, int par2)
		{
			if (par1 == 0)
			{
				TileBrewingStand.SetBrewTime(par2);
			}
		}

		public override bool CanInteractWith(EntityPlayer par1EntityPlayer)
		{
			return TileBrewingStand.IsUseableByPlayer(par1EntityPlayer);
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

				if (par1 >= 0 && par1 <= 2 || par1 == 3)
				{
					if (!MergeItemStack(itemstack1, 4, 40, true))
					{
						return null;
					}

					slot.Func_48433_a(itemstack1, itemstack);
				}
				else if (par1 >= 4 && par1 < 31)
				{
					if (!MergeItemStack(itemstack1, 31, 40, false))
					{
						return null;
					}
				}
				else if (par1 >= 31 && par1 < 40)
				{
					if (!MergeItemStack(itemstack1, 4, 31, false))
					{
						return null;
					}
				}
				else if (!MergeItemStack(itemstack1, 4, 40, false))
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