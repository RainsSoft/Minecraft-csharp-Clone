using System.Collections.Generic;

namespace net.minecraft.src
{
	public class TileEntityBrewingStand : TileEntity, IInventory
	{
		private ItemStack[] BrewingItemStacks;
		private int BrewTime;

		/// <summary>
		/// an integer with each bit specifying whether that slot of the stand Contains a potion
		/// </summary>
		private int FilledSlots;
		private int IngredientID;

		public TileEntityBrewingStand()
		{
			BrewingItemStacks = new ItemStack[4];
		}

		/// <summary>
		/// Returns the name of the inventory.
		/// </summary>
		public virtual string GetInvName()
		{
			return "container.brewing";
		}

		/// <summary>
		/// Returns the number of slots in the inventory.
		/// </summary>
		public virtual int GetSizeInventory()
		{
			return BrewingItemStacks.Length;
		}

		/// <summary>
		/// Allows the entity to update its state. Overridden in most subclasses, e.g. the mob spawner uses this to count
		/// ticks and creates a new spawn inside its implementation.
		/// </summary>
		public override void UpdateEntity()
		{
			if (BrewTime > 0)
			{
				BrewTime--;

				if (BrewTime == 0)
				{
					BrewPotions();
					OnInventoryChanged();
				}
				else if (!CanBrew())
				{
					BrewTime = 0;
					OnInventoryChanged();
				}
				else if (IngredientID != BrewingItemStacks[3].ItemID)
				{
					BrewTime = 0;
					OnInventoryChanged();
				}
			}
			else if (CanBrew())
			{
				BrewTime = 400;
				IngredientID = BrewingItemStacks[3].ItemID;
			}

			int i = GetFilledSlots();

			if (i != FilledSlots)
			{
				FilledSlots = i;
				WorldObj.SetBlockMetadataWithNotify(XCoord, YCoord, ZCoord, i);
			}

			base.UpdateEntity();
		}

		public virtual int GetBrewTime()
		{
			return BrewTime;
		}

		private bool CanBrew()
		{
			if (BrewingItemStacks[3] == null || BrewingItemStacks[3].StackSize <= 0)
			{
				return false;
			}

			ItemStack itemstack = BrewingItemStacks[3];

			if (!Item.ItemsList[itemstack.ItemID].IsPotionIngredient())
			{
				return false;
			}

			bool flag = false;

			for (int i = 0; i < 3; i++)
			{
				if (BrewingItemStacks[i] == null || BrewingItemStacks[i].ItemID != Item.Potion.ShiftedIndex)
				{
					continue;
				}

				int j = BrewingItemStacks[i].GetItemDamage();
				int k = GetPotionResult(j, itemstack);

				if (!ItemPotion.IsSplash(j) && ItemPotion.IsSplash(k))
				{
					flag = true;
					break;
				}

				List<PotionEffect> list = Item.Potion.GetEffects(j);
				List<PotionEffect> list1 = Item.Potion.GetEffects(k);

				if (j > 0 && list == list1 || list != null && (list.Equals(list1) || list1 == null) || j == k)
				{
					continue;
				}

				flag = true;
				break;
			}

			return flag;
		}

		private void BrewPotions()
		{
			if (!CanBrew())
			{
				return;
			}

			ItemStack itemstack = BrewingItemStacks[3];

			for (int i = 0; i < 3; i++)
			{
				if (BrewingItemStacks[i] == null || BrewingItemStacks[i].ItemID != Item.Potion.ShiftedIndex)
				{
					continue;
				}

				int j = BrewingItemStacks[i].GetItemDamage();
				int k = GetPotionResult(j, itemstack);
				List<PotionEffect> list = Item.Potion.GetEffects(j);
				List<PotionEffect> list1 = Item.Potion.GetEffects(k);

				if (j > 0 && list == list1 || list != null && (list.Equals(list1) || list1 == null))
				{
					if (!ItemPotion.IsSplash(j) && ItemPotion.IsSplash(k))
					{
						BrewingItemStacks[i].SetItemDamage(k);
					}

					continue;
				}

				if (j != k)
				{
					BrewingItemStacks[i].SetItemDamage(k);
				}
			}

			if (Item.ItemsList[itemstack.ItemID].HasContainerItem())
			{
				BrewingItemStacks[3] = new ItemStack(Item.ItemsList[itemstack.ItemID].GetContainerItem());
			}
			else
			{
				BrewingItemStacks[3].StackSize--;

				if (BrewingItemStacks[3].StackSize <= 0)
				{
					BrewingItemStacks[3] = null;
				}
			}
		}

