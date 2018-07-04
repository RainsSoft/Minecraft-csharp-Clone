namespace net.minecraft.src
{

	public class BlockCloth : Block
	{
		public BlockCloth() : base(35, 64, Material.Cloth)
		{
		}

		/// <summary>
		/// From the specified side and block metadata retrieves the blocks texture. Args: side, metadata
		/// </summary>
		public override int GetBlockTextureFromSideAndMetadata(int par1, int par2)
		{
			if (par2 == 0)
			{
				return BlockIndexInTexture;
			}
			else
			{
				par2 = ~(par2 & 0xf);
				return 113 + ((par2 & 8) >> 3) + (par2 & 7) * 16;
			}
		}

		/// <summary>
		/// Determines the damage on the item the block drops. Used in cloth and wood.
		/// </summary>
		protected override int DamageDropped(int par1)
		{
			return par1;
		}

		/// <summary>
		/// Takes a dye damage value and returns the block damage value to match
		/// </summary>
		public static int GetBlockFromDye(int par0)
		{
			return ~par0 & 0xf;
		}

		/// <summary>
		/// Takes a block damage value and returns the dye damage value to match
		/// </summary>
		public static int GetDyeFromBlock(int par0)
		{
			return ~par0 & 0xf;
		}
	}

}