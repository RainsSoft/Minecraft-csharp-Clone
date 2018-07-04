using System;

namespace net.minecraft.src
{
	public class WorldGenTaiga2 : WorldGenerator
	{
		public WorldGenTaiga2(bool par1) : base(par1)
		{
		}

		public override bool Generate(World par1World, Random par2Random, int par3, int par4, int par5)
		{
			int i = par2Random.Next(4) + 6;
			int j = 1 + par2Random.Next(2);
			int k = i - j;
			int l = 2 + par2Random.Next(2);
			bool flag = true;

			if (par4 < 1 || par4 + i + 1 > 256)
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
					for (int k2 = par5 - k1; k2 <= par5 + k1 && flag; k2++)
					{
						if (i1 >= 0 && i1 < 256)
						{
							int l2 = par1World.GetBlockId(i2, i1, k2);

							if (l2 != 0 && l2 != Block.Leaves.BlockID)
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

			if (j1 != Block.Grass.BlockID && j1 != Block.Dirt.BlockID || par4 >= 256 - i - 1)
			{
				return false;
			}

			Func_50073_a(par1World, par3, par4 - 1, par5, Block.Dirt.BlockID);
			int l1 = par2Random.Next(2);
			int j2 = 1;
			bool flag1 = false;

			for (int i3 = 0; i3 <= k; i3++)
			{
				int k3 = (par4 + i) - i3;

				for (int i4 = par3 - l1; i4 <= par3 + l1; i4++)
				{
					int k4 = i4 - par3;

					for (int l4 = par5 - l1; l4 <= par5 + l1; l4++)
					{
						int i5 = l4 - par5;

						if ((Math.Abs(k4) != l1 || Math.Abs(i5) != l1 || l1 <= 0) && !Block.OpaqueCubeLookup[par1World.GetBlockId(i4, k3, l4)])
						{
							SetBlockAndMetadata(par1World, i4, k3, l4, Block.Leaves.BlockID, 1);
						}
					}
				}

				if (l1 >= j2)
				{
					l1 = ((flag1) ? 1 : 0);
					flag1 = true;

					if (++j2 > l)
					{
						j2 = l;
					}
				}
				else
				{
					l1++;
				}
			}

			int j3 = par2Random.Next(3);

			for (int l3 = 0; l3 < i - j3; l3++)
			{
				int j4 = par1World.GetBlockId(par3, par4 + l3, par5);

				if (j4 == 0 || j4 == Block.Leaves.BlockID)
				{
					SetBlockAndMetadata(par1World, par3, par4 + l3, par5, Block.Wood.BlockID, 1);
				}
			}

			return true;
		}
	}

}