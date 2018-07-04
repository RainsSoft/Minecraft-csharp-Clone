using System;
using System.Text;
using Microsoft.Xna.Framework;

namespace net.minecraft.src
{
	public class GuiTextField : Gui
	{
		/// <summary>
		/// Have the font renderer from GuiScreen to render the textbox text into the screen.
		/// </summary>
		private readonly FontRenderer FontRenderer;
		private readonly int XPos;
		private readonly int YPos;
		private readonly int Width;
		private readonly int Height;

		/// <summary>
		/// Have the current text beign edited on the textbox. </summary>
		private string Text;
		private int MaxStringLength;
		private int CursorCounter;
		private bool Field_50044_j;
		private bool Field_50045_k;

		/// <summary>
		/// If this value is true along isEnabled, keyTyped will process the keys.
		/// </summary>
		private bool IsFocused;
		private bool Field_50043_m;
		private int Field_50041_n;
		private int Field_50042_o;
		private int Field_50048_p;
		private int Field_50047_q;
		private int Field_50046_r;

		public GuiTextField(FontRenderer par1FontRenderer, int par2, int par3, int par4, int par5)
		{
			Text = "";
			MaxStringLength = 32;
			Field_50044_j = true;
			Field_50045_k = true;
			IsFocused = false;
			Field_50043_m = true;
			Field_50041_n = 0;
			Field_50042_o = 0;
			Field_50048_p = 0;
			Field_50047_q = 0xe0e0e0;
			Field_50046_r = 0x707070;
			FontRenderer = par1FontRenderer;
			XPos = par2;
			YPos = par3;
			Width = par4;
			Height = par5;
		}

		/// <summary>
		/// Increments the cursor counter
		/// </summary>
		public virtual void UpdateCursorCounter()
		{
			CursorCounter++;
		}

		/// <summary>
		/// Sets the text of the textbox.
		/// </summary>
		public virtual void SetText(string par1Str)
		{
			if (par1Str.Length > MaxStringLength)
			{
				Text = par1Str.Substring(0, MaxStringLength);
			}
			else
			{
				Text = par1Str;
			}

			Func_50038_e();
		}

		/// <summary>
		/// Returns the text beign edited on the textbox.
		/// </summary>
		public virtual string GetText()
		{
			return Text;
		}

		public virtual string Func_50039_c()
		{
			int i = Field_50042_o >= Field_50048_p ? Field_50048_p : Field_50042_o;
			int j = Field_50042_o >= Field_50048_p ? Field_50042_o : Field_50048_p;
			return Text.Substring(i, j - i);
		}

		public virtual void Func_50031_b(string par1Str)
		{
			string s = "";
			string s1 = ChatAllowedCharacters.Func_52019_a(par1Str);
			int i = Field_50042_o >= Field_50048_p ? Field_50048_p : Field_50042_o;
			int j = Field_50042_o >= Field_50048_p ? Field_50042_o : Field_50048_p;
			int k = MaxStringLength - Text.Length - (i - Field_50048_p);
			int l = 0;

			if (Text.Length > 0)
			{
				s = (new StringBuilder()).Append(s).Append(Text.Substring(0, i)).ToString();
			}

			if (k < s1.Length)
			{
				s = (new StringBuilder()).Append(s).Append(s1.Substring(0, k)).ToString();
				l = k;
			}
			else
			{
				s = (new StringBuilder()).Append(s).Append(s1).ToString();
				l = s1.Length;
			}

			if (Text.Length > 0 && j < Text.Length)
			{
				s = (new StringBuilder()).Append(s).Append(Text.Substring(j)).ToString();
			}

			Text = s;
			Func_50023_d((i - Field_50048_p) + l);
		}

		public virtual void Func_50021_a(int par1)
		{
			if (Text.Length == 0)
			{
				return;
			}

			if (Field_50048_p != Field_50042_o)
			{
				Func_50031_b("");
				return;
			}
			else
			{
				Func_50020_b(Func_50028_c(par1) - Field_50042_o);
				return;
			}
		}

		public virtual void Func_50020_b(int par1)
		{
			if (Text.Length == 0)
			{
				return;
			}

			if (Field_50048_p != Field_50042_o)
			{
				Func_50031_b("");
				return;
			}

			bool flag = par1 < 0;
			int i = flag ? Field_50042_o + par1 : Field_50042_o;
			int j = flag ? Field_50042_o : Field_50042_o + par1;
			string s = "";

			if (i >= 0)
			{
				s = Text.Substring(0, i);
			}

			if (j < Text.Length)
			{
				s = (new StringBuilder()).Append(s).Append(Text.Substring(j)).ToString();
			}

			Text = s;

			if (flag)
			{
				Func_50023_d(par1);
			}
		}

		public virtual int Func_50028_c(int par1)
		{
			return Func_50024_a(par1, Func_50035_h());
		}

