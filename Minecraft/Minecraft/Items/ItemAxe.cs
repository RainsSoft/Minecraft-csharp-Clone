namespace net.minecraft.src
{
	public class ItemAxe : ItemTool
	{
		private static Block[] BlocksEffectiveAgainst;

		public ItemAxe(int par1, ToolMaterial par2EnumToolMaterial) : base(par1, 3, par2EnumToolMaterial, BlocksEffectiveAgainst)
		{
		}

		/// <summary>
		/// Returns the strength of the stack against a given block. 1.0F base, (Quality+1)*2 if correct blocktype, 1.5F if
		/// sword
		/// </summary>
		public override float GetStrVsBlock(ItemStack par1ItemStack, Block par2Block)
		{
			if (par2Block != null && par2Block.BlockMaterial == Material.Wood)
			{
				return EfficiencyOnProperMaterial;
			}
			else
			{
				return base.GetStrVsBlock(par1ItemStack, par2Block);
			}
		}

		static ItemAxe()
		{
			BlocksEffectiveAgainst = (new Block[] { Block.Planks, Block.BookShelf, Block.Wood, Block.Chest, Block.StairDouble, Block.StairSingle, Block.Pumpkin, Block.PumpkinLantern });
		}
	}
}