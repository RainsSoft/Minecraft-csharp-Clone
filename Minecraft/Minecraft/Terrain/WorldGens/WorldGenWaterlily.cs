using System;

namespace net.minecraft.src
{
	public class WorldGenWaterlily : WorldGenerator
	{
		public WorldGenWaterlily()
		{
		}

		public override bool Generate(World par1World, Random par2Random, int par3, int par4, int par5)
		{
			for (int i = 0; i < 10; i++)
			{
				int j = (par3 + par2Random.Next(8)) - par2Random.Next(8);
				int k = (par4 + par2Random.Next(4)) - par2Random.Next(4);
				int l = (par5 + par2Random.Next(8)) - par2Random.Next(8);

				if (par1World.IsAirBlock(j, k, l) && Block.Waterlily.CanPlaceBlockAt(par1World, j, k, l))
				{
					par1World.SetBlock(j, k, l, Block.Waterlily.BlockID);
				}
			}

			return true;
		}
	}
}