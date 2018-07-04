namespace net.minecraft.src
{

	public class RecipesTools
	{
		private string[][] RecipePatterns = { new string[] { "XXX", " # ", " # " }, new string[] { "X", "#", "#" }, new string[] { "XX", "X#", " #" }, new string[] { "XX", " #", " #" } };
		private object[][] RecipeItems;

		public RecipesTools()
		{
			RecipeItems = (new object[][] { new object[] { Block.Planks, Block.Cobblestone, Item.IngotIron, Item.Diamond, Item.IngotGold }, new object[] { Item.PickaxeWood, Item.PickaxeStone, Item.PickaxeSteel, Item.PickaxeDiamond, Item.PickaxeGold }, new object[] { Item.ShovelWood, Item.ShovelStone, Item.ShovelSteel, Item.ShovelDiamond, Item.ShovelGold }, new object[] { Item.AxeWood, Item.AxeStone, Item.AxeSteel, Item.AxeDiamond, Item.AxeGold }, new object[] { Item.HoeWood, Item.HoeStone, Item.HoeSteel, Item.HoeDiamond, Item.HoeGold } });
		}

		/// <summary>
		/// Adds the tool recipes to the CraftingManager.
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

			par1CraftingManager.AddRecipe(new ItemStack(Item.Shears), new object[] { " #", "# ", '#', Item.IngotIron });
		}
	}

}