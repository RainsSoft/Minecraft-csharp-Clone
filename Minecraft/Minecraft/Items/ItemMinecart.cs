namespace net.minecraft.src
{

	public class ItemMinecart : Item
	{
		public int MinecartType;

		public ItemMinecart(int par1, int par2) : base(par1)
		{
			MaxStackSize = 1;
			MinecartType = par2;
		}

		/// <summary>
		/// Callback for item usage. If the item does something special on right clicking, he will have one of those. Return
		/// True if something happen and false if it don't. This is for ITEMS, not BLOCKS !
		/// </summary>
		public override bool OnItemUse(ItemStack par1ItemStack, EntityPlayer par2EntityPlayer, World par3World, int par4, int par5, int par6, int par7)
		{
			int i = par3World.GetBlockId(par4, par5, par6);

			if (BlockRail.IsRailBlock(i))
			{
				if (!par3World.IsRemote)
				{
					par3World.SpawnEntityInWorld(new EntityMinecart(par3World, (float)par4 + 0.5F, (float)par5 + 0.5F, (float)par6 + 0.5F, MinecartType));
				}

				par1ItemStack.StackSize--;
				return true;
			}
			else
			{
				return false;
			}
		}
	}

}