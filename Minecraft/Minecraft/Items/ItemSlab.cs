using System.Text;

namespace net.minecraft.src
{
	public class ItemSlab : ItemBlock
	{
		public ItemSlab(int par1) : base(par1)
		{
			SetMaxDamage(0);
			SetHasSubtypes(true);
		}

		/// <summary>
		/// Gets an icon index based on an item's damage value
		/// </summary>
		public override int GetIconFromDamage(int par1)
		{
			return Block.StairSingle.GetBlockTextureFromSideAndMetadata(2, par1);
		}

		/// <summary>
		/// Returns the metadata of the block which this Item (ItemBlock) can place
		/// </summary>
		public override int GetMetadata(int par1)
		{
			return par1;
		}

		public override string GetItemNameIS(ItemStack par1ItemStack)
		{
			int i = par1ItemStack.GetItemDamage();

			if (i < 0 || i >= BlockStep.BlockStepTypes.Length)
			{
				i = 0;
			}

			return (new StringBuilder()).Append(base.GetItemName()).Append(".").Append(BlockStep.BlockStepTypes[i]).ToString();
		}

		/// <summary>
		/// Callback for item usage. If the item does something special on right clicking, he will have one of those. Return
		/// True if something happen and false if it don't. This is for ITEMS, not BLOCKS !
		/// </summary>
		public override bool OnItemUse(ItemStack par1ItemStack, EntityPlayer par2EntityPlayer, World par3World, int par4, int par5, int par6, int par7)
		{
			if (par1ItemStack.StackSize == 0)
			{
				return false;
			}

			if (!par2EntityPlayer.CanPlayerEdit(par4, par5, par6))
			{
				return false;
			}

			int i = par3World.GetBlockId(par4, par5, par6);
			int j = par3World.GetBlockMetadata(par4, par5, par6);
			int k = j & 7;
			bool flag = (j & 8) != 0;

			if ((par7 == 1 && !flag || par7 == 0 && flag) && i == Block.StairSingle.BlockID && k == par1ItemStack.GetItemDamage())
			{
				if (par3World.CheckIfAABBIsClear(Block.StairDouble.GetCollisionBoundingBoxFromPool(par3World, par4, par5, par6)) && par3World.SetBlockAndMetadataWithNotify(par4, par5, par6, Block.StairDouble.BlockID, k))
				{
					par3World.PlaySoundEffect((float)par4 + 0.5F, (float)par5 + 0.5F, (float)par6 + 0.5F, Block.StairDouble.StepSound.GetStepSound(), (Block.StairDouble.StepSound.GetVolume() + 1.0F) / 2.0F, Block.StairDouble.StepSound.GetPitch() * 0.8F);
					par1ItemStack.StackSize--;
				}

				return true;
			}

			if (Func_50087_b(par1ItemStack, par2EntityPlayer, par3World, par4, par5, par6, par7))
			{
				return true;
			}
			else
			{
				return base.OnItemUse(par1ItemStack, par2EntityPlayer, par3World, par4, par5, par6, par7);
			}
		}

		private static bool Func_50087_b(ItemStack par0ItemStack, EntityPlayer par1EntityPlayer, World par2World, int par3, int par4, int par5, int par6)
		{
			if (par6 == 0)
			{
				par4--;
			}

			if (par6 == 1)
			{
				par4++;
			}

			if (par6 == 2)
			{
				par5--;
			}

			if (par6 == 3)
			{
				par5++;
			}

			if (par6 == 4)
			{
				par3--;
			}

			if (par6 == 5)
			{
				par3++;
			}

			int i = par2World.GetBlockId(par3, par4, par5);
			int j = par2World.GetBlockMetadata(par3, par4, par5);
			int k = j & 7;

			if (i == Block.StairSingle.BlockID && k == par0ItemStack.GetItemDamage())
			{
				if (par2World.CheckIfAABBIsClear(Block.StairDouble.GetCollisionBoundingBoxFromPool(par2World, par3, par4, par5)) && par2World.SetBlockAndMetadataWithNotify(par3, par4, par5, Block.StairDouble.BlockID, k))
				{
					par2World.PlaySoundEffect((float)par3 + 0.5F, (float)par4 + 0.5F, (float)par5 + 0.5F, Block.StairDouble.StepSound.GetStepSound(), (Block.StairDouble.StepSound.GetVolume() + 1.0F) / 2.0F, Block.StairDouble.StepSound.GetPitch() * 0.8F);
					par0ItemStack.StackSize--;
				}

				return true;
			}
			else
			{
				return false;
			}
		}
	}
}