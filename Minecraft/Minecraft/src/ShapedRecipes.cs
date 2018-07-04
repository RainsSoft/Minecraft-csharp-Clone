namespace net.minecraft.src
{

	public class ShapedRecipes : IRecipe
	{
		/// <summary>
		/// How many horizontal slots this recipe is wide. </summary>
		private int RecipeWidth;

		/// <summary>
		/// How many vertical slots this recipe uses. </summary>
		private int RecipeHeight;
		private ItemStack[] RecipeItems;

		/// <summary>
		/// Is the ItemStack that you get when craft the recipe. </summary>
		private ItemStack RecipeOutput;

		/// <summary>
		/// Is the itemID of the output item that you get when craft the recipe. </summary>
		public readonly int RecipeOutputItemID;

		public ShapedRecipes(int par1, int par2, ItemStack[] par3ArrayOfItemStack, ItemStack par4ItemStack)
		{
			RecipeOutputItemID = par4ItemStack.ItemID;
			RecipeWidth = par1;
			RecipeHeight = par2;
			RecipeItems = par3ArrayOfItemStack;
			RecipeOutput = par4ItemStack;
		}

		public virtual ItemStack GetRecipeOutput()
		{
			return RecipeOutput;
		}

		/// <summary>
		/// Used to check if a recipe matches current crafting inventory
		/// </summary>
		public virtual bool Matches(InventoryCrafting par1InventoryCrafting)
		{
			for (int i = 0; i <= 3 - RecipeWidth; i++)
			{
				for (int j = 0; j <= 3 - RecipeHeight; j++)
				{
					if (CheckMatch(par1InventoryCrafting, i, j, true))
					{
						return true;
					}

					if (CheckMatch(par1InventoryCrafting, i, j, false))
					{
						return true;
					}
				}
			}

			return false;
		}

		/// <summary>
		/// Checks if the region of a crafting inventory is match for the recipe.
		/// </summary>
		private bool CheckMatch(InventoryCrafting par1InventoryCrafting, int par2, int par3, bool par4)
		{
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					int k = i - par2;
					int l = j - par3;
					ItemStack itemstack = null;

					if (k >= 0 && l >= 0 && k < RecipeWidth && l < RecipeHeight)
					{
						if (par4)
						{
							itemstack = RecipeItems[(RecipeWidth - k - 1) + l * RecipeWidth];
						}
						else
						{
							itemstack = RecipeItems[k + l * RecipeWidth];
						}
					}

					ItemStack itemstack1 = par1InventoryCrafting.GetStackInRowAndColumn(i, j);

					if (itemstack1 == null && itemstack == null)
					{
						continue;
					}

					if (itemstack1 == null && itemstack != null || itemstack1 != null && itemstack == null)
					{
						return false;
					}

					if (itemstack.ItemID != itemstack1.ItemID)
					{
						return false;
					}

					if (itemstack.GetItemDamage() != -1 && itemstack.GetItemDamage() != itemstack1.GetItemDamage())
					{
						return false;
					}
				}
			}

			return true;
		}

		/// <summary>
		/// Returns an Item that is the result of this recipe
		/// </summary>
		public virtual ItemStack GetCraftingResult(InventoryCrafting par1InventoryCrafting)
		{
			return new ItemStack(RecipeOutput.ItemID, RecipeOutput.StackSize, RecipeOutput.GetItemDamage());
		}

		/// <summary>
		/// Returns the size of the recipe area
		/// </summary>
		public virtual int GetRecipeSize()
		{
			return RecipeWidth * RecipeHeight;
		}
	}

}