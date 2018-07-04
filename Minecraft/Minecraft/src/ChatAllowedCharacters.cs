using System;
using System.IO;
using System.Text;
using System.Reflection;

namespace net.minecraft.src
{
	public class ChatAllowedCharacters
	{
		/// <summary>
		/// This String have the characters allowed in any text drawing of minecraft.
		/// </summary>
		public static readonly string AllowedCharacters = GetAllowedCharacters();
		public static readonly char[] AllowedCharactersArray = { '/', '\n', '\r', '\t', '\0', '\f', '`', '?', '*', '\\', '<', '>', '|', '"', ':' };

		public ChatAllowedCharacters()
		{
		}

		/// <summary>
		/// Load the font.txt resource file, that is on UTF-8 format. This file Contains the characters that minecraft can
		/// render Strings on screen.
		/// </summary>
		private static string GetAllowedCharacters()
		{
			string s = "";

            try
            {
                StreamReader bufferedreader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Minecraft.Resources.font.txt"), Encoding.UTF8);

                do
                {
                    string s2;

                    if ((s2 = bufferedreader.ReadLine()) == null)
                    {
                        break;
                    }

                    if (!s2.StartsWith("#"))
                    {
                        s = (new StringBuilder()).Append(s).Append(s2).ToString();
                    }
                }
                while (true);

                bufferedreader.Close();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                Console.WriteLine();
            }

			return s;
		}

		public static bool IsAllowedCharacter(char par0)
		{
            return par0 != 0x247 && (AllowedCharacters.IndexOf(par0) >= 0 || par0 > ' ');
        }

        public static string Func_52019_a(string par0Str)
        {
            StringBuilder stringbuilder = new StringBuilder();
            char[] ac = par0Str.ToCharArray();
            int i = ac.Length;

            for (int j = 0; j < i; j++)
            {
                char c = ac[j];

                if (IsAllowedCharacter(c))
                {
                    stringbuilder.Append(c);
                }
            }

            return stringbuilder.ToString();
        }
    }
}