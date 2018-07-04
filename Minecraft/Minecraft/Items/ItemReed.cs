namespace net.minecraft.src
{

	public class ItemReed : Item
	{
		/// <summary>
		/// The ID of the block the reed will spawn when used from inventory bar. </summary>
		private int SpawnID;

		public ItemReed(int par1, Block par2Block) : base(par1)
		{
			SpawnID = par2Block.BlockID;
		}

		/// <summary>
		/// Callback for item usage. If the item does something special on right clicking, he will have one of those. Return
		/// True if something happen and false if it don't. This is for ITEMS, not BLOCKS !
		/// </summary>
		public override bool OnItemUse(ItemStack par1ItemStack, EntityPlayer par2EntityPlayer, World par3World, int par4, int par5, int par6, int par7)
		{
			int i = par3World.GetBlockId(par4, par5, par6);

			if (i == Block.Snow.BlockID)
			{
				par7 = 1;
			}
			else if (i != Block.Vine.BlockID && i != Block.TallGrass.BlockID && i != Block.DeadBush.BlockID)
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
			}

			if (!par2EntityPlayer.CanPlayerEdit(par4, par5, par6))
			{
				return false;
			}

			if (par1ItemStack.StackSize == 0)
			{
				return false;
			}

			if (par3World.CanBlockBePlacedAt(SpawnID, par4, par5, par6, false, par7))
			{
				Block block = Block.BlocksList[SpawnID];

				if (par3World.SetBlockWithNotify(par4, par5, par6, SpawnID))
				{
					if (par3World.GetBlockId(par4, par5, par6) == SpawnID)
					{
						Block.BlocksList[SpawnID].OnBlockPlaced(par3World, par4, par5, par6, par7);
						Block.BlocksList[SpawnID].OnBlockPlacedBy(par3World, par4, par5, par6, par2EntityPlayer);
					}

					par3World.PlaySoundEffect((float)par4 + 0.5F, (float)par5 + 0.5F, (float)par6 + 0.5F, block.StepSound.GetStepSound(), (block.StepSound.GetVolume() + 1.0F) / 2.0F, block.StepSound.GetPitch() * 0.8F);
					par1ItemStack.StackSize--;
				}
			}

			return true;
		}
	}
}