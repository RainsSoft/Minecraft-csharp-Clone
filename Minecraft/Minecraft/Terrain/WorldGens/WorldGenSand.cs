using System;

namespace net.minecraft.src
{
	public class WorldGenSand : WorldGenerator
	{
		/// <summary>
		/// Stores ID for WorldGenSand </summary>
		private int SandID;

		/// <summary>
		/// The maximum radius used when generating a patch of blocks. </summary>
		private int Radius;

		public WorldGenSand(int par1, int par2)
		{
			SandID = par2;
			Radius = par1;
		}

		public override bool Generate(World par1World, Random par2Random, int par3, int par4, int par5)
		{
			if (par1World.GetBlockMaterial(par3, par4, par5) != Material.Water)
			{
				return false;
			}

			int i = par2Random.Next(Radius - 2) + 2;
			sbyte byte0 = 2;

			for (int j = par3 - i; j <= par3 + i; j++)
			{
				for (int k = par5 - i; k <= par5 + i; k++)
				{
					int l = j - par3;
					int i1 = k - par5;

					if (l * l + i1 * i1 > i * i)
					{
						continue;
					}

					for (int j1 = par4 - byte0; j1 <= par4 + byte0; j1++)
					{
						int k1 = par1World.GetBlockId(j, j1, k);

						if (k1 == Block.Dirt.BlockID || k1 == Block.Grass.BlockID)
						{
							par1World.SetBlock(j, j1, k, SandID);
						}
					}
				}
			}

			return true;
		}
	}

}