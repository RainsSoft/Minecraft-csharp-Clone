using System.Collections.Generic;

namespace net.minecraft.src
{
	class SorterStatsBlock : IComparer<StatCrafting>
	{
		readonly GuiStats StatsGUI;
		readonly GuiSlotStatsBlock SlotStatsBlockGUI;

		public SorterStatsBlock(GuiSlotStatsBlock par1GuiSlotStatsBlock, GuiStats par2GuiStats)
		{
			SlotStatsBlockGUI = par1GuiSlotStatsBlock;
			StatsGUI = par2GuiStats;
		}

		public virtual int Func_27297_a(StatCrafting par1StatCrafting, StatCrafting par2StatCrafting)
		{
			int i = par1StatCrafting.GetItemID();
			int j = par2StatCrafting.GetItemID();
			StatBase statbase = null;
			StatBase statbase1 = null;

			if (SlotStatsBlockGUI.Field_27271_e == 2)
			{
				statbase = StatList.MineBlockStatArray[i];
				statbase1 = StatList.MineBlockStatArray[j];
			}
			else if (SlotStatsBlockGUI.Field_27271_e == 0)
			{
				statbase = StatList.ObjectCraftStats[i];
				statbase1 = StatList.ObjectCraftStats[j];
			}
			else if (SlotStatsBlockGUI.Field_27271_e == 1)
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

				int k = GuiStats.GetStatsFileWriter(SlotStatsBlockGUI.Field_27274_a).WriteStat(statbase);
				int l = GuiStats.GetStatsFileWriter(SlotStatsBlockGUI.Field_27274_a).WriteStat(statbase1);

				if (k != l)
				{
					return (k - l) * SlotStatsBlockGUI.Field_27270_f;
				}
			}

			return i - j;
		}

        public int Compare(StatCrafting par1Obj, StatCrafting par2Obj)
		{
			return Func_27297_a(par1Obj, par2Obj);
		}
	}
}