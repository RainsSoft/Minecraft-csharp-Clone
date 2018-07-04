using System.Collections.Generic;

namespace net.minecraft.src
{
	using net.minecraft.src;
	using Microsoft.Xna.Framework;

	public abstract class GuiSlot
	{
		private readonly Minecraft Mc;

		/// <summary>
		/// The width of the GuiScreen. Affects the container rendering, but not the overlays.
		/// </summary>
		private readonly int Width;

		/// <summary>
		/// The height of the GuiScreen. Affects the container rendering, but not the overlays or the scrolling.
		/// </summary>
		private readonly int Height;

		/// <summary>
		/// The top of the slot container. Affects the overlays and scrolling. </summary>
		protected readonly int Top;

		/// <summary>
		/// The bottom of the slot container. Affects the overlays and scrolling. </summary>
		protected readonly int Bottom;
		private readonly int Right;
		private readonly int Left = 0;

		/// <summary>
		/// The height of a slot. </summary>
		protected readonly int SlotHeight;

		/// <summary>
		/// button id of the button used to scroll up </summary>
		private int ScrollUpButtonID;

		/// <summary>
		/// the buttonID of the button used to scroll down </summary>
		private int ScrollDownButtonID;

		/// <summary>
		/// X axis position of the mouse </summary>
		protected int MouseX;

		/// <summary>
		/// Y axis position of the mouse </summary>
		protected int MouseY;

		/// <summary>
		/// where the mouse was in the window when you first clicked to scroll </summary>
		private float InitialClickY;

		/// <summary>
		/// what to multiply the amount you moved your mouse by(used for slowing down scrolling when over the items and no on
		/// scroll bar)
		/// </summary>
		private float ScrollMultiplier;

		/// <summary>
		/// how far down this slot has been scrolled </summary>
		private float AmountScrolled;

		/// <summary>
		/// the element in the list that was selected </summary>
		private int SelectedElement;

		/// <summary>
		/// the time when this button was last clicked. </summary>
		private long LastClicked;
		private bool Field_25123_p;
		private bool Field_27262_q;
		private int Field_27261_r;

		public GuiSlot(Minecraft par1Minecraft, int par2, int par3, int par4, int par5, int par6)
		{
			InitialClickY = -2F;
			SelectedElement = -1;
			LastClicked = 0L;
			Field_25123_p = true;
			Mc = par1Minecraft;
			Width = par2;
			Height = par3;
			Top = par4;
			Bottom = par5;
			SlotHeight = par6;
			Right = par2;
		}

		public virtual void Func_27258_a(bool par1)
		{
			Field_25123_p = par1;
		}

		protected virtual void Func_27259_a(bool par1, int par2)
		{
			Field_27262_q = par1;
			Field_27261_r = par2;

			if (!par1)
			{
				Field_27261_r = 0;
			}
		}

		/// <summary>
		/// Gets the size of the current slot list.
		/// </summary>
		public abstract int GetSize();

		/// <summary>
		/// the element in the slot that was clicked, bool for wether it was double clicked or not
		/// </summary>
		protected abstract void ElementClicked(int i, bool flag);

		/// <summary>
		/// returns true if the element passed in is currently selected
		/// </summary>
		protected abstract bool IsSelected(int i);

		/// <summary>
		/// return the height of the content being scrolled
		/// </summary>
		protected virtual int GetContentHeight()
		{
			return GetSize() * SlotHeight + Field_27261_r;
		}

		protected abstract void DrawBackground();

		protected abstract void DrawSlot(int i, int j, int k, int l, Tessellator tessellator);

		protected virtual void Func_27260_a(int i, int j, Tessellator tessellator)
		{
		}

		protected virtual void Func_27255_a(int i, int j)
		{
		}

		protected virtual void Func_27257_b(int i, int j)
		{
		}

		public virtual int Func_27256_c(int par1, int par2)
		{
			int i = Width / 2 - 110;
			int j = Width / 2 + 110;
			int k = ((par2 - Top - Field_27261_r) + (int)AmountScrolled) - 4;
			int l = k / SlotHeight;

			if (par1 >= i && par1 <= j && l >= 0 && k >= 0 && l < GetSize())
			{
				return l;
			}
			else
			{
				return -1;
			}
		}

