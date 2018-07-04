using System;

namespace net.minecraft.src
{
	public class WorldGenBigMushroom : WorldGenerator
	{
		/// <summary>
		/// The mushroom type. 0 for brown, 1 for red. </summary>
		private int MushroomType;

		public WorldGenBigMushroom(int par1) : base(true)
		{
			MushroomType = -1;
			MushroomType = par1;
		}

		public WorldGenBigMushroom() : base(false)
		{
			MushroomType = -1;
		}

		public override bool Generate(World par1World, Random par2Random, int par3, int par4, int par5)
		{
			int i = par2Random.Next(2);

			if (MushroomType >= 0)
			{
				i = MushroomType;
			}

			int j = par2Random.Next(3) + 4;
			bool flag = true;

			if (par4 < 1 || par4 + j + 1 >= 256)
			{
				return false;
			}

			for (int k = par4; k <= par4 + 1 + j; k++)
			{
				sbyte byte0 = 3;

				if (k == par4)
				{
					byte0 = 0;
				}

				for (int j1 = par3 - byte0; j1 <= par3 + byte0 && flag; j1++)
				{
					for (int i2 = par5 - byte0; i2 <= par5 + byte0 && flag; i2++)
					{
						if (k >= 0 && k < 256)
						{
							int l2 = par1World.GetBlockId(j1, k, i2);

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

			int l = par1World.GetBlockId(par3, par4 - 1, par5);

			if (l != Block.Dirt.BlockID && l != Block.Grass.BlockID && l != Block.Mycelium.BlockID)
			{
				return false;
			}

			if (!Block.MushroomBrown.CanPlaceBlockAt(par1World, par3, par4, par5))
			{
				return false;
			}

			SetBlockAndMetadata(par1World, par3, par4 - 1, par5, Block.Dirt.BlockID, 0);
			int i1 = par4 + j;

			if (i == 1)
			{
				i1 = (par4 + j) - 3;
			}

			for (int k1 = i1; k1 <= par4 + j; k1++)
			{
				int j2 = 1;

				if (k1 < par4 + j)
				{
					j2++;
				}

				if (i == 0)
				{
					j2 = 3;
				}

				for (int i3 = par3 - j2; i3 <= par3 + j2; i3++)
				{
					for (int j3 = par5 - j2; j3 <= par5 + j2; j3++)
					{
						int k3 = 5;

						if (i3 == par3 - j2)
						{
							k3--;
						}

						if (i3 == par3 + j2)
						{
							k3++;
						}

						if (j3 == par5 - j2)
						{
							k3 -= 3;
						}

						if (j3 == par5 + j2)
						{
							k3 += 3;
						}

						if (i == 0 || k1 < par4 + j)
						{
							if ((i3 == par3 - j2 || i3 == par3 + j2) && (j3 == par5 - j2 || j3 == par5 + j2))
							{
								continue;
							}

							if (i3 == par3 - (j2 - 1) && j3 == par5 - j2)
							{
								k3 = 1;
							}

							if (i3 == par3 - j2 && j3 == par5 - (j2 - 1))
							{
								k3 = 1;
							}

							if (i3 == par3 + (j2 - 1) && j3 == par5 - j2)
							{
								k3 = 3;
							}

							if (i3 == par3 + j2 && j3 == par5 - (j2 - 1))
							{
								k3 = 3;
							}

							if (i3 == par3 - (j2 - 1) && j3 == par5 + j2)
							{
								k3 = 7;
							}

							if (i3 == par3 - j2 && j3 == par5 + (j2 - 1))
							{
								k3 = 7;
							}

							if (i3 == par3 + (j2 - 1) && j3 == par5 + j2)
							{
								k3 = 9;
							}

							if (i3 == par3 + j2 && j3 == par5 + (j2 - 1))
							{
								k3 = 9;
							}
						}

						if (k3 == 5 && k1 < par4 + j)
						{
							k3 = 0;
						}

						if ((k3 != 0 || par4 >= (par4 + j) - 1) && !Block.OpaqueCubeLookup[par1World.GetBlockId(i3, k1, j3)])
						{
							SetBlockAndMetadata(par1World, i3, k1, j3, Block.MushroomCapBrown.BlockID + i, k3);
						}
					}
				}
			}

			for (int l1 = 0; l1 < j; l1++)
			{
				int k2 = par1World.GetBlockId(par3, par4 + l1, par5);

				if (!Block.OpaqueCubeLookup[k2])
				{
					SetBlockAndMetadata(par1World, par3, par4 + l1, par5, Block.MushroomCapBrown.BlockID + i, 10);
				}
			}

			return true;
		}
	}
}