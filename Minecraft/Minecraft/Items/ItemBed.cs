namespace net.minecraft.src
{

	public class ItemBed : Item
	{
		public ItemBed(int par1) : base(par1)
		{
		}

		/// <summary>
		/// Callback for item usage. If the item does something special on right clicking, he will have one of those. Return
		/// True if something happen and false if it don't. This is for ITEMS, not BLOCKS !
		/// </summary>
		public override bool OnItemUse(ItemStack par1ItemStack, EntityPlayer par2EntityPlayer, World par3World, int par4, int par5, int par6, int par7)
		{
			if (par7 != 1)
			{
				return false;
			}

			par5++;
			BlockBed blockbed = (BlockBed)Block.Bed;
			int i = MathHelper2.Floor_double((double)((par2EntityPlayer.RotationYaw * 4F) / 360F) + 0.5D) & 3;
			sbyte byte0 = 0;
			sbyte byte1 = 0;

			if (i == 0)
			{
				byte1 = 1;
			}

			if (i == 1)
			{
				byte0 = -1;
			}

			if (i == 2)
			{
				byte1 = -1;
			}

			if (i == 3)
			{
				byte0 = 1;
			}

			if (!par2EntityPlayer.CanPlayerEdit(par4, par5, par6) || !par2EntityPlayer.CanPlayerEdit(par4 + byte0, par5, par6 + byte1))
			{
				return false;
			}

			if (par3World.IsAirBlock(par4, par5, par6) && par3World.IsAirBlock(par4 + byte0, par5, par6 + byte1) && par3World.IsBlockNormalCube(par4, par5 - 1, par6) && par3World.IsBlockNormalCube(par4 + byte0, par5 - 1, par6 + byte1))
			{
				par3World.SetBlockAndMetadataWithNotify(par4, par5, par6, blockbed.BlockID, i);

				if (par3World.GetBlockId(par4, par5, par6) == blockbed.BlockID)
				{
					par3World.SetBlockAndMetadataWithNotify(par4 + byte0, par5, par6 + byte1, blockbed.BlockID, i + 8);
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