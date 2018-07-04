using System.Text;

namespace net.minecraft.src
{
	public class GuiOptions : GuiScreen
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
		private GameSettings Settings;
		private static Options[] RelevantOptions;

		public GuiOptions(GuiScreen par1GuiScreen, GameSettings par2GameSettings)
		{
			ScreenTitle = "Options";
			ParentScreen = par1GuiScreen;
			Settings = par2GameSettings;
		}

		/// <summary>
		/// Adds the buttons (and other controls) to the screen in question.
		/// </summary>
		public override void InitGui()
		{
			StringTranslate stringtranslate = StringTranslate.GetInstance();
			ScreenTitle = stringtranslate.TranslateKey("options.title");
			int i = 0;
            Options[] aenumoptions = RelevantOptions;
			int j = aenumoptions.Length;

			for (int k = 0; k < j; k++)
			{
                Options enumoptions = aenumoptions[k];
                
				if (!enumoptions.Float)
				{
					GuiSmallButton guismallbutton = new GuiSmallButton(enumoptions.Ordinal(), (Width / 2 - 155) + (i % 2) * 160, Height / 6 + 24 * (i >> 1), enumoptions, Settings.GetKeyBinding(enumoptions));

                    if (enumoptions == Options.DIFFICULTY && Mc.TheWorld != null && Mc.TheWorld.GetWorldInfo().IsHardcoreModeEnabled())
					{
						guismallbutton.Enabled = false;
						guismallbutton.DisplayString = (new StringBuilder()).Append(StatCollector.TranslateToLocal("options.difficulty")).Append(": ").Append(StatCollector.TranslateToLocal("options.difficulty.hardcore")).ToString();
					}

					ControlList.Add(guismallbutton);
				}
				else
				{
					ControlList.Add(new GuiSlider(enumoptions.Ordinal(), (Width / 2 - 155) + (i % 2) * 160, Height / 6 + 24 * (i >> 1), enumoptions, Settings.GetKeyBinding(enumoptions), Settings.GetOptionFloatValue(enumoptions)));
				}
                
				i++;
			}

			ControlList.Add(new GuiButton(101, Width / 2 - 100, (Height / 6 + 96) - 6, stringtranslate.TranslateKey("options.video")));
			ControlList.Add(new GuiButton(100, Width / 2 - 100, (Height / 6 + 120) - 6, stringtranslate.TranslateKey("options.controls")));
			ControlList.Add(new GuiButton(102, Width / 2 - 100, (Height / 6 + 144) - 6, stringtranslate.TranslateKey("options.language")));
			ControlList.Add(new GuiButton(200, Width / 2 - 100, Height / 6 + 168, stringtranslate.TranslateKey("gui.done")));
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

			if (par1GuiButton.Id < 100 && (par1GuiButton is GuiSmallButton))
			{
				Settings.SetOptionValue(((GuiSmallButton)par1GuiButton).ReturnOptions(), 1);
                par1GuiButton.DisplayString = Settings.GetKeyBinding(Options.GetOptions(par1GuiButton.Id));
			}

			if (par1GuiButton.Id == 101)
			{
				Mc.GameSettings.SaveOptions();
				Mc.DisplayGuiScreen(new GuiVideoSettings(this, Settings));
			}

			if (par1GuiButton.Id == 100)
			{
				Mc.GameSettings.SaveOptions();
				Mc.DisplayGuiScreen(new GuiControls(this, Settings));
			}

			if (par1GuiButton.Id == 102)
			{
				Mc.GameSettings.SaveOptions();
				Mc.DisplayGuiScreen(new GuiLanguage(this, Settings));
			}

			if (par1GuiButton.Id == 200)
			{
				Mc.GameSettings.SaveOptions();
				Mc.DisplayGuiScreen(ParentScreen);
			}
		}

		/// <summary>
		/// Draws the screen and all the components in it.
		/// </summary>
		public override void DrawScreen(int par1, int par2, float par3)
		{
			DrawDefaultBackground();
			DrawCenteredString(FontRenderer, ScreenTitle, Width / 2, 20, 0xffffff);
			base.DrawScreen(par1, par2, par3);
		}

		static GuiOptions()
		{
            RelevantOptions = (new Options[] { Options.MUSIC, Options.SOUND, Options.INVERT_MOUSE, Options.SENSITIVITY, Options.FOV, Options.DIFFICULTY });
		}
	}
}