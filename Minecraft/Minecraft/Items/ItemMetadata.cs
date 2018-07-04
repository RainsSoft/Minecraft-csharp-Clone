namespace net.minecraft.src
{

	public class ItemMetadata : ItemBlock
	{
		private Block BlockObj;

		public ItemMetadata(int par1, Block par2Block) : base(par1)
		{
			BlockObj = par2Block;
			SetMaxDamage(0);
			SetHasSubtypes(true);
		}

		/// <summary>
		/// Gets an icon index based on an item's damage value
		/// </summary>
		public override int GetIconFromDamage(int par1)
		{
			return BlockObj.GetBlockTextureFromSideAndMetadata(2, par1);
		}

		/// <summary>
		/// Returns the metadata of the block which this Item (ItemBlock) can place
		/// </summary>
		public override int GetMetadata(int par1)
		{
			return par1;
		}
	}

}