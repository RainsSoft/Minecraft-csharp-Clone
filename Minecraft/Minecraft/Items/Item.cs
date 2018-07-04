using System;
using System.Collections.Generic;
using System.Text;

namespace net.minecraft.src
{
	public class Item
	{
		/// <summary>
		/// The RNG used by the Item subclasses. </summary>
		protected static Random ItemRand = new Random();
		public static Item[] ItemsList = new Item[32000];
		public static Item ShovelSteel;
		public static Item PickaxeSteel;
		public static Item AxeSteel;
		public static Item FlintAndSteel = (new ItemFlintAndSteel(3)).SetIconCoord(5, 0).SetItemName("flintAndSteel");
		public static Item AppleRed = (new ItemFood(4, 4, 0.3F, false)).SetIconCoord(10, 0).SetItemName("apple");
		public static Item Bow = (new ItemBow(5)).SetIconCoord(5, 1).SetItemName("bow");
		public static Item Arrow = (new Item(6)).SetIconCoord(5, 2).SetItemName("arrow");
		public static Item Coal = (new ItemCoal(7)).SetIconCoord(7, 0).SetItemName("coal");
		public static Item Diamond = (new Item(8)).SetIconCoord(7, 3).SetItemName("emerald");
		public static Item IngotIron = (new Item(9)).SetIconCoord(7, 1).SetItemName("ingotIron");
		public static Item IngotGold = (new Item(10)).SetIconCoord(7, 2).SetItemName("ingotGold");
		public static Item SwordSteel;
		public static Item SwordWood;
		public static Item ShovelWood;
		public static Item PickaxeWood;
		public static Item AxeWood;
		public static Item SwordStone;
		public static Item ShovelStone;
		public static Item PickaxeStone;
		public static Item AxeStone;
		public static Item SwordDiamond;
		public static Item ShovelDiamond;
		public static Item PickaxeDiamond;
		public static Item AxeDiamond;
		public static Item Stick = (new Item(24)).SetIconCoord(5, 3).SetFull3D().SetItemName("stick");
		public static Item BowlEmpty = (new Item(25)).SetIconCoord(7, 4).SetItemName("bowl");
		public static Item BowlSoup = (new ItemSoup(26, 8)).SetIconCoord(8, 4).SetItemName("mushroomStew");
		public static Item SwordGold;
		public static Item ShovelGold;
		public static Item PickaxeGold;
		public static Item AxeGold;
		public static Item Silk = (new Item(31)).SetIconCoord(8, 0).SetItemName("string");
		public static Item Feather = (new Item(32)).SetIconCoord(8, 1).SetItemName("feather");
		public static Item Gunpowder;
		public static Item HoeWood;
		public static Item HoeStone;
		public static Item HoeSteel;
		public static Item HoeDiamond;
		public static Item HoeGold;
		public static Item Seeds;
		public static Item Wheat = (new Item(40)).SetIconCoord(9, 1).SetItemName("wheat");
		public static Item Bread = (new ItemFood(41, 5, 0.6F, false)).SetIconCoord(9, 2).SetItemName("bread");
		public static Item HelmetLeather;
		public static Item PlateLeather;
		public static Item LegsLeather;
		public static Item BootsLeather;
		public static Item HelmetChain;
		public static Item PlateChain;
		public static Item LegsChain;
		public static Item BootsChain;
		public static Item HelmetSteel;
		public static Item PlateSteel;
		public static Item LegsSteel;
		public static Item BootsSteel;
		public static Item HelmetDiamond;
		public static Item PlateDiamond;
		public static Item LegsDiamond;
		public static Item BootsDiamond;
		public static Item HelmetGold;
		public static Item PlateGold;
		public static Item LegsGold;
		public static Item BootsGold;
		public static Item Flint = (new Item(62)).SetIconCoord(6, 0).SetItemName("flint");
		public static Item PorkRaw = (new ItemFood(63, 3, 0.3F, true)).SetIconCoord(7, 5).SetItemName("porkchopRaw");
		public static Item PorkCooked = (new ItemFood(64, 8, 0.8F, true)).SetIconCoord(8, 5).SetItemName("porkchopCooked");
		public static Item Painting = (new ItemPainting(65)).SetIconCoord(10, 1).SetItemName("painting");
		public static Item AppleGold;
		public static Item Sign = (new ItemSign(67)).SetIconCoord(10, 2).SetItemName("sign");
		public static Item DoorWood;
		public static Item BucketEmpty;
		public static Item BucketWater;
		public static Item BucketLava;
		public static Item MinecartEmpty = (new ItemMinecart(72, 0)).SetIconCoord(7, 8).SetItemName("minecart");
		public static Item Saddle = (new ItemSaddle(73)).SetIconCoord(8, 6).SetItemName("saddle");
		public static Item DoorSteel;
		public static Item Redstone;
		public static Item Snowball = (new ItemSnowball(76)).SetIconCoord(14, 0).SetItemName("snowball");
		public static Item Boat = (new ItemBoat(77)).SetIconCoord(8, 8).SetItemName("boat");
		public static Item Leather = (new Item(78)).SetIconCoord(7, 6).SetItemName("leather");
		public static Item BucketMilk;
		public static Item Brick = (new Item(80)).SetIconCoord(6, 1).SetItemName("brick");
		public static Item Clay = (new Item(81)).SetIconCoord(9, 3).SetItemName("clay");
		public static Item Reed;
		public static Item Paper = (new Item(83)).SetIconCoord(10, 3).SetItemName("paper");
		public static Item Book = (new Item(84)).SetIconCoord(11, 3).SetItemName("book");
		public static Item SlimeBall = (new Item(85)).SetIconCoord(14, 1).SetItemName("slimeball");
		public static Item MinecartCrate = (new ItemMinecart(86, 1)).SetIconCoord(7, 9).SetItemName("minecartChest");
		public static Item MinecartPowered = (new ItemMinecart(87, 2)).SetIconCoord(7, 10).SetItemName("minecartFurnace");
		public static Item Egg = (new ItemEgg(88)).SetIconCoord(12, 0).SetItemName("egg");
		public static Item Compass = (new Item(89)).SetIconCoord(6, 3).SetItemName("compass");
		public static Item FishingRod = (new ItemFishingRod(90)).SetIconCoord(5, 4).SetItemName("fishingRod");
		public static Item PocketSundial = (new Item(91)).SetIconCoord(6, 4).SetItemName("clock");
		public static Item LightStoneDust;
		public static Item FishRaw = (new ItemFood(93, 2, 0.3F, false)).SetIconCoord(9, 5).SetItemName("fishRaw");
		public static Item FishCooked = (new ItemFood(94, 5, 0.6F, false)).SetIconCoord(10, 5).SetItemName("fishCooked");
		public static Item DyePowder = (new ItemDye(95)).SetIconCoord(14, 4).SetItemName("dyePowder");
		public static Item Bone = (new Item(96)).SetIconCoord(12, 1).SetItemName("bone").SetFull3D();
		public static Item Sugar;
		public static Item Cake;
		public static Item Bed = (new ItemBed(99)).SetMaxStackSize(1).SetIconCoord(13, 2).SetItemName("bed");
		public static Item RedstoneRepeater;
		public static Item Cookie = (new ItemFood(101, 1, 0.1F, false)).SetIconCoord(12, 5).SetItemName("cookie");
		public static ItemMap Map = (ItemMap)(new ItemMap(102)).SetIconCoord(12, 3).SetItemName("map");

