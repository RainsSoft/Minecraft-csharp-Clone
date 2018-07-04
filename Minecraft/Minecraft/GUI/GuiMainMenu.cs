using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace net.minecraft.src
{
	public class GuiMainMenu : GuiScreen
	{
		/// <summary>
		/// The RNG used by the Main Menu Screen. </summary>
		private static readonly Random Rand = new Random();

		/// <summary>
		/// Counts the number of screen updates. </summary>
		private float UpdateCounter;

		/// <summary>
		/// The splash message. </summary>
		private string SplashText;
		private GuiButton MultiplayerButton;

		/// <summary>
		/// Timer used to rotate the panorama, increases every tick. </summary>
		private int PanoramaTimer;

		/// <summary>
		/// Texture allocated for the current viewport of the main menu's panorama background.
		/// </summary>
		private string ViewportTexture;

		public GuiMainMenu()
		{
			UpdateCounter = 0.0F;
			PanoramaTimer = 0;
			SplashText = "missingno";

			try
			{
				List<string> arraylist = new List<string>();
				StreamReader bufferedreader = new StreamReader(Minecraft.GetResourceStream("title.splashes.txt"), Encoding.UTF8);

				do
				{
					string s1;

					if ((s1 = bufferedreader.ReadLine()) == null)
					{
						break;
					}

					s1 = s1.Trim();

					if (s1.Length > 0)
					{
						arraylist.Add(s1);
					}
				}
				while (true);

				do
				{
					SplashText = arraylist[Rand.Next(arraylist.Count)];
				}
				while (SplashText.GetHashCode() == 0x77f432f);
			}
			catch (Exception exception)
			{
                Utilities.LogException(exception);
			}

			UpdateCounter = Rand.NextFloat();
		}

		/// <summary>
		/// Called from the main game loop to update the screen.
		/// </summary>
		public override void UpdateScreen()
		{
			PanoramaTimer++;
		}

		/// <summary>
		/// Returns true if this GUI should pause the game when it is displayed in single-player
		/// </summary>
		public override bool DoesGuiPauseGame()
		{
			return false;
		}

		/// <summary>
		/// Fired when a key is typed. This is the equivalent of KeyListener.keyTyped(KeyEvent e).
		/// </summary>
		protected override void KeyTyped(char c, int i)
		{
		}

		/// <summary>
		/// Adds the buttons (and other controls) to the screen in question.
		/// </summary>
		public override void InitGui()
		{
			ViewportTexture = Mc.RenderEngine.AllocateTexture(Mc.RenderEngine.GenerateNewTexture(256, 256));
			DateTime calendar = new DateTime();
			calendar = DateTime.Now;

			if (calendar.Month + 1 == 11 && calendar.Day == 9)
			{
				SplashText = "Happy birthday, ez!";
			}
			else if (calendar.Month + 1 == 6 && calendar.Day == 1)
			{
				SplashText = "Happy birthday, Notch!";
			}
			else if (calendar.Month + 1 == 12 && calendar.Day == 24)
			{
				SplashText = "Merry X-mas!";
			}
			else if (calendar.Month + 1 == 1 && calendar.Day == 1)
			{
				SplashText = "Happy new year!";
			}

			StringTranslate stringtranslate = StringTranslate.GetInstance();
			int i = Height / 4 + 48;
			ControlList.Add(new GuiButton(1, Width / 2 - 100, i, stringtranslate.TranslateKey("menu.singleplayer")));
			ControlList.Add(MultiplayerButton = new GuiButton(2, Width / 2 - 100, i + 24, stringtranslate.TranslateKey("menu.multiplayer")));
			ControlList.Add(new GuiButton(3, Width / 2 - 100, i + 48, stringtranslate.TranslateKey("menu.mods")));

			if (Mc.HideQuitButton)
			{
				ControlList.Add(new GuiButton(0, Width / 2 - 100, i + 72, stringtranslate.TranslateKey("menu.options")));
			}
			else
			{
				ControlList.Add(new GuiButton(0, Width / 2 - 100, i + 72 + 12, 98, 20, stringtranslate.TranslateKey("menu.options")));
				ControlList.Add(new GuiButton(4, Width / 2 + 2, i + 72 + 12, 98, 20, stringtranslate.TranslateKey("menu.quit")));
			}

			ControlList.Add(new GuiButtonLanguage(5, Width / 2 - 124, i + 72 + 12));

			if (Mc.Session == null)
			{
				MultiplayerButton.Enabled = false;
			}
		}

		/// <summary>
		/// Fired when a control is clicked. This is the equivalent of ActionListener.actionPerformed(ActionEvent e).
		/// </summary>
		protected override void ActionPerformed(GuiButton par1GuiButton)
		{
			if (par1GuiButton.Id == 0)
			{
				Mc.DisplayGuiScreen(new GuiOptions(this, Mc.GameSettings));
			}

			if (par1GuiButton.Id == 5)
			{
				Mc.DisplayGuiScreen(new GuiLanguage(this, Mc.GameSettings));
			}

			if (par1GuiButton.Id == 1)
			{
				Mc.DisplayGuiScreen(new GuiSelectWorld(this));
			}

			if (par1GuiButton.Id == 2)
			{
				Mc.DisplayGuiScreen(new GuiMultiplayer(this));
			}

			if (par1GuiButton.Id == 3)
			{
				Mc.DisplayGuiScreen(new GuiTexturePacks(this));
			}

			if (par1GuiButton.Id == 4)
			{
				Mc.Shutdown();
			}
		}

		/// <summary>
		/// Draws the main menu panorama
		/// </summary>
		private void DrawPanorama(int par1, int par2, float par3)
		{
			Tessellator tessellator = Tessellator.Instance;
			//GL.MatrixMode(MatrixMode.Projection);
			//GL.PushMatrix();
			//GL.LoadIdentity();
            var projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(120F), 1.0F, 0.05F, 10F);
			//GL.MatrixMode(MatrixMode.Modelview);
			//GL.PushMatrix();
			//GL.LoadIdentity();
            var view = Matrix.Identity;
			//GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
			//GL.Rotate(180F, 1.0F, 0.0F, 0.0F);
            view *= Matrix.CreateFromAxisAngle(new Vector3(1, 0, 0), MathHelper.ToRadians(180));
			//GL.Enable(EnableCap.Blend);
			//GL.Disable(EnableCap.AlphaTest);
			//GL.Disable(EnableCap.CullFace);
			//GL.DepthMask(false);
			//GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			int i = 8;

			for (int j = 0; j < i * i; j++)
			{
				//GL.PushMatrix();
				float f = ((float)(j % i) / (float)i - 0.5F) / 64F;
				float f1 = ((float)(j / i) / (float)i - 0.5F) / 64F;
				float f2 = 0.0F;
				//GL.Translate(f, f1, f2);
				//GL.Rotate(MathHelper.Sin(((float)PanoramaTimer + par3) / 400F) * 25F + 20F, 1.0F, 0.0F, 0.0F);
                view *= Matrix.CreateFromAxisAngle(new Vector3(1, 0, 0), (float)Math.Sin(((float)PanoramaTimer + par3) / 400F) * 25 + 20);
				//GL.Rotate(-((float)PanoramaTimer + par3) * 0.1F, 0.0F, 1.0F, 0.0F);
                view *= Matrix.CreateFromAxisAngle(new Vector3(0, 1, 0), -((float)PanoramaTimer + par3) * 0.1F);

				for (int k = 0; k < 6; k++)
				{
					//GL.PushMatrix();

					if (k == 1)
					{
						//GL.Rotate(90F, 0.0F, 1.0F, 0.0F);
                        view *= Matrix.CreateRotationY(MathHelper.PiOver2);
					}

					if (k == 2)
					{
                        //GL.Rotate(180F, 0.0F, 1.0F, 0.0F);
                        view *= Matrix.CreateRotationY(MathHelper.Pi);
					}

					if (k == 3)
					{
                        //GL.Rotate(-90F, 0.0F, 1.0F, 0.0F);
                        view *= Matrix.CreateRotationY(-MathHelper.PiOver2);
					}

					if (k == 4)
					{
                        //GL.Rotate(90F, 1.0F, 0.0F, 0.0F);
                        view *= Matrix.CreateRotationX(MathHelper.PiOver2);
					}

					if (k == 5)
					{
                        //GL.Rotate(-90F, 1.0F, 0.0F, 0.0F);
                        view *= Matrix.CreateRotationX(-MathHelper.PiOver2);
					}

                    RenderEngine.Instance.SetProjection(Matrix.Identity);
                    //RenderEngine.Instance.SetView(view);
                    RenderEngine.Instance.SetView(Matrix.Identity);//Matrix.CreateLookAt(Vector3.Zero, Vector3.Forward, Vector3.UnitX));

					//GL.BindTexture(TextureTarget.Texture2D, Mc.RenderEngineOld.GetTexture((new StringBuilder()).Append("title.bg.panorama").Append(k).Append(".png").ToString()));
                    RenderEngine.Instance.BindTexture(new StringBuilder().Append("title.bg.panorama").Append(k).Append(".png").ToString());/*
					tessellator.StartDrawingQuads();
					tessellator.SetColorRGBA_I(0xffffff, 255 / (j + 1));
					float f3 = 0.0F;
					tessellator.AddVertexWithUV(-1D, -1D, 1.0D, 0.0F + f3, 0.0F + f3);
					tessellator.AddVertexWithUV(1.0D, -1D, 1.0D, 1.0F - f3, 0.0F + f3);
					tessellator.AddVertexWithUV(1.0D, 1.0D, 1.0D, 1.0F - f3, 1.0F - f3);
					tessellator.AddVertexWithUV(-1D, 1.0D, 1.0D, 0.0F + f3, 1.0F - f3);
					tessellator.Draw();*/
					//GL.PopMatrix();
                    
                    VertexPositionTexture[] verts = new VertexPositionTexture[]
                    {
                        new VertexPositionTexture(new Vector3(-1, -1, 1), new Vector2(0, 0)),
                        new VertexPositionTexture(new Vector3( 1, -1, 1), new Vector2(1 - 0, 0)),
                        new VertexPositionTexture(new Vector3( 1,  1, 1), new Vector2(1 - 0, 1 - 0)),
                        new VertexPositionTexture(new Vector3(-1,  1, 1), new Vector2(0, 1 - 0)),
                        
                        new VertexPositionTexture(new Vector3(-1,  1, 1), new Vector2(0, 0)),
                        new VertexPositionTexture(new Vector3( 1, -1, 1), new Vector2(1, 0)),
                        new VertexPositionTexture(new Vector3( 1, -1, 1), new Vector2(1, 1)),
                        new VertexPositionTexture(new Vector3(-1,  1, 1), new Vector2(0, 1)),
                    };

                    int[] indices = new int[]
                    {
                        0, 1, 3, 3, 1, 2
                    };
                    
                    //RenderEngine.Instance.DrawIndexed(verts, indices);
				}

				//GL.PopMatrix();
				//GL.ColorMask(true, true, true, false);
			}
            /*
			tessellator.SetTranslation(0.0F, 0.0F, 0.0F);
			GL.ColorMask(true, true, true, true);
			GL.MatrixMode(MatrixMode.Projection);
            GL.PopMatrix();
			GL.MatrixMode(MatrixMode.Modelview);
			GL.PopMatrix();
			GL.DepthMask(true);
			GL.Enable(EnableCap.CullFace);
			GL.Enable(EnableCap.AlphaTest);
			GL.Enable(EnableCap.DepthTest);*/
        }

		/// <summary>
		/// Rotate and blurs the skybox view in the main menu
		/// </summary>
		private void RotateAndBlurSkybox(float par1)
		{
			//GL.BindTexture(TextureTarget.Texture2D, ViewportTexture);
            RenderEngine.Instance.BindTexture(ViewportTexture);
			//GL.CopyTexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, 0, 0, 256, 256);
			//GL.Enable(EnableCap.Blend);
			//GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			//GL.ColorMask(true, true, true, false);
			Tessellator tessellator = Tessellator.Instance;
			tessellator.StartDrawingQuads();
			byte byte0 = 3;

			for (int i = 0; i < byte0; i++)
			{
				tessellator.SetColorRGBA_F(1.0F, 1.0F, 1.0F, 1.0F / (float)(i + 1));
				int j = Width;
				int k = Height;
				float f = (float)(i - byte0 / 2) / 256F;
				tessellator.AddVertexWithUV(j, k, ZLevel, 0.0F + f, 0.0F);
				tessellator.AddVertexWithUV(j, 0.0F, ZLevel, 1.0F + f, 0.0F);
				tessellator.AddVertexWithUV(0.0F, 0.0F, ZLevel, 1.0F + f, 1.0D);
				tessellator.AddVertexWithUV(0.0F, k, ZLevel, 0.0F + f, 1.0D);
			}

			tessellator.Draw();
			//GL.ColorMask(true, true, true, true);
		}

		/// <summary>
		/// Renders the skybox in the main menu
		/// </summary>
		private void RenderSkybox(int par1, int par2, float par3)
		{/*
			GL.Viewport(0, 0, 256, 256);
			DrawPanorama(par1, par2, par3);
			GL.Disable(EnableCap.Texture2D);
			GL.Enable(EnableCap.Texture2D);
			RotateAndBlurSkybox(par3);
			RotateAndBlurSkybox(par3);
			RotateAndBlurSkybox(par3);
			RotateAndBlurSkybox(par3);
			RotateAndBlurSkybox(par3);
			RotateAndBlurSkybox(par3);
			RotateAndBlurSkybox(par3);
			RotateAndBlurSkybox(par3);
             */
			//GL.Viewport(0, 0, Mc.DisplayWidth, Mc.DisplayHeight);
			/*Tessellator tessellator = Tessellator.Instance;
			tessellator.StartDrawingQuads();
			float f = Width <= Height ? 120F / (float)Height : 120F / (float)Width;
			float f1 = ((float)Height * f) / 256F;
			float f2 = ((float)Width * f) / 256F;
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
			tessellator.SetColorRGBA_F(1.0F, 1.0F, 1.0F, 1.0F);
             * 
			int i = Width;
			int j = Height;
			tessellator.AddVertexWithUV(0.0F, j, ZLevel, 0.5F - f1, 0.5F + f2);
			tessellator.AddVertexWithUV(i, j, ZLevel, 0.5F - f1, 0.5F - f2);
			tessellator.AddVertexWithUV(i, 0.0F, ZLevel, 0.5F + f1, 0.5F - f2);
			tessellator.AddVertexWithUV(0.0F, 0.0F, ZLevel, 0.5F + f1, 0.5F + f2);
			tessellator.Draw();*/
		}

		/// <summary>
		/// Draws the screen and all the components in it.
		/// </summary>
		public override void DrawScreen(int par1, int par2, float par3)
		{
			RenderSkybox(par1, par2, par3);
			Tessellator tessellator = Tessellator.Instance;
			int c = 274;
			int i = Width / 2 - c / 2;
			sbyte byte0 = 30;
			DrawGradientRect(0, 0, Width, Height, 0x80fffff, 0xffffff);
			DrawGradientRect(0, 0, Width, Height, 0, 0x8000000);
			//GL.BindTexture(TextureTarget.Texture2D, Mc.RenderEngineOld.GetTexture("title.mclogo.png"));
            RenderEngine.Instance.BindTexture("title.mclogo.png");
			//GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
            
			if ((double)UpdateCounter < 0.0001D)
			{
				DrawTexturedModalRect(i + 0, byte0 + 0, 0, 0, 99, 44);
				DrawTexturedModalRect(i + 99, byte0 + 0, 129, 0, 27, 44);
				DrawTexturedModalRect(i + 99 + 26, byte0 + 0, 126, 0, 3, 44);
				DrawTexturedModalRect(i + 99 + 26 + 3, byte0 + 0, 99, 0, 26, 44);
				DrawTexturedModalRect(i + 155, byte0 + 0, 0, 45, 155, 44);
			}
			else
			{
				DrawTexturedModalRect(i + 0, byte0 + 0, 0, 0, 155, 44);
				DrawTexturedModalRect(i + 155, byte0 + 0, 0, 45, 155, 44);
			}
            /*
			tessellator.SetColorOpaque_I(0xffffff);
			GL.PushMatrix();
			GL.Translate(Width / 2 + 90, 70F, 0.0F);
			GL.Rotate(-20F, 0.0F, 0.0F, 1.0F);*/
			float f = 1.8F - MathHelper2.Abs(MathHelper2.Sin(((float)(JavaHelper.CurrentTimeMillis() % 1000L) / 1000F) * (float)Math.PI * 2.0F) * 0.1F);
			f = (f * 100F) / (float)(FontRenderer.GetStringWidth(SplashText) + 32);
			//GL.Scale(f, f, f);
			DrawCenteredString(FontRenderer, SplashText, 300, 80, 0xffff00);
			//GL.PopMatrix();
			DrawString(FontRenderer, Minecraft.GetMinecraftTitle(), 2, Height - FontRenderer.FontHeight, 0xffffff);
			string s = "Copyright Mojang AB. Do not distribute!";
            DrawString(FontRenderer, s, Width - (int)FontRenderer.GetStringWidth(s) - 2, Height - FontRenderer.FontHeight, 0xffffff);
			base.DrawScreen(par1, par2, par3);
		}
	}
}