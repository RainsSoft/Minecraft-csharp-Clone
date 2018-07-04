using System;

namespace net.minecraft.src
{
	public class WorldGenReed : WorldGenerator
	{
		public WorldGenReed()
		{
		}

		public override bool Generate(World par1World, Random par2Random, int par3, int par4, int par5)
		{
			for (int i = 0; i < 20; i++)
			{
				int j = (par3 + par2Random.Next(4)) - par2Random.Next(4);
				int k = par4;
				int l = (par5 + par2Random.Next(4)) - par2Random.Next(4);

				if (!par1World.IsAirBlock(j, k, l) || par1World.GetBlockMaterial(j - 1, k - 1, l) != Material.Water && par1World.GetBlockMaterial(j + 1, k - 1, l) != Material.Water && par1World.GetBlockMaterial(j, k - 1, l - 1) != Material.Water && par1World.GetBlockMaterial(j, k - 1, l + 1) != Material.Water)
				{
					continue;
				}

				int i1 = 2 + par2Random.Next(par2Random.Next(3) + 1);

				for (int j1 = 0; j1 < i1; j1++)
				{
					if (Block.Reed.CanBlockStay(par1World, j, k + j1, l))
					{
						par1World.SetBlock(j, k + j1, l, Block.Reed.BlockID);
					}
				}
			}

			return true;
		}
	}
}