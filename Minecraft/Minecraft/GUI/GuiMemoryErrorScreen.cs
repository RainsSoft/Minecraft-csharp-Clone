namespace net.minecraft.src
{

	using net.minecraft.src;

	public class GuiMemoryErrorScreen : GuiScreen
	{
		public GuiMemoryErrorScreen()
		{
		}

		/// <summary>
		/// Called from the main game loop to update the screen.
		/// </summary>
		public override void UpdateScreen()
		{
		}

		/// <summary>
		/// Adds the buttons (and other controls) to the screen in question.
		/// </summary>
		public override void InitGui()
		{
			StringTranslate stringtranslate = StringTranslate.GetInstance();
			ControlList.Clear();
			ControlList.Add(new GuiSmallButton(0, Width / 2 - 155, Height / 4 + 120 + 12, stringtranslate.TranslateKey("gui.toMenu")));
			ControlList.Add(new GuiSmallButton(1, (Width / 2 - 155) + 160, Height / 4 + 120 + 12, stringtranslate.TranslateKey("menu.quit")));
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
			else if (par1GuiButton.Id == 1)
			{
				Mc.Shutdown();
			}
		}

		/// <summary>
		/// Fired when a key is typed. This is the equivalent of KeyListener.keyTyped(KeyEvent e).
		/// </summary>
		protected override void KeyTyped(char c, int i)
		{
		}

		/// <summary>
		/// Draws the screen and all the components in it.
		/// </summary>
		public override void DrawScreen(int par1, int par2, float par3)
		{
			DrawDefaultBackground();
			DrawCenteredString(FontRenderer, "Out of memory!", Width / 2, (Height / 4 - 60) + 20, 0xffffff);
			DrawString(FontRenderer, "Minecraft has run out of memory.", Width / 2 - 140, (Height / 4 - 60) + 60 + 0, 0xa0a0a0);
			DrawString(FontRenderer, "This could be caused by a bug in the game or by the", Width / 2 - 140, (Height / 4 - 60) + 60 + 18, 0xa0a0a0);
			DrawString(FontRenderer, "Java Virtual Machine not being allocated enough", Width / 2 - 140, (Height / 4 - 60) + 60 + 27, 0xa0a0a0);
			DrawString(FontRenderer, "memory. If you are playing in a web browser, try", Width / 2 - 140, (Height / 4 - 60) + 60 + 36, 0xa0a0a0);
			DrawString(FontRenderer, "downloading the game and playing it offline.", Width / 2 - 140, (Height / 4 - 60) + 60 + 45, 0xa0a0a0);
			DrawString(FontRenderer, "To prevent level corruption, the current game has quit.", Width / 2 - 140, (Height / 4 - 60) + 60 + 63, 0xa0a0a0);
			DrawString(FontRenderer, "We've tried to free up enough memory to let you go back to", Width / 2 - 140, (Height / 4 - 60) + 60 + 81, 0xa0a0a0);
			DrawString(FontRenderer, "the main menu and back to playing, but this may not have worked.", Width / 2 - 140, (Height / 4 - 60) + 60 + 90, 0xa0a0a0);
			DrawString(FontRenderer, "Please restart the game if you see this message again.", Width / 2 - 140, (Height / 4 - 60) + 60 + 99, 0xa0a0a0);
			base.DrawScreen(par1, par2, par3);
		}
	}

}