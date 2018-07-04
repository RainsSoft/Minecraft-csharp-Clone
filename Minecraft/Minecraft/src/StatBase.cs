using System;
using System.Globalization;
using System.Text;

namespace net.minecraft.src
{
	public class StatBase
	{
		/// <summary>
		/// The Stat ID </summary>
		public readonly int StatId;

		/// <summary>
		/// The Stat name </summary>
		private readonly string StatName;
		public bool IsIndependent;

		/// <summary>
		/// Holds the GUID of the stat. </summary>
		public string StatGuid;
		private readonly IStatType Type;
		private static NumberFormatInfo NumberFormat;
		public static IStatType SimpleStatType = new StatTypeSimple();
		public static IStatType TimeStatType = new StatTypeTime();
		public static IStatType DistanceStatType = new StatTypeDistance();

		public StatBase(int par1, string par2Str, IStatType par3IStatType)
		{
			IsIndependent = false;
			StatId = par1;
			StatName = par2Str;
			Type = par3IStatType;
		}

		public StatBase(int par1, string par2Str) : this(par1, par2Str, SimpleStatType)
		{
		}

		/// <summary>
		/// Initializes the current stat as independent (i.e., lacking prerequisites for being updated) and returns the
		/// current instance.
		/// </summary>
		public virtual StatBase InitIndependentStat()
		{
			IsIndependent = true;
			return this;
		}

		/// <summary>
		/// Register the stat into StatList.
		/// </summary>
		public virtual StatBase RegisterStat()
		{
			if (StatList.OneShotStats.ContainsKey(StatId))
			{
				throw new Exception((new StringBuilder()).Append("Duplicate stat id: \"").Append(StatList.OneShotStats[StatId].StatName).Append("\" and \"").Append(StatName).Append("\" at id ").Append(StatId).ToString());
			}
			else
			{
				StatList.AllStats.Add(this);
				StatList.OneShotStats[StatId] = this;
				StatGuid = AchievementMap.GetGuid(StatId);
				return this;
			}
		}

		/// <summary>
		/// Returns whether or not the StatBase-derived class is a statistic (running counter) or an achievement (one-shot).
		/// </summary>
		public virtual bool IsAchievement()
		{
			return false;
		}

		public virtual string Func_27084_a(int par1)
		{
			return Type.Format(par1);
		}

		public virtual string GetName()
		{
			return StatName;
		}

		public override string ToString()
		{
			return StatCollector.TranslateToLocal(StatName);
		}

		public static NumberFormatInfo GetNumberFormat()
		{
			return NumberFormat;
		}

		static StatBase()
		{
            NumberFormat = new NumberFormatInfo();
            NumberFormat.NumberDecimalDigits = 2;
            //.getIntegerInstance(Locale.US);
		}
	}
}