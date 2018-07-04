namespace net.minecraft.src
{
	public class GuiErrorScreen : GuiScreen
	{
		/// <summary>
		/// Unused class. Would contain a message drawn to the center of the screen.
		/// </summary>
		private string Message1;

		/// <summary>
		/// Unused class. Would contain a message drawn to the center of the screen.
		/// </summary>
		private string Message2;

		/// <summary>
		/// Adds the buttons (and other controls) to the screen in question.
		/// </summary>
		public override void InitGui()
		{
		}

		/// <summary>
		/// Draws the screen and all the components in it.
		/// </summary>
		public override void DrawScreen(int par1, int par2, float par3)
		{
			DrawGradientRect(0, 0, Width, Height, 0xff40202, 0xff50101);
			DrawCenteredString(FontRenderer, Message1, Width / 2, 90, 0xffffff);
			DrawCenteredString(FontRenderer, Message2, Width / 2, 110, 0xffffff);
			base.DrawScreen(par1, par2, par3);
		}

		/// <summary>
		/// Fired when a key is typed. This is the equivalent of KeyListener.keyTyped(KeyEvent e).
		/// </summary>
		protected override void KeyTyped(char c, int i)
		{
		}
	}
}