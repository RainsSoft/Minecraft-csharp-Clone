using System.Collections.Generic;

namespace net.minecraft.src
{
	public class ShapelessRecipes : IRecipe
	{
		/// <summary>
		/// Is the ItemStack that you get when craft the recipe. </summary>
		private readonly ItemStack RecipeOutput;

		/// <summary>
		/// Is a List of ItemStack that composes the recipe. </summary>
        private readonly List<ItemStack> RecipeItems;

        public ShapelessRecipes(ItemStack par1ItemStack, List<ItemStack> par2List)
		{
			RecipeOutput = par1ItemStack;
			RecipeItems = par2List;
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
            List<ItemStack> arraylist = new List<ItemStack>(RecipeItems);
			int i = 0;

			do
			{
				if (i >= 3)
				{
					break;
				}

				for (int j = 0; j < 3; j++)
				{
					ItemStack itemstack = par1InventoryCrafting.GetStackInRowAndColumn(j, i);

					if (itemstack == null)
					{
						continue;
					}

					bool flag = false;
					IEnumerator<ItemStack> iterator = arraylist.GetEnumerator();

					do
					{
						if (!iterator.MoveNext())
						{
							break;
						}

						ItemStack itemstack1 = iterator.Current;

						if (itemstack.ItemID != itemstack1.ItemID || itemstack1.GetItemDamage() != -1 && itemstack.GetItemDamage() != itemstack1.GetItemDamage())
						{
							continue;
						}

						flag = true;
						arraylist.Remove(itemstack1);
						break;
					}
					while (true);

					if (!flag)
					{
						return false;
					}
				}

				i++;
			}
			while (true);

			return arraylist.Count == 0;
		}

		/// <summary>
		/// Returns an Item that is the result of this recipe
		/// </summary>
		public virtual ItemStack GetCraftingResult(InventoryCrafting par1InventoryCrafting)
		{
			return RecipeOutput.Copy();
		}

		/// <summary>
		/// Returns the size of the recipe area
		/// </summary>
		public virtual int GetRecipeSize()
		{
			return RecipeItems.Count;
		}
	}
}