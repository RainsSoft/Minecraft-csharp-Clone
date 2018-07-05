using System;

namespace net.minecraft.src
{
    using net.minecraft.src;
    using Microsoft.Xna.Framework;

    public class GuiAchievements : GuiScreen
    {
        /// <summary>
        /// The top x coordinate of the achievement map </summary>
        private static readonly int guiMapTop;

        /// <summary>
        /// The left y coordinate of the achievement map </summary>
        private static readonly int guiMapLeft;

        /// <summary>
        /// The bottom x coordinate of the achievement map </summary>
        private static readonly int guiMapBottom;

        /// <summary>
        /// The right y coordinate of the achievement map </summary>
        private static readonly int guiMapRight;
        protected int AchievementsPaneWidth;
        protected int AchievementsPaneHeight;

        /// <summary>
        /// The current mouse x coordinate </summary>
        protected int MouseX;

        /// <summary>
        /// The current mouse y coordinate </summary>
        protected int MouseY;
        protected double Field_27116_m;
        protected double Field_27115_n;

        /// <summary>
        /// The x position of the achievement map </summary>
        protected double GuiMapX;

        /// <summary>
        /// The y position of the achievement map </summary>
        protected double GuiMapY;
        protected double Field_27112_q;
        protected double Field_27111_r;

        /// <summary>
        /// Whether the Mouse Button is down or not </summary>
        private int isMouseButtonDown;
        private StatFileWriter statFileWriter;

        public GuiAchievements(StatFileWriter par1StatFileWriter)
        {
            AchievementsPaneWidth = 256;
            AchievementsPaneHeight = 202;
            MouseX = 0;
            MouseY = 0;
            isMouseButtonDown = 0;
            statFileWriter = par1StatFileWriter;
            int c = 215;
            int c1 = 215;
            Field_27116_m = GuiMapX = Field_27112_q = AchievementList.OpenInventory.DisplayColumn * 24 - c / 2 - 12;
            Field_27115_n = GuiMapY = Field_27111_r = AchievementList.OpenInventory.DisplayRow * 24 - c1 / 2;
        }

        ///<summary>
        /// Adds the buttons (and other controls) to the screen in question.
        ///</summary>
        public new void InitGui()
        {
            ControlList.Clear();
            ControlList.Add(new GuiSmallButton(1, Width / 2 + 24, Height / 2 + 74, 80, 20, StatCollector.TranslateToLocal("gui.done")));
        }

        ///<summary>
        /// Fired when a control is clicked. This is the equivalent of ActionListener.actionPerformed(ActionEvent e).
        ///</summary>
        protected new void ActionPerformed(GuiButton par1GuiButton)
        {
            if (par1GuiButton.Id == 1)
            {
                Mc.DisplayGuiScreen(null);
                Mc.SetIngameFocus();
            }

            base.ActionPerformed(par1GuiButton);
        }

        ///<summary>
        /// Fired when a key is typed. This is the equivalent of KeyListener.keyTyped(KeyEvent e).
        ///</summary>
        protected new void KeyTyped(char par1, int par2)
        {
            if (par2 == Mc.GameSettings.KeyBindInventory.KeyCode)
            {
                Mc.DisplayGuiScreen(null);
                Mc.SetIngameFocus();
            }
            else
            {
                base.KeyTyped(par1, par2);
            }
        }

