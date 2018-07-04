using System.Text;

namespace net.minecraft.src
{
	sealed class StatTypeDistance : IStatType
	{
		public StatTypeDistance()
		{
		}

		/// <summary>
		/// Formats a given stat for human consumption.
		/// </summary>
		public string Format(int par1)
		{
			int i = par1;
			double d = (double)i / 100D;
			double d1 = d / 1000D;

			if (d1 > 0.5D)
			{
				return (new StringBuilder()).Append(d1.ToString(StatBase.GetNumberFormat())).Append(" km").ToString();
			}

			if (d > 0.5D)
			{
                return (new StringBuilder()).Append(d.ToString(StatBase.GetNumberFormat())).Append(" m").ToString();
			}
			else
			{
				return (new StringBuilder()).Append(par1).Append(" cm").ToString();
			}
		}
	}
}