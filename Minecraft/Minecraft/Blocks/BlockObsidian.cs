using System;

namespace net.minecraft.src
{
	public class BlockObsidian : BlockStone
	{
		public BlockObsidian(int par1, int par2) : base(par1, par2)
		{
		}

		/// <summary>
		/// Returns the quantity of items to drop on block destruction.
		/// </summary>
		public override int QuantityDropped(Random par1Random)
		{
			return 1;
		}

		/// <summary>
		/// Returns the ID of the items to drop on destruction.
		/// </summary>
		public override int IdDropped(int par1, Random par2Random, int par3)
		{
			return Block.Obsidian.BlockID;
		}
	}

}