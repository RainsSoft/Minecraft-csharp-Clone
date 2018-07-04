namespace net.minecraft.src
{
	public class ContainerFurnace : Container
	{
		private TileEntityFurnace Furnace;
		private int LastCookTime;
		private int LastBurnTime;
		private int LastItemBurnTime;

		public ContainerFurnace(InventoryPlayer par1InventoryPlayer, TileEntityFurnace par2TileEntityFurnace)
		{
			LastCookTime = 0;
			LastBurnTime = 0;
			LastItemBurnTime = 0;
			Furnace = par2TileEntityFurnace;
			AddSlot(new Slot(par2TileEntityFurnace, 0, 56, 17));
			AddSlot(new Slot(par2TileEntityFurnace, 1, 56, 53));
			AddSlot(new SlotFurnace(par1InventoryPlayer.Player, par2TileEntityFurnace, 2, 116, 35));

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

				if (LastCookTime != Furnace.FurnaceCookTime)
				{
					icrafting.UpdateCraftingInventoryInfo(this, 0, Furnace.FurnaceCookTime);
				}

				if (LastBurnTime != Furnace.FurnaceBurnTime)
				{
					icrafting.UpdateCraftingInventoryInfo(this, 1, Furnace.FurnaceBurnTime);
				}

				if (LastItemBurnTime != Furnace.CurrentItemBurnTime)
				{
					icrafting.UpdateCraftingInventoryInfo(this, 2, Furnace.CurrentItemBurnTime);
				}
			}

			LastCookTime = Furnace.FurnaceCookTime;
			LastBurnTime = Furnace.FurnaceBurnTime;
			LastItemBurnTime = Furnace.CurrentItemBurnTime;
		}

		public override void UpdateProgressBar(int par1, int par2)
		{
			if (par1 == 0)
			{
				Furnace.FurnaceCookTime = par2;
			}

			if (par1 == 1)
			{
				Furnace.FurnaceBurnTime = par2;
			}

			if (par1 == 2)
			{
				Furnace.CurrentItemBurnTime = par2;
			}
		}

		public override bool CanInteractWith(EntityPlayer par1EntityPlayer)
		{
			return Furnace.IsUseableByPlayer(par1EntityPlayer);
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

				if (par1 == 2)
				{
					if (!MergeItemStack(itemstack1, 3, 39, true))
					{
						return null;
					}

					slot.Func_48433_a(itemstack1, itemstack);
				}
				else if (par1 == 1 || par1 == 0)
				{
					if (!MergeItemStack(itemstack1, 3, 39, false))
					{
						return null;
					}
				}
				else if (FurnaceRecipes.Smelting().GetSmeltingResult(itemstack1.GetItem().ShiftedIndex) != null)
				{
					if (!MergeItemStack(itemstack1, 0, 1, false))
					{
						return null;
					}
				}
				else if (TileEntityFurnace.Func_52005_b(itemstack1))
				{
					if (!MergeItemStack(itemstack1, 1, 2, false))
					{
						return null;
					}
				}
				else if (par1 >= 3 && par1 < 30)
				{
					if (!MergeItemStack(itemstack1, 30, 39, false))
					{
						return null;
					}
				}
				else if (par1 >= 30 && par1 < 39 && !MergeItemStack(itemstack1, 3, 30, false))
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