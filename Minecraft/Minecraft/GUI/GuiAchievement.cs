using Microsoft.Xna.Framework;

namespace net.minecraft.src
{
	public class GuiAchievement : Gui
	{
		/// <summary>
		/// Holds the instance of the game (Minecraft) </summary>
		private Minecraft TheGame;

		/// <summary>
		/// Holds the latest width scaled to fit the game window. </summary>
		private int AchievementWindowWidth;

		/// <summary>
		/// Holds the latest height scaled to fit the game window. </summary>
		private int AchievementWindowHeight;
		private string AchievementGetLocalText;
		private string AchievementStatName;

		/// <summary>
		/// Holds the achievement that will be displayed on the GUI. </summary>
		private Achievement TheAchievement;
		private long AchievementTime;

		/// <summary>
		/// Holds a instance of RenderItem, used to draw the achievement icons on screen (is based on ItemStack)
		/// </summary>
		private RenderItem ItemRender;
		private bool HaveAchiement;

		public GuiAchievement(Minecraft par1Minecraft)
		{
			TheGame = par1Minecraft;
			ItemRender = new RenderItem();
		}

		/// <summary>
		/// Queue a taken achievement to be displayed.
		/// </summary>
		public virtual void QueueTakenAchievement(Achievement par1Achievement)
		{
			AchievementGetLocalText = StatCollector.TranslateToLocal("achievement.get");
			AchievementStatName = StatCollector.TranslateToLocal(par1Achievement.GetName());
			AchievementTime = JavaHelper.CurrentTimeMillis();
			TheAchievement = par1Achievement;
			HaveAchiement = false;
		}

		/// <summary>
		/// Queue a information about a achievement to be displayed.
		/// </summary>
		public virtual void QueueAchievementInformation(Achievement par1Achievement)
		{
			AchievementGetLocalText = StatCollector.TranslateToLocal(par1Achievement.GetName());
			AchievementStatName = par1Achievement.GetDescription();
			AchievementTime = JavaHelper.CurrentTimeMillis() - 2500L;
			TheAchievement = par1Achievement;
			HaveAchiement = true;
		}

		/// <summary>
		/// Update the display of the achievement window to match the game window.
		/// </summary>
		private void UpdateAchievementWindowScale()
		{
			//GL.Viewport(0, 0, TheGame.DisplayWidth, TheGame.DisplayHeight);
			//GL.MatrixMode(MatrixMode.Projection);
			//GL.LoadIdentity();
			//GL.MatrixMode(MatrixMode.Modelview);
			//GL.LoadIdentity();
			AchievementWindowWidth = TheGame.DisplayWidth;
			AchievementWindowHeight = TheGame.DisplayHeight;
			ScaledResolution scaledresolution = new ScaledResolution(TheGame.GameSettings, TheGame.DisplayWidth, TheGame.DisplayHeight);
			AchievementWindowWidth = scaledresolution.GetScaledWidth();
			AchievementWindowHeight = scaledresolution.GetScaledHeight();
			//GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			//GL.MatrixMode(MatrixMode.Projection);
			//GL.LoadIdentity();
			//GL.Ortho(0.0F, AchievementWindowWidth, AchievementWindowHeight, 0.0F, 1000D, 3000D);
			//GL.MatrixMode(MatrixMode.Modelview);
			//GL.LoadIdentity();
			//GL.Translate(0.0F, 0.0F, -2000F);
		}

		/// <summary>
		/// Updates the small achievement tooltip window, showing a queued achievement if is needed.
		/// </summary>
		public virtual void UpdateAchievementWindow()
		{
			if (TheAchievement == null || AchievementTime == 0L)
			{
				return;
			}

			double d = (JavaHelper.CurrentTimeMillis() - AchievementTime) / 3000D;

			if (!HaveAchiement && (d < 0.0F || d > 1.0D))
			{
				AchievementTime = 0L;
				return;
			}

			UpdateAchievementWindowScale();
			//GL.Disable(EnableCap.DepthTest);
			//GL.DepthMask(false);
			double d1 = d * 2D;

			if (d1 > 1.0D)
			{
				d1 = 2D - d1;
			}

			d1 *= 4D;
			d1 = 1.0D - d1;

			if (d1 < 0.0F)
			{
				d1 = 0.0F;
			}

			d1 *= d1;
			d1 *= d1;
			int i = AchievementWindowWidth - 160;
			int j = 0 - (int)(d1 * 36D);
			int k = TheGame.RenderEngineOld.GetTexture("Minecraft.Resources.achievement.bg.png");
			//GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
			//GL.Enable(EnableCap.Texture2D);
			//GL.BindTexture(TextureTarget.Texture2D, k);
			//GL.Disable(EnableCap.Lighting);
			DrawTexturedModalRect(i, j, 96, 202, 160, 32);

			if (HaveAchiement)
			{
				TheGame.FontRendererOld.DrawSplitString(AchievementStatName, i + 30, j + 7, 120, -1);
			}
			else
			{
				TheGame.FontRendererOld.DrawString(AchievementGetLocalText, i + 30, j + 7, -256);
				TheGame.FontRendererOld.DrawString(AchievementStatName, i + 30, j + 18, -1);
			}

			RenderHelper.EnableGUIStandardItemLighting();
			//GL.Disable(EnableCap.Lighting);
			//GL.Enable(EnableCap.RescaleNormal);
			//GL.Enable(EnableCap.ColorMaterial);
			//GL.Enable(EnableCap.Lighting);
			ItemRender.RenderItemIntoGUI(TheGame.FontRenderer, TheGame.RenderEngineOld, TheAchievement.TheItemStack, i + 8, j + 8);
			//GL.Disable(EnableCap.Lighting);
			//GL.DepthMask(true);
			//GL.Enable(EnableCap.DepthTest);
		}
	}
}