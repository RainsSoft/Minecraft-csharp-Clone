using System;

namespace net.minecraft.src
{
	public class WorldGenTallGrass : WorldGenerator
	{
		/// <summary>
		/// Stores ID for WorldGenTallGrass </summary>
		private int TallGrassID;
		private int TallGrassMetadata;

		public WorldGenTallGrass(int par1, int par2)
		{
			TallGrassID = par1;
			TallGrassMetadata = par2;
		}

		public override bool Generate(World par1World, Random par2Random, int par3, int par4, int par5)
		{
			for (int i = 0; ((i = par1World.GetBlockId(par3, par4, par5)) == 0 || i == Block.Leaves.BlockID) && par4 > 0; par4--)
			{
			}

			for (int j = 0; j < 128; j++)
			{
				int k = (par3 + par2Random.Next(8)) - par2Random.Next(8);
				int l = (par4 + par2Random.Next(4)) - par2Random.Next(4);
				int i1 = (par5 + par2Random.Next(8)) - par2Random.Next(8);

				if (par1World.IsAirBlock(k, l, i1) && ((BlockFlower)Block.BlocksList[TallGrassID]).CanBlockStay(par1World, k, l, i1))
				{
					par1World.SetBlockAndMetadata(k, l, i1, TallGrassID, TallGrassMetadata);
				}
			}

			return true;
		}
	}

}