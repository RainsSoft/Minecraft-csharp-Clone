using System.Text;

namespace net.minecraft.src
{
	public class GuiLanguage : GuiScreen
	{
		/// <summary>
		/// This GUI's parent GUI. </summary>
		protected GuiScreen ParentGui;

		/// <summary>
		/// Timer used to update texture packs, decreases every tick and is reset to 20 and updates texture packs upon
		/// reaching 0.
		/// </summary>
		private int UpdateTimer;

		/// <summary>
		/// This GUI's language list. </summary>
		private GuiSlotLanguage LanguageList;
		private readonly GameSettings Field_44006_d;

		/// <summary>
		/// This GUI's 'Done' button. </summary>
		private GuiSmallButton DoneButton;

		public GuiLanguage(GuiScreen par1GuiScreen, GameSettings par2GameSettings)
		{
			UpdateTimer = -1;
			ParentGui = par1GuiScreen;
			Field_44006_d = par2GameSettings;
		}

		/// <summary>
		/// Adds the buttons (and other controls) to the screen in question.
		/// </summary>
		public override void InitGui()
		{
			StringTranslate stringtranslate = StringTranslate.GetInstance();
			ControlList.Add(DoneButton = new GuiSmallButton(6, Width / 2 - 75, Height - 38, stringtranslate.TranslateKey("gui.done")));
			LanguageList = new GuiSlotLanguage(this);
			LanguageList.RegisterScrollButtons(ControlList, 7, 8);
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

			switch (par1GuiButton.Id)
			{
				case 6:
					Field_44006_d.SaveOptions();
					Mc.DisplayGuiScreen(ParentGui);
					break;

				default:
					LanguageList.ActionPerformed(par1GuiButton);
					break;

				case 5:
					break;
			}
		}

		/// <summary>
		/// Called when the mouse is clicked.
		/// </summary>
		protected override void MouseClicked(int par1, int par2, int par3)
		{
			base.MouseClicked(par1, par2, par3);
		}

		/// <summary>
		/// Called when the mouse is moved or a mouse button is released.  Signature: (mouseX, mouseY, which) which==-1 is
		/// mouseMove, which==0 or which==1 is mouseUp
		/// </summary>
		protected override void MouseMovedOrUp(int par1, int par2, int par3)
		{
			base.MouseMovedOrUp(par1, par2, par3);
		}

		/// <summary>
		/// Draws the screen and all the components in it.
		/// </summary>
		public override void DrawScreen(int par1, int par2, float par3)
		{
			LanguageList.DrawScreen(par1, par2, par3);

			if (UpdateTimer <= 0)
			{
				Mc.TexturePackList.UpdateAvaliableTexturePacks();
				UpdateTimer += 20;
			}

			StringTranslate stringtranslate = StringTranslate.GetInstance();
			DrawCenteredString(FontRenderer, stringtranslate.TranslateKey("options.language"), Width / 2, 16, 0xffffff);
			DrawCenteredString(FontRenderer, (new StringBuilder()).Append("(").Append(stringtranslate.TranslateKey("options.languageWarning")).Append(")").ToString(), Width / 2, Height - 56, 0x808080);
			base.DrawScreen(par1, par2, par3);
		}

		/// <summary>
		/// Called from the main game loop to update the screen.
		/// </summary>
		public override void UpdateScreen()
		{
			base.UpdateScreen();
			UpdateTimer--;
		}

		public static GameSettings Func_44005_a(GuiLanguage par0GuiLanguage)
		{
			return par0GuiLanguage.Field_44006_d;
		}

		public static GuiSmallButton Func_46028_b(GuiLanguage par0GuiLanguage)
		{
			return par0GuiLanguage.DoneButton;
		}
	}
}