using System.Collections.Generic;
using System.Text;

namespace net.minecraft.src
{
	public class GuiSelectWorld : GuiScreen
	{
		/// <summary>
		/// A reference to the screen object that created this. Used for navigating between screens.
		/// </summary>
		protected GuiScreen ParentScreen;

		/// <summary>
		/// The title string that is displayed in the top-center of the screen. </summary>
		protected string ScreenTitle;

		/// <summary>
		/// True if a world has been selected. </summary>
		private bool Selected;

		/// <summary>
		/// the currently selected world </summary>
		private int SelectedWorld;

		/// <summary>
		/// The save list for the world selection screen </summary>
		private List<SaveFormatComparator> SaveList;
		private GuiWorldSlot WorldSlotContainer;

		/// <summary>
		/// E.g. World, Welt, Monde, Mundo </summary>
		private string LocalizedWorldText;
		private string LocalizedMustConvertText;
		private string[] LocalizedGameModeText;

		/// <summary>
		/// set to true if you arein the process of deleteing a world/save </summary>
		private bool Deleting;

		/// <summary>
		/// the rename button in the world selection gui </summary>
		private GuiButton ButtonRename;

		/// <summary>
		/// the select button in the world selection gui </summary>
		private GuiButton ButtonSelect;

		/// <summary>
		/// the delete button in the world selection gui </summary>
		private GuiButton ButtonDelete;

		public GuiSelectWorld(GuiScreen par1GuiScreen)
		{
			ScreenTitle = "Select world";
			Selected = false;
			LocalizedGameModeText = new string[2];
			ParentScreen = par1GuiScreen;
		}

		/// <summary>
		/// Adds the buttons (and other controls) to the screen in question.
		/// </summary>
		public override void InitGui()
		{
			StringTranslate stringtranslate = StringTranslate.GetInstance();
			ScreenTitle = stringtranslate.TranslateKey("selectWorld.title");
			LocalizedWorldText = stringtranslate.TranslateKey("selectWorld.world");
			LocalizedMustConvertText = stringtranslate.TranslateKey("selectWorld.conversion");
			LocalizedGameModeText[0] = stringtranslate.TranslateKey("gameMode.survival");
			LocalizedGameModeText[1] = stringtranslate.TranslateKey("gameMode.creative");
			LoadSaves();
			WorldSlotContainer = new GuiWorldSlot(this);
			WorldSlotContainer.RegisterScrollButtons(ControlList, 4, 5);
			InitButtons();
		}

		/// <summary>
		/// loads the saves
		/// </summary>
		private void LoadSaves()
		{
			ISaveFormat isaveformat = Mc.GetSaveLoader();
			SaveList = isaveformat.GetSaveList();
			SaveList.Sort();
			SelectedWorld = -1;
		}

		/// <summary>
		/// returns the file name of the specified save number
		/// </summary>
		protected virtual string GetSaveFileName(int par1)
		{
			return SaveList[par1].GetFileName();
		}

		/// <summary>
		/// returns the name of the saved game
		/// </summary>
		protected virtual string GetSaveName(int par1)
		{
			string s = SaveList[par1].GetDisplayName();

			if (s == null || MathHelper2.StringNullOrLengthZero(s))
			{
				StringTranslate stringtranslate = StringTranslate.GetInstance();
				s = (new StringBuilder()).Append(stringtranslate.TranslateKey("selectWorld.world")).Append(" ").Append(par1 + 1).ToString();
			}

			return s;
		}

		/// <summary>
		/// intilize the buttons for this GUI
		/// </summary>
		public virtual void InitButtons()
		{
			StringTranslate stringtranslate = StringTranslate.GetInstance();
			ControlList.Add(ButtonSelect = new GuiButton(1, Width / 2 - 154, Height - 52, 150, 20, stringtranslate.TranslateKey("selectWorld.select")));
			ControlList.Add(ButtonDelete = new GuiButton(6, Width / 2 - 154, Height - 28, 70, 20, stringtranslate.TranslateKey("selectWorld.rename")));
			ControlList.Add(ButtonRename = new GuiButton(2, Width / 2 - 74, Height - 28, 70, 20, stringtranslate.TranslateKey("selectWorld.delete")));
			ControlList.Add(new GuiButton(3, Width / 2 + 4, Height - 52, 150, 20, stringtranslate.TranslateKey("selectWorld.create")));
			ControlList.Add(new GuiButton(0, Width / 2 + 4, Height - 28, 150, 20, stringtranslate.TranslateKey("gui.cancel")));
			ButtonSelect.Enabled = false;
			ButtonRename.Enabled = false;
			ButtonDelete.Enabled = false;
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

			if (par1GuiButton.Id == 2)
			{
				string s = GetSaveName(SelectedWorld);

				if (s != null)
				{
					Deleting = true;
					StringTranslate stringtranslate = StringTranslate.GetInstance();
					string s1 = stringtranslate.TranslateKey("selectWorld.deleteQuestion");
					string s2 = (new StringBuilder()).Append("'").Append(s).Append("' ").Append(stringtranslate.TranslateKey("selectWorld.deleteWarning")).ToString();
					string s3 = stringtranslate.TranslateKey("selectWorld.deleteButton");
					string s4 = stringtranslate.TranslateKey("gui.cancel");
					GuiYesNo guiyesno = new GuiYesNo(this, s1, s2, s3, s4, SelectedWorld);
					Mc.DisplayGuiScreen(guiyesno);
				}
			}
			else if (par1GuiButton.Id == 1)
			{
				SelectWorld(SelectedWorld);
			}
			else if (par1GuiButton.Id == 3)
			{
				Mc.DisplayGuiScreen(new GuiCreateWorld(this));
			}
			else if (par1GuiButton.Id == 6)
			{
				Mc.DisplayGuiScreen(new GuiRenameWorld(this, GetSaveFileName(SelectedWorld)));
			}
			else if (par1GuiButton.Id == 0)
			{
				Mc.DisplayGuiScreen(ParentScreen);
			}
			else
			{
				WorldSlotContainer.ActionPerformed(par1GuiButton);
			}
		}

