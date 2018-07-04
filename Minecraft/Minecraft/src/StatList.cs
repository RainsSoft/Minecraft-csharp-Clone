using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class StatList
	{
		/// <summary>
		/// Tracks one-off stats. </summary>
        public static Dictionary<int, StatBase> OneShotStats = new Dictionary<int, StatBase>();
        public static List<StatBase> AllStats = new List<StatBase>();
        public static List<StatBase> GeneralStats = new List<StatBase>();
        public static List<StatCrafting> ItemStats = new List<StatCrafting>();

		/// <summary>
		/// Tracks the number of times a given block or item has been mined. </summary>
        public static List<StatBase> ObjectMineStats = new List<StatBase>();

		/// <summary>
		/// times the game has been started </summary>
		public static StatBase StartGameStat = (new StatBasic(1000, "stat.startGame")).InitIndependentStat().RegisterStat();

		/// <summary>
		/// times a world has been created </summary>
		public static StatBase CreateWorldStat = (new StatBasic(1001, "stat.createWorld")).InitIndependentStat().RegisterStat();

		/// <summary>
		/// the number of times you have loaded a world </summary>
		public static StatBase LoadWorldStat = (new StatBasic(1002, "stat.loadWorld")).InitIndependentStat().RegisterStat();

		/// <summary>
		/// number of times you've joined a multiplayer world </summary>
		public static StatBase JoinMultiplayerStat = (new StatBasic(1003, "stat.joinMultiplayer")).InitIndependentStat().RegisterStat();

		/// <summary>
		/// number of times you've left a game </summary>
		public static StatBase LeaveGameStat = (new StatBasic(1004, "stat.leaveGame")).InitIndependentStat().RegisterStat();

		/// <summary>
		/// number of minutes you have played </summary>
		public static StatBase MinutesPlayedStat;

		/// <summary>
		/// distance you've walked </summary>
		public static StatBase DistanceWalkedStat;

		/// <summary>
		/// distance you have swam </summary>
		public static StatBase DistanceSwumStat;

		/// <summary>
		/// the distance you have fallen </summary>
		public static StatBase DistanceFallenStat;

		/// <summary>
		/// the distance you've climbed </summary>
		public static StatBase DistanceClimbedStat;

		/// <summary>
		/// the distance you've flown </summary>
		public static StatBase DistanceFlownStat;

		/// <summary>
		/// the distance you've dived </summary>
		public static StatBase DistanceDoveStat;

		/// <summary>
		/// the distance you've traveled by minecart </summary>
		public static StatBase DistanceByMinecartStat;

		/// <summary>
		/// the distance you've traveled by boat </summary>
		public static StatBase DistanceByBoatStat;

		/// <summary>
		/// the distance you've traveled by pig </summary>
		public static StatBase DistanceByPigStat;

		/// <summary>
		/// the times you've jumped </summary>
		public static StatBase JumpStat = (new StatBasic(2010, "stat.jump")).InitIndependentStat().RegisterStat();

		/// <summary>
		/// the distance you've dropped (or times you've fallen?) </summary>
		public static StatBase DropStat = (new StatBasic(2011, "stat.drop")).InitIndependentStat().RegisterStat();

		/// <summary>
		/// the amount of damage you've dealt </summary>
		public static StatBase DamageDealtStat = (new StatBasic(2020, "stat.damageDealt")).RegisterStat();

		/// <summary>
		/// the amount of damage you have taken </summary>
		public static StatBase DamageTakenStat = (new StatBasic(2021, "stat.damageTaken")).RegisterStat();

		/// <summary>
		/// the number of times you have died </summary>
		public static StatBase DeathsStat = (new StatBasic(2022, "stat.deaths")).RegisterStat();

		/// <summary>
		/// the number of mobs you have killed </summary>
		public static StatBase MobKillsStat = (new StatBasic(2023, "stat.mobKills")).RegisterStat();

		/// <summary>
		/// counts the number of times you've killed a player </summary>
		public static StatBase PlayerKillsStat = (new StatBasic(2024, "stat.playerKills")).RegisterStat();
		public static StatBase FishCaughtStat = (new StatBasic(2025, "stat.fishCaught")).RegisterStat();
		public static StatBase[] MineBlockStatArray = InitMinableStats("stat.mineBlock", 0x1000000);
		public static StatBase[] ObjectCraftStats;
		public static StatBase[] ObjectUseStats;
		public static StatBase[] ObjectBreakStats;
		private static bool BlockStatsInitialized = false;
		private static bool ItemStatsInitialized = false;

		public StatList()
		{
		}

		public static void Init()
		{
		}

		/// <summary>
		/// Initializes statistic fields related to breakable items and blocks.
		/// </summary>
		public static void InitBreakableStats()
		{
			ObjectUseStats = InitUsableStats(ObjectUseStats, "stat.useItem", 0x1020000, 0, 256);
			ObjectBreakStats = InitBreakStats(ObjectBreakStats, "stat.breakItem", 0x1030000, 0, 256);
			BlockStatsInitialized = true;
			InitCraftableStats();
		}

		public static void InitStats()
		{
			ObjectUseStats = InitUsableStats(ObjectUseStats, "stat.useItem", 0x1020000, 256, 32000);
			ObjectBreakStats = InitBreakStats(ObjectBreakStats, "stat.breakItem", 0x1030000, 256, 32000);
			ItemStatsInitialized = true;
			InitCraftableStats();
		}

		/// <summary>
		/// Initializes statistics related to craftable items. Is only called after both block and item stats have been
		/// initialized.
		/// </summary>
		public static void InitCraftableStats()
		{
			if (!BlockStatsInitialized || !ItemStatsInitialized)
			{
				return;
			}

            List<int> hashset = new List<int>();
			IRecipe irecipe;

			for (IEnumerator<IRecipe> iterator = CraftingManager.GetInstance().GetRecipeList().GetEnumerator(); iterator.MoveNext(); hashset.Add(irecipe.GetRecipeOutput().ItemID))
			{
				irecipe = iterator.Current;
			}

			ItemStack itemstack;

			for (IEnumerator<ItemStack> iterator1 = FurnaceRecipes.Smelting().GetSmeltingList().Values.GetEnumerator(); iterator1.MoveNext(); hashset.Add(itemstack.ItemID))
			{
				itemstack = iterator1.Current;
			}

			ObjectCraftStats = new StatBase[32000];
			IEnumerator<int> iterator2 = hashset.GetEnumerator();

			do
			{
				if (!iterator2.MoveNext())
				{
					break;
				}

				int integer = iterator2.Current;

				if (Item.ItemsList[(int)integer] != null)
				{
					string s = StatCollector.TranslateToLocalFormatted("stat.craftItem", new object[] { Item.ItemsList[integer].GetStatName() });
					ObjectCraftStats[integer] = new StatCrafting(0x1010000 + integer, s, integer).RegisterStat();
				}
			}
			while (true);

			ReplaceAllSimilarBlocks(ObjectCraftStats);
		}

		/// <summary>
		/// Initializes statistic fields related to minable items and blocks.
		/// </summary>
		private static StatBase[] InitMinableStats(string par0Str, int par1)
		{
			StatBase[] astatbase = new StatBase[256];

			for (int i = 0; i < 256; i++)
			{
                var block = Block.BlocksList[i];

				if (block != null && block.GetEnableStats())
				{
					string s = StatCollector.TranslateToLocalFormatted(par0Str, new object[] { Block.BlocksList[i].TranslateBlockName() });
					astatbase[i] = new StatCrafting(par1 + i, s, i).RegisterStat();
					ObjectMineStats.Add((StatCrafting)astatbase[i]);
				}
			}

			ReplaceAllSimilarBlocks(astatbase);
			return astatbase;
		}

		/// <summary>
		/// Initializes statistic fields related to usable items and blocks.
		/// </summary>
		private static StatBase[] InitUsableStats(StatBase[] par0ArrayOfStatBase, string par1Str, int par2, int par3, int par4)
		{
			if (par0ArrayOfStatBase == null)
			{
				par0ArrayOfStatBase = new StatBase[32000];
			}

			for (int i = par3; i < par4; i++)
			{
				if (Item.ItemsList[i] == null)
				{
					continue;
				}

				string s = StatCollector.TranslateToLocalFormatted(par1Str, new object[] { Item.ItemsList[i].GetStatName() });
				par0ArrayOfStatBase[i] = (new StatCrafting(par2 + i, s, i)).RegisterStat();

				if (i >= 256)
				{
					ItemStats.Add((StatCrafting)par0ArrayOfStatBase[i]);
				}
			}

			ReplaceAllSimilarBlocks(par0ArrayOfStatBase);
			return par0ArrayOfStatBase;
		}

		private static StatBase[] InitBreakStats(StatBase[] par0ArrayOfStatBase, string par1Str, int par2, int par3, int par4)
		{
			if (par0ArrayOfStatBase == null)
			{
				par0ArrayOfStatBase = new StatBase[32000];
			}

			for (int i = par3; i < par4; i++)
			{
				if (Item.ItemsList[i] != null && Item.ItemsList[i].IsDamageable())
				{
					string s = StatCollector.TranslateToLocalFormatted(par1Str, new object[] { Item.ItemsList[i].GetStatName() });
					par0ArrayOfStatBase[i] = new StatCrafting(par2 + i, s, i).RegisterStat();
				}
			}

			ReplaceAllSimilarBlocks(par0ArrayOfStatBase);
			return par0ArrayOfStatBase;
		}

		/// <summary>
		/// Forces all dual blocks to count for each other on the stats list
		/// </summary>
		private static void ReplaceAllSimilarBlocks(StatBase[] par0ArrayOfStatBase)
		{
			ReplaceSimilarBlocks(par0ArrayOfStatBase, Block.WaterStill.BlockID,             Block.WaterMoving.BlockID);
			ReplaceSimilarBlocks(par0ArrayOfStatBase, Block.LavaStill.BlockID,              Block.LavaStill.BlockID);
			ReplaceSimilarBlocks(par0ArrayOfStatBase, Block.PumpkinLantern.BlockID,         Block.Pumpkin.BlockID);
			ReplaceSimilarBlocks(par0ArrayOfStatBase, Block.StoneOvenActive.BlockID,        Block.StoneOvenIdle.BlockID);
			ReplaceSimilarBlocks(par0ArrayOfStatBase, Block.OreRedstoneGlowing.BlockID,     Block.OreRedstone.BlockID);
			ReplaceSimilarBlocks(par0ArrayOfStatBase, Block.RedstoneRepeaterActive.BlockID, Block.RedstoneRepeaterIdle.BlockID);
			ReplaceSimilarBlocks(par0ArrayOfStatBase, Block.TorchRedstoneActive.BlockID,    Block.TorchRedstoneIdle.BlockID);
			ReplaceSimilarBlocks(par0ArrayOfStatBase, Block.MushroomRed.BlockID,            Block.MushroomBrown.BlockID);
			ReplaceSimilarBlocks(par0ArrayOfStatBase, Block.StairDouble.BlockID,            Block.StairSingle.BlockID);
			ReplaceSimilarBlocks(par0ArrayOfStatBase, Block.Grass.BlockID,                  Block.Dirt.BlockID);
			ReplaceSimilarBlocks(par0ArrayOfStatBase, Block.TilledField.BlockID,            Block.Dirt.BlockID);
		}

		/// <summary>
		/// Forces stats for one block to add to another block, such as idle and active furnaces
		/// </summary>
		private static void ReplaceSimilarBlocks(StatBase[] par0ArrayOfStatBase, int par1, int par2)
		{
			if (par0ArrayOfStatBase[par1] != null && par0ArrayOfStatBase[par2] == null)
			{
				par0ArrayOfStatBase[par2] = par0ArrayOfStatBase[par1];
				return;
			}
			else
			{
				AllStats.Remove(par0ArrayOfStatBase[par1]);
				ObjectMineStats.Remove(par0ArrayOfStatBase[par1]);
				GeneralStats.Remove(par0ArrayOfStatBase[par1]);
				par0ArrayOfStatBase[par1] = par0ArrayOfStatBase[par2];
				return;
			}
		}

		public static StatBase GetOneShotStat(int par0)
		{
			return OneShotStats[par0];
		}

		static StatList()
		{
			MinutesPlayedStat = (new StatBasic(1100, "stat.playOneMinute", StatBase.TimeStatType)).InitIndependentStat().RegisterStat();
			DistanceWalkedStat = (new StatBasic(2000, "stat.walkOneCm", StatBase.DistanceStatType)).InitIndependentStat().RegisterStat();
			DistanceSwumStat = (new StatBasic(2001, "stat.swimOneCm", StatBase.DistanceStatType)).InitIndependentStat().RegisterStat();
			DistanceFallenStat = (new StatBasic(2002, "stat.fallOneCm", StatBase.DistanceStatType)).InitIndependentStat().RegisterStat();
			DistanceClimbedStat = (new StatBasic(2003, "stat.climbOneCm", StatBase.DistanceStatType)).InitIndependentStat().RegisterStat();
			DistanceFlownStat = (new StatBasic(2004, "stat.flyOneCm", StatBase.DistanceStatType)).InitIndependentStat().RegisterStat();
			DistanceDoveStat = (new StatBasic(2005, "stat.diveOneCm", StatBase.DistanceStatType)).InitIndependentStat().RegisterStat();
			DistanceByMinecartStat = (new StatBasic(2006, "stat.minecartOneCm", StatBase.DistanceStatType)).InitIndependentStat().RegisterStat();
			DistanceByBoatStat = (new StatBasic(2007, "stat.boatOneCm", StatBase.DistanceStatType)).InitIndependentStat().RegisterStat();
			DistanceByPigStat = (new StatBasic(2008, "stat.pigOneCm", StatBase.DistanceStatType)).InitIndependentStat().RegisterStat();
			AchievementList.Init();
		}
	}
}