using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace net.minecraft.src
{
	public class GuiScreen : Gui
	{
		/// <summary>
		/// Reference to the Minecraft object. </summary>
		public Minecraft Mc;

		/// <summary>
		/// The width of the screen object. </summary>
		public int Width;

		/// <summary>
		/// The height of the screen object. </summary>
		public int Height;

		/// <summary>
		/// A list of all the controls added to this container. </summary>
		protected List<GuiButton> ControlList;
		public bool AllowUserInput;

		/// <summary>
		/// The FontRenderer used by GuiScreen </summary>
		public FontRenderer FontRenderer;
		public GuiParticle GuiParticles;

		/// <summary>
		/// The button that was just pressed. </summary>
		private GuiButton SelectedButton;

		public GuiScreen()
		{
            ControlList = new List<GuiButton>();
			AllowUserInput = false;
			SelectedButton = null;
		}

		/// <summary>
		/// Draws the screen and all the components in it.
		/// </summary>
		public virtual void DrawScreen(int mouseX, int mouseY, float par3)
		{
			for (int i = 0; i < ControlList.Count; i++)
			{
				GuiButton guibutton = ControlList[i];
				guibutton.DrawButton(Mc, mouseX, mouseY);
			}
		}

		/// <summary>
		/// Fired when a key is typed. This is the equivalent of KeyListener.keyTyped(KeyEvent e).
		/// </summary>
		protected virtual void KeyTyped(char par1, int par2)
		{
			if (par2 == 1)
			{
				Mc.DisplayGuiScreen(null);
				Mc.SetIngameFocus();
			}
		}

		/// <summary>
		/// Returns a string stored in the system clipboard.
		/// </summary>
		public static string GetClipboardString()
		{/*
			try
			{
				java.awt.datatransfer.Transferable transferable = java.awt.Toolkit.getDefaultToolkit().getSystemClipboard().getContents(null);

				if (transferable != null && transferable.isDataFlavorSupported(java.awt.datatransfer.DataFlavor.stringFlavor))
				{
					return (string)transferable.getTransferData(java.awt.datatransfer.DataFlavor.stringFlavor);
				}
			}
			catch (Exception exception)
			{
			}
            */
			return "";
		}

		public static void WriteToClipboard(string par0Str)
		{/*
			try
			{
				java.awt.datatransfer.StringSelection stringselection = new java.awt.datatransfer.StringSelection(par0Str);
				java.awt.Toolkit.getDefaultToolkit().getSystemClipboard().setContents(stringselection, null);
			}
			catch (Exception exception)
			{
			}*/
		}

		/// <summary>
		/// Called when the mouse is clicked.
		/// </summary>
		protected virtual void MouseClicked(int mouseX, int mouseY, int par3)
		{
			if (par3 == 0)
			{
				for (int i = 0; i < ControlList.Count; i++)
				{
					GuiButton guibutton = ControlList[i];

					if (guibutton.MousePressed(Mc, mouseX, mouseY))
					{
						SelectedButton = guibutton;
						Mc.SndManager.PlaySoundFX("random.click", 1.0F, 1.0F);
						ActionPerformed(guibutton);
					}
				}
			}
		}

		/// <summary>
		/// Called when the mouse is moved or a mouse button is released.  Signature: (mouseX, mouseY, which) which==-1 is
		/// mouseMove, which==0 or which==1 is mouseUp
		/// </summary>
		protected virtual void MouseMovedOrUp(int par1, int par2, int par3)
		{
			if (SelectedButton != null && par3 == 0)
			{
				SelectedButton.MouseReleased(par1, par2);
				SelectedButton = null;
			}
		}

		/// <summary>
		/// Fired when a control is clicked. This is the equivalent of ActionListener.actionPerformed(ActionEvent e).
		/// </summary>
		protected virtual void ActionPerformed(GuiButton guibutton)
		{
		}

		/// <summary>
		/// Causes the screen to lay out its subcomponents again. This is the equivalent of the Java call
		/// Container.validate()
		/// </summary>
		public virtual void SetWorldAndResolution(Minecraft par1Minecraft, int par2, int par3)
		{
			GuiParticles = new GuiParticle(par1Minecraft);
			Mc = par1Minecraft;
			FontRenderer = par1Minecraft.FontRenderer;
			Width = par2;
			Height = par3;
			ControlList.Clear();
			InitGui();
		}

		/// <summary>
		/// Adds the buttons (and other controls) to the screen in question.
		/// </summary>
		public virtual void InitGui()
		{
		}

		/// <summary>
		/// Delegates mouse and keyboard input.
		/// </summary>
		public virtual void HandleInput()
		{/*
			for (; Mouse.next(); handleMouseInput())
			{
			}

			for (; Keyboard.next(); handleKeyboardInput())
			{
			}*/

            HandleMouseInput();

            HandleKeyboardInput();
		}

		/// <summary>
		/// Handles mouse input.
		/// </summary>
        public virtual void HandleMouseInput()
        {/*
			if (Mouse.getEventButtonState())
			{
				int i = (Mouse.getEventX() * Width) / Mc.DisplayWidth;
				int k = Height - (Mouse.getEventY() * Height) / Mc.DisplayHeight - 1;
				MouseClicked(i, k, Mouse.getEventButton());
			}
			else
			{
				int j = (Mouse.getEventX() * Width) / Mc.DisplayWidth;
				int l = Height - (Mouse.getEventY() * Height) / Mc.DisplayHeight - 1;
				MouseMovedOrUp(j, l, Mouse.getEventButton());
			}*/

            int x = (int)(Mc.Input.Mouse.X * ((float)Width / Mc.DisplayWidth));
            int y = (int)(Mc.Input.Mouse.Y * ((float)Height / Mc.DisplayHeight));

            if (Mc.Input.Mouse.WasButtonJustPressed())
            {
                if (Mc.Input.Mouse.WasButtonJustPressed(MouseButtons.Left))
                    MouseClicked(x, y, 0);
                else if (Mc.Input.Mouse.WasButtonJustPressed(MouseButtons.Right))
                    MouseClicked(x, y, 1);
                else
                    MouseClicked(x, y, -1);
            }
            else
            {
                if (Mc.Input.Mouse.WasButtonReleased())
                {
                    if (Mc.Input.Mouse.WasButtonReleased(MouseButtons.Left))
                        MouseMovedOrUp(x, y, 0);
                    else if (Mc.Input.Mouse.WasButtonReleased(MouseButtons.Right))
                        MouseMovedOrUp(x, y, -1);
                    else
                        MouseMovedOrUp(x, y, -1);
                }
            }
        }

		/// <summary>
		/// Handles keyboard input.
		/// </summary>
		public virtual void HandleKeyboardInput()
		{/*
			if (Keyboard.getEventKeyState())
			{
				if (Keyboard.getEventKey() == 87)
				{
					Mc.ToggleFullscreen();
					return;
				}

				KeyTyped(Keyboard.getEventCharacter(), Keyboard.getEventKey());
			}*/
            var key = Mc.Input.GetPressedKey();

            if (key != Microsoft.Xna.Framework.Input.Keys.None)
            {
                KeyTyped((char)key, (int)key);
            }
		}

		/// <summary>
		/// Called from the main game loop to update the screen.
		/// </summary>
		public virtual void UpdateScreen()
		{
		}

		/// <summary>
		/// Called when the screen is unloaded. Used to disable keyboard repeat events
		/// </summary>
		public virtual void OnGuiClosed()
		{
		}

		/// <summary>
		/// Draws either a gradient over the background screen (when it exists) or a flat gradient over background.png
		/// </summary>
		public virtual void DrawDefaultBackground()
		{
			DrawWorldBackground(0);
		}

		public virtual void DrawWorldBackground(int par1)
		{
			if (Mc.TheWorld != null)
			{
				DrawGradientRect(0, 0, Width, Height, 0xc010101, 0xd010101);
			}
			else
			{
				DrawBackground(par1);
			}
		}

		/// <summary>
		/// Draws the background (i is always 0 as of 1.2.2)
		/// </summary>
		public virtual void DrawBackground(int par1)
		{
			//GL.Disable(EnableCap.Lighting);
			//GL.Disable(EnableCap.Fog);
			Tessellator tessellator = Tessellator.Instance;
			//GL.BindTexture(TextureTarget.Texture2D, Mc.RenderEngineOld.GetTexture("/gui/background.png"));
            Mc.RenderEngine.BindTexture("gui.background.png");
			//GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
			/*float f = 32F;
			tessellator.StartDrawingQuads();
			tessellator.SetColorOpaque_I(0x404040);
			tessellator.AddVertexWithUV(0.0F, Height, 0.0F, 0.0F, (float)Height / f + (float)par1);
			tessellator.AddVertexWithUV(Width, Height, 0.0F, (float)Width / f, (float)Height / f + (float)par1);
			tessellator.AddVertexWithUV(Width, 0.0F, 0.0F, (float)Width / f, par1);
			tessellator.AddVertexWithUV(0.0F, 0.0F, 0.0F, 0.0F, par1);
			tessellator.Draw();*/

            Mc.RenderEngine.RenderSprite(new Rectangle(0, 0, Width, Height), new RectangleF(0, 0, Width / 32, Height / 32), 0.9f);
		}

		/// <summary>
		/// Returns true if this GUI should pause the game when it is displayed in single-player
		/// </summary>
		public virtual bool DoesGuiPauseGame()
		{
			return true;
		}

		public virtual void ConfirmClicked(bool flag, int i)
		{
		}

		public static bool Func_50051_l()
		{
            return false;// Keyboard.isKeyDown(29) || Keyboard.isKeyDown(157);
		}

		public static bool Func_50049_m()
		{
			return false;//Keyboard.isKeyDown(42) || Keyboard.isKeyDown(54);
		}
	}
}