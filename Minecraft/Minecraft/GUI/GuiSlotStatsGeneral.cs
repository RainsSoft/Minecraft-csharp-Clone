namespace net.minecraft.src
{
	class GuiSlotStatsGeneral : GuiSlot
	{
		readonly GuiStats Field_27276_a;

		public GuiSlotStatsGeneral(GuiStats par1GuiStats) : base(GuiStats.GetMinecraft(par1GuiStats), par1GuiStats.Width, par1GuiStats.Height, 32, par1GuiStats.Height - 64, 10)
		{
			Field_27276_a = par1GuiStats;
			Func_27258_a(false);
		}

		/// <summary>
		/// Gets the size of the current slot list.
		/// </summary>
        public override int GetSize()
		{
			return StatList.GeneralStats.Count;
		}

		/// <summary>
		/// the element in the slot that was clicked, bool for wether it was double clicked or not
		/// </summary>
		protected override void ElementClicked(int i, bool flag)
		{
		}

		/// <summary>
		/// returns true if the element passed in is currently selected
		/// </summary>
		protected override bool IsSelected(int par1)
		{
			return false;
		}

		/// <summary>
		/// return the height of the content being scrolled
		/// </summary>
		protected override int GetContentHeight()
		{
			return GetSize() * 10;
		}

		protected override void DrawBackground()
		{
			Field_27276_a.DrawDefaultBackground();
		}

		protected override void DrawSlot(int par1, int par2, int par3, int par4, Tessellator par5Tessellator)
		{
			StatBase statbase = (StatBase)StatList.GeneralStats[par1];
			Field_27276_a.DrawString(GuiStats.GetFontRenderer(Field_27276_a), StatCollector.TranslateToLocal(statbase.GetName()), par2 + 2, par3 + 1, par1 % 2 != 0 ? 0x909090 : 0xffffff);
			string s = statbase.Func_27084_a(GuiStats.GetStatsFileWriter(Field_27276_a).WriteStat(statbase));
			Field_27276_a.DrawString(GuiStats.GetFontRenderer(Field_27276_a), s, (par2 + 2 + 213) - (int)GuiStats.GetFontRenderer(Field_27276_a).GetStringWidth(s), par3 + 1, par1 % 2 != 0 ? 0x909090 : 0xffffff);
		}
	}
}