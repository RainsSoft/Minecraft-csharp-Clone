namespace net.minecraft.src
{

	public class BlockStoneBrick : Block
	{
		public BlockStoneBrick(int par1) : base(par1, 54, Material.Rock)
		{
		}

		/// <summary>
		/// From the specified side and block metadata retrieves the blocks texture. Args: side, metadata
		/// </summary>
		public override int GetBlockTextureFromSideAndMetadata(int par1, int par2)
		{
			switch (par2)
			{
				default:
					return 54;

				case 1:
					return 100;

				case 2:
					return 101;

				case 3:
					return 213;
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