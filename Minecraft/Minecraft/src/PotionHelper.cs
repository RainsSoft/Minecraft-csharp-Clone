using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class PotionHelper
	{
		public const string Field_40367_a = null;
		public const string SugarEffect = "-0+1-2-3&4-4+13";
		public const string GhastTearEffect = "+0-1-2-3&4-4+13";
		public const string SpiderEyeEffect = "-0-1+2-3&4-4+13";
		public const string FermentedSpiderEyeEffect = "-0+3-4+13";
		public const string SpeckledMelonEffect = "+0-1+2-3&4-4+13";
		public const string BlazePowderEffect = "+0-1-2+3&4-4+13";
		public const string MagmaCreamEffect = "+0+1-2-3&4-4+13";
		public const string RedstoneEffect = "-5+6-7";
		public const string GlowstoneEffect = "+5-6-7";
		public const string GunpowderEffect = "+14&13-13";
        private static readonly Dictionary<int, string> PotionRequirements;
        private static readonly Dictionary<int, string> Field_40371_m;
        private static readonly Dictionary<int, int> Field_40368_n = new Dictionary<int, int>();
		private static readonly string[] PotionPrefixes = { "potion.prefix.mundane", "potion.prefix.uninteresting", "potion.prefix.bland", "potion.prefix.clear", "potion.prefix.milky", "potion.prefix.diffuse", "potion.prefix.artless", "potion.prefix.thin", "potion.prefix.awkward", "potion.prefix.flat", "potion.prefix.bulky", "potion.prefix.bungling", "potion.prefix.buttered", "potion.prefix.smooth", "potion.prefix.suave", "potion.prefix.debonair", "potion.prefix.thick", "potion.prefix.elegant", "potion.prefix.fancy", "potion.prefix.charming", "potion.prefix.dashing", "potion.prefix.refined", "potion.prefix.cordial", "potion.prefix.sparkling", "potion.prefix.potent", "potion.prefix.foul", "potion.prefix.odorless", "potion.prefix.rank", "potion.prefix.harsh", "potion.prefix.acrid", "potion.prefix.gross", "potion.prefix.stinky" };

		public PotionHelper()
		{
		}

		/// <summary>
		/// Is the bit given set to 1?
		/// </summary>
		public static bool CheckFlag(int par0, int par1)
		{
			return (par0 & 1 << par1) != 0;
		}

		/// <summary>
		/// Returns 1 if the flag is set, 0 if it is not set.
		/// </summary>
		private static int IsFlagSet(int par0, int par1)
		{
			return CheckFlag(par0, par1) ? 1 : 0;
		}

		/// <summary>
		/// Returns 0 if the flag is set, 1 if it is not set.
		/// </summary>
		private static int IsFlagUnset(int par0, int par1)
		{
			return CheckFlag(par0, par1) ? 0 : 1;
		}

		public static int Func_40352_a(int par0)
		{
			return Func_40351_a(par0, 5, 4, 3, 2, 1);
		}

		public static int Func_40354_a(ICollection<PotionEffect> par0Collection)
		{
			int i = 0x385dc6;

			if (par0Collection == null || par0Collection.Count == 0)
			{
				return i;
			}

			float f = 0.0F;
			float f1 = 0.0F;
			float f2 = 0.0F;
			float f3 = 0.0F;

			for (IEnumerator<PotionEffect> iterator = par0Collection.GetEnumerator(); iterator.MoveNext();)
			{
				PotionEffect potioneffect = iterator.Current;
				int j = Potion.PotionTypes[potioneffect.GetPotionID()].GetLiquidColor();
				int k = 0;

				while (k <= potioneffect.GetAmplifier())
				{
					f += (float)(j >> 16 & 0xff) / 255F;
					f1 += (float)(j >> 8 & 0xff) / 255F;
					f2 += (float)(j >> 0 & 0xff) / 255F;
					f3++;
					k++;
				}
			}

			f = (f / f3) * 255F;
			f1 = (f1 / f3) * 255F;
			f2 = (f2 / f3) * 255F;
			return (int)f << 16 | (int)f1 << 8 | (int)f2;
		}

		public static int Func_40358_a(int par0, bool par1)
		{
			if (!par1)
			{
				if (Field_40368_n.ContainsKey(par0))
				{
					return (int)Field_40368_n[par0];
				}
				else
				{
					int i = Func_40354_a(GetPotionEffects(par0, false));
					Field_40368_n[par0] = i;
					return i;
				}
			}
			else
			{
				return Func_40354_a(GetPotionEffects(par0, par1));
			}
		}

		public static string Func_40359_b(int par0)
		{
			int i = Func_40352_a(par0);
			return PotionPrefixes[i];
		}

		private static int Func_40347_a(bool par0, bool par1, bool par2, int par3, int par4, int par5, int par6)
		{
			int i = 0;

			if (par0)
			{
				i = IsFlagUnset(par6, par4);
			}
			else if (par3 != -1)
			{
				if (par3 == 0 && CountSetFlags(par6) == par4)
				{
					i = 1;
				}
				else if (par3 == 1 && CountSetFlags(par6) > par4)
				{
					i = 1;
				}
				else if (par3 == 2 && CountSetFlags(par6) < par4)
				{
					i = 1;
				}
			}
			else
			{
				i = IsFlagSet(par6, par4);
			}

			if (par1)
			{
				i *= par5;
			}

			if (par2)
			{
				i *= -1;
			}

			return i;
		}

		/// <summary>
		/// Count the number of bits in an integer set to ON.
		/// </summary>
		private static int CountSetFlags(int par0)
		{
			int i;

			for (i = 0; par0 > 0; i++)
			{
				par0 &= par0 - 1;
			}

			return i;
		}

		private static int Func_40355_a(string par0Str, int par1, int par2, int par3)
		{
			if (par1 >= par0Str.Length || par2 < 0 || par1 >= par2)
			{
				return 0;
			}

			int i = par0Str.IndexOf('|', par1);

			if (i >= 0 && i < par2)
			{
				int j = Func_40355_a(par0Str, par1, i - 1, par3);

				if (j > 0)
				{
					return j;
				}

				int l = Func_40355_a(par0Str, i + 1, par2, par3);

				if (l > 0)
				{
					return l;
				}
				else
				{
					return 0;
				}
			}

			int k = par0Str.IndexOf('&', par1);

			if (k >= 0 && k < par2)
			{
				int i1 = Func_40355_a(par0Str, par1, k - 1, par3);

				if (i1 <= 0)
				{
					return 0;
				}

				int j1 = Func_40355_a(par0Str, k + 1, par2, par3);

				if (j1 <= 0)
				{
					return 0;
				}

				if (i1 > j1)
				{
					return i1;
				}
				else
				{
					return j1;
				}
			}

			bool flag = false;
			bool flag1 = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			sbyte byte0 = -1;
			int k1 = 0;
			int l1 = 0;
			int i2 = 0;

			for (int j2 = par1; j2 < par2; j2++)
			{
				char c = par0Str[j2];

				if (c >= '0' && c <= '9')
				{
					if (flag)
					{
						l1 = c - 48;
						flag1 = true;
					}
					else
					{
						k1 *= 10;
						k1 += c - 48;
						flag2 = true;
					}

					continue;
				}

				if (c == '*')
				{
					flag = true;
					continue;
				}

				if (c == '!')
				{
					if (flag2)
					{
						i2 += Func_40347_a(flag3, flag1, flag4, byte0, k1, l1, par3);
						flag2 = flag1 = flag = flag4 = flag3 = false;
						k1 = l1 = 0;
						byte0 = -1;
					}

					flag3 = true;
					continue;
				}

				if (c == '-')
				{
					if (flag2)
					{
						i2 += Func_40347_a(flag3, flag1, flag4, byte0, k1, l1, par3);
						flag2 = flag1 = flag = flag4 = flag3 = false;
						k1 = l1 = 0;
						byte0 = -1;
					}

					flag4 = true;
					continue;
				}

				if (c == '=' || c == '<' || c == '>')
				{
					if (flag2)
					{
						i2 += Func_40347_a(flag3, flag1, flag4, byte0, k1, l1, par3);
						flag2 = flag1 = flag = flag4 = flag3 = false;
						k1 = l1 = 0;
						byte0 = -1;
					}

					if (c == '=')
					{
						byte0 = 0;
						continue;
					}

					if (c == '<')
					{
						byte0 = 2;
						continue;
					}

					if (c == '>')
					{
						byte0 = 1;
					}

					continue;
				}

				if (c == '+' && flag2)
				{
					i2 += Func_40347_a(flag3, flag1, flag4, byte0, k1, l1, par3);
					flag2 = flag1 = flag = flag4 = flag3 = false;
					k1 = l1 = 0;
					byte0 = -1;
				}
			}

			if (flag2)
			{
				i2 += Func_40347_a(flag3, flag1, flag4, byte0, k1, l1, par3);
			}

			return i2;
		}

		/// <summary>
		/// Returns a list of effects for the specified potion damage value.
		/// </summary>
		public static List<PotionEffect> GetPotionEffects(int par0, bool par1)
		{
			List<PotionEffect> arraylist = null;
			Potion[] apotion = Potion.PotionTypes;
			int i = apotion.Length;

			for (int j = 0; j < i; j++)
			{
				Potion potion = apotion[j];

				if (potion == null || potion.IsUsable() && !par1)
				{
					continue;
				}

				string s = (string)PotionRequirements[potion.GetId()];

				if (s == null)
				{
					continue;
				}

				int k = Func_40355_a(s, 0, s.Length, par0);

				if (k <= 0)
				{
					continue;
				}

				int l = 0;
				string s1 = (string)Field_40371_m[potion.GetId()];

				if (s1 != null)
				{
					l = Func_40355_a(s1, 0, s1.Length, par0);

					if (l < 0)
					{
						l = 0;
					}
				}

				if (potion.IsInstant())
				{
					k = 1;
				}
				else
				{
					k = 1200 * (k * 3 + (k - 1) * 2);
					k >>= l;
					k = (int)Math.Round((double)k * potion.GetEffectiveness());

					if ((par0 & 0x4000) != 0)
					{
						k = (int)Math.Round((double)k * 0.75D + 0.5D);
					}
				}

				if (arraylist == null)
				{
					arraylist = new List<PotionEffect>();
				}

				arraylist.Add(new PotionEffect(potion.GetId(), k, l));
			}

			return arraylist;
		}

		/// <summary>
		/// Does bit operations for brewPotionData, given data, the index of the bit being operated upon, whether the bit
		/// will be removed, whether the bit will be toggled (NOT), or whether the data field will be set to 0 if the bit is
		/// not present.
		/// </summary>
		private static int BrewBitOperations(int par0, int par1, bool par2, bool par3, bool par4)
		{
			if (par4)
			{
				if (!CheckFlag(par0, par1))
				{
					return 0;
				}
			}
			else if (par2)
			{
				par0 &= ~(1 << par1);
			}
			else if (par3)
			{
				if ((par0 & 1 << par1) != 0)
				{
					par0 &= ~(1 << par1);
				}
				else
				{
					par0 |= 1 << par1;
				}
			}
			else
			{
				par0 |= 1 << par1;
			}

			return par0;
		}

		/// <summary>
		/// Generate a data value for a potion, given its previous data value and the encoded string of new effects it will
		/// receive
		/// </summary>
		public static int ApplyIngredient(int par0, string par1Str)
		{
			bool flag = false;
			int i = par1Str.Length;
			bool flag1 = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			int j = 0;

			for (int k = ((flag) ? 1 : 0); k < i; k++)
			{
				char c = par1Str[k];

				if (c >= '0' && c <= '9')
				{
					j *= 10;
					j += c - 48;
					flag1 = true;
					continue;
				}

				if (c == '!')
				{
					if (flag1)
					{
						par0 = BrewBitOperations(par0, j, flag3, flag2, flag4);
						flag1 = flag3 = flag2 = flag4 = false;
						j = 0;
					}

					flag2 = true;
					continue;
				}

				if (c == '-')
				{
					if (flag1)
					{
						par0 = BrewBitOperations(par0, j, flag3, flag2, flag4);
						flag1 = flag3 = flag2 = flag4 = false;
						j = 0;
					}

					flag3 = true;
					continue;
				}

				if (c == '+')
				{
					if (flag1)
					{
						par0 = BrewBitOperations(par0, j, flag3, flag2, flag4);
						flag1 = flag3 = flag2 = flag4 = false;
						j = 0;
					}

					continue;
				}

				if (c != '&')
				{
					continue;
				}

				if (flag1)
				{
					par0 = BrewBitOperations(par0, j, flag3, flag2, flag4);
					flag1 = flag3 = flag2 = flag4 = false;
					j = 0;
				}

				flag4 = true;
			}

			if (flag1)
			{
				par0 = BrewBitOperations(par0, j, flag3, flag2, flag4);
			}

			return par0 & 0x7fff;
		}

		public static int Func_40351_a(int par0, int par1, int par2, int par3, int par4, int par5)
		{
			return (CheckFlag(par0, par1) ? 0x10 : 0) | (CheckFlag(par0, par2) ? 8 : 0) | (CheckFlag(par0, par3) ? 4 : 0) | (CheckFlag(par0, par4) ? 2 : 0) | (CheckFlag(par0, par5) ? 1 : 0);
		}

		static PotionHelper()
		{
			PotionRequirements = new Dictionary<int, string>();
            Field_40371_m = new Dictionary<int, string>();
			PotionRequirements[Potion.Regeneration.GetId()] = "0 & !1 & !2 & !3 & 0+6";
			PotionRequirements[Potion.MoveSpeed.GetId()] = "!0 & 1 & !2 & !3 & 1+6";
			PotionRequirements[Potion.FireResistance.GetId()] = "0 & 1 & !2 & !3 & 0+6";
			PotionRequirements[Potion.Heal.GetId()] = "0 & !1 & 2 & !3";
			PotionRequirements[Potion.Poison.GetId()] = "!0 & !1 & 2 & !3 & 2+6";
			PotionRequirements[Potion.Weakness.GetId()] = "!0 & !1 & !2 & 3 & 3+6";
			PotionRequirements[Potion.Harm.GetId()] = "!0 & !1 & 2 & 3";
			PotionRequirements[Potion.MoveSlowdown.GetId()] = "!0 & 1 & !2 & 3 & 3+6";
			PotionRequirements[Potion.DamageBoost.GetId()] = "0 & !1 & !2 & 3 & 3+6";
			Field_40371_m[Potion.MoveSpeed.GetId()] = "5";
			Field_40371_m[Potion.DigSpeed.GetId()] = "5";
			Field_40371_m[Potion.DamageBoost.GetId()] = "5";
			Field_40371_m[Potion.Regeneration.GetId()] = "5";
			Field_40371_m[Potion.Harm.GetId()] = "5";
			Field_40371_m[Potion.Heal.GetId()] = "5";
			Field_40371_m[Potion.Resistance.GetId()] = "5";
			Field_40371_m[Potion.Poison.GetId()] = "5";
		}
	}
}