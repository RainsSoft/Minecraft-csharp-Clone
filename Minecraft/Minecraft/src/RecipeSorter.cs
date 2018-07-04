using System.Collections.Generic;

namespace net.minecraft.src
{
	class RecipeSorter : IComparer<IRecipe>
	{
		readonly CraftingManager CraftingManager;

		public RecipeSorter(CraftingManager par1CraftingManager)
		{
			CraftingManager = par1CraftingManager;
		}

		public virtual int CompareRecipes(IRecipe par1IRecipe, IRecipe par2IRecipe)
		{
			if ((par1IRecipe is ShapelessRecipes) && (par2IRecipe is ShapedRecipes))
			{
				return 1;
			}

			if ((par2IRecipe is ShapelessRecipes) && (par1IRecipe is ShapedRecipes))
			{
				return -1;
			}

			if (par2IRecipe.GetRecipeSize() < par1IRecipe.GetRecipeSize())
			{
				return -1;
			}

			return par2IRecipe.GetRecipeSize() <= par1IRecipe.GetRecipeSize() ? 0 : 1;
		}

        public virtual int Compare(IRecipe par1Obj, IRecipe par2Obj)
		{
			return CompareRecipes(par1Obj, par2Obj);
		}
	}
}