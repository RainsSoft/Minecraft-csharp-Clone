using System;
using System.Collections.Generic;
using System.Text;

namespace net.minecraft.src
{
	public class ItemPotion : Item
	{
		/// <summary>
		/// maps potion damage values to lists of effect names </summary>
		private Dictionary<int, List<PotionEffect>> EffectCache;

		public ItemPotion(int par1) : base(par1)
		{
            EffectCache = new Dictionary<int, List<PotionEffect>>();
			SetMaxStackSize(1);
			SetHasSubtypes(true);
			SetMaxDamage(0);
		}

		/// <summary>
		/// Returns a list of potion effects for the specified itemstack.
		/// </summary>
		public virtual List<PotionEffect> GetEffects(ItemStack par1ItemStack)
		{
			return GetEffects(par1ItemStack.GetItemDamage());
		}

		/// <summary>
		/// Returns a list of effects for the specified potion damage value.
		/// </summary>
		public virtual List<PotionEffect> GetEffects(int par1)
		{
			List<PotionEffect> list = EffectCache[par1];

			if (list == null)
			{
				list = PotionHelper.GetPotionEffects(par1, false);
				EffectCache[par1] = list;
			}

			return list;
		}

		public override ItemStack OnFoodEaten(ItemStack par1ItemStack, World par2World, EntityPlayer par3EntityPlayer)
		{
			par1ItemStack.StackSize--;

			if (!par2World.IsRemote)
			{
				List<PotionEffect> list = GetEffects(par1ItemStack);

				if (list != null)
				{
					PotionEffect potioneffect;

					for (IEnumerator<PotionEffect> iterator = list.GetEnumerator(); iterator.MoveNext(); par3EntityPlayer.AddPotionEffect(new PotionEffect(potioneffect)))
					{
						potioneffect = iterator.Current;
					}
				}
			}

			if (par1ItemStack.StackSize <= 0)
			{
				return new ItemStack(Item.GlassBottle);
			}
			else
			{
				par3EntityPlayer.Inventory.AddItemStackToInventory(new ItemStack(Item.GlassBottle));
				return par1ItemStack;
			}
		}

		/// <summary>
		/// How long it takes to use or consume an item
		/// </summary>
		public override int GetMaxItemUseDuration(ItemStack par1ItemStack)
		{
			return 32;
		}

		/// <summary>
		/// returns the action that specifies what animation to play when the items is being used
		/// </summary>
		public override EnumAction GetItemUseAction(ItemStack par1ItemStack)
		{
			return EnumAction.Drink;
		}

		/// <summary>
		/// Called whenever this item is equipped and the right mouse button is pressed. Args: itemStack, world, entityPlayer
		/// </summary>
		public override ItemStack OnItemRightClick(ItemStack par1ItemStack, World par2World, EntityPlayer par3EntityPlayer)
		{
			if (IsSplash(par1ItemStack.GetItemDamage()))
			{
				par1ItemStack.StackSize--;
				par2World.PlaySoundAtEntity(par3EntityPlayer, "random.bow", 0.5F, 0.4F / ((float)ItemRand.NextDouble() * 0.4F + 0.8F));

				if (!par2World.IsRemote)
				{
					par2World.SpawnEntityInWorld(new EntityPotion(par2World, par3EntityPlayer, par1ItemStack.GetItemDamage()));
				}

				return par1ItemStack;
			}
			else
			{
				par3EntityPlayer.SetItemInUse(par1ItemStack, GetMaxItemUseDuration(par1ItemStack));
				return par1ItemStack;
			}
		}

		/// <summary>
		/// Callback for item usage. If the item does something special on right clicking, he will have one of those. Return
		/// True if something happen and false if it don't. This is for ITEMS, not BLOCKS !
		/// </summary>
		public override bool OnItemUse(ItemStack par1ItemStack, EntityPlayer par2EntityPlayer, World par3World, int i, int j, int k, int l)
		{
			return false;
		}

		/// <summary>
		/// Gets an icon index based on an item's damage value
		/// </summary>
		public override int GetIconFromDamage(int par1)
		{
			return !IsSplash(par1) ? 140 : 154;
		}

		public override int Func_46057_a(int par1, int par2)
		{
			if (par2 == 0)
			{
				return 141;
			}
			else
			{
				return base.Func_46057_a(par1, par2);
			}
		}

