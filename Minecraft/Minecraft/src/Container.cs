using System.Collections.Generic;

namespace net.minecraft.src
{
	public abstract class Container
	{
		/// <summary>
		/// the list of all items(stacks) for the corresponding slot </summary>
		public List<ItemStack> InventoryItemStacks;

		/// <summary>
		/// the list of all slots in the inventory </summary>
		public List<Slot> InventorySlots;
		public int WindowId;
		private short TransactionID;

		/// <summary>
		/// list of all people that need to be notified when this craftinventory changes
		/// </summary>
		protected List<ICrafting> Crafters;
		//private Set Field_20918_b;

		public Container()
		{
            InventoryItemStacks = new List<ItemStack>();
            InventorySlots = new List<Slot>();
			WindowId = 0;
			TransactionID = 0;
            Crafters = new List<ICrafting>();
			//Field_20918_b = new HashSet();
		}

		/// <summary>
		/// adds the slot to the inventory it is in
		/// </summary>
		protected virtual void AddSlot(Slot par1Slot)
		{
			par1Slot.SlotNumber = InventorySlots.Count;
			InventorySlots.Add(par1Slot);
			InventoryItemStacks.Add(null);
		}

		/// <summary>
		/// Updates crafting matrix; called from onCraftMatrixChanged. Args: none
		/// </summary>
		public virtual void UpdateCraftingResults()
		{
			for (int i = 0; i < InventorySlots.Count; i++)
			{
				ItemStack itemstack = InventorySlots[i].GetStack();
				ItemStack itemstack1 = InventoryItemStacks[i];

				if (ItemStack.AreItemStacksEqual(itemstack1, itemstack))
				{
					continue;
				}

				itemstack1 = itemstack != null ? itemstack.Copy() : null;
				InventoryItemStacks[i] = itemstack1;

				for (int j = 0; j < Crafters.Count; j++)
				{
					Crafters[j].UpdateCraftingInventorySlot(this, i, itemstack1);
				}
			}
		}

		/// <summary>
		/// enchants the item on the table using the specified slot; also deducts XP from player
		/// </summary>
		public virtual bool EnchantItem(EntityPlayer par1EntityPlayer, int par2)
		{
			return false;
		}

		public virtual Slot GetSlot(int par1)
		{
			return InventorySlots[par1];
		}

		/// <summary>
		/// Called to transfer a stack from one inventory to the other eg. when shift clicking.
		/// </summary>
		public virtual ItemStack TransferStackInSlot(int par1)
		{
			Slot slot = InventorySlots[par1];

			if (slot != null)
			{
				return slot.GetStack();
			}
			else
			{
				return null;
			}
		}

