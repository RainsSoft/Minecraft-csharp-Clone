using System;

namespace net.minecraft.src
{
	public class BlockOre : Block
	{
		public BlockOre(int par1, int par2) : base(par1, par2, Material.Rock)
		{
		}

		/// <summary>
		/// Returns the ID of the items to drop on destruction.
		/// </summary>
		public override int IdDropped(int par1, Random par2Random, int par3)
		{
			if (BlockID == Block.OreCoal.BlockID)
			{
				return Item.Coal.ShiftedIndex;
			}

			if (BlockID == Block.OreDiamond.BlockID)
			{
				return Item.Diamond.ShiftedIndex;
			}

			if (BlockID == Block.OreLapis.BlockID)
			{
				return Item.DyePowder.ShiftedIndex;
			}
			else
			{
				return BlockID;
			}
		}

		/// <summary>
		/// Returns the quantity of items to drop on block destruction.
		/// </summary>
		public override int QuantityDropped(Random par1Random)
		{
			if (BlockID == Block.OreLapis.BlockID)
			{
				return 4 + par1Random.Next(5);
			}
			else
			{
				return 1;
			}
		}

		/// <summary>
		/// Returns the usual quantity dropped by the block plus a bonus of 1 to 'i' (inclusive).
		/// </summary>
		public override int QuantityDroppedWithBonus(int par1, Random par2Random)
		{
			if (par1 > 0 && BlockID != IdDropped(0, par2Random, par1))
			{
				int i = par2Random.Next(par1 + 2) - 1;

				if (i < 0)
				{
					i = 0;
				}

				return QuantityDropped(par2Random) * (i + 1);
			}
			else
			{
				return QuantityDropped(par2Random);
			}
		}

		/// <summary>
		/// Determines the damage on the item the block drops. Used in cloth and wood.
		/// </summary>
		protected override int DamageDropped(int par1)
		{
			return BlockID != Block.OreLapis.BlockID ? 0 : 4;
		}
	}

}