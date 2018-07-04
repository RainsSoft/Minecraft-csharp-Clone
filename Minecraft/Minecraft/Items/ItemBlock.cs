namespace net.minecraft.src
{
	public class ItemBlock : Item
	{
		/// <summary>
		/// The block ID of the Block associated with this ItemBlock </summary>
		private int BlockID;

		public ItemBlock(int par1) : base(par1)
		{
			BlockID = par1 + 256;
			SetIconIndex(Block.BlocksList[par1 + 256].GetBlockTextureFromSide(2));
		}

		/// <summary>
		/// Returns the BlockID for this Item
		/// </summary>
		public virtual int GetBlockID()
		{
			return BlockID;
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

			if (par1ItemStack.StackSize == 0)
			{
				return false;
			}

			if (!par2EntityPlayer.CanPlayerEdit(par4, par5, par6))
			{
				return false;
			}

			if (par5 == 255 && Block.BlocksList[BlockID].BlockMaterial.IsSolid())
			{
				return false;
			}

			if (par3World.CanBlockBePlacedAt(BlockID, par4, par5, par6, false, par7))
			{
				Block block = Block.BlocksList[BlockID];

				if (par3World.SetBlockAndMetadataWithNotify(par4, par5, par6, BlockID, GetMetadata(par1ItemStack.GetItemDamage())))
				{
					if (par3World.GetBlockId(par4, par5, par6) == BlockID)
					{
						Block.BlocksList[BlockID].OnBlockPlaced(par3World, par4, par5, par6, par7);
						Block.BlocksList[BlockID].OnBlockPlacedBy(par3World, par4, par5, par6, par2EntityPlayer);
					}

					par3World.PlaySoundEffect((float)par4 + 0.5F, (float)par5 + 0.5F, (float)par6 + 0.5F, block.StepSound.GetStepSound(), (block.StepSound.GetVolume() + 1.0F) / 2.0F, block.StepSound.GetPitch() * 0.8F);
					par1ItemStack.StackSize--;
				}

				return true;
			}
			else
			{
				return false;
			}
		}

		public override string GetItemNameIS(ItemStack par1ItemStack)
		{
			return Block.BlocksList[BlockID].GetBlockName();
		}

		public override string GetItemName()
		{
			return Block.BlocksList[BlockID].GetBlockName();
		}
	}
}