namespace net.minecraft.src
{

	public interface IRecipe
	{
		/// <summary>
		/// Used to check if a recipe matches current crafting inventory
		/// </summary>
		bool Matches(InventoryCrafting inventorycrafting);

		/// <summary>
		/// Returns an Item that is the result of this recipe
		/// </summary>
		ItemStack GetCraftingResult(InventoryCrafting inventorycrafting);

		/// <summary>
		/// Returns the size of the recipe area
		/// </summary>
		int GetRecipeSize();

		ItemStack GetRecipeOutput();
	}

}