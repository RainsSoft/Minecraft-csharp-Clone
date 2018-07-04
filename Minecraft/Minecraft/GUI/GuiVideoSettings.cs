using System;

namespace net.minecraft.src
{
	public class GuiVideoSettings : GuiScreen
	{
		private GuiScreen ParentGuiScreen;

		/// <summary>
		/// The title string that is displayed in the top-center of the screen. </summary>
		protected string ScreenTitle;

		/// <summary>
		/// GUI game settings </summary>
		private GameSettings GuiGameSettings;

		/// <summary>
		/// True if the system is 64-bit (using a simple indexOf test on a system property)
		/// </summary>
		private bool Is64bit;
		private static Options[] VideoOptions;

		public GuiVideoSettings(GuiScreen par1GuiScreen, GameSettings par2GameSettings)
		{
			ScreenTitle = "Video Settings";
			Is64bit = false;
			ParentGuiScreen = par1GuiScreen;
			GuiGameSettings = par2GameSettings;
		}

		/// <summary>
		/// Adds the buttons (and other controls) to the screen in question.
		/// </summary>
		public override void InitGui()
		{
			StringTranslate stringtranslate = StringTranslate.GetInstance();
			ScreenTitle = stringtranslate.TranslateKey("options.videoTitle");
			int i = 0;
			object[] aobj = VideoOptions;
			int j = aobj.Length;

			for (int k = 0; k < j; k++)
			{
                Options enumoptions = (Options)aobj[k];
                
				if (!enumoptions.Float)
				{
					ControlList.Add(new GuiSmallButton(enumoptions.Ordinal(), (Width / 2 - 155) + (i % 2) * 160, Height / 6 + 24 * (i >> 1), enumoptions, GuiGameSettings.GetKeyBinding(enumoptions)));
				}
				else
				{
					ControlList.Add(new GuiSlider(enumoptions.Ordinal(), (Width / 2 - 155) + (i % 2) * 160, Height / 6 + 24 * (i >> 1), enumoptions, GuiGameSettings.GetKeyBinding(enumoptions), GuiGameSettings.GetOptionFloatValue(enumoptions)));
				}
                
				i++;
			}

			ControlList.Add(new GuiButton(200, Width / 2 - 100, Height / 6 + 168, stringtranslate.TranslateKey("gui.done")));
			Is64bit = false;
            Is64bit = Environment.Is64BitProcess;
            /*
			aobj = (new string[] { "sun.arch.data.model", "com.ibm.vm.bitmode", "os.arch" });
			string[] @as = ((string [])(aobj));
			int l = @as.Length;
			int i1 = 0;

			do
			{
				if (i1 >= l)
				{
					break;
				}

				string s = @as[i1];
				string s1 = System.getProperty(s);

				if (s1 != null && s1.IndexOf("64") >= 0)
				{
					Is64bit = true;
					break;
				}

				i1++;
			}
			while (true);*/
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

			int i = GuiGameSettings.GuiScale;

			if (par1GuiButton.Id < 100 && (par1GuiButton is GuiSmallButton))
			{
				GuiGameSettings.SetOptionValue(((GuiSmallButton)par1GuiButton).ReturnOptions(), 1);
                par1GuiButton.DisplayString = GuiGameSettings.GetKeyBinding(Options.GetOptions(par1GuiButton.Id));
			}

			if (par1GuiButton.Id == 200)
			{
				Mc.GameSettings.SaveOptions();
				Mc.DisplayGuiScreen(ParentGuiScreen);
			}

			if (GuiGameSettings.GuiScale != i)
			{
				ScaledResolution scaledresolution = new ScaledResolution(Mc.GameSettings, Mc.DisplayWidth, Mc.DisplayHeight);
				int j = scaledresolution.GetScaledWidth();
				int k = scaledresolution.GetScaledHeight();
				SetWorldAndResolution(Mc, j, k);
			}
		}

		/// <summary>
		/// Draws the screen and all the components in it.
		/// </summary>
		public override void DrawScreen(int par1, int par2, float par3)
		{
			DrawDefaultBackground();
			DrawCenteredString(FontRenderer, ScreenTitle, Width / 2, 20, 0xffffff);

			if (!Is64bit && GuiGameSettings.RenderDistance == 0)
			{
				DrawCenteredString(FontRenderer, StatCollector.TranslateToLocal("options.farWarning1"), Width / 2, Height / 6 + 144, 0xaf0000);
				DrawCenteredString(FontRenderer, StatCollector.TranslateToLocal("options.farWarning2"), Width / 2, Height / 6 + 144 + 12, 0xaf0000);
			}

			base.DrawScreen(par1, par2, par3);
		}

		static GuiVideoSettings()
		{
            VideoOptions = (new Options[] { Options.GRAPHICS, Options.RENDER_DISTANCE, Options.AMBIENT_OCCLUSION, Options.FRAMERATE_LIMIT, Options.ANAGLYPH, Options.VIEW_BOBBING, Options.GUI_SCALE, Options.ADVANCED_OPENGL, Options.GAMMA, Options.RENDER_CLOUDS, Options.PARTICLES });
		}
	}
}