using System;

namespace net.minecraft.src
{
	public class WorldGenHugeTrees : WorldGenerator
	{
		private readonly int Field_48195_a;

		/// <summary>
		/// Sets the metadata for the wood blocks used </summary>
		private readonly int WoodMetadata;

		/// <summary>
		/// Sets the metadata for the leaves used in huge trees </summary>
		private readonly int LeavesMetadata;

		public WorldGenHugeTrees(bool par1, int par2, int par3, int par4) : base(par1)
		{
			Field_48195_a = par2;
			WoodMetadata = par3;
			LeavesMetadata = par4;
		}

		public override bool Generate(World par1World, Random par2Random, int par3, int par4, int par5)
		{
			int i = par2Random.Next(3) + Field_48195_a;
			bool flag = true;

			if (par4 < 1 || par4 + i + 1 > 256)
			{
				return false;
			}

			for (int j = par4; j <= par4 + 1 + i; j++)
			{
				sbyte byte0 = 2;

				if (j == par4)
				{
					byte0 = 1;
				}

				if (j >= (par4 + 1 + i) - 2)
				{
					byte0 = 2;
				}

				for (int i1 = par3 - byte0; i1 <= par3 + byte0 && flag; i1++)
				{
					for (int k1 = par5 - byte0; k1 <= par5 + byte0 && flag; k1++)
					{
						if (j >= 0 && j < 256)
						{
							int k2 = par1World.GetBlockId(i1, j, k1);

							if (k2 != 0 && k2 != Block.Leaves.BlockID && k2 != Block.Grass.BlockID && k2 != Block.Dirt.BlockID && k2 != Block.Wood.BlockID && k2 != Block.Sapling.BlockID)
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

			par1World.SetBlock(par3, par4 - 1, par5, Block.Dirt.BlockID);
			par1World.SetBlock(par3 + 1, par4 - 1, par5, Block.Dirt.BlockID);
			par1World.SetBlock(par3, par4 - 1, par5 + 1, Block.Dirt.BlockID);
			par1World.SetBlock(par3 + 1, par4 - 1, par5 + 1, Block.Dirt.BlockID);
			Func_48192_a(par1World, par3, par5, par4 + i, 2, par2Random);

			for (int l = (par4 + i) - 2 - par2Random.Next(4); l > par4 + i / 2; l -= 2 + par2Random.Next(4))
			{
				float f = par2Random.NextFloat() * (float)Math.PI * 2.0F;
				int l1 = par3 + (int)(0.5F + MathHelper2.Cos(f) * 4F);
				int l2 = par5 + (int)(0.5F + MathHelper2.Sin(f) * 4F);
				Func_48192_a(par1World, l1, l2, l, 0, par2Random);

				for (int j3 = 0; j3 < 5; j3++)
				{
					int i2 = par3 + (int)(1.5F + MathHelper2.Cos(f) * (float)j3);
					int i3 = par5 + (int)(1.5F + MathHelper2.Sin(f) * (float)j3);
					SetBlockAndMetadata(par1World, i2, (l - 3) + j3 / 2, i3, Block.Wood.BlockID, WoodMetadata);
				}
			}

			for (int j1 = 0; j1 < i; j1++)
			{
				int j2 = par1World.GetBlockId(par3, par4 + j1, par5);

				if (j2 == 0 || j2 == Block.Leaves.BlockID)
				{
					SetBlockAndMetadata(par1World, par3, par4 + j1, par5, Block.Wood.BlockID, WoodMetadata);

					if (j1 > 0)
					{
						if (par2Random.Next(3) > 0 && par1World.IsAirBlock(par3 - 1, par4 + j1, par5))
						{
							SetBlockAndMetadata(par1World, par3 - 1, par4 + j1, par5, Block.Vine.BlockID, 8);
						}

						if (par2Random.Next(3) > 0 && par1World.IsAirBlock(par3, par4 + j1, par5 - 1))
						{
							SetBlockAndMetadata(par1World, par3, par4 + j1, par5 - 1, Block.Vine.BlockID, 1);
						}
					}
				}

				if (j1 >= i - 1)
				{
					continue;
				}

				j2 = par1World.GetBlockId(par3 + 1, par4 + j1, par5);

				if (j2 == 0 || j2 == Block.Leaves.BlockID)
				{
					SetBlockAndMetadata(par1World, par3 + 1, par4 + j1, par5, Block.Wood.BlockID, WoodMetadata);

					if (j1 > 0)
					{
						if (par2Random.Next(3) > 0 && par1World.IsAirBlock(par3 + 2, par4 + j1, par5))
						{
							SetBlockAndMetadata(par1World, par3 + 2, par4 + j1, par5, Block.Vine.BlockID, 2);
						}

						if (par2Random.Next(3) > 0 && par1World.IsAirBlock(par3 + 1, par4 + j1, par5 - 1))
						{
							SetBlockAndMetadata(par1World, par3 + 1, par4 + j1, par5 - 1, Block.Vine.BlockID, 1);
						}
					}
				}

				j2 = par1World.GetBlockId(par3 + 1, par4 + j1, par5 + 1);

				if (j2 == 0 || j2 == Block.Leaves.BlockID)
				{
					SetBlockAndMetadata(par1World, par3 + 1, par4 + j1, par5 + 1, Block.Wood.BlockID, WoodMetadata);

					if (j1 > 0)
					{
						if (par2Random.Next(3) > 0 && par1World.IsAirBlock(par3 + 2, par4 + j1, par5 + 1))
						{
							SetBlockAndMetadata(par1World, par3 + 2, par4 + j1, par5 + 1, Block.Vine.BlockID, 2);
						}

						if (par2Random.Next(3) > 0 && par1World.IsAirBlock(par3 + 1, par4 + j1, par5 + 2))
						{
							SetBlockAndMetadata(par1World, par3 + 1, par4 + j1, par5 + 2, Block.Vine.BlockID, 4);
						}
					}
				}

				j2 = par1World.GetBlockId(par3, par4 + j1, par5 + 1);

				if (j2 != 0 && j2 != Block.Leaves.BlockID)
				{
					continue;
				}

				SetBlockAndMetadata(par1World, par3, par4 + j1, par5 + 1, Block.Wood.BlockID, WoodMetadata);

				if (j1 <= 0)
				{
					continue;
				}

				if (par2Random.Next(3) > 0 && par1World.IsAirBlock(par3 - 1, par4 + j1, par5 + 1))
				{
					SetBlockAndMetadata(par1World, par3 - 1, par4 + j1, par5 + 1, Block.Vine.BlockID, 8);
				}

				if (par2Random.Next(3) > 0 && par1World.IsAirBlock(par3, par4 + j1, par5 + 2))
				{
					SetBlockAndMetadata(par1World, par3, par4 + j1, par5 + 2, Block.Vine.BlockID, 4);
				}
			}

			return true;
		}

		private void Func_48192_a(World par1World, int par2, int par3, int par4, int par5, Random par6Random)
		{
			sbyte byte0 = 2;

			for (int i = par4 - byte0; i <= par4; i++)
			{
				int j = i - par4;
				int k = (par5 + 1) - j;

				for (int l = par2 - k; l <= par2 + k + 1; l++)
				{
					int i1 = l - par2;

					for (int j1 = par3 - k; j1 <= par3 + k + 1; j1++)
					{
						int k1 = j1 - par3;

						if ((i1 >= 0 || k1 >= 0 || i1 * i1 + k1 * k1 <= k * k) && (i1 <= 0 && k1 <= 0 || i1 * i1 + k1 * k1 <= (k + 1) * (k + 1)) && (par6Random.Next(4) != 0 || i1 * i1 + k1 * k1 <= (k - 1) * (k - 1)) && !Block.OpaqueCubeLookup[par1World.GetBlockId(l, i, j1)])
						{
							SetBlockAndMetadata(par1World, l, i, j1, Block.Leaves.BlockID, LeavesMetadata);
						}
					}
				}
			}
		}
	}

}