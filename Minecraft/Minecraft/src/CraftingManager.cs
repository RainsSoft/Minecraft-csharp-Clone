using System;
using System.Collections.Generic;
using System.Text;

namespace net.minecraft.src
{
	public class CraftingManager
	{
		/// <summary>
		/// The static instance of this class </summary>
		private static readonly CraftingManager Instance = new CraftingManager();

		/// <summary>
		/// A list of all the recipes added </summary>
		private List<IRecipe> Recipes;

		/// <summary>
		/// Returns the static instance of this class
		/// </summary>
		public static CraftingManager GetInstance()
		{
			return Instance;
		}

		private CraftingManager()
		{
            Recipes = new List<IRecipe>();
			(new RecipesTools()).AddRecipes(this);
			(new RecipesWeapons()).AddRecipes(this);
			(new RecipesIngots()).AddRecipes(this);
			(new RecipesFood()).AddRecipes(this);
			(new RecipesCrafting()).AddRecipes(this);
			(new RecipesArmor()).AddRecipes(this);
			(new RecipesDyes()).AddRecipes(this);
			AddRecipe(new ItemStack(Item.Paper, 3), new object[] { "###", '#', Item.Reed });
			AddRecipe(new ItemStack(Item.Book, 1), new object[] { "#", "#", "#", '#', Item.Paper });
			AddRecipe(new ItemStack(Block.Fence, 2), new object[] { "###", "###", '#', Item.Stick });
			AddRecipe(new ItemStack(Block.NetherFence, 6), new object[] { "###", "###", '#', Block.NetherBrick });
			AddRecipe(new ItemStack(Block.FenceGate, 1), new object[] { "#W#", "#W#", '#', Item.Stick, 'W', Block.Planks });
			AddRecipe(new ItemStack(Block.Jukebox, 1), new object[] { "###", "#X#", "###", '#', Block.Planks, 'X', Item.Diamond });
			AddRecipe(new ItemStack(Block.Music, 1), new object[] { "###", "#X#", "###", '#', Block.Planks, 'X', Item.Redstone });
			AddRecipe(new ItemStack(Block.BookShelf, 1), new object[] { "###", "XXX", "###", '#', Block.Planks, 'X', Item.Book });
			AddRecipe(new ItemStack(Block.BlockSnow, 1), new object[] { "##", "##", '#', Item.Snowball });
			AddRecipe(new ItemStack(Block.BlockClay, 1), new object[] { "##", "##", '#', Item.Clay });
			AddRecipe(new ItemStack(Block.Brick, 1), new object[] { "##", "##", '#', Item.Brick });
			AddRecipe(new ItemStack(Block.GlowStone, 1), new object[] { "##", "##", '#', Item.LightStoneDust });
			AddRecipe(new ItemStack(Block.Cloth, 1), new object[] { "##", "##", '#', Item.Silk });
			AddRecipe(new ItemStack(Block.Tnt, 1), new object[] { "X#X", "#X#", "X#X", 'X', Item.Gunpowder, '#', Block.Sand });
			AddRecipe(new ItemStack(Block.StairSingle, 6, 3), new object[] { "###", '#', Block.Cobblestone });
			AddRecipe(new ItemStack(Block.StairSingle, 6, 0), new object[] { "###", '#', Block.Stone });
			AddRecipe(new ItemStack(Block.StairSingle, 6, 1), new object[] { "###", '#', Block.SandStone });
			AddRecipe(new ItemStack(Block.StairSingle, 6, 2), new object[] { "###", '#', Block.Planks });
			AddRecipe(new ItemStack(Block.StairSingle, 6, 4), new object[] { "###", '#', Block.Brick });
			AddRecipe(new ItemStack(Block.StairSingle, 6, 5), new object[] { "###", '#', Block.StoneBrick });
			AddRecipe(new ItemStack(Block.Ladder, 3), new object[] { "# #", "###", "# #", '#', Item.Stick });
			AddRecipe(new ItemStack(Item.DoorWood, 1), new object[] { "##", "##", "##", '#', Block.Planks });
			AddRecipe(new ItemStack(Block.Trapdoor, 2), new object[] { "###", "###", '#', Block.Planks });
			AddRecipe(new ItemStack(Item.DoorSteel, 1), new object[] { "##", "##", "##", '#', Item.IngotIron });
			AddRecipe(new ItemStack(Item.Sign, 1), new object[] { "###", "###", " X ", '#', Block.Planks, 'X', Item.Stick });
			AddRecipe(new ItemStack(Item.Cake, 1), new object[] { "AAA", "BEB", "CCC", 'A', Item.BucketMilk, 'B', Item.Sugar, 'C', Item.Wheat, 'E', Item.Egg });
			AddRecipe(new ItemStack(Item.Sugar, 1), new object[] { "#", '#', Item.Reed });
			AddRecipe(new ItemStack(Block.Planks, 4, 0), new object[] { "#", '#', new ItemStack(Block.Wood, 1, 0) });
			AddRecipe(new ItemStack(Block.Planks, 4, 1), new object[] { "#", '#', new ItemStack(Block.Wood, 1, 1) });
			AddRecipe(new ItemStack(Block.Planks, 4, 2), new object[] { "#", '#', new ItemStack(Block.Wood, 1, 2) });
			AddRecipe(new ItemStack(Block.Planks, 4, 3), new object[] { "#", '#', new ItemStack(Block.Wood, 1, 3) });
			AddRecipe(new ItemStack(Item.Stick, 4), new object[] { "#", "#", '#', Block.Planks });
			AddRecipe(new ItemStack(Block.TorchWood, 4), new object[] { "X", "#", 'X', Item.Coal, '#', Item.Stick });
			AddRecipe(new ItemStack(Block.TorchWood, 4), new object[] { "X", "#", 'X', new ItemStack(Item.Coal, 1, 1), '#', Item.Stick });
			AddRecipe(new ItemStack(Item.BowlEmpty, 4), new object[] { "# #", " # ", '#', Block.Planks });
			AddRecipe(new ItemStack(Item.GlassBottle, 3), new object[] { "# #", " # ", '#', Block.Glass });
			AddRecipe(new ItemStack(Block.Rail, 16), new object[] { "X X", "X#X", "X X", 'X', Item.IngotIron, '#', Item.Stick });
			AddRecipe(new ItemStack(Block.RailPowered, 6), new object[] { "X X", "X#X", "XRX", 'X', Item.IngotGold, 'R', Item.Redstone, '#', Item.Stick });
			AddRecipe(new ItemStack(Block.RailDetector, 6), new object[] { "X X", "X#X", "XRX", 'X', Item.IngotIron, 'R', Item.Redstone, '#', Block.PressurePlateStone });
			AddRecipe(new ItemStack(Item.MinecartEmpty, 1), new object[] { "# #", "###", '#', Item.IngotIron });
			AddRecipe(new ItemStack(Item.Cauldron, 1), new object[] { "# #", "# #", "###", '#', Item.IngotIron });
			AddRecipe(new ItemStack(Item.BrewingStand, 1), new object[] { " B ", "###", '#', Block.Cobblestone, 'B', Item.BlazeRod });
			AddRecipe(new ItemStack(Block.PumpkinLantern, 1), new object[] { "A", "B", 'A', Block.Pumpkin, 'B', Block.TorchWood });
			AddRecipe(new ItemStack(Item.MinecartCrate, 1), new object[] { "A", "B", 'A', Block.Chest, 'B', Item.MinecartEmpty });
			AddRecipe(new ItemStack(Item.MinecartPowered, 1), new object[] { "A", "B", 'A', Block.StoneOvenIdle, 'B', Item.MinecartEmpty });
			AddRecipe(new ItemStack(Item.Boat, 1), new object[] { "# #", "###", '#', Block.Planks });
			AddRecipe(new ItemStack(Item.BucketEmpty, 1), new object[] { "# #", " # ", '#', Item.IngotIron });
			AddRecipe(new ItemStack(Item.FlintAndSteel, 1), new object[] { "A ", " B", 'A', Item.IngotIron, 'B', Item.Flint });
			AddRecipe(new ItemStack(Item.Bread, 1), new object[] { "###", '#', Item.Wheat });
			AddRecipe(new ItemStack(Block.StairCompactPlanks, 4), new object[] { "#  ", "## ", "###", '#', Block.Planks });
			AddRecipe(new ItemStack(Item.FishingRod, 1), new object[] { "  #", " #X", "# X", '#', Item.Stick, 'X', Item.Silk });
			AddRecipe(new ItemStack(Block.StairCompactCobblestone, 4), new object[] { "#  ", "## ", "###", '#', Block.Cobblestone });
			AddRecipe(new ItemStack(Block.StairsBrick, 4), new object[] { "#  ", "## ", "###", '#', Block.Brick });
			AddRecipe(new ItemStack(Block.StairsStoneBrickSmooth, 4), new object[] { "#  ", "## ", "###", '#', Block.StoneBrick });
			AddRecipe(new ItemStack(Block.StairsNetherBrick, 4), new object[] { "#  ", "## ", "###", '#', Block.NetherBrick });
			AddRecipe(new ItemStack(Item.Painting, 1), new object[] { "###", "#X#", "###", '#', Item.Stick, 'X', Block.Cloth });
			AddRecipe(new ItemStack(Item.AppleGold, 1), new object[] { "###", "#X#", "###", '#', Item.GoldNugget, 'X', Item.AppleRed });
			AddRecipe(new ItemStack(Block.Lever, 1), new object[] { "X", "#", '#', Block.Cobblestone, 'X', Item.Stick });
			AddRecipe(new ItemStack(Block.TorchRedstoneActive, 1), new object[] { "X", "#", '#', Item.Stick, 'X', Item.Redstone });
			AddRecipe(new ItemStack(Item.RedstoneRepeater, 1), new object[] { "#X#", "III", '#', Block.TorchRedstoneActive, 'X', Item.Redstone, 'I', Block.Stone });
			AddRecipe(new ItemStack(Item.PocketSundial, 1), new object[] { " # ", "#X#", " # ", '#', Item.IngotGold, 'X', Item.Redstone });
			AddRecipe(new ItemStack(Item.Compass, 1), new object[] { " # ", "#X#", " # ", '#', Item.IngotIron, 'X', Item.Redstone });
			AddRecipe(new ItemStack(Item.Map, 1), new object[] { "###", "#X#", "###", '#', Item.Paper, 'X', Item.Compass });
			AddRecipe(new ItemStack(Block.Button, 1), new object[] { "#", "#", '#', Block.Stone });
			AddRecipe(new ItemStack(Block.PressurePlateStone, 1), new object[] { "##", '#', Block.Stone });
			AddRecipe(new ItemStack(Block.PressurePlatePlanks, 1), new object[] { "##", '#', Block.Planks });
			AddRecipe(new ItemStack(Block.Dispenser, 1), new object[] { "###", "#X#", "#R#", '#', Block.Cobblestone, 'X', Item.Bow, 'R', Item.Redstone });
			AddRecipe(new ItemStack(Block.PistonBase, 1), new object[] { "TTT", "#X#", "#R#", '#', Block.Cobblestone, 'X', Item.IngotIron, 'R', Item.Redstone, 'T', Block.Planks });
			AddRecipe(new ItemStack(Block.PistonStickyBase, 1), new object[] { "S", "P", 'S', Item.SlimeBall, 'P', Block.PistonBase });
			AddRecipe(new ItemStack(Item.Bed, 1), new object[] { "###", "XXX", '#', Block.Cloth, 'X', Block.Planks });
			AddRecipe(new ItemStack(Block.EnchantmentTable, 1), new object[] { " B ", "D#D", "###", '#', Block.Obsidian, 'B', Item.Book, 'D', Item.Diamond });
			AddShapelessRecipe(new ItemStack(Item.EyeOfEnder, 1), new object[] { Item.EnderPearl, Item.BlazePowder });
			AddShapelessRecipe(new ItemStack(Item.FireballCharge, 3), new object[] { Item.Gunpowder, Item.BlazePowder, Item.Coal });
			AddShapelessRecipe(new ItemStack(Item.FireballCharge, 3), new object[] { Item.Gunpowder, Item.BlazePowder, new ItemStack(Item.Coal, 1, 1) });
			Recipes.Sort(new RecipeSorter(this));
			Console.WriteLine((new StringBuilder()).Append(Recipes.Count).Append(" recipes").ToString());
		}

