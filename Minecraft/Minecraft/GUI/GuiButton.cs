using Microsoft.Xna.Framework;

namespace net.minecraft.src
{
	public class GuiButton : Gui
	{
		protected int Width;
		protected int Height;

		/// <summary>
		/// The x position of this control. </summary>
		public int XPosition;

		/// <summary>
		/// The y position of this control. </summary>
		public int YPosition;

		/// <summary>
		/// The string displayed on this control. </summary>
		public string DisplayString;

		/// <summary>
		/// ID for this control. </summary>
		public int Id;

		/// <summary>
		/// True if this control is enabled, false to disable. </summary>
		public bool Enabled;

		/// <summary>
		/// Hides the button completely if false. </summary>
		public bool ShowButton;

		public GuiButton(int par1, int par2, int par3, string par4Str)
            : this(par1, par2, par3, 200, 20, par4Str)
		{
		}

		public GuiButton(int par1, int par2, int par3, int par4, int par5, string par6Str)
		{
			Width = 200;
			Height = 20;
			Enabled = true;
			ShowButton = true;
			Id = par1;
			XPosition = par2;
			YPosition = par3;
			Width = par4;
			Height = par5;
			DisplayString = par6Str;
		}

		/// <summary>
		/// Returns 0 if the button is disabled, 1 if the mouse is NOT hovering over this button and 2 if it IS hovering over
		/// this button.
		/// </summary>
		protected virtual int GetHoverState(bool par1)
		{
			byte byte0 = 1;

			if (!Enabled)
			{
				byte0 = 0;
			}
			else if (par1)
			{
				byte0 = 2;
			}

			return byte0;
		}

		/// <summary>
		/// Draws this button to the screen.
		/// </summary>
		public virtual void DrawButton(Minecraft par1Minecraft, int mouseX, int mouseY)
		{
			if (!ShowButton)
			{
				return;
			}

			FontRenderer fontrenderer = par1Minecraft.FontRenderer;
			//GL.BindTexture(TextureTarget.Texture2D, par1Minecraft.RenderEngineOld.GetTexture("/gui/gui.png"));
            par1Minecraft.RenderEngine.BindTexture("gui.gui.png");
			//GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);

			bool flag = 
                mouseX >= XPosition && mouseX < XPosition + Width && 
                mouseY >= YPosition && mouseY < YPosition + Height;

			int i = GetHoverState(flag);

			DrawTexturedModalRect(XPosition, YPosition, 0, 46 + i * 20, Width / 2, Height);
			DrawTexturedModalRect(XPosition + Width / 2, YPosition, 200 - Width / 2, 46 + i * 20, Width / 2, Height);

			MouseDragged(par1Minecraft, mouseX, mouseY);

			int j = 0xe0e0e0;

			if (!Enabled)
			{
				j = 0xffa0a0a;
			}
			else if (flag)
			{
				j = 0xffffa0;
			}

			DrawCenteredString(fontrenderer, DisplayString, XPosition + Width / 2, YPosition + (Height - 8) / 2, j);
		}

		/// <summary>
		/// Fired when the mouse button is dragged. Equivalent of MouseListener.mouseDragged(MouseEvent e).
		/// </summary>
		protected virtual void MouseDragged(Minecraft minecraft, int i, int j)
		{
		}

		/// <summary>
		/// Fired when the mouse button is released. Equivalent of MouseListener.mouseReleased(MouseEvent e).
		/// </summary>
		public virtual void MouseReleased(int i, int j)
		{
		}

		/// <summary>
		/// Returns true if the mouse has been pressed on this control. Equivalent of MouseListener.mousePressed(MouseEvent
		/// e).
		/// </summary>
		public virtual bool MousePressed(Minecraft par1Minecraft, int mouseX, int mouseY)
		{
			return Enabled && ShowButton && 
                mouseX >= XPosition && mouseX < XPosition + Width && 
                mouseY >= YPosition && mouseY < YPosition + Height;
		}
	}
}