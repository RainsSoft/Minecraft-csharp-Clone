using System;

namespace net.minecraft.src
{

	public class SlotFurnace : Slot
	{
		/// <summary>
		/// The player that is using the GUI where this slot resides. </summary>
		private EntityPlayer ThePlayer;
		private int Field_48437_f;

		public SlotFurnace(EntityPlayer par1EntityPlayer, IInventory par2IInventory, int par3, int par4, int par5) : base(par2IInventory, par3, par4, par5)
		{
			ThePlayer = par1EntityPlayer;
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
				Field_48437_f += Math.Min(par1, GetStack().StackSize);
			}

			return base.DecrStackSize(par1);
		}

		/// <summary>
		/// Called when the player picks up an item from an inventory slot
		/// </summary>
		public override void OnPickupFromSlot(ItemStack par1ItemStack)
		{
			Func_48434_c(par1ItemStack);
			base.OnPickupFromSlot(par1ItemStack);
		}

		protected override void Func_48435_a(ItemStack par1ItemStack, int par2)
		{
			Field_48437_f += par2;
			Func_48434_c(par1ItemStack);
		}

		protected override void Func_48434_c(ItemStack par1ItemStack)
		{
			par1ItemStack.OnCrafting(ThePlayer.WorldObj, ThePlayer, Field_48437_f);
			Field_48437_f = 0;

			if (par1ItemStack.ItemID == Item.IngotIron.ShiftedIndex)
			{
				ThePlayer.AddStat(AchievementList.AcquireIron, 1);
			}

			if (par1ItemStack.ItemID == Item.FishCooked.ShiftedIndex)
			{
				ThePlayer.AddStat(AchievementList.CookFish, 1);
			}
		}
	}

}