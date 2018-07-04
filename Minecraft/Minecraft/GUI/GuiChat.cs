using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace net.minecraft.src
{
	public class GuiChat : GuiScreen
	{
		private string Field_50062_b;
		private int Field_50063_c;
		private bool Field_50060_d;
		private string Field_50061_e;
		private string Field_50059_f;
		private int Field_50067_h;
		private List<GuiPlayerInfo> Field_50068_i;
		private string Field_50065_j;
		protected GuiTextField Field_50064_a;
		private string Field_50066_k;

		public GuiChat()
		{
			Field_50062_b = "";
			Field_50063_c = -1;
			Field_50060_d = false;
			Field_50061_e = "";
			Field_50059_f = "";
			Field_50067_h = 0;
            Field_50068_i = new List<GuiPlayerInfo>();
			Field_50065_j = null;
			Field_50066_k = "";
		}

		public GuiChat(string par1Str)
		{
			Field_50062_b = "";
			Field_50063_c = -1;
			Field_50060_d = false;
			Field_50061_e = "";
			Field_50059_f = "";
			Field_50067_h = 0;
            Field_50068_i = new List<GuiPlayerInfo>();
			Field_50065_j = null;
			Field_50066_k = "";
			Field_50066_k = par1Str;
		}

		/// <summary>
		/// Adds the buttons (and other controls) to the screen in question.
		/// </summary>
		public override void InitGui()
		{
			//Keyboard.enableRepeatEvents(true);
			Field_50063_c = Mc.IngameGUI.Func_50013_c().Count;
			Field_50064_a = new GuiTextField(FontRenderer, 4, Height - 12, Width - 4, 12);
			Field_50064_a.SetMaxStringLength(100);
			Field_50064_a.Func_50027_a(false);
			Field_50064_a.Func_50033_b(true);
			Field_50064_a.SetText(Field_50066_k);
			Field_50064_a.Func_50026_c(false);
		}

		/// <summary>
		/// Called when the screen is unloaded. Used to disable keyboard repeat events
		/// </summary>
		public override void OnGuiClosed()
		{
			//Keyboard.enableRepeatEvents(false);
			Mc.IngameGUI.Func_50014_d();
		}

		/// <summary>
		/// Called from the main game loop to update the screen.
		/// </summary>
		public override void UpdateScreen()
		{
			Field_50064_a.UpdateCursorCounter();
		}

		/// <summary>
		/// Fired when a key is typed. This is the equivalent of KeyListener.keyTyped(KeyEvent e).
		/// </summary>
		protected override void KeyTyped(char par1, int par2)
		{
			if (par2 == 15)
			{
				CompletePlayerName();
			}
			else
			{
				Field_50060_d = false;
			}

			if (par2 == 1)
			{
				Mc.DisplayGuiScreen(null);
			}
			else if (par2 == 28)
			{
				string s = Field_50064_a.GetText().Trim();

				if (s.Length > 0 && !Mc.LineIsCommand(s))
				{
					Mc.ThePlayer.SendChatMessage(s);
				}

				Mc.DisplayGuiScreen(null);
			}
			else if (par2 == 200)
			{
				Func_50058_a(-1);
			}
			else if (par2 == 208)
			{
				Func_50058_a(1);
			}
			else if (par2 == 201)
			{
				Mc.IngameGUI.Func_50011_a(19);
			}
			else if (par2 == 209)
			{
				Mc.IngameGUI.Func_50011_a(-19);
			}
			else
			{
				Field_50064_a.Func_50037_a(par1, par2);
			}
		}

		/// <summary>
		/// Handles mouse input.
		/// </summary>
		public override void HandleMouseInput()
		{
			base.HandleMouseInput();
            int i = 0;// Mouse.getEventDWheel();

			if (i != 0)
			{
				if (i > 1)
				{
					i = 1;
				}

				if (i < -1)
				{
					i = -1;
				}

				if (!Func_50049_m())
				{
					i *= 7;
				}

				Mc.IngameGUI.Func_50011_a(i);
			}
		}

		/// <summary>
		/// Called when the mouse is clicked.
		/// </summary>
		protected override void MouseClicked(int par1, int par2, int par3)
		{
			if (par3 == 0)
			{
				ChatClickData chatclickdata = Mc.IngameGUI.Func_50012_a(0, 0);//Mouse.getX(), Mouse.getY());

				if (chatclickdata != null)
				{
					string uri = chatclickdata.Func_50089_b();

					if (uri != null)
					{
						Field_50065_j = uri;
						Mc.DisplayGuiScreen(new GuiChatConfirmLink(this, this, chatclickdata.Func_50088_a(), 0, chatclickdata));
						return;
					}
				}
			}

			Field_50064_a.MouseClicked(par1, par2, par3);
			base.MouseClicked(par1, par2, par3);
		}

		public override void ConfirmClicked(bool par1, int par2)
		{
			if (par2 == 0)
			{
				if (par1)
				{
					try
					{
						Type class1 = Type.GetType("java.awt.Desktop");
						object obj = class1.GetMethod("getDesktop", new Type[0]).Invoke(null, new object[0]);
						//class1.GetMethod("browse", new Type[] { typeof(URI) }).Invoke(obj, new object[] { Field_50065_j });
					}
					catch (Exception throwable)
					{
						Console.WriteLine(throwable.ToString());
						Console.Write(throwable.StackTrace);
					}
				}

				Field_50065_j = null;
				Mc.DisplayGuiScreen(this);
			}
		}

		/// <summary>
		/// Autocompletes player name
		/// </summary>
		public virtual void CompletePlayerName()
		{
			if (Field_50060_d)
			{
				Field_50064_a.Func_50021_a(-1);

				if (Field_50067_h >= Field_50068_i.Count)
				{
					Field_50067_h = 0;
				}
			}
			else
			{
				int i = Field_50064_a.Func_50028_c(-1);

				if (Field_50064_a.Func_50035_h() - i < 1)
				{
					return;
				}

				Field_50068_i.Clear();
				Field_50061_e = Field_50064_a.GetText().Substring(i);
				Field_50059_f = Field_50061_e.ToLower();
				IEnumerator<GuiPlayerInfo> iterator = ((EntityClientPlayerMP)Mc.ThePlayer).SendQueue.PlayerNames.GetEnumerator();

				do
				{
					if (!iterator.MoveNext())
					{
						break;
					}

					GuiPlayerInfo guiplayerinfo = iterator.Current;

					if (guiplayerinfo.NameStartsWith(Field_50059_f))
					{
						Field_50068_i.Add(guiplayerinfo);
					}
				}
				while (true);

				if (Field_50068_i.Count == 0)
				{
					return;
				}

				Field_50060_d = true;
				Field_50067_h = 0;
				Field_50064_a.Func_50020_b(i - Field_50064_a.Func_50035_h());
			}

			if (Field_50068_i.Count > 1)
			{
				StringBuilder stringbuilder = new StringBuilder();
				GuiPlayerInfo guiplayerinfo1;

				for (IEnumerator<GuiPlayerInfo> iterator1 = Field_50068_i.GetEnumerator(); iterator1.MoveNext(); stringbuilder.Append(guiplayerinfo1.Name))
				{
					guiplayerinfo1 = iterator1.Current;

					if (stringbuilder.Length > 0)
					{
						stringbuilder.Append(", ");
					}
				}

				Mc.IngameGUI.AddChatMessage(stringbuilder.ToString());
			}

			Field_50064_a.Func_50031_b(Field_50068_i[Field_50067_h++].Name);
		}

		public virtual void Func_50058_a(int par1)
		{
			int i = Field_50063_c + par1;
			int j = Mc.IngameGUI.Func_50013_c().Count;

			if (i < 0)
			{
				i = 0;
			}

			if (i > j)
			{
				i = j;
			}

			if (i == Field_50063_c)
			{
				return;
			}

			if (i == j)
			{
				Field_50063_c = j;
				Field_50064_a.SetText(Field_50062_b);
				return;
			}

			if (Field_50063_c == j)
			{
				Field_50062_b = Field_50064_a.GetText();
			}

			Field_50064_a.SetText((string)Mc.IngameGUI.Func_50013_c()[i]);
			Field_50063_c = i;
		}

		/// <summary>
		/// Draws the screen and all the components in it.
		/// </summary>
		public override void DrawScreen(int par1, int par2, float par3)
		{
			DrawRect(2, Height - 14, Width - 2, Height - 2, 0x8000000);
			Field_50064_a.DrawTextBox();
			base.DrawScreen(par1, par2, par3);
		}
	}
}