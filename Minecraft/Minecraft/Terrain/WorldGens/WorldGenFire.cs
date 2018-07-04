using System;

namespace net.minecraft.src
{
	public class WorldGenFire : WorldGenerator
	{
		public WorldGenFire()
		{
		}

		public override bool Generate(World par1World, Random par2Random, int par3, int par4, int par5)
		{
			for (int i = 0; i < 64; i++)
			{
				int j = (par3 + par2Random.Next(8)) - par2Random.Next(8);
				int k = (par4 + par2Random.Next(4)) - par2Random.Next(4);
				int l = (par5 + par2Random.Next(8)) - par2Random.Next(8);

				if (par1World.IsAirBlock(j, k, l) && par1World.GetBlockId(j, k - 1, l) == Block.Netherrack.BlockID)
				{
					par1World.SetBlockWithNotify(j, k, l, Block.Fire.BlockID);
				}
			}

			return true;
		}
	}

}