		/// <summary>
		/// Registers the IDs that can be used for the scrollbar's buttons.
		/// </summary>
		public virtual void RegisterScrollButtons(List<GuiButton> par1List, int par2, int par3)
		{
			ScrollUpButtonID = par2;
			ScrollDownButtonID = par3;
		}

		/// <summary>
		/// stop the thing from scrolling out of bounds
		/// </summary>
		private void BindAmountScrolled()
		{
			int i = GetContentHeight() - (Bottom - Top - 4);

			if (i < 0)
			{
				i /= 2;
			}

			if (AmountScrolled < 0.0F)
			{
				AmountScrolled = 0.0F;
			}

			if (AmountScrolled > (float)i)
			{
				AmountScrolled = i;
			}
		}

		public virtual void ActionPerformed(GuiButton par1GuiButton)
		{
			if (!par1GuiButton.Enabled)
			{
				return;
			}

			if (par1GuiButton.Id == ScrollUpButtonID)
			{
				AmountScrolled -= (SlotHeight * 2) / 3;
				InitialClickY = -2F;
				BindAmountScrolled();
			}
			else if (par1GuiButton.Id == ScrollDownButtonID)
			{
				AmountScrolled += (SlotHeight * 2) / 3;
				InitialClickY = -2F;
				BindAmountScrolled();
			}
		}

