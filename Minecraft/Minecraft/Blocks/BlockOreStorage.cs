namespace net.minecraft.src
{

	public class BlockOreStorage : Block
	{
		public BlockOreStorage(int par1, int par2) : base(par1, Material.Iron)
		{
			BlockIndexInTexture = par2;
		}

		/// <summary>
		/// Returns the block texture based on the side being looked at.  Args: side
		/// </summary>
		public override int GetBlockTextureFromSide(int par1)
		{
			return BlockIndexInTexture;
		}
	}

}