namespace net.minecraft.src
{
	public class ItemBow : Item
	{
		public ItemBow(int par1) : base(par1)
		{
			MaxStackSize = 1;
			SetMaxDamage(384);
		}

		/// <summary>
		/// called when the player releases the use item button. Args: itemstack, world, entityplayer, itemInUseCount
		/// </summary>
		public override void OnPlayerStoppedUsing(ItemStack par1ItemStack, World par2World, EntityPlayer par3EntityPlayer, int par4)
		{
			bool flag = par3EntityPlayer.Capabilities.IsCreativeMode || EnchantmentHelper.GetEnchantmentLevel(Enchantment.Infinity.EffectId, par1ItemStack) > 0;

			if (flag || par3EntityPlayer.Inventory.HasItem(Item.Arrow.ShiftedIndex))
			{
				int i = GetMaxItemUseDuration(par1ItemStack) - par4;
				float f = (float)i / 20F;
				f = (f * f + f * 2.0F) / 3F;

				if ((double)f < 0.10000000000000001D)
				{
					return;
				}

				if (f > 1.0F)
				{
					f = 1.0F;
				}

				EntityArrow entityarrow = new EntityArrow(par2World, par3EntityPlayer, f * 2.0F);

				if (f == 1.0F)
				{
					entityarrow.ArrowCritical = true;
				}

				int j = EnchantmentHelper.GetEnchantmentLevel(Enchantment.Power.EffectId, par1ItemStack);

				if (j > 0)
				{
					entityarrow.SetDamage(entityarrow.GetDamage() + (double)j * 0.5D + 0.5D);
				}

				int k = EnchantmentHelper.GetEnchantmentLevel(Enchantment.Punch.EffectId, par1ItemStack);

				if (k > 0)
				{
					entityarrow.Func_46023_b(k);
				}

				if (EnchantmentHelper.GetEnchantmentLevel(Enchantment.Flame.EffectId, par1ItemStack) > 0)
				{
					entityarrow.SetFire(100);
				}

				par1ItemStack.DamageItem(1, par3EntityPlayer);
				par2World.PlaySoundAtEntity(par3EntityPlayer, "random.bow", 1.0F, 1.0F / (ItemRand.NextFloat() * 0.4F + 1.2F) + f * 0.5F);

				if (!flag)
				{
					par3EntityPlayer.Inventory.ConsumeInventoryItem(Item.Arrow.ShiftedIndex);
				}
				else
				{
					entityarrow.DoesArrowBelongToPlayer = false;
				}

				if (!par2World.IsRemote)
				{
					par2World.SpawnEntityInWorld(entityarrow);
				}
			}
		}

		public override ItemStack OnFoodEaten(ItemStack par1ItemStack, World par2World, EntityPlayer par3EntityPlayer)
		{
			return par1ItemStack;
		}

		/// <summary>
		/// How long it takes to use or consume an item
		/// </summary>
		public override int GetMaxItemUseDuration(ItemStack par1ItemStack)
		{
			return 0x11940;
		}

		/// <summary>
		/// returns the action that specifies what animation to play when the items is being used
		/// </summary>
		public override EnumAction GetItemUseAction(ItemStack par1ItemStack)
		{
			return EnumAction.Bow;
		}

		/// <summary>
		/// Called whenever this item is equipped and the right mouse button is pressed. Args: itemStack, world, entityPlayer
		/// </summary>
		public override ItemStack OnItemRightClick(ItemStack par1ItemStack, World par2World, EntityPlayer par3EntityPlayer)
		{
			if (par3EntityPlayer.Capabilities.IsCreativeMode || par3EntityPlayer.Inventory.HasItem(Item.Arrow.ShiftedIndex))
			{
				par3EntityPlayer.SetItemInUse(par1ItemStack, GetMaxItemUseDuration(par1ItemStack));
			}

			return par1ItemStack;
		}

		/// <summary>
		/// Return the enchantability factor of the item, most of the time is based on material.
		/// </summary>
		public override int GetItemEnchantability()
		{
			return 1;
		}
	}
}