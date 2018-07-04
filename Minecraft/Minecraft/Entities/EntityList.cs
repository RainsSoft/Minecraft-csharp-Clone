using System;
using System.Collections.Generic;
using System.Text;

namespace net.minecraft.src
{
	public class EntityList
	{
		/// <summary>
		/// Provides a mapping between entity classes and a string </summary>
        private static Dictionary<string, Type> StringToClassMapping = new Dictionary<string, Type>();

		/// <summary>
		/// Provides a mapping between a string and an entity classes </summary>
        private static Dictionary<Type, string> ClassToStringMapping = new Dictionary<Type, string>();

		/// <summary>
		/// provides a mapping between an entityID and an Entity Class </summary>
        private static Dictionary<int, Type> IDtoClassMapping = new Dictionary<int, Type>();

		/// <summary>
		/// provides a mapping between an Entity Class and an entity ID </summary>
        private static Dictionary<Type, int> ClassToIDMapping = new Dictionary<Type, int>();

		/// <summary>
		/// Maps entity names to their numeric identifiers </summary>
        private static Dictionary<string, int> StringToIDMapping = new Dictionary<string, int>();

		/// <summary>
		/// This is a HashMap of the Creative Entity Eggs/Spawners. </summary>
        public static Dictionary<int, EntityEggInfo> EntityEggs = new Dictionary<int, EntityEggInfo>();

		public EntityList()
		{
		}

		/// <summary>
		/// adds a mapping between Entity classes and both a string representation and an ID
		/// </summary>
		private static void AddMapping(Type par0Class, string par1Str, int par2)
		{
			StringToClassMapping[par1Str] = par0Class;
			ClassToStringMapping[par0Class] = par1Str;
			IDtoClassMapping[par2] = par0Class;
			ClassToIDMapping[par0Class] = par2;
			StringToIDMapping[par1Str] = par2;
		}

		/// <summary>
		/// Adds a entity mapping with egg info.
		/// </summary>
		private static void AddMapping(Type par0Class, string par1Str, int par2, int par3, int par4)
		{
			AddMapping(par0Class, par1Str, par2);
			EntityEggs[par2] = new EntityEggInfo(par2, par3, par4);
		}

		/// <summary>
		/// Create a new instance of an entity in the world by using the entity name.
		/// </summary>
		public static Entity CreateEntityByName(string par0Str, World par1World)
		{
			Entity entity = null;

			try
			{
				Type class1 = StringToClassMapping[par0Str];

				if (class1 != null)
				{
					entity = (Entity)Activator.CreateInstance(class1, new object[] { par1World });
				}
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception.ToString());
				Console.Write(exception.StackTrace);
			}

			return entity;
		}

		/// <summary>
		/// create a new instance of an entity from NBT store
		/// </summary>
		public static Entity CreateEntityFromNBT(NBTTagCompound par0NBTTagCompound, World par1World)
		{
			Entity entity = null;

			try
			{
				Type class1 = StringToClassMapping[par0NBTTagCompound.GetString("id")];

				if (class1 != null)
				{
					entity = (Entity)Activator.CreateInstance(class1, new object[] { par1World });
				}
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception.ToString());
				Console.Write(exception.StackTrace);
			}

			if (entity != null)
			{
				entity.ReadFromNBT(par0NBTTagCompound);
			}
			else
			{
				Console.WriteLine((new StringBuilder()).Append("Skipping Entity with id ").Append(par0NBTTagCompound.GetString("id")).ToString());
			}

