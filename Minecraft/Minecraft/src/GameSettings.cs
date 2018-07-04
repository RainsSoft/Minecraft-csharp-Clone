using System;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace net.minecraft.src
{
    public class GameSettings
    {
        private static readonly string[] RENDER_DISTANCES = { "options.renderDistance.far", "options.renderDistance.normal", "options.renderDistance.short", "options.renderDistance.tiny" };
        private static readonly string[] DIFFICULTIES = { "options.difficulty.peaceful", "options.difficulty.easy", "options.difficulty.normal", "options.difficulty.hard" };
        private static readonly string[] GUISCALES = { "options.guiScale.auto", "options.guiScale.small", "options.guiScale.normal", "options.guiScale.large" };
        private static readonly string[] PARTICLES = { "options.particles.all", "options.particles.decreased", "options.particles.minimal" };
        private static readonly string[] LIMIT_FRAMERATES = { "performance.max", "performance.balanced", "performance.powersaver" };
        public float MusicVolume;
        public float SoundVolume;
        public float MouseSensitivity;
        public bool InvertMouse;
        public int RenderDistance;
        public bool ViewBobbing;
        public bool Anaglyph;

        /// <summary>
        /// Advanced OpenGL </summary>
        public bool AdvancedOpengl;
        public int LimitFramerate;
        public bool FancyGraphics;

        /// <summary>
        /// Smooth Lighting </summary>
        public bool AmbientOcclusion;

        /// <summary>
        /// Clouds flag </summary>
        public bool Clouds;

        /// <summary>
        /// The name of the selected texture pack. </summary>
        public string Skin;
        public KeyBinding KeyBindForward;
        public KeyBinding KeyBindLeft;
        public KeyBinding KeyBindBack;
        public KeyBinding KeyBindRight;
        public KeyBinding KeyBindJump;
        public KeyBinding KeyBindInventory;
        public KeyBinding KeyBindDrop;
        public KeyBinding KeyBindChat;
        public KeyBinding KeyBindSneak;
        public KeyBinding KeyBindAttack;
        public KeyBinding KeyBindUseItem;
        public KeyBinding KeyBindPlayerList;
        public KeyBinding KeyBindPickBlock;
        public KeyBinding[] KeyBindings;
        protected Minecraft Mc;
        private string OptionsFile;
        public int Difficulty;
        public bool HideGUI;
        public int ThirdPersonView;

        /// <summary>
        /// true if debug info should be displayed instead of version </summary>
        public bool ShowDebugInfo;
        public bool Field_50119_G;

        /// <summary>
        /// The lastServer string. </summary>
        public string LastServer;

        /// <summary>
        /// No clipping for singleplayer </summary>
        public bool Noclip;

        /// <summary>
        /// Smooth Camera Toggle </summary>
        public bool SmoothCamera;
        public bool DebugCamEnable;

        /// <summary>
        /// No clipping movement rate </summary>
        public float NoclipRate;

        /// <summary>
        /// Change rate for debug camera </summary>
        public float DebugCamRate;
        public float FovSetting;
        public float GammaSetting;

        /// <summary>
        /// GUI scale </summary>
        public int GuiScale;

        /// <summary>
        /// Determines amount of particles. 0 = All, 1 = Decreased, 2 = Minimal </summary>
        public int ParticleSetting;

        /// <summary>
        /// Game settings language </summary>
        public string Language;

        public GameSettings(Minecraft par1Minecraft, string par2File)
        {
            MusicVolume = 1.0F;
            SoundVolume = 1.0F;
            MouseSensitivity = 0.5F;
            InvertMouse = false;
            RenderDistance = 0;
            ViewBobbing = true;
            Anaglyph = false;
            AdvancedOpengl = false;
            LimitFramerate = 1;
            FancyGraphics = true;
            AmbientOcclusion = true;
            Clouds = true;
            Skin = "Default";
            KeyBindForward = new KeyBinding("key.forward", 17);
            KeyBindLeft = new KeyBinding("key.left", 30);
            KeyBindBack = new KeyBinding("key.back", 31);
            KeyBindRight = new KeyBinding("key.right", 32);
            KeyBindJump = new KeyBinding("key.jump", 57);
            KeyBindInventory = new KeyBinding("key.inventory", 18);
            KeyBindDrop = new KeyBinding("key.drop", 16);
            KeyBindChat = new KeyBinding("key.chat", 20);
            KeyBindSneak = new KeyBinding("key.sneak", 42);
            KeyBindAttack = new KeyBinding("key.attack", -100);
            KeyBindUseItem = new KeyBinding("key.use", -99);
            KeyBindPlayerList = new KeyBinding("key.playerlist", 15);
            KeyBindPickBlock = new KeyBinding("key.pickItem", -98);
            KeyBindings = (new KeyBinding[] { KeyBindAttack, KeyBindUseItem, KeyBindForward, KeyBindLeft, KeyBindBack, KeyBindRight, KeyBindJump, KeyBindSneak, KeyBindDrop, KeyBindInventory, KeyBindChat, KeyBindPlayerList, KeyBindPickBlock });
            Difficulty = 2;
            HideGUI = false;
            ThirdPersonView = 0;
            ShowDebugInfo = false;
            Field_50119_G = false;
            LastServer = "";
            Noclip = false;
            SmoothCamera = false;
            DebugCamEnable = false;
            NoclipRate = 1.0F;
            DebugCamRate = 1.0F;
            FovSetting = 0.0F;
            GammaSetting = 0.0F;
            GuiScale = 0;
            ParticleSetting = 0;
            Language = "en_US";
            Mc = par1Minecraft;
            OptionsFile = System.IO.Path.Combine(par2File, "options.txt");
            LoadOptions();
        }

        public GameSettings()
        {
            MusicVolume = 1.0F;
            SoundVolume = 1.0F;
            MouseSensitivity = 0.5F;
            InvertMouse = false;
            RenderDistance = 0;
            ViewBobbing = true;
            Anaglyph = false;
            AdvancedOpengl = false;
            LimitFramerate = 1;
            FancyGraphics = true;
            AmbientOcclusion = true;
            Clouds = true;
            Skin = "Default";
            KeyBindForward = new KeyBinding("key.forward", 17);
            KeyBindLeft = new KeyBinding("key.left", 30);
            KeyBindBack = new KeyBinding("key.back", 31);
            KeyBindRight = new KeyBinding("key.right", 32);
            KeyBindJump = new KeyBinding("key.jump", 57);
            KeyBindInventory = new KeyBinding("key.Inventory", 18);
            KeyBindDrop = new KeyBinding("key.drop", 16);
            KeyBindChat = new KeyBinding("key.chat", 20);
            KeyBindSneak = new KeyBinding("key.sneak", 42);
            KeyBindAttack = new KeyBinding("key.attack", -100);
            KeyBindUseItem = new KeyBinding("key.use", -99);
            KeyBindPlayerList = new KeyBinding("key.playerlist", 15);
            KeyBindPickBlock = new KeyBinding("key.pickItem", -98);
            KeyBindings = (new KeyBinding[] { KeyBindAttack, KeyBindUseItem, KeyBindForward, KeyBindLeft, KeyBindBack, KeyBindRight, KeyBindJump, KeyBindSneak, KeyBindDrop, KeyBindInventory, KeyBindChat, KeyBindPlayerList, KeyBindPickBlock });
            Difficulty = 2;
            HideGUI = false;
            ThirdPersonView = 0;
            ShowDebugInfo = false;
            Field_50119_G = false;
            LastServer = "";
            Noclip = false;
            SmoothCamera = false;
            DebugCamEnable = false;
            NoclipRate = 1.0F;
            DebugCamRate = 1.0F;
            FovSetting = 0.0F;
            GammaSetting = 0.0F;
            GuiScale = 0;
            ParticleSetting = 0;
            Language = "en_US";
        }

        public virtual string GetKeyBindingDescription(int par1)
        {
            StringTranslate stringtranslate = StringTranslate.GetInstance();
            return stringtranslate.TranslateKey(KeyBindings[par1].KeyDescription);
        }

        /// <summary>
        /// The string that appears inside the button/slider in the options menu.
        /// </summary>
        public virtual string GetOptionDisplayString(int par1)
        {
            int i = KeyBindings[par1].KeyCode;
            return GetKeyDisplayString(i);
        }

        /// <summary>
        /// Represents a key or mouse button as a string. Args: key
        /// </summary>
        public static string GetKeyDisplayString(int par0)
        {
            if (par0 < 0)
            {
                return StatCollector.TranslateToLocalFormatted("key.mouseButton", new object[] { par0 + 101 });
            }
            else
            {
                return ((Keys)par0).ToString();
            }
        }

        /// <summary>
        /// Sets a key binding.
        /// </summary>
        public virtual void SetKeyBinding(int par1, int par2)
        {
            KeyBindings[par1].KeyCode = par2;
            SaveOptions();
        }

        /// <summary>
        /// If the specified option is controlled by a slider (float value), this will set the float value.
        /// </summary>
        public virtual void SetOptionFloatValue(Options par1Options, float par2)
        {
            if (par1Options == Options.MUSIC)
            {
                MusicVolume = par2;
                Mc.SndManager.OnSoundOptionsChanged();
            }

            if (par1Options == Options.SOUND)
            {
                SoundVolume = par2;
                Mc.SndManager.OnSoundOptionsChanged();
            }

            if (par1Options == Options.SENSITIVITY)
            {
                MouseSensitivity = par2;
            }

            if (par1Options == Options.FOV)
            {
                FovSetting = par2;
            }

            if (par1Options == Options.GAMMA)
            {
                GammaSetting = par2;
            }
        }

        /// <summary>
        /// For non-float options. Toggles the option on/off, or cycles through the list i.e. render distances.
        /// </summary>
        public virtual void SetOptionValue(Options par1Options, int par2)
        {
            if (par1Options == Options.INVERT_MOUSE)
            {
                InvertMouse = !InvertMouse;
            }

            if (par1Options == Options.RENDER_DISTANCE)
            {
                RenderDistance = RenderDistance + par2 & 3;
            }

            if (par1Options == Options.GUI_SCALE)
            {
                GuiScale = GuiScale + par2 & 3;
            }

            if (par1Options == Options.PARTICLES)
            {
                ParticleSetting = (ParticleSetting + par2) % 3;
            }

            if (par1Options == Options.VIEW_BOBBING)
            {
                ViewBobbing = !ViewBobbing;
            }

            if (par1Options == Options.RENDER_CLOUDS)
            {
                Clouds = !Clouds;
            }

            if (par1Options == Options.ADVANCED_OPENGL)
            {
                AdvancedOpengl = !AdvancedOpengl;
                Mc.RenderGlobal.LoadRenderers();
            }

            if (par1Options == Options.ANAGLYPH)
            {
                Anaglyph = !Anaglyph;
                Mc.RenderEngineOld.RefreshTextures();
            }

            if (par1Options == Options.FRAMERATE_LIMIT)
            {
                LimitFramerate = (LimitFramerate + par2 + 3) % 3;
            }

            if (par1Options == Options.DIFFICULTY)
            {
                Difficulty = Difficulty + par2 & 3;
            }

            if (par1Options == Options.GRAPHICS)
            {
                FancyGraphics = !FancyGraphics;
                Mc.RenderGlobal.LoadRenderers();
            }

            if (par1Options == Options.AMBIENT_OCCLUSION)
            {
                AmbientOcclusion = !AmbientOcclusion;
                Mc.RenderGlobal.LoadRenderers();
            }

            SaveOptions();
        }

        public virtual float GetOptionFloatValue(Options par1Options)
        {
            if (par1Options == Options.FOV)
            {
                return FovSetting;
            }

            if (par1Options == Options.GAMMA)
            {
                return GammaSetting;
            }

            if (par1Options == Options.MUSIC)
            {
                return MusicVolume;
            }

            if (par1Options == Options.SOUND)
            {
                return SoundVolume;
            }

            if (par1Options == Options.SENSITIVITY)
            {
                return MouseSensitivity;
            }
            else
            {
                return 0.0F;
            }
        }
        /*
        public virtual bool GetOptionOrdinalValue(Options par1Options)
        {
            switch (par1Options)
            {
                case 1:
                    return InvertMouse;

                case 2:
                    return ViewBobbing;

                case 3:
                    return Anaglyph;

                case 4:
                    return AdvancedOpengl;

                case 5:
                    return AmbientOcclusion;

                case 6:
                    return Clouds;
            }

            return false;
        }
        */
        private static string Func_48571_a(string[] par0ArrayOfStr, int par1)
        {
            if (par1 < 0 || par1 >= par0ArrayOfStr.Length)
            {
                par1 = 0;
            }

            StringTranslate stringtranslate = StringTranslate.GetInstance();
            return stringtranslate.TranslateKey(par0ArrayOfStr[par1]);
        }

        /// <summary>
        /// Gets a key binding.
        /// </summary>
        public virtual string GetKeyBinding(Options par1Options)
        {
            StringTranslate stringtranslate = StringTranslate.GetInstance();
            string s = (new StringBuilder()).Append(stringtranslate.TranslateKey(par1Options.String)).Append(": ").ToString();

            if (par1Options.Float)
            {
                float f = GetOptionFloatValue(par1Options);

                if (par1Options == Options.SENSITIVITY)
                {
                    if (f == 0.0F)
                    {
                        return (new StringBuilder()).Append(s).Append(stringtranslate.TranslateKey("options.sensitivity.min")).ToString();
                    }

                    if (f == 1.0F)
                    {
                        return (new StringBuilder()).Append(s).Append(stringtranslate.TranslateKey("options.sensitivity.max")).ToString();
                    }
                    else
                    {
                        return (new StringBuilder()).Append(s).Append((int)(f * 200F)).Append("%").ToString();
                    }
                }

                if (par1Options == Options.FOV)
                {
                    if (f == 0.0F)
                    {
                        return (new StringBuilder()).Append(s).Append(stringtranslate.TranslateKey("options.fov.min")).ToString();
                    }

                    if (f == 1.0F)
                    {
                        return (new StringBuilder()).Append(s).Append(stringtranslate.TranslateKey("options.fov.max")).ToString();
                    }
                    else
                    {
                        return (new StringBuilder()).Append(s).Append((int)(70F + f * 40F)).ToString();
                    }
                }

                if (par1Options == Options.GAMMA)
                {
                    if (f == 0.0F)
                    {
                        return (new StringBuilder()).Append(s).Append(stringtranslate.TranslateKey("options.gamma.min")).ToString();
                    }

                    if (f == 1.0F)
                    {
                        return (new StringBuilder()).Append(s).Append(stringtranslate.TranslateKey("options.gamma.max")).ToString();
                    }
                    else
                    {
                        return (new StringBuilder()).Append(s).Append("+").Append((int)(f * 100F)).Append("%").ToString();
                    }
                }

                if (f == 0.0F)
                {
                    return (new StringBuilder()).Append(s).Append(stringtranslate.TranslateKey("options.off")).ToString();
                }
                else
                {
                    return (new StringBuilder()).Append(s).Append((int)(f * 100F)).Append("%").ToString();
                }
            }

            if (par1Options.Bool)
            {
                //bool flag = GetOptionOrdinalValue(par1Options);

                //if (flag)
                {
                    return (new StringBuilder()).Append(s).Append(stringtranslate.TranslateKey("options.on")).ToString();
                }/*
                else
                {
                    return (new StringBuilder()).Append(s).Append(stringtranslate.TranslateKey("options.off")).ToString();
                }*/
            }

            if (par1Options == Options.RENDER_DISTANCE)
            {
                return (new StringBuilder()).Append(s).Append(Func_48571_a(RENDER_DISTANCES, RenderDistance)).ToString();
            }

            if (par1Options == Options.DIFFICULTY)
            {
                return (new StringBuilder()).Append(s).Append(Func_48571_a(DIFFICULTIES, Difficulty)).ToString();
            }

            if (par1Options == Options.GUI_SCALE)
            {
                return (new StringBuilder()).Append(s).Append(Func_48571_a(GUISCALES, GuiScale)).ToString();
            }

            if (par1Options == Options.PARTICLES)
            {
                return (new StringBuilder()).Append(s).Append(Func_48571_a(PARTICLES, ParticleSetting)).ToString();
            }

            if (par1Options == Options.FRAMERATE_LIMIT)
            {
                return (new StringBuilder()).Append(s).Append(Func_48571_a(LIMIT_FRAMERATES, LimitFramerate)).ToString();
            }

            if (par1Options == Options.GRAPHICS)
            {
                if (FancyGraphics)
                {
                    return (new StringBuilder()).Append(s).Append(stringtranslate.TranslateKey("options.graphics.fancy")).ToString();
                }
                else
                {
                    return (new StringBuilder()).Append(s).Append(stringtranslate.TranslateKey("options.graphics.fast")).ToString();
                }
            }
            else
            {
                return s;
            }
        }

        /// <summary>
        /// Loads the options from the options file. It appears that this has replaced the previous 'loadOptions'
        /// </summary>
        public virtual void LoadOptions()
        {
            try
            {
                if (!File.Exists(OptionsFile))
                {
                    return;
                }

                StreamReader bufferedreader = new StreamReader(OptionsFile);

                for (string s = ""; (s = bufferedreader.ReadLine()) != null; )
                {
                    try
                    {
                        string[] @as = StringHelperClass.StringSplit(s, ":", true);

                        if (@as[0].Equals("music"))
                        {
                            MusicVolume = ParseFloat(@as[1]);
                        }

                        if (@as[0].Equals("sound"))
                        {
                            SoundVolume = ParseFloat(@as[1]);
                        }

                        if (@as[0].Equals("mouseSensitivity"))
                        {
                            MouseSensitivity = ParseFloat(@as[1]);
                        }

                        if (@as[0].Equals("fov"))
                        {
                            FovSetting = ParseFloat(@as[1]);
                        }

                        if (@as[0].Equals("gamma"))
                        {
                            GammaSetting = ParseFloat(@as[1]);
                        }

                        if (@as[0].Equals("invertYMouse"))
                        {
                            InvertMouse = @as[1].Equals("true");
                        }

                        if (@as[0].Equals("viewDistance"))
                        {
                            RenderDistance = Convert.ToInt32(@as[1]);
                        }

                        if (@as[0].Equals("guiScale"))
                        {
                            GuiScale = Convert.ToInt32(@as[1]);
                        }

                        if (@as[0].Equals("particles"))
                        {
                            ParticleSetting = Convert.ToInt32(@as[1]);
                        }

                        if (@as[0].Equals("bobView"))
                        {
                            ViewBobbing = @as[1].Equals("true");
                        }

                        if (@as[0].Equals("anaglyph3d"))
                        {
                            Anaglyph = @as[1].Equals("true");
                        }

                        if (@as[0].Equals("advancedOpengl"))
                        {
                            AdvancedOpengl = @as[1].Equals("true");
                        }

                        if (@as[0].Equals("fpsLimit"))
                        {
                            LimitFramerate = Convert.ToInt32(@as[1]);
                        }

                        if (@as[0].Equals("difficulty"))
                        {
                            Difficulty = Convert.ToInt32(@as[1]);
                        }

                        if (@as[0].Equals("fancyGraphics"))
                        {
                            FancyGraphics = @as[1].Equals("true");
                        }

                        if (@as[0].Equals("ao"))
                        {
                            AmbientOcclusion = @as[1].Equals("true");
                        }

                        if (@as[0].Equals("clouds"))
                        {
                            Clouds = @as[1].Equals("true");
                        }

                        if (@as[0].Equals("skin"))
                        {
                            Skin = @as[1];
                        }

                        if (@as[0].Equals("lastServer") && @as.Length >= 2)
                        {
                            LastServer = @as[1];
                        }

                        if (@as[0].Equals("lang") && @as.Length >= 2)
                        {
                            Language = @as[1];
                        }

                        int i = 0;

                        while (i < KeyBindings.Length)
                        {
                            if (@as[0].Equals((new StringBuilder()).Append("key_").Append(KeyBindings[i].KeyDescription).ToString()))
                            {
                                KeyBindings[i].KeyCode = Convert.ToInt32(@as[1]);
                            }

                            i++;
                        }
                    }
                    catch (Exception exception1)
                    {
                        Console.WriteLine(exception1.ToString());
                        Console.WriteLine();

                        Console.WriteLine((new StringBuilder()).Append("Skipping bad option: ").Append(s).ToString());
                    }
                }

                KeyBinding.ResetKeyBindingArrayAndHash();
                bufferedreader.Close();
            }
            catch (Exception exception)
            {
                Console.WriteLine("Failed to load options");
                Console.WriteLine(exception.ToString());
                Console.Write(exception.StackTrace);
            }
        }

        /// <summary>
        /// Parses a string into a float.
        /// </summary>
        private float ParseFloat(string par1Str)
        {
            if (par1Str.Equals("true"))
            {
                return 1.0F;
            }

            if (par1Str.Equals("false"))
            {
                return 0.0F;
            }
            else
            {
                return Convert.ToSingle(par1Str);
            }
        }

        /// <summary>
        /// Saves the options to the options file.
        /// </summary>
        public virtual void SaveOptions()
        {
            try
            {
                StreamWriter printwriter = new StreamWriter(OptionsFile);
                printwriter.WriteLine((new StringBuilder()).Append("music:").Append(MusicVolume).ToString());
                printwriter.WriteLine((new StringBuilder()).Append("sound:").Append(SoundVolume).ToString());
                printwriter.WriteLine((new StringBuilder()).Append("invertYMouse:").Append(InvertMouse).ToString());
                printwriter.WriteLine((new StringBuilder()).Append("mouseSensitivity:").Append(MouseSensitivity).ToString());
                printwriter.WriteLine((new StringBuilder()).Append("fov:").Append(FovSetting).ToString());
                printwriter.WriteLine((new StringBuilder()).Append("gamma:").Append(GammaSetting).ToString());
                printwriter.WriteLine((new StringBuilder()).Append("viewDistance:").Append(RenderDistance).ToString());
                printwriter.WriteLine((new StringBuilder()).Append("guiScale:").Append(GuiScale).ToString());
                printwriter.WriteLine((new StringBuilder()).Append("particles:").Append(ParticleSetting).ToString());
                printwriter.WriteLine((new StringBuilder()).Append("bobView:").Append(ViewBobbing).ToString());
                printwriter.WriteLine((new StringBuilder()).Append("anaglyph3d:").Append(Anaglyph).ToString());
                printwriter.WriteLine((new StringBuilder()).Append("advancedOpengl:").Append(AdvancedOpengl).ToString());
                printwriter.WriteLine((new StringBuilder()).Append("fpsLimit:").Append(LimitFramerate).ToString());
                printwriter.WriteLine((new StringBuilder()).Append("difficulty:").Append(Difficulty).ToString());
                printwriter.WriteLine((new StringBuilder()).Append("fancyGraphics:").Append(FancyGraphics).ToString());
                printwriter.WriteLine((new StringBuilder()).Append("ao:").Append(AmbientOcclusion).ToString());
                printwriter.WriteLine((new StringBuilder()).Append("clouds:").Append(Clouds).ToString());
                printwriter.WriteLine((new StringBuilder()).Append("skin:").Append(Skin).ToString());
                printwriter.WriteLine((new StringBuilder()).Append("lastServer:").Append(LastServer).ToString());
                printwriter.WriteLine((new StringBuilder()).Append("lang:").Append(Language).ToString());

                for (int i = 0; i < KeyBindings.Length; i++)
                {
                    printwriter.WriteLine((new StringBuilder()).Append("key_").Append(KeyBindings[i].KeyDescription).Append(":").Append(KeyBindings[i].KeyCode).ToString());
                }

                printwriter.Close();
            }
            catch (Exception exception)
            {
                Console.WriteLine("Failed to save options");
                Console.WriteLine(exception.ToString());
                Console.Write(exception.StackTrace);
            }
        }

        /// <summary>
        /// Should render clouds
        /// </summary>
        public virtual bool ShouldRenderClouds()
        {
            return RenderDistance < 2 && Clouds;
        }
    }
}