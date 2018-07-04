namespace net.minecraft.src
{
	public class GuiYesNo : GuiScreen
	{
		/// <summary>
		/// A reference to the screen object that created this. Used for navigating between screens.
		/// </summary>
		private GuiScreen ParentScreen;

		/// <summary>
		/// First line of text. </summary>
		private string Message1;

		/// <summary>
		/// Second line of text. </summary>
		private string Message2;

		/// <summary>
		/// The text shown for the first button in GuiYesNo </summary>
		protected string ButtonText1;

		/// <summary>
		/// The text shown for the second button in GuiYesNo </summary>
		protected string ButtonText2;

		/// <summary>
		/// World number to be deleted. </summary>
		private int WorldNumber;

		public GuiYesNo(GuiScreen par1GuiScreen, string par2Str, string par3Str, int par4)
		{
			ParentScreen = par1GuiScreen;
			Message1 = par2Str;
			Message2 = par3Str;
			WorldNumber = par4;
			StringTranslate stringtranslate = StringTranslate.GetInstance();
			ButtonText1 = stringtranslate.TranslateKey("gui.yes");
			ButtonText2 = stringtranslate.TranslateKey("gui.no");
		}

		public GuiYesNo(GuiScreen par1GuiScreen, string par2Str, string par3Str, string par4Str, string par5Str, int par6)
		{
			ParentScreen = par1GuiScreen;
			Message1 = par2Str;
			Message2 = par3Str;
			ButtonText1 = par4Str;
			ButtonText2 = par5Str;
			WorldNumber = par6;
		}

		/// <summary>
		/// Adds the buttons (and other controls) to the screen in question.
		/// </summary>
		public override void InitGui()
		{
			ControlList.Add(new GuiSmallButton(0, Width / 2 - 155, Height / 6 + 96, ButtonText1));
			ControlList.Add(new GuiSmallButton(1, (Width / 2 - 155) + 160, Height / 6 + 96, ButtonText2));
		}

		/// <summary>
		/// Fired when a control is clicked. This is the equivalent of ActionListener.actionPerformed(ActionEvent e).
		/// </summary>
		protected override void ActionPerformed(GuiButton par1GuiButton)
		{
			ParentScreen.ConfirmClicked(par1GuiButton.Id == 0, WorldNumber);
		}

		/// <summary>
		/// Draws the screen and all the components in it.
		/// </summary>
		public override void DrawScreen(int par1, int par2, float par3)
		{
			DrawDefaultBackground();
			DrawCenteredString(FontRenderer, Message1, Width / 2, 70, 0xffffff);
			DrawCenteredString(FontRenderer, Message2, Width / 2, 90, 0xffffff);
			base.DrawScreen(par1, par2, par3);
		}
	}

}