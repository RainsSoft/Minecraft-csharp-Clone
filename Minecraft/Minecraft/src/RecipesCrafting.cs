namespace net.minecraft.src
{

	public class RecipesCrafting
	{
		public RecipesCrafting()
		{
		}

		/// <summary>
		/// Adds the crafting recipes to the CraftingManager.
		/// </summary>
		public virtual void AddRecipes(CraftingManager par1CraftingManager)
		{
			par1CraftingManager.AddRecipe(new ItemStack(Block.Chest), new object[] { "###", "# #", "###", '#', Block.Planks });
			par1CraftingManager.AddRecipe(new ItemStack(Block.StoneOvenIdle), new object[] { "###", "# #", "###", '#', Block.Cobblestone });
			par1CraftingManager.AddRecipe(new ItemStack(Block.Workbench), new object[] { "##", "##", '#', Block.Planks });
			par1CraftingManager.AddRecipe(new ItemStack(Block.SandStone), new object[] { "##", "##", '#', Block.Sand });
			par1CraftingManager.AddRecipe(new ItemStack(Block.SandStone, 4, 2), new object[] { "##", "##", '#', Block.SandStone });
			par1CraftingManager.AddRecipe(new ItemStack(Block.SandStone, 1, 1), new object[] { "#", "#", '#', new ItemStack(Block.StairSingle, 1, 1)
		});
			par1CraftingManager.AddRecipe(new ItemStack(Block.StoneBrick, 4), new object[] { "##", "##", '#', Block.Stone });
			par1CraftingManager.AddRecipe(new ItemStack(Block.FenceIron, 16), new object[] { "###", "###", '#', Item.IngotIron });
			par1CraftingManager.AddRecipe(new ItemStack(Block.ThinGlass, 16), new object[] { "###", "###", '#', Block.Glass });
			par1CraftingManager.AddRecipe(new ItemStack(Block.RedstoneLampIdle, 1), new object[] { " R ", "RGR", " R ", 'R', Item.Redstone, 'G', Block.GlowStone });
	}
}

}