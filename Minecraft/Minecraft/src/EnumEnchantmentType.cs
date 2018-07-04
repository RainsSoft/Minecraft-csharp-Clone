namespace net.minecraft.src
{
	public enum EnumEnchantmentType
	{
		all,
		armor,
		armor_feet,
		armor_legs,
		armor_torso,
		armor_head,
		weapon,
		digger,
		bow

		/// <summary>
		/// Return true if the item passed can be enchanted by a enchantment of this type.
		/// </summary>
	}

	public static partial class EnumExtensionMethods
	{
		public static bool CanEnchantItem(this EnumEnchantmentType instance, Item par1Item)
		{
            if (instance == EnumEnchantmentType.all)
			{
				return true;
			}

			if (par1Item is ItemArmor)
			{
                if (instance == EnumEnchantmentType.armor)
				{
					return true;
				}

				ItemArmor itemarmor = (ItemArmor)par1Item;

				if (itemarmor.ArmorType == 0)
				{
                    return instance == EnumEnchantmentType.armor_head;
				}

				if (itemarmor.ArmorType == 2)
				{
                    return instance == EnumEnchantmentType.armor_legs;
				}

				if (itemarmor.ArmorType == 1)
				{
                    return instance == EnumEnchantmentType.armor_torso;
				}

				if (itemarmor.ArmorType == 3)
				{
                    return instance == EnumEnchantmentType.armor_feet;
				}
				else
				{
					return false;
				}
			}

			if (par1Item is ItemSword)
			{
                return instance == EnumEnchantmentType.weapon;
			}

			if (par1Item is ItemTool)
			{
                return instance == EnumEnchantmentType.digger;
			}

			if (par1Item is ItemBow)
			{
                return instance == EnumEnchantmentType.bow;
			}
			else
			{
				return false;
			}
		}
	}
}