        ///<summary>
        /// Draws the screen and all the components in it.
        ///</summary>
        public new void DrawScreen(int par1, int par2, float par3)
        {
            //if (Mouse.isButtonDown(0))
            {
                int i = (Width - AchievementsPaneWidth) / 2;
                int j = (Height - AchievementsPaneHeight) / 2;
                int k = i + 8;
                int l = j + 17;

                if ((isMouseButtonDown == 0 || isMouseButtonDown == 1) && par1 >= k && par1 < k + 224 && par2 >= l && par2 < l + 155)
                {
                    if (isMouseButtonDown == 0)
                    {
                        isMouseButtonDown = 1;
                    }
                    else
                    {
                        GuiMapX -= par1 - MouseX;
                        GuiMapY -= par2 - MouseY;
                        Field_27112_q = Field_27116_m = GuiMapX;
                        Field_27111_r = Field_27115_n = GuiMapY;
                    }

                    MouseX = par1;
                    MouseY = par2;
                }

                if (Field_27112_q < (double)guiMapTop)
                {
                    Field_27112_q = guiMapTop;
                }

                if (Field_27111_r < (double)guiMapLeft)
                {
                    Field_27111_r = guiMapLeft;
                }

                if (Field_27112_q >= (double)guiMapBottom)
                {
                    Field_27112_q = guiMapBottom - 1;
                }

                if (Field_27111_r >= (double)guiMapRight)
                {
                    Field_27111_r = guiMapRight - 1;
                }
            }/*
            else
            {
                isMouseButtonDown = 0;
            }*/

            DrawDefaultBackground();
            GenAchievementBackground(par1, par2, par3);
            //GL.Disable(EnableCap.Lighting);
            //GL.Disable(EnableCap.DepthTest);
            Func_27110_k();
            //GL.Enable(EnableCap.Lighting);
            //GL.Enable(EnableCap.DepthTest);
        }

        ///<summary>
        /// Called from the main game loop to update the screen.
        ///</summary>
        public new void UpdateScreen()
        {
            Field_27116_m = GuiMapX;
            Field_27115_n = GuiMapY;
            double d = Field_27112_q - GuiMapX;
            double d1 = Field_27111_r - GuiMapY;

            if (d * d + d1 * d1 < 4D)
            {
                GuiMapX += d;
                GuiMapY += d1;
            }
            else
            {
                GuiMapX += d * 0.84999999999999998D;
                GuiMapY += d1 * 0.84999999999999998D;
            }
        }

        protected void Func_27110_k()
        {
            int i = (Width - AchievementsPaneWidth) / 2;
            int j = (Height - AchievementsPaneHeight) / 2;
            FontRenderer.DrawString("Achievements", i + 15, j + 5, 0x404040);
        }