		/// <summary>
		/// Item introduced on 1.7 version, is a shear to cut leaves (you can keep the block) or get wool from sheeps.
		/// </summary>
		public static ItemShears Shears = (ItemShears)(new ItemShears(103)).SetIconCoord(13, 5).SetItemName("shears");
		public static Item Melon = (new ItemFood(104, 2, 0.3F, false)).SetIconCoord(13, 6).SetItemName("melon");
		public static Item PumpkinSeeds;
		public static Item MelonSeeds;
		public static Item BeefRaw = (new ItemFood(107, 3, 0.3F, true)).SetIconCoord(9, 6).SetItemName("beefRaw");
		public static Item BeefCooked = (new ItemFood(108, 8, 0.8F, true)).SetIconCoord(10, 6).SetItemName("beefCooked");
		public static Item ChickenRaw;
		public static Item ChickenCooked = (new ItemFood(110, 6, 0.6F, true)).SetIconCoord(10, 7).SetItemName("chickenCooked");
		public static Item RottenFlesh;
		public static Item EnderPearl = (new ItemEnderPearl(112)).SetIconCoord(11, 6).SetItemName("enderPearl");
		public static Item BlazeRod = (new Item(113)).SetIconCoord(12, 6).SetItemName("blazeRod");
		public static Item GhastTear;
		public static Item GoldNugget = (new Item(115)).SetIconCoord(12, 7).SetItemName("goldNugget");
		public static Item NetherStalkSeeds;
		public static ItemPotion Potion = (ItemPotion)(new ItemPotion(117)).SetIconCoord(13, 8).SetItemName("potion");
		public static Item GlassBottle = (new ItemGlassBottle(118)).SetIconCoord(12, 8).SetItemName("glassBottle");
		public static Item SpiderEye;
		public static Item FermentedSpiderEye;
		public static Item BlazePowder;
		public static Item MagmaCream;
		public static Item BrewingStand;
		public static Item Cauldron;
		public static Item EyeOfEnder = (new ItemEnderEye(125)).SetIconCoord(11, 9).SetItemName("eyeOfEnder");
		public static Item SpeckledMelon;
		public static Item MonsterPlacer = (new ItemMonsterPlacer(127)).SetIconCoord(9, 9).SetItemName("monsterPlacer");

