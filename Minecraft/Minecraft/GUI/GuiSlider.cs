using Microsoft.Xna.Framework;
namespace net.minecraft.src
{
	public class GuiSlider : GuiButton
	{
		/// <summary>
		/// The value of this slider control. </summary>
		public float SliderValue;

		/// <summary>
		/// Is this slider control being dragged. </summary>
		public bool Dragging;

		/// <summary>
		/// Additional ID for this slider control. </summary>
        private Options IdFloat;

        public GuiSlider(int par1, int par2, int par3, Options par4Options, string par5Str, float par6)
            : base(par1, par2, par3, 150, 20, par5Str)
		{
			SliderValue = 1.0F;
			Dragging = false;
			IdFloat = null;
			IdFloat = par4Options;
			SliderValue = par6;
		}

		/// <summary>
		/// Returns 0 if the button is disabled, 1 if the mouse is NOT hovering over this button and 2 if it IS hovering over
		/// this button.
		/// </summary>
		protected override int GetHoverState(bool par1)
		{
			return 0;
		}

		/// <summary>
		/// Fired when the mouse button is dragged. Equivalent of MouseListener.mouseDragged(MouseEvent e).
		/// </summary>
		protected override void MouseDragged(Minecraft par1Minecraft, int par2, int par3)
		{
			if (!ShowButton)
			{
				return;
			}

			if (Dragging)
			{
				SliderValue = (float)(par2 - (XPosition + 4)) / (float)(Width - 8);

				if (SliderValue < 0.0F)
				{
					SliderValue = 0.0F;
				}

				if (SliderValue > 1.0F)
				{
					SliderValue = 1.0F;
				}

				par1Minecraft.GameSettings.SetOptionFloatValue(IdFloat, SliderValue);
				DisplayString = par1Minecraft.GameSettings.GetKeyBinding(IdFloat);
			}

			//GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
			DrawTexturedModalRect(XPosition + (int)(SliderValue * (float)(Width - 8)), YPosition, 0, 66, 4, 20);
			DrawTexturedModalRect(XPosition + (int)(SliderValue * (float)(Width - 8)) + 4, YPosition, 196, 66, 4, 20);
		}

		/// <summary>
		/// Returns true if the mouse has been pressed on this control. Equivalent of MouseListener.mousePressed(MouseEvent
		/// e).
		/// </summary>
		public override bool MousePressed(Minecraft par1Minecraft, int mouseX, int mouseY)
		{
			if (base.MousePressed(par1Minecraft, mouseX, mouseY))
			{
				SliderValue = (float)(mouseX - (XPosition + 4)) / (float)(Width - 8);

				if (SliderValue < 0.0F)
				{
					SliderValue = 0.0F;
				}

				if (SliderValue > 1.0F)
				{
					SliderValue = 1.0F;
				}

				par1Minecraft.GameSettings.SetOptionFloatValue(IdFloat, SliderValue);
				DisplayString = par1Minecraft.GameSettings.GetKeyBinding(IdFloat);
				Dragging = true;
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Fired when the mouse button is released. Equivalent of MouseListener.mouseReleased(MouseEvent e).
		/// </summary>
		public override void MouseReleased(int par1, int par2)
		{
			Dragging = false;
		}
	}
}