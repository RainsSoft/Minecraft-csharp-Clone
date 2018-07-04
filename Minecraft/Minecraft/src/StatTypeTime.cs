using System.Text;

namespace net.minecraft.src
{
	sealed class StatTypeTime : IStatType
	{
		public StatTypeTime()
		{
		}

		/// <summary>
		/// Formats a given stat for human consumption.
		/// </summary>
		public string Format(int par1)
		{
			double d = (double)par1 / 20D;
			double d1 = d / 60D;
			double d2 = d1 / 60D;
			double d3 = d2 / 24D;
			double d4 = d3 / 365D;

			if (d4 > 0.5D)
			{
				return (new StringBuilder()).Append(d4.ToString(StatBase.GetNumberFormat())).Append(" y").ToString();
			}

			if (d3 > 0.5D)
			{
				return (new StringBuilder()).Append(d3.ToString(StatBase.GetNumberFormat())).Append(" d").ToString();
			}

			if (d2 > 0.5D)
			{
				return (new StringBuilder()).Append(d2.ToString(StatBase.GetNumberFormat())).Append(" h").ToString();
			}

			if (d1 > 0.5D)
			{
				return (new StringBuilder()).Append(d1.ToString(StatBase.GetNumberFormat())).Append(" m").ToString();
			}
			else
			{
				return (new StringBuilder()).Append(d).Append(" s").ToString();
			}
		}
	}
}