		/// <summary>
		/// Bottle o' Enchanting. Drops between 1 and 3 experience orbs when thrown.
		/// </summary>
		public static Item ExpBottle = (new ItemExpBottle(128)).SetIconCoord(11, 10).SetItemName("expBottle");

		/// <summary>
		/// Fire Charge. When used in a dispenser it fires a fireball similiar to a Ghast's.
		/// </summary>
		public static Item FireballCharge = (new ItemFireball(129)).SetIconCoord(14, 2).SetItemName("fireball");
		public static Item Record13 = (new ItemRecord(2000, "13")).SetIconCoord(0, 15).SetItemName("record");
		public static Item RecordCat = (new ItemRecord(2001, "cat")).SetIconCoord(1, 15).SetItemName("record");
		public static Item RecordBlocks = (new ItemRecord(2002, "blocks")).SetIconCoord(2, 15).SetItemName("record");
		public static Item RecordChirp = (new ItemRecord(2003, "chirp")).SetIconCoord(3, 15).SetItemName("record");
		public static Item RecordFar = (new ItemRecord(2004, "far")).SetIconCoord(4, 15).SetItemName("record");
		public static Item RecordMall = (new ItemRecord(2005, "mall")).SetIconCoord(5, 15).SetItemName("record");
		public static Item RecordMellohi = (new ItemRecord(2006, "mellohi")).SetIconCoord(6, 15).SetItemName("record");
		public static Item RecordStal = (new ItemRecord(2007, "stal")).SetIconCoord(7, 15).SetItemName("record");
		public static Item RecordStrad = (new ItemRecord(2008, "strad")).SetIconCoord(8, 15).SetItemName("record");
		public static Item RecordWard = (new ItemRecord(2009, "ward")).SetIconCoord(9, 15).SetItemName("record");
		public static Item Record11 = (new ItemRecord(2010, "11")).SetIconCoord(10, 15).SetItemName("record");

		/// <summary>
		/// Item index + 256 </summary>
		public readonly int ShiftedIndex;

		/// <summary>
		/// Maximum size of the stack. </summary>
		protected int MaxStackSize;

		/// <summary>
		/// Maximum damage an item can handle. </summary>
		private int MaxDamage;

		/// <summary>
		/// Icon index in the icons table. </summary>
		protected int IconIndex;

		/// <summary>
		/// If true, render the object in full 3D, like weapons and tools. </summary>
		protected bool BFull3D;

		/// <summary>
		/// Some items (like dyes) have multiple subtypes on same item, this is field define this behavior
		/// </summary>
		protected bool HasSubtypes;
		private Item ContainerItem;
		private string PotionEffect;

		/// <summary>
		/// full name of item from language file </summary>
		private string ItemName;

		protected Item(int par1)
		{
			MaxStackSize = 64;
			MaxDamage = 0;
			BFull3D = false;
			HasSubtypes = false;
			ContainerItem = null;
			PotionEffect = null;
			ShiftedIndex = 256 + par1;

			if (ItemsList[256 + par1] != null)
			{
				Console.WriteLine((new StringBuilder()).Append("CONFLICT @ ").Append(par1).ToString());
			}

			ItemsList[256 + par1] = this;
		}

		/// <summary>
		/// Sets the icon index for this item. Returns the item.
		/// </summary>
		public virtual Item SetIconIndex(int par1)
		{
			IconIndex = par1;
			return this;
		}

		public virtual Item SetMaxStackSize(int par1)
		{
			MaxStackSize = par1;
			return this;
		}

		public virtual Item SetIconCoord(int par1, int par2)
		{
			IconIndex = par1 + par2 * 16;
			return this;
		}

		/// <summary>
		/// Gets an icon index based on an item's damage value
		/// </summary>
		public virtual int GetIconFromDamage(int par1)
		{
			return IconIndex;
		}

		/// <summary>
		/// Returns the icon index of the stack given as argument.
		/// </summary>
		public int GetIconIndex(ItemStack par1ItemStack)
		{
			return GetIconFromDamage(par1ItemStack.GetItemDamage());
		}

