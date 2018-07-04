using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using net.minecraft.src;
using IOPath = System.IO.Path;
using MathHelper = net.minecraft.src.MathHelper2;

namespace net.minecraft.src
{
    public class Minecraft : Game
    {
        /// <summary>
        /// Set to 'this' in Minecraft constructor; used by some settings get methods
        /// </summary>
        private static Minecraft theMinecraft;
        public PlayerController PlayerController;
        private bool fullscreen;
        private bool hasCrashed;
        public int DisplayWidth;
        public int DisplayHeight;

        /// <summary>
        /// Checks OpenGL capabilities (as of 1.2.3_04 effectively unused). </summary>
        private OpenGlCapsChecker glCapabilities;
        private net.minecraft.src.Timer timer;

        /// <summary>
        /// The World instance that Minecraft uses. </summary>
        public World TheWorld;
        public RenderGlobal RenderGlobal;

        /// <summary>
        /// The player who's actually in control of this game. </summary>
        public EntityPlayerSP ThePlayer;

        /// <summary>
        /// The Entity from which the renderer determines the render viewpoint. Currently is always the parent Minecraft
        /// class's 'ThePlayer' instance. Modification of its location, rotation, or other settings at render time will
        /// modify the camera likewise, with the caveat of triggering chunk rebuilds as it moves, making it unsuitable for
        /// changing the viewpoint mid-render.
        /// </summary>
        public EntityLiving RenderViewEntity;
        public EffectRenderer EffectRenderer;
        public Session Session;
        public string MinecraftUri;

        /// <summary>
        /// a bool to hide a Quit button from the main menu </summary>
        public bool HideQuitButton;
        public volatile bool IsGamePaused;

        /// <summary>
        /// The RenderEngine instance used by Minecraft </summary>
        public RenderEngineOld RenderEngineOld;

        /// <summary>
        /// The font renderer used for displaying and measuring text. </summary>
        public FontRendererOld FontRendererOld;
        public FontRendererOld StandardGalacticFontRenderer;

        /// <summary>
        /// The GuiScreen that's being displayed at the moment. </summary>
        public GuiScreen CurrentScreen;
        public LoadingScreenRenderer LoadingScreen;
        public EntityRenderer EntityRenderer;

        /// <summary>
        /// Reference to the download resources thread. </summary>
        private ResourceDownloader resourceDownloader;

        /// <summary>
        /// Number of ticks ran since the program was started. </summary>
        private int ticksRan;

        /// <summary>
        /// Mouse left click counter </summary>
        private int leftClickCounter;

        /// <summary>
        /// Display width </summary>
        private int tempDisplayWidth;

        /// <summary>
        /// Display height </summary>
        private int tempDisplayHeight;

        /// <summary>
        /// Gui achievement </summary>
        public GuiAchievement GuiAchievement;
        public GuiIngame IngameGUI;

        /// <summary>
        /// Skip render world </summary>
        public bool SkipRenderWorld;

        /// <summary>
        /// The ModelBiped of the player </summary>
        public ModelBiped PlayerModelBiped;

        /// <summary>
        /// The ray trace hit that the mouse is over. </summary>
        public MovingObjectPosition ObjectMouseOver;

        /// <summary>
        /// The game settings that currently hold effect. </summary>
        public GameSettings GameSettings;
        public SoundManager SndManager;

        /// <summary>
        /// The TexturePackLister used by this instance of Minecraft... </summary>
        public TexturePackList TexturePackList;
        public string McDataDir;
        private ISaveFormat saveLoader;
        public static long[] FrameTimes = new long[512];
        public static long[] TickTimes = new long[512];
        public static int NumRecordedFrameTimes = 0;

        /// <summary>
        /// time in milliseconds when TheadCheckHasPaid determined you have not paid. 0 if you have paid. Used in
        /// GuiAchievement whether to display the nag text.
        /// </summary>
        public static long HasPaidCheckTime = 0L;

        /// <summary>
        /// When you place a block, it's set to 6, decremented once per tick, when it's 0, you can place another block.
        /// </summary>
        private int rightClickDelayTimer;

        /// <summary>
        /// Stat file writer </summary>
        public StatFileWriter StatFileWriter;
        private string serverName;
        private int serverPort;
        private TextureWaterFX textureWaterFX;
        private TextureLavaFX textureLavaFX;

        /// <summary>
        /// The working dir (OS specific) for minecraft </summary>
        private static string minecraftDir = null;

        /// <summary>
        /// Set to true to keep the game loop running. Set to false by shutdown() to allow the game loop to exit cleanly.
        /// </summary>
        public volatile bool Running;

        /// <summary>
        /// String that shows the debug information </summary>
        public string Debug;

        /// <summary>
        /// Approximate time (in ms) of last update to debug string </summary>
        long DebugUpdateTime;

        /// <summary>
        /// holds the current fps </summary>
        int FpsCounter;

        /// <summary>
        /// Makes sure it doesn't keep taking screenshots when both buttons are down.
        /// </summary>
        bool IsTakingScreenshot;
        long PrevFrameTime;

        /// <summary>
        /// Profiler currently displayed in the debug screen pie chart </summary>
        private string DebugProfilerName;

        /// <summary>
        /// Does the actual gameplay have focus. If so then mouse and keys will effect the player instead of menus.
        /// </summary>
        public bool InGameHasFocus;
        public bool IsRaining;
        long SystemTime;

        /// <summary>
        /// Join player counter </summary>
        private int joinPlayerCounter;


        private GraphicsDeviceManager graphics;

        public RenderEngine RenderEngine;
        public FontRenderer FontRenderer;

        public InputHelper Input;

        private bool showSplash;

        public Minecraft(int width, int height, bool useFullscreen)
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = width;
            graphics.PreferredBackBufferHeight = height;
            graphics.IsFullScreen = useFullscreen;
            graphics.ApplyChanges();

            Window.Title = GetMinecraftTitle();
            Window.AllowUserResizing = true;

            hasCrashed = false;
            timer = new net.minecraft.src.Timer(20F);
            Session = null;
            HideQuitButton = false;
            IsGamePaused = false;
            CurrentScreen = null;
            ticksRan = 0;
            leftClickCounter = 0;
            GuiAchievement = new GuiAchievement(this);
            SkipRenderWorld = false;
            PlayerModelBiped = new ModelBiped(0.0F);
            ObjectMouseOver = null;
            SndManager = new SoundManager();
            rightClickDelayTimer = 0;
            textureWaterFX = new TextureWaterFX();
            textureLavaFX = new TextureLavaFX();
            Running = true;
            Debug = "";
            DebugUpdateTime = JavaHelper.CurrentTimeMillis();
            FpsCounter = 0;
            IsTakingScreenshot = false;
            PrevFrameTime = -1L;
            DebugProfilerName = "root";
            InGameHasFocus = false;
            IsRaining = false;
            SystemTime = JavaHelper.CurrentTimeMillis();
            joinPlayerCounter = 0;
            StatList.Init();
            tempDisplayHeight = height;
            Packet3Chat.Field_52010_b = 32767;

            //new ThreadClientSleep(this, "Timer hack thread");

            DisplayWidth = width;
            DisplayHeight = height;
            fullscreen = useFullscreen;
            theMinecraft = this;
            IsMouseVisible = true;
            Content.RootDirectory = "Content";
            Window.ClientSizeChanged += OnResize;
        }
        /*
        public virtual void OnMinecraftCrash(UnexpectedThrowable par1UnexpectedThrowable)
        {
            hasCrashed = true;
            DisplayUnexpectedThrowable(par1UnexpectedThrowable);
        }

        /// <summary>
        /// Displays an unexpected error that has come up during the game.
        /// </summary>
        public abstract void DisplayUnexpectedThrowable(UnexpectedThrowable unexpectedthrowable);
        */
        public virtual void SetServer(string par1Str, int par2)
        {
            serverName = par1Str;
            serverPort = par2;
        }

        /// <summary>
        /// Starts the game: initializes the canvas, the title, the settings, etcetera.
        /// </summary>
        protected override void Initialize()
        {
            OpenGlHelper.InitializeTextures();
            McDataDir = GetMinecraftDir();
            saveLoader = new AnvilSaveConverter(System.IO.Path.Combine(McDataDir, "Saves"));
            GameSettings = new GameSettings(this, McDataDir);
            TexturePackList = new TexturePackList(this, McDataDir);
            RenderEngineOld = new RenderEngineOld(TexturePackList, GameSettings);
            RenderEngine.Instance = new RenderEngine(this);
            RenderEngine = RenderEngine.Instance;

            Input = new InputHelper(this);

            //LoadScreen();

            showSplash = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            FontRendererOld = new FontRendererOld(GameSettings, "font.default.png", RenderEngine, false);
            StandardGalacticFontRenderer = new FontRendererOld(GameSettings, "font.alternate.png", RenderEngine, false);

            if (GameSettings.Language != null)
            {
                StringTranslate.GetInstance().SetLanguage(GameSettings.Language);
                FontRendererOld.SetUnicodeFlag(StringTranslate.GetInstance().IsUnicode());
                FontRendererOld.SetBidiFlag(StringTranslate.IsBidrectional(GameSettings.Language));
            }

            ColorizerWater.SetWaterBiomeColorizer(RenderEngineOld.GetTextureContents("/misc/watercolor.png"));
            ColorizerGrass.SetGrassBiomeColorizer(RenderEngineOld.GetTextureContents("/misc/grasscolor.png"));
            ColorizerFoliage.GetFoilageBiomeColorizer(RenderEngineOld.GetTextureContents("/misc/foliagecolor.png"));

            EntityRenderer = new EntityRenderer(this);
            FontRenderer = new FontRenderer(this);

            RenderManager.Instance.ItemRenderer = new ItemRenderer(this);
            StatFileWriter = new StatFileWriter(Session, McDataDir);
            AchievementList.OpenInventory.StatStringFormatter = new StatStringFormatKeyInv(this);

            //LoadScreen();

            SendSnoopData();

            CheckGLError("Pre startup");/*
            //GL.Enable(EnableCap.Texture2D);
            //GL.ShadeModel(ShadingModel.Smooth);
            //GL.ClearDepth(1.0D);
            //GL.Enable(EnableCap.DepthTest);
            //GL.DepthFunc(DepthFunction.Lequal);
            //GL.Enable(EnableCap.AlphaTest);
            //GL.AlphaFunc(AlphaFunction.Greater, 0.1F);
            //GL.CullFace(CullFaceMode.Back);
            //GL.MatrixMode(MatrixMode.Projection);
            //GL.LoadIdentity();
            //GL.MatrixMode(MatrixMode.Modelview);*/
            CheckGLError("Startup");
            glCapabilities = new OpenGlCapsChecker();
            SndManager.LoadSoundSettings(GameSettings);
            RenderEngineOld.RegisterTextureFX(textureLavaFX);
            RenderEngineOld.RegisterTextureFX(textureWaterFX);
            RenderEngineOld.RegisterTextureFX(new TexturePortalFX());
            RenderEngineOld.RegisterTextureFX(new TextureCompassFX(this));
            RenderEngineOld.RegisterTextureFX(new TextureWatchFX(this));
            RenderEngineOld.RegisterTextureFX(new TextureWaterFlowFX());
            RenderEngineOld.RegisterTextureFX(new TextureLavaFlowFX());
            RenderEngineOld.RegisterTextureFX(new TextureFlamesFX(0));
            RenderEngineOld.RegisterTextureFX(new TextureFlamesFX(1));
            RenderGlobal = new RenderGlobal(this, RenderEngine);
            ////GL.Viewport(0, 0, DisplayWidth, DisplayHeight);
            EffectRenderer = new EffectRenderer(TheWorld, RenderEngineOld);
            
            try
            {
                resourceDownloader = new ResourceDownloader(McDataDir, this);
                resourceDownloader.Run();
            }
            catch (Exception exception1)
            {
                Utilities.LogException(exception1);
            }
            
            CheckGLError("Post startup");
            IngameGUI = new GuiIngame(this);

            if (serverName != null)
            {
                DisplayGuiScreen(new GuiConnecting(this, serverName, serverPort));
            }
            else
            {
                DisplayGuiScreen(new GuiMainMenu());
            }

            LoadingScreen = new LoadingScreenRenderer(this);
        }

