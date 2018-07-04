namespace net.minecraft.src
{
	public class ItemLilyPad : ItemColored
	{
		public ItemLilyPad(int par1) : base(par1, false)
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

				if (par2World.GetBlockMaterial(i, j, k) == Material.Water && par2World.GetBlockMetadata(i, j, k) == 0 && par2World.IsAirBlock(i, j + 1, k))
				{
					par2World.SetBlockWithNotify(i, j + 1, k, Block.Waterlily.BlockID);

					if (!par3EntityPlayer.Capabilities.IsCreativeMode)
					{
						par1ItemStack.StackSize--;
					}
				}
			}

			return par1ItemStack;
		}

		public override int GetColorFromDamage(int par1, int par2)
		{
			return Block.Waterlily.GetRenderColor(par1);
		}
	}
}