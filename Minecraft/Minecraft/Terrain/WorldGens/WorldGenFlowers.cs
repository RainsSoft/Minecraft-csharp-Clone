using System;

namespace net.minecraft.src
{
	public class WorldGenFlowers : WorldGenerator
	{
		/// <summary>
		/// The ID of the plant block used in this plant generator. </summary>
		private int PlantBlockId;

		public WorldGenFlowers(int par1)
		{
			PlantBlockId = par1;
		}

		public override bool Generate(World par1World, Random par2Random, int par3, int par4, int par5)
		{
			for (int i = 0; i < 64; i++)
			{
				int j = (par3 + par2Random.Next(8)) - par2Random.Next(8);
				int k = (par4 + par2Random.Next(4)) - par2Random.Next(4);
				int l = (par5 + par2Random.Next(8)) - par2Random.Next(8);

				if (par1World.IsAirBlock(j, k, l) && ((BlockFlower)Block.BlocksList[PlantBlockId]).CanBlockStay(par1World, j, k, l))
				{
					par1World.SetBlock(j, k, l, PlantBlockId);
				}
			}

			return true;
		}
	}

}