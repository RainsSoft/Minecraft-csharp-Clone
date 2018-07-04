namespace net.minecraft.src
{

	public class ItemSpade : ItemTool
	{
		private static Block[] BlocksEffectiveAgainst;

		public ItemSpade(int par1, ToolMaterial par2EnumToolMaterial) : base(par1, 1, par2EnumToolMaterial, BlocksEffectiveAgainst)
		{
		}

		/// <summary>
		/// Returns if the item (tool) can harvest results from the block type.
		/// </summary>
		public override bool CanHarvestBlock(Block par1Block)
		{
			if (par1Block == Block.Snow)
			{
				return true;
			}

			return par1Block == Block.BlockSnow;
		}

		static ItemSpade()
		{
			BlocksEffectiveAgainst = (new Block[] { Block.Grass, Block.Dirt, Block.Sand, Block.Gravel, Block.Snow, Block.BlockSnow, Block.BlockClay, Block.TilledField, Block.SlowSand, Block.Mycelium });
		}
	}

}