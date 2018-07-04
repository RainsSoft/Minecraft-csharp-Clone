using System;

namespace net.minecraft.src
{
	public class WorldGenVines : WorldGenerator
	{
		public WorldGenVines()
		{
		}

		public override bool Generate(World par1World, Random par2Random, int par3, int par4, int par5)
		{
			int i = par3;
			int j = par5;
			label0:

			for (; par4 < 128; par4++)
			{
				if (par1World.IsAirBlock(par3, par4, par5))
				{
					int k = 2;

					do
					{
						if (k > 5)
						{
							goto label0;
						}

						if (Block.Vine.CanPlaceBlockOnSide(par1World, par3, par4, par5, k))
						{
							par1World.SetBlockAndMetadata(par3, par4, par5, Block.Vine.BlockID, 1 << Direction.VineGrowth[Facing.FaceToSide[k]]);
							goto label0;
						}

						k++;
					}
					while (true);
				}

				par3 = (i + par2Random.Next(4)) - par2Random.Next(4);
				par5 = (j + par2Random.Next(4)) - par2Random.Next(4);
			}

			return true;
		}
	}
}