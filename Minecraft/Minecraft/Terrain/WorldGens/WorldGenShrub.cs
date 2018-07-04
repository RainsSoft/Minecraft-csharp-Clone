using System;

namespace net.minecraft.src
{
	public class WorldGenShrub : WorldGenerator
	{
		private int Field_48197_a;
		private int Field_48196_b;

		public WorldGenShrub(int par1, int par2)
		{
			Field_48196_b = par1;
			Field_48197_a = par2;
		}

		public override bool Generate(World par1World, Random par2Random, int par3, int par4, int par5)
		{
			for (int i = 0; ((i = par1World.GetBlockId(par3, par4, par5)) == 0 || i == Block.Leaves.BlockID) && par4 > 0; par4--)
			{
			}

			int j = par1World.GetBlockId(par3, par4, par5);

			if (j == Block.Dirt.BlockID || j == Block.Grass.BlockID)
			{
				par4++;
				SetBlockAndMetadata(par1World, par3, par4, par5, Block.Wood.BlockID, Field_48196_b);

				for (int k = par4; k <= par4 + 2; k++)
				{
					int l = k - par4;
					int i1 = 2 - l;

					for (int j1 = par3 - i1; j1 <= par3 + i1; j1++)
					{
						int k1 = j1 - par3;

						for (int l1 = par5 - i1; l1 <= par5 + i1; l1++)
						{
							int i2 = l1 - par5;

							if ((Math.Abs(k1) != i1 || Math.Abs(i2) != i1 || par2Random.Next(2) != 0) && !Block.OpaqueCubeLookup[par1World.GetBlockId(j1, k, l1)])
							{
								SetBlockAndMetadata(par1World, j1, k, l1, Block.Leaves.BlockID, Field_48197_a);
							}
						}
					}
				}
			}

			return true;
		}
	}

}