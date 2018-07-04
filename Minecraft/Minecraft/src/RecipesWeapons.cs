namespace net.minecraft.src
{

	public class RecipesWeapons
	{
		private string[][] RecipePatterns = { new string[] { "X", "X", "#" } };
		private object[][] RecipeItems;

		public RecipesWeapons()
		{
			RecipeItems = (new object[][] { new object[] { Block.Planks, Block.Cobblestone, Item.IngotIron, Item.Diamond, Item.IngotGold }, new object[] { Item.SwordWood, Item.SwordStone, Item.SwordSteel, Item.SwordDiamond, Item.SwordGold } });
		}

		/// <summary>
		/// Adds the weapon recipes to the CraftingManager.
		/// </summary>
		public virtual void AddRecipes(CraftingManager par1CraftingManager)
		{
			for (int i = 0; i < RecipeItems[0].Length; i++)
			{
				object obj = RecipeItems[0][i];

				for (int j = 0; j < RecipeItems.Length - 1; j++)
				{
					Item item = (Item)RecipeItems[j + 1][i];
					par1CraftingManager.AddRecipe(new ItemStack(item), new object[] { RecipePatterns[j], '#', Item.Stick, 'X', obj });
				}
			}

			par1CraftingManager.AddRecipe(new ItemStack(Item.Bow, 1), new object[] { " #X", "# X", " #X", 'X', Item.Silk, '#', Item.Stick });
			par1CraftingManager.AddRecipe(new ItemStack(Item.Arrow, 4), new object[] { "X", "#", "Y", 'Y', Item.Feather, 'X', Item.Flint, '#', Item.Stick });
		}
	}

}