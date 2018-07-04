using Microsoft.Xna.Framework;

namespace net.minecraft.src
{
	public class GuiRenameWorld : GuiScreen
	{
		private GuiScreen ParentGuiScreen;
		private GuiTextField TheGuiTextField;
		private readonly string WorldName;

		public GuiRenameWorld(GuiScreen par1GuiScreen, string par2Str)
		{
			ParentGuiScreen = par1GuiScreen;
			WorldName = par2Str;
		}

		/// <summary>
		/// Called from the main game loop to update the screen.
		/// </summary>
		public override void UpdateScreen()
		{
			TheGuiTextField.UpdateCursorCounter();
		}

		/// <summary>
		/// Adds the buttons (and other controls) to the screen in question.
		/// </summary>
		public override void InitGui()
		{
			StringTranslate stringtranslate = StringTranslate.GetInstance();
			//Keyboard.EnableRepeatEvents(true);
			ControlList.Clear();
			ControlList.Add(new GuiButton(0, Width / 2 - 100, Height / 4 + 96 + 12, stringtranslate.TranslateKey("selectWorld.renameButton")));
			ControlList.Add(new GuiButton(1, Width / 2 - 100, Height / 4 + 120 + 12, stringtranslate.TranslateKey("gui.cancel")));
			ISaveFormat isaveformat = Mc.GetSaveLoader();
			WorldInfo worldinfo = isaveformat.GetWorldInfo(WorldName);
			string s = worldinfo.GetWorldName();
			TheGuiTextField = new GuiTextField(FontRenderer, Width / 2 - 100, 60, 200, 20);
			TheGuiTextField.Func_50033_b(true);
			TheGuiTextField.SetText(s);
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
				Mc.DisplayGuiScreen(ParentGuiScreen);
			}
			else if (par1GuiButton.Id == 0)
			{
				ISaveFormat isaveformat = Mc.GetSaveLoader();
				isaveformat.RenameWorld(WorldName, TheGuiTextField.GetText().Trim());
				Mc.DisplayGuiScreen(ParentGuiScreen);
			}
		}

		/// <summary>
		/// Fired when a key is typed. This is the equivalent of KeyListener.keyTyped(KeyEvent e).
		/// </summary>
		protected override void KeyTyped(char par1, int par2)
		{
			TheGuiTextField.Func_50037_a(par1, par2);
			((GuiButton)ControlList[0]).Enabled = TheGuiTextField.GetText().Trim().Length > 0;

			if (par1 == '\r')
			{
				ActionPerformed((GuiButton)ControlList[0]);
			}
		}

		/// <summary>
		/// Called when the mouse is clicked.
		/// </summary>
		protected override void MouseClicked(int par1, int par2, int par3)
		{
			base.MouseClicked(par1, par2, par3);
			TheGuiTextField.MouseClicked(par1, par2, par3);
		}

		/// <summary>
		/// Draws the screen and all the components in it.
		/// </summary>
		public override void DrawScreen(int par1, int par2, float par3)
		{
			StringTranslate stringtranslate = StringTranslate.GetInstance();
			DrawDefaultBackground();
			DrawCenteredString(FontRenderer, stringtranslate.TranslateKey("selectWorld.renameTitle"), Width / 2, (Height / 4 - 60) + 20, 0xffffff);
			DrawString(FontRenderer, stringtranslate.TranslateKey("selectWorld.enterName"), Width / 2 - 100, 47, 0xa0a0a0);
			TheGuiTextField.DrawTextBox();
			base.DrawScreen(par1, par2, par3);
		}
	}
}