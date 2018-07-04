using Microsoft.Xna.Framework;

namespace net.minecraft.src
{
	using net.minecraft.src;

	public class GuiStats : GuiScreen
	{
		private static RenderItem RenderItem = new RenderItem();
		protected GuiScreen ParentGui;

		/// <summary>
		/// The title of the stats screen. </summary>
		protected string StatsTitle;

		/// <summary>
		/// The slot for general stats. </summary>
		private GuiSlotStatsGeneral SlotGeneral;

		/// <summary>
		/// The slot for item stats. </summary>
		private GuiSlotStatsItem SlotItem;

		/// <summary>
		/// The slot for block stats. </summary>
		private GuiSlotStatsBlock SlotBlock;
		private StatFileWriter StatFileWriter;

		/// <summary>
		/// The currently-selected slot. </summary>
		private GuiSlot SelectedSlot;

		public GuiStats(GuiScreen par1GuiScreen, StatFileWriter par2StatFileWriter)
		{
			StatsTitle = "Select world";
			SelectedSlot = null;
			ParentGui = par1GuiScreen;
			StatFileWriter = par2StatFileWriter;
		}

		/// <summary>
		/// Adds the buttons (and other controls) to the screen in question.
		/// </summary>
		public override void InitGui()
		{
			StatsTitle = StatCollector.TranslateToLocal("gui.stats");
			SlotGeneral = new GuiSlotStatsGeneral(this);
			SlotGeneral.RegisterScrollButtons(ControlList, 1, 1);
			SlotItem = new GuiSlotStatsItem(this);
			SlotItem.RegisterScrollButtons(ControlList, 1, 1);
			SlotBlock = new GuiSlotStatsBlock(this);
			SlotBlock.RegisterScrollButtons(ControlList, 1, 1);
			SelectedSlot = SlotGeneral;
			AddHeaderButtons();
		}

		/// <summary>
		/// Creates the buttons that appear at the top of the Stats GUI.
		/// </summary>
		public virtual void AddHeaderButtons()
		{
			StringTranslate stringtranslate = StringTranslate.GetInstance();
			ControlList.Add(new GuiButton(0, Width / 2 + 4, Height - 28, 150, 20, stringtranslate.TranslateKey("gui.done")));
			ControlList.Add(new GuiButton(1, Width / 2 - 154, Height - 52, 100, 20, stringtranslate.TranslateKey("stat.generalButton")));
			GuiButton guibutton;
			ControlList.Add(guibutton = new GuiButton(2, Width / 2 - 46, Height - 52, 100, 20, stringtranslate.TranslateKey("stat.blocksButton")));
			GuiButton guibutton1;
			ControlList.Add(guibutton1 = new GuiButton(3, Width / 2 + 62, Height - 52, 100, 20, stringtranslate.TranslateKey("stat.itemsButton")));

			if (SlotBlock.GetSize() == 0)
			{
				guibutton.Enabled = false;
			}

			if (SlotItem.GetSize() == 0)
			{
				guibutton1.Enabled = false;
			}
		}

		/// <summary>
		/// Fired when a control is clicked. This is the equivalent of ActionListener.actionPerformed(ActionEvent e).
		/// </summary>
		protected override void ActionPerformed(GuiButton par1GuiButton)
		{
			if (!par1GuiButton.Enabled)
			{
				return;
			}

			if (par1GuiButton.Id == 0)
			{
				Mc.DisplayGuiScreen(ParentGui);
			}
			else if (par1GuiButton.Id == 1)
			{
				SelectedSlot = SlotGeneral;
			}
			else if (par1GuiButton.Id == 3)
			{
				SelectedSlot = SlotItem;
			}
			else if (par1GuiButton.Id == 2)
			{
				SelectedSlot = SlotBlock;
			}
			else
			{
				SelectedSlot.ActionPerformed(par1GuiButton);
			}
		}

		/// <summary>
		/// Draws the screen and all the components in it.
		/// </summary>
		public override void DrawScreen(int par1, int par2, float par3)
		{
			SelectedSlot.DrawScreen(par1, par2, par3);
			DrawCenteredString(FontRenderer, StatsTitle, Width / 2, 20, 0xffffff);
			base.DrawScreen(par1, par2, par3);
		}

		/// <summary>
		/// Draws the item sprite on top of the background sprite.
		/// </summary>
		private void DrawItemSprite(int par1, int par2, int par3)
		{
			DrawButtonBackground(par1 + 1, par2 + 1);
			//GL.Enable(EnableCap.RescaleNormal);
			RenderHelper.EnableGUIStandardItemLighting();
			RenderItem.DrawItemIntoGui(FontRenderer, Mc.RenderEngineOld, par3, 0, Item.ItemsList[par3].GetIconFromDamage(0), par1 + 2, par2 + 2);
			RenderHelper.DisableStandardItemLighting();
			//GL.Disable(EnableCap.RescaleNormal);
		}

		/// <summary>
		/// Draws a gray box that serves as a button background.
		/// </summary>
		private void DrawButtonBackground(int par1, int par2)
		{
			DrawSprite(par1, par2, 0, 0);
		}

		/// <summary>
		/// Draws a sprite from /gui/slot.png.
		/// </summary>
		private void DrawSprite(int par1, int par2, int par3, int par4)
		{
			int i = Mc.RenderEngineOld.GetTexture("/gui/slot.png");
			//GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
			Mc.RenderEngineOld.BindTexture(i);
			Tessellator tessellator = Tessellator.Instance;
			tessellator.StartDrawingQuads();
			tessellator.AddVertexWithUV(par1 + 0, par2 + 18, ZLevel, (float)(par3 + 0) * 0.0078125F, (float)(par4 + 18) * 0.0078125F);
			tessellator.AddVertexWithUV(par1 + 18, par2 + 18, ZLevel, (float)(par3 + 18) * 0.0078125F, (float)(par4 + 18) * 0.0078125F);
			tessellator.AddVertexWithUV(par1 + 18, par2 + 0, ZLevel, (float)(par3 + 18) * 0.0078125F, (float)(par4 + 0) * 0.0078125F);
			tessellator.AddVertexWithUV(par1 + 0, par2 + 0, ZLevel, (float)(par3 + 0) * 0.0078125F, (float)(par4 + 0) * 0.0078125F);
			tessellator.Draw();
		}

        public static StatFileWriter GetStatsFileWriter(GuiStats par0GuiStats)
		{
			return par0GuiStats.StatFileWriter;
		}

		public static Minecraft GetMinecraft(GuiStats par0GuiStats)
		{
			return par0GuiStats.Mc;
		}

		/// <summary>
		/// Draws a sprite from /gui/slot.png.
		/// </summary>
		public static void DrawSprite(GuiStats par0GuiStats, int par1, int par2, int par3, int par4)
		{
			par0GuiStats.DrawSprite(par1, par2, par3, par4);
		}

        public static FontRenderer GetFontRenderer(GuiStats par0GuiStats)
        {
            return par0GuiStats.FontRenderer;
        }

        public static void DrawGradientRect(GuiStats par0GuiStats, int par1, int par2, int par3, int par4, int par5, int par6)
		{
			par0GuiStats.DrawGradientRect(par1, par2, par3, par4, par5, par6);
		}

		/// <summary>
		/// Draws the item sprite on top of the background sprite.
		/// </summary>
		public static void DrawItemSprite(GuiStats par0GuiStats, int par1, int par2, int par3)
		{
			par0GuiStats.DrawItemSprite(par1, par2, par3);
		}
	}
}