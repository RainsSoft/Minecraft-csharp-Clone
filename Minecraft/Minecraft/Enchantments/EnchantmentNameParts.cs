using System;
using System.Text;

namespace net.minecraft.src
{
	public class EnchantmentNameParts
	{
		/// <summary>
		/// The static instance of this class. </summary>
		public static readonly EnchantmentNameParts Instance = new EnchantmentNameParts();

		/// <summary>
		/// The RNG used to generate enchant names. </summary>
		private Random Rand;
		private string[] WordList;

		private EnchantmentNameParts()
		{
			Rand = new Random();
			WordList = "the elder scrolls klaatu berata niktu xyzzy bless curse light darkness fire air earth water hot dry cold wet ignite snuff embiggen twist shorten stretch fiddle destroy imbue galvanize enchant free limited range of towards inside sphere cube self other ball mental physical grow shrink demon elemental spirit animal creature beast humanoid undead fresh stale ".Split(' ');
		}

		/// <summary>
		/// Generates a random enchant name.
		/// </summary>
		public virtual string GenerateRandomEnchantName()
		{
			int i = Rand.Next(2) + 3;
			string s = "";

			for (int j = 0; j < i; j++)
			{
				if (j > 0)
				{
					s = (new StringBuilder()).Append(s).Append(" ").ToString();
				}

				s = (new StringBuilder()).Append(s).Append(WordList[Rand.Next(WordList.Length)]).ToString();
			}

			return s;
		}

		/// <summary>
		/// Sets the seed for the enchant name RNG.
		/// </summary>
		public virtual void SetRandSeed(long par1)
		{
            Rand.SetSeed((int)par1);
		}
	}
}