        /// <summary>
        /// Displays a new screen.
        /// </summary>
        private void LoadScreen()
        {/*
            ScaledResolution scaledresolution = new ScaledResolution(GameSettings, DisplayWidth, DisplayHeight);
            //GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            //GL.MatrixMode(MatrixMode.Projection);
            //GL.LoadIdentity();
            //GL.Ortho(0.0F, scaledresolution.ScaledWidthD, scaledresolution.ScaledHeightD, 0.0F, 1000D, 3000D);
            //GL.MatrixMode(MatrixMode.Modelview);
            //GL.LoadIdentity();
            //GL.Translate(0.0F, 0.0F, -2000F);
            //GL.Viewport(0, 0, DisplayWidth, DisplayHeight);
            //GL.ClearColor(0.0F, 0.0F, 0.0F, 0.0F);
            Tessellator tessellator = Tessellator.Instance;
            //GL.Disable(EnableCap.Lighting);
            //GL.Enable(EnableCap.Texture2D);
            //GL.Disable(EnableCap.Fog);
            //GL.BindTexture(TextureTarget.Texture2D, RenderEngineOld.GetTexture("Minecraft.Resources.title.mojang.png"));
            tessellator.StartDrawingQuads();
            tessellator.SetColorOpaque_I(0xffffff);
            tessellator.AddVertexWithUV(0.0F, DisplayHeight, 0.0F, 0.0F, 0.0F);
            tessellator.AddVertexWithUV(DisplayWidth, DisplayHeight, 0.0F, 0.0F, 0.0F);
            tessellator.AddVertexWithUV(DisplayWidth, 0.0F, 0.0F, 0.0F, 0.0F);
            tessellator.AddVertexWithUV(0.0F, 0.0F, 0.0F, 0.0F, 0.0F);
            tessellator.Draw();
            int c = 256;
            int c1 = 256;
            //GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
            tessellator.SetColorOpaque_I(0xffffff);
            ScaledTessellator((scaledresolution.GetScaledWidth() - c) / 2, (scaledresolution.GetScaledHeight() - c1) / 2, 0, 0, c, c1);
            //GL.Disable(EnableCap.Lighting);
            //GL.Disable(EnableCap.Fog);
            //GL.Enable(EnableCap.AlphaTest);
            //GL.AlphaFunc(AlphaFunction.Greater, 0.1F);
            SwapBuffers();*/
        }

        /// <summary>
        /// Loads Tessellator with a scaled resolution
        /// </summary>
        public void ScaledTessellator(int par1, int par2, int par3, int par4, int par5, int par6)
        {
            float f = 0.00390625F;
            float f1 = 0.00390625F;
            Tessellator tessellator = Tessellator.Instance;
            tessellator.StartDrawingQuads();
            tessellator.AddVertexWithUV(par1 + 0, par2 + par6, 0.0F, (float)(par3 + 0) * f, (float)(par4 + par6) * f1);
            tessellator.AddVertexWithUV(par1 + par5, par2 + par6, 0.0F, (float)(par3 + par5) * f, (float)(par4 + par6) * f1);
            tessellator.AddVertexWithUV(par1 + par5, par2 + 0, 0.0F, (float)(par3 + par5) * f, (float)(par4 + 0) * f1);
            tessellator.AddVertexWithUV(par1 + 0, par2 + 0, 0.0F, (float)(par3 + 0) * f, (float)(par4 + 0) * f1);
            tessellator.Draw();
        }

        /// <summary>
        /// Returns the save loader that is currently being used
        /// </summary>
        public ISaveFormat GetSaveLoader()
        {
            return saveLoader;
        }

        /// <summary>
        /// Sets the argument GuiScreen as the main (topmost visible) screen.
        /// </summary>
        public void DisplayGuiScreen(GuiScreen par1GuiScreen)
        {
            if (CurrentScreen is GuiErrorScreen)
            {
                return;
            }

            if (CurrentScreen != null)
            {
                CurrentScreen.OnGuiClosed();
            }

            if (par1GuiScreen is GuiMainMenu)
            {
                StatFileWriter.Func_27175_b();
            }

            StatFileWriter.SyncStats();

            if (par1GuiScreen == null && TheWorld == null)
            {
                par1GuiScreen = new GuiMainMenu();
            }
            else if (par1GuiScreen == null && ThePlayer.GetHealth() <= 0)
            {
                par1GuiScreen = new GuiGameOver();
            }

            if (par1GuiScreen is GuiMainMenu)
            {
                GameSettings.ShowDebugInfo = false;
                IngameGUI.ClearChatMessages();
            }

            CurrentScreen = par1GuiScreen;

            if (par1GuiScreen != null)
            {
                SetIngameNotInFocus();
                ScaledResolution scaledresolution = new ScaledResolution(GameSettings, DisplayWidth, DisplayHeight);
                int i = scaledresolution.GetScaledWidth();
                int j = scaledresolution.GetScaledHeight();
                par1GuiScreen.SetWorldAndResolution(this, i, j);
                SkipRenderWorld = false;
            }
            else
            {
                SetIngameFocus();
            }
        }

        /// <summary>
        /// Checks for an OpenGL error. If there is one, prints the error ID and error string.
        /// </summary>
        private void CheckGLError(string par1Str)
        {/*
            ErrorCode errorCode = //GL.GetError();

            if (errorCode != 0)
            {
                Console.WriteLine("########## GL ERROR ##########");
                Console.WriteLine(new StringBuilder().Append("@ ").Append(par1Str).ToString());
                Console.WriteLine(new StringBuilder().Append(errorCode).ToString());
                Console.WriteLine();
            }*/
        }

        /// <summary>
        /// Shuts down the minecraft applet by stopping the resource downloads, and clearing up GL stuff; called when the
        /// application (or web page) is exited.
        /// </summary>
        /*public virtual void ShutdownMinecraftApplet()
        {
            try
            {
                StatFileWriter.Func_27175_b();
                StatFileWriter.SyncStats();

                if (McApplet != null)
                {
                    McApplet.ClearApplet();
                }

                try
                {
                    if (resourceDownloader != null)
                    {
                        resourceDownloader.CloseMinecraft();
                    }
                }
                catch (Exception exception)
                {
                }

                Console.WriteLine("Stopping!");

                try
                {
                    ChangeWorld1(null);
                }
                catch (Exception throwable)
                {
                }

                try
                {
                    GLAllocation.DeleteTexturesAndDisplayLists();
                }
                catch (Exception throwable1)
                {
                }

                SndManager.CloseMinecraft();
                //Mouse.destroy();
                //Keyboard.Destroy();
            }
            finally
            {
                Display.destroy();

                if (!hasCrashed)
                {
                    Environment.Exit(0);
                }
            }

            GC.Collect();
        }
        /*
        public virtual void Run()
        {
            Running = true;

            try
            {
                StartGame();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                Console.Write(exception.StackTrace);
                OnMinecraftCrash(new UnexpectedThrowable("Failed to start game", exception));
                return;
            }

            try
            {
                while (Running)
                {
                    try
                    {
                        RunGameLoop();
                    }
                    catch (MinecraftException minecraftexception)
                    {
                        TheWorld = null;
                        ChangeWorld1(null);
                        DisplayGuiScreen(new GuiConflictWarning());
                    }
                    catch (System.OutOfMemoryException outofmemoryerror)
                    {
                        FreeMemory();
                        DisplayGuiScreen(new GuiMemoryErrorScreen());
                        GC.Collect();
                    }
                }
            }
            catch (MinecraftError minecrafterror)
            {
            }
            catch (Exception throwable)
            {
                FreeMemory();
                Console.WriteLine(throwable.ToString());
                Console.Write(throwable.StackTrace);
                OnMinecraftCrash(new UnexpectedThrowable("Unexpected error", throwable));
            }
            finally
            {
                ShutdownMinecraftApplet();
            }
        }*/
        
