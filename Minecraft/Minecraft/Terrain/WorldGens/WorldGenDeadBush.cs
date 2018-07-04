using System;

namespace net.minecraft.src
{
	public class WorldGenDeadBush : WorldGenerator
	{
		/// <summary>
		/// stores the ID for WorldGenDeadBush </summary>
		private int DeadBushID;

		public WorldGenDeadBush(int par1)
		{
			DeadBushID = par1;
		}

		public override bool Generate(World par1World, Random par2Random, int par3, int par4, int par5)
		{
			for (int i = 0; ((i = par1World.GetBlockId(par3, par4, par5)) == 0 || i == Block.Leaves.BlockID) && par4 > 0; par4--)
			{
			}

			for (int j = 0; j < 4; j++)
			{
				int k = (par3 + par2Random.Next(8)) - par2Random.Next(8);
				int l = (par4 + par2Random.Next(4)) - par2Random.Next(4);
				int i1 = (par5 + par2Random.Next(8)) - par2Random.Next(8);

				if (par1World.IsAirBlock(k, l, i1) && ((BlockFlower)Block.BlocksList[DeadBushID]).CanBlockStay(par1World, k, l, i1))
				{
					par1World.SetBlock(k, l, i1, DeadBushID);
				}
			}

			return true;
		}
	}

}