namespace net.minecraft.src
{
	public class InventoryPlayer : IInventory
	{
		public ItemStack[] MainInventory;
		public ItemStack[] ArmorInventory;

		/// <summary>
		/// The index of the currently held item (0-8). </summary>
		public int CurrentItem;

		/// <summary>
		/// The player whose inventory this is. </summary>
		public EntityPlayer Player;
		private ItemStack ItemStack;

		/// <summary>
		/// Set true whenever the inventory changes. Nothing sets it false so you will have to write your own code to check
		/// it and reset the value.
		/// </summary>
		public bool InventoryChanged;

		public InventoryPlayer(EntityPlayer par1EntityPlayer)
		{
			MainInventory = new ItemStack[36];
			ArmorInventory = new ItemStack[4];
			CurrentItem = 0;
			InventoryChanged = false;
			Player = par1EntityPlayer;
		}

		/// <summary>
		/// Returns the item stack currently held by the player.
		/// </summary>
		public virtual ItemStack GetCurrentItem()
		{
			if (CurrentItem < 9 && CurrentItem >= 0)
			{
				return MainInventory[CurrentItem];
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Returns a slot index in main inventory containing a specific itemID
		/// </summary>
		private int GetInventorySlotContainItem(int par1)
		{
			for (int i = 0; i < MainInventory.Length; i++)
			{
				if (MainInventory[i] != null && MainInventory[i].ItemID == par1)
				{
					return i;
				}
			}

			return -1;
		}

		private int GetInventorySlotContainItemAndDamage(int par1, int par2)
		{
			for (int i = 0; i < MainInventory.Length; i++)
			{
				if (MainInventory[i] != null && MainInventory[i].ItemID == par1 && MainInventory[i].GetItemDamage() == par2)
				{
					return i;
				}
			}

			return -1;
		}

		/// <summary>
		/// stores an itemstack in the users inventory
		/// </summary>
		private int StoreItemStack(ItemStack par1ItemStack)
		{
			for (int i = 0; i < MainInventory.Length; i++)
			{
				if (MainInventory[i] != null && MainInventory[i].ItemID == par1ItemStack.ItemID && MainInventory[i].IsStackable() && MainInventory[i].StackSize < MainInventory[i].GetMaxStackSize() && MainInventory[i].StackSize < GetInventoryStackLimit() && (!MainInventory[i].GetHasSubtypes() || MainInventory[i].GetItemDamage() == par1ItemStack.GetItemDamage()) && ItemStack.Func_46154_a(MainInventory[i], par1ItemStack))
				{
					return i;
				}
			}

			return -1;
		}

		/// <summary>
		/// Returns the first item stack that is empty.
		/// </summary>
		private int GetFirstEmptyStack()
		{
			for (int i = 0; i < MainInventory.Length; i++)
			{
				if (MainInventory[i] == null)
				{
					return i;
				}
			}

			return -1;
		}

		/// <summary>
		/// Sets a specific itemID as the current item being held (only if it exists on the hotbar)
		/// </summary>
		public virtual void SetCurrentItem(int par1, int par2, bool par3, bool par4)
		{
			int i = -1;

			if (par3)
			{
				i = GetInventorySlotContainItemAndDamage(par1, par2);
			}
			else
			{
				i = GetInventorySlotContainItem(par1);
			}

			if (i >= 0 && i < 9)
			{
				CurrentItem = i;
				return;
			}

			if (par4 && par1 > 0)
			{
				int j = GetFirstEmptyStack();

				if (j >= 0 && j < 9)
				{
					CurrentItem = j;
				}

				Func_52006_a(Item.ItemsList[par1], par2);
			}
		}

		/// <summary>
		/// Switch the current item to the next one or the previous one
		/// </summary>
		public virtual void ChangeCurrentItem(int par1)
		{
			if (par1 > 0)
			{
				par1 = 1;
			}

			if (par1 < 0)
			{
				par1 = -1;
			}

			for (CurrentItem -= par1; CurrentItem < 0; CurrentItem += 9)
			{
			}

			for (; CurrentItem >= 9; CurrentItem -= 9)
			{
			}
		}

		public virtual void Func_52006_a(Item par1Item, int par2)
		{
			if (par1Item != null)
			{
				int i = GetInventorySlotContainItemAndDamage(par1Item.ShiftedIndex, par2);

				if (i >= 0)
				{
					MainInventory[i] = MainInventory[CurrentItem];
				}

				MainInventory[CurrentItem] = new ItemStack(Item.ItemsList[par1Item.ShiftedIndex], 1, par2);
			}
		}

		/// <summary>
		/// This function stores as many items of an ItemStack as possible in a matching slot and returns the quantity of
		/// left over items.
		/// </summary>
		private int StorePartialItemStack(ItemStack par1ItemStack)
		{
			int i = par1ItemStack.ItemID;
			int j = par1ItemStack.StackSize;

			if (par1ItemStack.GetMaxStackSize() == 1)
			{
				int k = GetFirstEmptyStack();

				if (k < 0)
				{
					return j;
				}

				if (MainInventory[k] == null)
				{
					MainInventory[k] = ItemStack.CopyItemStack(par1ItemStack);
				}

				return 0;
			}

			int l = StoreItemStack(par1ItemStack);

			if (l < 0)
			{
				l = GetFirstEmptyStack();
			}

			if (l < 0)
			{
				return j;
			}

			if (MainInventory[l] == null)
			{
				MainInventory[l] = new ItemStack(i, 0, par1ItemStack.GetItemDamage());

				if (par1ItemStack.HasTagCompound())
				{
					MainInventory[l].SetTagCompound((NBTTagCompound)par1ItemStack.GetTagCompound().Copy());
				}
			}

			int i1 = j;

			if (i1 > MainInventory[l].GetMaxStackSize() - MainInventory[l].StackSize)
			{
				i1 = MainInventory[l].GetMaxStackSize() - MainInventory[l].StackSize;
			}

			if (i1 > GetInventoryStackLimit() - MainInventory[l].StackSize)
			{
				i1 = GetInventoryStackLimit() - MainInventory[l].StackSize;
			}

			if (i1 == 0)
			{
				return j;
			}
			else
			{
				j -= i1;
				MainInventory[l].StackSize += i1;
				MainInventory[l].AnimationsToGo = 5;
				return j;
			}
		}

		/// <summary>
		/// Decrement the number of animations remaining. Only called on client side. This is used to handle the animation of
		/// receiving a block.
		/// </summary>
		public virtual void DecrementAnimations()
		{
			for (int i = 0; i < MainInventory.Length; i++)
			{
				if (MainInventory[i] != null)
				{
					MainInventory[i].UpdateAnimation(Player.WorldObj, Player, i, CurrentItem == i);
				}
			}
		}

		/// <summary>
		/// removed one item of specified itemID from inventory (if it is in a stack, the stack size will reduce with 1)
		/// </summary>
		public virtual bool ConsumeInventoryItem(int par1)
		{
			int i = GetInventorySlotContainItem(par1);

			if (i < 0)
			{
				return false;
			}

			if (--MainInventory[i].StackSize <= 0)
			{
				MainInventory[i] = null;
			}

			return true;
		}

		/// <summary>
		/// Get if a specifiied item id is inside the inventory.
		/// </summary>
		public virtual bool HasItem(int par1)
		{
			int i = GetInventorySlotContainItem(par1);
			return i >= 0;
		}

		/// <summary>
		/// Adds the item stack to the inventory, returns false if it is impossible.
		/// </summary>
		public virtual bool AddItemStackToInventory(ItemStack par1ItemStack)
		{
			if (!par1ItemStack.IsItemDamaged())
			{
				int i;

				do
				{
					i = par1ItemStack.StackSize;
					par1ItemStack.StackSize = StorePartialItemStack(par1ItemStack);
				}
				while (par1ItemStack.StackSize > 0 && par1ItemStack.StackSize < i);

				if (par1ItemStack.StackSize == i && Player.Capabilities.IsCreativeMode)
				{
					par1ItemStack.StackSize = 0;
					return true;
				}
				else
				{
					return par1ItemStack.StackSize < i;
				}
			}

			int j = GetFirstEmptyStack();

			if (j >= 0)
			{
				MainInventory[j] = ItemStack.CopyItemStack(par1ItemStack);
				MainInventory[j].AnimationsToGo = 5;
				par1ItemStack.StackSize = 0;
				return true;
			}

			if (Player.Capabilities.IsCreativeMode)
			{
				par1ItemStack.StackSize = 0;
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Decrease the size of the stack in slot (first int arg) by the amount of the second int arg. Returns the new
		/// stack.
		/// </summary>
		public virtual ItemStack DecrStackSize(int par1, int par2)
		{
			ItemStack[] aitemstack = MainInventory;

			if (par1 >= MainInventory.Length)
			{
				aitemstack = ArmorInventory;
				par1 -= MainInventory.Length;
			}

			if (aitemstack[par1] != null)
			{
				if (aitemstack[par1].StackSize <= par2)
				{
					ItemStack itemstack = aitemstack[par1];
					aitemstack[par1] = null;
					return itemstack;
				}

				ItemStack itemstack1 = aitemstack[par1].SplitStack(par2);

				if (aitemstack[par1].StackSize == 0)
				{
					aitemstack[par1] = null;
				}

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
			ItemStack[] aitemstack = MainInventory;

			if (par1 >= MainInventory.Length)
			{
				aitemstack = ArmorInventory;
				par1 -= MainInventory.Length;
			}

			if (aitemstack[par1] != null)
			{
				ItemStack itemstack = aitemstack[par1];
				aitemstack[par1] = null;
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
			ItemStack[] aitemstack = MainInventory;

			if (par1 >= aitemstack.Length)
			{
				par1 -= aitemstack.Length;
				aitemstack = ArmorInventory;
			}

			aitemstack[par1] = par2ItemStack;
		}

		/// <summary>
		/// Gets the strength of the current item (tool) against the specified block, 1.0f if not holding anything.
		/// </summary>
		public virtual float GetStrVsBlock(Block par1Block)
		{
			float f = 1.0F;

			if (MainInventory[CurrentItem] != null)
			{
				f *= MainInventory[CurrentItem].GetStrVsBlock(par1Block);
			}

			return f;
		}

		/// <summary>
		/// Writes the inventory out as a list of compound tags. This is where the slot indices are used (+100 for armor, +80
		/// for crafting).
		/// </summary>
		public virtual NBTTagList WriteToNBT(NBTTagList par1NBTTagList)
		{
			for (int i = 0; i < MainInventory.Length; i++)
			{
				if (MainInventory[i] != null)
				{
					NBTTagCompound nbttagcompound = new NBTTagCompound();
					nbttagcompound.SetByte("Slot", (byte)i);
					MainInventory[i].WriteToNBT(nbttagcompound);
					par1NBTTagList.AppendTag(nbttagcompound);
				}
			}

			for (int j = 0; j < ArmorInventory.Length; j++)
			{
				if (ArmorInventory[j] != null)
				{
					NBTTagCompound nbttagcompound1 = new NBTTagCompound();
					nbttagcompound1.SetByte("Slot", (byte)(j + 100));
					ArmorInventory[j].WriteToNBT(nbttagcompound1);
					par1NBTTagList.AppendTag(nbttagcompound1);
				}
			}

			return par1NBTTagList;
		}

		/// <summary>
		/// Reads from the given tag list and fills the slots in the inventory with the correct items.
		/// </summary>
		public virtual void ReadFromNBT(NBTTagList par1NBTTagList)
		{
			MainInventory = new ItemStack[36];
			ArmorInventory = new ItemStack[4];

			for (int i = 0; i < par1NBTTagList.TagCount(); i++)
			{
				NBTTagCompound nbttagcompound = (NBTTagCompound)par1NBTTagList.TagAt(i);
				int j = nbttagcompound.GetByte("Slot") & 0xff;
				ItemStack itemstack = ItemStack.LoadItemStackFromNBT(nbttagcompound);

				if (itemstack == null)
				{
					continue;
				}

				if (j >= 0 && j < MainInventory.Length)
				{
					MainInventory[j] = itemstack;
				}

				if (j >= 100 && j < ArmorInventory.Length + 100)
				{
					ArmorInventory[j - 100] = itemstack;
				}
			}
		}

		/// <summary>
		/// Returns the number of slots in the inventory.
		/// </summary>
		public virtual int GetSizeInventory()
		{
			return MainInventory.Length + 4;
		}

		/// <summary>
		/// Returns the stack in slot i
		/// </summary>
		public virtual ItemStack GetStackInSlot(int par1)
		{
			ItemStack[] aitemstack = MainInventory;

			if (par1 >= aitemstack.Length)
			{
				par1 -= aitemstack.Length;
				aitemstack = ArmorInventory;
			}

			return aitemstack[par1];
		}

		/// <summary>
		/// Returns the name of the inventory.
		/// </summary>
		public virtual string GetInvName()
		{
			return "container.Inventory";
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
		/// Return damage vs an entity done by the current held weapon, or 1 if nothing is held
		/// </summary>
		public virtual int GetDamageVsEntity(Entity par1Entity)
		{
			ItemStack itemstack = GetStackInSlot(CurrentItem);

			if (itemstack != null)
			{
				return itemstack.GetDamageVsEntity(par1Entity);
			}
			else
			{
				return 1;
			}
		}

		/// <summary>
		/// Returns whether the current item (tool) can harvest from the specified block (actually get a result).
		/// </summary>
		public virtual bool CanHarvestBlock(Block par1Block)
		{
			if (par1Block.BlockMaterial.IsHarvestable())
			{
				return true;
			}

			ItemStack itemstack = GetStackInSlot(CurrentItem);

			if (itemstack != null)
			{
				return itemstack.CanHarvestBlock(par1Block);
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// returns a player armor item (as itemstack) contained in specified armor slot.
		/// </summary>
		public virtual ItemStack ArmorItemInSlot(int par1)
		{
			return ArmorInventory[par1];
		}

		/// <summary>
		/// Based on the damage values and maximum damage values of each armor item, returns the current armor value.
		/// </summary>
		public virtual int GetTotalArmorValue()
		{
			int i = 0;

			for (int j = 0; j < ArmorInventory.Length; j++)
			{
				if (ArmorInventory[j] != null && (ArmorInventory[j].GetItem() is ItemArmor))
				{
					int k = ((ItemArmor)ArmorInventory[j].GetItem()).DamageReduceAmount;
					i += k;
				}
			}

			return i;
		}

		/// <summary>
		/// Damages armor in each slot by the specified amount.
		/// </summary>
		public virtual void DamageArmor(int par1)
		{
			par1 /= 4;

			if (par1 < 1)
			{
				par1 = 1;
			}

			for (int i = 0; i < ArmorInventory.Length; i++)
			{
				if (ArmorInventory[i] == null || !(ArmorInventory[i].GetItem() is ItemArmor))
				{
					continue;
				}

				ArmorInventory[i].DamageItem(par1, Player);

				if (ArmorInventory[i].StackSize == 0)
				{
					ArmorInventory[i].OnItemDestroyedByUse(Player);
					ArmorInventory[i] = null;
				}
			}
		}

		/// <summary>
		/// Drop all armor and main inventory items.
		/// </summary>
		public virtual void DropAllItems()
		{
			for (int i = 0; i < MainInventory.Length; i++)
			{
				if (MainInventory[i] != null)
				{
					Player.DropPlayerItemWithRandomChoice(MainInventory[i], true);
					MainInventory[i] = null;
				}
			}

			for (int j = 0; j < ArmorInventory.Length; j++)
			{
				if (ArmorInventory[j] != null)
				{
					Player.DropPlayerItemWithRandomChoice(ArmorInventory[j], true);
					ArmorInventory[j] = null;
				}
			}
		}

		/// <summary>
		/// Called when an the contents of an Inventory change, usually
		/// </summary>
		public virtual void OnInventoryChanged()
		{
			InventoryChanged = true;
		}

		public virtual void SetItemStack(ItemStack par1ItemStack)
		{
			ItemStack = par1ItemStack;
			Player.OnItemStackChanged(par1ItemStack);
		}

		public virtual ItemStack GetItemStack()
		{
			return ItemStack;
		}

		/// <summary>
		/// Do not make give this method the name canInteractWith because it clashes with Container
		/// </summary>
		public virtual bool IsUseableByPlayer(EntityPlayer par1EntityPlayer)
		{
			if (Player.IsDead)
			{
				return false;
			}

			return par1EntityPlayer.GetDistanceSqToEntity(Player) <= 64D;
		}

		/// <summary>
		/// Returns true if the specified ItemStack exists in the inventory.
		/// </summary>
		public virtual bool HasItemStack(ItemStack par1ItemStack)
		{
			for (int i = 0; i < ArmorInventory.Length; i++)
			{
				if (ArmorInventory[i] != null && ArmorInventory[i].IsStackEqual(par1ItemStack))
				{
					return true;
				}
			}

			for (int j = 0; j < MainInventory.Length; j++)
			{
				if (MainInventory[j] != null && MainInventory[j].IsStackEqual(par1ItemStack))
				{
					return true;
				}
			}

			return false;
		}

		public virtual void OpenChest()
		{
		}

		public virtual void CloseChest()
		{
		}

		/// <summary>
		/// Copy the ItemStack contents from another InventoryPlayer instance
		/// </summary>
		public virtual void CopyInventory(InventoryPlayer par1InventoryPlayer)
		{
			for (int i = 0; i < MainInventory.Length; i++)
			{
				MainInventory[i] = ItemStack.CopyItemStack(par1InventoryPlayer.MainInventory[i]);
			}

			for (int j = 0; j < ArmorInventory.Length; j++)
			{
				ArmorInventory[j] = ItemStack.CopyItemStack(par1InventoryPlayer.ArmorInventory[j]);
			}
		}
	}
}