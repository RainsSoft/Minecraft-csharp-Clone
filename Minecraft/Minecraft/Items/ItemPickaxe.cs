namespace net.minecraft.src
{
	public class ItemPickaxe : ItemTool
	{
		private static Block[] BlocksEffectiveAgainst;

		public ItemPickaxe(int par1, ToolMaterial par2EnumToolMaterial) : base(par1, 2, par2EnumToolMaterial, BlocksEffectiveAgainst)
		{
		}

		/// <summary>
		/// Returns if the item (tool) can harvest results from the block type.
		/// </summary>
		public override bool CanHarvestBlock(Block par1Block)
		{
			if (par1Block == Block.Obsidian)
			{
				return ToolMaterial.HarvestLevel == 3;
			}

			if (par1Block == Block.BlockDiamond || par1Block == Block.OreDiamond)
			{
				return ToolMaterial.HarvestLevel >= 2;
			}

			if (par1Block == Block.BlockGold || par1Block == Block.OreGold)
			{
				return ToolMaterial.HarvestLevel >= 2;
			}

			if (par1Block == Block.BlockSteel || par1Block == Block.OreIron)
			{
				return ToolMaterial.HarvestLevel >= 1;
			}

			if (par1Block == Block.BlockLapis || par1Block == Block.OreLapis)
			{
				return ToolMaterial.HarvestLevel >= 1;
			}

			if (par1Block == Block.OreRedstone || par1Block == Block.OreRedstoneGlowing)
			{
				return ToolMaterial.HarvestLevel >= 2;
			}

			if (par1Block.BlockMaterial == Material.Rock)
			{
				return true;
			}

			return par1Block.BlockMaterial == Material.Iron;
		}

		/// <summary>
		/// Returns the strength of the stack against a given block. 1.0F base, (Quality+1)*2 if correct blocktype, 1.5F if
		/// sword
		/// </summary>
		public override float GetStrVsBlock(ItemStack par1ItemStack, Block par2Block)
		{
			if (par2Block != null && (par2Block.BlockMaterial == Material.Iron || par2Block.BlockMaterial == Material.Rock))
			{
				return EfficiencyOnProperMaterial;
			}
			else
			{
				return base.GetStrVsBlock(par1ItemStack, par2Block);
			}
		}

		static ItemPickaxe()
		{
			BlocksEffectiveAgainst = (new Block[] { Block.Cobblestone, Block.StairDouble, Block.StairSingle, Block.Stone, Block.SandStone, Block.CobblestoneMossy, Block.OreIron, Block.BlockSteel, Block.OreCoal, Block.BlockGold, Block.OreGold, Block.OreDiamond, Block.BlockDiamond, Block.Ice, Block.Netherrack, Block.OreLapis, Block.BlockLapis, Block.OreRedstone, Block.OreRedstoneGlowing, Block.Rail, Block.RailDetector, Block.RailPowered });
		}
	}
}