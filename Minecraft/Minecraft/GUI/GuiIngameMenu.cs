using System;

namespace net.minecraft.src
{

	using net.minecraft.src;

	public class GuiIngameMenu : GuiScreen
	{
		/// <summary>
		/// Also counts the number of updates, not certain as to why yet. </summary>
		private int UpdateCounter2;

		/// <summary>
		/// Counts the number of screen updates. </summary>
		private int UpdateCounter;

		public GuiIngameMenu()
		{
			UpdateCounter2 = 0;
			UpdateCounter = 0;
		}

		/// <summary>
		/// Adds the buttons (and other controls) to the screen in question.
		/// </summary>
		public override void InitGui()
		{
			UpdateCounter2 = 0;
			ControlList.Clear();
			sbyte byte0 = -16;
			ControlList.Add(new GuiButton(1, Width / 2 - 100, Height / 4 + 120 + byte0, StatCollector.TranslateToLocal("menu.returnToMenu")));

			if (Mc.IsMultiplayerWorld())
			{
				((GuiButton)ControlList[0]).DisplayString = StatCollector.TranslateToLocal("menu.disconnect");
			}

			ControlList.Add(new GuiButton(4, Width / 2 - 100, Height / 4 + 24 + byte0, StatCollector.TranslateToLocal("menu.returnToGame")));
			ControlList.Add(new GuiButton(0, Width / 2 - 100, Height / 4 + 96 + byte0, StatCollector.TranslateToLocal("menu.options")));
			ControlList.Add(new GuiButton(5, Width / 2 - 100, Height / 4 + 48 + byte0, 98, 20, StatCollector.TranslateToLocal("gui.achievements")));
			ControlList.Add(new GuiButton(6, Width / 2 + 2, Height / 4 + 48 + byte0, 98, 20, StatCollector.TranslateToLocal("gui.stats")));
		}

		/// <summary>
		/// Fired when a control is clicked. This is the equivalent of ActionListener.actionPerformed(ActionEvent e).
		/// </summary>
		protected override void ActionPerformed(GuiButton par1GuiButton)
		{
			switch (par1GuiButton.Id)
			{
				case 2:
				case 3:
				default:
					break;

				case 0:
					Mc.DisplayGuiScreen(new GuiOptions(this, Mc.GameSettings));
					break;

				case 1:
					Mc.StatFileWriter.ReadStat(StatList.LeaveGameStat, 1);

					if (Mc.IsMultiplayerWorld())
					{
						Mc.TheWorld.SendQuittingDisconnectingPacket();
					}

					Mc.ChangeWorld1(null);
					Mc.DisplayGuiScreen(new GuiMainMenu());
					break;

				case 4:
					Mc.DisplayGuiScreen(null);
					Mc.SetIngameFocus();
					break;

				case 5:
					Mc.DisplayGuiScreen(new GuiAchievements(Mc.StatFileWriter));
					break;

				case 6:
					Mc.DisplayGuiScreen(new GuiStats(this, Mc.StatFileWriter));
					break;
			}
		}

		/// <summary>
		/// Called from the main game loop to update the screen.
		/// </summary>
		public override void UpdateScreen()
		{
			base.UpdateScreen();
			UpdateCounter++;
		}

		/// <summary>
		/// Draws the screen and all the components in it.
		/// </summary>
		public override void DrawScreen(int par1, int par2, float par3)
		{
			DrawDefaultBackground();
			bool flag = !Mc.TheWorld.QuickSaveWorld(UpdateCounter2++);

			if (flag || UpdateCounter < 20)
			{
				float f = ((float)(UpdateCounter % 10) + par3) / 10F;
				f = MathHelper2.Sin(f * (float)Math.PI * 2.0F) * 0.2F + 0.8F;
				int i = (int)(255F * f);
				DrawString(FontRenderer, "Saving level..", 8, Height - 16, i << 16 | i << 8 | i);
			}

			DrawCenteredString(FontRenderer, "Game menu", Width / 2, 40, 0xffffff);
			base.DrawScreen(par1, par2, par3);
		}
	}
}