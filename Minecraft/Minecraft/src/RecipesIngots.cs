namespace net.minecraft.src
{
    public class RecipesIngots
    {
        private object[][] recipeItems;

        public RecipesIngots()
		{
			recipeItems = ( new object[][]
            {
                new object[] { Block.BlockGold, new ItemStack(Item.IngotGold, 9) },
                new object[] { Block.BlockSteel, new ItemStack(Item.IngotIron, 9) },
                new object[] { Block.BlockDiamond, new ItemStack(Item.Diamond, 9) },
                new object[] { Block.BlockLapis, new ItemStack(Item.DyePowder, 9, 4) }}
            );
		}

        /// <summary>
        /// Adds the ingot recipes to the CraftingManager.
        /// </summary>
        //JAVA TO C# CONVERTER TODO TASK: The following line could not be converted:
        public void AddRecipes(CraftingManager par1CraftingManager)
        {
            for (int i = 0; i < recipeItems.Length; i++)
            {
                Block block = (Block)recipeItems[i][0];
                ItemStack itemstack = (ItemStack)recipeItems[i][1];
                par1CraftingManager.AddRecipe(new ItemStack(block), new object[] { "###", "###", "###", '#', itemstack });
                par1CraftingManager.AddRecipe(itemstack, new object[] { "#", '#', block });
            }

            par1CraftingManager.AddRecipe(new ItemStack(Item.IngotGold), new object[] { "###", "###", "###", '#', Item.GoldNugget });
            par1CraftingManager.AddRecipe(new ItemStack(Item.GoldNugget, 9), new object[] { "#", '#', Item.IngotGold });
        }
    }
}