using System;
using System.Text;

namespace net.minecraft.src
{
	class GuiWorldSlot : GuiSlot
	{
		readonly GuiSelectWorld ParentWorldGui;

		public GuiWorldSlot(GuiSelectWorld par1GuiSelectWorld) : base(par1GuiSelectWorld.Mc, par1GuiSelectWorld.Width, par1GuiSelectWorld.Height, 32, par1GuiSelectWorld.Height - 64, 36)
		{
			ParentWorldGui = par1GuiSelectWorld;
		}

		/// <summary>
		/// Gets the size of the current slot list.
		/// </summary>
		public override int GetSize()
		{
			return GuiSelectWorld.GetSize(ParentWorldGui).Count;
		}

		/// <summary>
		/// the element in the slot that was clicked, bool for wether it was double clicked or not
		/// </summary>
		protected override void ElementClicked(int par1, bool par2)
		{
			GuiSelectWorld.OnElementSelected(ParentWorldGui, par1);
			bool flag = GuiSelectWorld.GetSelectedWorld(ParentWorldGui) >= 0 && GuiSelectWorld.GetSelectedWorld(ParentWorldGui) < GetSize();
			GuiSelectWorld.GetSelectButton(ParentWorldGui).Enabled = flag;
			GuiSelectWorld.GetRenameButton(ParentWorldGui).Enabled = flag;
			GuiSelectWorld.GetDeleteButton(ParentWorldGui).Enabled = flag;

			if (par2 && flag)
			{
				ParentWorldGui.SelectWorld(par1);
			}
		}

		/// <summary>
		/// returns true if the element passed in is currently selected
		/// </summary>
		protected override bool IsSelected(int par1)
		{
			return par1 == GuiSelectWorld.GetSelectedWorld(ParentWorldGui);
		}

		/// <summary>
		/// return the height of the content being scrolled
		/// </summary>
		protected override int GetContentHeight()
		{
			return GuiSelectWorld.GetSize(ParentWorldGui).Count * 36;
		}

		protected override void DrawBackground()
		{
			ParentWorldGui.DrawDefaultBackground();
		}

		protected override void DrawSlot(int par1, int par2, int par3, int par4, Tessellator par5Tessellator)
		{
			SaveFormatComparator saveformatcomparator = (SaveFormatComparator)GuiSelectWorld.GetSize(ParentWorldGui)[par1];
			string s = saveformatcomparator.GetDisplayName();

			if (s == null || MathHelper2.StringNullOrLengthZero(s))
			{
				s = (new StringBuilder()).Append(GuiSelectWorld.GetLocalizedWorldName(ParentWorldGui)).Append(" ").Append(par1 + 1).ToString();
			}

			string s1 = saveformatcomparator.GetFileName();
			s1 = (new StringBuilder()).Append(s1).Append(" (").Append(new DateTime(saveformatcomparator.GetLastTimePlayed())).ToString();
			s1 = (new StringBuilder()).Append(s1).Append(")").ToString();
			string s2 = "";

			if (saveformatcomparator.RequiresConversion())
			{
				s2 = (new StringBuilder()).Append(GuiSelectWorld.GetLocalizedMustConvert(ParentWorldGui)).Append(" ").Append(s2).ToString();
			}
			else
			{
				s2 = GuiSelectWorld.GetLocalizedGameMode(ParentWorldGui)[saveformatcomparator.GetGameType()];

				if (saveformatcomparator.IsHardcoreModeEnabled())
				{
					s2 = (new StringBuilder()).Append(FontRenderer.SpecialChar).Append("4").Append(StatCollector.TranslateToLocal("gameMode.hardcore")).Append(FontRenderer.SpecialChar).Append("8").ToString();
				}
			}

			ParentWorldGui.DrawString(ParentWorldGui.FontRenderer, s, par2 + 2, par3 + 1, 0xffffff);
			ParentWorldGui.DrawString(ParentWorldGui.FontRenderer, s1, par2 + 2, par3 + 12, 0x808080);
			ParentWorldGui.DrawString(ParentWorldGui.FontRenderer, s2, par2 + 2, par3 + 12 + 10, 0x808080);
		}
	}
}