namespace net.minecraft.src
{

	public class ItemSeeds : Item
	{
		/// <summary>
		/// the type of block this seed turns into (wheat or pumpkin stems for instance)
		/// </summary>
		private int BlockType;

		/// <summary>
		/// BlockID of the block the seeds can be planted on. </summary>
		private int SoilBlockID;

		public ItemSeeds(int par1, int par2, int par3) : base(par1)
		{
			BlockType = par2;
			SoilBlockID = par3;
		}

		/// <summary>
		/// Callback for item usage. If the item does something special on right clicking, he will have one of those. Return
		/// True if something happen and false if it don't. This is for ITEMS, not BLOCKS !
		/// </summary>
		public override bool OnItemUse(ItemStack par1ItemStack, EntityPlayer par2EntityPlayer, World par3World, int par4, int par5, int par6, int par7)
		{
			if (par7 != 1)
			{
				return false;
			}

			if (!par2EntityPlayer.CanPlayerEdit(par4, par5, par6) || !par2EntityPlayer.CanPlayerEdit(par4, par5 + 1, par6))
			{
				return false;
			}

			int i = par3World.GetBlockId(par4, par5, par6);

			if (i == SoilBlockID && par3World.IsAirBlock(par4, par5 + 1, par6))
			{
				par3World.SetBlockWithNotify(par4, par5 + 1, par6, BlockType);
				par1ItemStack.StackSize--;
				return true;
			}
			else
			{
				return false;
			}
		}
	}

}