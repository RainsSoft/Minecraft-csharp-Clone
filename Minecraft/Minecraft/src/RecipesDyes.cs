namespace net.minecraft.src
{

	public class RecipesDyes
	{
		public RecipesDyes()
		{
		}

		/// <summary>
		/// Adds the dye recipes to the CraftingManager.
		/// </summary>
		public virtual void AddRecipes(CraftingManager par1CraftingManager)
		{
			for (int i = 0; i < 16; i++)
			{
				par1CraftingManager.AddShapelessRecipe(new ItemStack(Block.Cloth, 1, BlockCloth.GetDyeFromBlock(i)), new object[] { new ItemStack(Item.DyePowder, 1, i), new ItemStack(Item.ItemsList[Block.Cloth.BlockID], 1, 0)
			});
		}

			par1CraftingManager.AddShapelessRecipe(new ItemStack(Item.DyePowder, 2, 11), new object[] { Block.PlantYellow });
			par1CraftingManager.AddShapelessRecipe(new ItemStack(Item.DyePowder, 2, 1), new object[] { Block.PlantRed });
			par1CraftingManager.AddShapelessRecipe(new ItemStack(Item.DyePowder, 3, 15), new object[] { Item.Bone });
			par1CraftingManager.AddShapelessRecipe(new ItemStack(Item.DyePowder, 2, 9), new object[] { new ItemStack(Item.DyePowder, 1, 1), new ItemStack(Item.DyePowder, 1, 15)
	});
			par1CraftingManager.AddShapelessRecipe(new ItemStack(Item.DyePowder, 2, 14), new object[] { new ItemStack(Item.DyePowder, 1, 1), new ItemStack(Item.DyePowder, 1, 11)
});
			par1CraftingManager.AddShapelessRecipe(new ItemStack(Item.DyePowder, 2, 10), new object[] { new ItemStack(Item.DyePowder, 1, 2), new ItemStack(Item.DyePowder, 1, 15)
					});
			par1CraftingManager.AddShapelessRecipe(new ItemStack(Item.DyePowder, 2, 8), new object[] { new ItemStack(Item.DyePowder, 1, 0), new ItemStack(Item.DyePowder, 1, 15)
					});
			par1CraftingManager.AddShapelessRecipe(new ItemStack(Item.DyePowder, 2, 7), new object[] { new ItemStack(Item.DyePowder, 1, 8), new ItemStack(Item.DyePowder, 1, 15)
					});
			par1CraftingManager.AddShapelessRecipe(new ItemStack(Item.DyePowder, 3, 7), new object[] { new ItemStack(Item.DyePowder, 1, 0), new ItemStack(Item.DyePowder, 1, 15), new ItemStack(Item.DyePowder, 1, 15)
					});
			par1CraftingManager.AddShapelessRecipe(new ItemStack(Item.DyePowder, 2, 12), new object[] { new ItemStack(Item.DyePowder, 1, 4), new ItemStack(Item.DyePowder, 1, 15)
					});
			par1CraftingManager.AddShapelessRecipe(new ItemStack(Item.DyePowder, 2, 6), new object[] { new ItemStack(Item.DyePowder, 1, 4), new ItemStack(Item.DyePowder, 1, 2)
					});
			par1CraftingManager.AddShapelessRecipe(new ItemStack(Item.DyePowder, 2, 5), new object[] { new ItemStack(Item.DyePowder, 1, 4), new ItemStack(Item.DyePowder, 1, 1)
					});
			par1CraftingManager.AddShapelessRecipe(new ItemStack(Item.DyePowder, 2, 13), new object[] { new ItemStack(Item.DyePowder, 1, 5), new ItemStack(Item.DyePowder, 1, 9)
					});
			par1CraftingManager.AddShapelessRecipe(new ItemStack(Item.DyePowder, 3, 13), new object[] { new ItemStack(Item.DyePowder, 1, 4), new ItemStack(Item.DyePowder, 1, 1), new ItemStack(Item.DyePowder, 1, 9)
					});
			par1CraftingManager.AddShapelessRecipe(new ItemStack(Item.DyePowder, 4, 13), new object[] { new ItemStack(Item.DyePowder, 1, 4), new ItemStack(Item.DyePowder, 1, 1), new ItemStack(Item.DyePowder, 1, 1), new ItemStack(Item.DyePowder, 1, 15)
					});
		}
	}

}