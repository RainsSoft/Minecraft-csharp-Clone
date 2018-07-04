using System;

namespace net.minecraft.src
{
	public class BlockSapling : BlockFlower
	{
        public BlockSapling(int par1, int par2)
            : base(par1, par2)
		{
			float f = 0.4F;
			SetBlockBounds(0.5F - f, 0.0F, 0.5F - f, 0.5F + f, f * 2.0F, 0.5F + f);
		}

		/// <summary>
		/// Ticks the block if it's been scheduled
		/// </summary>
		public override void UpdateTick(World par1World, int par2, int par3, int par4, Random par5Random)
		{
			if (par1World.IsRemote)
			{
				return;
			}

			base.UpdateTick(par1World, par2, par3, par4, par5Random);

			if (par1World.GetBlockLightValue(par2, par3 + 1, par4) >= 9 && par5Random.Next(7) == 0)
			{
				int i = par1World.GetBlockMetadata(par2, par3, par4);

				if ((i & 8) == 0)
				{
					par1World.SetBlockMetadataWithNotify(par2, par3, par4, i | 8);
				}
				else
				{
					GrowTree(par1World, par2, par3, par4, par5Random);
				}
			}
		}

		/// <summary>
		/// From the specified side and block metadata retrieves the blocks texture. Args: side, metadata
		/// </summary>
		public override int GetBlockTextureFromSideAndMetadata(int par1, int par2)
		{
			par2 &= 3;

			if (par2 == 1)
			{
				return 63;
			}

			if (par2 == 2)
			{
				return 79;
			}

			if (par2 == 3)
			{
				return 30;
			}
			else
			{
				return base.GetBlockTextureFromSideAndMetadata(par1, par2);
			}
		}

		/// <summary>
		/// Attempts to grow a sapling into a tree
		/// </summary>
		public virtual void GrowTree(World par1World, int par2, int par3, int par4, Random par5Random)
		{
			int i = par1World.GetBlockMetadata(par2, par3, par4) & 3;
			object obj = null;
			int j = 0;
			int k = 0;
			bool flag = false;

			if (i == 1)
			{
				obj = new WorldGenTaiga2(true);
			}
			else if (i == 2)
			{
				obj = new WorldGenForest(true);
			}
			else if (i == 3)
			{
				j = 0;

				do
				{
					if (j < -1)
					{
						break;
					}

					k = 0;

					do
					{
						if (k < -1)
						{
							break;
						}

						if (Func_50076_f(par1World, par2 + j, par3, par4 + k, 3) && Func_50076_f(par1World, par2 + j + 1, par3, par4 + k, 3) && Func_50076_f(par1World, par2 + j, par3, par4 + k + 1, 3) && Func_50076_f(par1World, par2 + j + 1, par3, par4 + k + 1, 3))
						{
							obj = new WorldGenHugeTrees(true, 10 + par5Random.Next(20), 3, 3);
							flag = true;
							break;
						}

						k--;
					}
					while (true);

					if (obj != null)
					{
						break;
					}

					j--;
				}
				while (true);

				if (obj == null)
				{
					j = k = 0;
					obj = new WorldGenTrees(true, 4 + par5Random.Next(7), 3, 3, false);
				}
			}
			else
			{
				obj = new WorldGenTrees(true);

				if (par5Random.Next(10) == 0)
				{
					obj = new WorldGenBigTree(true);
				}
			}

			if (flag)
			{
				par1World.SetBlock(par2 + j, par3, par4 + k, 0);
				par1World.SetBlock(par2 + j + 1, par3, par4 + k, 0);
				par1World.SetBlock(par2 + j, par3, par4 + k + 1, 0);
				par1World.SetBlock(par2 + j + 1, par3, par4 + k + 1, 0);
			}
			else
			{
				par1World.SetBlock(par2, par3, par4, 0);
			}

			if (!((WorldGenerator)(obj)).Generate(par1World, par5Random, par2 + j, par3, par4 + k))
			{
				if (flag)
				{
					par1World.SetBlockAndMetadata(par2 + j, par3, par4 + k, BlockID, i);
					par1World.SetBlockAndMetadata(par2 + j + 1, par3, par4 + k, BlockID, i);
					par1World.SetBlockAndMetadata(par2 + j, par3, par4 + k + 1, BlockID, i);
					par1World.SetBlockAndMetadata(par2 + j + 1, par3, par4 + k + 1, BlockID, i);
				}
				else
				{
					par1World.SetBlockAndMetadata(par2, par3, par4, BlockID, i);
				}
			}
		}

		public virtual bool Func_50076_f(World par1World, int par2, int par3, int par4, int par5)
		{
			return par1World.GetBlockId(par2, par3, par4) == BlockID && (par1World.GetBlockMetadata(par2, par3, par4) & 3) == par5;
		}

		/// <summary>
		/// Determines the damage on the item the block drops. Used in cloth and wood.
		/// </summary>
		protected override int DamageDropped(int par1)
		{
			return par1 & 3;
		}
	}

}