			return entity;
		}

		/// <summary>
		/// Create a new instance of an entity in the world by using an entity ID.
		/// </summary>
		public static Entity CreateEntityByID(int par0, World par1World)
		{
			Entity entity = null;

			try
			{
				Type class1 = IDtoClassMapping[par0];

				if (class1 != null)
				{
					entity = (Entity)Activator.CreateInstance(class1, new object[] { par1World });
				}
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception.ToString());
				Console.Write(exception.StackTrace);
			}

			if (entity == null)
			{
				Console.WriteLine((new StringBuilder()).Append("Skipping Entity with id ").Append(par0).ToString());
			}

			return entity;
		}

		/// <summary>
		/// gets the entityID of a specific entity
		/// </summary>
		public static int GetEntityID(Entity par0Entity)
		{
			return ClassToIDMapping[par0Entity.GetType()];
		}

		/// <summary>
		/// Gets the string representation of a specific entity.
		/// </summary>
		public static string GetEntityString(Entity par0Entity)
		{
			return ClassToStringMapping[par0Entity.GetType()];
		}

		/// <summary>
		/// Finds the class using IDtoClassMapping and classToStringMapping
		/// </summary>
		public static string GetStringFromID(int par0)
		{
			Type class1 = IDtoClassMapping[par0];

			if (class1 != null)
			{
				return ClassToStringMapping[class1];
			}
			else
			{
				return null;
			}
		}

		static EntityList()
		{
			AddMapping(typeof(net.minecraft.src.EntityItem), "Item", 1);
			AddMapping(typeof(net.minecraft.src.EntityXPOrb), "XPOrb", 2);
			AddMapping(typeof(net.minecraft.src.EntityPainting), "Painting", 9);
			AddMapping(typeof(net.minecraft.src.EntityArrow), "Arrow", 10);
			AddMapping(typeof(net.minecraft.src.EntitySnowball), "Snowball", 11);
			AddMapping(typeof(net.minecraft.src.EntityFireball), "Fireball", 12);
			AddMapping(typeof(net.minecraft.src.EntitySmallFireball), "SmallFireball", 13);
			AddMapping(typeof(net.minecraft.src.EntityEnderPearl), "ThrownEnderpearl", 14);
			AddMapping(typeof(net.minecraft.src.EntityEnderEye), "EyeOfEnderSignal", 15);
			AddMapping(typeof(net.minecraft.src.EntityPotion), "ThrownPotion", 16);
			AddMapping(typeof(net.minecraft.src.EntityExpBottle), "ThrownExpBottle", 17);
			AddMapping(typeof(net.minecraft.src.EntityTNTPrimed), "PrimedTnt", 20);
			AddMapping(typeof(net.minecraft.src.EntityFallingSand), "FallingSand", 21);
			AddMapping(typeof(net.minecraft.src.EntityMinecart), "Minecart", 40);
			AddMapping(typeof(net.minecraft.src.EntityBoat), "Boat", 41);
			AddMapping(typeof(net.minecraft.src.EntityLiving), "Mob", 48);
			AddMapping(typeof(net.minecraft.src.EntityMob), "Monster", 49);
			AddMapping(typeof(net.minecraft.src.EntityCreeper), "Creeper", 50, 0xda70b, 0);
			AddMapping(typeof(net.minecraft.src.EntitySkeleton), "Skeleton", 51, 0xc1c1c1, 0x494949);
			AddMapping(typeof(net.minecraft.src.EntitySpider), "Spider", 52, 0x342d27, 0xa80e0e);
			AddMapping(typeof(net.minecraft.src.EntityGiantZombie), "Giant", 53);
			AddMapping(typeof(net.minecraft.src.EntityZombie), "Zombie", 54, 44975, 0x799c65);
			AddMapping(typeof(net.minecraft.src.EntitySlime), "Slime", 55, 0x51a03e, 0x7ebf6e);
			AddMapping(typeof(net.minecraft.src.EntityGhast), "Ghast", 56, 0xf9f9f9, 0xbcbcbc);
			AddMapping(typeof(net.minecraft.src.EntityPigZombie), "PigZombie", 57, 0xea9393, 0x4c7129);
			AddMapping(typeof(net.minecraft.src.EntityEnderman), "Enderman", 58, 0x161616, 0);
			AddMapping(typeof(net.minecraft.src.EntityCaveSpider), "CaveSpider", 59, 0xc424e, 0xa80e0e);
			AddMapping(typeof(net.minecraft.src.EntitySilverfish), "Silverfish", 60, 0x6e6e6e, 0x303030);
			AddMapping(typeof(net.minecraft.src.EntityBlaze), "Blaze", 61, 0xf6b201, 0xfff87e);
			AddMapping(typeof(net.minecraft.src.EntityMagmaCube), "LavaSlime", 62, 0x340000, 0xfcfc00);
			AddMapping(typeof(net.minecraft.src.EntityDragon), "EnderDragon", 63);
			AddMapping(typeof(net.minecraft.src.EntityPig), "Pig", 90, 0xf0a5a2, 0xdb635f);
			AddMapping(typeof(net.minecraft.src.EntitySheep), "Sheep", 91, 0xe7e7e7, 0xffb5b5);
			AddMapping(typeof(net.minecraft.src.EntityCow), "Cow", 92, 0x443626, 0xa1a1a1);
			AddMapping(typeof(net.minecraft.src.EntityChicken), "Chicken", 93, 0xa1a1a1, 0xff0000);
			AddMapping(typeof(net.minecraft.src.EntitySquid), "Squid", 94, 0x223b4d, 0x708899);
			AddMapping(typeof(net.minecraft.src.EntityWolf), "Wolf", 95, 0xd7d3d3, 0xceaf96);
			AddMapping(typeof(net.minecraft.src.EntityMooshroom), "MushroomCow", 96, 0xa00f10, 0xb7b7b7);
			AddMapping(typeof(net.minecraft.src.EntitySnowman), "SnowMan", 97);
			AddMapping(typeof(net.minecraft.src.EntityOcelot), "Ozelot", 98, 0xefde7d, 0x564434);
			AddMapping(typeof(net.minecraft.src.EntityIronGolem), "VillagerGolem", 99);
			AddMapping(typeof(net.minecraft.src.EntityVillager), "Villager", 120, 0x563c33, 0xbd8b72);
			AddMapping(typeof(net.minecraft.src.EntityEnderCrystal), "EnderCrystal", 200);
		}
	}
}