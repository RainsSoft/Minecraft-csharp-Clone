using System.Collections.Generic;

namespace net.minecraft.src
{
	class GuiSlotStatsBlock : GuiSlotStats
	{
		public readonly GuiStats Field_27274_a;

		public GuiSlotStatsBlock(GuiStats par1GuiStats) : base(par1GuiStats)
		{
			Field_27274_a = par1GuiStats;
			Field_27273_c = new List<StatCrafting>();
			IEnumerator<StatBase> iterator = StatList.ObjectMineStats.GetEnumerator();

			do
			{
				if (!iterator.MoveNext())
				{
					break;
				}

				StatCrafting statcrafting = (StatCrafting)iterator.Current;
				bool flag = false;
				int i = statcrafting.GetItemID();

				if (GuiStats.GetStatsFileWriter(par1GuiStats).WriteStat(statcrafting) > 0)
				{
					flag = true;
				}
				else if (StatList.ObjectUseStats[i] != null && GuiStats.GetStatsFileWriter(par1GuiStats).WriteStat(StatList.ObjectUseStats[i]) > 0)
				{
					flag = true;
				}
				else if (StatList.ObjectCraftStats[i] != null && GuiStats.GetStatsFileWriter(par1GuiStats).WriteStat(StatList.ObjectCraftStats[i]) > 0)
				{
					flag = true;
				}

				if (flag)
				{
					Field_27273_c.Add(statcrafting);
				}
			}
			while (true);

			Field_27272_d = new SorterStatsBlock(this, par1GuiStats);
		}

		protected override void Func_27260_a(int par1, int par2, Tessellator par3Tessellator)
		{
			base.Func_27260_a(par1, par2, par3Tessellator);

			if (Field_27268_b == 0)
			{
				GuiStats.DrawSprite(Field_27274_a, ((par1 + 115) - 18) + 1, par2 + 1 + 1, 18, 18);
			}
			else
			{
				GuiStats.DrawSprite(Field_27274_a, (par1 + 115) - 18, par2 + 1, 18, 18);
			}

			if (Field_27268_b == 1)
			{
				GuiStats.DrawSprite(Field_27274_a, ((par1 + 165) - 18) + 1, par2 + 1 + 1, 36, 18);
			}
			else
			{
				GuiStats.DrawSprite(Field_27274_a, (par1 + 165) - 18, par2 + 1, 36, 18);
			}

			if (Field_27268_b == 2)
			{
				GuiStats.DrawSprite(Field_27274_a, ((par1 + 215) - 18) + 1, par2 + 1 + 1, 54, 18);
			}
			else
			{
				GuiStats.DrawSprite(Field_27274_a, (par1 + 215) - 18, par2 + 1, 54, 18);
			}
		}

		protected override void DrawSlot(int par1, int par2, int par3, int par4, Tessellator par5Tessellator)
		{
			StatCrafting statcrafting = Func_27264_b(par1);
			int i = statcrafting.GetItemID();
			GuiStats.DrawItemSprite(Field_27274_a, par2 + 40, par3, i);
			Func_27265_a((StatCrafting)StatList.ObjectCraftStats[i], par2 + 115, par3, par1 % 2 == 0);
			Func_27265_a((StatCrafting)StatList.ObjectUseStats[i], par2 + 165, par3, par1 % 2 == 0);
			Func_27265_a(statcrafting, par2 + 215, par3, par1 % 2 == 0);
		}

		protected override string Func_27263_a(int par1)
		{
			if (par1 == 0)
			{
				return "stat.crafted";
			}

			if (par1 == 1)
			{
				return "stat.used";
			}
			else
			{
				return "stat.mined";
			}
		}
	}
}