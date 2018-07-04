using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class FurnaceRecipes
	{
		private static readonly FurnaceRecipes SmeltingBase = new FurnaceRecipes();

		/// <summary>
		/// The list of smelting results. </summary>
		private Dictionary<int, ItemStack> SmeltingList;

		/// <summary>
		/// Used to call methods addSmelting and getSmeltingResult.
		/// </summary>
		public static FurnaceRecipes Smelting()
		{
			return SmeltingBase;
		}

		private FurnaceRecipes()
		{
            SmeltingList = new Dictionary<int, ItemStack>();
			AddSmelting(Block.OreIron.BlockID, new ItemStack(Item.IngotIron));
			AddSmelting(Block.OreGold.BlockID, new ItemStack(Item.IngotGold));
			AddSmelting(Block.OreDiamond.BlockID, new ItemStack(Item.Diamond));
			AddSmelting(Block.Sand.BlockID, new ItemStack(Block.Glass));
			AddSmelting(Item.PorkRaw.ShiftedIndex, new ItemStack(Item.PorkCooked));
			AddSmelting(Item.BeefRaw.ShiftedIndex, new ItemStack(Item.BeefCooked));
			AddSmelting(Item.ChickenRaw.ShiftedIndex, new ItemStack(Item.ChickenCooked));
			AddSmelting(Item.FishRaw.ShiftedIndex, new ItemStack(Item.FishCooked));
			AddSmelting(Block.Cobblestone.BlockID, new ItemStack(Block.Stone));
			AddSmelting(Item.Clay.ShiftedIndex, new ItemStack(Item.Brick));
			AddSmelting(Block.Cactus.BlockID, new ItemStack(Item.DyePowder, 1, 2));
			AddSmelting(Block.Wood.BlockID, new ItemStack(Item.Coal, 1, 1));
			AddSmelting(Block.OreCoal.BlockID, new ItemStack(Item.Coal));
			AddSmelting(Block.OreRedstone.BlockID, new ItemStack(Item.Redstone));
			AddSmelting(Block.OreLapis.BlockID, new ItemStack(Item.DyePowder, 1, 4));
		}

		/// <summary>
		/// Adds a smelting recipe.
		/// </summary>
		public virtual void AddSmelting(int par1, ItemStack par2ItemStack)
		{
			SmeltingList.Add(par1, par2ItemStack);
		}

		/// <summary>
		/// Returns the smelting result of an item.
		/// </summary>
		public virtual ItemStack GetSmeltingResult(int par1)
		{
			return SmeltingList[par1];
		}

        public virtual Dictionary<int, ItemStack> GetSmeltingList()
		{
			return SmeltingList;
		}
	}
}