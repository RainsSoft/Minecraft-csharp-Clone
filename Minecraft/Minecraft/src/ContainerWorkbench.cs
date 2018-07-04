namespace net.minecraft.src
{
	public class ContainerWorkbench : Container
	{
		/// <summary>
		/// The crafting matrix inventory (3x3). </summary>
		public InventoryCrafting CraftMatrix;
		public IInventory CraftResult;
		private World WorldObj;
		private int PosX;
		private int PosY;
		private int PosZ;

		public ContainerWorkbench(InventoryPlayer par1InventoryPlayer, World par2World, int par3, int par4, int par5)
		{
			CraftMatrix = new InventoryCrafting(this, 3, 3);
			CraftResult = new InventoryCraftResult();
			WorldObj = par2World;
			PosX = par3;
			PosY = par4;
			PosZ = par5;
			AddSlot(new SlotCrafting(par1InventoryPlayer.Player, CraftMatrix, CraftResult, 0, 124, 35));

			for (int i = 0; i < 3; i++)
			{
				for (int l = 0; l < 3; l++)
				{
					AddSlot(new Slot(CraftMatrix, l + i * 3, 30 + l * 18, 17 + i * 18));
				}
			}

			for (int j = 0; j < 3; j++)
			{
				for (int i1 = 0; i1 < 9; i1++)
				{
					AddSlot(new Slot(par1InventoryPlayer, i1 + j * 9 + 9, 8 + i1 * 18, 84 + j * 18));
				}
			}

			for (int k = 0; k < 9; k++)
			{
				AddSlot(new Slot(par1InventoryPlayer, k, 8 + k * 18, 142));
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

			if (WorldObj.IsRemote)
			{
				return;
			}

			for (int i = 0; i < 9; i++)
			{
				ItemStack itemstack = CraftMatrix.GetStackInSlotOnClosing(i);

				if (itemstack != null)
				{
					par1EntityPlayer.DropPlayerItem(itemstack);
				}
			}
		}

		public override bool CanInteractWith(EntityPlayer par1EntityPlayer)
		{
			if (WorldObj.GetBlockId(PosX, PosY, PosZ) != Block.Workbench.BlockID)
			{
				return false;
			}

			return par1EntityPlayer.GetDistanceSq(PosX + 0.5F, PosY + 0.5F, PosZ + 0.5F) <= 64;
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
					if (!MergeItemStack(itemstack1, 10, 46, true))
					{
						return null;
					}

					slot.Func_48433_a(itemstack1, itemstack);
				}
				else if (par1 >= 10 && par1 < 37)
				{
					if (!MergeItemStack(itemstack1, 37, 46, false))
					{
						return null;
					}
				}
				else if (par1 >= 37 && par1 < 46)
				{
					if (!MergeItemStack(itemstack1, 10, 37, false))
					{
						return null;
					}
				}
				else if (!MergeItemStack(itemstack1, 10, 46, false))
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