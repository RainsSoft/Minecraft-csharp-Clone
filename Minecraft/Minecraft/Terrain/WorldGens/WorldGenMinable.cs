using System;

namespace net.minecraft.src
{
	public class WorldGenMinable : WorldGenerator
	{
		/// <summary>
		/// The block ID of the ore to be placed using this generator. </summary>
		private int MinableBlockId;

		/// <summary>
		/// The number of blocks to generate. </summary>
		private int NumberOfBlocks;

		public WorldGenMinable(int par1, int par2)
		{
			MinableBlockId = par1;
			NumberOfBlocks = par2;
		}

		public override bool Generate(World par1World, Random par2Random, int par3, int par4, int par5)
		{
			float f = par2Random.NextFloat() * (float)Math.PI;
			double d = (float)(par3 + 8) + (MathHelper2.Sin(f) * (float)NumberOfBlocks) / 8F;
			double d1 = (float)(par3 + 8) - (MathHelper2.Sin(f) * (float)NumberOfBlocks) / 8F;
			double d2 = (float)(par5 + 8) + (MathHelper2.Cos(f) * (float)NumberOfBlocks) / 8F;
			double d3 = (float)(par5 + 8) - (MathHelper2.Cos(f) * (float)NumberOfBlocks) / 8F;
			double d4 = (par4 + par2Random.Next(3)) - 2;
			double d5 = (par4 + par2Random.Next(3)) - 2;

			for (int i = 0; i <= NumberOfBlocks; i++)
			{
				double d6 = d + ((d1 - d) * (double)i) / (double)NumberOfBlocks;
				double d7 = d4 + ((d5 - d4) * (double)i) / (double)NumberOfBlocks;
				double d8 = d2 + ((d3 - d2) * (double)i) / (double)NumberOfBlocks;
				double d9 = (par2Random.NextDouble() * (double)NumberOfBlocks) / 16D;
				double d10 = (double)(MathHelper2.Sin(((float)i * (float)Math.PI) / (float)NumberOfBlocks) + 1.0F) * d9 + 1.0D;
				double d11 = (double)(MathHelper2.Sin(((float)i * (float)Math.PI) / (float)NumberOfBlocks) + 1.0F) * d9 + 1.0D;
				int j = MathHelper2.Floor_double(d6 - d10 / 2D);
				int k = MathHelper2.Floor_double(d7 - d11 / 2D);
				int l = MathHelper2.Floor_double(d8 - d10 / 2D);
				int i1 = MathHelper2.Floor_double(d6 + d10 / 2D);
				int j1 = MathHelper2.Floor_double(d7 + d11 / 2D);
				int k1 = MathHelper2.Floor_double(d8 + d10 / 2D);

				for (int l1 = j; l1 <= i1; l1++)
				{
					double d12 = (((double)l1 + 0.5D) - d6) / (d10 / 2D);

					if (d12 * d12 >= 1.0D)
					{
						continue;
					}

					for (int i2 = k; i2 <= j1; i2++)
					{
						double d13 = (((double)i2 + 0.5D) - d7) / (d11 / 2D);

						if (d12 * d12 + d13 * d13 >= 1.0D)
						{
							continue;
						}

						for (int j2 = l; j2 <= k1; j2++)
						{
							double d14 = (((double)j2 + 0.5D) - d8) / (d10 / 2D);

							if (d12 * d12 + d13 * d13 + d14 * d14 < 1.0D && par1World.GetBlockId(l1, i2, j2) == Block.Stone.BlockID)
							{
								par1World.SetBlock(l1, i2, j2, MinableBlockId);
							}
						}
					}
				}
			}

			return true;
		}
	}
}