		public virtual int Func_50024_a(int par1, int par2)
		{
			int i = par2;
			bool flag = par1 < 0;
			int j = Math.Abs(par1);

			for (int k = 0; k < j; k++)
			{
				if (flag)
				{
					for (; i > 0 && Text[i - 1] == ' '; i--)
					{
					}

					for (; i > 0 && Text[i - 1] != ' '; i--)
					{
					}

					continue;
				}

				int l = Text.Length;
				i = Text.IndexOf(' ', i);

				if (i == -1)
				{
					i = l;
					continue;
				}

				for (; i < l && Text[i] == ' '; i++)
				{
				}
			}

			return i;
		}

		public virtual void Func_50023_d(int par1)
		{
			Func_50030_e(Field_50048_p + par1);
		}

		public virtual void Func_50030_e(int par1)
		{
			Field_50042_o = par1;
			int i = Text.Length;

			if (Field_50042_o < 0)
			{
				Field_50042_o = 0;
			}

			if (Field_50042_o > i)
			{
				Field_50042_o = i;
			}

			Func_50032_g(Field_50042_o);
		}

		public virtual void Func_50034_d()
		{
			Func_50030_e(0);
		}

		public virtual void Func_50038_e()
		{
			Func_50030_e(Text.Length);
		}

		public virtual bool Func_50037_a(int par1, int par2)
		{
			if (!Field_50043_m || !IsFocused)
			{
				return false;
			}

			switch (par1)
			{
				case 1:
					Func_50038_e();
					Func_50032_g(0);
					return true;

				case 3:
					GuiScreen.WriteToClipboard(Func_50039_c());
					return true;

				case 22:
					Func_50031_b(GuiScreen.GetClipboardString());
					return true;

				case 24:
					GuiScreen.WriteToClipboard(Func_50039_c());
					Func_50031_b("");
					return true;
			}

			switch (par2)
			{
				case 203:
					if (GuiScreen.Func_50049_m())
					{
						if (GuiScreen.Func_50051_l())
						{
							Func_50032_g(Func_50024_a(-1, Func_50036_k()));
						}
						else
						{
							Func_50032_g(Func_50036_k() - 1);
						}
					}
					else if (GuiScreen.Func_50051_l())
					{
						Func_50030_e(Func_50028_c(-1));
					}
					else
					{
						Func_50023_d(-1);
					}

					return true;

				case 205:
					if (GuiScreen.Func_50049_m())
					{
						if (GuiScreen.Func_50051_l())
						{
							Func_50032_g(Func_50024_a(1, Func_50036_k()));
						}
						else
						{
							Func_50032_g(Func_50036_k() + 1);
						}
					}
					else if (GuiScreen.Func_50051_l())
					{
						Func_50030_e(Func_50028_c(1));
					}
					else
					{
						Func_50023_d(1);
					}

					return true;

				case 14:
					if (GuiScreen.Func_50051_l())
					{
						Func_50021_a(-1);
					}
					else
					{
						Func_50020_b(-1);
					}

					return true;

				case 211:
					if (GuiScreen.Func_50051_l())
					{
						Func_50021_a(1);
					}
					else
					{
						Func_50020_b(1);
					}

					return true;

				case 199:
					if (GuiScreen.Func_50049_m())
					{
						Func_50032_g(0);
					}
					else
					{
						Func_50034_d();
					}

					return true;

				case 207:
					if (GuiScreen.Func_50049_m())
					{
						Func_50032_g(Text.Length);
					}
					else
					{
						Func_50038_e();
					}

					return true;
			}

			if (ChatAllowedCharacters.IsAllowedCharacter((char)par1))
			{
				Func_50031_b(((char)par1).ToString());
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Args: x, y, buttonClicked
		/// </summary>
		public virtual void MouseClicked(int par1, int par2, int par3)
		{
			bool flag = par1 >= XPos && par1 < XPos + Width && par2 >= YPos && par2 < YPos + Height;

			if (Field_50045_k)
			{
				Func_50033_b(Field_50043_m && flag);
			}

			if (IsFocused && par3 == 0)
			{
				int i = par1 - XPos;

				if (Field_50044_j)
				{
					i -= 4;
				}

				string s = FontRenderer.Func_50107_a(Text.Substring(Field_50041_n), Func_50019_l());
				Func_50030_e(FontRenderer.Func_50107_a(s, i).Length + Field_50041_n);
			}
		}

		/// <summary>
		/// Draws the textbox
		/// </summary>
		public virtual void DrawTextBox()
		{
			if (Func_50022_i())
			{
				DrawRect(XPos - 1, YPos - 1, XPos + Width + 1, YPos + Height + 1, 0xffa0a0a);
				DrawRect(XPos, YPos, XPos + Width, YPos + Height, 0xff00000);
			}

			int i = Field_50043_m ? Field_50047_q : Field_50046_r;
			int j = Field_50042_o - Field_50041_n;
			int k = Field_50048_p - Field_50041_n;
			string s = FontRenderer.Func_50107_a(Text.Substring(Field_50041_n), Func_50019_l());
			bool flag = j >= 0 && j <= s.Length;
			bool flag1 = IsFocused && (CursorCounter / 6) % 2 == 0 && flag;
			int l = Field_50044_j ? XPos + 4 : XPos;
			int i1 = Field_50044_j ? YPos + (Height - 8) / 2 : YPos;
			int j1 = l;

			if (k > s.Length)
			{
				k = s.Length;
			}

			if (s.Length > 0)
			{
				string s1 = flag ? s.Substring(0, j) : s;
				/*j1 = */FontRenderer.DrawStringWithShadow(s1, j1, i1, i);
			}

			bool flag2 = Field_50042_o < Text.Length || Text.Length >= Func_50040_g();
			int k1 = j1;

			if (!flag)
			{
				k1 = j <= 0 ? l : l + Width;
			}
			else if (flag2)
			{
				k1--;
				j1--;
			}

			if (s.Length > 0 && flag && j < s.Length)
			{
				/*j1 = */FontRenderer.DrawStringWithShadow(s.Substring(j), j1, i1, i);
			}

			if (flag1)
			{
				if (flag2)
				{
					Gui.DrawRect(k1, i1 - 1, k1 + 1, i1 + 1 + FontRenderer.FontHeight, 0xffd0d0d);
				}
				else
				{
					FontRenderer.DrawStringWithShadow("_", k1, i1, i);
				}
			}

			if (k != j)
			{
				int l1 = l + FontRenderer.GetStringWidth(s.Substring(0, k));
				Func_50029_c(k1, i1 - 1, l1 - 1, i1 + 1 + FontRenderer.FontHeight);
			}
		}

		private void Func_50029_c(int par1, int par2, int par3, int par4)
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
            /*
			Tessellator tessellator = Tessellator.Instance;
			GL.Color4(0.0F, 0.0F, 255F, 255F);
			GL.Disable(EnableCap.Texture2D);
            GL.Enable(EnableCap.ColorLogicOp);
			GL.LogicOp(LogicOp.OrReverse);
			tessellator.StartDrawingQuads();
			tessellator.AddVertex(par1, par4, 0.0F);
			tessellator.AddVertex(par3, par4, 0.0F);
			tessellator.AddVertex(par3, par2, 0.0F);
			tessellator.AddVertex(par1, par2, 0.0F);
			tessellator.Draw();
			GL.Disable(EnableCap.ColorLogicOp);
			GL.Enable(EnableCap.Texture2D);*/

            RenderEngine.Instance.RenderSprite(new Rectangle(par1, par4, par3, par2), null);
        }

		public virtual void SetMaxStringLength(int par1)
		{
			MaxStringLength = par1;

			if (Text.Length > par1)
			{
				Text = Text.Substring(0, par1);
			}
		}

		public virtual int Func_50040_g()
		{
			return MaxStringLength;
		}

		public virtual int Func_50035_h()
		{
			return Field_50042_o;
		}

		public virtual bool Func_50022_i()
		{
			return Field_50044_j;
		}

		public virtual void Func_50027_a(bool par1)
		{
			Field_50044_j = par1;
		}

		public virtual void Func_50033_b(bool par1)
		{
			if (par1 && !IsFocused)
			{
				CursorCounter = 0;
			}

			IsFocused = par1;
		}

		public virtual bool Func_50025_j()
		{
			return IsFocused;
		}

		public virtual int Func_50036_k()
		{
			return Field_50048_p;
		}

		public virtual int Func_50019_l()
		{
			return Func_50022_i() ? Width - 8 : Width;
		}

		public virtual void Func_50032_g(int par1)
		{
			int i = Text.Length;

			if (par1 > i)
			{
				par1 = i;
			}

			if (par1 < 0)
			{
				par1 = 0;
			}

			Field_50048_p = par1;

			if (FontRenderer != null)
			{
				if (Field_50041_n > i)
				{
					Field_50041_n = i;
				}

				int j = Func_50019_l();
				string s = FontRenderer.Func_50107_a(Text.Substring(Field_50041_n), j);
				int k = s.Length + Field_50041_n;

				if (par1 == Field_50041_n)
				{
					Field_50041_n -= FontRenderer.Func_50104_a(Text, j, true).Length;
				}

				if (par1 > k)
				{
					Field_50041_n += par1 - k;
				}
				else if (par1 <= Field_50041_n)
				{
					Field_50041_n -= Field_50041_n - par1;
				}

				if (Field_50041_n < 0)
				{
					Field_50041_n = 0;
				}

				if (Field_50041_n > i)
				{
					Field_50041_n = i;
				}
			}
		}

		public virtual void Func_50026_c(bool par1)
		{
			Field_50045_k = par1;
		}
	}
}