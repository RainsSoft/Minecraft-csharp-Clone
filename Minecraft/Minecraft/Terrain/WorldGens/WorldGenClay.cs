using System;

namespace net.minecraft.src
{
	public class WorldGenClay : WorldGenerator
	{
		/// <summary>
		/// The block ID for clay. </summary>
		private int ClayBlockId;

		/// <summary>
		/// The number of blocks to generate. </summary>
		private int NumberOfBlocks;

		public WorldGenClay(int par1)
		{
			ClayBlockId = Block.BlockClay.BlockID;
			NumberOfBlocks = par1;
		}

		public override bool Generate(World par1World, Random par2Random, int par3, int par4, int par5)
		{
			if (par1World.GetBlockMaterial(par3, par4, par5) != Material.Water)
			{
				return false;
			}

			int i = par2Random.Next(NumberOfBlocks - 2) + 2;
			int j = 1;

			for (int k = par3 - i; k <= par3 + i; k++)
			{
				for (int l = par5 - i; l <= par5 + i; l++)
				{
					int i1 = k - par3;
					int j1 = l - par5;

					if (i1 * i1 + j1 * j1 > i * i)
					{
						continue;
					}

					for (int k1 = par4 - j; k1 <= par4 + j; k1++)
					{
						int l1 = par1World.GetBlockId(k, k1, l);

						if (l1 == Block.Dirt.BlockID || l1 == Block.BlockClay.BlockID)
						{
							par1World.SetBlock(k, k1, l, ClayBlockId);
						}
					}
				}
			}

			return true;
		}
	}

}