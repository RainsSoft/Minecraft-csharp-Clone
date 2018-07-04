using System;

namespace net.minecraft.src
{
	public class WorldGenTaiga1 : WorldGenerator
	{
		public WorldGenTaiga1()
		{
		}

		public override bool Generate(World par1World, Random par2Random, int par3, int par4, int par5)
		{
			int i = par2Random.Next(5) + 7;
			int j = i - par2Random.Next(2) - 3;
			int k = i - j;
			int l = 1 + par2Random.Next(k + 1);
			bool flag = true;

			if (par4 < 1 || par4 + i + 1 > 128)
			{
				return false;
			}

			for (int i1 = par4; i1 <= par4 + 1 + i && flag; i1++)
			{
				int k1 = 1;

				if (i1 - par4 < j)
				{
					k1 = 0;
				}
				else
				{
					k1 = l;
				}

				for (int i2 = par3 - k1; i2 <= par3 + k1 && flag; i2++)
				{
					for (int l2 = par5 - k1; l2 <= par5 + k1 && flag; l2++)
					{
						if (i1 >= 0 && i1 < 128)
						{
							int k3 = par1World.GetBlockId(i2, i1, l2);

							if (k3 != 0 && k3 != Block.Leaves.BlockID)
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

			int j1 = par1World.GetBlockId(par3, par4 - 1, par5);

			if (j1 != Block.Grass.BlockID && j1 != Block.Dirt.BlockID || par4 >= 128 - i - 1)
			{
				return false;
			}

			Func_50073_a(par1World, par3, par4 - 1, par5, Block.Dirt.BlockID);
			int l1 = 0;

			for (int j2 = par4 + i; j2 >= par4 + j; j2--)
			{
				for (int i3 = par3 - l1; i3 <= par3 + l1; i3++)
				{
					int l3 = i3 - par3;

					for (int i4 = par5 - l1; i4 <= par5 + l1; i4++)
					{
						int j4 = i4 - par5;

						if ((Math.Abs(l3) != l1 || Math.Abs(j4) != l1 || l1 <= 0) && !Block.OpaqueCubeLookup[par1World.GetBlockId(i3, j2, i4)])
						{
							SetBlockAndMetadata(par1World, i3, j2, i4, Block.Leaves.BlockID, 1);
						}
					}
				}

				if (l1 >= 1 && j2 == par4 + j + 1)
				{
					l1--;
					continue;
				}

				if (l1 < l)
				{
					l1++;
				}
			}

			for (int k2 = 0; k2 < i - 1; k2++)
			{
				int j3 = par1World.GetBlockId(par3, par4 + k2, par5);

				if (j3 == 0 || j3 == Block.Leaves.BlockID)
				{
					SetBlockAndMetadata(par1World, par3, par4 + k2, par5, Block.Wood.BlockID, 1);
				}
			}

			return true;
		}
	}

}