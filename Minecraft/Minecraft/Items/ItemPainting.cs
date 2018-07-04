namespace net.minecraft.src
{

	public class ItemPainting : Item
	{
		public ItemPainting(int par1) : base(par1)
		{
		}

		/// <summary>
		/// Callback for item usage. If the item does something special on right clicking, he will have one of those. Return
		/// True if something happen and false if it don't. This is for ITEMS, not BLOCKS !
		/// </summary>
		public override bool OnItemUse(ItemStack par1ItemStack, EntityPlayer par2EntityPlayer, World par3World, int par4, int par5, int par6, int par7)
		{
			if (par7 == 0)
			{
				return false;
			}

			if (par7 == 1)
			{
				return false;
			}

			sbyte byte0 = 0;

			if (par7 == 4)
			{
				byte0 = 1;
			}

			if (par7 == 3)
			{
				byte0 = 2;
			}

			if (par7 == 5)
			{
				byte0 = 3;
			}

			if (!par2EntityPlayer.CanPlayerEdit(par4, par5, par6))
			{
				return false;
			}

			EntityPainting entitypainting = new EntityPainting(par3World, par4, par5, par6, byte0);

			if (entitypainting.OnValidSurface())
			{
				if (!par3World.IsRemote)
				{
					par3World.SpawnEntityInWorld(entitypainting);
				}

				par1ItemStack.StackSize--;
			}

			return true;
		}
	}

}