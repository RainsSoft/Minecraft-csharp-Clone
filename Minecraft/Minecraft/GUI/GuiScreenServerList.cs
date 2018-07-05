using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace net.minecraft.src
{
	public class GuiScreenServerList : GuiScreen
	{
		private static string Field_52009_d = "";

		/// <summary>
		/// Needed a change as a local variable was conflicting on construct </summary>
		private readonly GuiScreen GuiScreen;

		/// <summary>
		/// This GUI's instance to the server list's storage </summary>
		private readonly ServerNBTStorage ServerListStorage;
		private GuiTextField ServerTextField;

		public GuiScreenServerList(GuiScreen par1GuiScreen, ServerNBTStorage par2ServerNBTStorage)
		{
			GuiScreen = par1GuiScreen;
			ServerListStorage = par2ServerNBTStorage;
		}

		/// <summary>
		/// Called from the main game loop to update the screen.
		/// </summary>
		public override void UpdateScreen()
		{
			ServerTextField.UpdateCursorCounter();
		}

		/// <summary>
		/// Adds the buttons (and other controls) to the screen in question.
		/// </summary>
		public override void InitGui()
		{
			StringTranslate stringtranslate = StringTranslate.GetInstance();
			//Keyboard.EnableRepeatEvents(true);
			ControlList.Clear();
			ControlList.Add(new GuiButton(0, Width / 2 - 100, Height / 4 + 96 + 12, stringtranslate.TranslateKey("selectServer.select")));
			ControlList.Add(new GuiButton(1, Width / 2 - 100, Height / 4 + 120 + 12, stringtranslate.TranslateKey("gui.cancel")));
			ServerTextField = new GuiTextField(FontRenderer, Width / 2 - 100, 116, 200, 20);
			ServerTextField.SetMaxStringLength(128);
			ServerTextField.setFocused(true);
			ServerTextField.SetText(Field_52009_d);
			ControlList[0].Enabled = ServerTextField.GetText().Length > 0 && StringHelperClass.StringSplit(ServerTextField.GetText(), ":", true).Length > 0;
		}

		/// <summary>
		/// Called when the screen is unloaded. Used to disable keyboard repeat events
		/// </summary>
		public override void OnGuiClosed()
		{
			//Keyboard.EnableRepeatEvents(false);
			Field_52009_d = ServerTextField.GetText();
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
				GuiScreen.ConfirmClicked(false, 0);
			}
			else if (par1GuiButton.Id == 0)
			{
				ServerListStorage.Host = ServerTextField.GetText();
				GuiScreen.ConfirmClicked(true, 0);
			}
		}

		/// <summary>
		/// Fired when a key is typed. This is the equivalent of KeyListener.keyTyped(KeyEvent e).
		/// </summary>
		protected override void KeyTyped(char par1, int par2)
		{
			ServerTextField.Func_50037_a(par1, par2);

			if (par1 == 034)
			{
				ActionPerformed(ControlList[0]);
			}

			ControlList[0].Enabled = ServerTextField.GetText().Length > 0 && StringHelperClass.StringSplit(ServerTextField.GetText(), ":", true).Length > 0;
		}

		/// <summary>
		/// Called when the mouse is clicked.
		/// </summary>
		protected override void MouseClicked(int par1, int par2, int par3)
		{
			base.MouseClicked(par1, par2, par3);
			ServerTextField.MouseClicked(par1, par2, par3);
		}

		/// <summary>
		/// Draws the screen and all the components in it.
		/// </summary>
		public override void DrawScreen(int par1, int par2, float par3)
		{
			StringTranslate stringtranslate = StringTranslate.GetInstance();
			DrawDefaultBackground();
			DrawCenteredString(FontRenderer, stringtranslate.TranslateKey("selectServer.direct"), Width / 2, (Height / 4 - 60) + 20, 0xffffff);
			DrawString(FontRenderer, stringtranslate.TranslateKey("addServer.enterIp"), Width / 2 - 100, 100, 0xa0a0a0);
			ServerTextField.DrawTextBox();
			base.DrawScreen(par1, par2, par3);
		}
	}
}