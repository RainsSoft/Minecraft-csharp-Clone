namespace net.minecraft.src
{
	public class ItemSapling : ItemBlock
	{
		public ItemSapling(int par1) : base(par1)
		{
			SetMaxDamage(0);
			SetHasSubtypes(true);
		}

		/// <summary>
		/// Returns the metadata of the block which this Item (ItemBlock) can place
		/// </summary>
		public override int GetMetadata(int par1)
		{
			return par1;
		}

		/// <summary>
		/// Gets an icon index based on an item's damage value
		/// </summary>
		public override int GetIconFromDamage(int par1)
		{
			return Block.Sapling.GetBlockTextureFromSideAndMetadata(0, par1);
		}
	}
}