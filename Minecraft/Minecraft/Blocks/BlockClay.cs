using System;

namespace net.minecraft.src
{
	public class BlockClay : Block
	{
		public BlockClay(int par1, int par2) : base(par1, par2, Material.Clay)
		{
		}

		/// <summary>
		/// Returns the ID of the items to drop on destruction.
		/// </summary>
		public override int IdDropped(int par1, Random par2Random, int par3)
		{
			return Item.Clay.ShiftedIndex;
		}

		/// <summary>
		/// Returns the quantity of items to drop on block destruction.
		/// </summary>
		public override int QuantityDropped(Random par1Random)
		{
			return 4;
		}
	}

}