		/// <summary>
		/// draws the slot to the screen, pass in mouse's current x and y and partial ticks
		/// </summary>
		public virtual void DrawScreen(int par1, int par2, float par3)
		{
			MouseX = par1;
			MouseY = par2;
			DrawBackground();
			int i = GetSize();
			int j = Width / 2 + 124;
			int k = j + 6;

			if (Mc.Input.Mouse.WasButtonPressed(MouseButtons.Left))
			{
				if (InitialClickY == -1F)
				{
					bool flag = true;

					if (par2 >= Top && par2 <= Bottom)
					{
						int i1 = Width / 2 - 110;
						int j1 = Width / 2 + 110;
						int l1 = ((par2 - Top - Field_27261_r) + (int)AmountScrolled) - 4;
						int j2 = l1 / SlotHeight;

						if (par1 >= i1 && par1 <= j1 && j2 >= 0 && l1 >= 0 && j2 < i)
						{
							bool flag1 = j2 == SelectedElement && JavaHelper.CurrentTimeMillis() - LastClicked < 250L;
							ElementClicked(j2, flag1);
							SelectedElement = j2;
							LastClicked = JavaHelper.CurrentTimeMillis();
						}
						else if (par1 >= i1 && par1 <= j1 && l1 < 0)
						{
							Func_27255_a(par1 - i1, ((par2 - Top) + (int)AmountScrolled) - 4);
							flag = false;
						}

						if (par1 >= j && par1 <= k)
						{
							ScrollMultiplier = -1F;
							int l2 = GetContentHeight() - (Bottom - Top - 4);

							if (l2 < 1)
							{
								l2 = 1;
							}

							int k3 = (int)((float)((Bottom - Top) * (Bottom - Top)) / (float)GetContentHeight());

							if (k3 < 32)
							{
								k3 = 32;
							}

							if (k3 > Bottom - Top - 8)
							{
								k3 = Bottom - Top - 8;
							}

							ScrollMultiplier /= (float)(Bottom - Top - k3) / (float)l2;
						}
						else
						{
							ScrollMultiplier = 1.0F;
						}

						if (flag)
						{
							InitialClickY = par2;
						}
						else
						{
							InitialClickY = -2F;
						}
					}
					else
					{
						InitialClickY = -2F;
					}
				}
				else if (InitialClickY >= 0.0F)
				{
					AmountScrolled -= ((float)par2 - InitialClickY) * ScrollMultiplier;
					InitialClickY = par2;
				}
			}
			else
			{/*
				do
				{
					if (!Mouse.next())
					{
						break;
					}
                    */
					int l = Mc.Input.Mouse.WheelDelta;

					if (l != 0)
					{
						if (l > 0)
						{
							l = -1;
						}
						else if (l < 0)
						{
							l = 1;
						}

						AmountScrolled += (l * SlotHeight) / 2;
					}/*
				}
				while (true);*/

				InitialClickY = -1F;
			}

			BindAmountScrolled();
			//GL.Disable(EnableCap.Lighting);
			//GL.Disable(EnableCap.Fog);
			Tessellator tessellator = Tessellator.Instance;
			//GL.BindTexture(TextureTarget.Texture2D, Mc.RenderEngineOld.GetTexture("/gui/background.png"));
            Mc.RenderEngine.BindTexture("gui.background.png");
			//GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
			float f = 32F;
			tessellator.StartDrawingQuads();
			tessellator.SetColorOpaque_I(0x202020);
			tessellator.AddVertexWithUV(Left, Bottom, 0.0F, (float)Left / f, (float)(Bottom + (int)AmountScrolled) / f);
			tessellator.AddVertexWithUV(Right, Bottom, 0.0F, (float)Right / f, (float)(Bottom + (int)AmountScrolled) / f);
			tessellator.AddVertexWithUV(Right, Top, 0.0F, (float)Right / f, (float)(Top + (int)AmountScrolled) / f);
			tessellator.AddVertexWithUV(Left, Top, 0.0F, (float)Left / f, (float)(Top + (int)AmountScrolled) / f);
			tessellator.Draw();
			int k1 = Width / 2 - 92 - 16;
			int i2 = (Top + 4) - (int)AmountScrolled;

			if (Field_27262_q)
			{
				Func_27260_a(k1, i2, tessellator);
			}

			for (int k2 = 0; k2 < i; k2++)
			{
				int i3 = i2 + k2 * SlotHeight + Field_27261_r;
				int l3 = SlotHeight - 4;

				if (i3 > Bottom || i3 + l3 < Top)
				{
					continue;
				}

				if (Field_25123_p && IsSelected(k2))
				{
					int j4 = Width / 2 - 110;
					int l4 = Width / 2 + 110;
					//GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
					//GL.Disable(EnableCap.Texture2D);
					tessellator.StartDrawingQuads();
					tessellator.SetColorOpaque_I(0x808080);
					tessellator.AddVertexWithUV(j4, i3 + l3 + 2, 0.0F, 0.0F, 1.0D);
					tessellator.AddVertexWithUV(l4, i3 + l3 + 2, 0.0F, 1.0D, 1.0D);
					tessellator.AddVertexWithUV(l4, i3 - 2, 0.0F, 1.0D, 0.0F);
					tessellator.AddVertexWithUV(j4, i3 - 2, 0.0F, 0.0F, 0.0F);
					tessellator.SetColorOpaque_I(0);
					tessellator.AddVertexWithUV(j4 + 1, i3 + l3 + 1, 0.0F, 0.0F, 1.0D);
					tessellator.AddVertexWithUV(l4 - 1, i3 + l3 + 1, 0.0F, 1.0D, 1.0D);
					tessellator.AddVertexWithUV(l4 - 1, i3 - 1, 0.0F, 1.0D, 0.0F);
					tessellator.AddVertexWithUV(j4 + 1, i3 - 1, 0.0F, 0.0F, 0.0F);
					tessellator.Draw();
					//GL.Enable(EnableCap.Texture2D);
				}

				DrawSlot(k2, k1, i3, l3, tessellator);
			}

			//GL.Disable(EnableCap.DepthTest);
			sbyte byte0 = 4;
			OverlayBackground(0, Top, 255, 255);
			OverlayBackground(Bottom, Height, 255, 255);
			//GL.Enable(EnableCap.Blend);
			//GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			//GL.Disable(EnableCap.AlphaTest);
			//GL.ShadeModel(ShadingModel.Smooth);
			//GL.Disable(EnableCap.Texture2D);
			tessellator.StartDrawingQuads();
			tessellator.SetColorRGBA_I(0, 0);
			tessellator.AddVertexWithUV(Left, Top + byte0, 0.0F, 0.0F, 1.0D);
			tessellator.AddVertexWithUV(Right, Top + byte0, 0.0F, 1.0D, 1.0D);
			tessellator.SetColorRGBA_I(0, 255);
			tessellator.AddVertexWithUV(Right, Top, 0.0F, 1.0D, 0.0F);
			tessellator.AddVertexWithUV(Left, Top, 0.0F, 0.0F, 0.0F);
			tessellator.Draw();
			tessellator.StartDrawingQuads();
			tessellator.SetColorRGBA_I(0, 255);
			tessellator.AddVertexWithUV(Left, Bottom, 0.0F, 0.0F, 1.0D);
			tessellator.AddVertexWithUV(Right, Bottom, 0.0F, 1.0D, 1.0D);
			tessellator.SetColorRGBA_I(0, 0);
			tessellator.AddVertexWithUV(Right, Bottom - byte0, 0.0F, 1.0D, 0.0F);
			tessellator.AddVertexWithUV(Left, Bottom - byte0, 0.0F, 0.0F, 0.0F);
			tessellator.Draw();
			int j3 = GetContentHeight() - (Bottom - Top - 4);

			if (j3 > 0)
			{
				int i4 = ((Bottom - Top) * (Bottom - Top)) / GetContentHeight();

				if (i4 < 32)
				{
					i4 = 32;
				}

				if (i4 > Bottom - Top - 8)
				{
					i4 = Bottom - Top - 8;
				}

				int k4 = ((int)AmountScrolled * (Bottom - Top - i4)) / j3 + Top;

				if (k4 < Top)
				{
					k4 = Top;
				}

				tessellator.StartDrawingQuads();
				tessellator.SetColorRGBA_I(0, 255);
				tessellator.AddVertexWithUV(j, Bottom, 0.0F, 0.0F, 1.0D);
				tessellator.AddVertexWithUV(k, Bottom, 0.0F, 1.0D, 1.0D);
				tessellator.AddVertexWithUV(k, Top, 0.0F, 1.0D, 0.0F);
				tessellator.AddVertexWithUV(j, Top, 0.0F, 0.0F, 0.0F);
				tessellator.Draw();
				tessellator.StartDrawingQuads();
				tessellator.SetColorRGBA_I(0x808080, 255);
				tessellator.AddVertexWithUV(j, k4 + i4, 0.0F, 0.0F, 1.0D);
				tessellator.AddVertexWithUV(k, k4 + i4, 0.0F, 1.0D, 1.0D);
				tessellator.AddVertexWithUV(k, k4, 0.0F, 1.0D, 0.0F);
				tessellator.AddVertexWithUV(j, k4, 0.0F, 0.0F, 0.0F);
				tessellator.Draw();
				tessellator.StartDrawingQuads();
				tessellator.SetColorRGBA_I(0xc0c0c0, 255);
				tessellator.AddVertexWithUV(j, (k4 + i4) - 1, 0.0F, 0.0F, 1.0D);
				tessellator.AddVertexWithUV(k - 1, (k4 + i4) - 1, 0.0F, 1.0D, 1.0D);
				tessellator.AddVertexWithUV(k - 1, k4, 0.0F, 1.0D, 0.0F);
				tessellator.AddVertexWithUV(j, k4, 0.0F, 0.0F, 0.0F);
				tessellator.Draw();
			}

			Func_27257_b(par1, par2);
			//GL.Enable(EnableCap.Texture2D);
			//GL.ShadeModel(ShadingModel.Flat);
			//GL.Enable(EnableCap.AlphaTest);
			//GL.Disable(EnableCap.Blend);
		}

