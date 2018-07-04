using System;
using System.Text;

namespace net.minecraft.src
{
	public abstract class Enchantment
	{
		public static readonly Enchantment[] EnchantmentsList = new Enchantment[256];

		/// <summary>
		/// Converts environmental damage to armour damage </summary>
		public static readonly Enchantment Protection = new EnchantmentProtection(0, 10, 0);

		/// <summary>
		/// Protection against fire </summary>
		public static readonly Enchantment FireProtection = new EnchantmentProtection(1, 5, 1);

		/// <summary>
		/// Less fall damage </summary>
		public static readonly Enchantment FeatherFalling = new EnchantmentProtection(2, 5, 2);

		/// <summary>
		/// Protection against explosions </summary>
		public static readonly Enchantment BlastProtection = new EnchantmentProtection(3, 2, 3);

		/// <summary>
		/// Protection against projectile entities (e.g. arrows) </summary>
		public static readonly Enchantment ProjectileProtection = new EnchantmentProtection(4, 5, 4);

		/// <summary>
		/// Decreases the rate of air loss underwater; increases time between damage while suffocating
		/// </summary>
		public static readonly Enchantment Respiration = new EnchantmentOxygen(5, 2);

		/// <summary>
		/// Increases underwater mining rate </summary>
		public static readonly Enchantment AquaAffinity = new EnchantmentWaterWorker(6, 2);

		/// <summary>
		/// Extra damage to mobs </summary>
		public static readonly Enchantment Sharpness = new EnchantmentDamage(16, 10, 0);

		/// <summary>
		/// Extra damage to zombies, zombie pigmen and skeletons </summary>
		public static readonly Enchantment Smite = new EnchantmentDamage(17, 5, 1);

		/// <summary>
		/// Extra damage to spiders, cave spiders and silverfish </summary>
		public static readonly Enchantment BaneOfArthropods = new EnchantmentDamage(18, 5, 2);

		/// <summary>
		/// Knocks mob and players backwards upon hit </summary>
		public static readonly Enchantment Knockback = new EnchantmentKnockback(19, 5);

		/// <summary>
		/// Lights mobs on fire </summary>
		public static readonly Enchantment FireAspect = new EnchantmentFireAspect(20, 2);

		/// <summary>
		/// Mobs have a chance to drop more loot </summary>
		public static readonly Enchantment Looting;

		/// <summary>
		/// Faster resource gathering while in use </summary>
		public static readonly Enchantment Efficiency = new EnchantmentDigging(32, 10);

		/// <summary>
		/// Blocks mined will drop themselves, even if it should drop something else (e.g. stone will drop stone, not
		/// cobblestone)
		/// </summary>
		public static readonly Enchantment SilkTouch = new EnchantmentUntouching(33, 1);

		/// <summary>
		/// Sometimes, the tool's durability will not be spent when the tool is used
		/// </summary>
		public static readonly Enchantment Unbreaking = new EnchantmentDurability(34, 5);

		/// <summary>
		/// Can multiply the drop rate of items from blocks </summary>
		public static readonly Enchantment Fortune;

		/// <summary>
		/// Power enchantment for bows, add's extra damage to arrows. </summary>
		public static readonly Enchantment Power = new EnchantmentArrowDamage(48, 10);

		/// <summary>
		/// Knockback enchantments for bows, the arrows will knockback the target when hit.
		/// </summary>
		public static readonly Enchantment Punch = new EnchantmentArrowKnockback(49, 2);

		/// <summary>
		/// Flame enchantment for bows. Arrows fired by the bow will be on fire. Any target hit will also set on fire.
		/// </summary>
		public static readonly Enchantment Flame = new EnchantmentArrowFire(50, 2);

		/// <summary>
		/// Infinity enchantment for bows. The bow will not consume arrows anymore, but will still required at least one
		/// arrow on inventory use the bow.
		/// </summary>
		public static readonly Enchantment Infinity = new EnchantmentArrowInfinite(51, 1);
		public readonly int EffectId;
		private readonly int Weight;

		/// <summary>
		/// The EnumEnchantmentType given to this Enchantment. </summary>
		public EnumEnchantmentType Type;

		/// <summary>
		/// Used in localisation and stats. </summary>
		protected string Name;

		protected Enchantment(int par1, int par2, EnumEnchantmentType par3EnumEnchantmentType)
		{
			EffectId = par1;
			Weight = par2;
			Type = par3EnumEnchantmentType;

			if (EnchantmentsList[par1] != null)
			{
				throw new System.ArgumentException("Duplicate enchantment id!");
			}
			else
			{
				EnchantmentsList[par1] = this;
				return;
			}
		}

		public virtual int GetWeight()
		{
			return Weight;
		}

		/// <summary>
		/// Returns the minimum level that the enchantment can have.
		/// </summary>
		public virtual int GetMinLevel()
		{
			return 1;
		}

		/// <summary>
		/// Returns the maximum level that the enchantment can have.
		/// </summary>
		public virtual int GetMaxLevel()
		{
			return 1;
		}

		/// <summary>
		/// Returns the minimal value of enchantability nedded on the enchantment level passed.
		/// </summary>
		public virtual int GetMinEnchantability(int par1)
		{
			return 1 + par1 * 10;
		}

		/// <summary>
		/// Returns the maximum value of enchantability nedded on the enchantment level passed.
		/// </summary>
		public virtual int GetMaxEnchantability(int par1)
		{
			return GetMinEnchantability(par1) + 5;
		}

		/// <summary>
		/// Calculates de damage protection of the enchantment based on level and damage source passed.
		/// </summary>
		public virtual int CalcModifierDamage(int par1, DamageSource par2DamageSource)
		{
			return 0;
		}

		/// <summary>
		/// Calculates de (magic) damage done by the enchantment on a living entity based on level and entity passed.
		/// </summary>
		public virtual int CalcModifierLiving(int par1, EntityLiving par2EntityLiving)
		{
			return 0;
		}

		/// <summary>
		/// Determines if the enchantment passed can be applyied together with this enchantment.
		/// </summary>
		public virtual bool CanApplyTogether(Enchantment par1Enchantment)
		{
			return this != par1Enchantment;
		}

		/// <summary>
		/// Sets the enchantment name
		/// </summary>
		public virtual Enchantment SetName(string par1Str)
		{
			Name = par1Str;
			return this;
		}

		/// <summary>
		/// Return the name of key in translation table of this enchantment.
		/// </summary>
		public virtual string GetName()
		{
			return (new StringBuilder()).Append("enchantment.").Append(Name).ToString();
		}

		/// <summary>
		/// Returns the correct traslated name of the enchantment and the level in roman numbers.
		/// </summary>
		public virtual string GetTranslatedName(int par1)
		{
			string s = StatCollector.TranslateToLocal(GetName());
			return (new StringBuilder()).Append(s).Append(" ").Append(StatCollector.TranslateToLocal((new StringBuilder()).Append("enchantment.level.").Append(par1).ToString())).ToString();
		}

		static Enchantment()
		{
			Looting = new EnchantmentLootBonus(21, 2, EnumEnchantmentType.weapon);
			Fortune = new EnchantmentLootBonus(35, 2, EnumEnchantmentType.digger);
		}
	}
}