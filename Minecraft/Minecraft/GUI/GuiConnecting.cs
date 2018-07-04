using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace net.minecraft.src
{
	using net.minecraft.src;

	public class GuiConnecting : GuiScreen
	{
		/// <summary>
		/// A reference to the NetClientHandler. </summary>
		private NetClientHandler ClientHandler;

		/// <summary>
		/// True if the connection attempt has been cancelled. </summary>
		private bool Cancelled;

        private string IP;

        private int Port;

		public GuiConnecting(Minecraft par1Minecraft, string par2Str, int par3)
		{
			Cancelled = false;
			Console.WriteLine((new StringBuilder()).Append("Connecting to ").Append(par2Str).Append(", ").Append(par3).ToString());
			par1Minecraft.ChangeWorld1(null);
			new Thread(ConnectToServer).Start();
		}

        private void ConnectToServer()
        {
            try
            {
                ClientHandler = new NetClientHandler(Mc, IP, Port);

                if (Cancelled)
                {
                    return;
                }

                ClientHandler.AddToSendQueue(new Packet2Handshake(Mc.Session.Username, IP, Port));
            }/*
            catch (UnknownHostException unknownhostexception)
            {
                if (Cancelled)
                {
                    return;
                }

                Mc.DisplayGuiScreen(new GuiDisconnected("connect.failed", "disconnect.genericReason", new object[] { (new StringBuilder()).Append("Unknown host '").Append(IP).Append("'").ToString() }));
            }*/
            catch (SocketException connectexception)
            {
                if (Cancelled)
                {
                    return;
                }

                Mc.DisplayGuiScreen(new GuiDisconnected("connect.failed", "disconnect.genericReason", new object[] { connectexception.Message }));
            }
            catch (Exception exception)
            {
                if (Cancelled)
                {
                    return;
                }

                Console.WriteLine(exception.ToString());
                Console.Write(exception.StackTrace);
                Mc.DisplayGuiScreen(new GuiDisconnected("connect.failed", "disconnect.genericReason", new object[] { exception.ToString() }));
            }
        }

		/// <summary>
		/// Called from the main game loop to update the screen.
		/// </summary>
		public override void UpdateScreen()
		{
			if (ClientHandler != null)
			{
				ClientHandler.ProcessReadPackets();
			}
		}

		/// <summary>
		/// Fired when a key is typed. This is the equivalent of KeyListener.keyTyped(KeyEvent e).
		/// </summary>
		protected override void KeyTyped(char c, int i)
		{
		}

		/// <summary>
		/// Adds the buttons (and other controls) to the screen in question.
		/// </summary>
		public override void InitGui()
		{
			StringTranslate stringtranslate = StringTranslate.GetInstance();
			ControlList.Clear();
			ControlList.Add(new GuiButton(0, Width / 2 - 100, Height / 4 + 120 + 12, stringtranslate.TranslateKey("gui.cancel")));
		}

		/// <summary>
		/// Fired when a control is clicked. This is the equivalent of ActionListener.actionPerformed(ActionEvent e).
		/// </summary>
		protected override void ActionPerformed(GuiButton par1GuiButton)
		{
			if (par1GuiButton.Id == 0)
			{
				Cancelled = true;

				if (ClientHandler != null)
				{
					ClientHandler.Disconnect();
				}

				Mc.DisplayGuiScreen(new GuiMainMenu());
			}
		}

		/// <summary>
		/// Draws the screen and all the components in it.
		/// </summary>
		public override void DrawScreen(int par1, int par2, float par3)
		{
			DrawDefaultBackground();
			StringTranslate stringtranslate = StringTranslate.GetInstance();

			if (ClientHandler == null)
			{
				DrawCenteredString(FontRenderer, stringtranslate.TranslateKey("connect.connecting"), Width / 2, Height / 2 - 50, 0xffffff);
				DrawCenteredString(FontRenderer, "", Width / 2, Height / 2 - 10, 0xffffff);
			}
			else
			{
				DrawCenteredString(FontRenderer, stringtranslate.TranslateKey("connect.authorizing"), Width / 2, Height / 2 - 50, 0xffffff);
				DrawCenteredString(FontRenderer, ClientHandler.Field_1209_a, Width / 2, Height / 2 - 10, 0xffffff);
			}

			base.DrawScreen(par1, par2, par3);
		}
	}
}