        /// <summary>
        /// Called repeatedly from run()
        /// </summary>
        protected override void  Update(GameTime gameTime)
        {
 	         base.Update(gameTime);

            AxisAlignedBB.ClearBoundingBoxPool();
            Vec3D.Initialize();

            Profiler.StartSection("root");

            UpdateInput(gameTime);

            if (IsGamePaused && TheWorld != null)
            {
                float f = timer.RenderPartialTicks;
                timer.UpdateTimer();
                timer.RenderPartialTicks = f;
            }
            else
            {
                timer.UpdateTimer();
            }

            Profiler.StartSection("tick");

            for (int i = 0; i < timer.ElapsedTicks; i++)
            {
                ticksRan++;

                try
                {
                    RunTick();
                    continue;
                }
                catch (MinecraftException minecraftexception)
                {
                    Utilities.LogException(minecraftexception);

                    TheWorld = null;
                }

                ChangeWorld1(null);
                DisplayGuiScreen(new GuiConflictWarning());
            }

            Profiler.EndSection();

            CheckGLError("Pre render");
            RenderBlocks.FancyGrass = GameSettings.FancyGraphics;

            Profiler.StartSection("sound");
            SndManager.SetListener(ThePlayer, timer.RenderPartialTicks);
            Profiler.EndStartSection("updatelights");

            if (TheWorld != null)
            {
                TheWorld.UpdatingLighting();
            }

            Profiler.EndSection();
        }

        private void UpdateInput(GameTime gameTime)
        {
            Input.Update(gameTime);

            if (CurrentScreen == null && ThePlayer != null)
            {
                if (ThePlayer.GetHealth() <= 0)
                {
                    DisplayGuiScreen(null);
                }
                else if (ThePlayer.IsPlayerSleeping() && TheWorld != null && TheWorld.IsRemote)
                {
                    DisplayGuiScreen(new GuiSleepMP());
                }
            }
            else if (CurrentScreen != null && (CurrentScreen is GuiSleepMP) && !ThePlayer.IsPlayerSleeping())
            {
                DisplayGuiScreen(null);
            }

            if (CurrentScreen != null)
            {
                leftClickCounter = 10000;
            }

            if (CurrentScreen != null)
            {
                CurrentScreen.HandleInput();

                if (CurrentScreen != null)
                {
                    CurrentScreen.GuiParticles.Update();
                    CurrentScreen.UpdateScreen();
                }
            }

            if (CurrentScreen == null || CurrentScreen.AllowUserInput)
            {
                Profiler.EndStartSection("mouse");

                do
                {/*
                    if (!Mouse.next())
                    {
                        break;
                    }
                    
                    KeyBinding.SetKeyBindState(Mouse.EventButton - 100, Mouse.EventButtonState);
                    
                    if (Mouse.EventButtonState)
                    {
                        KeyBinding.OnTick(Mouse.EventButton - 100);
                    }
                    */
                    long l = JavaHelper.CurrentTimeMillis() - SystemTime;

                    if (l <= 200L)
                    {
                        int i1 = Input.Mouse.WheelDelta;

                        if (i1 != 0)
                        {
                            ThePlayer.Inventory.ChangeCurrentItem(i1);

                            if (GameSettings.Noclip)
                            {
                                if (i1 > 0)
                                {
                                    i1 = 1;
                                }

                                if (i1 < 0)
                                {
                                    i1 = -1;
                                }

                                GameSettings.NoclipRate += (float)i1 * 0.25F;
                            }
                        }

                        if (CurrentScreen == null)
                        {
                            if (!InGameHasFocus)// && Mouse.EventButtonState)
                            {
                                SetIngameFocus();
                            }
                        }
                        else if (CurrentScreen != null)
                        {
                            CurrentScreen.HandleMouseInput();
                        }
                    }
                }
                while (true);

                if (leftClickCounter > 0)
                {
                    leftClickCounter--;
                }

                Profiler.EndStartSection("keyboard");
                /*
                do
                {
                    if (!Keyboard.next())
                    {
                        break;
                    }

                    KeyBinding.SetKeyBindState(Keyboard.EventKey, Keyboard.EventKeyState);

                    if (Keyboard.EventKeyState)
                    {
                        KeyBinding.OnTick(Keyboard.EventKey);
                    }
                    
                    if (Keyboard.EventKeyState)
                    {
                        if (Keyboard.EventKey == 87)
                        {
                            ToggleFullscreen();
                        }
                        else
                        {
                            if (CurrentScreen != null)
                            {
                                CurrentScreen.HandleKeyboardInput();
                            }
                            else
                            {
                                if (Keyboard.EventKey == 1)
                                {
                                    DisplayInGameMenu();
                                }

                                if (Keyboard.EventKey == 31 && currentKeyboardState.IsKeyDown((Key)61))
                                {
                                    ForceReload();
                                }

                                if (Keyboard.EventKey == 20 && currentKeyboardState.IsKeyDown((Key)61))
                                {
                                    RenderEngine.refreshTextures();
                                }

                                if (Keyboard.EventKey == 33 && currentKeyboardState.IsKeyDown((Key)61))
                                {
                                    bool flag = currentKeyboardState.IsKeyDown((Key)42) | currentKeyboardState.IsKeyDown((Key)54);
                                    GameSettings.SetOptionValue(Options.RENDER_DISTANCE, flag ? -1 : 1);
                                }

                                if (Keyboard.EventKey == 30 && currentKeyboardState.IsKeyDown((Key)61))
                                {
                                    RenderGlobal.LoadRenderers();
                                }

                                if (Keyboard.EventKey == 59)
                                {
                                    GameSettings.HideGUI = !GameSettings.HideGUI;
                                }

                                if (Keyboard.EventKey == 61)
                                {
                                    GameSettings.ShowDebugInfo = !GameSettings.ShowDebugInfo;
                                    GameSettings.Field_50119_G = !GuiScreen.Func_50049_m();
                                }

                                if (Keyboard.EventKey == 63)
                                {
                                    GameSettings.ThirdPersonView++;

                                    if (GameSettings.ThirdPersonView > 2)
                                    {
                                        GameSettings.ThirdPersonView = 0;
                                    }
                                }

                                if (Keyboard.EventKey == 66)
                                {
                                    GameSettings.SmoothCamera = !GameSettings.SmoothCamera;
                                }
                            }

                            for (int i = 0; i < 9; i++)
                            {
                                if (Keyboard.EventKey == 2 + i)
                                {
                                    ThePlayer.Inventory.CurrentItem = i;
                                }
                            }

                            if (GameSettings.ShowDebugInfo && GameSettings.Field_50119_G)
                            {
                                if (Keyboard.EventKey == 11)
                                {
                                    UpdateDebugProfilerName(0);
                                }

                                int j = 0;

                                while (j < 9)
                                {
                                    if (Keyboard.EventKey == 2 + j)
                                    {
                                        UpdateDebugProfilerName(j + 1);
                                    }

                                    j++;
                                }
                            }
                        }
                    }
                }
                while (true);
                */
                for (; GameSettings.KeyBindInventory.Pressed; DisplayGuiScreen(new GuiInventory(ThePlayer)))
                {
                }

                for (; GameSettings.KeyBindDrop.Pressed; ThePlayer.DropOneItem())
                {
                }

                for (; IsMultiplayerWorld() && GameSettings.KeyBindChat.Pressed; DisplayGuiScreen(new GuiChat()))
                {
                }

                if (IsMultiplayerWorld() && CurrentScreen == null && (Input.IsKeyDown((Keys)53) || Input.IsKeyDown((Keys)181)))
                {
                    DisplayGuiScreen(new GuiChat("/"));
                }

                if (ThePlayer.IsUsingItem())
                {
                    if (!GameSettings.KeyBindUseItem.Pressed)
                    {
                        PlayerController.OnStoppedUsingItem(ThePlayer);
                    }

                    while (GameSettings.KeyBindAttack.Pressed) ;

                    while (GameSettings.KeyBindUseItem.Pressed) ;

                    while (GameSettings.KeyBindPickBlock.Pressed) ;
                }
                else
                {
                    for (; GameSettings.KeyBindAttack.Pressed; ClickMouse(0))
                    {
                    }

                    for (; GameSettings.KeyBindUseItem.Pressed; ClickMouse(1))
                    {
                    }

                    for (; GameSettings.KeyBindPickBlock.Pressed; ClickMiddleMouseButton())
                    {
                    }
                }

                if (GameSettings.KeyBindUseItem.Pressed && rightClickDelayTimer == 0 && !ThePlayer.IsUsingItem())
                {
                    ClickMouse(1);
                }

                SendClickBlockToController(0, CurrentScreen == null && GameSettings.KeyBindAttack.Pressed && InGameHasFocus);
            }
        }

        /// <summary>
        /// Do all rendering for the current frame.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            Profiler.StartSection("render");
            Profiler.StartSection("display");
            ////GL.Enable(EnableCap.Texture2D);
            /*
            if (!currentKeyboardState.IsKeyDown((Key)65))
            {
                Display.Update();
            }
            */
            if (showSplash && gameTime.TotalGameTime.TotalSeconds < 3)
            {
                RenderSplashScreen();
                return;
            }

            RenderEngine.StartDrawing();

            if (ThePlayer != null && ThePlayer.IsEntityInsideOpaqueBlock())
            {
                GameSettings.ThirdPersonView = 0;
            }

            Profiler.EndSection();

            if (!SkipRenderWorld)
            {
                Profiler.StartSection("gameMode");

                if (PlayerController != null)
                {
                    PlayerController.SetPartialTime(timer.RenderPartialTicks);
                }

                Profiler.EndStartSection("gameRenderer");
                EntityRenderer.UpdateCameraAndRender(timer.RenderPartialTicks);
                Profiler.EndSection();
            }

            ////GL.Flush();
            Profiler.EndSection();
            
            if (!IsActive && fullscreen)
            {
                ToggleFullscreen();
            }
            
            RenderEngine.StopDrawing();

            Profiler.EndSection();

            long l = JavaHelper.NanoTime();
            long l1 = JavaHelper.NanoTime() - l;
            if (GameSettings.ShowDebugInfo && GameSettings.Field_50119_G)
            {
                if (!Profiler.ProfilingEnabled)
                {
                    Profiler.ClearProfiling();
                }

                Profiler.ProfilingEnabled = true;
                DisplayDebugInfo(l1);
            }
            else
            {
                Profiler.ProfilingEnabled = false;
                PrevFrameTime = JavaHelper.NanoTime();
            }

            GuiAchievement.UpdateAchievementWindow();

