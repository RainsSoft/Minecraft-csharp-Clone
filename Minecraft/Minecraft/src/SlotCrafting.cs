using System;

namespace net.minecraft.src
{
	public class SlotCrafting : Slot
	{
		/// <summary>
		/// The craft matrix inventory linked to this result slot. </summary>
		private readonly IInventory CraftMatrix;

		/// <summary>
		/// The player that is using the GUI where this slot resides. </summary>
		private EntityPlayer ThePlayer;
		private int Field_48436_g;

		public SlotCrafting(EntityPlayer par1EntityPlayer, IInventory par2IInventory, IInventory par3IInventory, int par4, int par5, int par6) : base(par3IInventory, par4, par5, par6)
		{
			ThePlayer = par1EntityPlayer;
			CraftMatrix = par2IInventory;
		}

		/// <summary>
		/// Check if the stack is a valid item for this slot. Always true beside for the armor slots.
		/// </summary>
		public override bool IsItemValid(ItemStack par1ItemStack)
		{
			return false;
		}

		/// <summary>
		/// Decrease the size of the stack in slot (first int arg) by the amount of the second int arg. Returns the new
		/// stack.
		/// </summary>
		public override ItemStack DecrStackSize(int par1)
		{
			if (GetHasStack())
			{
				Field_48436_g += Math.Min(par1, GetStack().StackSize);
			}

			return base.DecrStackSize(par1);
		}

		protected override void Func_48435_a(ItemStack par1ItemStack, int par2)
		{
			Field_48436_g += par2;
			Func_48434_c(par1ItemStack);
		}

		protected override void Func_48434_c(ItemStack par1ItemStack)
		{
			par1ItemStack.OnCrafting(ThePlayer.WorldObj, ThePlayer, Field_48436_g);
			Field_48436_g = 0;

			if (par1ItemStack.ItemID == Block.Workbench.BlockID)
			{
				ThePlayer.AddStat(AchievementList.BuildWorkBench, 1);
			}
			else if (par1ItemStack.ItemID == Item.PickaxeWood.ShiftedIndex)
			{
				ThePlayer.AddStat(AchievementList.BuildPickaxe, 1);
			}
			else if (par1ItemStack.ItemID == Block.StoneOvenIdle.BlockID)
			{
				ThePlayer.AddStat(AchievementList.BuildFurnace, 1);
			}
			else if (par1ItemStack.ItemID == Item.HoeWood.ShiftedIndex)
			{
				ThePlayer.AddStat(AchievementList.BuildHoe, 1);
			}
			else if (par1ItemStack.ItemID == Item.Bread.ShiftedIndex)
			{
				ThePlayer.AddStat(AchievementList.MakeBread, 1);
			}
			else if (par1ItemStack.ItemID == Item.Cake.ShiftedIndex)
			{
				ThePlayer.AddStat(AchievementList.BakeCake, 1);
			}
			else if (par1ItemStack.ItemID == Item.PickaxeStone.ShiftedIndex)
			{
				ThePlayer.AddStat(AchievementList.BuildBetterPickaxe, 1);
			}
			else if (par1ItemStack.ItemID == Item.SwordWood.ShiftedIndex)
			{
				ThePlayer.AddStat(AchievementList.BuildSword, 1);
			}
			else if (par1ItemStack.ItemID == Block.EnchantmentTable.BlockID)
			{
				ThePlayer.AddStat(AchievementList.Enchantments, 1);
			}
			else if (par1ItemStack.ItemID == Block.BookShelf.BlockID)
			{
				ThePlayer.AddStat(AchievementList.Bookcase, 1);
			}
		}

		/// <summary>
		/// Called when the player picks up an item from an inventory slot
		/// </summary>
		public override void OnPickupFromSlot(ItemStack par1ItemStack)
		{
			Func_48434_c(par1ItemStack);

			for (int i = 0; i < CraftMatrix.GetSizeInventory(); i++)
			{
				ItemStack itemstack = CraftMatrix.GetStackInSlot(i);

				if (itemstack == null)
				{
					continue;
				}

				CraftMatrix.DecrStackSize(i, 1);

				if (!itemstack.GetItem().HasContainerItem())
				{
					continue;
				}

				ItemStack itemstack1 = new ItemStack(itemstack.GetItem().GetContainerItem());

				if (itemstack.GetItem().DoesContainerItemLeaveCraftingGrid(itemstack) && ThePlayer.Inventory.AddItemStackToInventory(itemstack1))
				{
					continue;
				}

				if (CraftMatrix.GetStackInSlot(i) == null)
				{
					CraftMatrix.SetInventorySlotContents(i, itemstack1);
				}
				else
				{
					ThePlayer.DropPlayerItem(itemstack1);
				}
			}
		}
	}
}