namespace net.minecraft.src
{
	sealed class StatTypeSimple : IStatType
	{
		public StatTypeSimple()
		{
		}

		/// <summary>
		/// Formats a given stat for human consumption.
		/// </summary>
		public string Format(int par1)
		{
			return par1.ToString(StatBase.GetNumberFormat());
		}
	}
}