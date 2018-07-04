using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.Xna.Framework;

namespace net.minecraft.src
{
	public class GuiWinGame : GuiScreen
	{
		/// <summary>
		/// Counts the number of screen updates. </summary>
		private int UpdateCounter;

		/// <summary>
		/// List of lines on the ending poem and credits. </summary>
		private List<string> Lines;
		private int Field_41042_d;
		private float Field_41043_e;

		public GuiWinGame()
		{
			UpdateCounter = 0;
			Field_41042_d = 0;
			Field_41043_e = 0.5F;
		}

		/// <summary>
		/// Called from the main game loop to update the screen.
		/// </summary>
		public override void UpdateScreen()
		{
			UpdateCounter++;
			float f = (float)(Field_41042_d + Height + Height + 24) / Field_41043_e;

			if ((float)UpdateCounter > f)
			{
				RespawnPlayer();
			}
		}

		/// <summary>
		/// Fired when a key is typed. This is the equivalent of KeyListener.keyTyped(KeyEvent e).
		/// </summary>
		protected override void KeyTyped(char par1, int par2)
		{
			if (par2 == 1)
			{
				RespawnPlayer();
			}
		}

		/// <summary>
		/// Respawns the player.
		/// </summary>
		private void RespawnPlayer()
		{
			if (Mc.TheWorld.IsRemote)
			{
				EntityClientPlayerMP entityclientplayermp = (EntityClientPlayerMP)Mc.ThePlayer;
				entityclientplayermp.SendQueue.AddToSendQueue(new Packet9Respawn(entityclientplayermp.Dimension, (sbyte)Mc.TheWorld.DifficultySetting, Mc.TheWorld.GetWorldInfo().GetTerrainType(), Mc.TheWorld.GetHeight(), 0));
			}
			else
			{
				Mc.DisplayGuiScreen(null);
				Mc.Respawn(Mc.TheWorld.IsRemote, 0, true);
			}
		}

		/// <summary>
		/// Returns true if this GUI should pause the game when it is displayed in single-player
		/// </summary>
		public override bool DoesGuiPauseGame()
		{
			return true;
		}

		/// <summary>
		/// Adds the buttons (and other controls) to the screen in question.
		/// </summary>
		public override void InitGui()
		{
			if (Lines != null)
			{
				return;
			}

			Lines = new List<string>();

			try
			{
				string s = "";
				string s1 = "0xa7f0xa7k0xa7a0xa7b";
				int c = 274;
				StreamReader bufferedreader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("/title/win.txt"), Encoding.UTF8);
				Random random = new Random(0x7bf7d3);

				while ((s = bufferedreader.ReadLine()) != null)
				{
					string s2;
					string s3;

                    for (s = s.Replace("PLAYERNAME", Mc.Session.Username); s.IndexOf(s1) >= 0; s = (new StringBuilder()).Append(s2).Append("0xa7f0xa7k").Append("XXXXXXXX".Substring(0, random.Next(4) + 3)).Append(s3).ToString())
					{
						int i = s.IndexOf(s1);
						s2 = s.Substring(0, i);
						s3 = s.Substring(i + s1.Length);
					}

					Lines.AddRange(Mc.FontRendererOld.Func_50108_c(s, c));
					Lines.Add("");
				}

				for (int j = 0; j < 8; j++)
				{
					Lines.Add("");
				}

				bufferedreader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("/title/credits.txt"), Encoding.UTF8);

				while ((s = bufferedreader.ReadLine()) != null)
				{
					s = s.Replace("PLAYERNAME", Mc.Session.Username);
					s = s.Replace("\t", "    ");
					Lines.AddRange(Mc.FontRendererOld.Func_50108_c(s, c));
					Lines.Add("");
				}

