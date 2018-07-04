using System;

namespace net.minecraft.src
{
	public class BlockBookshelf : Block
	{
		public BlockBookshelf(int par1, int par2) : base(par1, par2, Material.Wood)
		{
		}

		/// <summary>
		/// Returns the block texture based on the side being looked at.  Args: side
		/// </summary>
		public override int GetBlockTextureFromSide(int par1)
		{
			if (par1 <= 1)
			{
				return 4;
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
			return 3;
		}

		/// <summary>
		/// Returns the ID of the items to drop on destruction.
		/// </summary>
		public override int IdDropped(int par1, Random par2Random, int par3)
		{
			return Item.Book.ShiftedIndex;
		}
	}

}