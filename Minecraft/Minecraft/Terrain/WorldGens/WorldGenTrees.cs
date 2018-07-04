using System;

namespace net.minecraft.src
{
	public class WorldGenTrees : WorldGenerator
	{
		private readonly int Field_48202_a;
		private readonly bool Field_48200_b;
		private readonly int Field_48201_c;
		private readonly int Field_48199_d;

		public WorldGenTrees(bool par1) : this(par1, 4, 0, 0, false)
		{
		}

		public WorldGenTrees(bool par1, int par2, int par3, int par4, bool par5) : base(par1)
		{
			Field_48202_a = par2;
			Field_48201_c = par3;
			Field_48199_d = par4;
			Field_48200_b = par5;
		}

		public override bool Generate(World par1World, Random par2Random, int par3, int par4, int par5)
		{
			int i = par2Random.Next(3) + Field_48202_a;
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

				for (int l = par3 - byte0; l <= par3 + byte0 && flag; l++)
				{
					for (int j1 = par5 - byte0; j1 <= par5 + byte0 && flag; j1++)
					{
						if (j >= 0 && j < 256)
						{
							int j2 = par1World.GetBlockId(l, j, j1);

							if (j2 != 0 && j2 != Block.Leaves.BlockID && j2 != Block.Grass.BlockID && j2 != Block.Dirt.BlockID && j2 != Block.Wood.BlockID)
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
			sbyte byte1 = 3;
			int i1 = 0;

			for (int k1 = (par4 - byte1) + i; k1 <= par4 + i; k1++)
			{
				int k2 = k1 - (par4 + i);
				int j3 = (i1 + 1) - k2 / 2;

				for (int l3 = par3 - j3; l3 <= par3 + j3; l3++)
				{
					int j4 = l3 - par3;

					for (int l4 = par5 - j3; l4 <= par5 + j3; l4++)
					{
						int i5 = l4 - par5;

						if ((Math.Abs(j4) != j3 || Math.Abs(i5) != j3 || par2Random.Next(2) != 0 && k2 != 0) && !Block.OpaqueCubeLookup[par1World.GetBlockId(l3, k1, l4)])
						{
							SetBlockAndMetadata(par1World, l3, k1, l4, Block.Leaves.BlockID, Field_48199_d);
						}
					}
				}
			}

			for (int l1 = 0; l1 < i; l1++)
			{
				int l2 = par1World.GetBlockId(par3, par4 + l1, par5);

				if (l2 != 0 && l2 != Block.Leaves.BlockID)
				{
					continue;
				}

				SetBlockAndMetadata(par1World, par3, par4 + l1, par5, Block.Wood.BlockID, Field_48201_c);

				if (!Field_48200_b || l1 <= 0)
				{
					continue;
				}

				if (par2Random.Next(3) > 0 && par1World.IsAirBlock(par3 - 1, par4 + l1, par5))
				{
					SetBlockAndMetadata(par1World, par3 - 1, par4 + l1, par5, Block.Vine.BlockID, 8);
				}

				if (par2Random.Next(3) > 0 && par1World.IsAirBlock(par3 + 1, par4 + l1, par5))
				{
					SetBlockAndMetadata(par1World, par3 + 1, par4 + l1, par5, Block.Vine.BlockID, 2);
				}

				if (par2Random.Next(3) > 0 && par1World.IsAirBlock(par3, par4 + l1, par5 - 1))
				{
					SetBlockAndMetadata(par1World, par3, par4 + l1, par5 - 1, Block.Vine.BlockID, 1);
				}

				if (par2Random.Next(3) > 0 && par1World.IsAirBlock(par3, par4 + l1, par5 + 1))
				{
					SetBlockAndMetadata(par1World, par3, par4 + l1, par5 + 1, Block.Vine.BlockID, 4);
				}
			}

			if (Field_48200_b)
			{
				for (int i2 = (par4 - 3) + i; i2 <= par4 + i; i2++)
				{
					int i3 = i2 - (par4 + i);
					int k3 = 2 - i3 / 2;

					for (int i4 = par3 - k3; i4 <= par3 + k3; i4++)
					{
						for (int k4 = par5 - k3; k4 <= par5 + k3; k4++)
						{
							if (par1World.GetBlockId(i4, i2, k4) != Block.Leaves.BlockID)
							{
								continue;
							}

							if (par2Random.Next(4) == 0 && par1World.GetBlockId(i4 - 1, i2, k4) == 0)
							{
								Func_48198_a(par1World, i4 - 1, i2, k4, 8);
							}

							if (par2Random.Next(4) == 0 && par1World.GetBlockId(i4 + 1, i2, k4) == 0)
							{
								Func_48198_a(par1World, i4 + 1, i2, k4, 2);
							}

							if (par2Random.Next(4) == 0 && par1World.GetBlockId(i4, i2, k4 - 1) == 0)
							{
								Func_48198_a(par1World, i4, i2, k4 - 1, 1);
							}

							if (par2Random.Next(4) == 0 && par1World.GetBlockId(i4, i2, k4 + 1) == 0)
							{
								Func_48198_a(par1World, i4, i2, k4 + 1, 4);
							}
						}
					}
				}
			}

			return true;
		}

		private void Func_48198_a(World par1World, int par2, int par3, int par4, int par5)
		{
			SetBlockAndMetadata(par1World, par2, par3, par4, Block.Vine.BlockID, par5);

			for (int i = 4; par1World.GetBlockId(par2, --par3, par4) == 0 && i > 0; i--)
			{
				SetBlockAndMetadata(par1World, par2, par3, par4, Block.Vine.BlockID, par5);
			}
		}
	}
}