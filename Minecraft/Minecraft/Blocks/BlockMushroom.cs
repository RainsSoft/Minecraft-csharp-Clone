using System;

namespace net.minecraft.src
{
	public class BlockMushroom : BlockFlower
	{
        public BlockMushroom(int par1, int par2)
            : base(par1, par2)
		{
			float f = 0.2F;
			SetBlockBounds(0.5F - f, 0.0F, 0.5F - f, 0.5F + f, f * 2.0F, 0.5F + f);
			SetTickRandomly(true);
		}

		/// <summary>
		/// Ticks the block if it's been scheduled
		/// </summary>
		public override void UpdateTick(World par1World, int par2, int par3, int par4, Random par5Random)
		{
			if (par5Random.Next(25) == 0)
			{
				sbyte byte0 = 4;
				int i = 5;

				for (int j = par2 - byte0; j <= par2 + byte0; j++)
				{
					for (int l = par4 - byte0; l <= par4 + byte0; l++)
					{
						for (int j1 = par3 - 1; j1 <= par3 + 1; j1++)
						{
							if (par1World.GetBlockId(j, j1, l) == BlockID && --i <= 0)
							{
								return;
							}
						}
					}
				}

				int k = (par2 + par5Random.Next(3)) - 1;
				int i1 = (par3 + par5Random.Next(2)) - par5Random.Next(2);
				int k1 = (par4 + par5Random.Next(3)) - 1;

				for (int l1 = 0; l1 < 4; l1++)
				{
					if (par1World.IsAirBlock(k, i1, k1) && CanBlockStay(par1World, k, i1, k1))
					{
						par2 = k;
						par3 = i1;
						par4 = k1;
					}

					k = (par2 + par5Random.Next(3)) - 1;
					i1 = (par3 + par5Random.Next(2)) - par5Random.Next(2);
					k1 = (par4 + par5Random.Next(3)) - 1;
				}

				if (par1World.IsAirBlock(k, i1, k1) && CanBlockStay(par1World, k, i1, k1))
				{
					par1World.SetBlockWithNotify(k, i1, k1, BlockID);
				}
			}
		}

		/// <summary>
		/// Checks to see if its valid to put this block at the specified coordinates. Args: world, x, y, z
		/// </summary>
		public override bool CanPlaceBlockAt(World par1World, int par2, int par3, int par4)
		{
			return base.CanPlaceBlockAt(par1World, par2, par3, par4) && CanBlockStay(par1World, par2, par3, par4);
		}

		/// <summary>
		/// Gets passed in the BlockID of the block below and supposed to return true if its allowed to grow on the type of
		/// BlockID passed in. Args: BlockID
		/// </summary>
		protected override bool CanThisPlantGrowOnThisBlockID(int par1)
		{
			return Block.OpaqueCubeLookup[par1];
		}

		/// <summary>
		/// Can this block stay at this position.  Similar to CanPlaceBlockAt except gets checked often with plants.
		/// </summary>
		public override bool CanBlockStay(World par1World, int par2, int par3, int par4)
		{
			if (par3 < 0 || par3 >= 256)
			{
				return false;
			}
			else
			{
				int i = par1World.GetBlockId(par2, par3 - 1, par4);
				return i == Block.Mycelium.BlockID || par1World.GetFullBlockLightValue(par2, par3, par4) < 13 && CanThisPlantGrowOnThisBlockID(i);
			}
		}

		/// <summary>
		/// Fertilize the mushroom.
		/// </summary>
		public virtual bool FertilizeMushroom(World par1World, int par2, int par3, int par4, Random par5Random)
		{
			int i = par1World.GetBlockMetadata(par2, par3, par4);
			par1World.SetBlock(par2, par3, par4, 0);
			WorldGenBigMushroom worldgenbigmushroom = null;

			if (BlockID == Block.MushroomBrown.BlockID)
			{
				worldgenbigmushroom = new WorldGenBigMushroom(0);
			}
			else if (BlockID == Block.MushroomRed.BlockID)
			{
				worldgenbigmushroom = new WorldGenBigMushroom(1);
			}

			if (worldgenbigmushroom == null || !worldgenbigmushroom.Generate(par1World, par5Random, par2, par3, par4))
			{
				par1World.SetBlockAndMetadata(par2, par3, par4, BlockID, i);
				return false;
			}
			else
			{
				return true;
			}
		}
	}
}