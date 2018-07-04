using System.Text;

namespace net.minecraft.src
{

	public class EnchantmentDamage : Enchantment
	{
		private static readonly string[] ProtectionName = { "all", "undead", "arthropods" };
		private static readonly int[] BaseEnchantability = { 1, 5, 5 };
		private static readonly int[] LevelEnchantability = { 16, 8, 8 };
		private static readonly int[] ThresholdEnchantability = { 20, 20, 20 };

		/// <summary>
		/// Defines the type of damage of the enchantment, 0 = all, 1 = undead, 3 = arthropods
		/// </summary>
		public readonly int DamageType;

		public EnchantmentDamage(int par1, int par2, int par3) : base(par1, par2, EnumEnchantmentType.weapon)
		{
			DamageType = par3;
		}

		/// <summary>
		/// Returns the minimal value of enchantability nedded on the enchantment level passed.
		/// </summary>
		public override int GetMinEnchantability(int par1)
		{
			return BaseEnchantability[DamageType] + (par1 - 1) * LevelEnchantability[DamageType];
		}

		/// <summary>
		/// Returns the maximum value of enchantability nedded on the enchantment level passed.
		/// </summary>
		public override int GetMaxEnchantability(int par1)
		{
			return GetMinEnchantability(par1) + ThresholdEnchantability[DamageType];
		}

		/// <summary>
		/// Returns the maximum level that the enchantment can have.
		/// </summary>
		public override int GetMaxLevel()
		{
			return 5;
		}

		/// <summary>
		/// Calculates de (magic) damage done by the enchantment on a living entity based on level and entity passed.
		/// </summary>
		public override int CalcModifierLiving(int par1, EntityLiving par2EntityLiving)
		{
			if (DamageType == 0)
			{
				return par1 * 3;
			}

			if (DamageType == 1 && par2EntityLiving.GetCreatureAttribute() == EnumCreatureAttribute.UNDEAD)
			{
				return par1 * 4;
			}

			if (DamageType == 2 && par2EntityLiving.GetCreatureAttribute() == EnumCreatureAttribute.ARTHROPOD)
			{
				return par1 * 4;
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
			return (new StringBuilder()).Append("enchantment.damage.").Append(ProtectionName[DamageType]).ToString();
		}

		/// <summary>
		/// Determines if the enchantment passed can be applyied together with this enchantment.
		/// </summary>
		public override bool CanApplyTogether(Enchantment par1Enchantment)
		{
			return !(par1Enchantment is EnchantmentDamage);
		}
	}

}