using System;

namespace net.minecraft.src
{
	public class BlockMushroomCap : Block
	{
		/// <summary>
		/// The mushroom type. 0 for brown, 1 for red. </summary>
		private int MushroomType;

		public BlockMushroomCap(int par1, Material par2Material, int par3, int par4) : base(par1, par3, par2Material)
		{
			MushroomType = par4;
		}

		/// <summary>
		/// From the specified side and block metadata retrieves the blocks texture. Args: side, metadata
		/// </summary>
		public override int GetBlockTextureFromSideAndMetadata(int par1, int par2)
		{
			if (par2 == 10 && par1 > 1)
			{
				return BlockIndexInTexture - 1;
			}

			if (par2 >= 1 && par2 <= 9 && par1 == 1)
			{
				return BlockIndexInTexture - 16 - MushroomType;
			}

			if (par2 >= 1 && par2 <= 3 && par1 == 2)
			{
				return BlockIndexInTexture - 16 - MushroomType;
			}

			if (par2 >= 7 && par2 <= 9 && par1 == 3)
			{
				return BlockIndexInTexture - 16 - MushroomType;
			}

			if ((par2 == 1 || par2 == 4 || par2 == 7) && par1 == 4)
			{
				return BlockIndexInTexture - 16 - MushroomType;
			}

			if ((par2 == 3 || par2 == 6 || par2 == 9) && par1 == 5)
			{
				return BlockIndexInTexture - 16 - MushroomType;
			}

			if (par2 == 14)
			{
				return BlockIndexInTexture - 16 - MushroomType;
			}

			if (par2 == 15)
			{
				return BlockIndexInTexture - 1;
			}
			else
			{
				return BlockIndexInTexture;
			}
		}

		/// <summary>
		/// Returns the quantity of items to drop on block destruction.
		/// </summary>
		public override int QuantityDropped(Random par1Random)
		{
			int i = par1Random.Next(10) - 7;

			if (i < 0)
			{
				i = 0;
			}

			return i;
		}

		/// <summary>
		/// Returns the ID of the items to drop on destruction.
		/// </summary>
		public override int IdDropped(int par1, Random par2Random, int par3)
		{
			return Block.MushroomBrown.BlockID + MushroomType;
		}
	}

}