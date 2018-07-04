using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class ContainerEnchantment : Container
	{
		/// <summary>
		/// SlotEnchantmentTable object with ItemStack to be enchanted </summary>
		public IInventory TableInventory;

		/// <summary>
		/// current world (for bookshelf counting) </summary>
		private World WorldPointer;
		private int PosX;
		private int PosY;
		private int PosZ;
		private Random Rand;

		/// <summary>
		/// used as seed for EnchantmentNameParts (see GuiEnchantment) </summary>
		public long NameSeed;
		public int[] EnchantLevels;

		public ContainerEnchantment(InventoryPlayer par1InventoryPlayer, World par2World, int par3, int par4, int par5)
		{
			TableInventory = new SlotEnchantmentTable(this, "Enchant", 1);
			Rand = new Random();
			EnchantLevels = new int[3];
			WorldPointer = par2World;
			PosX = par3;
			PosY = par4;
			PosZ = par5;
			AddSlot(new SlotEnchantment(this, TableInventory, 0, 25, 47));

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
				icrafting.UpdateCraftingInventoryInfo(this, 0, EnchantLevels[0]);
				icrafting.UpdateCraftingInventoryInfo(this, 1, EnchantLevels[1]);
				icrafting.UpdateCraftingInventoryInfo(this, 2, EnchantLevels[2]);
			}
		}

		public override void UpdateProgressBar(int par1, int par2)
		{
			if (par1 >= 0 && par1 <= 2)
			{
				EnchantLevels[par1] = par2;
			}
			else
			{
				base.UpdateProgressBar(par1, par2);
			}
		}

		/// <summary>
		/// Callback for when the crafting matrix is changed.
		/// </summary>
		public override void OnCraftMatrixChanged(IInventory par1IInventory)
		{
			if (par1IInventory == TableInventory)
			{
				ItemStack itemstack = par1IInventory.GetStackInSlot(0);

				if (itemstack == null || !itemstack.IsItemEnchantable())
				{
					for (int i = 0; i < 3; i++)
					{
						EnchantLevels[i] = 0;
					}
				}
				else
				{
					NameSeed = Rand.Next();

					if (!WorldPointer.IsRemote)
					{
						int j = 0;

						for (int k = -1; k <= 1; k++)
						{
							for (int i1 = -1; i1 <= 1; i1++)
							{
								if (k == 0 && i1 == 0 || !WorldPointer.IsAirBlock(PosX + i1, PosY, PosZ + k) || !WorldPointer.IsAirBlock(PosX + i1, PosY + 1, PosZ + k))
								{
									continue;
								}

								if (WorldPointer.GetBlockId(PosX + i1 * 2, PosY, PosZ + k * 2) == Block.BookShelf.BlockID)
								{
									j++;
								}

								if (WorldPointer.GetBlockId(PosX + i1 * 2, PosY + 1, PosZ + k * 2) == Block.BookShelf.BlockID)
								{
									j++;
								}

								if (i1 == 0 || k == 0)
								{
									continue;
								}

								if (WorldPointer.GetBlockId(PosX + i1 * 2, PosY, PosZ + k) == Block.BookShelf.BlockID)
								{
									j++;
								}

								if (WorldPointer.GetBlockId(PosX + i1 * 2, PosY + 1, PosZ + k) == Block.BookShelf.BlockID)
								{
									j++;
								}

								if (WorldPointer.GetBlockId(PosX + i1, PosY, PosZ + k * 2) == Block.BookShelf.BlockID)
								{
									j++;
								}

								if (WorldPointer.GetBlockId(PosX + i1, PosY + 1, PosZ + k * 2) == Block.BookShelf.BlockID)
								{
									j++;
								}
							}
						}

						for (int l = 0; l < 3; l++)
						{
							EnchantLevels[l] = EnchantmentHelper.CalcItemStackEnchantability(Rand, l, j, itemstack);
						}

						UpdateCraftingResults();
					}
				}
			}
		}

		/// <summary>
		/// enchants the item on the table using the specified slot; also deducts XP from player
		/// </summary>
		public override bool EnchantItem(EntityPlayer par1EntityPlayer, int par2)
		{
			ItemStack itemstack = TableInventory.GetStackInSlot(0);

			if (EnchantLevels[par2] > 0 && itemstack != null && (par1EntityPlayer.ExperienceLevel >= EnchantLevels[par2] || par1EntityPlayer.Capabilities.IsCreativeMode))
			{
				if (!WorldPointer.IsRemote)
				{
					List<EnchantmentData> list = EnchantmentHelper.BuildEnchantmentList(Rand, itemstack, EnchantLevels[par2]);

					if (list != null)
					{
						par1EntityPlayer.RemoveExperience(EnchantLevels[par2]);
						EnchantmentData enchantmentdata;

						for (IEnumerator<EnchantmentData> iterator = list.GetEnumerator(); iterator.MoveNext(); itemstack.AddEnchantment(enchantmentdata.Enchantmentobj, enchantmentdata.EnchantmentLevel))
						{
							enchantmentdata = iterator.Current;
						}

						OnCraftMatrixChanged(TableInventory);
					}
				}

				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Callback for when the crafting gui is closed.
		/// </summary>
		public override void OnCraftGuiClosed(EntityPlayer par1EntityPlayer)
		{
			base.OnCraftGuiClosed(par1EntityPlayer);

			if (WorldPointer.IsRemote)
			{
				return;
			}

			ItemStack itemstack = TableInventory.GetStackInSlotOnClosing(0);

			if (itemstack != null)
			{
				par1EntityPlayer.DropPlayerItem(itemstack);
			}
		}

		public override bool CanInteractWith(EntityPlayer par1EntityPlayer)
		{
			if (WorldPointer.GetBlockId(PosX, PosY, PosZ) != Block.EnchantmentTable.BlockID)
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
					if (!MergeItemStack(itemstack1, 1, 37, true))
					{
						return null;
					}
				}
				else
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