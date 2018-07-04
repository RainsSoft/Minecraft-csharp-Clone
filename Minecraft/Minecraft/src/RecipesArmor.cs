namespace net.minecraft.src
{

	public class RecipesArmor
	{
		private string[][] RecipePatterns = { new string[] { "XXX", "X X" }, new string[] { "X X", "XXX", "XXX" }, new string[] { "XXX", "X X", "X X" }, new string[] { "X X", "X X" } };
		private object[][] RecipeItems;

		public RecipesArmor()
		{
			RecipeItems = (new object[][] { new object[] { Item.Leather, Block.Fire, Item.IngotIron, Item.Diamond, Item.IngotGold }, new object[] { Item.HelmetLeather, Item.HelmetChain, Item.HelmetSteel, Item.HelmetDiamond, Item.HelmetGold }, new object[] { Item.PlateLeather, Item.PlateChain, Item.PlateSteel, Item.PlateDiamond, Item.PlateGold }, new object[] { Item.LegsLeather, Item.LegsChain, Item.LegsSteel, Item.LegsDiamond, Item.LegsGold }, new object[] { Item.BootsLeather, Item.BootsChain, Item.BootsSteel, Item.BootsDiamond, Item.BootsGold } });
		}

		/// <summary>
		/// Adds the armor recipes to the CraftingManager.
		/// </summary>
		public virtual void AddRecipes(CraftingManager par1CraftingManager)
		{
			for (int i = 0; i < RecipeItems[0].Length; i++)
			{
				object obj = RecipeItems[0][i];

				for (int j = 0; j < RecipeItems.Length - 1; j++)
				{
					Item item = (Item)RecipeItems[j + 1][i];
					par1CraftingManager.AddRecipe(new ItemStack(item), new object[] { RecipePatterns[j], 'X', obj });
				}
			}
		}
	}

}