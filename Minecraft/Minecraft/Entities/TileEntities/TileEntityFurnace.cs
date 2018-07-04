namespace net.minecraft.src
{

	public class TileEntityFurnace : TileEntity, IInventory
	{
		private ItemStack[] FurnaceItemStacks;

		/// <summary>
		/// The number of ticks that the furnace will keep burning </summary>
		public int FurnaceBurnTime;

		/// <summary>
		/// The number of ticks that a fresh copy of the currently-burning item would keep the furnace burning for
		/// </summary>
		public int CurrentItemBurnTime;

		/// <summary>
		/// The number of ticks that the current item has been cooking for </summary>
		public int FurnaceCookTime;

		public TileEntityFurnace()
		{
			FurnaceItemStacks = new ItemStack[3];
			FurnaceBurnTime = 0;
			CurrentItemBurnTime = 0;
			FurnaceCookTime = 0;
		}

		/// <summary>
		/// Returns the number of slots in the inventory.
		/// </summary>
		public virtual int GetSizeInventory()
		{
			return FurnaceItemStacks.Length;
		}

		/// <summary>
		/// Returns the stack in slot i
		/// </summary>
		public virtual ItemStack GetStackInSlot(int par1)
		{
			return FurnaceItemStacks[par1];
		}

		/// <summary>
		/// Decrease the size of the stack in slot (first int arg) by the amount of the second int arg. Returns the new
		/// stack.
		/// </summary>
		public virtual ItemStack DecrStackSize(int par1, int par2)
		{
			if (FurnaceItemStacks[par1] != null)
			{
				if (FurnaceItemStacks[par1].StackSize <= par2)
				{
					ItemStack itemstack = FurnaceItemStacks[par1];
					FurnaceItemStacks[par1] = null;
					return itemstack;
				}

				ItemStack itemstack1 = FurnaceItemStacks[par1].SplitStack(par2);

				if (FurnaceItemStacks[par1].StackSize == 0)
				{
					FurnaceItemStacks[par1] = null;
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
			if (FurnaceItemStacks[par1] != null)
			{
				ItemStack itemstack = FurnaceItemStacks[par1];
				FurnaceItemStacks[par1] = null;
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
			FurnaceItemStacks[par1] = par2ItemStack;

			if (par2ItemStack != null && par2ItemStack.StackSize > GetInventoryStackLimit())
			{
				par2ItemStack.StackSize = GetInventoryStackLimit();
			}
		}

		/// <summary>
		/// Returns the name of the inventory.
		/// </summary>
		public virtual string GetInvName()
		{
			return "container.furnace";
		}

		/// <summary>
		/// Reads a tile entity from NBT.
		/// </summary>
		public override void ReadFromNBT(NBTTagCompound par1NBTTagCompound)
		{
			base.ReadFromNBT(par1NBTTagCompound);
			NBTTagList nbttaglist = par1NBTTagCompound.GetTagList("Items");
			FurnaceItemStacks = new ItemStack[GetSizeInventory()];

			for (int i = 0; i < nbttaglist.TagCount(); i++)
			{
				NBTTagCompound nbttagcompound = (NBTTagCompound)nbttaglist.TagAt(i);
				byte byte0 = nbttagcompound.GetByte("Slot");

				if (byte0 >= 0 && byte0 < FurnaceItemStacks.Length)
				{
					FurnaceItemStacks[byte0] = ItemStack.LoadItemStackFromNBT(nbttagcompound);
				}
			}

			FurnaceBurnTime = par1NBTTagCompound.GetShort("BurnTime");
			FurnaceCookTime = par1NBTTagCompound.GetShort("CookTime");
			CurrentItemBurnTime = GetItemBurnTime(FurnaceItemStacks[1]);
		}

		/// <summary>
		/// Writes a tile entity to NBT.
		/// </summary>
		public override void WriteToNBT(NBTTagCompound par1NBTTagCompound)
		{
			base.WriteToNBT(par1NBTTagCompound);
			par1NBTTagCompound.SetShort("BurnTime", (short)FurnaceBurnTime);
			par1NBTTagCompound.SetShort("CookTime", (short)FurnaceCookTime);
			NBTTagList nbttaglist = new NBTTagList();

			for (int i = 0; i < FurnaceItemStacks.Length; i++)
			{
				if (FurnaceItemStacks[i] != null)
				{
					NBTTagCompound nbttagcompound = new NBTTagCompound();
					nbttagcompound.SetByte("Slot", (byte)i);
					FurnaceItemStacks[i].WriteToNBT(nbttagcompound);
					nbttaglist.AppendTag(nbttagcompound);
				}
			}

			par1NBTTagCompound.SetTag("Items", nbttaglist);
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
		/// Returns an integer between 0 and the passed value representing how close the current item is to being completely
		/// cooked
		/// </summary>
		public virtual int GetCookProgressScaled(int par1)
		{
			return (FurnaceCookTime * par1) / 200;
		}

		/// <summary>
		/// Returns an integer between 0 and the passed value representing how much burn time is left on the current fuel
		/// item, where 0 means that the item is exhausted and the passed value means that the item is fresh
		/// </summary>
		public virtual int GetBurnTimeRemainingScaled(int par1)
		{
			if (CurrentItemBurnTime == 0)
			{
				CurrentItemBurnTime = 200;
			}

			return (FurnaceBurnTime * par1) / CurrentItemBurnTime;
		}

		/// <summary>
		/// Returns true if the furnace is currently burning
		/// </summary>
		public virtual bool IsBurning()
		{
			return FurnaceBurnTime > 0;
		}

		/// <summary>
		/// Allows the entity to update its state. Overridden in most subclasses, e.g. the mob spawner uses this to count
		/// ticks and creates a new spawn inside its implementation.
		/// </summary>
		public override void UpdateEntity()
		{
			bool flag = FurnaceBurnTime > 0;
			bool flag1 = false;

			if (FurnaceBurnTime > 0)
			{
				FurnaceBurnTime--;
			}

			if (!WorldObj.IsRemote)
			{
				if (FurnaceBurnTime == 0 && CanSmelt())
				{
					CurrentItemBurnTime = FurnaceBurnTime = GetItemBurnTime(FurnaceItemStacks[1]);

					if (FurnaceBurnTime > 0)
					{
						flag1 = true;

						if (FurnaceItemStacks[1] != null)
						{
							FurnaceItemStacks[1].StackSize--;

							if (FurnaceItemStacks[1].StackSize == 0)
							{
								FurnaceItemStacks[1] = null;
							}
						}
					}
				}

				if (IsBurning() && CanSmelt())
				{
					FurnaceCookTime++;

					if (FurnaceCookTime == 200)
					{
						FurnaceCookTime = 0;
						SmeltItem();
						flag1 = true;
					}
				}
				else
				{
					FurnaceCookTime = 0;
				}

				if (flag != (FurnaceBurnTime > 0))
				{
					flag1 = true;
					BlockFurnace.UpdateFurnaceBlockState(FurnaceBurnTime > 0, WorldObj, XCoord, YCoord, ZCoord);
				}
			}

			if (flag1)
			{
				OnInventoryChanged();
			}
		}

		/// <summary>
		/// Returns true if the furnace can smelt an item, i.e. has a source item, destination stack isn't full, etc.
		/// </summary>
		private bool CanSmelt()
		{
			if (FurnaceItemStacks[0] == null)
			{
				return false;
			}

			ItemStack itemstack = FurnaceRecipes.Smelting().GetSmeltingResult(FurnaceItemStacks[0].GetItem().ShiftedIndex);

			if (itemstack == null)
			{
				return false;
			}

			if (FurnaceItemStacks[2] == null)
			{
				return true;
			}

			if (!FurnaceItemStacks[2].IsItemEqual(itemstack))
			{
				return false;
			}

			if (FurnaceItemStacks[2].StackSize < GetInventoryStackLimit() && FurnaceItemStacks[2].StackSize < FurnaceItemStacks[2].GetMaxStackSize())
			{
				return true;
			}

			return FurnaceItemStacks[2].StackSize < itemstack.GetMaxStackSize();
		}

		/// <summary>
		/// Turn one item from the furnace source stack into the appropriate smelted item in the furnace result stack
		/// </summary>
		public virtual void SmeltItem()
		{
			if (!CanSmelt())
			{
				return;
			}

			ItemStack itemstack = FurnaceRecipes.Smelting().GetSmeltingResult(FurnaceItemStacks[0].GetItem().ShiftedIndex);

			if (FurnaceItemStacks[2] == null)
			{
				FurnaceItemStacks[2] = itemstack.Copy();
			}
			else if (FurnaceItemStacks[2].ItemID == itemstack.ItemID)
			{
				FurnaceItemStacks[2].StackSize++;
			}

			FurnaceItemStacks[0].StackSize--;

			if (FurnaceItemStacks[0].StackSize <= 0)
			{
				FurnaceItemStacks[0] = null;
			}
		}

		/// <summary>
		/// Returns the number of ticks that the supplied fuel item will keep the furnace burning, or 0 if the item isn't
		/// fuel
		/// </summary>
		public static int GetItemBurnTime(ItemStack par1ItemStack)
		{
			if (par1ItemStack == null)
			{
				return 0;
			}

			int i = par1ItemStack.GetItem().ShiftedIndex;

			if (i < 256 && Block.BlocksList[i].BlockMaterial == Material.Wood)
			{
				return 300;
			}

			if (i == Item.Stick.ShiftedIndex)
			{
				return 100;
			}

			if (i == Item.Coal.ShiftedIndex)
			{
				return 1600;
			}

			if (i == Item.BucketLava.ShiftedIndex)
			{
				return 20000;
			}

			if (i == Block.Sapling.BlockID)
			{
				return 100;
			}

			return i != Item.BlazeRod.ShiftedIndex ? 0 : 2400;
		}

		public static bool Func_52005_b(ItemStack par0ItemStack)
		{
			return GetItemBurnTime(par0ItemStack) > 0;
		}

		/// <summary>
		/// Do not make give this method the name canInteractWith because it clashes with Container
		/// </summary>
		public virtual bool IsUseableByPlayer(EntityPlayer par1EntityPlayer)
		{
			if (WorldObj.GetBlockTileEntity(XCoord, YCoord, ZCoord) != this)
			{
				return false;
			}

			return par1EntityPlayer.GetDistanceSq(XCoord + 0.5F, YCoord + 0.5F, ZCoord + 0.5F) <= 64;
		}

		public virtual void OpenChest()
		{
		}

		public virtual void CloseChest()
		{
		}
	}
}