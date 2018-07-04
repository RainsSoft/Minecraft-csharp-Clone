using System;

namespace net.minecraft.src
{
	public class WorldGenCactus : WorldGenerator
	{
		public WorldGenCactus()
		{
		}

		public override bool Generate(World par1World, Random par2Random, int par3, int par4, int par5)
		{
			for (int i = 0; i < 10; i++)
			{
				int j = (par3 + par2Random.Next(8)) - par2Random.Next(8);
				int k = (par4 + par2Random.Next(4)) - par2Random.Next(4);
				int l = (par5 + par2Random.Next(8)) - par2Random.Next(8);

				if (!par1World.IsAirBlock(j, k, l))
				{
					continue;
				}

				int i1 = 1 + par2Random.Next(par2Random.Next(3) + 1);

				for (int j1 = 0; j1 < i1; j1++)
				{
					if (Block.Cactus.CanBlockStay(par1World, j, k + j1, l))
					{
						par1World.SetBlock(j, k + j1, l, Block.Cactus.BlockID);
					}
				}
			}

			return true;
		}
	}
}