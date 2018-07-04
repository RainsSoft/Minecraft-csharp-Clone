namespace net.minecraft.src
{

	using net.minecraft.src;

	public class GuiDisconnected : GuiScreen
	{
		/// <summary>
		/// The error message. </summary>
		private string ErrorMessage;

		/// <summary>
		/// The details about the error. </summary>
		private string ErrorDetail;

		public GuiDisconnected(string par1Str, string par2Str, object[] par3ArrayOfObj)
		{
			StringTranslate stringtranslate = StringTranslate.GetInstance();
			ErrorMessage = stringtranslate.TranslateKey(par1Str);

			if (par3ArrayOfObj != null)
			{
				ErrorDetail = stringtranslate.TranslateKeyFormat(par2Str, par3ArrayOfObj);
			}
			else
			{
				ErrorDetail = stringtranslate.TranslateKey(par2Str);
			}
		}

		/// <summary>
		/// Called from the main game loop to update the screen.
		/// </summary>
		public override void UpdateScreen()
		{
		}

		/// <summary>
		/// Fired when a key is typed. This is the equivalent of KeyListener.keyTyped(KeyEvent e).
		/// </summary>
		protected override void KeyTyped(char c, int i)
		{
		}

		/// <summary>
		/// Adds the buttons (and other controls) to the screen in question.
		/// </summary>
		public override void InitGui()
		{
			StringTranslate stringtranslate = StringTranslate.GetInstance();
			ControlList.Clear();
			ControlList.Add(new GuiButton(0, Width / 2 - 100, Height / 4 + 120 + 12, stringtranslate.TranslateKey("gui.toMenu")));
		}

		/// <summary>
		/// Fired when a control is clicked. This is the equivalent of ActionListener.actionPerformed(ActionEvent e).
		/// </summary>
		protected override void ActionPerformed(GuiButton par1GuiButton)
		{
			if (par1GuiButton.Id == 0)
			{
				Mc.DisplayGuiScreen(new GuiMainMenu());
			}
		}

		/// <summary>
		/// Draws the screen and all the components in it.
		/// </summary>
		public override void DrawScreen(int par1, int par2, float par3)
		{
			DrawDefaultBackground();
			DrawCenteredString(FontRenderer, ErrorMessage, Width / 2, Height / 2 - 50, 0xffffff);
			DrawCenteredString(FontRenderer, ErrorDetail, Width / 2, Height / 2 - 10, 0xffffff);
			base.DrawScreen(par1, par2, par3);
		}
	}

}