		public virtual ItemStack SlotClick(int par1, int par2, bool par3, EntityPlayer par4EntityPlayer)
		{
			ItemStack itemstack = null;

			if (par2 > 1)
			{
				return null;
			}

			if (par2 == 0 || par2 == 1)
			{
				InventoryPlayer inventoryplayer = par4EntityPlayer.Inventory;

				if (par1 == -999)
				{
					if (inventoryplayer.GetItemStack() != null && par1 == -999)
					{
						if (par2 == 0)
						{
							par4EntityPlayer.DropPlayerItem(inventoryplayer.GetItemStack());
							inventoryplayer.SetItemStack(null);
						}

						if (par2 == 1)
						{
							par4EntityPlayer.DropPlayerItem(inventoryplayer.GetItemStack().SplitStack(1));

							if (inventoryplayer.GetItemStack().StackSize == 0)
							{
								inventoryplayer.SetItemStack(null);
							}
						}
					}
				}
				else if (par3)
				{
					ItemStack itemstack1 = TransferStackInSlot(par1);

					if (itemstack1 != null)
					{
						int i = itemstack1.ItemID;
						itemstack = itemstack1.Copy();
						Slot slot1 = InventorySlots[par1];

						if (slot1 != null && slot1.GetStack() != null && slot1.GetStack().ItemID == i)
						{
							RetrySlotClick(par1, par2, par3, par4EntityPlayer);
						}
					}
				}
				else
				{
					if (par1 < 0)
					{
						return null;
					}

					Slot slot = InventorySlots[par1];

					if (slot != null)
					{
						slot.OnSlotChanged();
						ItemStack itemstack2 = slot.GetStack();
						ItemStack itemstack4 = inventoryplayer.GetItemStack();

						if (itemstack2 != null)
						{
							itemstack = itemstack2.Copy();
						}

						if (itemstack2 == null)
						{
							if (itemstack4 != null && slot.IsItemValid(itemstack4))
							{
								int j = par2 != 0 ? 1 : itemstack4.StackSize;

								if (j > slot.GetSlotStackLimit())
								{
									j = slot.GetSlotStackLimit();
								}

								slot.PutStack(itemstack4.SplitStack(j));

								if (itemstack4.StackSize == 0)
								{
									inventoryplayer.SetItemStack(null);
								}
							}
						}
						else if (itemstack4 == null)
						{
							int k = par2 != 0 ? (itemstack2.StackSize + 1) / 2 : itemstack2.StackSize;
							ItemStack itemstack6 = slot.DecrStackSize(k);
							inventoryplayer.SetItemStack(itemstack6);

							if (itemstack2.StackSize == 0)
							{
								slot.PutStack(null);
							}

							slot.OnPickupFromSlot(inventoryplayer.GetItemStack());
						}
						else if (slot.IsItemValid(itemstack4))
						{
							if (itemstack2.ItemID != itemstack4.ItemID || itemstack2.GetHasSubtypes() && itemstack2.GetItemDamage() != itemstack4.GetItemDamage() || !ItemStack.Func_46154_a(itemstack2, itemstack4))
							{
								if (itemstack4.StackSize <= slot.GetSlotStackLimit())
								{
									ItemStack itemstack5 = itemstack2;
									slot.PutStack(itemstack4);
									inventoryplayer.SetItemStack(itemstack5);
								}
							}
							else
							{
								int l = par2 != 0 ? 1 : itemstack4.StackSize;

								if (l > slot.GetSlotStackLimit() - itemstack2.StackSize)
								{
									l = slot.GetSlotStackLimit() - itemstack2.StackSize;
								}

								if (l > itemstack4.GetMaxStackSize() - itemstack2.StackSize)
								{
									l = itemstack4.GetMaxStackSize() - itemstack2.StackSize;
								}

								itemstack4.SplitStack(l);

								if (itemstack4.StackSize == 0)
								{
									inventoryplayer.SetItemStack(null);
								}

								itemstack2.StackSize += l;
							}
						}
						else if (itemstack2.ItemID == itemstack4.ItemID && itemstack4.GetMaxStackSize() > 1 && (!itemstack2.GetHasSubtypes() || itemstack2.GetItemDamage() == itemstack4.GetItemDamage()) && ItemStack.Func_46154_a(itemstack2, itemstack4))
						{
							int i1 = itemstack2.StackSize;

							if (i1 > 0 && i1 + itemstack4.StackSize <= itemstack4.GetMaxStackSize())
							{
								itemstack4.StackSize += i1;
								ItemStack itemstack3 = slot.DecrStackSize(i1);

								if (itemstack3.StackSize == 0)
								{
									slot.PutStack(null);
								}

								slot.OnPickupFromSlot(inventoryplayer.GetItemStack());
							}
						}
					}
				}
			}

			return itemstack;
		}

		protected virtual void RetrySlotClick(int par1, int par2, bool par3, EntityPlayer par4EntityPlayer)
		{
			SlotClick(par1, par2, par3, par4EntityPlayer);
		}

		/// <summary>
		/// Callback for when the crafting gui is closed.
		/// </summary>
		public virtual void OnCraftGuiClosed(EntityPlayer par1EntityPlayer)
		{
			InventoryPlayer inventoryplayer = par1EntityPlayer.Inventory;

			if (inventoryplayer.GetItemStack() != null)
			{
				par1EntityPlayer.DropPlayerItem(inventoryplayer.GetItemStack());
				inventoryplayer.SetItemStack(null);
			}
		}

