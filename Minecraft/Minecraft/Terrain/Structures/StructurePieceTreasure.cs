namespace net.minecraft.src
{

	public class StructurePieceTreasure : WeightedRandomChoice
	{
		/// <summary>
		/// The ID for this treasure item </summary>
		public int ItemID;

		/// <summary>
		/// The metadata to be used when creating the treasure item. </summary>
		public int ItemMetadata;

		/// <summary>
		/// This is how many items can be in each stack at minimun </summary>
		public int MinItemStack;

		/// <summary>
		/// This is how many items can be max in the itemstack </summary>
		public int MaxItemStack;

		public StructurePieceTreasure(int par1, int par2, int par3, int par4, int par5) : base(par5)
		{
			ItemID = par1;
			ItemMetadata = par2;
			MinItemStack = par3;
			MaxItemStack = par4;
		}
	}

}