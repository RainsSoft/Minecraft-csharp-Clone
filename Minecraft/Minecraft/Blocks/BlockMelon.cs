using System;

namespace net.minecraft.src
{
	public class BlockMelon : Block
	{
        public BlockMelon(int par1)
            : base(par1, Material.Pumpkin)
		{
			BlockIndexInTexture = 136;
		}

		/// <summary>
		/// From the specified side and block metadata retrieves the blocks texture. Args: side, metadata
		/// </summary>
		public override int GetBlockTextureFromSideAndMetadata(int par1, int par2)
		{
			return par1 != 1 && par1 != 0 ? 136 : 137;
		}

		/// <summary>
		/// Returns the block texture based on the side being looked at.  Args: side
		/// </summary>
		public override int GetBlockTextureFromSide(int par1)
		{
			return par1 != 1 && par1 != 0 ? 136 : 137;
		}

		/// <summary>
		/// Returns the ID of the items to drop on destruction.
		/// </summary>
		public override int IdDropped(int par1, Random par2Random, int par3)
		{
			return Item.Melon.ShiftedIndex;
		}

		/// <summary>
		/// Returns the quantity of items to drop on block destruction.
		/// </summary>
		public override int QuantityDropped(Random par1Random)
		{
			return 3 + par1Random.Next(5);
		}

		/// <summary>
		/// Returns the usual quantity dropped by the block plus a bonus of 1 to 'i' (inclusive).
		/// </summary>
		public override int QuantityDroppedWithBonus(int par1, Random par2Random)
		{
			int i = QuantityDropped(par2Random) + par2Random.Next(1 + par1);

			if (i > 9)
			{
				i = 9;
			}

			return i;
		}
	}

}