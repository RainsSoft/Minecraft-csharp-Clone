using System;
using System.Collections.Generic;
using System.Text;

namespace net.minecraft.src
{
	public class AchievementList
	{
		/// <summary>
		/// Is the smallest column used to display a achievement on the GUI. </summary>
		public static int MinDisplayColumn;

		/// <summary>
		/// Is the smallest row used to display a achievement on the GUI. </summary>
		public static int MinDisplayRow;

		/// <summary>
		/// Is the biggest column used to display a achievement on the GUI. </summary>
		public static int MaxDisplayColumn;

		/// <summary>
		/// Is the biggest row used to display a achievement on the GUI. </summary>
		public static int MaxDisplayRow;

		/// <summary>
		/// Holds a list of all registered achievements. </summary>
		public static List<Achievement> Achievements;

		/// <summary>
		/// Is the 'open inventory' achievement. </summary>
		public static Achievement OpenInventory;

		/// <summary>
		/// Is the 'getting wood' achievement. </summary>
		public static Achievement MineWood;

		/// <summary>
		/// Is the 'benchmarking' achievement. </summary>
		public static Achievement BuildWorkBench;

		/// <summary>
		/// Is the 'time to mine' achievement. </summary>
		public static Achievement BuildPickaxe;

		/// <summary>
		/// Is the 'hot topic' achievement. </summary>
		public static Achievement BuildFurnace;

		/// <summary>
		/// Is the 'acquire hardware' achievement. </summary>
		public static Achievement AcquireIron;

		/// <summary>
		/// Is the 'time to farm' achievement. </summary>
		public static Achievement BuildHoe;

		/// <summary>
		/// Is the 'bake bread' achievement. </summary>
		public static Achievement MakeBread;

		/// <summary>
		/// Is the 'the lie' achievement. </summary>
		public static Achievement BakeCake;

		/// <summary>
		/// Is the 'getting a upgrade' achievement. </summary>
		public static Achievement BuildBetterPickaxe;

		/// <summary>
		/// Is the 'delicious fish' achievement. </summary>
		public static Achievement CookFish;

		/// <summary>
		/// Is the 'on a rail' achievement </summary>
		public static Achievement OnARail;

		/// <summary>
		/// Is the 'time to strike' achievement. </summary>
		public static Achievement BuildSword;

		/// <summary>
		/// Is the 'monster hunter' achievement. </summary>
		public static Achievement KillEnemy;

		/// <summary>
		/// is the 'cow tipper' achievement. </summary>
		public static Achievement KillCow;

		/// <summary>
		/// Is the 'when pig fly' achievement. </summary>
		public static Achievement FlyPig;

		/// <summary>
		/// The achievement for killing a Skeleton from 50 meters aways. </summary>
		public static Achievement SnipeSkeleton;

		/// <summary>
		/// Is the 'DIAMONDS!' achievement </summary>
		public static Achievement Diamonds;

		/// <summary>
		/// Is the 'We Need to Go Deeper' achievement </summary>
		public static Achievement Portal;

		/// <summary>
		/// Is the 'Return to Sender' achievement </summary>
		public static Achievement Ghast;

		/// <summary>
		/// Is the 'Into Fire' achievement </summary>
		public static Achievement BlazeRod;

		/// <summary>
		/// Is the 'Local Brewery' achievement </summary>
		public static Achievement Potion;

		/// <summary>
		/// Is the 'The End?' achievement </summary>
		public static Achievement TheEnd;

		/// <summary>
		/// Is the 'The End.' achievement </summary>
		public static Achievement TheEnd2;

		/// <summary>
		/// Is the 'Enchanter' achievement </summary>
		public static Achievement Enchantments;
		public static Achievement Overkill;

		/// <summary>
		/// Is the 'Librarian' achievement </summary>
		public static Achievement Bookcase;

		public AchievementList()
		{
		}

		/// <summary>
		/// A stub functions called to make the static initializer for this class run.
		/// </summary>
		public static void Init()
		{
		}