		/// <summary>
		/// Callback for item usage. If the item does something special on right clicking, he will have one of those. Return
		/// True if something happen and false if it don't. This is for ITEMS, not BLOCKS !
		/// </summary>
		public virtual bool OnItemUse(ItemStack par1ItemStack, EntityPlayer par2EntityPlayer, World par3World, int i, int j, int k, int l)
		{
			return false;
		}

		/// <summary>
		/// Returns the strength of the stack against a given block. 1.0F base, (Quality+1)*2 if correct blocktype, 1.5F if
		/// sword
		/// </summary>
		public virtual float GetStrVsBlock(ItemStack par1ItemStack, Block par2Block)
		{
			return 1.0F;
		}

		/// <summary>
		/// Called whenever this item is equipped and the right mouse button is pressed. Args: itemStack, world, entityPlayer
		/// </summary>
		public virtual ItemStack OnItemRightClick(ItemStack par1ItemStack, World par2World, EntityPlayer par3EntityPlayer)
		{
			return par1ItemStack;
		}

		public virtual ItemStack OnFoodEaten(ItemStack par1ItemStack, World par2World, EntityPlayer par3EntityPlayer)
		{
			return par1ItemStack;
		}

		/// <summary>
		/// Returns the maximum size of the stack for a specific item. *Isn't this more a Set than a Get?*
		/// </summary>
		public virtual int GetItemStackLimit()
		{
			return MaxStackSize;
		}

		/// <summary>
		/// Returns the metadata of the block which this Item (ItemBlock) can place
		/// </summary>
		public virtual int GetMetadata(int par1)
		{
			return 0;
		}

		public virtual bool GetHasSubtypes()
		{
			return HasSubtypes;
		}

		protected virtual Item SetHasSubtypes(bool par1)
		{
			HasSubtypes = par1;
			return this;
		}

		/// <summary>
		/// Returns the maximum damage an item can take.
		/// </summary>
		public virtual int GetMaxDamage()
		{
			return MaxDamage;
		}

		/// <summary>
		/// set max damage of an Item
		/// </summary>
		protected virtual Item SetMaxDamage(int par1)
		{
			MaxDamage = par1;
			return this;
		}

		public virtual bool IsDamageable()
		{
			return MaxDamage > 0 && !HasSubtypes;
		}

		/// <summary>
		/// Current implementations of this method in child classes do not use the entry argument beside ev. They just raise
		/// the damage on the stack.
		/// </summary>
		public virtual bool HitEntity(ItemStack par1ItemStack, EntityLiving par2EntityLiving, EntityLiving par3EntityLiving)
		{
			return false;
		}

		public virtual bool OnBlockDestroyed(ItemStack par1ItemStack, int par2, int par3, int i, int j, EntityLiving entityliving)
		{
			return false;
		}

		/// <summary>
		/// Returns the damage against a given entity.
		/// </summary>
		public virtual int GetDamageVsEntity(Entity par1Entity)
		{
			return 1;
		}

		/// <summary>
		/// Returns if the item (tool) can harvest results from the block type.
		/// </summary>
		public virtual bool CanHarvestBlock(Block par1Block)
		{
			return false;
		}

		/// <summary>
		/// Called when a player right clicks a entity with a item.
		/// </summary>
		public virtual void UseItemOnEntity(ItemStack itemstack, EntityLiving entityliving)
		{
		}

		/// <summary>
		/// Sets bFull3D to True and return the object.
		/// </summary>
		public virtual Item SetFull3D()
		{
			BFull3D = true;
			return this;
		}

		/// <summary>
		/// Returns True is the item is renderer in full 3D when hold.
		/// </summary>
		public virtual bool IsFull3D()
		{
			return BFull3D;
		}

		/// <summary>
		/// Returns true if this item should be rotated by 180 degrees around the Y axis when being held in an entities
		/// hands.
		/// </summary>
		public virtual bool ShouldRotateAroundWhenRendering()
		{
			return false;
		}

		/// <summary>
		/// set name of item from language file
		/// </summary>
		public virtual Item SetItemName(string par1Str)
		{
			ItemName = (new StringBuilder()).Append("item.").Append(par1Str).ToString();
			return this;
		}

		public virtual string GetLocalItemName(ItemStack par1ItemStack)
		{
			string s = GetItemNameIS(par1ItemStack);

			if (s == null)
			{
				return "";
			}
			else
			{
				return StatCollector.TranslateToLocal(s);
			}
		}

		public virtual string GetItemName()
		{
			return ItemName;
		}

		public virtual string GetItemNameIS(ItemStack par1ItemStack)
		{
			return ItemName;
		}

		public virtual Item SetContainerItem(Item par1Item)
		{
			ContainerItem = par1Item;
			return this;
		}

