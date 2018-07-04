namespace net.minecraft.src
{

	public class ItemGlassBottle : Item
	{
		public ItemGlassBottle(int par1) : base(par1)
		{
		}

		/// <summary>
		/// Called whenever this item is equipped and the right mouse button is pressed. Args: itemStack, world, entityPlayer
		/// </summary>
		public override ItemStack OnItemRightClick(ItemStack par1ItemStack, World par2World, EntityPlayer par3EntityPlayer)
		{
			MovingObjectPosition movingobjectposition = GetMovingObjectPositionFromPlayer(par2World, par3EntityPlayer, true);

			if (movingobjectposition == null)
			{
				return par1ItemStack;
			}

			if (movingobjectposition.TypeOfHit == EnumMovingObjectType.TILE)
			{
				int i = movingobjectposition.BlockX;
				int j = movingobjectposition.BlockY;
				int k = movingobjectposition.BlockZ;

				if (!par2World.CanMineBlock(par3EntityPlayer, i, j, k))
				{
					return par1ItemStack;
				}

				if (!par3EntityPlayer.CanPlayerEdit(i, j, k))
				{
					return par1ItemStack;
				}

				if (par2World.GetBlockMaterial(i, j, k) == Material.Water)
				{
					par1ItemStack.StackSize--;

					if (par1ItemStack.StackSize <= 0)
					{
						return new ItemStack(Item.Potion);
					}

					if (!par3EntityPlayer.Inventory.AddItemStackToInventory(new ItemStack(Item.Potion)))
					{
						par3EntityPlayer.DropPlayerItem(new ItemStack(Item.Potion.ShiftedIndex, 1, 0));
					}
				}
			}

			return par1ItemStack;
		}
	}

}