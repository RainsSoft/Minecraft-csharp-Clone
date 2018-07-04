using System.Collections.Generic;

namespace net.minecraft.src
{
	class SorterStatsItem : IComparer<StatCrafting>
	{
		readonly GuiStats StatsGUI;
		readonly GuiSlotStatsItem SlotStatsItemGUI;

		public SorterStatsItem(GuiSlotStatsItem par1GuiSlotStatsItem, GuiStats par2GuiStats)
		{
			SlotStatsItemGUI = par1GuiSlotStatsItem;
			StatsGUI = par2GuiStats;
		}

		public virtual int Func_27371_a(StatCrafting par1StatCrafting, StatCrafting par2StatCrafting)
		{
			int i = par1StatCrafting.GetItemID();
			int j = par2StatCrafting.GetItemID();
			StatBase statbase = null;
			StatBase statbase1 = null;

			if (SlotStatsItemGUI.Field_27271_e == 0)
			{
				statbase = StatList.ObjectBreakStats[i];
				statbase1 = StatList.ObjectBreakStats[j];
			}
			else if (SlotStatsItemGUI.Field_27271_e == 1)
			{
				statbase = StatList.ObjectCraftStats[i];
				statbase1 = StatList.ObjectCraftStats[j];
			}
			else if (SlotStatsItemGUI.Field_27271_e == 2)
			{
				statbase = StatList.ObjectUseStats[i];
				statbase1 = StatList.ObjectUseStats[j];
			}

			if (statbase != null || statbase1 != null)
			{
				if (statbase == null)
				{
					return 1;
				}

				if (statbase1 == null)
				{
					return -1;
				}

				int k = GuiStats.GetStatsFileWriter(SlotStatsItemGUI.Field_27275_a).WriteStat(statbase);
				int l = GuiStats.GetStatsFileWriter(SlotStatsItemGUI.Field_27275_a).WriteStat(statbase1);

				if (k != l)
				{
					return (k - l) * SlotStatsItemGUI.Field_27270_f;
				}
			}

			return i - j;
		}

        public virtual int Compare(StatCrafting par1Obj, StatCrafting par2Obj)
		{
			return Func_27371_a(par1Obj, par2Obj);
		}
	}
}