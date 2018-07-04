using System;

namespace net.minecraft.src
{
	public class WorldGenDesertWells : WorldGenerator
	{
		public WorldGenDesertWells()
		{
		}

		public override bool Generate(World par1World, Random par2Random, int par3, int par4, int par5)
		{
			for (; par1World.IsAirBlock(par3, par4, par5) && par4 > 2; par4--)
			{
			}

			int i = par1World.GetBlockId(par3, par4, par5);

			if (i != Block.Sand.BlockID)
			{
				return false;
			}

			for (int j = -2; j <= 2; j++)
			{
				for (int k1 = -2; k1 <= 2; k1++)
				{
					if (par1World.IsAirBlock(par3 + j, par4 - 1, par5 + k1) && par1World.IsAirBlock(par3 + j, par4 - 2, par5 + k1))
					{
						return false;
					}
				}
			}

			for (int k = -1; k <= 0; k++)
			{
				for (int l1 = -2; l1 <= 2; l1++)
				{
					for (int k2 = -2; k2 <= 2; k2++)
					{
						par1World.SetBlock(par3 + l1, par4 + k, par5 + k2, Block.SandStone.BlockID);
					}
				}
			}

			par1World.SetBlock(par3, par4, par5, Block.WaterMoving.BlockID);
			par1World.SetBlock(par3 - 1, par4, par5, Block.WaterMoving.BlockID);
			par1World.SetBlock(par3 + 1, par4, par5, Block.WaterMoving.BlockID);
			par1World.SetBlock(par3, par4, par5 - 1, Block.WaterMoving.BlockID);
			par1World.SetBlock(par3, par4, par5 + 1, Block.WaterMoving.BlockID);

			for (int l = -2; l <= 2; l++)
			{
				for (int i2 = -2; i2 <= 2; i2++)
				{
					if (l == -2 || l == 2 || i2 == -2 || i2 == 2)
					{
						par1World.SetBlock(par3 + l, par4 + 1, par5 + i2, Block.SandStone.BlockID);
					}
				}
			}

			par1World.SetBlockAndMetadata(par3 + 2, par4 + 1, par5, Block.StairSingle.BlockID, 1);
			par1World.SetBlockAndMetadata(par3 - 2, par4 + 1, par5, Block.StairSingle.BlockID, 1);
			par1World.SetBlockAndMetadata(par3, par4 + 1, par5 + 2, Block.StairSingle.BlockID, 1);
			par1World.SetBlockAndMetadata(par3, par4 + 1, par5 - 2, Block.StairSingle.BlockID, 1);

			for (int i1 = -1; i1 <= 1; i1++)
			{
				for (int j2 = -1; j2 <= 1; j2++)
				{
					if (i1 == 0 && j2 == 0)
					{
						par1World.SetBlock(par3 + i1, par4 + 4, par5 + j2, Block.SandStone.BlockID);
					}
					else
					{
						par1World.SetBlockAndMetadata(par3 + i1, par4 + 4, par5 + j2, Block.StairSingle.BlockID, 1);
					}
				}
			}

			for (int j1 = 1; j1 <= 3; j1++)
			{
				par1World.SetBlock(par3 - 1, par4 + j1, par5 - 1, Block.SandStone.BlockID);
				par1World.SetBlock(par3 - 1, par4 + j1, par5 + 1, Block.SandStone.BlockID);
				par1World.SetBlock(par3 + 1, par4 + j1, par5 - 1, Block.SandStone.BlockID);
				par1World.SetBlock(par3 + 1, par4 + j1, par5 + 1, Block.SandStone.BlockID);
			}

			return true;
		}
	}

}