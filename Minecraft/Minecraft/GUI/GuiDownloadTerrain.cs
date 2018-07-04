namespace net.minecraft.src
{
	public class GuiDownloadTerrain : GuiScreen
	{
		/// <summary>
		/// Network object that downloads the terrain data. </summary>
		private NetClientHandler NetHandler;

		/// <summary>
		/// Counts the number of screen updates. </summary>
		private int UpdateCounter;

		public GuiDownloadTerrain(NetClientHandler par1NetClientHandler)
		{
			UpdateCounter = 0;
			NetHandler = par1NetClientHandler;
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
			ControlList.Clear();
		}

		/// <summary>
		/// Called from the main game loop to update the screen.
		/// </summary>
		public override void UpdateScreen()
		{
			UpdateCounter++;

			if (UpdateCounter % 20 == 0)
			{
				NetHandler.AddToSendQueue(new Packet0KeepAlive());
			}

			if (NetHandler != null)
			{
				NetHandler.ProcessReadPackets();
			}
		}

		/// <summary>
		/// Fired when a control is clicked. This is the equivalent of ActionListener.actionPerformed(ActionEvent e).
		/// </summary>
		protected override void ActionPerformed(GuiButton guibutton)
		{
		}

		/// <summary>
		/// Draws the screen and all the components in it.
		/// </summary>
		public override void DrawScreen(int par1, int par2, float par3)
		{
			DrawBackground(0);
			StringTranslate stringtranslate = StringTranslate.GetInstance();
			DrawCenteredString(FontRenderer, stringtranslate.TranslateKey("multiplayer.downloadingTerrain"), Width / 2, Height / 2 - 50, 0xffffff);
			base.DrawScreen(par1, par2, par3);
		}
	}

}