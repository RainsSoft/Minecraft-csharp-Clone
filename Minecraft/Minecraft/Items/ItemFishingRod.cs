namespace net.minecraft.src
{
	public class ItemFishingRod : Item
	{
		public ItemFishingRod(int par1) : base(par1)
		{
			SetMaxDamage(64);
			SetMaxStackSize(1);
		}

		/// <summary>
		/// Returns True is the item is renderer in full 3D when hold.
		/// </summary>
		public override bool IsFull3D()
		{
			return true;
		}

		/// <summary>
		/// Returns true if this item should be rotated by 180 degrees around the Y axis when being held in an entities
		/// hands.
		/// </summary>
		public override bool ShouldRotateAroundWhenRendering()
		{
			return true;
		}

		/// <summary>
		/// Called whenever this item is equipped and the right mouse button is pressed. Args: itemStack, world, entityPlayer
		/// </summary>
		public override ItemStack OnItemRightClick(ItemStack par1ItemStack, World par2World, EntityPlayer par3EntityPlayer)
		{
			if (par3EntityPlayer.FishEntity != null)
			{
				int i = par3EntityPlayer.FishEntity.CatchFish();
				par1ItemStack.DamageItem(i, par3EntityPlayer);
				par3EntityPlayer.SwingItem();
			}
			else
			{
				par2World.PlaySoundAtEntity(par3EntityPlayer, "random.bow", 0.5F, 0.4F / (ItemRand.NextFloat() * 0.4F + 0.8F));

				if (!par2World.IsRemote)
				{
					par2World.SpawnEntityInWorld(new EntityFishHook(par2World, par3EntityPlayer));
				}

				par3EntityPlayer.SwingItem();
			}

			return par1ItemStack;
		}
	}
}