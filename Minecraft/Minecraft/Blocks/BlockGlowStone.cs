using System;

namespace net.minecraft.src
{
	public class BlockGlowStone : Block
	{
		public BlockGlowStone(int par1, int par2, Material par3Material) : base(par1, par2, par3Material)
		{
		}

		/// <summary>
		/// Returns the usual quantity dropped by the block plus a bonus of 1 to 'i' (inclusive).
		/// </summary>
		public override int QuantityDroppedWithBonus(int par1, Random par2Random)
		{
			return MathHelper2.Clamp_int(QuantityDropped(par2Random) + par2Random.Next(par1 + 1), 1, 4);
		}

		/// <summary>
		/// Returns the quantity of items to drop on block destruction.
		/// </summary>
		public override int QuantityDropped(Random par1Random)
		{
			return 2 + par1Random.Next(3);
		}

		/// <summary>
		/// Returns the ID of the items to drop on destruction.
		/// </summary>
		public override int IdDropped(int par1, Random par2Random, int par3)
		{
			return Item.LightStoneDust.ShiftedIndex;
		}
	}

}