		/// <summary>
		/// Gets the selected world.
		/// </summary>
		public virtual void SelectWorld(int par1)
		{
			Mc.DisplayGuiScreen(null);

			if (Selected)
			{
				return;
			}

			Selected = true;
			int i = SaveList[par1].GetGameType();

			if (i == 0)
			{
				Mc.PlayerController = new PlayerControllerSP(Mc);
			}
			else
			{
				Mc.PlayerController = new PlayerControllerCreative(Mc);
			}

			string s = GetSaveFileName(par1);

			if (s == null)
			{
				s = (new StringBuilder()).Append("World").Append(par1).ToString();
			}

			Mc.StartWorld(s, GetSaveName(par1), null);
			Mc.DisplayGuiScreen(null);
		}

		public override void ConfirmClicked(bool par1, int par2)
		{
			if (Deleting)
			{
				Deleting = false;

				if (par1)
				{
					ISaveFormat isaveformat = Mc.GetSaveLoader();
					isaveformat.FlushCache();
					isaveformat.DeleteWorldDirectory(GetSaveFileName(par2));
					LoadSaves();
				}

				Mc.DisplayGuiScreen(this);
			}
		}

		/// <summary>
		/// Draws the screen and all the components in it.
		/// </summary>
		public override void DrawScreen(int par1, int par2, float par3)
		{
			WorldSlotContainer.DrawScreen(par1, par2, par3);
			DrawCenteredString(FontRenderer, ScreenTitle, Width / 2, 20, 0xffffff);
			base.DrawScreen(par1, par2, par3);
		}

		public static List<SaveFormatComparator> GetSize(GuiSelectWorld par0GuiSelectWorld)
		{
			return par0GuiSelectWorld.SaveList;
		}

		/// <summary>
		/// called whenever an element in this gui is selected
		/// </summary>
        public static int OnElementSelected(GuiSelectWorld par0GuiSelectWorld, int par1)
		{
			return par0GuiSelectWorld.SelectedWorld = par1;
		}

		/// <summary>
		/// returns the world currently selected
		/// </summary>
        public static int GetSelectedWorld(GuiSelectWorld par0GuiSelectWorld)
		{
			return par0GuiSelectWorld.SelectedWorld;
		}

		/// <summary>
		/// returns the select button
		/// </summary>
        public static GuiButton GetSelectButton(GuiSelectWorld par0GuiSelectWorld)
		{
			return par0GuiSelectWorld.ButtonSelect;
		}

		/// <summary>
		/// returns the rename button
		/// </summary>
        public static GuiButton GetRenameButton(GuiSelectWorld par0GuiSelectWorld)
		{
			return par0GuiSelectWorld.ButtonRename;
		}

		/// <summary>
		/// returns the delete button
		/// </summary>
        public static GuiButton GetDeleteButton(GuiSelectWorld par0GuiSelectWorld)
		{
			return par0GuiSelectWorld.ButtonDelete;
		}

		/// <summary>
		/// Gets the localized world name
		/// </summary>
        public static string GetLocalizedWorldName(GuiSelectWorld par0GuiSelectWorld)
		{
			return par0GuiSelectWorld.LocalizedWorldText;
		}

		/// <summary>
		/// Gets the localized must convert text
		/// </summary>
        public static string GetLocalizedMustConvert(GuiSelectWorld par0GuiSelectWorld)
		{
			return par0GuiSelectWorld.LocalizedMustConvertText;
		}

		/// <summary>
		/// Gets the localized GameMode
		/// </summary>
        public static string[] GetLocalizedGameMode(GuiSelectWorld par0GuiSelectWorld)
		{
			return par0GuiSelectWorld.LocalizedGameModeText;
		}
	}
}