		/// <summary>
		/// Callback for when the crafting matrix is changed.
		/// </summary>
		public virtual void OnCraftMatrixChanged(IInventory par1IInventory)
		{
			UpdateCraftingResults();
		}

		/// <summary>
		/// args: slotID, itemStack to put in slot
		/// </summary>
		public virtual void PutStackInSlot(int par1, ItemStack par2ItemStack)
		{
			GetSlot(par1).PutStack(par2ItemStack);
		}

		/// <summary>
		/// places itemstacks in first x slots, x being aitemstack.lenght
		/// </summary>
		public virtual void PutStacksInSlots(ItemStack[] par1ArrayOfItemStack)
		{
			for (int i = 0; i < par1ArrayOfItemStack.Length; i++)
			{
				GetSlot(i).PutStack(par1ArrayOfItemStack[i]);
			}
		}

		public virtual void UpdateProgressBar(int i, int j)
		{
		}

		/// <summary>
		/// Gets a unique transaction ID. Parameter is unused.
		/// </summary>
		public virtual short GetNextTransactionID(InventoryPlayer par1InventoryPlayer)
		{
			TransactionID++;
			return TransactionID;
		}

		public virtual void Func_20113_a(short word0)
		{
		}

		public virtual void Func_20110_b(short word0)
		{
		}

		public abstract bool CanInteractWith(EntityPlayer entityplayer);

		/// <summary>
		/// merges provided ItemStack with the first avaliable one in the container/player inventory
		/// </summary>
		protected virtual bool MergeItemStack(ItemStack par1ItemStack, int par2, int par3, bool par4)
		{
			bool flag = false;
			int i = par2;

			if (par4)
			{
				i = par3 - 1;
			}

			if (par1ItemStack.IsStackable())
			{
				while (par1ItemStack.StackSize > 0 && (!par4 && i < par3 || par4 && i >= par2))
				{
					Slot slot = InventorySlots[i];
					ItemStack itemstack = slot.GetStack();

					if (itemstack != null && itemstack.ItemID == par1ItemStack.ItemID && (!par1ItemStack.GetHasSubtypes() || par1ItemStack.GetItemDamage() == itemstack.GetItemDamage()) && ItemStack.Func_46154_a(par1ItemStack, itemstack))
					{
						int k = itemstack.StackSize + par1ItemStack.StackSize;

						if (k <= par1ItemStack.GetMaxStackSize())
						{
							par1ItemStack.StackSize = 0;
							itemstack.StackSize = k;
							slot.OnSlotChanged();
							flag = true;
						}
						else if (itemstack.StackSize < par1ItemStack.GetMaxStackSize())
						{
							par1ItemStack.StackSize -= par1ItemStack.GetMaxStackSize() - itemstack.StackSize;
							itemstack.StackSize = par1ItemStack.GetMaxStackSize();
							slot.OnSlotChanged();
							flag = true;
						}
					}

					if (par4)
					{
						i--;
					}
					else
					{
						i++;
					}
				}
			}

			if (par1ItemStack.StackSize > 0)
			{
				int j;

				if (par4)
				{
					j = par3 - 1;
				}
				else
				{
					j = par2;
				}

				do
				{
					if ((par4 || j >= par3) && (!par4 || j < par2))
					{
						break;
					}

					Slot slot1 = InventorySlots[j];
					ItemStack itemstack1 = slot1.GetStack();

					if (itemstack1 == null)
					{
						slot1.PutStack(par1ItemStack.Copy());
						slot1.OnSlotChanged();
						par1ItemStack.StackSize = 0;
						flag = true;
						break;
					}

					if (par4)
					{
						j--;
					}
					else
					{
						j++;
					}
				}
				while (true);
			}

			return flag;
		}
	}
}