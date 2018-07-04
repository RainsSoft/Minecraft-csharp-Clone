using System.Text;

namespace net.minecraft.src
{
	public class ItemDye : Item
	{
		public static readonly string[] DyeColorNames = { "black", "red", "green", "brown", "blue", "purple", "cyan", "silver", "gray", "pink", "lime", "yellow", "lightBlue", "magenta", "orange", "white" };
		public static readonly int[] DyeColors = { 0x1e1b1b, 0xb3312c, 0x3b511a, 0x51301a, 0x253192, 0x7b2fbe, 0x287697, 0x287697, 0x434343, 0xd88198, 0x41cd34, 0xdecf2a, 0x6689d3, 0xc354cd, 0xeb8844, 0xf0f0f0 };

		public ItemDye(int par1) : base(par1)
		{
			SetHasSubtypes(true);
			SetMaxDamage(0);
		}

		/// <summary>
		/// Gets an icon index based on an item's damage value
		/// </summary>
		public override int GetIconFromDamage(int par1)
		{
			int i = MathHelper2.Clamp_int(par1, 0, 15);
			return IconIndex + (i % 8) * 16 + i / 8;
		}

		public override string GetItemNameIS(ItemStack par1ItemStack)
		{
			int i = MathHelper2.Clamp_int(par1ItemStack.GetItemDamage(), 0, 15);
			return (new StringBuilder()).Append(base.GetItemName()).Append(".").Append(DyeColorNames[i]).ToString();
		}

		/// <summary>
		/// Callback for item usage. If the item does something special on right clicking, he will have one of those. Return
		/// True if something happen and false if it don't. This is for ITEMS, not BLOCKS !
		/// </summary>
		public override bool OnItemUse(ItemStack par1ItemStack, EntityPlayer par2EntityPlayer, World par3World, int par4, int par5, int par6, int par7)
		{
			if (!par2EntityPlayer.CanPlayerEdit(par4, par5, par6))
			{
				return false;
			}

			if (par1ItemStack.GetItemDamage() == 15)
			{
				int i = par3World.GetBlockId(par4, par5, par6);

				if (i == Block.Sapling.BlockID)
				{
					if (!par3World.IsRemote)
					{
						((BlockSapling)Block.Sapling).GrowTree(par3World, par4, par5, par6, par3World.Rand);
						par1ItemStack.StackSize--;
					}

					return true;
				}

				if (i == Block.MushroomBrown.BlockID || i == Block.MushroomRed.BlockID)
				{
					if (!par3World.IsRemote && ((BlockMushroom)Block.BlocksList[i]).FertilizeMushroom(par3World, par4, par5, par6, par3World.Rand))
					{
						par1ItemStack.StackSize--;
					}

					return true;
				}

				if (i == Block.MelonStem.BlockID || i == Block.PumpkinStem.BlockID)
				{
					if (!par3World.IsRemote)
					{
						((BlockStem)Block.BlocksList[i]).FertilizeStem(par3World, par4, par5, par6);
						par1ItemStack.StackSize--;
					}

					return true;
				}

				if (i == Block.Crops.BlockID)
				{
					if (!par3World.IsRemote)
					{
						((BlockCrops)Block.Crops).Fertilize(par3World, par4, par5, par6);
						par1ItemStack.StackSize--;
					}

					return true;
				}

				if (i == Block.Grass.BlockID)
				{
					if (!par3World.IsRemote)
					{
						par1ItemStack.StackSize--;
						label0:

						for (int j = 0; j < 128; j++)
						{
							int k = par4;
							int l = par5 + 1;
							int i1 = par6;

							for (int j1 = 0; j1 < j / 16; j1++)
							{
								k += ItemRand.Next(3) - 1;
								l += ((ItemRand.Next(3) - 1) * ItemRand.Next(3)) / 2;
								i1 += ItemRand.Next(3) - 1;

								if (par3World.GetBlockId(k, l - 1, i1) != Block.Grass.BlockID || par3World.IsBlockNormalCube(k, l, i1))
								{
									goto label0;
								}
							}

							if (par3World.GetBlockId(k, l, i1) != 0)
							{
								continue;
							}

							if (ItemRand.Next(10) != 0)
							{
								par3World.SetBlockAndMetadataWithNotify(k, l, i1, Block.TallGrass.BlockID, 1);
								continue;
							}

							if (ItemRand.Next(3) != 0)
							{
								par3World.SetBlockWithNotify(k, l, i1, Block.PlantYellow.BlockID);
							}
							else
							{
								par3World.SetBlockWithNotify(k, l, i1, Block.PlantRed.BlockID);
							}
						}
					}

					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Called when a player right clicks a entity with a item.
		/// </summary>
		public override void UseItemOnEntity(ItemStack par1ItemStack, EntityLiving par2EntityLiving)
		{
			if (par2EntityLiving is EntitySheep)
			{
				EntitySheep entitysheep = (EntitySheep)par2EntityLiving;
				int i = BlockCloth.GetBlockFromDye(par1ItemStack.GetItemDamage());

				if (!entitysheep.GetSheared() && entitysheep.GetFleeceColor() != i)
				{
					entitysheep.SetFleeceColor(i);
					par1ItemStack.StackSize--;
				}
			}
		}
	}

}