using System;

namespace net.minecraft.src
{
	public class WorldGenGlowStone2 : WorldGenerator
	{
		public WorldGenGlowStone2()
		{
		}

		public override bool Generate(World par1World, Random par2Random, int par3, int par4, int par5)
		{
			if (!par1World.IsAirBlock(par3, par4, par5))
			{
				return false;
			}

			if (par1World.GetBlockId(par3, par4 + 1, par5) != Block.Netherrack.BlockID)
			{
				return false;
			}

			par1World.SetBlockWithNotify(par3, par4, par5, Block.GlowStone.BlockID);

			for (int i = 0; i < 1500; i++)
			{
				int j = (par3 + par2Random.Next(8)) - par2Random.Next(8);
				int k = par4 - par2Random.Next(12);
				int l = (par5 + par2Random.Next(8)) - par2Random.Next(8);

				if (par1World.GetBlockId(j, k, l) != 0)
				{
					continue;
				}

				int i1 = 0;

				for (int j1 = 0; j1 < 6; j1++)
				{
					int k1 = 0;

					if (j1 == 0)
					{
						k1 = par1World.GetBlockId(j - 1, k, l);
					}

					if (j1 == 1)
					{
						k1 = par1World.GetBlockId(j + 1, k, l);
					}

					if (j1 == 2)
					{
						k1 = par1World.GetBlockId(j, k - 1, l);
					}

					if (j1 == 3)
					{
						k1 = par1World.GetBlockId(j, k + 1, l);
					}

					if (j1 == 4)
					{
						k1 = par1World.GetBlockId(j, k, l - 1);
					}

					if (j1 == 5)
					{
						k1 = par1World.GetBlockId(j, k, l + 1);
					}

					if (k1 == Block.GlowStone.BlockID)
					{
						i1++;
					}
				}

				if (i1 == 1)
				{
					par1World.SetBlockWithNotify(j, k, l, Block.GlowStone.BlockID);
				}
			}

			return true;
		}
	}

}