		/// <summary>
		/// Adds a recipe. See spreadsheet on first page for details.
		/// </summary>
        public virtual void AddRecipe(ItemStack par1ItemStack, object[] par2ArrayOfObj)
		{
			string s = "";
			int i = 0;
			int j = 0;
			int k = 0;

			if (par2ArrayOfObj[i] is string[])
			{
				string[] @as = (string[])par2ArrayOfObj[i++];

				for (int l = 0; l < @as.Length; l++)
				{
					string s2 = @as[l];
					k++;
					j = s2.Length;
					s = new StringBuilder().Append(s).Append(s2).ToString();
				}
			}
			else
			{
				while (par2ArrayOfObj[i] is string)
				{
					string s1 = (string)par2ArrayOfObj[i++];
					k++;
					j = s1.Length;
					s = new StringBuilder().Append(s).Append(s1).ToString();
				}
			}

            Dictionary<char, ItemStack> hashmap = new Dictionary<char, ItemStack>();

			for (; i < par2ArrayOfObj.Length; i += 2)
			{
				char character = (char)par2ArrayOfObj[i];
				ItemStack itemstack = null;

				if (par2ArrayOfObj[i + 1] is Item)
				{
					itemstack = new ItemStack((Item)par2ArrayOfObj[i + 1]);
				}
				else if (par2ArrayOfObj[i + 1] is Block)
				{
					itemstack = new ItemStack((Block)par2ArrayOfObj[i + 1], 1, -1);
				}
				else if (par2ArrayOfObj[i + 1] is ItemStack)
				{
					itemstack = (ItemStack)par2ArrayOfObj[i + 1];
				}

				hashmap[character] = itemstack;
			}

			ItemStack[] aitemstack = new ItemStack[j * k];

			for (int i1 = 0; i1 < j * k; i1++)
			{
				char c = s[i1];

				if (hashmap.ContainsKey(c))
				{
					aitemstack[i1] = hashmap[c].Copy();
				}
				else
				{
					aitemstack[i1] = null;
				}
			}

			Recipes.Add(new ShapedRecipes(j, k, aitemstack, par1ItemStack));
		}

