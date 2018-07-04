namespace net.minecraft.src
{

	using net.minecraft.src;

	public class GuiConflictWarning : GuiScreen
	{
		/// <summary>
		/// Counts the number of screen updates. Not used. </summary>
		private int UpdateCounter;

		public GuiConflictWarning()
		{
			UpdateCounter = 0;
		}

		/// <summary>
		/// Called from the main game loop to update the screen.
		/// </summary>
		public override void UpdateScreen()
		{
			UpdateCounter++;
		}

		/// <summary>
		/// Adds the buttons (and other controls) to the screen in question.
		/// </summary>
		public override void InitGui()
		{
			ControlList.Clear();
			ControlList.Add(new GuiButton(0, Width / 2 - 100, Height / 4 + 120 + 12, "Back to title screen"));
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
				Mc.DisplayGuiScreen(new GuiMainMenu());
			}
		}

		/// <summary>
		/// Draws the screen and all the components in it.
		/// </summary>
		public override void DrawScreen(int par1, int par2, float par3)
		{
			DrawDefaultBackground();
			DrawCenteredString(FontRenderer, "Level save conflict", Width / 2, (Height / 4 - 60) + 20, 0xffffff);
			DrawString(FontRenderer, "Minecraft detected a conflict in the level save data.", Width / 2 - 140, (Height / 4 - 60) + 60 + 0, 0xa0a0a0);
			DrawString(FontRenderer, "This could be caused by two copies of the game", Width / 2 - 140, (Height / 4 - 60) + 60 + 18, 0xa0a0a0);
			DrawString(FontRenderer, "accessing the same level.", Width / 2 - 140, (Height / 4 - 60) + 60 + 27, 0xa0a0a0);
			DrawString(FontRenderer, "To prevent level corruption, the current game has quit.", Width / 2 - 140, (Height / 4 - 60) + 60 + 45, 0xa0a0a0);
			base.DrawScreen(par1, par2, par3);
		}
	}

}