		/// <summary>
		/// The result of brewing a potion of the specified damage value with an ingredient itemstack.
		/// </summary>
		private int GetPotionResult(int par1, ItemStack par2ItemStack)
		{
			if (par2ItemStack == null)
			{
				return par1;
			}

			if (Item.ItemsList[par2ItemStack.ItemID].IsPotionIngredient())
			{
				return PotionHelper.ApplyIngredient(par1, Item.ItemsList[par2ItemStack.ItemID].GetPotionEffect());
			}
			else
			{
				return par1;
			}
		}

		/// <summary>
		/// Reads a tile entity from NBT.
		/// </summary>
		public override void ReadFromNBT(NBTTagCompound par1NBTTagCompound)
		{
			base.ReadFromNBT(par1NBTTagCompound);
			NBTTagList nbttaglist = par1NBTTagCompound.GetTagList("Items");
			BrewingItemStacks = new ItemStack[GetSizeInventory()];

			for (int i = 0; i < nbttaglist.TagCount(); i++)
			{
				NBTTagCompound nbttagcompound = (NBTTagCompound)nbttaglist.TagAt(i);
				byte byte0 = nbttagcompound.GetByte("Slot");

				if (byte0 >= 0 && byte0 < BrewingItemStacks.Length)
				{
					BrewingItemStacks[byte0] = ItemStack.LoadItemStackFromNBT(nbttagcompound);
				}
			}

			BrewTime = par1NBTTagCompound.GetShort("BrewTime");
		}

		/// <summary>
		/// Writes a tile entity to NBT.
		/// </summary>
		public override void WriteToNBT(NBTTagCompound par1NBTTagCompound)
		{
			base.WriteToNBT(par1NBTTagCompound);
			par1NBTTagCompound.SetShort("BrewTime", (short)BrewTime);
			NBTTagList nbttaglist = new NBTTagList();

			for (int i = 0; i < BrewingItemStacks.Length; i++)
			{
				if (BrewingItemStacks[i] != null)
				{
					NBTTagCompound nbttagcompound = new NBTTagCompound();
					nbttagcompound.SetByte("Slot", (byte)i);
					BrewingItemStacks[i].WriteToNBT(nbttagcompound);
					nbttaglist.AppendTag(nbttagcompound);
				}
			}

			par1NBTTagCompound.SetTag("Items", nbttaglist);
		}

		/// <summary>
		/// Returns the stack in slot i
		/// </summary>
		public virtual ItemStack GetStackInSlot(int par1)
		{
			if (par1 >= 0 && par1 < BrewingItemStacks.Length)
			{
				return BrewingItemStacks[par1];
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Decrease the size of the stack in slot (first int arg) by the amount of the second int arg. Returns the new
		/// stack.
		/// </summary>
		public virtual ItemStack DecrStackSize(int par1, int par2)
		{
			if (par1 >= 0 && par1 < BrewingItemStacks.Length)
			{
				ItemStack itemstack = BrewingItemStacks[par1];
				BrewingItemStacks[par1] = null;
				return itemstack;
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
			if (par1 >= 0 && par1 < BrewingItemStacks.Length)
			{
				ItemStack itemstack = BrewingItemStacks[par1];
				BrewingItemStacks[par1] = null;
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
			if (par1 >= 0 && par1 < BrewingItemStacks.Length)
			{
				BrewingItemStacks[par1] = par2ItemStack;
			}
		}

		/// <summary>
		/// Returns the maximum stack size for a inventory slot. Seems to always be 64, possibly will be extended. *Isn't
		/// this more of a set than a get?*
		/// </summary>
		public virtual int GetInventoryStackLimit()
		{
			return 1;
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

		public virtual void SetBrewTime(int par1)
		{
			BrewTime = par1;
		}

		/// <summary>
		/// returns an integer with each bit specifying wether that slot of the stand Contains a potion
		/// </summary>
		public virtual int GetFilledSlots()
		{
			int i = 0;

			for (int j = 0; j < 3; j++)
			{
				if (BrewingItemStacks[j] != null)
				{
					i |= 1 << j;
				}
			}

			return i;
		}
	}
}