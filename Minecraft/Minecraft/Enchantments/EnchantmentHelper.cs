using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class EnchantmentHelper
	{
		/// <summary>
		/// Is the random seed of enchantment effects. </summary>
		private static readonly Random EnchantmentRand = new Random();

		/// <summary>
		/// Used to calculate the extra armor of enchantments on armors equipped on player.
		/// </summary>
		private static readonly EnchantmentModifierDamage EnchantmentModifierDamage = new EnchantmentModifierDamage(null);

		/// <summary>
		/// Used to calculate the (magic) extra damage done by enchantments on current equipped item of player.
		/// </summary>
		private static readonly EnchantmentModifierLiving EnchantmentModifierLiving = new EnchantmentModifierLiving(null);

		public EnchantmentHelper()
		{
		}

		/// <summary>
		/// Returns the level of enchantment on the ItemStack passed.
		/// </summary>
		public static int GetEnchantmentLevel(int par0, ItemStack par1ItemStack)
		{
			if (par1ItemStack == null)
			{
				return 0;
			}

			NBTTagList nbttaglist = par1ItemStack.GetEnchantmentTagList();

			if (nbttaglist == null)
			{
				return 0;
			}

			for (int i = 0; i < nbttaglist.TagCount(); i++)
			{
				short word0 = ((NBTTagCompound)nbttaglist.TagAt(i)).GetShort("id");
				short word1 = ((NBTTagCompound)nbttaglist.TagAt(i)).GetShort("lvl");

				if (word0 == par0)
				{
					return word1;
				}
			}

			return 0;
		}

		/// <summary>
		/// Returns the biggest level of the enchantment on the array of ItemStack passed.
		/// </summary>
		private static int GetMaxEnchantmentLevel(int par0, ItemStack[] par1ArrayOfItemStack)
		{
			int i = 0;
			ItemStack[] aitemstack = par1ArrayOfItemStack;
			int j = aitemstack.Length;

			for (int k = 0; k < j; k++)
			{
				ItemStack itemstack = aitemstack[k];
				int l = GetEnchantmentLevel(par0, itemstack);

				if (l > i)
				{
					i = l;
				}
			}

			return i;
		}

		/// <summary>
		/// Executes the enchantment modifier on the ItemStack passed.
		/// </summary>
		private static void ApplyEnchantmentModifier(IEnchantmentModifier par0IEnchantmentModifier, ItemStack par1ItemStack)
		{
			if (par1ItemStack == null)
			{
				return;
			}

			NBTTagList nbttaglist = par1ItemStack.GetEnchantmentTagList();

			if (nbttaglist == null)
			{
				return;
			}

			for (int i = 0; i < nbttaglist.TagCount(); i++)
			{
				short word0 = ((NBTTagCompound)nbttaglist.TagAt(i)).GetShort("id");
				short word1 = ((NBTTagCompound)nbttaglist.TagAt(i)).GetShort("lvl");

				if (Enchantment.EnchantmentsList[word0] != null)
				{
					par0IEnchantmentModifier.CalculateModifier(Enchantment.EnchantmentsList[word0], word1);
				}
			}
		}

		/// <summary>
		/// Executes the enchantment modifier on the array of ItemStack passed.
		/// </summary>
		private static void ApplyEnchantmentModifierArray(IEnchantmentModifier par0IEnchantmentModifier, ItemStack[] par1ArrayOfItemStack)
		{
			ItemStack[] aitemstack = par1ArrayOfItemStack;
			int i = aitemstack.Length;

			for (int j = 0; j < i; j++)
			{
				ItemStack itemstack = aitemstack[j];
				ApplyEnchantmentModifier(par0IEnchantmentModifier, itemstack);
			}
		}

		/// <summary>
		/// Returns the modifier of protection enchantments on armors equipped on player.
		/// </summary>
		public static int GetEnchantmentModifierDamage(InventoryPlayer par0InventoryPlayer, DamageSource par1DamageSource)
		{
			EnchantmentModifierDamage.DamageModifier = 0;
			EnchantmentModifierDamage.DamageSource = par1DamageSource;
			ApplyEnchantmentModifierArray(EnchantmentModifierDamage, par0InventoryPlayer.ArmorInventory);

			if (EnchantmentModifierDamage.DamageModifier > 25)
			{
				EnchantmentModifierDamage.DamageModifier = 25;
			}

			return (EnchantmentModifierDamage.DamageModifier + 1 >> 1) + EnchantmentRand.Next((EnchantmentModifierDamage.DamageModifier >> 1) + 1);
		}

		/// <summary>
		/// Return the (magic) extra damage of the enchantments on player equipped item.
		/// </summary>
		public static int GetEnchantmentModifierLiving(InventoryPlayer par0InventoryPlayer, EntityLiving par1EntityLiving)
		{
			EnchantmentModifierLiving.LivingModifier = 0;
			EnchantmentModifierLiving.EntityLiving = par1EntityLiving;
			ApplyEnchantmentModifier(EnchantmentModifierLiving, par0InventoryPlayer.GetCurrentItem());

			if (EnchantmentModifierLiving.LivingModifier > 0)
			{
				return 1 + EnchantmentRand.Next(EnchantmentModifierLiving.LivingModifier);
			}
			else
			{
				return 0;
			}
		}

		/// <summary>
		/// Returns the knockback value of enchantments on equipped player item.
		/// </summary>
		public static int GetKnockbackModifier(InventoryPlayer par0InventoryPlayer, EntityLiving par1EntityLiving)
		{
			return GetEnchantmentLevel(Enchantment.Knockback.EffectId, par0InventoryPlayer.GetCurrentItem());
		}

		/// <summary>
		/// Return the fire aspect value of enchantments on equipped player item.
		/// </summary>
		public static int GetFireAspectModifier(InventoryPlayer par0InventoryPlayer, EntityLiving par1EntityLiving)
		{
			return GetEnchantmentLevel(Enchantment.FireAspect.EffectId, par0InventoryPlayer.GetCurrentItem());
		}

		/// <summary>
		/// Returns the 'Water Breathing' modifier of enchantments on player equipped armors.
		/// </summary>
		public static int GetRespiration(InventoryPlayer par0InventoryPlayer)
		{
			return GetMaxEnchantmentLevel(Enchantment.Respiration.EffectId, par0InventoryPlayer.ArmorInventory);
		}

		/// <summary>
		/// Return the extra efficiency of tools based on enchantments on equipped player item.
		/// </summary>
		public static int GetEfficiencyModifier(InventoryPlayer par0InventoryPlayer)
		{
			return GetEnchantmentLevel(Enchantment.Efficiency.EffectId, par0InventoryPlayer.GetCurrentItem());
		}

		/// <summary>
		/// Returns the unbreaking enchantment modifier on current equipped item of player.
		/// </summary>
		public static int GetUnbreakingModifier(InventoryPlayer par0InventoryPlayer)
		{
			return GetEnchantmentLevel(Enchantment.Unbreaking.EffectId, par0InventoryPlayer.GetCurrentItem());
		}

		/// <summary>
		/// Returns the silk touch status of enchantments on current equipped item of player.
		/// </summary>
		public static bool GetSilkTouchModifier(InventoryPlayer par0InventoryPlayer)
		{
			return GetEnchantmentLevel(Enchantment.SilkTouch.EffectId, par0InventoryPlayer.GetCurrentItem()) > 0;
		}

		/// <summary>
		/// Returns the fortune enchantment modifier of the current equipped item of player.
		/// </summary>
		public static int GetFortuneModifier(InventoryPlayer par0InventoryPlayer)
		{
			return GetEnchantmentLevel(Enchantment.Fortune.EffectId, par0InventoryPlayer.GetCurrentItem());
		}

		/// <summary>
		/// Returns the looting enchantment modifier of the current equipped item of player.
		/// </summary>
		public static int GetLootingModifier(InventoryPlayer par0InventoryPlayer)
		{
			return GetEnchantmentLevel(Enchantment.Looting.EffectId, par0InventoryPlayer.GetCurrentItem());
		}

		/// <summary>
		/// Returns the aqua affinity status of enchantments on current equipped item of player.
		/// </summary>
		public static bool GetAquaAffinityModifier(InventoryPlayer par0InventoryPlayer)
		{
			return GetMaxEnchantmentLevel(Enchantment.AquaAffinity.EffectId, par0InventoryPlayer.ArmorInventory) > 0;
		}

		/// <summary>
		/// Returns the enchantability of itemstack, it's uses a singular formula for each index (2nd parameter: 0, 1 and 2),
		/// cutting to the max enchantability power of the table (3rd parameter)
		/// </summary>
		public static int CalcItemStackEnchantability(Random par0Random, int par1, int par2, ItemStack par3ItemStack)
		{
			Item item = par3ItemStack.GetItem();
			int i = item.GetItemEnchantability();

			if (i <= 0)
			{
				return 0;
			}

			if (par2 > 30)
			{
				par2 = 30;
			}

			par2 = 1 + (par2 >> 1) + par0Random.Next(par2 + 1);
			int j = par0Random.Next(5) + par2;

			if (par1 == 0)
			{
				return (j >> 1) + 1;
			}

			if (par1 == 1)
			{
				return (j * 2) / 3 + 1;
			}
			else
			{
				return j;
			}
		}

		public static void Func_48441_a(Random par0Random, ItemStack par1ItemStack, int par2)
		{
			List<EnchantmentData> list = BuildEnchantmentList(par0Random, par1ItemStack, par2);

			if (list != null)
			{
				EnchantmentData enchantmentdata;

				for (IEnumerator<EnchantmentData> iterator = list.GetEnumerator(); iterator.MoveNext(); par1ItemStack.AddEnchantment(enchantmentdata.Enchantmentobj, enchantmentdata.EnchantmentLevel))
				{
					enchantmentdata = iterator.Current;
				}
			}
		}

		/// <summary>
		/// Create a list of random EnchantmentData (enchantments) that can be added together to the ItemStack, the 3rd
		/// parameter is the total enchantability level.
		/// </summary>
		public static List<EnchantmentData> BuildEnchantmentList(Random par0Random, ItemStack par1ItemStack, int par2)
		{
			Item item = par1ItemStack.GetItem();
			int i = item.GetItemEnchantability();

			if (i <= 0)
			{
				return null;
			}

			i = 1 + par0Random.Next((i >> 1) + 1) + par0Random.Next((i >> 1) + 1);
			int j = i + par2;
			float f = ((par0Random.NextFloat() + par0Random.NextFloat()) - 1.0F) * 0.25F;
			int k = (int)((float)j * (1.0F + f) + 0.5F);
			List<EnchantmentData> arraylist = null;
            Dictionary<int, EnchantmentData> map = MapEnchantmentData(k, par1ItemStack);

			if (map != null && map.Count > 0)
			{
				EnchantmentData enchantmentdata = (EnchantmentData)WeightedRandom.GetRandomItem(par0Random, map.Values);

				if (enchantmentdata != null)
				{
                    arraylist = new List<EnchantmentData>();
					arraylist.Add(enchantmentdata);

					for (int l = k >> 1; par0Random.Next(50) <= l; l >>= 1)
					{/*
						IEnumerator<int> iterator = map.Keys.GetEnumerator();

						do
						{
							if (!iterator.MoveNext())
							{
								break;
							}

							int integer = iterator.Current;
							bool flag = true;
							IEnumerator<EnchantmentData> iterator1 = arraylist.GetEnumerator();

							do
							{
								if (!iterator1.MoveNext())
								{
									break;
								}

								EnchantmentData enchantmentdata2 = iterator1.Current;

								if (enchantmentdata2.Enchantmentobj.CanApplyTogether(Enchantment.EnchantmentsList[integer]))
								{
									continue;
								}

								flag = false;
								break;
							}
							while (true);

							if (!flag)
							{
								iterator.Remove();
							}
						}
						while (true);
                        */
						if (map.Count > 0)
						{
							EnchantmentData enchantmentdata1 = (EnchantmentData)WeightedRandom.GetRandomItem(par0Random, map.Values);
							arraylist.Add(enchantmentdata1);
						}
					}
				}
			}

			return arraylist;
		}

		/// <summary>
		/// Creates a 'Map' of EnchantmentData (enchantments) possible to add on the ItemStack and the enchantability level
		/// passed.
		/// </summary>
        public static Dictionary<int, EnchantmentData> MapEnchantmentData(int par0, ItemStack par1ItemStack)
		{
			Item item = par1ItemStack.GetItem();
			Dictionary<int, EnchantmentData> hashmap = null;
			Enchantment[] aenchantment = Enchantment.EnchantmentsList;
			int i = aenchantment.Length;

			for (int j = 0; j < i; j++)
			{
				Enchantment enchantment = aenchantment[j];

				if (enchantment == null || !enchantment.Type.CanEnchantItem(item))
				{
					continue;
				}

				for (int k = enchantment.GetMinLevel(); k <= enchantment.GetMaxLevel(); k++)
				{
					if (par0 < enchantment.GetMinEnchantability(k) || par0 > enchantment.GetMaxEnchantability(k))
					{
						continue;
					}

					if (hashmap == null)
					{
                        hashmap = new Dictionary<int, EnchantmentData>();
					}

					hashmap[enchantment.EffectId] = new EnchantmentData(enchantment, k);
				}
			}

			return hashmap;
		}
	}
}