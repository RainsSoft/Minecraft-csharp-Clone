using System.Collections.Generic;

namespace net.minecraft.src
{
	using net.minecraft.src;

	class GuiSlotLanguage : GuiSlot
	{
		private List<string> Field_44013_b;
		private SortedDictionary<string, string> Field_44014_c;
		readonly GuiLanguage Field_44015_a;

		public GuiSlotLanguage(GuiLanguage par1GuiLanguage) : base(par1GuiLanguage.Mc, par1GuiLanguage.Width, par1GuiLanguage.Height, 32, (par1GuiLanguage.Height - 65) + 4, 18)
		{
			Field_44015_a = par1GuiLanguage;
			Field_44014_c = StringTranslate.GetInstance().GetLanguageList();
            Field_44013_b = new List<string>();
			string s;

			for (IEnumerator<string> iterator = Field_44014_c.Keys.GetEnumerator(); iterator.MoveNext(); Field_44013_b.Add(s))
			{
				s = iterator.Current;
			}
		}

		/// <summary>
		/// Gets the size of the current slot list.
		/// </summary>
        public override int GetSize()
		{
			return Field_44013_b.Count;
		}

		/// <summary>
		/// the element in the slot that was clicked, bool for wether it was double clicked or not
		/// </summary>
		protected override void ElementClicked(int par1, bool par2)
		{
			StringTranslate.GetInstance().SetLanguage((string)Field_44013_b[par1]);
			Field_44015_a.Mc.FontRendererOld.SetUnicodeFlag(StringTranslate.GetInstance().IsUnicode());
			GuiLanguage.Func_44005_a(Field_44015_a).Language = (string)Field_44013_b[par1];
			//Field_44015_a.FontRenderer.SetBidiFlag(StringTranslate.IsBidrectional(GuiLanguage.Func_44005_a(Field_44015_a).Language));
			GuiLanguage.Func_46028_b(Field_44015_a).DisplayString = StringTranslate.GetInstance().TranslateKey("gui.done");
		}

		/// <summary>
		/// returns true if the element passed in is currently selected
		/// </summary>
		protected override bool IsSelected(int par1)
		{
			return ((string)Field_44013_b[par1]).Equals(StringTranslate.GetInstance().GetCurrentLanguage());
		}

		/// <summary>
		/// return the height of the content being scrolled
		/// </summary>
		protected override int GetContentHeight()
		{
			return GetSize() * 18;
		}

		protected override void DrawBackground()
		{
			Field_44015_a.DrawDefaultBackground();
		}

		protected override void DrawSlot(int par1, int par2, int par3, int par4, Tessellator par5Tessellator)
		{
			//Field_44015_a.FontRenderer.SetBidiFlag(true);
			Field_44015_a.DrawCenteredString(Field_44015_a.FontRenderer, (string)Field_44014_c[Field_44013_b[par1]], Field_44015_a.Width / 2, par3 + 1, 0xffffff);
			//Field_44015_a.FontRenderer.SetBidiFlag(StringTranslate.IsBidrectional(GuiLanguage.Func_44005_a(Field_44015_a).Language));
		}
	}
}