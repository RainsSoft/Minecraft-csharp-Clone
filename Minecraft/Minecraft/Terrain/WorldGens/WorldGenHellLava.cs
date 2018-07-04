using System;

namespace net.minecraft.src
{
	public class WorldGenHellLava : WorldGenerator
	{
		/// <summary>
		/// Stores ID for WorldGenHellLava </summary>
		private int HellLavaID;

		public WorldGenHellLava(int par1)
		{
			HellLavaID = par1;
		}

		public override bool Generate(World par1World, Random par2Random, int par3, int par4, int par5)
		{
			if (par1World.GetBlockId(par3, par4 + 1, par5) != Block.Netherrack.BlockID)
			{
				return false;
			}

			if (par1World.GetBlockId(par3, par4, par5) != 0 && par1World.GetBlockId(par3, par4, par5) != Block.Netherrack.BlockID)
			{
				return false;
			}

			int i = 0;

			if (par1World.GetBlockId(par3 - 1, par4, par5) == Block.Netherrack.BlockID)
			{
				i++;
			}

			if (par1World.GetBlockId(par3 + 1, par4, par5) == Block.Netherrack.BlockID)
			{
				i++;
			}

			if (par1World.GetBlockId(par3, par4, par5 - 1) == Block.Netherrack.BlockID)
			{
				i++;
			}

			if (par1World.GetBlockId(par3, par4, par5 + 1) == Block.Netherrack.BlockID)
			{
				i++;
			}

			if (par1World.GetBlockId(par3, par4 - 1, par5) == Block.Netherrack.BlockID)
			{
				i++;
			}

			int j = 0;

			if (par1World.IsAirBlock(par3 - 1, par4, par5))
			{
				j++;
			}

			if (par1World.IsAirBlock(par3 + 1, par4, par5))
			{
				j++;
			}

			if (par1World.IsAirBlock(par3, par4, par5 - 1))
			{
				j++;
			}

			if (par1World.IsAirBlock(par3, par4, par5 + 1))
			{
				j++;
			}

			if (par1World.IsAirBlock(par3, par4 - 1, par5))
			{
				j++;
			}

			if (i == 4 && j == 1)
			{
				par1World.SetBlockWithNotify(par3, par4, par5, HellLavaID);
				par1World.ScheduledUpdatesAreImmediate = true;
				Block.BlocksList[HellLavaID].UpdateTick(par1World, par3, par4, par5, par2Random);
				par1World.ScheduledUpdatesAreImmediate = false;
			}

			return true;
		}
	}
}