using System;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace net.minecraft.src
{
	public class GuiCreateWorld : GuiScreen
	{
		private GuiScreen ParentGuiScreen;
		private GuiTextField TextboxWorldName;
		private GuiTextField TextboxSeed;
		private string FolderName;

		/// <summary>
		/// 'hardcore', 'creative' or 'survival' </summary>
		private string GameMode;
		private bool Field_35365_g;
		private bool Field_40232_h;
		private bool CreateClicked;

		/// <summary>
		/// True if the extra options (Seed box, structure toggle button, world type button, etc.) are being shown
		/// </summary>
		private bool MoreOptions;

		/// <summary>
		/// The GUIButton that you click to change game modes. </summary>
		private GuiButton GameModeButton;

		/// <summary>
		/// The GUIButton that you click to get to options like the seed when creating a world.
		/// </summary>
		private GuiButton MoreWorldOptions;

		/// <summary>
		/// The GuiButton in the 'More World Options' screen. Toggles ON/OFF </summary>
		private GuiButton GenerateStructuresButton;

		/// <summary>
		/// the GUIButton in the more world options screen. It's currently greyed out and unused in minecraft 1.0.0
		/// </summary>
		private GuiButton WorldTypeButton;

		/// <summary>
		/// The first line of text describing the currently selected game mode. </summary>
		private string GameModeDescriptionLine1;

		/// <summary>
		/// The second line of text describing the currently selected game mode. </summary>
		private string GameModeDescriptionLine2;

		/// <summary>
		/// The current textboxSeed text </summary>
		private string Seed;

		/// <summary>
		/// E.g. New World, Neue Welt, Nieuwe wereld, Neuvo Mundo </summary>
		private string LocalizedNewWorldText;
		private int Field_46030_z;

		public GuiCreateWorld(GuiScreen par1GuiScreen)
		{
			GameMode = "survival";
			Field_35365_g = true;
			Field_40232_h = false;
			Field_46030_z = 0;
			ParentGuiScreen = par1GuiScreen;
			Seed = "";
			LocalizedNewWorldText = StatCollector.TranslateToLocal("selectWorld.newWorld");
		}

		/// <summary>
		/// Called from the main game loop to update the screen.
		/// </summary>
		public override void UpdateScreen()
		{
			TextboxWorldName.UpdateCursorCounter();
			TextboxSeed.UpdateCursorCounter();
		}

		/// <summary>
		/// Adds the buttons (and other controls) to the screen in question.
		/// </summary>
		public override void InitGui()
		{
			StringTranslate stringtranslate = StringTranslate.GetInstance();
			//Keyboard.enableRepeatEvents(true);
			ControlList.Clear();
			ControlList.Add(new GuiButton(0, Width / 2 - 155, Height - 28, 150, 20, stringtranslate.TranslateKey("selectWorld.create")));
			ControlList.Add(new GuiButton(1, Width / 2 + 5, Height - 28, 150, 20, stringtranslate.TranslateKey("gui.cancel")));
			ControlList.Add(GameModeButton = new GuiButton(2, Width / 2 - 75, 100, 150, 20, stringtranslate.TranslateKey("selectWorld.gameMode")));
			ControlList.Add(MoreWorldOptions = new GuiButton(3, Width / 2 - 75, 172, 150, 20, stringtranslate.TranslateKey("selectWorld.moreWorldOptions")));
			ControlList.Add(GenerateStructuresButton = new GuiButton(4, Width / 2 - 155, 100, 150, 20, stringtranslate.TranslateKey("selectWorld.mapFeatures")));
			GenerateStructuresButton.ShowButton = false;
			ControlList.Add(WorldTypeButton = new GuiButton(5, Width / 2 + 5, 100, 150, 20, stringtranslate.TranslateKey("selectWorld.mapType")));
			WorldTypeButton.ShowButton = false;
			TextboxWorldName = new GuiTextField(FontRenderer, Width / 2 - 100, 60, 200, 20);
			TextboxWorldName.Func_50033_b(true);
			TextboxWorldName.SetText(LocalizedNewWorldText);
			TextboxSeed = new GuiTextField(FontRenderer, Width / 2 - 100, 60, 200, 20);
			TextboxSeed.SetText(Seed);
			MakeUseableName();
			Func_35363_g();
		}

		/// <summary>
		/// Makes a the name for a world save folder based on your world name, replacing specific characters for _s and
		/// Appending -s to the end until a free name is available.
		/// </summary>
		private void MakeUseableName()
		{
			FolderName = TextboxWorldName.GetText().Trim();
			char[] ac = ChatAllowedCharacters.AllowedCharactersArray;
			int i = ac.Length;

			for (int j = 0; j < i; j++)
			{
				char c = ac[j];
				FolderName = FolderName.Replace(c, '_');
			}

			if (MathHelper2.StringNullOrLengthZero(FolderName))
			{
				FolderName = "World";
			}

			FolderName = Func_25097_a(Mc.GetSaveLoader(), FolderName);
		}

		private void Func_35363_g()
		{
			StringTranslate stringtranslate;
			stringtranslate = StringTranslate.GetInstance();
			GameModeButton.DisplayString = (new StringBuilder()).Append(stringtranslate.TranslateKey("selectWorld.gameMode")).Append(" ").Append(stringtranslate.TranslateKey((new StringBuilder()).Append("selectWorld.gameMode.").Append(GameMode).ToString())).ToString();
			GameModeDescriptionLine1 = stringtranslate.TranslateKey((new StringBuilder()).Append("selectWorld.gameMode.").Append(GameMode).Append(".line1").ToString());
			GameModeDescriptionLine2 = stringtranslate.TranslateKey((new StringBuilder()).Append("selectWorld.gameMode.").Append(GameMode).Append(".line2").ToString());
			GenerateStructuresButton.DisplayString = (new StringBuilder()).Append(stringtranslate.TranslateKey("selectWorld.mapFeatures")).Append(" ").ToString();

			if (!(!Field_35365_g))
			{
				GenerateStructuresButton.DisplayString += stringtranslate.TranslateKey("options.on");
			}
			else
			{
				GenerateStructuresButton.DisplayString += stringtranslate.TranslateKey("options.off");
			}

			WorldTypeButton.DisplayString = (new StringBuilder()).Append(stringtranslate.TranslateKey("selectWorld.mapType")).Append(" ").Append(stringtranslate.TranslateKey(WorldType.WorldTypes[Field_46030_z].GetTranslateName())).ToString();
			return;
		}

		public static string Func_25097_a(ISaveFormat par0ISaveFormat, string par1Str)
		{
			for (par1Str = par1Str.Replace("[\\./\"]|COM", "_"); par0ISaveFormat.GetWorldInfo(par1Str) != null; par1Str = new StringBuilder().Append(par1Str).Append("-").ToString())
			{
			}

			return par1Str;
		}

		/// <summary>
		/// Called when the screen is unloaded. Used to disable keyboard repeat events
		/// </summary>
		public override void OnGuiClosed()
		{
			//Keyboard.enableRepeatEvents(false);
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
				Mc.DisplayGuiScreen(null);

				if (CreateClicked)
				{
					return;
				}

				CreateClicked = true;
				long l = (new Random()).Next();
				string s = TextboxSeed.GetText();

				if (!string.IsNullOrEmpty(s))
				{
					try
					{
						long l1 = Convert.ToInt64(s);

						if (l1 != 0L)
						{
							l = l1;
						}
					}
					catch (FormatException numberformatexception)
                    {
                        Utilities.LogException(numberformatexception);

						l = s.GetHashCode();
					}
				}

				int i = 0;

				if (GameMode.Equals("creative"))
				{
					i = 1;
					Mc.PlayerController = new PlayerControllerCreative(Mc);
				}
				else
				{
					Mc.PlayerController = new PlayerControllerSP(Mc);
				}

				Mc.StartWorld(FolderName, TextboxWorldName.GetText(), new WorldSettings(l, i, Field_35365_g, Field_40232_h, WorldType.WorldTypes[Field_46030_z]));
				Mc.DisplayGuiScreen(null);
			}
			else if (par1GuiButton.Id == 3)
			{
				MoreOptions = !MoreOptions;
				GameModeButton.ShowButton = !MoreOptions;
				GenerateStructuresButton.ShowButton = MoreOptions;
				WorldTypeButton.ShowButton = MoreOptions;

				if (MoreOptions)
				{
					StringTranslate stringtranslate = StringTranslate.GetInstance();
					MoreWorldOptions.DisplayString = stringtranslate.TranslateKey("gui.done");
				}
				else
				{
					StringTranslate stringtranslate1 = StringTranslate.GetInstance();
					MoreWorldOptions.DisplayString = stringtranslate1.TranslateKey("selectWorld.moreWorldOptions");
				}
			}
			else if (par1GuiButton.Id == 2)
			{
				if (GameMode.Equals("survival"))
				{
					Field_40232_h = false;
					GameMode = "hardcore";
					Field_40232_h = true;
					Func_35363_g();
				}
				else if (GameMode.Equals("hardcore"))
				{
					Field_40232_h = false;
					GameMode = "creative";
					Func_35363_g();
					Field_40232_h = false;
				}
				else
				{
					GameMode = "survival";
					Func_35363_g();
					Field_40232_h = false;
				}

				Func_35363_g();
			}
			else if (par1GuiButton.Id == 4)
			{
				Field_35365_g = !Field_35365_g;
				Func_35363_g();
			}
			else if (par1GuiButton.Id == 5)
			{
				Field_46030_z++;

				if (Field_46030_z >= WorldType.WorldTypes.Length)
				{
					Field_46030_z = 0;
				}

				do
				{
					if (WorldType.WorldTypes[Field_46030_z] != null && WorldType.WorldTypes[Field_46030_z].GetCanBeCreated())
					{
						break;
					}

					Field_46030_z++;

					if (Field_46030_z >= WorldType.WorldTypes.Length)
					{
						Field_46030_z = 0;
					}
				}
				while (true);

				Func_35363_g();
			}
		}

		/// <summary>
		/// Fired when a key is typed. This is the equivalent of KeyListener.keyTyped(KeyEvent e).
		/// </summary>
		protected override void KeyTyped(char par1, int par2)
		{
			if (TextboxWorldName.Func_50025_j() && !MoreOptions)
			{
				TextboxWorldName.Func_50037_a(par1, par2);
				LocalizedNewWorldText = TextboxWorldName.GetText();
			}
			else if (TextboxSeed.Func_50025_j() && MoreOptions)
			{
				TextboxSeed.Func_50037_a(par1, par2);
				Seed = TextboxSeed.GetText();
			}

			if (par1 == '\r')
			{
				ActionPerformed(ControlList[0]);
			}

			ControlList[0].Enabled = TextboxWorldName.GetText().Length > 0;
			MakeUseableName();
		}

		/// <summary>
		/// Called when the mouse is clicked.
		/// </summary>
		protected override void MouseClicked(int par1, int par2, int par3)
		{
			base.MouseClicked(par1, par2, par3);

			if (!MoreOptions)
			{
				TextboxWorldName.MouseClicked(par1, par2, par3);
			}
			else
			{
				TextboxSeed.MouseClicked(par1, par2, par3);
			}
		}

		/// <summary>
		/// Draws the screen and all the components in it.
		/// </summary>
		public override void DrawScreen(int par1, int par2, float par3)
		{
			StringTranslate stringtranslate = StringTranslate.GetInstance();
			DrawDefaultBackground();
			DrawCenteredString(FontRenderer, stringtranslate.TranslateKey("selectWorld.create"), Width / 2, 20, 0xffffff);

			if (!MoreOptions)
			{
				DrawString(FontRenderer, stringtranslate.TranslateKey("selectWorld.enterName"), Width / 2 - 100, 47, 0xa0a0a0);
				DrawString(FontRenderer, (new StringBuilder()).Append(stringtranslate.TranslateKey("selectWorld.resultFolder")).Append(" ").Append(FolderName).ToString(), Width / 2 - 100, 85, 0xa0a0a0);
				TextboxWorldName.DrawTextBox();
				DrawString(FontRenderer, GameModeDescriptionLine1, Width / 2 - 100, 122, 0xa0a0a0);
				DrawString(FontRenderer, GameModeDescriptionLine2, Width / 2 - 100, 134, 0xa0a0a0);
			}
			else
			{
				DrawString(FontRenderer, stringtranslate.TranslateKey("selectWorld.enterSeed"), Width / 2 - 100, 47, 0xa0a0a0);
				DrawString(FontRenderer, stringtranslate.TranslateKey("selectWorld.seedInfo"), Width / 2 - 100, 85, 0xa0a0a0);
				DrawString(FontRenderer, stringtranslate.TranslateKey("selectWorld.mapFeatures.info"), Width / 2 - 150, 122, 0xa0a0a0);
				TextboxSeed.DrawTextBox();
			}

			base.DrawScreen(par1, par2, par3);
		}
	}
}