namespace net.minecraft.src
{

	public class ChatLine
	{
		/// <summary>
		/// The chat message. </summary>
		public string Message;

		/// <summary>
		/// Counts the number of screen updates. </summary>
		public int UpdateCounter;

		public ChatLine(string par1Str)
		{
			Message = par1Str;
			UpdateCounter = 0;
		}
	}

}