				Field_41042_d = Lines.Count * 12;
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception.ToString());
				Console.Write(exception.StackTrace);
			}
		}

		/// <summary>
		/// Fired when a control is clicked. This is the equivalent of ActionListener.actionPerformed(ActionEvent e).
		/// </summary>
		protected override void ActionPerformed(GuiButton guibutton)
		{
		}

		private void Func_41040_b(int par1, int par2, float par3)
		{
			Tessellator tessellator = Tessellator.Instance;
			//GL.BindTexture(TextureTarget.Texture2D, Mc.RenderEngineOld.GetTexture("%blur%/gui/background.png"));
			tessellator.StartDrawingQuads();
			tessellator.SetColorRGBA_F(1.0F, 1.0F, 1.0F, 1.0F);
			int i = Width;
			float f = 0.0F - ((float)UpdateCounter + par3) * 0.5F * Field_41043_e;
			float f1 = (float)Height - ((float)UpdateCounter + par3) * 0.5F * Field_41043_e;
			float f2 = 0.015625F;
			float f3 = (((float)UpdateCounter + par3) - 0.0F) * 0.02F;
			float f4 = (float)(Field_41042_d + Height + Height + 24) / Field_41043_e;
			float f5 = (f4 - 20F - ((float)UpdateCounter + par3)) * 0.005F;

			if (f5 < f3)
			{
				f3 = f5;
			}

			if (f3 > 1.0F)
			{
				f3 = 1.0F;
			}

			f3 *= f3;
			f3 = (f3 * 96F) / 255F;
			tessellator.SetColorOpaque_F(f3, f3, f3);
			tessellator.AddVertexWithUV(0.0F, Height, ZLevel, 0.0F, f * f2);
			tessellator.AddVertexWithUV(i, Height, ZLevel, (float)i * f2, f * f2);
			tessellator.AddVertexWithUV(i, 0.0F, ZLevel, (float)i * f2, f1 * f2);
			tessellator.AddVertexWithUV(0.0F, 0.0F, ZLevel, 0.0F, f1 * f2);
			tessellator.Draw();
		}

		/// <summary>
		/// Draws the screen and all the components in it.
		/// </summary>
		public override void DrawScreen(int par1, int par2, float par3)
		{
			Func_41040_b(par1, par2, par3);
			Tessellator tessellator = Tessellator.Instance;
			int c = 274;
			int i = Width / 2 - c / 2;
			int j = Height + 50;
			float f = -((float)UpdateCounter + par3) * Field_41043_e;
			//GL.PushMatrix();
			//GL.Translate(0.0F, f, 0.0F);
			//GL.BindTexture(TextureTarget.Texture2D, Mc.RenderEngineOld.GetTexture("/title/mclogo.png"));
			//GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
			DrawTexturedModalRect(i, j, 0, 0, 155, 44);
			DrawTexturedModalRect(i + 155, j, 0, 45, 155, 44);
			tessellator.SetColorOpaque_I(0xffffff);
			int k = j + 200;

			for (int l = 0; l < Lines.Count; l++)
			{
				if (l == Lines.Count - 1)
				{
					float f1 = ((float)k + f) - (float)(Height / 2 - 6);

					if (f1 < 0.0F)
					{
						//GL.Translate(0.0F, -f1, 0.0F);
					}
				}

				if ((float)k + f + 12F + 8F > 0.0F && (float)k + f < (float)Height)
				{
					string s = (string)Lines[l];

					if (s.StartsWith("[C]"))
					{
                        FontRenderer.DrawStringWithShadow(s.Substring(3), i + (c - (int)FontRenderer.GetStringWidth(s.Substring(3))) / 2, k, 0xffffff);
					}
					else
					{
                        FontRenderer.FontRandom.SetSeed((int)l * (int)0xfca9953 + (UpdateCounter / 4));
						FontRenderer.Func_50101_a(s, i + 1, k + 1, 0xffffff, true);
                        FontRenderer.FontRandom.SetSeed((int)l * (int)0xfca9953 + (UpdateCounter / 4));
						FontRenderer.Func_50101_a(s, i, k, 0xffffff, false);
					}
				}

				k += 12;
			}

			//GL.PopMatrix();
			//GL.BindTexture(TextureTarget.Texture2D, Mc.RenderEngineOld.GetTexture("%blur%/misc/vignette.png"));
			//GL.Enable(EnableCap.Blend);
			//GL.BlendFunc(BlendingFactorSrc.Zero, BlendingFactorDest.OneMinusSrcColor);
			tessellator.StartDrawingQuads();
			tessellator.SetColorRGBA_F(1.0F, 1.0F, 1.0F, 1.0F);
			int i1 = Width;
			int j1 = Height;
			tessellator.AddVertexWithUV(0.0F, j1, ZLevel, 0.0F, 1.0D);
			tessellator.AddVertexWithUV(i1, j1, ZLevel, 1.0D, 1.0D);
			tessellator.AddVertexWithUV(i1, 0.0F, ZLevel, 1.0D, 0.0F);
			tessellator.AddVertexWithUV(0.0F, 0.0F, ZLevel, 0.0F, 0.0F);
			tessellator.Draw();
			//GL.Disable(EnableCap.Blend);
			base.DrawScreen(par1, par2, par3);
		}
	}
}