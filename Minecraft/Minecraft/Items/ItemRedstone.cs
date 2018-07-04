namespace net.minecraft.src
{
	public class ItemRedstone : Item
	{
		public ItemRedstone(int par1) : base(par1)
		{
		}

		/// <summary>
		/// Callback for item usage. If the item does something special on right clicking, he will have one of those. Return
		/// True if something happen and false if it don't. This is for ITEMS, not BLOCKS !
		/// </summary>
		public override bool OnItemUse(ItemStack par1ItemStack, EntityPlayer par2EntityPlayer, World par3World, int par4, int par5, int par6, int par7)
		{
			if (par3World.GetBlockId(par4, par5, par6) != Block.Snow.BlockID)
			{
				if (par7 == 0)
				{
					par5--;
				}

				if (par7 == 1)
				{
					par5++;
				}

				if (par7 == 2)
				{
					par6--;
				}

				if (par7 == 3)
				{
					par6++;
				}

				if (par7 == 4)
				{
					par4--;
				}

				if (par7 == 5)
				{
					par4++;
				}

				if (!par3World.IsAirBlock(par4, par5, par6))
				{
					return false;
				}
			}

			if (!par2EntityPlayer.CanPlayerEdit(par4, par5, par6))
			{
				return false;
			}

			if (Block.RedstoneWire.CanPlaceBlockAt(par3World, par4, par5, par6))
			{
				par1ItemStack.StackSize--;
				par3World.SetBlockWithNotify(par4, par5, par6, Block.RedstoneWire.BlockID);
			}

			return true;
		}
	}
}