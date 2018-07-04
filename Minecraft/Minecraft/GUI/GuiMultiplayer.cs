using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace net.minecraft.src
{
    public class GuiMultiplayer : GuiScreen
    {
        /// <summary>
        /// Number of outstanding ThreadPollServers threads </summary>
        private static int ThreadsPending = 0;

        /// <summary>
        /// Lock object for use with synchronized() </summary>
        private static object Lock = new object();

        /// <summary>
        /// A reference to the screen object that created this. Used for navigating between screens.
        /// </summary>
        private GuiScreen ParentScreen;

        /// <summary>
        /// Slot container for the server list </summary>
        private GuiSlotServer ServerSlotContainer;

        /// <summary>
        /// List of ServerNBTStorage objects </summary>
        private List<ServerNBTStorage> ServerList;

        /// <summary>
        /// Index of the currently selected server </summary>
        private int SelectedServer;

        /// <summary>
        /// The 'Edit' button </summary>
        private GuiButton ButtonEdit;

        /// <summary>
        /// The 'Join Server' button </summary>
        private GuiButton ButtonSelect;

        /// <summary>
        /// The 'Delete' button </summary>
        private GuiButton ButtonDelete;

        /// <summary>
        /// The 'Delete' button was clicked </summary>
        private bool DeleteClicked;

        /// <summary>
        /// The 'Add server' button was clicked </summary>
        private bool AddClicked;

        /// <summary>
        /// The 'Edit' button was clicked </summary>
        private bool EditClicked;

        /// <summary>
        /// The 'Direct Connect' button was clicked </summary>
        private bool DirectClicked;

        /// <summary>
        /// This GUI's lag tooltip text or null if no lag icon is being hovered. </summary>
        private string LagTooltip;

        /// <summary>
        /// Temporary ServerNBTStorage used by the Edit/Add/Direct Connect dialogs
        /// </summary>
        private ServerNBTStorage TempServer;

        public GuiMultiplayer(GuiScreen par1GuiScreen)
        {
            ServerList = new List<ServerNBTStorage>();
            SelectedServer = -1;
            DeleteClicked = false;
            AddClicked = false;
            EditClicked = false;
            DirectClicked = false;
            LagTooltip = null;
            TempServer = null;
            ParentScreen = par1GuiScreen;
        }

        /// <summary>
        /// Called from the main game loop to update the screen.
        /// </summary>
        public override void UpdateScreen()
        {
        }

        /// <summary>
        /// Adds the buttons (and other controls) to the screen in question.
        /// </summary>
        public override void InitGui()
        {
            LoadServerList();
            //Keyboard.enableRepeatEvents(true);
            ControlList.Clear();
            ServerSlotContainer = new GuiSlotServer(this);
            InitGuiControls();
        }

        /// <summary>
        /// Load the server list from servers.dat
        /// </summary>
        private void LoadServerList()
        {
            try
            {
                NBTTagCompound nbttagcompound = CompressedStreamTools.Read(System.IO.Path.Combine(Mc.McDataDir, "servers.dat"));
                NBTTagList nbttaglist = nbttagcompound.GetTagList("servers");
                ServerList.Clear();

                for (int i = 0; i < nbttaglist.TagCount(); i++)
                {
                    ServerList.Add(ServerNBTStorage.CreateServerNBTStorage((NBTTagCompound)nbttaglist.TagAt(i)));
                }
            }
            catch (Exception exception)
            {
                Utilities.LogException(exception);
            }
        }

        /// <summary>
        /// Save the server list to servers.dat
        /// </summary>
        private void SaveServerList()
        {
            try
            {
                NBTTagList nbttaglist = new NBTTagList();

                for (int i = 0; i < ServerList.Count; i++)
                {
                    nbttaglist.AppendTag(ServerList[i].GetCompoundTag());
                }

                NBTTagCompound nbttagcompound = new NBTTagCompound();
                nbttagcompound.SetTag("servers", nbttaglist);
                CompressedStreamTools.SafeWrite(nbttagcompound, System.IO.Path.Combine(Mc.McDataDir, "servers.dat"));
            }
            catch (Exception exception)
            {
                Utilities.LogException(exception);
            }
        }

        /// <summary>
        /// Populate the GuiScreen controlList
        /// </summary>
        public virtual void InitGuiControls()
        {
            StringTranslate stringtranslate = StringTranslate.GetInstance();
            ControlList.Add(ButtonEdit = new GuiButton(7, Width / 2 - 154, Height - 28, 70, 20, stringtranslate.TranslateKey("selectServer.edit")));
            ControlList.Add(ButtonDelete = new GuiButton(2, Width / 2 - 74, Height - 28, 70, 20, stringtranslate.TranslateKey("selectServer.delete")));
            ControlList.Add(ButtonSelect = new GuiButton(1, Width / 2 - 154, Height - 52, 100, 20, stringtranslate.TranslateKey("selectServer.select")));
            ControlList.Add(new GuiButton(4, Width / 2 - 50, Height - 52, 100, 20, stringtranslate.TranslateKey("selectServer.direct")));
            ControlList.Add(new GuiButton(3, Width / 2 + 4 + 50, Height - 52, 100, 20, stringtranslate.TranslateKey("selectServer.add")));
            ControlList.Add(new GuiButton(8, Width / 2 + 4, Height - 28, 70, 20, stringtranslate.TranslateKey("selectServer.refresh")));
            ControlList.Add(new GuiButton(0, Width / 2 + 4 + 76, Height - 28, 75, 20, stringtranslate.TranslateKey("gui.cancel")));
            bool flag = SelectedServer >= 0 && SelectedServer < ServerSlotContainer.GetSize();
            ButtonSelect.Enabled = flag;
            ButtonEdit.Enabled = flag;
            ButtonDelete.Enabled = flag;
        }

        /// <summary>
        /// Called when the screen is unloaded. Used to disable keyboard repeat events
        /// </summary>
        public override void OnGuiClosed()
        {
            //Keyboard.enableRepeatEvents(false);
        }

        /// <summary>
        /// Fired when a control is clicked. This is the equivalent of ActionListener.actionPerformed(ActionEvent e).
        /// </summary>
        protected override void ActionPerformed(GuiButton par1GuiButton)
        {
            if (!par1GuiButton.Enabled)
            {
                return;
            }

            if (par1GuiButton.Id == 2)
            {
                string s = ServerList[SelectedServer].Name;

                if (s != null)
                {
                    DeleteClicked = true;
                    StringTranslate stringtranslate = StringTranslate.GetInstance();
                    string s1 = stringtranslate.TranslateKey("selectServer.deleteQuestion");
                    string s2 = new StringBuilder().Append("'").Append(s).Append("' ").Append(stringtranslate.TranslateKey("selectServer.deleteWarning")).ToString();
                    string s3 = stringtranslate.TranslateKey("selectServer.deleteButton");
                    string s4 = stringtranslate.TranslateKey("gui.cancel");
                    GuiYesNo guiyesno = new GuiYesNo(this, s1, s2, s3, s4, SelectedServer);
                    Mc.DisplayGuiScreen(guiyesno);
                }
            }
            else if (par1GuiButton.Id == 1)
            {
                JoinServer(SelectedServer);
            }
            else if (par1GuiButton.Id == 4)
            {
                DirectClicked = true;
                Mc.DisplayGuiScreen(new GuiScreenServerList(this, TempServer = new ServerNBTStorage(StatCollector.TranslateToLocal("selectServer.defaultName"), "")));
            }
            else if (par1GuiButton.Id == 3)
            {
                AddClicked = true;
                Mc.DisplayGuiScreen(new GuiScreenAddServer(this, TempServer = new ServerNBTStorage(StatCollector.TranslateToLocal("selectServer.defaultName"), "")));
            }
            else if (par1GuiButton.Id == 7)
            {
                EditClicked = true;
                ServerNBTStorage servernbtstorage = (ServerNBTStorage)ServerList[SelectedServer];
                Mc.DisplayGuiScreen(new GuiScreenAddServer(this, TempServer = new ServerNBTStorage(servernbtstorage.Name, servernbtstorage.Host)));
            }
            else if (par1GuiButton.Id == 0)
            {
                Mc.DisplayGuiScreen(ParentScreen);
            }
            else if (par1GuiButton.Id == 8)
            {
                Mc.DisplayGuiScreen(new GuiMultiplayer(ParentScreen));
            }
            else
            {
                ServerSlotContainer.ActionPerformed(par1GuiButton);
            }
        }

        public override void ConfirmClicked(bool par1, int par2)
        {
            if (DeleteClicked)
            {
                DeleteClicked = false;

                if (par1)
                {
                    ServerList.RemoveAt(par2);
                    SaveServerList();
                }

                Mc.DisplayGuiScreen(this);
            }
            else if (DirectClicked)
            {
                DirectClicked = false;

                if (par1)
                {
                    JoinServer(TempServer);
                }
                else
                {
                    Mc.DisplayGuiScreen(this);
                }
            }
            else if (AddClicked)
            {
                AddClicked = false;

                if (par1)
                {
                    ServerList.Add(TempServer);
                    SaveServerList();
                }

                Mc.DisplayGuiScreen(this);
            }
            else if (EditClicked)
            {
                EditClicked = false;

                if (par1)
                {
                    ServerNBTStorage servernbtstorage = ServerList[SelectedServer];
                    servernbtstorage.Name = TempServer.Name;
                    servernbtstorage.Host = TempServer.Host;
                    SaveServerList();
                }

                Mc.DisplayGuiScreen(this);
            }
        }

        private int ParseIntWithDefault(string par1Str, int par2)
        {
            try
            {
                return Convert.ToInt32(par1Str.Trim());
            }
            catch (Exception exception)
            {
                Utilities.LogException(exception);

                return par2;
            }
        }

        /// <summary>
        /// Fired when a key is typed. This is the equivalent of KeyListener.keyTyped(KeyEvent e).
        /// </summary>
        protected override void KeyTyped(char par1, int par2)
        {
            if (par1 == '\r')
            {
                ActionPerformed(ControlList[2]);
            }
        }

        /// <summary>
        /// Called when the mouse is clicked.
        /// </summary>
        protected override void MouseClicked(int par1, int par2, int par3)
        {
            base.MouseClicked(par1, par2, par3);
        }

        /// <summary>
        /// Draws the screen and all the components in it.
        /// </summary>
        public override void DrawScreen(int par1, int par2, float par3)
        {
            LagTooltip = null;
            StringTranslate stringtranslate = StringTranslate.GetInstance();
            DrawDefaultBackground();
            ServerSlotContainer.DrawScreen(par1, par2, par3);
            DrawCenteredString(FontRenderer, stringtranslate.TranslateKey("multiplayer.title"), Width / 2, 20, 0xffffff);
            base.DrawScreen(par1, par2, par3);

            if (LagTooltip != null)
            {
                Func_35325_a(LagTooltip, par1, par2);
            }
        }

        /// <summary>
        /// Join server by slot index
        /// </summary>
        private void JoinServer(int par1)
        {
            JoinServer(ServerList[par1]);
        }

        /// <summary>
        /// Join server by ServerNBTStorage
        /// </summary>
        private void JoinServer(ServerNBTStorage par1ServerNBTStorage)
        {
            string s = par1ServerNBTStorage.Host;
            string[] @as = StringHelperClass.StringSplit(s, ":", true);

            if (s.StartsWith("["))
            {
                int i = s.IndexOf("]");

                if (i > 0)
                {
                    string s1 = s.Substring(1, i - 1);
                    string s2 = s.Substring(i + 1).Trim();

                    if (s2.StartsWith(":") && s2.Length > 0)
                    {
                        s2 = s2.Substring(1);
                        @as = new string[2];
                        @as[0] = s1;
                        @as[1] = s2;
                    }
                    else
                    {
                        @as = new string[1];
                        @as[0] = s1;
                    }
                }
            }

            if (@as.Length > 2)
            {
                @as = new string[1];
                @as[0] = s;
            }

            Mc.DisplayGuiScreen(new GuiConnecting(Mc, @as[0], @as.Length <= 1 ? 25565 : ParseIntWithDefault(@as[1], 25565)));
        }

        /// <summary>
        /// Poll server for MOTD, lag, and player count/max
        /// </summary>
        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
        //ORIGINAL LINE: private void pollServer(ServerNBTStorage par1ServerNBTStorage) throws IOException
        public void PollServer(ServerNBTStorage par1ServerNBTStorage)
        {
            string s = par1ServerNBTStorage.Host;
            string[] @as = StringHelperClass.StringSplit(s, ":", true);

            if (s.StartsWith("["))
            {
                int i = s.IndexOf("]");

                if (i > 0)
                {
                    string s2 = s.Substring(1, i - 1);
                    string s3 = s.Substring(i + 1).Trim();

                    if (s3.StartsWith(":") && s3.Length > 0)
                    {
                        s3 = s3.Substring(1);
                        @as = new string[2];
                        @as[0] = s2;
                        @as[1] = s3;
                    }
                    else
                    {
                        @as = new string[1];
                        @as[0] = s2;
                    }
                }
            }

            if (@as.Length > 2)
            {
                @as = new string[1];
                @as[0] = s;
            }

            string s1 = @as[0];
            int j = @as.Length <= 1 ? 25565 : ParseIntWithDefault(@as[1], 25565);
            Socket socket = null;
            NetworkStream dataStream = null;

            try
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.SendTimeout = 3000;
                //socket.SetTcpNoDelay(true);
                //socket.SetTrafficClass(18);
                socket.Connect(s1, j);
                dataStream = new NetworkStream(socket);
                dataStream.WriteByte(254);

                if (dataStream.ReadByte() != 255)
                {
                    throw new IOException("Bad message");
                }

                string s4 = Packet.ReadString(new BinaryReader(dataStream), 256);
                char[] ac = s4.ToCharArray();

                for (int k = 0; k < ac.Length; k++)
                {
                    if (ac[k] != '\u0247' && ChatAllowedCharacters.AllowedCharacters.IndexOf(ac[k]) < 0)
                    {
                        ac[k] = '?';
                    }
                }
                s4 = new string(ac);
                string[] as1 = StringHelperClass.StringSplit(s4, "\u0247", true);
                s4 = as1[0];
                int l = -1;
                int i1 = -1;
                try
                {
                    l = Convert.ToInt32(as1[1]);
                    i1 = Convert.ToInt32(as1[2]);
                }
                catch (Exception exception)
                {
                    Utilities.LogException(exception);
                }

                par1ServerNBTStorage.Motd = (new StringBuilder()).Append("\u02477").Append(s4).ToString();

                if (l >= 0 && i1 > 0)
                {
                    par1ServerNBTStorage.PlayerCount = (new StringBuilder()).Append("\u02477").Append(l).Append("\u02478/\u02477").Append(i1).ToString();
                }
                else
                {
                    par1ServerNBTStorage.PlayerCount = "\u02478???";
                }
            }
            finally
            {
                try
                {
                    if (dataStream != null)
                    {
                        dataStream.Close();
                    }
                }
                catch (Exception throwable)
                {
                    Utilities.LogException(throwable);
                }

                try
                {
                    if (socket != null)
                    {
                        socket.Close();
                    }
                }
                catch (Exception throwable2)
                {
                    Utilities.LogException(throwable2);
                }
            }
        }

        protected void Func_35325_a(string par1Str, int par2, int par3)
        {
            if (par1Str == null)
            {
                return;
            }
            else
            {
                int i = par2 + 12;
                int j = par3 - 12;
                int k = FontRenderer.GetStringWidth(par1Str);
                DrawGradientRect(i - 3, j - 3, i + k + 3, j + 8 + 3, 0xc000000, 0xc000000);
                FontRenderer.DrawStringWithShadow(par1Str, i, j, -1);
                return;
            }
        }

        /// <summary>
        /// Return the List of ServerNBTStorage objects
        /// </summary>
        public static List<ServerNBTStorage> GetServerList(GuiMultiplayer par0GuiMultiplayer)
        {
            return par0GuiMultiplayer.ServerList;
        }

        /// <summary>
        /// Set index of the currently selected server
        /// </summary>
        public static int SetSelectedServer(GuiMultiplayer par0GuiMultiplayer, int par1)
        {
            return par0GuiMultiplayer.SelectedServer = par1;
        }

        /// <summary>
        /// Return index of the currently selected server
        /// </summary>
        public static int GetSelectedServer(GuiMultiplayer par0GuiMultiplayer)
        {
            return par0GuiMultiplayer.SelectedServer;
        }

        /// <summary>
        /// Return buttonSelect GuiButton
        /// </summary>
        public static GuiButton GetButtonSelect(GuiMultiplayer par0GuiMultiplayer)
        {
            return par0GuiMultiplayer.ButtonSelect;
        }

        /// <summary>
        /// Return buttonEdit GuiButton
        /// </summary>
        public static GuiButton GetButtonEdit(GuiMultiplayer par0GuiMultiplayer)
        {
            return par0GuiMultiplayer.ButtonEdit;
        }

        /// <summary>
        /// Return buttonDelete GuiButton
        /// </summary>
        public static GuiButton GetButtonDelete(GuiMultiplayer par0GuiMultiplayer)
        {
            return par0GuiMultiplayer.ButtonDelete;
        }

        /// <summary>
        /// Join server by slot index (called on double click from GuiSlotServer)
        /// </summary>
        public static void JoinServer(GuiMultiplayer par0GuiMultiplayer, int par1)
        {
            par0GuiMultiplayer.JoinServer(par1);
        }

        /// <summary>
        /// Get lock object for use with synchronized()
        /// </summary>
        public static object GetLock()
        {
            return Lock;
        }

        /// <summary>
        /// Return number of outstanding ThreadPollServers threads
        /// </summary>
        public static int GetThreadsPending()
        {
            return ThreadsPending;
        }

        /// <summary>
        /// Increment number of outstanding ThreadPollServers threads by 1
        /// </summary>
        public static int IncrementThreadsPending()
        {
            return ThreadsPending++;
        }

        /// <summary>
        /// Poll server for MOTD, lag, and player count/max
        /// </summary>
        public static void PollServer(GuiMultiplayer par0GuiMultiplayer, ServerNBTStorage par1ServerNBTStorage)
        {
            try
            {
                par0GuiMultiplayer.PollServer(par1ServerNBTStorage);
            }
            catch (IOException e)
            {
                Utilities.LogException(e);
            }
        }

        /// <summary>
        /// Decrement number of outstanding ThreadPollServers threads by 1
        /// </summary>
        public static int DecrementThreadsPending()
        {
            return ThreadsPending--;
        }

        /// <summary>
        /// Sets a GUI's lag tooltip text.
        /// </summary>
        public static string SetTooltipText(GuiMultiplayer par0GuiMultiplayer, string par1Str)
        {
            return par0GuiMultiplayer.LagTooltip = par1Str;
        }
    }
}