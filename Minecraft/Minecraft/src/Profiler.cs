using System;
using System.Collections.Generic;
using System.Text;

namespace net.minecraft.src
{
	public class Profiler
	{
		/// <summary>
		/// Flag profiling enabled </summary>
		public static bool ProfilingEnabled = false;

		/// <summary>
		/// List of parent sections </summary>
        private static List<string> SectionList = new List<string>();

		/// <summary>
		/// List of timestamps (JavaHelper.NanoTime) </summary>
        private static List<long> TimestampList = new List<long>();

		/// <summary>
		/// Current profiling section </summary>
		private static string ProfilingSection = "";

		/// <summary>
		/// Profiling map </summary>
        private static Dictionary<string, long> ProfilingMap = new Dictionary<string, long>();

		public Profiler()
		{
		}

		/// <summary>
		/// Clear profiling
		/// </summary>
		public static void ClearProfiling()
		{
			ProfilingMap.Clear();
		}

		/// <summary>
		/// Start section
		/// </summary>
		public static void StartSection(string par0Str)
		{
			if (!ProfilingEnabled)
			{
				return;
			}

			if (ProfilingSection.Length > 0)
			{
				ProfilingSection = (new StringBuilder()).Append(ProfilingSection).Append(".").ToString();
			}

			ProfilingSection = (new StringBuilder()).Append(ProfilingSection).Append(par0Str).ToString();
			SectionList.Add(ProfilingSection);
			TimestampList.Add(Convert.ToInt64(JavaHelper.NanoTime()));
		}

		/// <summary>
		/// End section
		/// </summary>
		public static void EndSection()
		{
			if (!ProfilingEnabled)
			{
				return;
			}

			long l = JavaHelper.NanoTime();
            long l1 = TimestampList[TimestampList.Count - 1];
            TimestampList.RemoveAt(TimestampList.Count - 1);
			SectionList.RemoveAt(SectionList.Count - 1);
			long l2 = l - l1;

			if (ProfilingMap.ContainsKey(ProfilingSection))
			{
				ProfilingMap[ProfilingSection] = (long)((long?)ProfilingMap[ProfilingSection]) + l2;
			}
			else
			{
				ProfilingMap[ProfilingSection] = l2;
			}

			ProfilingSection = SectionList.Count <= 0 ? "" : (string)SectionList[SectionList.Count - 1];

			if (l2 > 0x5f5e100L)
			{
				Console.WriteLine((new StringBuilder()).Append(ProfilingSection).Append(" ").Append(l2).ToString());
			}
		}

		/// <summary>
		/// Get profiling data
		/// </summary>
		public static List<ProfilerResult> GetProfilingData(string par0Str)
		{
			if (!ProfilingEnabled)
			{
				return null;
			}

			string s = par0Str;
			long l = ProfilingMap.ContainsKey("root") ? (long)((long?)ProfilingMap["root"]) : 0L;
			long l1 = ProfilingMap.ContainsKey(par0Str) ? (long)((long?)ProfilingMap[par0Str]) : -1L;
			List<ProfilerResult> arraylist = new List<ProfilerResult>();

			if (par0Str.Length > 0)
			{
				par0Str = (new StringBuilder()).Append(par0Str).Append(".").ToString();
			}

			long l2 = 0L;

            foreach (string s1 in ProfilingMap.Keys)
            {
				if (s1.Length > par0Str.Length && s1.StartsWith(par0Str) && s1.IndexOf(".", par0Str.Length + 1) < 0)
				{
					l2 += (long)ProfilingMap[s1];
				}
            }

			float f = l2;

			if (l2 < l1)
			{
				l2 = l1;
			}

			if (l < l2)
			{
				l = l2;
			}

            foreach (string s2 in ProfilingMap.Keys)
            {
				if (s2.Length > par0Str.Length && s2.StartsWith(par0Str) && s2.IndexOf(".", par0Str.Length + 1) < 0)
				{
					long l3 = (long)((long?)ProfilingMap[s2]);
					double d = ((double)l3 * 100D) / (double)l2;
					double d1 = ((double)l3 * 100D) / (double)l;
					string s4 = s2.Substring(par0Str.Length);
					arraylist.Add(new ProfilerResult(s4, d, d1));
				}
            }

            foreach (string s3 in ProfilingMap.Keys)
            {
                ProfilingMap[s3] = (ProfilingMap[s3] * 999L) / 1000L;
            }

			if ((float)l2 > f)
			{
				arraylist.Add(new ProfilerResult("unspecified", ((double)((float)l2 - f) * 100D) / (double)l2, ((double)((float)l2 - f) * 100D) / (double)l));
			}

            arraylist.Sort();
			arraylist.Insert(0, new ProfilerResult(s, 100D, ((double)l2 * 100D) / (double)l));
			return arraylist;
		}

		/// <summary>
		/// End current section and start a new section
		/// </summary>
		public static void EndStartSection(string par0Str)
		{
			EndSection();
			StartSection(par0Str);
		}
	}
}