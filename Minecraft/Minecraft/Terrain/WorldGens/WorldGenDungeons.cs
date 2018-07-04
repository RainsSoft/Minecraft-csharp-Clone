using System;
using System.Text;

namespace net.minecraft.src
{
	public class WorldGenDungeons : WorldGenerator
	{
		public WorldGenDungeons()
		{
		}

		public override bool Generate(World par1World, Random par2Random, int par3, int par4, int par5)
		{
			sbyte byte0 = 3;
			int i = par2Random.Next(2) + 2;
			int j = par2Random.Next(2) + 2;
			int k = 0;

			for (int l = par3 - i - 1; l <= par3 + i + 1; l++)
			{
				for (int k1 = par4 - 1; k1 <= par4 + byte0 + 1; k1++)
				{
					for (int j2 = par5 - j - 1; j2 <= par5 + j + 1; j2++)
					{
						Material material = par1World.GetBlockMaterial(l, k1, j2);

						if (k1 == par4 - 1 && !material.IsSolid())
						{
							return false;
						}

						if (k1 == par4 + byte0 + 1 && !material.IsSolid())
						{
							return false;
						}

						if ((l == par3 - i - 1 || l == par3 + i + 1 || j2 == par5 - j - 1 || j2 == par5 + j + 1) && k1 == par4 && par1World.IsAirBlock(l, k1, j2) && par1World.IsAirBlock(l, k1 + 1, j2))
						{
							k++;
						}
					}
				}
			}

			if (k < 1 || k > 5)
			{
				return false;
			}

			for (int i1 = par3 - i - 1; i1 <= par3 + i + 1; i1++)
			{
				for (int l1 = par4 + byte0; l1 >= par4 - 1; l1--)
				{
					for (int k2 = par5 - j - 1; k2 <= par5 + j + 1; k2++)
					{
						if (i1 == par3 - i - 1 || l1 == par4 - 1 || k2 == par5 - j - 1 || i1 == par3 + i + 1 || l1 == par4 + byte0 + 1 || k2 == par5 + j + 1)
						{
							if (l1 >= 0 && !par1World.GetBlockMaterial(i1, l1 - 1, k2).IsSolid())
							{
								par1World.SetBlockWithNotify(i1, l1, k2, 0);
								continue;
							}

							if (!par1World.GetBlockMaterial(i1, l1, k2).IsSolid())
							{
								continue;
							}

							if (l1 == par4 - 1 && par2Random.Next(4) != 0)
							{
								par1World.SetBlockWithNotify(i1, l1, k2, Block.CobblestoneMossy.BlockID);
							}
							else
							{
								par1World.SetBlockWithNotify(i1, l1, k2, Block.Cobblestone.BlockID);
							}
						}
						else
						{
							par1World.SetBlockWithNotify(i1, l1, k2, 0);
						}
					}
				}
			}

			for (int j1 = 0; j1 < 2; j1++)
			{
				label0:

				for (int i2 = 0; i2 < 3; i2++)
				{
					int l2 = (par3 + par2Random.Next(i * 2 + 1)) - i;
					int i3 = par4;
					int j3 = (par5 + par2Random.Next(j * 2 + 1)) - j;

					if (!par1World.IsAirBlock(l2, i3, j3))
					{
						continue;
					}

					int k3 = 0;

					if (par1World.GetBlockMaterial(l2 - 1, i3, j3).IsSolid())
					{
						k3++;
					}

					if (par1World.GetBlockMaterial(l2 + 1, i3, j3).IsSolid())
					{
						k3++;
					}

					if (par1World.GetBlockMaterial(l2, i3, j3 - 1).IsSolid())
					{
						k3++;
					}

					if (par1World.GetBlockMaterial(l2, i3, j3 + 1).IsSolid())
					{
						k3++;
					}

					if (k3 != 1)
					{
						continue;
					}

					par1World.SetBlockWithNotify(l2, i3, j3, Block.Chest.BlockID);
					TileEntityChest tileentitychest = (TileEntityChest)par1World.GetBlockTileEntity(l2, i3, j3);

					if (tileentitychest == null)
					{
						break;
					}

					int l3 = 0;

					do
					{
						if (l3 >= 8)
						{
							goto label0;
						}

						ItemStack itemstack = PickCheckLootItem(par2Random);

						if (itemstack != null)
						{
							tileentitychest.SetInventorySlotContents(par2Random.Next(tileentitychest.GetSizeInventory()), itemstack);
						}

						l3++;
					}
					while (true);
				}
			}

			par1World.SetBlockWithNotify(par3, par4, par5, Block.MobSpawner.BlockID);
			TileEntityMobSpawner tileentitymobspawner = (TileEntityMobSpawner)par1World.GetBlockTileEntity(par3, par4, par5);

			if (tileentitymobspawner != null)
			{
				tileentitymobspawner.SetMobID(PickMobSpawner(par2Random));
			}
			else
			{
				Console.Error.WriteLine((new StringBuilder()).Append("Failed to fetch mob spawner entity at (").Append(par3).Append(", ").Append(par4).Append(", ").Append(par5).Append(")").ToString());
			}

			return true;
		}

		/// <summary>
		/// Picks potentially a random item to add to a dungeon chest.
		/// </summary>
		private ItemStack PickCheckLootItem(Random par1Random)
		{
			int i = par1Random.Next(11);

			if (i == 0)
			{
				return new ItemStack(Item.Saddle);
			}

			if (i == 1)
			{
				return new ItemStack(Item.IngotIron, par1Random.Next(4) + 1);
			}

			if (i == 2)
			{
				return new ItemStack(Item.Bread);
			}

			if (i == 3)
			{
				return new ItemStack(Item.Wheat, par1Random.Next(4) + 1);
			}

			if (i == 4)
			{
				return new ItemStack(Item.Gunpowder, par1Random.Next(4) + 1);
			}

			if (i == 5)
			{
				return new ItemStack(Item.Silk, par1Random.Next(4) + 1);
			}

			if (i == 6)
			{
				return new ItemStack(Item.BucketEmpty);
			}

			if (i == 7 && par1Random.Next(100) == 0)
			{
				return new ItemStack(Item.AppleGold);
			}

			if (i == 8 && par1Random.Next(2) == 0)
			{
				return new ItemStack(Item.Redstone, par1Random.Next(4) + 1);
			}

			if (i == 9 && par1Random.Next(10) == 0)
			{
				return new ItemStack(Item.ItemsList[Item.Record13.ShiftedIndex + par1Random.Next(2)]);
			}

			if (i == 10)
			{
				return new ItemStack(Item.DyePowder, 1, 3);
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Randomly decides which spawner to use in a dungeon
		/// </summary>
		private string PickMobSpawner(Random par1Random)
		{
			int i = par1Random.Next(4);

			if (i == 0)
			{
				return "Skeleton";
			}

			if (i == 1)
			{
				return "Zombie";
			}

			if (i == 2)
			{
				return "Zombie";
			}

			if (i == 3)
			{
				return "Spider";
			}
			else
			{
				return "";
			}
		}
	}
}