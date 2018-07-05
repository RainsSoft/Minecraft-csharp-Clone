using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace net.minecraft.src
{
	public class PlayerUsageSnooper
	{
		private Dictionary<string, object> SnoopProperties;
		private readonly string Field_52024_b;

		public PlayerUsageSnooper(string par1Str)
		{
            SnoopProperties = new Dictionary<string, object>();

			//try
			{
				Field_52024_b = new StringBuilder().Append("http://snoop.minecraft.net/").Append(par1Str).ToString();
			}/*
			catch (MalformedURLException malformedurlexception)
			{
				throw new ArgumentException();
			}*/
		}

		public virtual void SetSnoopProperty(string name, object value)
		{
			SnoopProperties[name] = value;
		}

		public virtual void SendSnoopData()
		{
            Action snoopSendDelegate = () =>
            {
                //PostHttp.Func_52018_a(PlayerUsageSnooper.Func_52023_a(this), PlayerUsageSnooper.Func_52020_b(this), true);
            };
            Thread playerusagesnooperthread = new Thread(new ThreadStart(snoopSendDelegate)) { Name = "reporter" };
			playerusagesnooperthread.IsBackground = true;
			playerusagesnooperthread.Start();
		}

		static string Func_52023_a(PlayerUsageSnooper par0PlayerUsageSnooper)
		{
			return par0PlayerUsageSnooper.Field_52024_b;
		}

        static Dictionary<string, object> Func_52020_b(PlayerUsageSnooper par0PlayerUsageSnooper)
		{
			return par0PlayerUsageSnooper.SnoopProperties;
		}
	}
}