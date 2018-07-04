namespace net.minecraft.src
{
	public class ItemSign : Item
	{
		public ItemSign(int par1) : base(par1)
		{
			MaxStackSize = 1;
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

			if (!par3World.GetBlockMaterial(par4, par5, par6).IsSolid())
			{
				return false;
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

			if (!Block.SignPost.CanPlaceBlockAt(par3World, par4, par5, par6))
			{
				return false;
			}

			if (par7 == 1)
			{
				int i = MathHelper2.Floor_double((double)(((par2EntityPlayer.RotationYaw + 180F) * 16F) / 360F) + 0.5D) & 0xf;
				par3World.SetBlockAndMetadataWithNotify(par4, par5, par6, Block.SignPost.BlockID, i);
			}
			else
			{
				par3World.SetBlockAndMetadataWithNotify(par4, par5, par6, Block.SignWall.BlockID, par7);
			}

			par1ItemStack.StackSize--;
			TileEntitySign tileentitysign = (TileEntitySign)par3World.GetBlockTileEntity(par4, par5, par6);

			if (tileentitysign != null)
			{
				par2EntityPlayer.DisplayGUIEditSign(tileentitysign);
			}

			return true;
		}
	}
}