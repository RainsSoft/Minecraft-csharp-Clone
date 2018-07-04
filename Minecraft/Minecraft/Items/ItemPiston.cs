namespace net.minecraft.src
{

	public class ItemPiston : ItemBlock
	{
		public ItemPiston(int par1) : base(par1)
		{
		}

		/// <summary>
		/// Returns the metadata of the block which this Item (ItemBlock) can place
		/// </summary>
		public override int GetMetadata(int par1)
		{
			return 7;
		}
	}

}