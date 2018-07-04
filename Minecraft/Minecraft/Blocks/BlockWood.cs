namespace net.minecraft.src
{

	public class BlockWood : Block
	{
		public BlockWood(int par1) : base(par1, 4, Material.Wood)
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
					return 4;

				case 1:
					return 198;

				case 2:
					return 214;

				case 3:
					return 199;
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