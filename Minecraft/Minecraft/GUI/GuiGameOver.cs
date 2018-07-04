using System.Collections.Generic;
using System.Text;

namespace net.minecraft.src
{
	using net.minecraft.src;
	using Microsoft.Xna.Framework;

	public class GuiGameOver : GuiScreen
	{
		/// <summary>
		/// The cooldown timer for the buttons, increases every tick and enables all buttons when reaching 20.
		/// </summary>
		private int CooldownTimer;

		public GuiGameOver()
		{
		}

		/// <summary>
		/// Adds the buttons (and other controls) to the screen in question.
		/// </summary>
		public override void InitGui()
		{
			ControlList.Clear();

			if (Mc.TheWorld.GetWorldInfo().IsHardcoreModeEnabled())
			{
				ControlList.Add(new GuiButton(1, Width / 2 - 100, Height / 4 + 96, StatCollector.TranslateToLocal("deathScreen.deleteWorld")));
			}
			else
			{
				ControlList.Add(new GuiButton(1, Width / 2 - 100, Height / 4 + 72, StatCollector.TranslateToLocal("deathScreen.respawn")));
				ControlList.Add(new GuiButton(2, Width / 2 - 100, Height / 4 + 96, StatCollector.TranslateToLocal("deathScreen.titleScreen")));

				if (Mc.Session == null)
				{
					ControlList[1].Enabled = false;
				}
			}

			for (IEnumerator<GuiButton> iterator = ControlList.GetEnumerator(); iterator.MoveNext();)
			{
				GuiButton guibutton = iterator.Current;
				guibutton.Enabled = false;
			}
		}

		/// <summary>
		/// Fired when a key is typed. This is the equivalent of KeyListener.keyTyped(KeyEvent e).
		/// </summary>
		protected override void KeyTyped(char c, int i)
		{
		}

		/// <summary>
		/// Fired when a control is clicked. This is the equivalent of ActionListener.actionPerformed(ActionEvent e).
		/// </summary>
		protected override void ActionPerformed(GuiButton par1GuiButton)
		{
			switch (par1GuiButton.Id)
			{
				default:
					break;

				case 1:
					if (Mc.TheWorld.GetWorldInfo().IsHardcoreModeEnabled())
					{
						string s = Mc.TheWorld.GetSaveHandler().GetSaveDirectoryName();
						Mc.ExitToMainMenu("Deleting world");
						ISaveFormat isaveformat = Mc.GetSaveLoader();
						isaveformat.FlushCache();
						isaveformat.DeleteWorldDirectory(s);
						Mc.DisplayGuiScreen(new GuiMainMenu());
					}
					else
					{
						Mc.ThePlayer.RespawnPlayer();
						Mc.DisplayGuiScreen(null);
					}

					break;

				case 2:
					if (Mc.IsMultiplayerWorld())
					{
						Mc.TheWorld.SendQuittingDisconnectingPacket();
					}

					Mc.ChangeWorld1(null);
					Mc.DisplayGuiScreen(new GuiMainMenu());
					break;
			}
		}

		/// <summary>
		/// Draws the screen and all the components in it.
		/// </summary>
		public override void DrawScreen(int par1, int par2, float par3)
		{
			DrawGradientRect(0, 0, Width, Height, 0x6050000, 0xa080303);
			//GL.PushMatrix();
			//GL.Scale(2.0F, 2.0F, 2.0F);
			bool flag = Mc.TheWorld.GetWorldInfo().IsHardcoreModeEnabled();
			string s = flag ? StatCollector.TranslateToLocal("deathScreen.title.hardcore") : StatCollector.TranslateToLocal("deathScreen.title");
			DrawCenteredString(FontRenderer, s, Width / 2 / 2, 30, 0xffffff);
			//GL.PopMatrix();

			if (flag)
			{
				DrawCenteredString(FontRenderer, StatCollector.TranslateToLocal("deathScreen.hardcoreInfo"), Width / 2, 144, 0xffffff);
			}

			DrawCenteredString(FontRenderer, (new StringBuilder()).Append(StatCollector.TranslateToLocal("deathScreen.score")).Append(": ").Append(Mc.ThePlayer.GetScore()).ToString(), Width / 2, 100, 0xffffff);
			base.DrawScreen(par1, par2, par3);
		}

		/// <summary>
		/// Returns true if this GUI should pause the game when it is displayed in single-player
		/// </summary>
		public override bool DoesGuiPauseGame()
		{
			return false;
		}

		/// <summary>
		/// Called from the main game loop to update the screen.
		/// </summary>
		public override void UpdateScreen()
		{
			base.UpdateScreen();
			CooldownTimer++;

			if (CooldownTimer == 20)
			{
				for (IEnumerator<GuiButton> iterator = ControlList.GetEnumerator(); iterator.MoveNext();)
				{
					GuiButton guibutton = iterator.Current;
					guibutton.Enabled = true;
				}
			}
		}
	}
}