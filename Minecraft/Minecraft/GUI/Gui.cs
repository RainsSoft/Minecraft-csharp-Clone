using Microsoft.Xna.Framework;

namespace net.minecraft.src
{
	public class Gui
	{
		protected float ZLevel;

		public Gui()
		{
			ZLevel = 0.0F;
		}

		protected virtual void DrawHorizontalLine(int par1, int par2, int par3, int par4)
		{
			if (par2 < par1)
			{
				int i = par1;
				par1 = par2;
				par2 = i;
			}

			DrawRect(par1, par3, par2 + 1, par3 + 1, par4);
		}

		protected virtual void DrawVerticalLine(int par1, int par2, int par3, int par4)
		{
			if (par3 < par2)
			{
				int i = par2;
				par2 = par3;
				par3 = i;
			}

			DrawRect(par1, par2 + 1, par1 + 1, par3, par4);
		}

		/// <summary>
		/// Draws a solid color rectangle with the specified coordinates and color.
		/// </summary>
		public static void DrawRect(int par1, int par2, int par3, int par4, int par5)
		{
			if (par1 < par3)
			{
				int i = par1;
				par1 = par3;
				par3 = i;
			}

			if (par2 < par4)
			{
				int j = par2;
				par2 = par4;
				par4 = j;
			}

			float f = (float)(par5 >> 24 & 0xff) / 255F;
			float f1 = (float)(par5 >> 16 & 0xff) / 255F;
			float f2 = (float)(par5 >> 8 & 0xff) / 255F;
			float f3 = (float)(par5 & 0xff) / 255F;
			Tessellator tessellator = Tessellator.Instance;
			//GL.Enable(EnableCap.Blend);
			//GL.Disable(EnableCap.Texture2D);
			//GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			//GL.Color4(f1, f2, f3, f);
			tessellator.StartDrawingQuads();
			tessellator.AddVertex(par1, par4, 0.0F);
			tessellator.AddVertex(par3, par4, 0.0F);
			tessellator.AddVertex(par3, par2, 0.0F);
			tessellator.AddVertex(par1, par2, 0.0F);
			tessellator.Draw();
			//GL.Enable(EnableCap.Texture2D);
			//GL.Disable(EnableCap.Blend);
		}

		/// <summary>
		/// Draws a rectangle with a vertical gradient between the specified colors.
		/// </summary>
		protected virtual void DrawGradientRect(int par1, int par2, int par3, int par4, int par5, int par6)
		{
			float f = (float)(par5 >> 24 & 0xff) / 255F;
			float f1 = (float)(par5 >> 16 & 0xff) / 255F;
			float f2 = (float)(par5 >> 8 & 0xff) / 255F;
			float f3 = (float)(par5 & 0xff) / 255F;
			float f4 = (float)(par6 >> 24 & 0xff) / 255F;
			float f5 = (float)(par6 >> 16 & 0xff) / 255F;
			float f6 = (float)(par6 >> 8 & 0xff) / 255F;
			float f7 = (float)(par6 & 0xff) / 255F;
			//GL.Disable(EnableCap.Texture2D);
			//GL.Enable(EnableCap.Blend);
			//GL.Disable(EnableCap.AlphaTest);
			//GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			//GL.ShadeModel(ShadingModel.Smooth);
			Tessellator tessellator = Tessellator.Instance;
			tessellator.StartDrawingQuads();
			tessellator.SetColorRGBA_F(f1, f2, f3, f);
			tessellator.AddVertex(par3, par2, ZLevel);
			tessellator.AddVertex(par1, par2, ZLevel);
			tessellator.SetColorRGBA_F(f5, f6, f7, f4);
			tessellator.AddVertex(par1, par4, ZLevel);
			tessellator.AddVertex(par3, par4, ZLevel);
			tessellator.Draw();
			//GL.ShadeModel(ShadingModel.Flat);
			//GL.Disable(EnableCap.Blend);
			//GL.Enable(EnableCap.AlphaTest);
			//GL.Enable(EnableCap.Texture2D);
		}

		/// <summary>
		/// Renders the specified text to the screen, center-aligned.
		/// </summary>
		public virtual void DrawCenteredString(FontRenderer par1FontRenderer, string par2Str, int par3, int par4, int par5)
		{
			par1FontRenderer.DrawStringWithShadow(par2Str, par3 - (int)(par1FontRenderer.GetStringWidth(par2Str) / 2), par4, par5);
		}

		/// <summary>
		/// Renders the specified text to the screen.
		/// </summary>
		public virtual void DrawString(FontRenderer par1FontRenderer, string par2Str, int par3, int par4, int par5)
		{
			par1FontRenderer.DrawStringWithShadow(par2Str, par3, par4, par5);
		}

		/// <summary>
		/// Draws a textured rectangle at the stored z-value. Args: x, y, u, v, width, height
		/// </summary>
		public virtual void DrawTexturedModalRect(int x, int y, int u, int v, int width, int height)
		{/*
			float f = 0.00390625F;
			float f1 = 0.00390625F;
			Tessellator tessellator = Tessellator.Instance;
			tessellator.StartDrawingQuads();
			tessellator.AddVertexWithUV(x + 0, y + height, ZLevel, u * f, (float)(v + height) * f1);
			tessellator.AddVertexWithUV(x + width, y + height, ZLevel, (float)(u + width) * f, (float)(v + height) * f1);
			tessellator.AddVertexWithUV(x + width, y + 0, ZLevel, (u + width) * f, v * f1);
			tessellator.AddVertexWithUV(x + 0, y + 0, ZLevel, u * f, v * f1);
			tessellator.Draw();*/

            RenderEngine.Instance.RenderSprite(new Rectangle(x, y, width, height), new Rectangle(u, v, width, height), 0.1f);        
		}
	}
}