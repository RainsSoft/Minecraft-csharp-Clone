namespace net.minecraft.src
{
	using net.minecraft.src;
	using Microsoft.Xna.Framework;

	public class GuiButtonLanguage : GuiButton
	{
		public GuiButtonLanguage(int par1, int par2, int par3) : base(par1, par2, par3, 20, 20, "")
		{
		}

		/// <summary>
		/// Draws this button to the screen.
		/// </summary>
		public override void DrawButton(Minecraft par1Minecraft, int par2, int par3)
		{
			if (!ShowButton)
			{
				return;
			}

			//GL.BindTexture(TextureTarget.Texture2D, par1Minecraft.RenderEngineOld.GetTexture("/gui/gui.png"));
            RenderEngine.Instance.BindTexture("gui.gui.png");
			//GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
			bool flag = par2 >= XPosition && par3 >= YPosition && par2 < XPosition + Width && par3 < YPosition + Height;
			int i = 106;

			if (flag)
			{
				i += Height;
			}

			DrawTexturedModalRect(XPosition, YPosition, 0, i, Width, Height);
		}
	}
}