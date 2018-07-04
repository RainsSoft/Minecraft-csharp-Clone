using System;
using System.Text;

namespace net.minecraft.src
{

	public class EnchantmentProtection : Enchantment
	{
		private static readonly string[] ProtectionName = { "all", "fire", "fall", "explosion", "projectile" };
		private static readonly int[] BaseEnchantability = { 1, 10, 5, 5, 3 };
		private static readonly int[] LevelEnchantability = { 16, 8, 6, 8, 6 };
		private static readonly int[] ThresholdEnchantability = { 20, 12, 10, 12, 15 };

		/// <summary>
		/// Defines the type of protection of the enchantment, 0 = all, 1 = fire, 2 = fall (feather fall), 3 = explosion and
		/// 4 = projectile.
		/// </summary>
		public readonly int ProtectionType;

		public EnchantmentProtection(int par1, int par2, int par3) : base(par1, par2, EnumEnchantmentType.armor)
		{
			ProtectionType = par3;

			if (par3 == 2)
			{
				Type = EnumEnchantmentType.armor_feet;
			}
		}

		/// <summary>
		/// Returns the minimal value of enchantability nedded on the enchantment level passed.
		/// </summary>
		public override int GetMinEnchantability(int par1)
		{
			return BaseEnchantability[ProtectionType] + (par1 - 1) * LevelEnchantability[ProtectionType];
		}

		/// <summary>
		/// Returns the maximum value of enchantability nedded on the enchantment level passed.
		/// </summary>
		public override int GetMaxEnchantability(int par1)
		{
			return GetMinEnchantability(par1) + ThresholdEnchantability[ProtectionType];
		}

		/// <summary>
		/// Returns the maximum level that the enchantment can have.
		/// </summary>
		public override int GetMaxLevel()
		{
			return 4;
		}

		/// <summary>
		/// Calculates de damage protection of the enchantment based on level and damage source passed.
		/// </summary>
		public override int CalcModifierDamage(int par1, DamageSource par2DamageSource)
		{
			if (par2DamageSource.CanHarmInCreative())
			{
				return 0;
			}

			int i = (6 + par1 * par1) / 2;

			if (ProtectionType == 0)
			{
				return i;
			}

			if (ProtectionType == 1 && par2DamageSource.FireDamage())
			{
				return i;
			}

			if (ProtectionType == 2 && par2DamageSource == DamageSource.Fall)
			{
				return i * 2;
			}

			if (ProtectionType == 3 && par2DamageSource == DamageSource.Explosion)
			{
				return i;
			}

			if (ProtectionType == 4 && par2DamageSource.IsProjectile())
			{
				return i;
			}
			else
			{
				return 0;
			}
		}

		/// <summary>
		/// Return the name of key in translation table of this enchantment.
		/// </summary>
		public override string GetName()
		{
			return (new StringBuilder()).Append("enchantment.protect.").Append(ProtectionName[ProtectionType]).ToString();
		}

		/// <summary>
		/// Determines if the enchantment passed can be applyied together with this enchantment.
		/// </summary>
		public override bool CanApplyTogether(Enchantment par1Enchantment)
		{
			if (par1Enchantment is EnchantmentProtection)
			{
				EnchantmentProtection enchantmentprotection = (EnchantmentProtection)par1Enchantment;

				if (enchantmentprotection.ProtectionType == ProtectionType)
				{
					return false;
				}

				return ProtectionType == 2 || enchantmentprotection.ProtectionType == 2;
			}
			else
			{
				return base.CanApplyTogether(par1Enchantment);
			}
		}
	}

}