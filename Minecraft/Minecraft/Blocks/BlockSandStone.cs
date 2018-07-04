namespace net.minecraft.src
{

	public class BlockSandStone : Block
	{
		public BlockSandStone(int par1) : base(par1, 192, Material.Rock)
		{
		}

		/// <summary>
		/// From the specified side and block metadata retrieves the blocks texture. Args: side, metadata
		/// </summary>
		public override int GetBlockTextureFromSideAndMetadata(int par1, int par2)
		{
			if (par1 == 1 || par1 == 0 && (par2 == 1 || par2 == 2))
			{
				return 176;
			}

			if (par1 == 0)
			{
				return 208;
			}

			if (par2 == 1)
			{
				return 229;
			}

			return par2 != 2 ? 192 : 230;
		}

		/// <summary>
		/// Returns the block texture based on the side being looked at.  Args: side
		/// </summary>
		public override int GetBlockTextureFromSide(int par1)
		{
			if (par1 == 1)
			{
				return BlockIndexInTexture - 16;
			}

			if (par1 == 0)
			{
				return BlockIndexInTexture + 16;
			}
			else
			{
				return BlockIndexInTexture;
			}
		}

		/// <summary>
		/// Determines the damage on the item the block drops. Used in cloth and wood.
		/// </summary>
		protected override int DamageDropped(int par1)
		{
			return par1;
		}
	}

}