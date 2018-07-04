using System.Text;

namespace net.minecraft.src
{
	public class Achievement : StatBase
	{
		/// <summary>
		/// Is the column (related to center of achievement gui, in 24 pixels unit) that the achievement will be displayed.
		/// </summary>
		public readonly int DisplayColumn;

		/// <summary>
		/// Is the row (related to center of achievement gui, in 24 pixels unit) that the achievement will be displayed.
		/// </summary>
		public readonly int DisplayRow;

		/// <summary>
		/// Holds the parent achievement, that must be taken before this achievement is avaiable.
		/// </summary>
		public readonly Achievement ParentAchievement;

		/// <summary>
		/// Holds the description of the achievement, ready to be formatted and/or displayed.
		/// </summary>
		private readonly string AchievementDescription;

		/// <summary>
		/// Holds a string formatter for the achievement, some of then needs extra dynamic info - like the key used to open
		/// the inventory.
		/// </summary>
		public IStatStringFormat StatStringFormatter;

		/// <summary>
		/// Holds the ItemStack that will be used to draw the achievement into the GUI.
		/// </summary>
		public readonly ItemStack TheItemStack;

		/// <summary>
		/// Special achievements have a 'spiked' (on normal texture pack) frame, special achievements are the hardest ones to
		/// achieve.
		/// </summary>
		private bool IsSpecial;

		public Achievement(int par1, string par2Str, int par3, int par4, Item par5Item, Achievement par6Achievement) : this(par1, par2Str, par3, par4, new ItemStack(par5Item), par6Achievement)
		{
		}

		public Achievement(int par1, string par2Str, int par3, int par4, Block par5Block, Achievement par6Achievement) : this(par1, par2Str, par3, par4, new ItemStack(par5Block), par6Achievement)
		{
		}

		public Achievement(int par1, string par2Str, int par3, int par4, ItemStack par5ItemStack, Achievement par6Achievement) : base(0x500000 + par1, (new StringBuilder()).Append("achievement.").Append(par2Str).ToString())
		{
			TheItemStack = par5ItemStack;
			AchievementDescription = (new StringBuilder()).Append("achievement.").Append(par2Str).Append(".desc").ToString();
			DisplayColumn = par3;
			DisplayRow = par4;

			if (par3 < AchievementList.MinDisplayColumn)
			{
				AchievementList.MinDisplayColumn = par3;
			}

			if (par4 < AchievementList.MinDisplayRow)
			{
				AchievementList.MinDisplayRow = par4;
			}

			if (par3 > AchievementList.MaxDisplayColumn)
			{
				AchievementList.MaxDisplayColumn = par3;
			}

			if (par4 > AchievementList.MaxDisplayRow)
			{
				AchievementList.MaxDisplayRow = par4;
			}

			ParentAchievement = par6Achievement;
		}

		/// <summary>
		/// Indicates whether or not the given achievement or statistic is independent (i.e., lacks prerequisites for being
		/// update).
		/// </summary>
		public virtual Achievement SetIndependent()
		{
			IsIndependent = true;
			return this;
		}

		/// <summary>
		/// Special achievements have a 'spiked' (on normal texture pack) frame, special achievements are the hardest ones to
		/// achieve.
		/// </summary>
		public virtual Achievement SetSpecial()
		{
			IsSpecial = true;
			return this;
		}

		/// <summary>
		/// Adds the achievement on the list of registered achievements, also, it's check for duplicated id's.
		/// </summary>
		public virtual Achievement RegisterAchievement()
		{
			base.RegisterStat();
			AchievementList.Achievements.Add(this);
			return this;
		}

		/// <summary>
		/// Returns whether or not the StatBase-derived class is a statistic (running counter) or an achievement (one-shot).
		/// </summary>
		public override bool IsAchievement()
		{
			return true;
		}

		/// <summary>
		/// Returns the fully description of the achievement - ready to be displayed on screen.
		/// </summary>
		public virtual string GetDescription()
		{
			if (StatStringFormatter != null)
			{
				return StatStringFormatter.FormatString(StatCollector.TranslateToLocal(AchievementDescription));
			}
			else
			{
				return StatCollector.TranslateToLocal(AchievementDescription);
			}
		}

		/// <summary>
		/// Defines a string formatter for the achievement.
		/// </summary>
		public virtual Achievement SetStatStringFormatter(IStatStringFormat par1IStatStringFormat)
		{
			StatStringFormatter = par1IStatStringFormat;
			return this;
		}

		/// <summary>
		/// Special achievements have a 'spiked' (on normal texture pack) frame, special achievements are the hardest ones to
		/// achieve.
		/// </summary>
		public virtual bool GetSpecial()
		{
			return IsSpecial;
		}

		/// <summary>
		/// Register the stat into StatList.
		/// </summary>
		public override StatBase RegisterStat()
		{
			return RegisterAchievement();
		}

		/// <summary>
		/// Initializes the current stat as independent (i.e., lacking prerequisites for being updated) and returns the
		/// current instance.
		/// </summary>
		public override StatBase InitIndependentStat()
		{
			return SetIndependent();
		}
	}
}