		static AchievementList()
		{
			Achievements = new List<Achievement>();
			OpenInventory = (new Achievement(0, "openInventory", 0, 0, Item.Book, null)).SetIndependent().RegisterAchievement();
			MineWood = (new Achievement(1, "mineWood", 2, 1, Block.Wood, OpenInventory)).RegisterAchievement();
			BuildWorkBench = (new Achievement(2, "buildWorkBench", 4, -1, Block.Workbench, MineWood)).RegisterAchievement();
			BuildPickaxe = (new Achievement(3, "buildPickaxe", 4, 2, Item.PickaxeWood, BuildWorkBench)).RegisterAchievement();
			BuildFurnace = (new Achievement(4, "buildFurnace", 3, 4, Block.StoneOvenActive, BuildPickaxe)).RegisterAchievement();
			AcquireIron = (new Achievement(5, "acquireIron", 1, 4, Item.IngotIron, BuildFurnace)).RegisterAchievement();
			BuildHoe = (new Achievement(6, "buildHoe", 2, -3, Item.HoeWood, BuildWorkBench)).RegisterAchievement();
			MakeBread = (new Achievement(7, "makeBread", -1, -3, Item.Bread, BuildHoe)).RegisterAchievement();
			BakeCake = (new Achievement(8, "bakeCake", 0, -5, Item.Cake, BuildHoe)).RegisterAchievement();
			BuildBetterPickaxe = (new Achievement(9, "buildBetterPickaxe", 6, 2, Item.PickaxeStone, BuildPickaxe)).RegisterAchievement();
			CookFish = (new Achievement(10, "cookFish", 2, 6, Item.FishCooked, BuildFurnace)).RegisterAchievement();
			OnARail = (new Achievement(11, "onARail", 2, 3, Block.Rail, AcquireIron)).SetSpecial().RegisterAchievement();
			BuildSword = (new Achievement(12, "buildSword", 6, -1, Item.SwordWood, BuildWorkBench)).RegisterAchievement();
			KillEnemy = (new Achievement(13, "killEnemy", 8, -1, Item.Bone, BuildSword)).RegisterAchievement();
			KillCow = (new Achievement(14, "killCow", 7, -3, Item.Leather, BuildSword)).RegisterAchievement();
			FlyPig = (new Achievement(15, "flyPig", 8, -4, Item.Saddle, KillCow)).SetSpecial().RegisterAchievement();
			SnipeSkeleton = (new Achievement(16, "snipeSkeleton", 7, 0, Item.Bow, KillEnemy)).SetSpecial().RegisterAchievement();
			Diamonds = (new Achievement(17, "diamonds", -1, 5, Item.Diamond, AcquireIron)).RegisterAchievement();
			Portal = (new Achievement(18, "portal", -1, 7, Block.Obsidian, Diamonds)).RegisterAchievement();
			Ghast = (new Achievement(19, "ghast", -4, 8, Item.GhastTear, Portal)).SetSpecial().RegisterAchievement();
			BlazeRod = (new Achievement(20, "blazeRod", 0, 9, Item.BlazeRod, Portal)).RegisterAchievement();
			Potion = (new Achievement(21, "potion", 2, 8, Item.Potion, BlazeRod)).RegisterAchievement();
			TheEnd = (new Achievement(22, "theEnd", 3, 10, Item.EyeOfEnder, BlazeRod)).SetSpecial().RegisterAchievement();
			TheEnd2 = (new Achievement(23, "theEnd2", 4, 13, Block.DragonEgg, TheEnd)).SetSpecial().RegisterAchievement();
			Enchantments = (new Achievement(24, "enchantments", -4, 4, Block.EnchantmentTable, Diamonds)).RegisterAchievement();
			Overkill = (new Achievement(25, "overkill", -4, 1, Item.SwordDiamond, Enchantments)).SetSpecial().RegisterAchievement();
			Bookcase = (new Achievement(26, "bookcase", -3, 6, Block.BookShelf, Enchantments)).RegisterAchievement();
			Console.WriteLine((new StringBuilder()).Append(Achievements.Count).Append(" achievements").ToString());
		}
	}
}