		/// <summary>
		/// If this returns true, after a recipe involving this item is crafted the container item will be added to the
		/// player's inventory instead of remaining in the crafting grid.
		/// </summary>
		public virtual bool DoesContainerItemLeaveCraftingGrid(ItemStack par1ItemStack)
		{
			return true;
		}

		public virtual bool Func_46056_k()
		{
			return false;
		}

		public virtual Item GetContainerItem()
		{
			return ContainerItem;
		}

		/// <summary>
		/// True if this Item has a container item (a.k.a. crafting result)
		/// </summary>
		public virtual bool HasContainerItem()
		{
			return ContainerItem != null;
		}

		public virtual string GetStatName()
		{
			return StatCollector.TranslateToLocal((new StringBuilder()).Append(GetItemName()).Append(".name").ToString());
		}

		public virtual int GetColorFromDamage(int par1, int par2)
		{
			return 0xffffff;
		}

		/// <summary>
		/// Called each tick as long the item is on a player inventory. Uses by maps to check if is on a player hand and
		/// update it's contents.
		/// </summary>
		public virtual void OnUpdate(ItemStack itemstack, World world, Entity entity, int i, bool flag)
		{
		}

		/// <summary>
		/// Called when item is crafted/smelted. Used only by maps so far.
		/// </summary>
		public virtual void OnCreated(ItemStack itemstack, World world, EntityPlayer entityplayer)
		{
		}

		/// <summary>
		/// returns the action that specifies what animation to play when the items is being used
		/// </summary>
		public virtual EnumAction GetItemUseAction(ItemStack par1ItemStack)
		{
			return EnumAction.None;
		}

		/// <summary>
		/// How long it takes to use or consume an item
		/// </summary>
		public virtual int GetMaxItemUseDuration(ItemStack par1ItemStack)
		{
			return 0;
		}

		/// <summary>
		/// called when the player releases the use item button. Args: itemstack, world, entityplayer, itemInUseCount
		/// </summary>
		public virtual void OnPlayerStoppedUsing(ItemStack itemstack, World world, EntityPlayer entityplayer, int i)
		{
		}

		/// <summary>
		/// Sets the string representing this item's effect on a potion when used as an ingredient.
		/// </summary>
		protected virtual Item SetPotionEffect(string par1Str)
		{
			PotionEffect = par1Str;
			return this;
		}

		/// <summary>
		/// Returns a string representing what this item does to a potion.
		/// </summary>
		public virtual string GetPotionEffect()
		{
			return PotionEffect;
		}

		/// <summary>
		/// Returns true if this item serves as a potion ingredient (its ingredient information is not null).
		/// </summary>
		public virtual bool IsPotionIngredient()
		{
			return PotionEffect != null;
		}

		/// <summary>
		/// allows items to add custom lines of information to the mouseover description
		/// </summary>
        public virtual void AddInformation(ItemStack itemstack, List<string> list)
		{
		}

		public virtual string GetItemDisplayName(ItemStack par1ItemStack)
		{
			string s = (new StringBuilder()).Append("").Append(StringTranslate.GetInstance().TranslateNamedKey(GetLocalItemName(par1ItemStack))).ToString().Trim();
			return s;
		}

		public virtual bool HasEffect(ItemStack par1ItemStack)
		{
			return par1ItemStack.IsItemEnchanted();
		}

		/// <summary>
		/// Return an item rarity from EnumRarity
		/// </summary>
		public virtual Rarity GetRarity(ItemStack par1ItemStack)
		{
			if (par1ItemStack.IsItemEnchanted())
			{
				return Rarity.Rare;
			}
			else
			{
				return Rarity.Common;
			}
		}

		/// <summary>
		/// Checks isDamagable and if it cannot be stacked
		/// </summary>
		public virtual bool IsItemTool(ItemStack par1ItemStack)
		{
			return GetItemStackLimit() == 1 && IsDamageable();
		}

