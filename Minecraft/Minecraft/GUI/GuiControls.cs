using System.Text;

namespace net.minecraft.src
{
	public class GuiControls : GuiScreen
	{
		/// <summary>
		/// A reference to the screen object that created this. Used for navigating between screens.
		/// </summary>
		private GuiScreen ParentScreen;

		/// <summary>
		/// The title string that is displayed in the top-center of the screen. </summary>
		protected string ScreenTitle;

		/// <summary>
		/// Reference to the GameSettings object. </summary>
		private GameSettings Options;

		/// <summary>
		/// The ID of the  button that has been pressed. </summary>
		private int ButtonId;

		public GuiControls(GuiScreen par1GuiScreen, GameSettings par2GameSettings)
		{
			ScreenTitle = "Controls";
			ButtonId = -1;
			ParentScreen = par1GuiScreen;
			Options = par2GameSettings;
		}

		private int Func_20080_j()
		{
			return Width / 2 - 155;
		}

		/// <summary>
		/// Adds the buttons (and other controls) to the screen in question.
		/// </summary>
		public override void InitGui()
		{
			StringTranslate stringtranslate = StringTranslate.GetInstance();
			int i = Func_20080_j();

			for (int j = 0; j < Options.KeyBindings.Length; j++)
			{
				ControlList.Add(new GuiSmallButton(j, i + (j % 2) * 160, Height / 6 + 24 * (j >> 1), 70, 20, Options.GetOptionDisplayString(j)));
			}

			ControlList.Add(new GuiButton(200, Width / 2 - 100, Height / 6 + 168, stringtranslate.TranslateKey("gui.done")));
			ScreenTitle = stringtranslate.TranslateKey("controls.title");
		}

		/// <summary>
		/// Fired when a control is clicked. This is the equivalent of ActionListener.actionPerformed(ActionEvent e).
		/// </summary>
		protected override void ActionPerformed(GuiButton par1GuiButton)
		{
			for (int i = 0; i < Options.KeyBindings.Length; i++)
			{
				ControlList[i].DisplayString = Options.GetOptionDisplayString(i);
			}

			if (par1GuiButton.Id == 200)
			{
				Mc.DisplayGuiScreen(ParentScreen);
			}
			else
			{
				ButtonId = par1GuiButton.Id;
				par1GuiButton.DisplayString = (new StringBuilder()).Append("> ").Append(Options.GetOptionDisplayString(par1GuiButton.Id)).Append(" <").ToString();
			}
		}

		/// <summary>
		/// Called when the mouse is clicked.
		/// </summary>
		protected override void MouseClicked(int par1, int par2, int par3)
		{
			if (ButtonId >= 0)
			{
				Options.SetKeyBinding(ButtonId, -100 + par3);
				ControlList[ButtonId].DisplayString = Options.GetOptionDisplayString(ButtonId);
				ButtonId = -1;
				KeyBinding.ResetKeyBindingArrayAndHash();
			}
			else
			{
				base.MouseClicked(par1, par2, par3);
			}
		}

		/// <summary>
		/// Fired when a key is typed. This is the equivalent of KeyListener.keyTyped(KeyEvent e).
		/// </summary>
		protected override void KeyTyped(char par1, int par2)
		{
			if (ButtonId >= 0)
			{
				Options.SetKeyBinding(ButtonId, par2);
				ControlList[ButtonId].DisplayString = Options.GetOptionDisplayString(ButtonId);
				ButtonId = -1;
				KeyBinding.ResetKeyBindingArrayAndHash();
			}
			else
			{
				base.KeyTyped(par1, par2);
			}
		}

		/// <summary>
		/// Draws the screen and all the components in it.
		/// </summary>
		public override void DrawScreen(int par1, int par2, float par3)
		{
			DrawDefaultBackground();
			DrawCenteredString(FontRenderer, ScreenTitle, Width / 2, 20, 0xffffff);
			int i = Func_20080_j();

			for (int j = 0; j < Options.KeyBindings.Length; j++)
			{
				bool flag = false;
				int k = 0;

				do
				{
					if (k >= Options.KeyBindings.Length)
					{
						break;
					}

					if (k != j && Options.KeyBindings[j].KeyCode == Options.KeyBindings[k].KeyCode)
					{
						flag = true;
						break;
					}

					k++;
				}
				while (true);

				k = j;

				if (ButtonId == j)
				{
					ControlList[k].DisplayString = new StringBuilder().Append(FontRenderer.SpecialChar).Append("f> ").Append(FontRenderer.SpecialChar).Append("e??? ").Append(FontRenderer.SpecialChar).Append("f<").ToString();
				}
				else if (flag)
				{
                    ControlList[k].DisplayString = new StringBuilder().Append(FontRenderer.SpecialChar).Append("c").Append(Options.GetOptionDisplayString(k)).ToString();
				}
				else
				{
					ControlList[k].DisplayString = Options.GetOptionDisplayString(k);
				}

				DrawString(FontRenderer, Options.GetKeyBindingDescription(j), i + (j % 2) * 160 + 70 + 6, Height / 6 + 24 * (j >> 1) + 7, -1);
			}

			base.DrawScreen(par1, par2, par3);
		}
	}
}