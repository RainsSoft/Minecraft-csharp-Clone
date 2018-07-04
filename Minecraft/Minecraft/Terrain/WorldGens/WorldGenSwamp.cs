using System;

namespace net.minecraft.src
{
	public class WorldGenSwamp : WorldGenerator
	{
		public WorldGenSwamp()
		{
		}

		public override bool Generate(World par1World, Random par2Random, int par3, int par4, int par5)
		{
			int i = par2Random.Next(4) + 5;

			for (; par1World.GetBlockMaterial(par3, par4 - 1, par5) == Material.Water; par4--)
			{
			}

			bool flag = true;

			if (par4 < 1 || par4 + i + 1 > 128)
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
					byte0 = 3;
				}

				for (int k1 = par3 - byte0; k1 <= par3 + byte0 && flag; k1++)
				{
					for (int k2 = par5 - byte0; k2 <= par5 + byte0 && flag; k2++)
					{
						if (j >= 0 && j < 128)
						{
							int j3 = par1World.GetBlockId(k1, j, k2);

							if (j3 == 0 || j3 == Block.Leaves.BlockID)
							{
								continue;
							}

							if (j3 == Block.WaterStill.BlockID || j3 == Block.WaterMoving.BlockID)
							{
								if (j > par4)
								{
									flag = false;
								}
							}
							else
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

			if (k != Block.Grass.BlockID && k != Block.Dirt.BlockID || par4 >= 128 - i - 1)
			{
				return false;
			}

			Func_50073_a(par1World, par3, par4 - 1, par5, Block.Dirt.BlockID);

			for (int l = (par4 - 3) + i; l <= par4 + i; l++)
			{
				int l1 = l - (par4 + i);
				int l2 = 2 - l1 / 2;

				for (int k3 = par3 - l2; k3 <= par3 + l2; k3++)
				{
					int i4 = k3 - par3;

					for (int k4 = par5 - l2; k4 <= par5 + l2; k4++)
					{
						int l4 = k4 - par5;

						if ((Math.Abs(i4) != l2 || Math.Abs(l4) != l2 || par2Random.Next(2) != 0 && l1 != 0) && !Block.OpaqueCubeLookup[par1World.GetBlockId(k3, l, k4)])
						{
							Func_50073_a(par1World, k3, l, k4, Block.Leaves.BlockID);
						}
					}
				}
			}

			for (int i1 = 0; i1 < i; i1++)
			{
				int i2 = par1World.GetBlockId(par3, par4 + i1, par5);

				if (i2 == 0 || i2 == Block.Leaves.BlockID || i2 == Block.WaterMoving.BlockID || i2 == Block.WaterStill.BlockID)
				{
					Func_50073_a(par1World, par3, par4 + i1, par5, Block.Wood.BlockID);
				}
			}

			for (int j1 = (par4 - 3) + i; j1 <= par4 + i; j1++)
			{
				int j2 = j1 - (par4 + i);
				int i3 = 2 - j2 / 2;

				for (int l3 = par3 - i3; l3 <= par3 + i3; l3++)
				{
					for (int j4 = par5 - i3; j4 <= par5 + i3; j4++)
					{
						if (par1World.GetBlockId(l3, j1, j4) != Block.Leaves.BlockID)
						{
							continue;
						}

						if (par2Random.Next(4) == 0 && par1World.GetBlockId(l3 - 1, j1, j4) == 0)
						{
							GenerateVines(par1World, l3 - 1, j1, j4, 8);
						}

						if (par2Random.Next(4) == 0 && par1World.GetBlockId(l3 + 1, j1, j4) == 0)
						{
							GenerateVines(par1World, l3 + 1, j1, j4, 2);
						}

						if (par2Random.Next(4) == 0 && par1World.GetBlockId(l3, j1, j4 - 1) == 0)
						{
							GenerateVines(par1World, l3, j1, j4 - 1, 1);
						}

						if (par2Random.Next(4) == 0 && par1World.GetBlockId(l3, j1, j4 + 1) == 0)
						{
							GenerateVines(par1World, l3, j1, j4 + 1, 4);
						}
					}
				}
			}

			return true;
		}

		/// <summary>
		/// Generates vines at the given position until it hits a block.
		/// </summary>
		private void GenerateVines(World par1World, int par2, int par3, int par4, int par5)
		{
			SetBlockAndMetadata(par1World, par2, par3, par4, Block.Vine.BlockID, par5);

			for (int i = 4; par1World.GetBlockId(par2, --par3, par4) == 0 && i > 0; i--)
			{
				SetBlockAndMetadata(par1World, par2, par3, par4, Block.Vine.BlockID, par5);
			}
		}
	}

}