            Profiler.StartSection("root");
            Thread.Yield();
            /*
            if (currentKeyboardState.IsKeyDown((Key)65))
            {
                Display.update();
            }
            */
            ScreenshotListener();

            CheckGLError("Post render");
            FpsCounter++;
            IsGamePaused = !IsMultiplayerWorld() && CurrentScreen != null && CurrentScreen.DoesGuiPauseGame();

            while (JavaHelper.CurrentTimeMillis() >= DebugUpdateTime + 1000L)
            {
                Debug = (new StringBuilder()).Append(FpsCounter).Append(" fps, ").Append(WorldRenderer.ChunksUpdated).Append(" chunk updates").ToString();
                WorldRenderer.ChunksUpdated = 0;
                DebugUpdateTime += 1000L;
                FpsCounter = 0;
            }

            Profiler.EndSection();
        }

        private void RenderSplashScreen()
        {/*
            ScaledResolution scaledresolution = new ScaledResolution(GameSettings, DisplayWidth, DisplayHeight);
            //GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            //GL.MatrixMode(MatrixMode.Projection);
            //GL.LoadIdentity();
            //GL.Ortho(0.0F, scaledresolution.ScaledWidthD, scaledresolution.ScaledHeightD, 0.0F, 1000D, 3000D);
            //GL.MatrixMode(MatrixMode.Modelview);
            //GL.LoadIdentity();
            //GL.Translate(0.0F, 0.0F, -2000F);
            //GL.Viewport(0, 0, DisplayWidth, DisplayHeight);
            //GL.ClearColor(0.0F, 0.0F, 0.0F, 0.0F);
            Tessellator tessellator = Tessellator.Instance;
            //GL.Disable(EnableCap.Lighting);
            //GL.Enable(EnableCap.Texture2D);
            //GL.Disable(EnableCap.Fog);
            //GL.BindTexture(TextureTarget.Texture2D, RenderEngineOld.GetTexture("Minecraft.Resources.title.mojang.png"));
            tessellator.StartDrawingQuads();
            tessellator.SetColorOpaque_I(0xffffff);
            tessellator.AddVertexWithUV(0.0F, DisplayHeight, 0.0F, 0.0F, 0.0F);
            tessellator.AddVertexWithUV(DisplayWidth, DisplayHeight, 0.0F, 0.0F, 0.0F);
            tessellator.AddVertexWithUV(DisplayWidth, 0.0F, 0.0F, 0.0F, 0.0F);
            tessellator.AddVertexWithUV(0.0F, 0.0F, 0.0F, 0.0F, 0.0F);
            tessellator.Draw();
            int c = 256;
            int c1 = 256;
            //GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
            tessellator.SetColorOpaque_I(0xffffff);
            ScaledTessellator((scaledresolution.GetScaledWidth() - c) / 2, (scaledresolution.GetScaledHeight() - c1) / 2, 0, 0, c, c1);
            //GL.Disable(EnableCap.Lighting);
            //GL.Disable(EnableCap.Fog);
            //GL.Enable(EnableCap.AlphaTest);
            //GL.AlphaFunc(AlphaFunction.Greater, 0.1F);
            SwapBuffers();*/

            RenderEngine.StartDrawing(Color.White);
            //RenderEngine.RenderSprite("title.mojang.png", new Rectangle((GraphicsDevice.Viewport.Bounds.X - 256) / 2, (GraphicsDevice.Viewport.Bounds.Y - 256) / 2, 256, 256));
            RenderEngine.RenderSprite("title.mojang.png", RenderEngine.GetTexture("title.mojang.png").Bounds.FitToRectangle(GraphicsDevice.Viewport.Bounds));
            RenderEngine.StopDrawing();
        }

        private void OnResize(object sender, EventArgs e)
        {
            DisplayWidth = Window.ClientBounds.Width;
            DisplayHeight = Window.ClientBounds.Height;

            if (DisplayWidth <= 0)
            {
                DisplayWidth = 1;
            }

            if (DisplayHeight <= 0)
            {
                DisplayHeight = 1;
            }

            Window.Title = new StringBuilder("Minecraft ").Append(GetVersion()).Append("Width: ").Append(DisplayWidth).Append(", Height: ").Append(DisplayHeight).ToString();

            ResizeScreen(DisplayWidth, DisplayHeight);
        }

        public void FreeMemory()
        {
            try
            {
                RenderGlobal.Func_28137_f();
            }
            catch (Exception throwable)
            {
                Console.WriteLine(throwable.ToString());
                Console.WriteLine();
            }

            try
            {
                GC.Collect();
                AxisAlignedBB.ClearBoundingBoxes();
                Vec3D.ClearVectorList();
            }
            catch (Exception throwable1)
            {
                Console.WriteLine(throwable1.ToString());
                Console.WriteLine();
            }

            try
            {
                GC.Collect();
                ChangeWorld1(null);
            }
            catch (Exception throwable2)
            {
                Console.WriteLine(throwable2.ToString());
                Console.WriteLine();
            }

            GC.Collect();
        }

        /// <summary>
        /// checks if keys are down
        /// </summary>
        private void ScreenshotListener()
        {
            if (Input.IsKeyDown(Keys.F12))
            {
                if (!IsTakingScreenshot)
                {
                    IsTakingScreenshot = true;
                    IngameGUI.AddChatMessage(ScreenShotHelper.SaveScreenshot(minecraftDir, DisplayWidth, DisplayHeight));
                }
            }
            else
            {
                IsTakingScreenshot = false;
            }
        }

        /// <summary>
        /// Update debugProfilerName in response to number keys in debug screen
        /// </summary>
        private void UpdateDebugProfilerName(int par1)
        {
            List<ProfilerResult> list = Profiler.GetProfilingData(DebugProfilerName);
            ProfilerResult profilerresult;

            if (list == null || list.Count == 0)
            {
                return;
            }

            profilerresult = list[0];
            list.RemoveAt(0);

            if (!(par1 != 0))
            {
                if (profilerresult.Name.Length > 0)
                {
                    int i = DebugProfilerName.LastIndexOf(".");

                    if (i >= 0)
                    {
                        DebugProfilerName = DebugProfilerName.Substring(0, i);
                    }
                }
            }
            else if (!(--par1 >= list.Count || list[par1].Name.Equals("unspecified")))
            {
                if (!(DebugProfilerName.Length <= 0))
                {
                    DebugProfilerName += ".";
                }

                DebugProfilerName += list[par1].Name;
            }
        }

        private void DisplayDebugInfo(long par1)
        {
            List<ProfilerResult> list = Profiler.GetProfilingData(DebugProfilerName);
            ProfilerResult profilerresult = list[0];
            list.RemoveAt(0);
            long l = 0xfe502aL;

            if (PrevFrameTime == -1L)
            {
                PrevFrameTime = JavaHelper.NanoTime();
            }

            long l1 = JavaHelper.NanoTime();
            TickTimes[NumRecordedFrameTimes & FrameTimes.Length - 1] = par1;
            FrameTimes[NumRecordedFrameTimes++ & FrameTimes.Length - 1] = l1 - PrevFrameTime;
            PrevFrameTime = l1;/*
            //GL.Clear(ClearBufferMask.ColorBufferBit);
            //GL.MatrixMode(MatrixMode.Projection);
            //GL.Enable(EnableCap.ColorMaterial);
            //GL.LoadIdentity();
            //GL.Ortho(0.0F, DisplayWidth, DisplayHeight, 0.0F, 1000D, 3000D);
            //GL.MatrixMode(MatrixMode.Modelview);
            //GL.LoadIdentity();
            //GL.Translate(0.0F, 0.0F, -2000F);
            //GL.LineWidth(1.0F);
            //GL.Disable(EnableCap.Texture2D);*/
            Tessellator tessellator = Tessellator.Instance;
            tessellator.StartDrawing(7);
            int i = (int)(l / 0x30d40L);
            tessellator.SetColorOpaque_I(0x20000000);
            tessellator.AddVertex(0.0F, DisplayHeight - i, 0.0F);
            tessellator.AddVertex(0.0F, DisplayHeight, 0.0F);
            tessellator.AddVertex(FrameTimes.Length, DisplayHeight, 0.0F);
            tessellator.AddVertex(FrameTimes.Length, DisplayHeight - i, 0.0F);
            tessellator.SetColorOpaque_I(0x20200000);
            tessellator.AddVertex(0.0F, DisplayHeight - i * 2, 0.0F);
            tessellator.AddVertex(0.0F, DisplayHeight - i, 0.0F);
            tessellator.AddVertex(FrameTimes.Length, DisplayHeight - i, 0.0F);
            tessellator.AddVertex(FrameTimes.Length, DisplayHeight - i * 2, 0.0F);
            tessellator.Draw();
            long l2 = 0L;

            for (int j = 0; j < FrameTimes.Length; j++)
            {
                l2 += FrameTimes[j];
            }

            int k = (int)(l2 / 0x30d40L / (long)FrameTimes.Length);
            tessellator.StartDrawing(7);
            tessellator.SetColorOpaque_I(0x20400000);
            tessellator.AddVertex(0.0F, DisplayHeight - k, 0.0F);
            tessellator.AddVertex(0.0F, DisplayHeight, 0.0F);
            tessellator.AddVertex(FrameTimes.Length, DisplayHeight, 0.0F);
            tessellator.AddVertex(FrameTimes.Length, DisplayHeight - k, 0.0F);
            tessellator.Draw();
            tessellator.StartDrawing(1);

            for (int i1 = 0; i1 < FrameTimes.Length; i1++)
            {
                int k1 = ((i1 - NumRecordedFrameTimes & FrameTimes.Length - 1) * 255) / FrameTimes.Length;
                int j2 = (k1 * k1) / 255;
                j2 = (j2 * j2) / 255;
                int i3 = (j2 * j2) / 255;
                i3 = (i3 * i3) / 255;

                if (FrameTimes[i1] > l)
                {
                    tessellator.SetColorOpaque_I(0xff00000 + j2 * 0x10000);
                }
                else
                {
                    tessellator.SetColorOpaque_I(0xff00000 + j2 * 256);
                }

                long l3 = FrameTimes[i1] / 0x30d40L;
                long l4 = TickTimes[i1] / 0x30d40L;
                tessellator.AddVertex((float)i1 + 0.5F, (float)((long)DisplayHeight - l3) + 0.5F, 0.0F);
                tessellator.AddVertex((float)i1 + 0.5F, (float)DisplayHeight + 0.5F, 0.0F);
                tessellator.SetColorOpaque_I(0xff00000 + j2 * 0x10000 + j2 * 256 + j2 * 1);
                tessellator.AddVertex((float)i1 + 0.5F, (float)((long)DisplayHeight - l3) + 0.5F, 0.0F);
                tessellator.AddVertex((float)i1 + 0.5F, (float)((long)DisplayHeight - (l3 - l4)) + 0.5F, 0.0F);
            }

            tessellator.Draw();
            int j1 = 160;
            int i2 = DisplayWidth - j1 - 10;
            int k2 = DisplayHeight - j1 * 2;
            ////GL.Enable(EnableCap.Blend);
            tessellator.StartDrawingQuads();
            tessellator.SetColorRGBA_I(0, 200);
            tessellator.AddVertex((float)i2 - (float)j1 * 1.1F, (float)k2 - (float)j1 * 0.6F - 16F, 0.0F);
            tessellator.AddVertex((float)i2 - (float)j1 * 1.1F, k2 + j1 * 2, 0.0F);
            tessellator.AddVertex((float)i2 + (float)j1 * 1.1F, k2 + j1 * 2, 0.0F);
            tessellator.AddVertex((float)i2 + (float)j1 * 1.1F, (float)k2 - (float)j1 * 0.6F - 16F, 0.0F);
            tessellator.Draw();
            ////GL.Disable(EnableCap.Blend);
            double d = 0.0F;

            for (int j3 = 0; j3 < list.Count; j3++)
            {
                ProfilerResult profilerresult1 = list[j3];
                int i4 = MathHelper2.Floor_double(profilerresult1.SectionPercentage / 4D) + 1;
                tessellator.StartDrawing(6);
                tessellator.SetColorOpaque_I(profilerresult1.GetDisplayColor());
                tessellator.AddVertex(i2, k2, 0.0F);

                for (int k4 = i4; k4 >= 0; k4--)
                {
                    float f = (float)(((d + (profilerresult1.SectionPercentage * (double)k4) / (double)i4) * Math.PI * 2D) / 100D);
                    float f2 = MathHelper2.Sin(f) * (float)j1;
                    float f4 = MathHelper2.Cos(f) * (float)j1 * 0.5F;
                    tessellator.AddVertex((float)i2 + f2, (float)k2 - f4, 0.0F);
                }

                tessellator.Draw();
                tessellator.StartDrawing(5);
                tessellator.SetColorOpaque_I((profilerresult1.GetDisplayColor() & 0xfefefe) >> 1);

                for (int i5 = i4; i5 >= 0; i5--)
                {
                    float f1 = (float)(((d + (profilerresult1.SectionPercentage * (double)i5) / (double)i4) * Math.PI * 2D) / 100D);
                    float f3 = MathHelper2.Sin(f1) * (float)j1;
                    float f5 = MathHelper2.Cos(f1) * (float)j1 * 0.5F;
                    tessellator.AddVertex((float)i2 + f3, (float)k2 - f5, 0.0F);
                    tessellator.AddVertex((float)i2 + f3, ((float)k2 - f5) + 10F, 0.0F);
                }

                tessellator.Draw();
                d += profilerresult1.SectionPercentage;
            }

            NumberFormatInfo decimalformat = new NumberFormatInfo();
            decimalformat.NumberDecimalSeparator = ".";
            decimalformat.NumberDecimalDigits = 2;
            ////GL.Enable(EnableCap.Texture2D);
            string s = "";

            if (!profilerresult.Name.Equals("unspecified"))
            {
                s = (new StringBuilder()).Append(s).Append("[0] ").ToString();
            }

            if (profilerresult.Name.Length == 0)
            {
                s = (new StringBuilder()).Append(s).Append("ROOT ").ToString();
            }
            else
            {
                s = (new StringBuilder()).Append(s).Append(profilerresult.Name).Append(" ").ToString();
            }

            int j4 = 0xffffff;
            FontRendererOld.DrawStringWithShadow(s, i2 - j1, k2 - j1 / 2 - 16, j4);
            FontRendererOld.DrawStringWithShadow(s = (new StringBuilder()).Append(profilerresult.GlobalPercentage.ToString(decimalformat)).Append("%").ToString(), (i2 + j1) - FontRendererOld.GetStringWidth(s), k2 - j1 / 2 - 16, j4);

            for (int k3 = 0; k3 < list.Count; k3++)
            {
                ProfilerResult profilerresult2 = list[k3];
                string s1 = "";

                if (!profilerresult2.Name.Equals("unspecified"))
                {
                    s1 = (new StringBuilder()).Append(s1).Append("[").Append(k3 + 1).Append("] ").ToString();
                }
                else
                {
                    s1 = (new StringBuilder()).Append(s1).Append("[?] ").ToString();
                }

                s1 = (new StringBuilder()).Append(s1).Append(profilerresult2.Name).ToString();
                FontRendererOld.DrawStringWithShadow(s1, i2 - j1, k2 + j1 / 2 + k3 * 8 + 20, profilerresult2.GetDisplayColor());
                FontRendererOld.DrawStringWithShadow(s1 = (new StringBuilder()).Append(profilerresult2.SectionPercentage.ToString(decimalformat)).Append("%").ToString(), (i2 + j1) - 50 - FontRendererOld.GetStringWidth(s1), k2 + j1 / 2 + k3 * 8 + 20, profilerresult2.GetDisplayColor());
                FontRendererOld.DrawStringWithShadow(s1 = (new StringBuilder()).Append(profilerresult2.GlobalPercentage.ToString(decimalformat)).Append("%").ToString(), (i2 + j1) - FontRendererOld.GetStringWidth(s1), k2 + j1 / 2 + k3 * 8 + 20, profilerresult2.GetDisplayColor());
            }
        }

        /// <summary>
        /// Called when the window is closing. Sets 'running' to false which allows the game loop to exit cleanly.
        /// </summary>
        public void Shutdown()
        {
            Running = false;

            Exit();
        }

        /// <summary>
        /// Will set the focus to ingame if the Minecraft window is the active with focus. Also clears any GUI screen
        /// currently displayed
        /// </summary>
        public void SetIngameFocus()
        {
            if (!IsActive)
            {
                return;
            }

            if (InGameHasFocus)
            {
                return;
            }
            else
            {
                InGameHasFocus = true;
                //MouseHelper.GrabMouseCursor();
                DisplayGuiScreen(null);
                leftClickCounter = 10000;
                return;
            }
        }

        /// <summary>
        /// Resets the player keystate, disables the ingame focus, and ungrabs the mouse cursor.
        /// </summary>
        public void SetIngameNotInFocus()
        {
            if (!InGameHasFocus)
            {
                return;
            }
            else
            {
                KeyBinding.UnPressAllKeys();
                InGameHasFocus = false;
                //MouseHelper.UngrabMouseCursor();
                return;
            }
        }

        /// <summary>
        /// Displays the ingame menu
        /// </summary>
        public void DisplayInGameMenu()
        {
            if (CurrentScreen != null)
            {
                return;
            }
            else
            {
                DisplayGuiScreen(new GuiIngameMenu());
                return;
            }
        }

        private void SendClickBlockToController(int par1, bool par2)
        {
            if (!par2)
            {
                leftClickCounter = 0;
            }

            if (par1 == 0 && leftClickCounter > 0)
            {
                return;
            }

            if (par2 && ObjectMouseOver != null && ObjectMouseOver.TypeOfHit == EnumMovingObjectType.TILE && par1 == 0)
            {
                int i = ObjectMouseOver.BlockX;
                int j = ObjectMouseOver.BlockY;
                int k = ObjectMouseOver.BlockZ;
                PlayerController.OnPlayerDamageBlock(i, j, k, ObjectMouseOver.SideHit);

                if (ThePlayer.CanPlayerEdit(i, j, k))
                {
                    EffectRenderer.AddBlockHitEffects(i, j, k, ObjectMouseOver.SideHit);
                    ThePlayer.SwingItem();
                }
            }
            else
            {
                PlayerController.ResetBlockRemoving();
            }
        }

        /// <summary>
        /// Called whenever the mouse is clicked. Button clicked is 0 for left clicking and 1 for right clicking. Args:
        /// buttonClicked
        /// </summary>
        private void ClickMouse(int par1)
        {
            if (par1 == 0 && leftClickCounter > 0)
            {
                return;
            }

            if (par1 == 0)
            {
                ThePlayer.SwingItem();
            }

            if (par1 == 1)
            {
                rightClickDelayTimer = 4;
            }

            bool flag = true;
            ItemStack itemstack = ThePlayer.Inventory.GetCurrentItem();

            if (ObjectMouseOver == null)
            {
                if (par1 == 0 && PlayerController.IsNotCreative())
                {
                    leftClickCounter = 10;
                }
            }
            else if (ObjectMouseOver.TypeOfHit == EnumMovingObjectType.ENTITY)
            {
                if (par1 == 0)
                {
                    PlayerController.AttackEntity(ThePlayer, ObjectMouseOver.EntityHit);
                }

                if (par1 == 1)
                {
                    PlayerController.InteractWithEntity(ThePlayer, ObjectMouseOver.EntityHit);
                }
            }
            else if (ObjectMouseOver.TypeOfHit == EnumMovingObjectType.TILE)
            {
                int i = ObjectMouseOver.BlockX;
                int j = ObjectMouseOver.BlockY;
                int k = ObjectMouseOver.BlockZ;
                int l = ObjectMouseOver.SideHit;

                if (par1 == 0)
                {
                    PlayerController.ClickBlock(i, j, k, ObjectMouseOver.SideHit);
                }
                else
                {
                    ItemStack itemstack2 = itemstack;
                    int i1 = itemstack2 == null ? 0 : itemstack2.StackSize;

                    if (PlayerController.OnPlayerRightClick(ThePlayer, TheWorld, itemstack2, i, j, k, l))
                    {
                        flag = false;
                        ThePlayer.SwingItem();
                    }

                    if (itemstack2 == null)
                    {
                        return;
                    }

                    if (itemstack2.StackSize == 0)
                    {
                        ThePlayer.Inventory.MainInventory[ThePlayer.Inventory.CurrentItem] = null;
                    }
                    else if (itemstack2.StackSize != i1 || PlayerController.IsInCreativeMode())
                    {
                        EntityRenderer.ItemRenderer.Func_9449_b();
                    }
                }
            }

            if (flag && par1 == 1)
            {
                ItemStack itemstack1 = ThePlayer.Inventory.GetCurrentItem();

                if (itemstack1 != null && PlayerController.SendUseItem(ThePlayer, TheWorld, itemstack1))
                {
                    EntityRenderer.ItemRenderer.Func_9450_c();
                }
            }
        }

        /// <summary>
        /// Toggles fullscreen mode.
        /// </summary>
        public void ToggleFullscreen()
        {
            try
            {
                fullscreen = !fullscreen;

                graphics.IsFullScreen = fullscreen;
                graphics.ApplyChanges();

                DisplayWidth = Window.ClientBounds.Width;
                DisplayHeight = Window.ClientBounds.Height;

                if (DisplayWidth <= 0)
                {
                    DisplayWidth = 1;
                }

                if (DisplayHeight <= 0)
                {
                    DisplayHeight = 1;
                }

                if (CurrentScreen != null)
                {
                    ResizeScreen(DisplayWidth, DisplayHeight);
                }

                //Display.Update();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                Console.Write(exception.StackTrace);
            }
        }

        /// <summary>
        /// Called to resize the current screen.
        /// </summary>
        private void ResizeScreen(int par1, int par2)
        {
            if (par1 <= 0)
            {
                par1 = 1;
            }

            if (par2 <= 0)
            {
                par2 = 1;
            }

            DisplayWidth = par1;
            DisplayHeight = par2;

            if (CurrentScreen != null)
            {
                ScaledResolution scaledresolution = new ScaledResolution(GameSettings, par1, par2);
                int i = scaledresolution.GetScaledWidth();
                int j = scaledresolution.GetScaledHeight();
                CurrentScreen.SetWorldAndResolution(this, i, j);
            }
        }

        private void StartThreadCheckHasPaid()
        {
            Action checkPaidDelegate = () =>
            {
                try
                {
                    string uri = new StringBuilder().Append("https://login.minecraft.net/session?name=").Append(Session.Username).Append("&session=").Append(Session.SessionId).ToString();
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                    var response = (HttpWebResponse)request.GetResponse();

                    if (response.StatusCode == HttpStatusCode.NotFound && this == null)
                    {
                        Minecraft.HasPaidCheckTime = JavaHelper.CurrentTimeMillis();
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.ToString());
                    Console.Write(exception.StackTrace);
                }
            };

            var thread = new Thread(new ThreadStart(checkPaidDelegate));
            thread.Start();
        }

        /// <summary>
        /// Runs the current tick.
        /// </summary>
        public void RunTick()
        {
            if (rightClickDelayTimer > 0)
            {
                rightClickDelayTimer--;
            }

            if (ticksRan == 6000)
            {
                StartThreadCheckHasPaid();
            }

            Profiler.StartSection("stats");
            StatFileWriter.Func_27178_d();
            Profiler.EndStartSection("gui");

            if (!IsGamePaused)
            {
                IngameGUI.UpdateTick();
            }

            Profiler.EndStartSection("pick");
            EntityRenderer.GetMouseOver(1.0F);
            Profiler.EndStartSection("centerChunkSource");

            if (ThePlayer != null)
            {
                IChunkProvider ichunkprovider = TheWorld.GetChunkProvider();

                if (ichunkprovider is ChunkProviderLoadOrGenerate)
                {
                    ChunkProviderLoadOrGenerate chunkproviderloadorgenerate = (ChunkProviderLoadOrGenerate)ichunkprovider;
                    int k = MathHelper2.Floor_float((int)ThePlayer.PosX) >> 4;
                    int j1 = MathHelper2.Floor_float((int)ThePlayer.PosZ) >> 4;
                    chunkproviderloadorgenerate.SetCurrentChunkOver(k, j1);
                }
            }

            Profiler.EndStartSection("gameMode");

            if (!IsGamePaused && TheWorld != null)
            {
                PlayerController.UpdateController();
            }

            ////GL.BindTexture(TextureTarget.Texture2D, RenderEngineOld.GetTexture("/terrain.png"));
            RenderEngine.BindTexture("terrain.png");
            Profiler.EndStartSection("textures");

            if (!IsGamePaused)
            {
                RenderEngineOld.UpdateDynamicTextures();
            }

            if (TheWorld != null)
            {
                if (ThePlayer != null)
                {
                    joinPlayerCounter++;

                    if (joinPlayerCounter == 30)
                    {
                        joinPlayerCounter = 0;
                        TheWorld.JoinEntityInSurroundings(ThePlayer);
                    }
                }

                if (TheWorld.GetWorldInfo().IsHardcoreModeEnabled())
                {
                    TheWorld.DifficultySetting = 3;
                }
                else
                {
                    TheWorld.DifficultySetting = GameSettings.Difficulty;
                }

                if (TheWorld.IsRemote)
                {
                    TheWorld.DifficultySetting = 1;
                }

                Profiler.EndStartSection("gameRenderer");

                if (!IsGamePaused)
                {
                    EntityRenderer.UpdateRenderer();
                }

                Profiler.EndStartSection("levelRenderer");

                if (!IsGamePaused)
                {
                    RenderGlobal.UpdateClouds();
                }

                Profiler.EndStartSection("level");

                if (!IsGamePaused)
                {
                    if (TheWorld.LightningFlash > 0)
                    {
                        TheWorld.LightningFlash--;
                    }

                    TheWorld.UpdateEntities();
                }

                if (!IsGamePaused || IsMultiplayerWorld())
                {
                    TheWorld.SetAllowedSpawnTypes(TheWorld.DifficultySetting > 0, true);
                    TheWorld.Tick();
                }

                Profiler.EndStartSection("animateTick");

                if (!IsGamePaused && TheWorld != null)
                {
                    TheWorld.RandomDisplayUpdates(MathHelper2.Floor_double(ThePlayer.PosX), MathHelper2.Floor_double(ThePlayer.PosY), MathHelper2.Floor_double(ThePlayer.PosZ));
                }

                Profiler.EndStartSection("particles");

                if (!IsGamePaused)
                {
                    EffectRenderer.UpdateEffects();
                }
            }

            Profiler.EndSection();
            SystemTime = JavaHelper.CurrentTimeMillis();
        }

        /// <summary>
        /// Forces a reload of the sound manager and all the resources. Called in game by holding 'F3' and pressing 'S'.
        /// </summary>
        private void ForceReload()
        {
            Console.WriteLine("FORCING RELOAD!");
            SndManager = new SoundManager();
            SndManager.LoadSoundSettings(GameSettings);
            resourceDownloader.ReloadResources();
        }

        /// <summary>
        /// Checks if the current world is a multiplayer world, returns true if it is, false otherwise.
        /// </summary>
        public bool IsMultiplayerWorld()
        {
            return TheWorld != null && TheWorld.IsRemote;
        }

        /// <summary>
        /// creates a new world or loads an existing one
        /// </summary>
        public void StartWorld(string par1Str, string par2Str, WorldSettings par3WorldSettings)
        {
            ChangeWorld1(null);
            //GC.Collect();

            if (saveLoader.IsOldMapFormat(par1Str))
            {
                ConvertMapFormat(par1Str, par2Str);
            }
            else
            {
                if (LoadingScreen != null)
                {
                    LoadingScreen.PrintText(StatCollector.TranslateToLocal("menu.switchingLevel"));
                    LoadingScreen.DisplayLoadingString("");
                }

                ISaveHandler isavehandler = saveLoader.GetSaveLoader(par1Str, false);
                World world = null;
                world = new World(isavehandler, par2Str, par3WorldSettings);

                if (world.IsNewWorld)
                {
                    StatFileWriter.ReadStat(StatList.CreateWorldStat, 1);
                    StatFileWriter.ReadStat(StatList.StartGameStat, 1);
                    ChangeWorld2(world, StatCollector.TranslateToLocal("menu.generatingLevel"));
                }
                else
                {
                    StatFileWriter.ReadStat(StatList.LoadWorldStat, 1);
                    StatFileWriter.ReadStat(StatList.StartGameStat, 1);
                    ChangeWorld2(world, StatCollector.TranslateToLocal("menu.loadingLevel"));
                }
            }
        }

        /// <summary>
        /// Will use a portal teleport switching the dimension the player is in.
        /// </summary>
        public void UsePortal(int par1)
        {
            int i = ThePlayer.Dimension;
            ThePlayer.Dimension = par1;
            TheWorld.SetEntityDead(ThePlayer);
            ThePlayer.IsDead = false;
            float d = ThePlayer.PosX;
            float d1 = ThePlayer.PosZ;
            float d2 = 1.0F;

            if (i > -1 && ThePlayer.Dimension == -1)
            {
                d2 = 0.125F;
            }
            else if (i == -1 && ThePlayer.Dimension > -1)
            {
                d2 = 8F;
            }

            d *= d2;
            d1 *= d2;

            if (ThePlayer.Dimension == -1)
            {
                ThePlayer.SetLocationAndAngles(d, ThePlayer.PosY, d1, ThePlayer.RotationYaw, ThePlayer.RotationPitch);

                if (ThePlayer.IsEntityAlive())
                {
                    TheWorld.UpdateEntityWithOptionalForce(ThePlayer, false);
                }

                World world = null;
                world = new World(TheWorld, WorldProvider.GetProviderForDimension(ThePlayer.Dimension));
                ChangeWorld(world, "Entering the Nether", ThePlayer);
            }
            else if (ThePlayer.Dimension == 0)
            {
                if (ThePlayer.IsEntityAlive())
                {
                    ThePlayer.SetLocationAndAngles(d, ThePlayer.PosY, d1, ThePlayer.RotationYaw, ThePlayer.RotationPitch);
                    TheWorld.UpdateEntityWithOptionalForce(ThePlayer, false);
                }

                World world1 = null;
                world1 = new World(TheWorld, WorldProvider.GetProviderForDimension(ThePlayer.Dimension));

                if (i == -1)
                {
                    ChangeWorld(world1, "Leaving the Nether", ThePlayer);
                }
                else
                {
                    ChangeWorld(world1, "Leaving the End", ThePlayer);
                }
            }
            else
            {
                World world2 = null;
                world2 = new World(TheWorld, WorldProvider.GetProviderForDimension(ThePlayer.Dimension));
                ChunkCoordinates chunkcoordinates = world2.GetEntrancePortalLocation();
                d = chunkcoordinates.PosX;
                ThePlayer.PosY = chunkcoordinates.PosY;
                d1 = chunkcoordinates.PosZ;
                ThePlayer.SetLocationAndAngles(d, ThePlayer.PosY, d1, 90F, 0.0F);

                if (ThePlayer.IsEntityAlive())
                {
                    world2.UpdateEntityWithOptionalForce(ThePlayer, false);
                }

                ChangeWorld(world2, "Entering the End", ThePlayer);
            }

            ThePlayer.WorldObj = TheWorld;
            Console.WriteLine((new StringBuilder()).Append("Teleported to ").Append(TheWorld.WorldProvider.TheWorldType).ToString());

            if (ThePlayer.IsEntityAlive() && i < 1)
            {
                ThePlayer.SetLocationAndAngles(d, ThePlayer.PosY, d1, ThePlayer.RotationYaw, ThePlayer.RotationPitch);
                TheWorld.UpdateEntityWithOptionalForce(ThePlayer, false);
                (new Teleporter()).PlaceInPortal(TheWorld, ThePlayer);
            }
        }

        /// <summary>
        /// Unloads the current world, and displays a String while waiting
        /// </summary>
        public void ExitToMainMenu(string par1Str)
        {
            TheWorld = null;
            ChangeWorld2(null, par1Str);
        }

        /// <summary>
        /// Changes the world, no message, no player.
        /// </summary>
        public void ChangeWorld1(World par1World)
        {
            ChangeWorld2(par1World, "");
        }

        /// <summary>
        /// Changes the world with given message, no player.
        /// </summary>
        public void ChangeWorld2(World par1World, string par2Str)
        {
            ChangeWorld(par1World, par2Str, null);
        }

        /// <summary>
        /// first argument is the world to change to, second one is a loading message and the third the player itself
        /// </summary>
        public void ChangeWorld(World par1World, string par2Str, EntityPlayer par3EntityPlayer)
        {
            StatFileWriter.Func_27175_b();
            StatFileWriter.SyncStats();
            RenderViewEntity = null;

            if (LoadingScreen != null)
            {
                LoadingScreen.PrintText(par2Str);
                LoadingScreen.DisplayLoadingString("");
            }

            SndManager.PlayStreaming(null, 0.0F, 0.0F, 0.0F, 0.0F, 0.0F);

            if (TheWorld != null)
            {
                TheWorld.SaveWorldIndirectly(LoadingScreen);
            }

            TheWorld = par1World;

            if (par1World != null)
            {
                if (PlayerController != null)
                {
                    PlayerController.OnWorldChange(par1World);
                }

                if (!IsMultiplayerWorld())
                {
                    if (par3EntityPlayer == null)
                    {
                        ThePlayer = (EntityPlayerSP)par1World.Func_4085_a(typeof(EntityPlayerSP));
                    }
                }
                else if (ThePlayer != null)
                {
                    ThePlayer.PreparePlayerToSpawn();

                    if (par1World != null)
                    {
                        par1World.SpawnEntityInWorld(ThePlayer);
                    }
                }

                if (!par1World.IsRemote)
                {
                    PreloadWorld(par2Str);
                }

                if (ThePlayer == null)
                {
                    ThePlayer = (EntityPlayerSP)PlayerController.CreatePlayer(par1World);
                    ThePlayer.PreparePlayerToSpawn();
                    PlayerController.FlipPlayer(ThePlayer);
                }

                ThePlayer.MovementInput = new MovementInputFromOptions(GameSettings);

                if (RenderGlobal != null)
                {
                    RenderGlobal.ChangeWorld(par1World);
                }

                if (EffectRenderer != null)
                {
                    EffectRenderer.ClearEffects(par1World);
                }

                if (par3EntityPlayer != null)
                {
                    par1World.Func_6464_c();
                }

                IChunkProvider ichunkprovider = par1World.GetChunkProvider();

                if (ichunkprovider is ChunkProviderLoadOrGenerate)
                {
                    ChunkProviderLoadOrGenerate chunkproviderloadorgenerate = (ChunkProviderLoadOrGenerate)ichunkprovider;
                    int i = MathHelper2.Floor_float((int)ThePlayer.PosX) >> 4;
                    int j = MathHelper2.Floor_float((int)ThePlayer.PosZ) >> 4;
                    chunkproviderloadorgenerate.SetCurrentChunkOver(i, j);
                }

                par1World.SpawnPlayerWithLoadedChunks(ThePlayer);
                PlayerController.Func_6473_b(ThePlayer);

                if (par1World.IsNewWorld)
                {
                    par1World.SaveWorldIndirectly(LoadingScreen);
                }

                RenderViewEntity = ThePlayer;
            }
            else
            {
                saveLoader.FlushCache();
                ThePlayer = null;
            }

            //GC.Collect();
            SystemTime = 0L;
        }

        /// <summary>
        /// Converts from old map format to new map format
        /// </summary>
        private void ConvertMapFormat(string par1Str, string par2Str)
        {
            LoadingScreen.PrintText((new StringBuilder()).Append("Converting World to ").Append(saveLoader.GetFormatName()).ToString());
            LoadingScreen.DisplayLoadingString("This may take a while :)");
            saveLoader.ConvertMapFormat(par1Str, LoadingScreen);
            StartWorld(par1Str, par2Str, new WorldSettings(0L, 0, true, false, WorldType.DEFAULT));
        }

        /// <summary>
        /// Display the preload world loading screen then load SP World.
        /// </summary>
        private void PreloadWorld(string par1Str)
        {
            if (LoadingScreen != null)
            {
                LoadingScreen.PrintText(par1Str);
                LoadingScreen.DisplayLoadingString(StatCollector.TranslateToLocal("menu.generatingTerrain"));
            }

            int c = 0x80;

            if (PlayerController.Func_35643_e())
            {
                c = '@';
            }

            int i = 0;
            int j = (c * 2) / 16 + 1;
            j *= j;
            net.minecraft.src.IChunkProvider ichunkprovider = TheWorld.GetChunkProvider();
            ChunkCoordinates chunkcoordinates = TheWorld.GetSpawnPoint();

            if (ThePlayer != null)
            {
                chunkcoordinates.PosX = (int)ThePlayer.PosX;
                chunkcoordinates.PosZ = (int)ThePlayer.PosZ;
            }

            if (ichunkprovider is ChunkProviderLoadOrGenerate)
            {
                ChunkProviderLoadOrGenerate chunkproviderloadorgenerate = (ChunkProviderLoadOrGenerate)ichunkprovider;
                chunkproviderloadorgenerate.SetCurrentChunkOver(chunkcoordinates.PosX >> 4, chunkcoordinates.PosZ >> 4);
            }

            for (int k = -c; k <= c; k += 16)
            {
                for (int l = -c; l <= c; l += 16)
                {
                    if (LoadingScreen != null)
                    {
                        LoadingScreen.SetLoadingProgress((i++ * 100) / j);
                    }

                    TheWorld.GetBlockId(chunkcoordinates.PosX + k, 64, chunkcoordinates.PosZ + l);

                    if (PlayerController.Func_35643_e())
                    {
                        continue;
                    }

                    while (TheWorld.UpdatingLighting()) ;
                }
            }

            if (!PlayerController.Func_35643_e())
            {
                if (LoadingScreen != null)
                {
                    LoadingScreen.DisplayLoadingString(StatCollector.TranslateToLocal("menu.simulating"));
                }

                TheWorld.DropOldChunks();
            }
        }

        ///<summary>
        ///Installs a resource. Currently only sounds are download so this method just adds them to the SoundManager.
        ///</summary>
        public void InstallResource(string par1Str, string par2File)
        {
            int i = par1Str.IndexOf("/");
            string s = par1Str.Substring(0, i);
            par1Str = par1Str.Substring(i + 1);

            if (string.Equals(s, "sound", StringComparison.CurrentCultureIgnoreCase))
            {
                SndManager.AddSound(par1Str, par2File);
            }
            else if (string.Equals(s, "newsound", StringComparison.CurrentCultureIgnoreCase))
            {
                SndManager.AddSound(par1Str, par2File);
            }
            else if (string.Equals(s, "streaming", StringComparison.CurrentCultureIgnoreCase))
            {
                SndManager.AddStreaming(par1Str, par2File);
            }
            else if (string.Equals(s, "music", StringComparison.CurrentCultureIgnoreCase))
            {
                SndManager.AddMusic(par1Str, par2File);
            }
            else if (string.Equals(s, "newmusic", StringComparison.CurrentCultureIgnoreCase))
            {
                SndManager.AddMusic(par1Str, par2File);
            }
        }

        ///<summary>
        /// A String of renderGlobal.getDebugInfoRenders
        ///</summary>
        public string DebugInfoRenders()
        {
            return RenderGlobal.GetDebugInfoRenders();
        }

        ///<summary>
        /// Gets the information in the F3 menu about how many entities are infront/around you
        ///</summary>
        public string GetEntityDebug()
        {
            return RenderGlobal.GetDebugInfoEntities();
        }

        ///<summary>
        /// Gets the name of the world's current chunk provider
        ///</summary>
        public string GetWorldProviderName()
        {
            return TheWorld.GetProviderName();
        }

        ///<summary>
        /// A String of how many entities are in the world
        ///</summary>
        public string DebugInfoEntities()
        {
            return (new StringBuilder()).Append("P: ").Append(EffectRenderer.GetStatistics()).Append(". T: ").Append(TheWorld.GetDebugLoadedEntities()).ToString();
        }

        ///<summary>
        /// Called when the respawn button is pressed after the player dies.
        ///</summary>
        public void Respawn(bool par1, int par2, bool par3)
        {
            if (!TheWorld.IsRemote && !TheWorld.WorldProvider.CanRespawnHere())
            {
                UsePortal(0);
            }

            ChunkCoordinates chunkcoordinates = null;
            ChunkCoordinates chunkcoordinates1 = null;
            bool flag = true;

            if (ThePlayer != null && !par1)
            {
                chunkcoordinates = ThePlayer.GetSpawnChunk();

                if (chunkcoordinates != null)
                {
                    chunkcoordinates1 = EntityPlayer.VerifyRespawnCoordinates(TheWorld, chunkcoordinates);

                    if (chunkcoordinates1 == null)
                    {
                        ThePlayer.AddChatMessage("tile.bed.notValid");
                    }
                }
            }

            if (chunkcoordinates1 == null)
            {
                chunkcoordinates1 = TheWorld.GetSpawnPoint();
                flag = false;
            }

            net.minecraft.src.IChunkProvider ichunkprovider = TheWorld.GetChunkProvider();

            if (ichunkprovider is ChunkProviderLoadOrGenerate)
            {
                ChunkProviderLoadOrGenerate chunkproviderloadorgenerate = (ChunkProviderLoadOrGenerate)ichunkprovider;
                chunkproviderloadorgenerate.SetCurrentChunkOver(chunkcoordinates1.PosX >> 4, chunkcoordinates1.PosZ >> 4);
            }

            TheWorld.SetSpawnLocation();
            TheWorld.UpdateEntityList();
            int i = 0;

            if (ThePlayer != null)
            {
                i = ThePlayer.EntityId;
                TheWorld.SetEntityDead(ThePlayer);
            }

            EntityPlayerSP entityplayersp = ThePlayer;
            RenderViewEntity = null;
            ThePlayer = (EntityPlayerSP)PlayerController.CreatePlayer(TheWorld);

            if (par3)
            {
                ThePlayer.CopyPlayer(entityplayersp);
            }

            ThePlayer.Dimension = par2;
            RenderViewEntity = ThePlayer;
            ThePlayer.PreparePlayerToSpawn();

            if (flag)
            {
                ThePlayer.SetSpawnChunk(chunkcoordinates);
                ThePlayer.SetLocationAndAngles((float)chunkcoordinates1.PosX + 0.5F, (float)chunkcoordinates1.PosY + 0.1F, (float)chunkcoordinates1.PosZ + 0.5F, 0.0F, 0.0F);
            }

            PlayerController.FlipPlayer(ThePlayer);
            TheWorld.SpawnPlayerWithLoadedChunks(ThePlayer);
            ThePlayer.MovementInput = new MovementInputFromOptions(GameSettings);
            ThePlayer.EntityId = i;
            ThePlayer.Func_6420_o();
            PlayerController.Func_6473_b(ThePlayer);
            PreloadWorld(StatCollector.TranslateToLocal("menu.respawning"));

            if (CurrentScreen is GuiGameOver)
            {
                DisplayGuiScreen(null);
            }
        }

        ///<summary>
        /// Returns true if string begins with '/'
        ///</summary>
        public bool LineIsCommand(string par1Str)
        {
            return par1Str.StartsWith("/");
        }

        ///<summary>
        /// Called when the middle mouse button gets clicked
        ///</summary>
        private void ClickMiddleMouseButton()
        {
            if (ObjectMouseOver != null)
            {
                bool flag = ThePlayer.Capabilities.IsCreativeMode;
                int i = TheWorld.GetBlockId(ObjectMouseOver.BlockX, ObjectMouseOver.BlockY, ObjectMouseOver.BlockZ);

                if (!flag)
                {
                    if (i == Block.Grass.BlockID)
                    {
                        i = Block.Dirt.BlockID;
                    }

                    if (i == Block.StairDouble.BlockID)
                    {
                        i = Block.StairSingle.BlockID;
                    }

                    if (i == Block.Bedrock.BlockID)
                    {
                        i = Block.Stone.BlockID;
                    }
                }

                int j = 0;
                bool flag1 = false;

                if (Item.ItemsList[i] != null && Item.ItemsList[i].GetHasSubtypes())
                {
                    j = TheWorld.GetBlockMetadata(ObjectMouseOver.BlockX, ObjectMouseOver.BlockY, ObjectMouseOver.BlockZ);
                    flag1 = true;
                }

                if (Item.ItemsList[i] != null && (Item.ItemsList[i] is ItemBlock))
                {
                    Block block = Block.BlocksList[i];
                    int l = block.IdDropped(j, ThePlayer.WorldObj.Rand, 0);

                    if (l > 0)
                    {
                        i = l;
                    }
                }

                ThePlayer.Inventory.SetCurrentItem(i, j, flag1, flag);

                if (flag)
                {
                    int k = (ThePlayer.InventorySlots.InventorySlots.Count - 9) + ThePlayer.Inventory.CurrentItem;
                    PlayerController.SendSlotPacket(ThePlayer.Inventory.GetStackInSlot(ThePlayer.Inventory.CurrentItem), k);
                }
            }
        }

        ///<summary>
        /// get the client packet send queue
        ///</summary>
        public NetClientHandler GetSendQueue()
        {
            if (ThePlayer is EntityClientPlayerMP)
            {
                return ((EntityClientPlayerMP)ThePlayer).SendQueue;
            }
            else
            {
                return null;
            }
        }

        public Texture2D GetTextureResource(string name)
        {
            return Texture2D.FromStream(GraphicsDevice, GetResourceStream(name));
        }

        /// <summary>
        /// gets the working dir (OS specific) for minecraft
        /// </summary>
        public static string GetMinecraftDir()
        {
            if (minecraftDir == null)
            {
                minecraftDir = GetAppDir("Minecraft");
            }

            return minecraftDir;
        }

        /// <summary>
        /// gets the working dir (OS specific) for the specific application (which is always minecraft)
        /// </summary>
        public static string GetAppDir(string par0Str)
        {
            string s = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);// System.getProperty("user.home", ".");
            string file;
            /*
            switch (EnumOSMappingHelper.EnumOSMappingArray[(int)GetOs()])
            {
                case 1:
                case 2:
                    file = IOPath.Combine(s, (new StringBuilder()).Append('.').Append(par0Str).Append('/').ToString());
                    break;

                case 3:
                    string s1 = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

                    if (s1 != null)
                    {
                        file = IOPath.Combine(s1, (new StringBuilder()).Append(".").Append(par0Str).Append('/').ToString());
                    }
                    else
                    {
                        file = IOPath.Combine(s, (new StringBuilder()).Append('.').Append(par0Str).Append('/').ToString());
                    }

                    break;

                case 4:
                    file = IOPath.Combine(s, (new StringBuilder()).Append("Library/Application Support/").Append(par0Str).ToString());
                    break;

                default:
                    file = IOPath.Combine(s, (new StringBuilder()).Append(par0Str).Append('/').ToString());
                    break;
            }
            */
            string s1 = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            if (s1 != null)
            {
                file = IOPath.Combine(s1, (new StringBuilder()).Append("").Append(par0Str).Append('/').ToString());
            }
            else
            {
                file = IOPath.Combine(s, (new StringBuilder()).Append('.').Append(par0Str).Append('/').ToString());
            }

            if (!File.Exists(file))
            {
                try
                {
                    Directory.CreateDirectory(file);
                }
                catch
                {
                    throw new Exception((new StringBuilder()).Append("The working directory could not be created: ").Append(file).ToString());
                }

                return file;
            }
            else
            {
                return file;
            }
        }

        private static EnumOS GetOs()
        {
            return EnumOS.Windows;
            /*
            string s = System.getProperty("os.name").ToLower();

            if (s.Contains("win"))
            {
                return EnumOS.windows;
            }

            if (s.Contains("mac"))
            {
                return EnumOS.macos;
            }

            if (s.Contains("solaris"))
            {
                return EnumOS.solaris;
            }

            if (s.Contains("sunos"))
            {
                return EnumOS.solaris;
            }

            if (s.Contains("linux"))
            {
                return EnumOS.linux;
            }

            if (s.Contains("unix"))
            {
                return EnumOS.linux;
            }
            else
            {
                return EnumOS.unknown;
            }*/
        }

        /// <summary>
        /// Gets a stream for the specified embedded resource.
        /// </summary>
        public static Stream GetResourceStream(string name)
        {
            return Assembly.GetExecutingAssembly().GetManifestResourceStream("Minecraft.Resources." + name);
        }

        public static bool IsGuiEnabled()
        {
            return theMinecraft == null || !theMinecraft.GameSettings.HideGUI;
        }

        public static bool IsFancyGraphicsEnabled()
        {
            return theMinecraft != null && theMinecraft.GameSettings.FancyGraphics;
        }

        ///<summary>
        /// Returns if ambient occlusion is enabled
        ///</summary>
        public static bool IsAmbientOcclusionEnabled()
        {
            return theMinecraft != null && theMinecraft.GameSettings.AmbientOcclusion;
        }

        public static bool IsDebugInfoEnabled()
        {
            return theMinecraft != null && theMinecraft.GameSettings.ShowDebugInfo;
        }

        public static string GetVersion()
        {
            return "1.2.5";
        }

        public static string GetMinecraftTitle()
        {
            return "Minecraft " + GetVersion();
        }

        public static void SendSnoopData()
        {
            PlayerUsageSnooper playerusagesnooper = new PlayerUsageSnooper("client");
            playerusagesnooper.SetSnoopProperty("version", "Minecraft in C# " + GetVersion());
            playerusagesnooper.SetSnoopProperty("os_name", Environment.OSVersion.VersionString + Environment.OSVersion.ServicePack);
            playerusagesnooper.SetSnoopProperty("os_version", Environment.OSVersion.Version);
            playerusagesnooper.SetSnoopProperty("os_architecture", Environment.Is64BitOperatingSystem ? "x64" : "x86");
            playerusagesnooper.SetSnoopProperty("memory_total", GC.GetTotalMemory(false));/*
            //playerusagesnooper.Func_52022_a("memory_max", Runtime.getRuntime().maxMemory());
            //playerusagesnooper.Func_52022_a("java_version", System.getProperty("java.version"));
            playerusagesnooper.SetSnoopProperty("opengl_version", //GL.GetString(StringName.Version));
            playerusagesnooper.SetSnoopProperty("opengl_vendor", //GL.GetString(StringName.Vendor));*/
            playerusagesnooper.SendSnoopData();
        }
    }
}