using System;
using System.Threading;
using Microsoft.Xna.Framework;

namespace net.minecraft.src
{
	public class LoadingScreenRenderer : IProgressUpdate
	{
		private string Field_1004_a;

		/// <summary>
		/// A reference to the Minecraft object. </summary>
		private Minecraft Mc;

		/// <summary>
		/// The text currently displayed (i.e. the argument to the last call to printText or Func_597_c)
		/// </summary>
		private string CurrentlyDisplayedText;
		private long Field_1006_d;
		private bool Field_1005_e;

		public LoadingScreenRenderer(Minecraft par1Minecraft)
		{
			Field_1004_a = "";
			CurrentlyDisplayedText = "";
			Field_1006_d = JavaHelper.CurrentTimeMillis();
			Field_1005_e = false;
			Mc = par1Minecraft;
		}

		public virtual void PrintText(string par1Str)
		{
			Field_1005_e = false;
			Func_597_c(par1Str);
		}

		/// <summary>
		/// Shows the 'Saving level' string.
		/// </summary>
		public virtual void DisplaySavingString(string par1Str)
		{
			Field_1005_e = true;
			Func_597_c(CurrentlyDisplayedText);
		}

		public virtual void Func_597_c(string par1Str)
		{
			if (!Mc.Running)
			{
				if (Field_1005_e)
				{
					return;
				}
				else
				{
					throw new MinecraftError();
				}
			}
			else
			{
				CurrentlyDisplayedText = par1Str;
				ScaledResolution scaledresolution = new ScaledResolution(Mc.GameSettings, Mc.DisplayWidth, Mc.DisplayHeight);
				//GL.Clear(ClearBufferMask.ColorBufferBit);
				//GL.MatrixMode(MatrixMode.Projection);
				//GL.LoadIdentity();
				//GL.Ortho(0.0F, scaledresolution.ScaledWidthD, scaledresolution.ScaledHeightD, 0.0F, 100D, 300D);
				//GL.MatrixMode(MatrixMode.Modelview);
				//GL.LoadIdentity();
				//GL.Translate(0.0F, 0.0F, -200F);
				return;
			}
		}

		/// <summary>
		/// Displays a string on the loading screen supposed to indicate what is being done currently.
		/// </summary>
		public virtual void DisplayLoadingString(string par1Str)
		{
			if (!Mc.Running)
			{
				if (Field_1005_e)
				{
					return;
				}
				else
				{
					throw new MinecraftError();
				}
			}
			else
			{
				Field_1006_d = 0L;
				Field_1004_a = par1Str;
				SetLoadingProgress(-1);
				Field_1006_d = 0L;
				return;
			}
		}

		/// <summary>
		/// Updates the progress bar on the loading screen to the specified amount. Args: loadProgress
		/// </summary>
		public virtual void SetLoadingProgress(int par1)
		{
			if (!Mc.Running)
			{
				if (Field_1005_e)
				{
					return;
				}
				else
				{
					throw new MinecraftError();
				}
			}

			long l = JavaHelper.CurrentTimeMillis();

			if (l - Field_1006_d < 100L)
			{
				return;
			}

			Field_1006_d = l;
			ScaledResolution scaledresolution = new ScaledResolution(Mc.GameSettings, Mc.DisplayWidth, Mc.DisplayHeight);
			int i = scaledresolution.GetScaledWidth();
			int j = scaledresolution.GetScaledHeight();/*
			GL.Clear(ClearBufferMask.ColorBufferBit);
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			GL.Ortho(0.0F, scaledresolution.ScaledWidthD, scaledresolution.ScaledHeightD, 0.0F, 100D, 300D);
			GL.MatrixMode(MatrixMode.Modelview);
			GL.LoadIdentity();
			GL.Translate(0.0F, 0.0F, -200F);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			Tessellator tessellator = Tessellator.Instance;
			int k = Mc.RenderEngineOld.GetTexture("/gui/background.png");*/
            //GL.BindTexture(TextureTarget.Texture2D, k);
            //Mc.RenderEngine.BindTexture("gui.background.png");
			float f = 32F;/*
			tessellator.StartDrawingQuads();
			tessellator.SetColorOpaque_I(0x404040);
			tessellator.AddVertexWithUV(0.0F, j, 0.0F, 0.0F, (float)j / f);
			tessellator.AddVertexWithUV(i, j, 0.0F, (float)i / f, (float)j / f);
			tessellator.AddVertexWithUV(i, 0.0F, 0.0F, (float)i / f, 0.0F);
			tessellator.AddVertexWithUV(0.0F, 0.0F, 0.0F, 0.0F, 0.0F);
			tessellator.Draw();
            */
            //Mc.RenderEngine.RenderSprite(new Rectangle(0, 0, i, j), new Rectangle(0, 0, i, j));

			if (par1 >= 0)
			{
				sbyte byte0 = 100;
				sbyte byte1 = 2;
				int i1 = i / 2 - byte0 / 2;
				int j1 = j / 2 + 16;/*
				//GL.Disable(EnableCap.Texture2D);
				tessellator.StartDrawingQuads();
				tessellator.SetColorOpaque_I(0x808080);
				tessellator.AddVertex(i1, j1, 0.0F);
				tessellator.AddVertex(i1, j1 + byte1, 0.0F);
				tessellator.AddVertex(i1 + byte0, j1 + byte1, 0.0F);
				tessellator.AddVertex(i1 + byte0, j1, 0.0F);
				tessellator.SetColorOpaque_I(0x80ff80);
				tessellator.AddVertex(i1, j1, 0.0F);
				tessellator.AddVertex(i1, j1 + byte1, 0.0F);
				tessellator.AddVertex(i1 + par1, j1 + byte1, 0.0F);
				tessellator.AddVertex(i1 + par1, j1, 0.0F);
				tessellator.Draw();*/
				//GL.Enable(EnableCap.Texture2D);
			}
            /*
			Mc.FontRenderer.DrawStringWithShadow(CurrentlyDisplayedText, (i - Mc.FontRenderer.GetStringWidth(CurrentlyDisplayedText)) / 2, j / 2 - 4 - 16, 0xffffff);
			Mc.FontRenderer.DrawStringWithShadow(Field_1004_a, (i - Mc.FontRenderer.GetStringWidth(Field_1004_a)) / 2, (j / 2 - 4) + 8, 0xffffff);*/
			//Display.update();

            try
            {
                Thread.Yield();
            }
            catch (Exception exception)
            {
                Utilities.LogException(exception);
            }
		}
	}
}