namespace net.minecraft.src
{
	public abstract class GuiConfirmOpenLink : GuiYesNo
	{
		private string Field_50054_a;
		private string Field_50053_b;

		public GuiConfirmOpenLink(GuiScreen par1GuiScreen, string par2Str, int par3) : base(par1GuiScreen, StringTranslate.GetInstance().TranslateKey("chat.link.confirm"), par2Str, par3)
		{
			StringTranslate stringtranslate = StringTranslate.GetInstance();
			ButtonText1 = stringtranslate.TranslateKey("gui.yes");
			ButtonText2 = stringtranslate.TranslateKey("gui.no");
			Field_50053_b = stringtranslate.TranslateKey("chat.copy");
			Field_50054_a = stringtranslate.TranslateKey("chat.link.warning");
		}

		/// <summary>
		/// Adds the buttons (and other controls) to the screen in question.
		/// </summary>
		public override void InitGui()
		{
			ControlList.Add(new GuiButton(0, (Width / 3 - 83) + 0, Height / 6 + 96, 100, 20, ButtonText1));
			ControlList.Add(new GuiButton(2, (Width / 3 - 83) + 105, Height / 6 + 96, 100, 20, Field_50053_b));
			ControlList.Add(new GuiButton(1, (Width / 3 - 83) + 210, Height / 6 + 96, 100, 20, ButtonText2));
		}

		/// <summary>
		/// Fired when a control is clicked. This is the equivalent of ActionListener.actionPerformed(ActionEvent e).
		/// </summary>
		protected override void ActionPerformed(GuiButton par1GuiButton)
		{
			if (par1GuiButton.Id == 2)
			{
				Func_50052_d();
				base.ActionPerformed(ControlList[1]);
			}
			else
			{
				base.ActionPerformed(par1GuiButton);
			}
		}

		public abstract void Func_50052_d();

		/// <summary>
		/// Draws the screen and all the components in it.
		/// </summary>
		public override void DrawScreen(int par1, int par2, float par3)
		{
			base.DrawScreen(par1, par2, par3);
			DrawCenteredString(FontRenderer, Field_50054_a, Width / 2, 110, 0xffcccc);
		}
	}
}