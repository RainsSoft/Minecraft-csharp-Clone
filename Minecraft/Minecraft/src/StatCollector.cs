namespace net.minecraft.src
{
	public class StatCollector
	{
		private static StringTranslate LocalizedName = StringTranslate.GetInstance();

		public StatCollector()
		{
		}

		/// <summary>
		/// Translates a Stat name
		/// </summary>
		public static string TranslateToLocal(string par0Str)
		{
			return LocalizedName.TranslateKey(par0Str);
		}

		/// <summary>
		/// Translates a Stat name with format args
		/// </summary>
		public static string TranslateToLocalFormatted(string par0Str, object[] par1ArrayOfObj)
		{
			return LocalizedName.TranslateKeyFormat(par0Str, par1ArrayOfObj);
		}
	}
}