		protected virtual MovingObjectPosition GetMovingObjectPositionFromPlayer(World par1World, EntityPlayer par2EntityPlayer, bool par3)
		{
			float f = 1.0F;
			float f1 = par2EntityPlayer.PrevRotationPitch + (par2EntityPlayer.RotationPitch - par2EntityPlayer.PrevRotationPitch) * f;
			float f2 = par2EntityPlayer.PrevRotationYaw + (par2EntityPlayer.RotationYaw - par2EntityPlayer.PrevRotationYaw) * f;
			double d = par2EntityPlayer.PrevPosX + (par2EntityPlayer.PosX - par2EntityPlayer.PrevPosX) * (double)f;
			double d1 = (par2EntityPlayer.PrevPosY + (par2EntityPlayer.PosY - par2EntityPlayer.PrevPosY) * (double)f + 1.6200000000000001D) - (double)par2EntityPlayer.YOffset;
			double d2 = par2EntityPlayer.PrevPosZ + (par2EntityPlayer.PosZ - par2EntityPlayer.PrevPosZ) * (double)f;
			Vec3D vec3d = Vec3D.CreateVector(d, d1, d2);
			float f3 = MathHelper2.Cos(-f2 * 0.01745329F - (float)Math.PI);
			float f4 = MathHelper2.Sin(-f2 * 0.01745329F - (float)Math.PI);
			float f5 = -MathHelper2.Cos(-f1 * 0.01745329F);
			float f6 = MathHelper2.Sin(-f1 * 0.01745329F);
			float f7 = f4 * f5;
			float f8 = f6;
			float f9 = f3 * f5;
			double d3 = 5D;
			Vec3D vec3d1 = vec3d.AddVector((double)f7 * d3, (double)f8 * d3, (double)f9 * d3);
			MovingObjectPosition movingobjectposition = par1World.RayTraceBlocks_do_do(vec3d, vec3d1, par3, !par3);
			return movingobjectposition;
		}

		/// <summary>
		/// Return the enchantability factor of the item, most of the time is based on material.
		/// </summary>
		public virtual int GetItemEnchantability()
		{
			return 0;
		}

		public virtual bool Func_46058_c()
		{
			return false;
		}

		public virtual int Func_46057_a(int par1, int par2)
		{
			return GetIconFromDamage(par1);
		}