        protected void GenAchievementBackground(int par1, int par2, float par3)
        {
            int i = MathHelper2.Floor_double(Field_27116_m + (GuiMapX - Field_27116_m) * (double)par3);
            int j = MathHelper2.Floor_double(Field_27115_n + (GuiMapY - Field_27115_n) * (double)par3);

            if (i < guiMapTop)
            {
                i = guiMapTop;
            }

            if (j < guiMapLeft)
            {
                j = guiMapLeft;
            }

            if (i >= guiMapBottom)
            {
                i = guiMapBottom - 1;
            }

            if (j >= guiMapRight)
            {
                j = guiMapRight - 1;
            }

            int k = Mc.RenderEngineOld.GetTexture("/terrain.png");
            int l = Mc.RenderEngineOld.GetTexture("/achievement/bg.png");
            int i1 = (Width - AchievementsPaneWidth) / 2;
            int j1 = (Height - AchievementsPaneHeight) / 2;
            int k1 = i1 + 16;
            int l1 = j1 + 17;
            ZLevel = 0.0F;
            //GL.DepthFunc(DepthFunction.Gequal);
            //GL.PushMatrix();
            //GL.Translate(0.0F, 0.0F, -200F);
            //GL.Enable(EnableCap.Texture2D);
            //GL.Disable(EnableCap.Lighting);
            //GL.Enable(EnableCap.RescaleNormal);
            //GL.Enable(EnableCap.ColorMaterial);
            Mc.RenderEngineOld.BindTexture(k);
            int i2 = i + 288 >> 4;
            int j2 = j + 288 >> 4;
            int k2 = (i + 288) % 16;
            int l2 = (j + 288) % 16;
            Random random = new Random();

            for (int i3 = 0; i3 * 16 - l2 < 155; i3++)
            {
                float f = 0.6F - ((float)(j2 + i3) / 25F) * 0.3F;
                //GL.Color4(f, f, f, 1.0F);

                for (int k3 = 0; k3 * 16 - k2 < 224; k3++)
                {
                    random = new Random(1234 + i2 + k3);
                    random.Next();
                    int j4 = random.Next(1 + j2 + i3) + (j2 + i3) / 2;
                    int l4 = Block.Sand.BlockIndexInTexture;

                    if (j4 > 37 || j2 + i3 == 35)
                    {
                        l4 = Block.Bedrock.BlockIndexInTexture;
                    }
                    else if (j4 == 22)
                    {
                        if (random.Next(2) == 0)
                        {
                            l4 = Block.OreDiamond.BlockIndexInTexture;
                        }
                        else
                        {
                            l4 = Block.OreRedstone.BlockIndexInTexture;
                        }
                    }
                    else if (j4 == 10)
                    {
                        l4 = Block.OreIron.BlockIndexInTexture;
                    }
                    else if (j4 == 8)
                    {
                        l4 = Block.OreCoal.BlockIndexInTexture;
                    }
                    else if (j4 > 4)
                    {
                        l4 = Block.Stone.BlockIndexInTexture;
                    }
                    else if (j4 > 0)
                    {
                        l4 = Block.Dirt.BlockIndexInTexture;
                    }

                    DrawTexturedModalRect((k1 + k3 * 16) - k2, (l1 + i3 * 16) - l2, l4 % 16 << 4, (l4 >> 4) << 4, 16, 16);
                }
            }

            //GL.Enable(EnableCap.DepthTest);
            //GL.DepthFunc(DepthFunction.Lequal);
            //GL.Disable(EnableCap.Texture2D);

            for (int j3 = 0; j3 < AchievementList.Achievements.Count; j3++)
            {
                Achievement achievement1 = (Achievement)AchievementList.Achievements[j3];

                if (achievement1.ParentAchievement == null)
                {
                    continue;
                }

                int l3 = (achievement1.DisplayColumn * 24 - i) + 11 + k1;
                int k4 = (achievement1.DisplayRow * 24 - j) + 11 + l1;
                int i5 = (achievement1.ParentAchievement.DisplayColumn * 24 - i) + 11 + k1;
                int l5 = (achievement1.ParentAchievement.DisplayRow * 24 - j) + 11 + l1;
                bool flag = statFileWriter.HasAchievementUnlocked(achievement1);
                bool flag1 = statFileWriter.CanUnlockAchievement(achievement1);
                int c = Math.Sin(((double)(JavaHelper.CurrentTimeMillis() % 600L) / 600D) * Math.PI * 2D) <= 0.59999999999999998D ? 202 : 377;
                long i8 = 0xff000000;

                if (flag)
                {
                    i8 = 0xff707070;
                }
                else if (flag1)
                {
                    i8 = 65280 + (c << 24);
                }

                DrawHorizontalLine(l3, i5, k4, (int)i8);
                DrawVerticalLine(i5, k4, l5, (int)i8);
            }

            Achievement achievement = null;
            RenderItem renderitem = new RenderItem();
            RenderHelper.EnableGUIStandardItemLighting();
            //GL.Disable(EnableCap.Lighting);
            //GL.Enable(EnableCap.RescaleNormal);
            //GL.Enable(EnableCap.ColorMaterial);

            for (int i4 = 0; i4 < AchievementList.Achievements.Count; i4++)
            {
                Achievement achievement2 = (Achievement)AchievementList.Achievements[i4];
                int j5 = achievement2.DisplayColumn * 24 - i;
                int i6 = achievement2.DisplayRow * 24 - j;

                if (j5 < -24 || i6 < -24 || j5 > 224 || i6 > 155)
                {
                    continue;
                }

                if (statFileWriter.HasAchievementUnlocked(achievement2))
                {
                    float f1 = 1.0F;
                    //GL.Color4(f1, f1, f1, 1.0F);
                }
                else if (statFileWriter.CanUnlockAchievement(achievement2))
                {
                    float f2 = Math.Sin(((double)(JavaHelper.CurrentTimeMillis() % 600L) / 600D) * Math.PI * 2D) >= 0.59999999999999998D ? 0.8F : 0.6F;
                    //GL.Color4(f2, f2, f2, 1.0F);
                }
                else
                {
                    float f3 = 0.3F;
                    //GL.Color4(f3, f3, f3, 1.0F);
                }

                Mc.RenderEngineOld.BindTexture(l);
                int k6 = k1 + j5;
                int j7 = l1 + i6;

                if (achievement2.GetSpecial())
                {
                    DrawTexturedModalRect(k6 - 2, j7 - 2, 26, 202, 26, 26);
                }
                else
                {
                    DrawTexturedModalRect(k6 - 2, j7 - 2, 0, 202, 26, 26);
                }

                if (!statFileWriter.CanUnlockAchievement(achievement2))
                {
                    float f4 = 0.1F;
                    //GL.Color4(f4, f4, f4, 1.0F);
                    renderitem.Field_27004_a = false;
                }

                //GL.Enable(EnableCap.Lighting);
                //GL.Enable(EnableCap.CullFace);
                renderitem.RenderItemIntoGUI(Mc.FontRenderer, Mc.RenderEngineOld, achievement2.TheItemStack, k6 + 3, j7 + 3);
                //GL.Disable(EnableCap.Lighting);

                if (!statFileWriter.CanUnlockAchievement(achievement2))
                {
                    renderitem.Field_27004_a = true;
                }

                //GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);

                if (par1 >= k1 && par2 >= l1 && par1 < k1 + 224 && par2 < l1 + 155 && par1 >= k6 && par1 <= k6 + 22 && par2 >= j7 && par2 <= j7 + 22)
                {
                    achievement = achievement2;
                }
            }

            //GL.Disable(EnableCap.DepthTest);
            //GL.Enable(EnableCap.Blend);
            //GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
            Mc.RenderEngineOld.BindTexture(l);
            DrawTexturedModalRect(i1, j1, 0, 0, AchievementsPaneWidth, AchievementsPaneHeight);
            //GL.PopMatrix();
            ZLevel = 0.0F;
            //GL.DepthFunc(DepthFunction.Lequal);
            //GL.Disable(EnableCap.DepthTest);
            //GL.Enable(EnableCap.Texture2D);
            base.DrawScreen(par1, par2, par3);

            if (achievement != null)
            {
                string s = StatCollector.TranslateToLocal(achievement.GetName());
                string s1 = achievement.GetDescription();
                int k5 = par1 + 12;
                int j6 = par2 - 4;

                if (statFileWriter.CanUnlockAchievement(achievement))
                {
                    int l6 = Math.Max(FontRenderer.GetStringWidth(s), 120);
                    int k7 = FontRenderer.SplitStringWidth(s1, l6);

                    if (statFileWriter.HasAchievementUnlocked(achievement))
                    {
                        k7 += 12;
                    }

                    DrawGradientRect(k5 - 3, j6 - 3, k5 + l6 + 3, j6 + k7 + 3 + 12, 0xc000000, 0xc000000);
                    FontRenderer.DrawSplitString(s1, k5, j6 + 12, l6, 0xffa0a0a);

                    if (statFileWriter.HasAchievementUnlocked(achievement))
                    {
                        FontRenderer.DrawStringWithShadow(StatCollector.TranslateToLocal("achievement.taken"), k5, j6 + k7 + 4, 0xff9090f);
                    }
                }
                else
                {
                    int i7 = Math.Max(FontRenderer.GetStringWidth(s), 120);
                    string s2 = StatCollector.TranslateToLocalFormatted("achievement.requires", new Object[]
                        {
                            StatCollector.TranslateToLocal(achievement.ParentAchievement.GetName())
                        });
                    int l7 = FontRenderer.SplitStringWidth(s2, i7);
                    DrawGradientRect(k5 - 3, j6 - 3, k5 + i7 + 3, j6 + l7 + 12 + 3, 0xc000000, 0xc000000);
                    FontRenderer.DrawSplitString(s2, k5, j6 + 12, i7, 0xff70505);
                }

                FontRenderer.DrawStringWithShadow(s, k5, j6, statFileWriter.CanUnlockAchievement(achievement) ? achievement.GetSpecial() ? -128 : -1 : achievement.GetSpecial() ? 0xff80804 : 0xff80808);
            }

            //GL.Enable(EnableCap.DepthTest);
            //GL.Enable(EnableCap.Lighting);
            RenderHelper.DisableStandardItemLighting();
        }

        ///<summary>
        // Returns true if this GUI should pause the game when it is displayed in single-player
        ///</summary>
        public new bool DoesGuiPauseGame()
        {
            return true;
        }

        static GuiAchievements()
        {
            guiMapTop = AchievementList.MinDisplayColumn * 24 - 112;
            guiMapLeft = AchievementList.MinDisplayRow * 24 - 112;
            guiMapBottom = AchievementList.MaxDisplayColumn * 24 - 77;
            guiMapRight = AchievementList.MaxDisplayRow * 24 - 77;
        }
    }
}