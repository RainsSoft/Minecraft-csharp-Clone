namespace net.minecraft.src
{
	public class ItemFireball : Item
	{
		public ItemFireball(int par1) : base(par1)
		{
		}

		/// <summary>
		/// Callback for item usage. If the item does something special on right clicking, he will have one of those. Return
		/// True if something happen and false if it don't. This is for ITEMS, not BLOCKS !
		/// </summary>
		public override bool OnItemUse(ItemStack par1ItemStack, EntityPlayer par2EntityPlayer, World par3World, int par4, int par5, int par6, int par7)
		{
			if (par3World.IsRemote)
			{
				return true;
			}

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

			if (!par2EntityPlayer.CanPlayerEdit(par4, par5, par6))
			{
				return false;
			}

			int i = par3World.GetBlockId(par4, par5, par6);

			if (i == 0)
			{
				par3World.PlaySoundEffect((double)par4 + 0.5D, (double)par5 + 0.5D, (double)par6 + 0.5D, "fire.ignite", 1.0F, ItemRand.NextFloat() * 0.4F + 0.8F);
				par3World.SetBlockWithNotify(par4, par5, par6, Block.Fire.BlockID);
			}

			if (!par2EntityPlayer.Capabilities.IsCreativeMode)
			{
				par1ItemStack.StackSize--;
			}

			return true;
		}
	}
}