		/// <summary>
		/// returns wether or not a potion is a throwable splash potion based on damage value
		/// </summary>
		public static bool IsSplash(int par0)
		{
			return (par0 & 0x4000) != 0;
		}

		public override int GetColorFromDamage(int par1, int par2)
		{
			if (par2 > 0)
			{
				return 0xffffff;
			}
			else
			{
				return PotionHelper.Func_40358_a(par1, false);
			}
		}

		public override bool Func_46058_c()
		{
			return true;
		}

		public virtual bool IsEffectInstant(int par1)
		{
			List<PotionEffect> list = GetEffects(par1);

			if (list == null || list.Count == 0)
			{
				return false;
			}

			for (IEnumerator<PotionEffect> iterator = list.GetEnumerator(); iterator.MoveNext();)
			{
				PotionEffect potioneffect = iterator.Current;

				if (net.minecraft.src.Potion.PotionTypes[potioneffect.GetPotionID()].IsInstant())
				{
					return true;
				}
			}

			return false;
		}

		public override string GetItemDisplayName(ItemStack par1ItemStack)
		{
			if (par1ItemStack.GetItemDamage() == 0)
			{
				return StatCollector.TranslateToLocal("item.emptyPotion.name").Trim();
			}

			string s = "";

			if (IsSplash(par1ItemStack.GetItemDamage()))
			{
				s = (new StringBuilder()).Append(StatCollector.TranslateToLocal("potion.prefix.grenade").Trim()).Append(" ").ToString();
			}

			List<PotionEffect> list = Item.Potion.GetEffects(par1ItemStack);

			if (list != null && list.Count > 0)
			{
				string s1 = ((PotionEffect)list[0]).GetEffectName();
				s1 = (new StringBuilder()).Append(s1).Append(".postfix").ToString();
				return (new StringBuilder()).Append(s).Append(StatCollector.TranslateToLocal(s1).Trim()).ToString();
			}
			else
			{
				string s2 = PotionHelper.Func_40359_b(par1ItemStack.GetItemDamage());
				return (new StringBuilder()).Append(StatCollector.TranslateToLocal(s2).Trim()).Append(" ").Append(base.GetItemDisplayName(par1ItemStack)).ToString();
			}
		}

		/// <summary>
		/// allows items to add custom lines of information to the mouseover description
		/// </summary>
		public virtual void AddInformation(ItemStack par1ItemStack, List<string> par2List)
		{
			if (par1ItemStack.GetItemDamage() == 0)
			{
				return;
			}

			List<PotionEffect> list = Item.Potion.GetEffects(par1ItemStack);

			if (list != null && list.Count > 0)
			{
				for (IEnumerator<PotionEffect> iterator = list.GetEnumerator(); iterator.MoveNext();)
				{
					PotionEffect potioneffect = iterator.Current;
					string s1 = StatCollector.TranslateToLocal(potioneffect.GetEffectName()).Trim();

					if (potioneffect.GetAmplifier() > 0)
					{
						s1 = (new StringBuilder()).Append(s1).Append(" ").Append(StatCollector.TranslateToLocal((new StringBuilder()).Append("potion.potency.").Append(potioneffect.GetAmplifier()).ToString()).Trim()).ToString();
					}

					if (potioneffect.GetDuration() > 20)
					{
						s1 = (new StringBuilder()).Append(s1).Append(" (").Append(net.minecraft.src.Potion.GetDurationString(potioneffect)).Append(")").ToString();
					}

					if (net.minecraft.src.Potion.PotionTypes[potioneffect.GetPotionID()].IsBadEffect())
					{
						par2List.Add((new StringBuilder()).Append((char)0xa7 + "c").Append(s1).ToString());
					}
					else
					{
						par2List.Add((new StringBuilder()).Append((char)0xa7 + "7").Append(s1).ToString());
					}
				}
			}
			else
			{
				string s = StatCollector.TranslateToLocal("potion.empty").Trim();
				par2List.Add((new StringBuilder()).Append((char)0xa7).Append(s).ToString());
			}
		}

		public override bool HasEffect(ItemStack par1ItemStack)
		{
			List<PotionEffect> list = GetEffects(par1ItemStack);
			return list != null && list.Count > 0;
		}
	}
}