		static Item()
		{
			ShovelSteel = (new ItemSpade(0, ToolMaterial.IRON)).SetIconCoord(2, 5).SetItemName("shovelIron");
			PickaxeSteel = (new ItemPickaxe(1, ToolMaterial.IRON)).SetIconCoord(2, 6).SetItemName("pickaxeIron");
			AxeSteel = (new ItemAxe(2, ToolMaterial.IRON)).SetIconCoord(2, 7).SetItemName("hatchetIron");
			SwordSteel = (new ItemSword(11, ToolMaterial.IRON)).SetIconCoord(2, 4).SetItemName("swordIron");
			SwordWood = (new ItemSword(12, ToolMaterial.WOOD)).SetIconCoord(0, 4).SetItemName("swordWood");
			ShovelWood = (new ItemSpade(13, ToolMaterial.WOOD)).SetIconCoord(0, 5).SetItemName("shovelWood");
			PickaxeWood = (new ItemPickaxe(14, ToolMaterial.WOOD)).SetIconCoord(0, 6).SetItemName("pickaxeWood");
			AxeWood = (new ItemAxe(15, ToolMaterial.WOOD)).SetIconCoord(0, 7).SetItemName("hatchetWood");
			SwordStone = (new ItemSword(16, ToolMaterial.STONE)).SetIconCoord(1, 4).SetItemName("swordStone");
			ShovelStone = (new ItemSpade(17, ToolMaterial.STONE)).SetIconCoord(1, 5).SetItemName("shovelStone");
			PickaxeStone = (new ItemPickaxe(18, ToolMaterial.STONE)).SetIconCoord(1, 6).SetItemName("pickaxeStone");
			AxeStone = (new ItemAxe(19, ToolMaterial.STONE)).SetIconCoord(1, 7).SetItemName("hatchetStone");
			SwordDiamond = (new ItemSword(20, ToolMaterial.EMERALD)).SetIconCoord(3, 4).SetItemName("swordDiamond");
			ShovelDiamond = (new ItemSpade(21, ToolMaterial.EMERALD)).SetIconCoord(3, 5).SetItemName("shovelDiamond");
			PickaxeDiamond = (new ItemPickaxe(22, ToolMaterial.EMERALD)).SetIconCoord(3, 6).SetItemName("pickaxeDiamond");
			AxeDiamond = (new ItemAxe(23, ToolMaterial.EMERALD)).SetIconCoord(3, 7).SetItemName("hatchetDiamond");
			SwordGold = (new ItemSword(27, ToolMaterial.GOLD)).SetIconCoord(4, 4).SetItemName("swordGold");
			ShovelGold = (new ItemSpade(28, ToolMaterial.GOLD)).SetIconCoord(4, 5).SetItemName("shovelGold");
			PickaxeGold = (new ItemPickaxe(29, ToolMaterial.GOLD)).SetIconCoord(4, 6).SetItemName("pickaxeGold");
			AxeGold = (new ItemAxe(30, ToolMaterial.GOLD)).SetIconCoord(4, 7).SetItemName("hatchetGold");
			Gunpowder = (new Item(33)).SetIconCoord(8, 2).SetItemName("sulphur").SetPotionEffect(PotionHelper.GunpowderEffect);
			HoeWood = (new ItemHoe(34, ToolMaterial.WOOD)).SetIconCoord(0, 8).SetItemName("hoeWood");
			HoeStone = (new ItemHoe(35, ToolMaterial.STONE)).SetIconCoord(1, 8).SetItemName("hoeStone");
			HoeSteel = (new ItemHoe(36, ToolMaterial.IRON)).SetIconCoord(2, 8).SetItemName("hoeIron");
			HoeDiamond = (new ItemHoe(37, ToolMaterial.EMERALD)).SetIconCoord(3, 8).SetItemName("hoeDiamond");
			HoeGold = (new ItemHoe(38, ToolMaterial.GOLD)).SetIconCoord(4, 8).SetItemName("hoeGold");
			Seeds = (new ItemSeeds(39, Block.Crops.BlockID, Block.TilledField.BlockID)).SetIconCoord(9, 0).SetItemName("seeds");
			HelmetLeather = (new ItemArmor(42, ArmorMaterial.CLOTH, 0, 0)).SetIconCoord(0, 0).SetItemName("helmetCloth");
			PlateLeather = (new ItemArmor(43, ArmorMaterial.CLOTH, 0, 1)).SetIconCoord(0, 1).SetItemName("chestplateCloth");
			LegsLeather = (new ItemArmor(44, ArmorMaterial.CLOTH, 0, 2)).SetIconCoord(0, 2).SetItemName("leggingsCloth");
			BootsLeather = (new ItemArmor(45, ArmorMaterial.CLOTH, 0, 3)).SetIconCoord(0, 3).SetItemName("bootsCloth");
			HelmetChain = (new ItemArmor(46, ArmorMaterial.CHAIN, 1, 0)).SetIconCoord(1, 0).SetItemName("helmetChain");
			PlateChain = (new ItemArmor(47, ArmorMaterial.CHAIN, 1, 1)).SetIconCoord(1, 1).SetItemName("chestplateChain");
			LegsChain = (new ItemArmor(48, ArmorMaterial.CHAIN, 1, 2)).SetIconCoord(1, 2).SetItemName("leggingsChain");
			BootsChain = (new ItemArmor(49, ArmorMaterial.CHAIN, 1, 3)).SetIconCoord(1, 3).SetItemName("bootsChain");
			HelmetSteel = (new ItemArmor(50, ArmorMaterial.IRON, 2, 0)).SetIconCoord(2, 0).SetItemName("helmetIron");
			PlateSteel = (new ItemArmor(51, ArmorMaterial.IRON, 2, 1)).SetIconCoord(2, 1).SetItemName("chestplateIron");
			LegsSteel = (new ItemArmor(52, ArmorMaterial.IRON, 2, 2)).SetIconCoord(2, 2).SetItemName("leggingsIron");
			BootsSteel = (new ItemArmor(53, ArmorMaterial.IRON, 2, 3)).SetIconCoord(2, 3).SetItemName("bootsIron");
			HelmetDiamond = (new ItemArmor(54, ArmorMaterial.DIAMOND, 3, 0)).SetIconCoord(3, 0).SetItemName("helmetDiamond");
			PlateDiamond = (new ItemArmor(55, ArmorMaterial.DIAMOND, 3, 1)).SetIconCoord(3, 1).SetItemName("chestplateDiamond");
			LegsDiamond = (new ItemArmor(56, ArmorMaterial.DIAMOND, 3, 2)).SetIconCoord(3, 2).SetItemName("leggingsDiamond");
			BootsDiamond = (new ItemArmor(57, ArmorMaterial.DIAMOND, 3, 3)).SetIconCoord(3, 3).SetItemName("bootsDiamond");
			HelmetGold = (new ItemArmor(58, ArmorMaterial.GOLD, 4, 0)).SetIconCoord(4, 0).SetItemName("helmetGold");
			PlateGold = (new ItemArmor(59, ArmorMaterial.GOLD, 4, 1)).SetIconCoord(4, 1).SetItemName("chestplateGold");
			LegsGold = (new ItemArmor(60, ArmorMaterial.GOLD, 4, 2)).SetIconCoord(4, 2).SetItemName("leggingsGold");
			BootsGold = (new ItemArmor(61, ArmorMaterial.GOLD, 4, 3)).SetIconCoord(4, 3).SetItemName("bootsGold");
			AppleGold = (new ItemAppleGold(66, 4, 1.2F, false)).SetAlwaysEdible().SetPotionEffect(net.minecraft.src.Potion.Regeneration.Id, 5, 0, 1.0F).SetIconCoord(11, 0).SetItemName("appleGold");
			DoorWood = (new ItemDoor(68, Material.Wood)).SetIconCoord(11, 2).SetItemName("doorWood");
			BucketEmpty = (new ItemBucket(69, 0)).SetIconCoord(10, 4).SetItemName("bucket");
			BucketWater = (new ItemBucket(70, Block.WaterMoving.BlockID)).SetIconCoord(11, 4).SetItemName("bucketWater").SetContainerItem(BucketEmpty);
			BucketLava = (new ItemBucket(71, Block.LavaMoving.BlockID)).SetIconCoord(12, 4).SetItemName("bucketLava").SetContainerItem(BucketEmpty);
			DoorSteel = (new ItemDoor(74, Material.Iron)).SetIconCoord(12, 2).SetItemName("doorIron");
			Redstone = (new ItemRedstone(75)).SetIconCoord(8, 3).SetItemName("redstone").SetPotionEffect(PotionHelper.RedstoneEffect);
			BucketMilk = (new ItemBucketMilk(79)).SetIconCoord(13, 4).SetItemName("milk").SetContainerItem(BucketEmpty);
			Reed = (new ItemReed(82, Block.Reed)).SetIconCoord(11, 1).SetItemName("reeds");
			LightStoneDust = (new Item(92)).SetIconCoord(9, 4).SetItemName("yellowDust").SetPotionEffect(PotionHelper.GlowstoneEffect);
			Sugar = (new Item(97)).SetIconCoord(13, 0).SetItemName("sugar").SetPotionEffect(PotionHelper.SugarEffect);
			Cake = (new ItemReed(98, Block.Cake)).SetMaxStackSize(1).SetIconCoord(13, 1).SetItemName("cake");
			RedstoneRepeater = (new ItemReed(100, Block.RedstoneRepeaterIdle)).SetIconCoord(6, 5).SetItemName("diode");
			PumpkinSeeds = (new ItemSeeds(105, Block.PumpkinStem.BlockID, Block.TilledField.BlockID)).SetIconCoord(13, 3).SetItemName("seeds_pumpkin");
			MelonSeeds = (new ItemSeeds(106, Block.MelonStem.BlockID, Block.TilledField.BlockID)).SetIconCoord(14, 3).SetItemName("seeds_melon");
			ChickenRaw = (new ItemFood(109, 2, 0.3F, true)).SetPotionEffect(net.minecraft.src.Potion.Hunger.Id, 30, 0, 0.3F).SetIconCoord(9, 7).SetItemName("chickenRaw");
			RottenFlesh = (new ItemFood(111, 4, 0.1F, true)).SetPotionEffect(net.minecraft.src.Potion.Hunger.Id, 30, 0, 0.8F).SetIconCoord(11, 5).SetItemName("rottenFlesh");
			GhastTear = (new Item(114)).SetIconCoord(11, 7).SetItemName("ghastTear").SetPotionEffect(PotionHelper.GhastTearEffect);
			NetherStalkSeeds = (new ItemSeeds(116, Block.NetherStalk.BlockID, Block.SlowSand.BlockID)).SetIconCoord(13, 7).SetItemName("netherStalkSeeds").SetPotionEffect("+4");
			SpiderEye = (new ItemFood(119, 2, 0.8F, false)).SetPotionEffect(net.minecraft.src.Potion.Poison.Id, 5, 0, 1.0F).SetIconCoord(11, 8).SetItemName("spiderEye").SetPotionEffect(PotionHelper.SpiderEyeEffect);
			FermentedSpiderEye = (new Item(120)).SetIconCoord(10, 8).SetItemName("fermentedSpiderEye").SetPotionEffect(PotionHelper.FermentedSpiderEyeEffect);
			BlazePowder = (new Item(121)).SetIconCoord(13, 9).SetItemName("blazePowder").SetPotionEffect(PotionHelper.BlazePowderEffect);
			MagmaCream = (new Item(122)).SetIconCoord(13, 10).SetItemName("magmaCream").SetPotionEffect(PotionHelper.MagmaCreamEffect);
			BrewingStand = (new ItemReed(123, Block.BrewingStand)).SetIconCoord(12, 10).SetItemName("brewingStand");
			Cauldron = (new ItemReed(124, Block.Cauldron)).SetIconCoord(12, 9).SetItemName("cauldron");
			SpeckledMelon = (new Item(126)).SetIconCoord(9, 8).SetItemName("speckledMelon").SetPotionEffect(PotionHelper.SpeckledMelonEffect);
			//StatList.InitStats();
		}
	}
}