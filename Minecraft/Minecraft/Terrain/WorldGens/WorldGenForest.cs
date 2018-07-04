using System;

namespace net.minecraft.src
{
	public class WorldGenForest : WorldGenerator
	{
		public WorldGenForest(bool par1) : base(par1)
		{
		}

		public override bool Generate(World par1World, Random par2Random, int par3, int par4, int par5)
		{
			int i = par2Random.Next(3) + 5;
			bool flag = true;

			if (par4 < 1 || par4 + i + 1 > 256)
			{
				return false;
			}

			for (int j = par4; j <= par4 + 1 + i; j++)
			{
				sbyte byte0 = 1;

				if (j == par4)
				{
					byte0 = 0;
				}

				if (j >= (par4 + 1 + i) - 2)
				{
					byte0 = 2;
				}

				for (int j1 = par3 - byte0; j1 <= par3 + byte0 && flag; j1++)
				{
					for (int i2 = par5 - byte0; i2 <= par5 + byte0 && flag; i2++)
					{
						if (j >= 0 && j < 256)
						{
							int k2 = par1World.GetBlockId(j1, j, i2);

							if (k2 != 0 && k2 != Block.Leaves.BlockID)
							{
								flag = false;
							}
						}
						else
						{
							flag = false;
						}
					}
				}
			}

			if (!flag)
			{
				return false;
			}

			int k = par1World.GetBlockId(par3, par4 - 1, par5);

			if (k != Block.Grass.BlockID && k != Block.Dirt.BlockID || par4 >= 256 - i - 1)
			{
				return false;
			}

			Func_50073_a(par1World, par3, par4 - 1, par5, Block.Dirt.BlockID);

			for (int l = (par4 - 3) + i; l <= par4 + i; l++)
			{
				int k1 = l - (par4 + i);
				int j2 = 1 - k1 / 2;

				for (int l2 = par3 - j2; l2 <= par3 + j2; l2++)
				{
					int i3 = l2 - par3;

					for (int j3 = par5 - j2; j3 <= par5 + j2; j3++)
					{
						int k3 = j3 - par5;

						if ((Math.Abs(i3) != j2 || Math.Abs(k3) != j2 || par2Random.Next(2) != 0 && k1 != 0) && !Block.OpaqueCubeLookup[par1World.GetBlockId(l2, l, j3)])
						{
							SetBlockAndMetadata(par1World, l2, l, j3, Block.Leaves.BlockID, 2);
						}
					}
				}
			}

			for (int i1 = 0; i1 < i; i1++)
			{
				int l1 = par1World.GetBlockId(par3, par4 + i1, par5);

				if (l1 == 0 || l1 == Block.Leaves.BlockID)
				{
					SetBlockAndMetadata(par1World, par3, par4 + i1, par5, Block.Wood.BlockID, 2);
				}
			}

			return true;
		}
	}

}