        public virtual void AddShapelessRecipe(ItemStack par1ItemStack, object[] par2ArrayOfObj)
		{
            List<ItemStack> arraylist = new List<ItemStack>();
			object[] aobj = par2ArrayOfObj;
			int i = aobj.Length;

			for (int j = 0; j < i; j++)
			{
				object obj = aobj[j];

				if (obj is ItemStack)
				{
					arraylist.Add(((ItemStack)obj).Copy());
					continue;
				}

				if (obj is Item)
				{
					arraylist.Add(new ItemStack((Item)obj));
					continue;
				}

				if (obj is Block)
				{
					arraylist.Add(new ItemStack((Block)obj));
				}
				else
				{
					throw new Exception("Invalid shapeless recipy!");
				}
			}

			Recipes.Add(new ShapelessRecipes(par1ItemStack, arraylist));
		}

		public virtual ItemStack FindMatchingRecipe(InventoryCrafting par1InventoryCrafting)
		{
			int i = 0;
			ItemStack itemstack = null;
			ItemStack itemstack1 = null;

			for (int j = 0; j < par1InventoryCrafting.GetSizeInventory(); j++)
			{
				ItemStack itemstack2 = par1InventoryCrafting.GetStackInSlot(j);

				if (itemstack2 == null)
				{
					continue;
				}

				if (i == 0)
				{
					itemstack = itemstack2;
				}

				if (i == 1)
				{
					itemstack1 = itemstack2;
				}

				i++;
			}

			if (i == 2 && itemstack.ItemID == itemstack1.ItemID && itemstack.StackSize == 1 && itemstack1.StackSize == 1 && Item.ItemsList[itemstack.ItemID].IsDamageable())
			{
				Item item = Item.ItemsList[itemstack.ItemID];
				int l = item.GetMaxDamage() - itemstack.GetItemDamageForDisplay();
				int i1 = item.GetMaxDamage() - itemstack1.GetItemDamageForDisplay();
				int j1 = l + i1 + (item.GetMaxDamage() * 10) / 100;
				int k1 = item.GetMaxDamage() - j1;

				if (k1 < 0)
				{
					k1 = 0;
				}

				return new ItemStack(itemstack.ItemID, 1, k1);
			}

			for (int k = 0; k < Recipes.Count; k++)
			{
				IRecipe irecipe = (IRecipe)Recipes[k];

				if (irecipe.Matches(par1InventoryCrafting))
				{
					return irecipe.GetCraftingResult(par1InventoryCrafting);
				}
			}

			return null;
		}

		/// <summary>
		/// returns the List<> of all recipes
		/// </summary>
		public virtual List<IRecipe> GetRecipeList()
		{
			return Recipes;
		}
	}
}