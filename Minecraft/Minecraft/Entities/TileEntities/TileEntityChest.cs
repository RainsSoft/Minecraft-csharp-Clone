namespace net.minecraft.src
{
	public class TileEntityChest : TileEntity, IInventory
	{
		private ItemStack[] ChestContents;

		/// <summary>
		/// determines if the check for adjacent chests has taken place. </summary>
		public bool AdjacentChestChecked;

		/// <summary>
		/// Contains the chest tile located adjacent to this one (if any) </summary>
		public TileEntityChest AdjacentChestZNeg;

		/// <summary>
		/// Contains the chest tile located adjacent to this one (if any) </summary>
		public TileEntityChest AdjacentChestXPos;

		/// <summary>
		/// Contains the chest tile located adjacent to this one (if any) </summary>
		public TileEntityChest AdjacentChestXNeg;

		/// <summary>
		/// Contains the chest tile located adjacent to this one (if any) </summary>
		public TileEntityChest AdjacentChestZPos;

		/// <summary>
		/// the current angle of the lid (between 0 and 1) </summary>
		public float LidAngle;

		/// <summary>
		/// the angle of the lid last tick </summary>
		public float PrevLidAngle;

		/// <summary>
		/// the number of players currently using this chest </summary>
		public int NumUsingPlayers;

		/// <summary>
		/// server sync counter (once per 20 ticks) </summary>
		private int TicksSinceSync;

		public TileEntityChest()
		{
			ChestContents = new ItemStack[36];
			AdjacentChestChecked = false;
		}

		/// <summary>
		/// Returns the number of slots in the inventory.
		/// </summary>
		public virtual int GetSizeInventory()
		{
			return 27;
		}

		/// <summary>
		/// Returns the stack in slot i
		/// </summary>
		public virtual ItemStack GetStackInSlot(int par1)
		{
			return ChestContents[par1];
		}

		/// <summary>
		/// Decrease the size of the stack in slot (first int arg) by the amount of the second int arg. Returns the new
		/// stack.
		/// </summary>
		public virtual ItemStack DecrStackSize(int par1, int par2)
		{
			if (ChestContents[par1] != null)
			{
				if (ChestContents[par1].StackSize <= par2)
				{
					ItemStack itemstack = ChestContents[par1];
					ChestContents[par1] = null;
					OnInventoryChanged();
					return itemstack;
				}

				ItemStack itemstack1 = ChestContents[par1].SplitStack(par2);

				if (ChestContents[par1].StackSize == 0)
				{
					ChestContents[par1] = null;
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
			if (ChestContents[par1] != null)
			{
				ItemStack itemstack = ChestContents[par1];
				ChestContents[par1] = null;
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
			ChestContents[par1] = par2ItemStack;

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
			return "container.chest";
		}

		/// <summary>
		/// Reads a tile entity from NBT.
		/// </summary>
		public override void ReadFromNBT(NBTTagCompound par1NBTTagCompound)
		{
			base.ReadFromNBT(par1NBTTagCompound);
			NBTTagList nbttaglist = par1NBTTagCompound.GetTagList("Items");
			ChestContents = new ItemStack[GetSizeInventory()];

			for (int i = 0; i < nbttaglist.TagCount(); i++)
			{
				NBTTagCompound nbttagcompound = (NBTTagCompound)nbttaglist.TagAt(i);
				int j = nbttagcompound.GetByte("Slot") & 0xff;

				if (j >= 0 && j < ChestContents.Length)
				{
					ChestContents[j] = ItemStack.LoadItemStackFromNBT(nbttagcompound);
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

			for (int i = 0; i < ChestContents.Length; i++)
			{
				if (ChestContents[i] != null)
				{
					NBTTagCompound nbttagcompound = new NBTTagCompound();
					nbttagcompound.SetByte("Slot", (byte)i);
					ChestContents[i].WriteToNBT(nbttagcompound);
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

		/// <summary>
		/// causes the TileEntity to reset all it's cached values for it's container block, BlockID, metaData and in the case
		/// of chests, the adjcacent chest check
		/// </summary>
		public override void UpdateContainingBlockInfo()
		{
			base.UpdateContainingBlockInfo();
			AdjacentChestChecked = false;
		}

		/// <summary>
		/// performs the check for adjacent chests to determine if this chest is double or not.
		/// </summary>
		public virtual void CheckForAdjacentChests()
		{
			if (AdjacentChestChecked)
			{
				return;
			}

			AdjacentChestChecked = true;
			AdjacentChestZNeg = null;
			AdjacentChestXPos = null;
			AdjacentChestXNeg = null;
			AdjacentChestZPos = null;

			if (WorldObj.GetBlockId(XCoord - 1, YCoord, ZCoord) == Block.Chest.BlockID)
			{
				AdjacentChestXNeg = (TileEntityChest)WorldObj.GetBlockTileEntity(XCoord - 1, YCoord, ZCoord);
			}

			if (WorldObj.GetBlockId(XCoord + 1, YCoord, ZCoord) == Block.Chest.BlockID)
			{
				AdjacentChestXPos = (TileEntityChest)WorldObj.GetBlockTileEntity(XCoord + 1, YCoord, ZCoord);
			}

			if (WorldObj.GetBlockId(XCoord, YCoord, ZCoord - 1) == Block.Chest.BlockID)
			{
				AdjacentChestZNeg = (TileEntityChest)WorldObj.GetBlockTileEntity(XCoord, YCoord, ZCoord - 1);
			}

			if (WorldObj.GetBlockId(XCoord, YCoord, ZCoord + 1) == Block.Chest.BlockID)
			{
				AdjacentChestZPos = (TileEntityChest)WorldObj.GetBlockTileEntity(XCoord, YCoord, ZCoord + 1);
			}

			if (AdjacentChestZNeg != null)
			{
				AdjacentChestZNeg.UpdateContainingBlockInfo();
			}

			if (AdjacentChestZPos != null)
			{
				AdjacentChestZPos.UpdateContainingBlockInfo();
			}

			if (AdjacentChestXPos != null)
			{
				AdjacentChestXPos.UpdateContainingBlockInfo();
			}

			if (AdjacentChestXNeg != null)
			{
				AdjacentChestXNeg.UpdateContainingBlockInfo();
			}
		}

		/// <summary>
		/// Allows the entity to update its state. Overridden in most subclasses, e.g. the mob spawner uses this to count
		/// ticks and creates a new spawn inside its implementation.
		/// </summary>
		public override void UpdateEntity()
		{
			base.UpdateEntity();
			CheckForAdjacentChests();

			if ((++TicksSinceSync % 20) * 4 == 0)
			{
				WorldObj.PlayNoteAt(XCoord, YCoord, ZCoord, 1, NumUsingPlayers);
			}

			PrevLidAngle = LidAngle;
			float f = 0.1F;

			if (NumUsingPlayers > 0 && LidAngle == 0.0F && AdjacentChestZNeg == null && AdjacentChestXNeg == null)
			{
				double d = (double)XCoord + 0.5D;
				double d1 = (double)ZCoord + 0.5D;

				if (AdjacentChestZPos != null)
				{
					d1 += 0.5D;
				}

				if (AdjacentChestXPos != null)
				{
					d += 0.5D;
				}

				WorldObj.PlaySoundEffect(d, (double)YCoord + 0.5D, d1, "random.chestopen", 0.5F, WorldObj.Rand.NextFloat() * 0.1F + 0.9F);
			}

			if (NumUsingPlayers == 0 && LidAngle > 0.0F || NumUsingPlayers > 0 && LidAngle < 1.0F)
			{
				float f1 = LidAngle;

				if (NumUsingPlayers > 0)
				{
					LidAngle += f;
				}
				else
				{
					LidAngle -= f;
				}

				if (LidAngle > 1.0F)
				{
					LidAngle = 1.0F;
				}

				float f2 = 0.5F;

				if (LidAngle < f2 && f1 >= f2 && AdjacentChestZNeg == null && AdjacentChestXNeg == null)
				{
					double d2 = (double)XCoord + 0.5D;
					double d3 = (double)ZCoord + 0.5D;

					if (AdjacentChestZPos != null)
					{
						d3 += 0.5D;
					}

					if (AdjacentChestXPos != null)
					{
						d2 += 0.5D;
					}

					WorldObj.PlaySoundEffect(d2, (double)YCoord + 0.5D, d3, "random.chestclosed", 0.5F, WorldObj.Rand.NextFloat() * 0.1F + 0.9F);
				}

				if (LidAngle < 0.0F)
				{
					LidAngle = 0.0F;
				}
			}
		}

		public override void OnTileEntityPowered(int par1, int par2)
		{
			if (par1 == 1)
			{
				NumUsingPlayers = par2;
			}
		}

		public virtual void OpenChest()
		{
			NumUsingPlayers++;
			WorldObj.PlayNoteAt(XCoord, YCoord, ZCoord, 1, NumUsingPlayers);
		}

		public virtual void CloseChest()
		{
			NumUsingPlayers--;
			WorldObj.PlayNoteAt(XCoord, YCoord, ZCoord, 1, NumUsingPlayers);
		}

		/// <summary>
		/// invalidates a tile entity
		/// </summary>
		public override void Invalidate()
		{
			UpdateContainingBlockInfo();
			CheckForAdjacentChests();
			base.Invalidate();
		}
	}
}