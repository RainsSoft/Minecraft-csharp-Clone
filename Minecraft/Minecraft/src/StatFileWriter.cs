using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace net.minecraft.src
{
	public class StatFileWriter
	{
        private Dictionary<StatBase, int> field_25102_a;
        private Dictionary<StatBase, int> field_25101_b;
		private bool field_27189_c;
		private StatsSyncher statsSyncher;

        public StatFileWriter(Session par1Session, string par2File)
		{
            field_25102_a = new Dictionary<StatBase, int>();
            field_25101_b = new Dictionary<StatBase, int>();
			field_27189_c = false;
            DirectoryInfo directory = new DirectoryInfo(System.IO.Path.Combine(par2File, "stats"));

            if (!directory.Exists)
			{
                directory.Create();
			}

            FileInfo[] afile = directory.GetFiles();
			int i = afile.Length;

			for (int j = 0; j < i; j++)
			{
                FileInfo file1 = afile[j];

				if (!file1.Name.StartsWith("stats_") || !file1.Name.EndsWith(".dat"))
				{
					continue;
				}
                /*
                FileInfo file2 = new FileInfo(file, file1.Name);

				if (!file2.Exists)
				{
					Console.WriteLine((new StringBuilder()).Append("Relocating ").Append(file1.Name).ToString());
					file1.Rename(file2);
				}*/
			}

			statsSyncher = new StatsSyncher(par1Session, this, directory.FullName);
		}

		public virtual void ReadStat(StatBase par1StatBase, int par2)
		{
			WriteStatToMap(field_25101_b, par1StatBase, par2);
			WriteStatToMap(field_25102_a, par1StatBase, par2);
			field_27189_c = true;
		}

        private void WriteStatToMap(Dictionary<StatBase, int> par1Map, StatBase par2StatBase, int par3)
		{
            int integer = 0;

            if (par1Map.ContainsKey(par2StatBase))
			    integer = par1Map[par2StatBase];

			par1Map[par2StatBase] = integer + par3;
		}

        public virtual Dictionary<StatBase, int> Func_27176_a()
		{
			return field_25101_b;
		}

        public virtual void Func_27179_a(Dictionary<StatBase, int> par1Map)
		{
			if (par1Map == null)
			{
				return;
			}

			field_27189_c = true;
			StatBase statbase;

			for (IEnumerator<StatBase> iterator = par1Map.Keys.GetEnumerator(); iterator.MoveNext(); WriteStatToMap(field_25102_a, statbase, par1Map[statbase]))
			{
				statbase = iterator.Current;
				WriteStatToMap(field_25101_b, statbase, par1Map[statbase]);
			}
		}

        public virtual void Func_27180_b(Dictionary<StatBase, int> par1Map)
		{
			if (par1Map == null)
			{
				return;
			}

			StatBase statbase;
			int i;

			for (IEnumerator<StatBase> iterator = par1Map.Keys.GetEnumerator(); iterator.MoveNext(); field_25102_a[statbase] = par1Map[statbase] + i)
			{
				statbase = iterator.Current;
				int integer = field_25101_b[statbase];
				i = integer != null ? integer : 0;
			}
		}

        public virtual void Func_27187_c(Dictionary<StatBase, int> par1Map)
		{
			if (par1Map == null)
			{
				return;
			}

			field_27189_c = true;
			StatBase statbase;

			for (IEnumerator<StatBase> iterator = par1Map.Keys.GetEnumerator(); iterator.MoveNext(); WriteStatToMap(field_25101_b, statbase, par1Map[statbase]))
			{
				statbase = iterator.Current;
			}
		}

        public static Dictionary<StatBase, int> Func_27177_a(string par0Str)
		{
            Dictionary<StatBase, int> hashmap = new Dictionary<StatBase, int>();
            /*
			try
			{
				string s = "local";
				StringBuilder stringbuilder = new StringBuilder();
				JsonRootNode jsonrootnode = (new JdomParser()).parse(par0Str);
				IList list = jsonrootnode.getArrayNode(new object[] { "stats-change" });

				for (IEnumerator iterator = list.GetEnumerator(); iterator.MoveNext();)
				{
					JsonNode jsonnode = (JsonNode)iterator.Current;
					IDictionary map = jsonnode.getFields();
					DictionaryEntry entry = (DictionaryEntry)map.GetEnumerator().MoveNext();
					int i = Convert.ToInt32(((JsonStringNode)entry.Key).getText());
					int j = Convert.ToInt32(((JsonNode)entry.Value).getText());
					StatBase statbase = StatList.GetOneShotStat(i);

					if (statbase == null)
					{
						Console.WriteLine((new StringBuilder()).Append(i).Append(" is not a valid stat").ToString());
					}
					else
					{
						stringbuilder.Append(StatList.GetOneShotStat(i).StatGuid).Append(",");
						stringbuilder.Append(j).Append(",");
						hashmap[statbase] = j;
					}
				}

				MD5String md5string = new MD5String(s);
				string s1 = md5string.GetMD5String(stringbuilder.ToString());

				if (!s1.Equals(jsonrootnode.getStringValue(new object[] { "checksum" })))
				{
					Console.WriteLine("CHECKSUM MISMATCH");
					return null;
				}
			}
			catch (InvalidSyntaxException invalidsyntaxexception)
			{
				Console.WriteLine(invalidsyntaxexception.ToString());
				Console.Write(invalidsyntaxexception.StackTrace);
			}
            */
			return hashmap;
		}

		public static string Func_27185_a(string par0Str, string par1Str, Dictionary<StatBase, int> par2Map)
		{
			StringBuilder stringbuilder = new StringBuilder();
			StringBuilder stringbuilder1 = new StringBuilder();
			bool flag = true;
			stringbuilder.Append("{\r\n");

			if (par0Str != null && par1Str != null)
			{
				stringbuilder.Append("  \"user\":{\r\n");
				stringbuilder.Append("    \"name\":\"").Append(par0Str).Append("\",\r\n");
				stringbuilder.Append("    \"sessionid\":\"").Append(par1Str).Append("\"\r\n");
				stringbuilder.Append("  },\r\n");
			}

			stringbuilder.Append("  \"stats-change\":[");
			StatBase statbase;

			for (IEnumerator<StatBase> iterator = par2Map.Keys.GetEnumerator(); iterator.MoveNext(); stringbuilder1.Append(par2Map[statbase]).Append(","))
			{
				statbase = iterator.Current;

				if (!flag)
				{
					stringbuilder.Append("},");
				}
				else
				{
					flag = false;
				}

				stringbuilder.Append("\r\n    {\"").Append(statbase.StatId).Append("\":").Append(par2Map[statbase]);
				stringbuilder1.Append(statbase.StatGuid).Append(",");
			}

			if (!flag)
			{
				stringbuilder.Append("}");
			}

			MD5String md5string = new MD5String(par1Str);
			stringbuilder.Append("\r\n  ],\r\n");
			stringbuilder.Append("  \"checksum\":\"").Append(md5string.GetMD5String(stringbuilder1.ToString())).Append("\"\r\n");
			stringbuilder.Append("}");
			return stringbuilder.ToString();
		}

		/// <summary>
		/// Returns true if the achievement has been unlocked.
		/// </summary>
		public virtual bool HasAchievementUnlocked(Achievement par1Achievement)
		{
			return field_25102_a.ContainsKey(par1Achievement);
		}

		/// <summary>
		/// Returns true if the parent has been unlocked, or there is no parent
		/// </summary>
		public virtual bool CanUnlockAchievement(Achievement par1Achievement)
		{
			return par1Achievement.ParentAchievement == null || HasAchievementUnlocked(par1Achievement.ParentAchievement);
		}

		public virtual int WriteStat(StatBase par1StatBase)
		{
			int integer = field_25102_a[par1StatBase];
			return integer != null ? integer : 0;
		}

		public virtual void Func_27175_b()
		{
		}

		public virtual void SyncStats()
		{
			statsSyncher.SyncStatsFileWithMap(Func_27176_a());
		}

		public virtual void Func_27178_d()
		{
			if (field_27189_c && statsSyncher.Func_27420_b())
			{
				statsSyncher.BeginSendStats(Func_27176_a());
			}

			statsSyncher.Func_27425_c();
		}
	}
}