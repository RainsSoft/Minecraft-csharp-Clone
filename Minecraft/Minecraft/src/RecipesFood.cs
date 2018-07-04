namespace net.minecraft.src
{

	public class RecipesFood
	{
		public RecipesFood()
		{
		}

		/// <summary>
		/// Adds the food recipes to the CraftingManager.
		/// </summary>
		public virtual void AddRecipes(CraftingManager par1CraftingManager)
		{
			par1CraftingManager.AddShapelessRecipe(new ItemStack(Item.BowlSoup), new object[] { Block.MushroomBrown, Block.MushroomRed, Item.BowlEmpty });
			par1CraftingManager.AddRecipe(new ItemStack(Item.Cookie, 8), new object[] { "#X#", 'X', new ItemStack(Item.DyePowder, 1, 3), '#', Item.Wheat });
			par1CraftingManager.AddRecipe(new ItemStack(Block.Melon), new object[] { "MMM", "MMM", "MMM", 'M', Item.Melon });
			par1CraftingManager.AddRecipe(new ItemStack(Item.MelonSeeds), new object[] { "M", 'M', Item.Melon });
			par1CraftingManager.AddRecipe(new ItemStack(Item.PumpkinSeeds, 4), new object[] { "M", 'M', Block.Pumpkin });
			par1CraftingManager.AddShapelessRecipe(new ItemStack(Item.FermentedSpiderEye), new object[] { Item.SpiderEye, Block.MushroomBrown, Item.Sugar });
			par1CraftingManager.AddShapelessRecipe(new ItemStack(Item.SpeckledMelon), new object[] { Item.Melon, Item.GoldNugget });
			par1CraftingManager.AddShapelessRecipe(new ItemStack(Item.BlazePowder, 2), new object[] { Item.BlazeRod });
			par1CraftingManager.AddShapelessRecipe(new ItemStack(Item.MagmaCream), new object[] { Item.BlazePowder, Item.SlimeBall });
		}
	}

}