using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace net.minecraft.src
{
	public class AchievementMap
	{
		/// <summary>
		/// Holds the singleton instance of AchievementMap. </summary>
		public static AchievementMap Instance = new AchievementMap();

		/// <summary>
		/// Maps a achievement id with it's unique GUID. </summary>
		private Dictionary<int, string> GuidMap;

		private AchievementMap()
		{
            GuidMap = new Dictionary<int, string>();

			try
			{
				StreamReader bufferedreader = new StreamReader(Minecraft.GetResourceStream("achievement.map.txt"));
				string s;

				while ((s = bufferedreader.ReadLine()) != null)
				{
					string[] @as = StringHelperClass.StringSplit(s, ",", true);
					int i = Convert.ToInt32(@as[0]);
					GuidMap[i] = @as[1];
				}

				bufferedreader.Close();
			}
			catch (Exception exception)
			{
                Utilities.LogException(exception);
			}
		}

		/// <summary>
		/// Returns the unique GUID of a achievement id.
		/// </summary>
		public static string GetGuid(int par0)
		{
            if (Instance.GuidMap.ContainsKey(par0))
                return Instance.GuidMap[par0];

            return null;
		}
	}
}