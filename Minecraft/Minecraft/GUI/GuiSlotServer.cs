using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;

namespace net.minecraft.src
{
	class GuiSlotServer : GuiSlot
	{
		/// <summary>
		/// Instance to the GUI this list is on. </summary>
		public readonly GuiMultiplayer ParentGui;

		public GuiSlotServer(GuiMultiplayer par1GuiMultiplayer) : base(par1GuiMultiplayer.Mc, par1GuiMultiplayer.Width, par1GuiMultiplayer.Height, 32, par1GuiMultiplayer.Height - 64, 36)
		{
			ParentGui = par1GuiMultiplayer;
		}

		/// <summary>
		/// Gets the size of the current slot list.
		/// </summary>
        public override int GetSize()
		{
			return GuiMultiplayer.GetServerList(ParentGui).Count;
		}

		/// <summary>
		/// the element in the slot that was clicked, bool for wether it was double clicked or not
		/// </summary>
		protected override void ElementClicked(int par1, bool par2)
		{
			GuiMultiplayer.SetSelectedServer(ParentGui, par1);
			bool flag = GuiMultiplayer.GetSelectedServer(ParentGui) >= 0 && GuiMultiplayer.GetSelectedServer(ParentGui) < GetSize();
			GuiMultiplayer.GetButtonSelect(ParentGui).Enabled = flag;
			GuiMultiplayer.GetButtonEdit(ParentGui).Enabled = flag;
			GuiMultiplayer.GetButtonDelete(ParentGui).Enabled = flag;

			if (par2 && flag)
			{
				GuiMultiplayer.JoinServer(ParentGui, par1);
			}
		}

		/// <summary>
		/// returns true if the element passed in is currently selected
		/// </summary>
		protected override bool IsSelected(int par1)
		{
			return par1 == GuiMultiplayer.GetSelectedServer(ParentGui);
		}

		/// <summary>
		/// return the height of the content being scrolled
		/// </summary>
		protected override int GetContentHeight()
		{
			return GuiMultiplayer.GetServerList(ParentGui).Count * 36;
		}

		protected override void DrawBackground()
		{
			ParentGui.DrawDefaultBackground();
		}

		protected override void DrawSlot(int par1, int par2, int par3, int par4, Tessellator par5Tessellator)
		{
			ServerNBTStorage servernbtstorage = GuiMultiplayer.GetServerList(ParentGui)[par1];

            lock (GuiMultiplayer.GetLock())
            {
                if (GuiMultiplayer.GetThreadsPending() < 5 && !servernbtstorage.Polled)
                {
                    servernbtstorage.Polled = true;
                    servernbtstorage.Lag = -2L;
                    servernbtstorage.Motd = "";
                    servernbtstorage.PlayerCount = "";
                    GuiMultiplayer.IncrementThreadsPending();

                    Action pollServers = () =>
                    {
                        try
                        {
                            servernbtstorage.Motd = "Polling..";
                            long l = JavaHelper.NanoTime();
                            GuiMultiplayer.PollServer(ParentGui, servernbtstorage);
                            long l1 = JavaHelper.NanoTime();
                            servernbtstorage.Lag = (l1 - l) / 0xf4240L;
                        }
                        catch (SocketException sockettimeoutexception)
                        {
                            Utilities.LogException(sockettimeoutexception);

                            servernbtstorage.Lag = -1L;
                            servernbtstorage.Motd = "Can't reach server";
                        }
                        catch (IOException ioexception)
                        {
                            Utilities.LogException(ioexception);

                            servernbtstorage.Lag = -1L;
                            servernbtstorage.Motd = "Communication error";
                        }
                        catch (Exception exception)
                        {
                            Utilities.LogException(exception);

                            servernbtstorage.Lag = -1L;
                            servernbtstorage.Motd = (new StringBuilder()).Append("ERROR: ").Append(exception.GetType()).ToString();
                        }
                        finally
                        {
                            lock (GuiMultiplayer.GetLock())
                            {
                                GuiMultiplayer.DecrementThreadsPending();
                            }
                        }
                    };

                    new Thread(new ThreadStart(pollServers)).Start();
                }
            }

			ParentGui.DrawString(ParentGui.FontRenderer, servernbtstorage.Name, par2 + 2, par3 + 1, 0xffffff);
			ParentGui.DrawString(ParentGui.FontRenderer, servernbtstorage.Motd, par2 + 2, par3 + 12, 0x808080);
			ParentGui.DrawString(ParentGui.FontRenderer, servernbtstorage.PlayerCount, (par2 + 215) - (int)ParentGui.FontRenderer.GetStringWidth(servernbtstorage.PlayerCount), par3 + 12, 0x808080);
			ParentGui.DrawString(ParentGui.FontRenderer, servernbtstorage.Host, par2 + 2, par3 + 12 + 11, 0x303030);
			//GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
			ParentGui.Mc.RenderEngine.BindTexture("gui.icons.png");
			string s = "";
			int i;
			int j;

			if (servernbtstorage.Polled && servernbtstorage.Lag != -2L)
			{
				i = 0;
				j = 0;

				if (servernbtstorage.Lag < 0L)
				{
					j = 5;
				}
				else if (servernbtstorage.Lag < 150L)
				{
					j = 0;
				}
				else if (servernbtstorage.Lag < 300L)
				{
					j = 1;
				}
				else if (servernbtstorage.Lag < 600L)
				{
					j = 2;
				}
				else if (servernbtstorage.Lag < 1000L)
				{
					j = 3;
				}
				else
				{
					j = 4;
				}

				if (servernbtstorage.Lag < 0L)
				{
					s = "(no connection)";
				}
				else
				{
					s = (new StringBuilder()).Append(servernbtstorage.Lag).Append("ms").ToString();
				}
			}
			else
			{
				i = 1;
				j = (int)(JavaHelper.CurrentTimeMillis() / 100L + (long)(par1 * 2) & 7L);

				if (j > 4)
				{
					j = 8 - j;
				}

				s = "Polling..";
			}

			ParentGui.DrawTexturedModalRect(par2 + 205, par3, 0 + i * 10, 176 + j * 8, 10, 8);
			sbyte byte0 = 4;

			if (MouseX >= (par2 + 205) - byte0 && MouseY >= par3 - byte0 && MouseX <= par2 + 205 + 10 + byte0 && MouseY <= par3 + 8 + byte0)
			{
				GuiMultiplayer.SetTooltipText(ParentGui, s);
			}
		}
	}
}