		/// <summary>
		/// Overlays the background to hide scrolled items
		/// </summary>
		private void OverlayBackground(int par1, int par2, int par3, int par4)
		{
			Tessellator tessellator = Tessellator.Instance;
			//GL.BindTexture(TextureTarget.Texture2D, Mc.RenderEngineOld.GetTexture("/gui/background.png"));
            Mc.RenderEngine.BindTexture("gui.background.png");
			//GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
			float f = 32F;/*
			tessellator.StartDrawingQuads();
			tessellator.SetColorRGBA_I(0x404040, par4);
			tessellator.AddVertexWithUV(0.0F,  par2, 0.0F, 0.0F,             (float)par2 / f);
			tessellator.AddVertexWithUV(Width, par2, 0.0F, (float)Width / f, (float)par2 / f);
			tessellator.SetColorRGBA_I(0x404040, par3);
			tessellator.AddVertexWithUV(Width, par1, 0.0F, (float)Width / f, (float)par1 / f);
			tessellator.AddVertexWithUV(0.0F,  par1, 0.0F, 0.0F,             (float)par1 / f);
			tessellator.Draw();*/

            Mc.RenderEngine.RenderSprite(new Rectangle(0, par2, Width, par2 - par1), new Rectangle(0, par2, Width, par2 - par1));
		}
	}
}