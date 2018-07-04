namespace net.minecraft.src
{

	public class GuiPlayerInfo
	{
		/// <summary>
		/// The string value of the object </summary>
		public readonly string Name;

		/// <summary>
		/// Player name in lowercase. </summary>
		private readonly string NameinLowerCase;

		/// <summary>
		/// Player response time to server in milliseconds </summary>
		public int ResponseTime;

		public GuiPlayerInfo(string par1Str)
		{
			Name = par1Str;
			NameinLowerCase = par1Str.ToLower();
		}

		/// <summary>
		/// Returns true if the current player name starts with string specified value.
		/// </summary>
		public virtual bool NameStartsWith(string par1Str)
		{
			return NameinLowerCase.StartsWith(par1Str);
		}
	}

}