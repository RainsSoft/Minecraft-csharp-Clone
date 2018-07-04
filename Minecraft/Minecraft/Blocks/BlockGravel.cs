using System;

namespace net.minecraft.src
{
	public class BlockGravel : BlockSand
	{
		public BlockGravel(int par1, int par2) : base(par1, par2)
		{
		}

		/// <summary>
		/// Returns the ID of the items to drop on destruction.
		/// </summary>
		public override int IdDropped(int par1, Random par2Random, int par3)
		{
			if (par2Random.Next(10 - par3 * 3) == 0)
			{
				return Item.Flint.ShiftedIndex;
			}
			else
			{
				return BlockID;
			}
		}
	}

}