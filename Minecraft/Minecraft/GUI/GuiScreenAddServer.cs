using Microsoft.Xna.Framework;

namespace net.minecraft.src
{
	public class GuiScreenAddServer : GuiScreen
	{
		/// <summary>
		/// This GUI's parent GUI. </summary>
		private GuiScreen ParentGui;
		private GuiTextField ServerAddress;
		private GuiTextField ServerName;
		private ServerNBTStorage ServerNBTStorage;

		public GuiScreenAddServer(GuiScreen par1GuiScreen, ServerNBTStorage par2ServerNBTStorage)
		{
			ParentGui = par1GuiScreen;
			ServerNBTStorage = par2ServerNBTStorage;
		}

		/// <summary>
		/// Called from the main game loop to update the screen.
		/// </summary>
		public override void UpdateScreen()
		{
			ServerName.UpdateCursorCounter();
			ServerAddress.UpdateCursorCounter();
		}

		/// <summary>
		/// Adds the buttons (and other controls) to the screen in question.
		/// </summary>
		public override void InitGui()
		{
			StringTranslate stringtranslate = StringTranslate.GetInstance();
			//Keyboard.EnableRepeatEvents(true);
			ControlList.Clear();
			ControlList.Add(new GuiButton(0, Width / 2 - 100, Height / 4 + 96 + 12, stringtranslate.TranslateKey("addServer.add")));
			ControlList.Add(new GuiButton(1, Width / 2 - 100, Height / 4 + 120 + 12, stringtranslate.TranslateKey("gui.cancel")));
			ServerName = new GuiTextField(FontRenderer, Width / 2 - 100, 76, 200, 20);
			ServerName.setFocused(true);
			ServerName.SetText(ServerNBTStorage.Name);
			ServerAddress = new GuiTextField(FontRenderer, Width / 2 - 100, 116, 200, 20);
			ServerAddress.SetMaxStringLength(128);
			ServerAddress.SetText(ServerNBTStorage.Host);
			ControlList[0].Enabled = ServerAddress.GetText().Length > 0 && StringHelperClass.StringSplit(ServerAddress.GetText(), ":", true).Length > 0 && ServerName.GetText().Length > 0;
		}

		/// <summary>
		/// Called when the screen is unloaded. Used to disable keyboard repeat events
		/// </summary>
		public override void OnGuiClosed()
		{
			//Keyboard.EnableRepeatEvents(false);
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

			if (par1GuiButton.Id == 1)
			{
				ParentGui.ConfirmClicked(false, 0);
			}
			else if (par1GuiButton.Id == 0)
			{
				ServerNBTStorage.Name = ServerName.GetText();
				ServerNBTStorage.Host = ServerAddress.GetText();
				ParentGui.ConfirmClicked(true, 0);
			}
		}

		/// <summary>
		/// Fired when a key is typed. This is the equivalent of KeyListener.keyTyped(KeyEvent e).
		/// </summary>
		protected override void KeyTyped(char par1, int par2)
		{
			ServerName.Func_50037_a(par1, par2);
			ServerAddress.Func_50037_a(par1, par2);

			if (par1 == '\t')
			{
				if (ServerName.Func_50025_j())
				{
					ServerName.setFocused(false);
					ServerAddress.setFocused(true);
				}
				else
				{
					ServerName.setFocused(true);
					ServerAddress.setFocused(false);
				}
			}

			if (par1 == '\r')
			{
				ActionPerformed(ControlList[0]);
			}

			ControlList[0].Enabled = ServerAddress.GetText().Length > 0 && StringHelperClass.StringSplit(ServerAddress.GetText(), ":", true).Length > 0 && ServerName.GetText().Length > 0;

			if (ControlList[0].Enabled)
			{
				string s = ServerAddress.GetText().Trim();
				string[] @as = StringHelperClass.StringSplit(s, ":", true);

				if (@as.Length > 2)
				{
					ControlList[0].Enabled = false;
				}
			}
		}

		/// <summary>
		/// Called when the mouse is clicked.
		/// </summary>
		protected override void MouseClicked(int par1, int par2, int par3)
		{
			base.MouseClicked(par1, par2, par3);
			ServerAddress.MouseClicked(par1, par2, par3);
			ServerName.MouseClicked(par1, par2, par3);
		}

		/// <summary>
		/// Draws the screen and all the components in it.
		/// </summary>
		public override void DrawScreen(int par1, int par2, float par3)
		{
			StringTranslate stringtranslate = StringTranslate.GetInstance();
			DrawDefaultBackground();
			DrawCenteredString(FontRenderer, stringtranslate.TranslateKey("addServer.title"), Width / 2, (Height / 4 - 60) + 20, 0xffffff);
			DrawString(FontRenderer, stringtranslate.TranslateKey("addServer.enterName"), Width / 2 - 100, 63, 0xa0a0a0);
			DrawString(FontRenderer, stringtranslate.TranslateKey("addServer.enterIp"), Width / 2 - 100, 104, 0xa0a0a0);
			ServerName.DrawTextBox();
			ServerAddress.DrawTextBox();
			base.DrawScreen(par1, par2, par3);
		}
	}
}