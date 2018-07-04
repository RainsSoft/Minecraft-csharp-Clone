using System;

namespace net.minecraft.src
{
	public class TileEntityDispenser : TileEntity, IInventory
	{
		private ItemStack[] DispenserContents;

		/// <summary>
		/// random number generator for instance. Used in random item stack selection.
		/// </summary>
		private Random DispenserRandom;

		public TileEntityDispenser()
		{
			DispenserContents = new ItemStack[9];
			DispenserRandom = new Random();
		}

		/// <summary>
		/// Returns the number of slots in the inventory.
		/// </summary>
		public virtual int GetSizeInventory()
		{
			return 9;
		}

		/// <summary>
		/// Returns the stack in slot i
		/// </summary>
		public virtual ItemStack GetStackInSlot(int par1)
		{
			return DispenserContents[par1];
		}

		/// <summary>
		/// Decrease the size of the stack in slot (first int arg) by the amount of the second int arg. Returns the new
		/// stack.
		/// </summary>
		public virtual ItemStack DecrStackSize(int par1, int par2)
		{
			if (DispenserContents[par1] != null)
			{
				if (DispenserContents[par1].StackSize <= par2)
				{
					ItemStack itemstack = DispenserContents[par1];
					DispenserContents[par1] = null;
					OnInventoryChanged();
					return itemstack;
				}

				ItemStack itemstack1 = DispenserContents[par1].SplitStack(par2);

				if (DispenserContents[par1].StackSize == 0)
				{
					DispenserContents[par1] = null;
				}

				OnInventoryChanged();
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
			if (DispenserContents[par1] != null)
			{
				ItemStack itemstack = DispenserContents[par1];
				DispenserContents[par1] = null;
				return itemstack;
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// gets stack of one item extracted from a stack chosen at random from the block inventory
		/// </summary>
		public virtual ItemStack GetRandomStackFromInventory()
		{
			int i = -1;
			int j = 1;

			for (int k = 0; k < DispenserContents.Length; k++)
			{
				if (DispenserContents[k] != null && DispenserRandom.Next(j++) == 0)
				{
					i = k;
				}
			}

			if (i >= 0)
			{
				return DecrStackSize(i, 1);
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
			DispenserContents[par1] = par2ItemStack;

			if (par2ItemStack != null && par2ItemStack.StackSize > GetInventoryStackLimit())
			{
				par2ItemStack.StackSize = GetInventoryStackLimit();
			}

			OnInventoryChanged();
		}

		/// <summary>
		/// Returns the name of the inventory.
		/// </summary>
		public virtual string GetInvName()
		{
			return "container.dispenser";
		}

		/// <summary>
		/// Reads a tile entity from NBT.
		/// </summary>
		public override void ReadFromNBT(NBTTagCompound par1NBTTagCompound)
		{
			base.ReadFromNBT(par1NBTTagCompound);
			NBTTagList nbttaglist = par1NBTTagCompound.GetTagList("Items");
			DispenserContents = new ItemStack[GetSizeInventory()];

			for (int i = 0; i < nbttaglist.TagCount(); i++)
			{
				NBTTagCompound nbttagcompound = (NBTTagCompound)nbttaglist.TagAt(i);
				int j = nbttagcompound.GetByte("Slot") & 0xff;

				if (j >= 0 && j < DispenserContents.Length)
				{
					DispenserContents[j] = ItemStack.LoadItemStackFromNBT(nbttagcompound);
				}
			}
		}

		/// <summary>
		/// Writes a tile entity to NBT.
		/// </summary>
		public override void WriteToNBT(NBTTagCompound par1NBTTagCompound)
		{
			base.WriteToNBT(par1NBTTagCompound);
			NBTTagList nbttaglist = new NBTTagList();

			for (int i = 0; i < DispenserContents.Length; i++)
			{
				if (DispenserContents[i] != null)
				{
					NBTTagCompound nbttagcompound = new NBTTagCompound();
					nbttagcompound.SetByte("Slot", (byte)i);
					DispenserContents[i].WriteToNBT(nbttagcompound);
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