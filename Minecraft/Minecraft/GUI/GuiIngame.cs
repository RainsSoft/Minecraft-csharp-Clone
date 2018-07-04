using System;
using System.Collections.Generic;
using System.Text;

namespace net.minecraft.src
{
    using net.minecraft.src;
    using Microsoft.Xna.Framework;
    using System.Drawing;

    public class GuiIngame : Gui
    {
        private static RenderItem itemRenderer = new RenderItem();

        /// <summary>
        /// A list with all the chat messages in. </summary>
        private List<ChatLine> chatMessageList;
        private List<string> field_50016_f;
        private Random rand;
        private Minecraft mc;
        private int updateCounter;

        /// <summary>
        /// The string specifying which record music is playing </summary>
        private string recordPlaying;

        /// <summary>
        /// How many ticks the record playing message will be displayed </summary>
        private int recordPlayingUpFor;
        private bool recordIsPlaying;
        private int field_50017_n;
        private bool field_50018_o;

        /// <summary>
        /// Damage partial time (GUI) </summary>
        public float DamageGuiPartialTime;

        /// <summary>
        /// Previous frame vignette brightness (slowly changes by 1% each frame) </summary>
        float PrevVignetteBrightness;

        public GuiIngame(Minecraft par1Minecraft)
        {
            chatMessageList = new List<ChatLine>();
            field_50016_f = new List<string>();
            rand = new Random();
            updateCounter = 0;
            recordPlaying = "";
            recordPlayingUpFor = 0;
            recordIsPlaying = false;
            field_50017_n = 0;
            field_50018_o = false;
            PrevVignetteBrightness = 1.0F;
            mc = par1Minecraft;
        }

        /// <summary>
        /// Render the ingame overlay with quick icon bar, ...
        /// </summary>
        public virtual void RenderGameOverlay(float par1, bool par2, int par3, int par4)
        {
            ScaledResolution scaledresolution = new ScaledResolution(mc.GameSettings, mc.DisplayWidth, mc.DisplayHeight);
            int i = scaledresolution.GetScaledWidth();
            int j = scaledresolution.GetScaledHeight();
            FontRenderer fontrenderer = mc.FontRenderer;
            mc.EntityRenderer.SetupOverlayRendering();
            //GL.Enable(EnableCap.Blend);

            if (Minecraft.IsFancyGraphicsEnabled())
            {
                RenderVignette(mc.ThePlayer.GetBrightness(par1), i, j);
            }
            else
            {
                //GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            }

            ItemStack itemstack = mc.ThePlayer.Inventory.ArmorItemInSlot(3);

            if (mc.GameSettings.ThirdPersonView == 0 && itemstack != null && itemstack.ItemID == Block.Pumpkin.BlockID)
            {
                RenderPumpkinBlur(i, j);
            }

            if (!mc.ThePlayer.IsPotionActive(Potion.Confusion))
            {
                float f = mc.ThePlayer.PrevTimeInPortal + (mc.ThePlayer.TimeInPortal - mc.ThePlayer.PrevTimeInPortal) * par1;

                if (f > 0.0F)
                {
                    RenderPortalOverlay(f, i, j);
                }
            }

            if (!mc.PlayerController.Func_35643_e())
            {
                //GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
                //GL.BindTexture(TextureTarget.Texture2D, mc.RenderEngineOld.GetTexture("/gui/gui.png"));
                InventoryPlayer inventoryplayer = mc.ThePlayer.Inventory;
                ZLevel = -90F;
                DrawTexturedModalRect(i / 2 - 91, j - 22, 0, 0, 182, 22);
                DrawTexturedModalRect((i / 2 - 91 - 1) + inventoryplayer.CurrentItem * 20, j - 22 - 1, 0, 22, 24, 22);
                //GL.BindTexture(TextureTarget.Texture2D, mc.RenderEngineOld.GetTexture("/gui/icons.png"));
                //GL.Enable(EnableCap.Blend);
                //GL.BlendFunc(BlendingFactorSrc.OneMinusDstColor, BlendingFactorDest.OneMinusSrcColor);
                DrawTexturedModalRect(i / 2 - 7, j / 2 - 7, 0, 0, 16, 16);
                //GL.Disable(EnableCap.Blend);
                bool flag = (mc.ThePlayer.HeartsLife / 3) % 2 == 1;

                if (mc.ThePlayer.HeartsLife < 10)
                {
                    flag = false;
                }

                int i1 = mc.ThePlayer.GetHealth();
                int i2 = mc.ThePlayer.PrevHealth;
                rand.SetSeed(updateCounter * 0x4c627);
                bool flag2 = false;
                FoodStats foodstats = mc.ThePlayer.GetFoodStats();
                int j4 = foodstats.GetFoodLevel();
                int l4 = foodstats.GetPrevFoodLevel();
                RenderBossHealth();

                if (mc.PlayerController.ShouldDrawHUD())
                {
                    int j5 = i / 2 - 91;
                    int i6 = i / 2 + 91;
                    int l6 = mc.ThePlayer.XpBarCap();

                    if (l6 > 0)
                    {
                        int c = 0xb6;
                        int j8 = (int)(mc.ThePlayer.Experience * (float)(c + 1));
                        int i9 = (j - 32) + 3;
                        DrawTexturedModalRect(j5, i9, 0, 64, c, 5);

                        if (j8 > 0)
                        {
                            DrawTexturedModalRect(j5, i9, 0, 69, j8, 5);
                        }
                    }

                    int k7 = j - 39;
                    int k8 = k7 - 10;
                    int j9 = mc.ThePlayer.GetTotalArmorValue();
                    int i10 = -1;

                    if (mc.ThePlayer.IsPotionActive(Potion.Regeneration))
                    {
                        i10 = updateCounter % 25;
                    }

                    for (int j10 = 0; j10 < 10; j10++)
                    {
                        if (j9 > 0)
                        {
                            int i11 = j5 + j10 * 8;

                            if (j10 * 2 + 1 < j9)
                            {
                                DrawTexturedModalRect(i11, k8, 34, 9, 9, 9);
                            }

                            if (j10 * 2 + 1 == j9)
                            {
                                DrawTexturedModalRect(i11, k8, 25, 9, 9, 9);
                            }

                            if (j10 * 2 + 1 > j9)
                            {
                                DrawTexturedModalRect(i11, k8, 16, 9, 9, 9);
                            }
                        }

                        int j11 = 16;

                        if (mc.ThePlayer.IsPotionActive(Potion.Poison))
                        {
                            j11 += 36;
                        }

                        int i12 = 0;

                        if (flag)
                        {
                            i12 = 1;
                        }

                        int l12 = j5 + j10 * 8;
                        int j13 = k7;

                        if (i1 <= 4)
                        {
                            j13 += rand.Next(2);
                        }

                        if (j10 == i10)
                        {
                            j13 -= 2;
                        }

                        byte byte3 = 0;

                        if (mc.TheWorld.GetWorldInfo().IsHardcoreModeEnabled())
                        {
                            byte3 = 5;
                        }

                        DrawTexturedModalRect(l12, j13, 16 + i12 * 9, 9 * byte3, 9, 9);

                        if (flag)
                        {
                            if (j10 * 2 + 1 < i2)
                            {
                                DrawTexturedModalRect(l12, j13, j11 + 54, 9 * byte3, 9, 9);
                            }

                            if (j10 * 2 + 1 == i2)
                            {
                                DrawTexturedModalRect(l12, j13, j11 + 63, 9 * byte3, 9, 9);
                            }
                        }

                        if (j10 * 2 + 1 < i1)
                        {
                            DrawTexturedModalRect(l12, j13, j11 + 36, 9 * byte3, 9, 9);
                        }

                        if (j10 * 2 + 1 == i1)
                        {
                            DrawTexturedModalRect(l12, j13, j11 + 45, 9 * byte3, 9, 9);
                        }
                    }

                    for (int k10 = 0; k10 < 10; k10++)
                    {
                        int k11 = k7;
                        int j12 = 16;
                        byte byte2 = 0;

                        if (mc.ThePlayer.IsPotionActive(Potion.Hunger))
                        {
                            j12 += 36;
                            byte2 = 13;
                        }

                        if (mc.ThePlayer.GetFoodStats().GetSaturationLevel() <= 0.0F && updateCounter % (j4 * 3 + 1) == 0)
                        {
                            k11 += rand.Next(3) - 1;
                        }

                        if (flag2)
                        {
                            byte2 = 1;
                        }

                        int k13 = i6 - k10 * 8 - 9;
                        DrawTexturedModalRect(k13, k11, 16 + byte2 * 9, 27, 9, 9);

                        if (flag2)
                        {
                            if (k10 * 2 + 1 < l4)
                            {
                                DrawTexturedModalRect(k13, k11, j12 + 54, 27, 9, 9);
                            }

                            if (k10 * 2 + 1 == l4)
                            {
                                DrawTexturedModalRect(k13, k11, j12 + 63, 27, 9, 9);
                            }
                        }

                        if (k10 * 2 + 1 < j4)
                        {
                            DrawTexturedModalRect(k13, k11, j12 + 36, 27, 9, 9);
                        }

                        if (k10 * 2 + 1 == j4)
                        {
                            DrawTexturedModalRect(k13, k11, j12 + 45, 27, 9, 9);
                        }
                    }

                    if (mc.ThePlayer.IsInsideOfMaterial(Material.Water))
                    {
                        int l10 = mc.ThePlayer.GetAir();
                        int l11 = (int)Math.Ceiling(((double)(l10 - 2) * 10D) / 300D);
                        int k12 = (int)Math.Ceiling(((double)l10 * 10D) / 300D) - l11;

                        for (int i13 = 0; i13 < l11 + k12; i13++)
                        {
                            if (i13 < l11)
                            {
                                DrawTexturedModalRect(i6 - i13 * 8 - 9, k8, 16, 18, 9, 9);
                            }
                            else
                            {
                                DrawTexturedModalRect(i6 - i13 * 8 - 9, k8, 25, 18, 9, 9);
                            }
                        }
                    }
                }

                //GL.Disable(EnableCap.Blend);
                //GL.Enable(EnableCap.RescaleNormal);
                RenderHelper.EnableGUIStandardItemLighting();

                for (int k5 = 0; k5 < 9; k5++)
                {
                    int j6 = (i / 2 - 90) + k5 * 20 + 2;
                    int i7 = j - 16 - 3;
                    RenderInventorySlot(k5, j6, i7, par1);
                }

                RenderHelper.DisableStandardItemLighting();
                //GL.Disable(EnableCap.RescaleNormal);
            }

            if (mc.ThePlayer.GetSleepTimer() > 0)
            {
                //GL.Disable(EnableCap.DepthTest);
                //GL.Disable(EnableCap.AlphaTest);
                int k = mc.ThePlayer.GetSleepTimer();
                float f1 = (float)k / 100F;

                if (f1 > 1.0F)
                {
                    f1 = 1.0F - (float)(k - 100) / 10F;
                }

                int j1 = (int)(220F * f1) << 24 | 0x101020;
                DrawRect(0, 0, i, j, j1);
                //GL.Enable(EnableCap.AlphaTest);
                //GL.Enable(EnableCap.DepthTest);
            }

            if (mc.PlayerController.Func_35642_f() && mc.ThePlayer.ExperienceLevel > 0)
            {
                bool flag1 = false;
                int k1 = flag1 ? 0xffffff : 0x80ff20;
                String s = (new StringBuilder()).Append("").Append(mc.ThePlayer.ExperienceLevel).ToString();
                int i3 = (i - fontrenderer.GetStringWidth(s)) / 2;
                int k3 = j - 31 - 4;
                fontrenderer.DrawString(s, i3 + 1, k3, 0);
                fontrenderer.DrawString(s, i3 - 1, k3, 0);
                fontrenderer.DrawString(s, i3, k3 + 1, 0);
                fontrenderer.DrawString(s, i3, k3 - 1, 0);
                fontrenderer.DrawString(s, i3, k3, k1);
            }

            if (mc.GameSettings.ShowDebugInfo)
            {
                //GL.PushMatrix();

                if (Minecraft.HasPaidCheckTime > 0L)
                {
                    //GL.Translate(0.0F, 32F, 0.0F);
                }

                fontrenderer.DrawStringWithShadow((new StringBuilder()).Append("Minecraft 1.2.5 (").Append(mc.Debug).Append(")").ToString(), 2, 2, 0xffffff);
                fontrenderer.DrawStringWithShadow(mc.DebugInfoRenders(), 2, 12, 0xffffff);
                fontrenderer.DrawStringWithShadow(mc.GetEntityDebug(), 2, 22, 0xffffff);
                fontrenderer.DrawStringWithShadow(mc.DebugInfoEntities(), 2, 32, 0xffffff);
                fontrenderer.DrawStringWithShadow(mc.GetWorldProviderName(), 2, 42, 0xffffff);
                //long l = Runtime.getRuntime().maxMemory();
                long l2 = GC.GetTotalMemory(false);
                //long l3 = Runtime.getRuntime().freeMemory();
                //long l5 = l2 - l3;
                //string s1 = (new StringBuilder()).Append("Used memory: ").Append((l5 * 100L) / l).Append("% (").Append(l5 / 1024L / 1024L).Append("MB) of ").Append(l / 1024L / 1024L).Append("MB").ToString();
                //DrawString(fontrenderer, s1, i - fontrenderer.GetStringWidth(s1) - 2, 2, 0xe0e0e0);
                //s1 = (new StringBuilder()).Append("Allocated memory: ").Append((l2 * 100L) / l).Append("% (").Append(l2 / 1024L / 1024L).Append("MB)").ToString();
                //DrawString(fontrenderer, s1, i - fontrenderer.GetStringWidth(s1) - 2, 12, 0xe0e0e0);
                DrawString(fontrenderer, (new StringBuilder()).Append("x: ").Append(mc.ThePlayer.PosX).ToString(), 2, 64, 0xe0e0e0);
                DrawString(fontrenderer, (new StringBuilder()).Append("y: ").Append(mc.ThePlayer.PosY).ToString(), 2, 72, 0xe0e0e0);
                DrawString(fontrenderer, (new StringBuilder()).Append("z: ").Append(mc.ThePlayer.PosZ).ToString(), 2, 80, 0xe0e0e0);
                DrawString(fontrenderer, (new StringBuilder()).Append("f: ").Append((int)Math.Floor((double)((mc.ThePlayer.RotationYaw * 4F) / 360F) + 0.5D) & 3).ToString(), 2, 88, 0xe0e0e0);
                int l7 = (int)Math.Floor(mc.ThePlayer.PosX);
                int l8 = (int)Math.Floor(mc.ThePlayer.PosY);
                int k9 = (int)Math.Floor(mc.ThePlayer.PosZ);

                if (mc.TheWorld != null && mc.TheWorld.BlockExists(l7, l8, k9))
                {
                    Chunk chunk = mc.TheWorld.GetChunkFromBlockCoords(l7, k9);
                    DrawString(fontrenderer, (new StringBuilder()).Append("lc: ").Append(chunk.GetTopFilledSegment() + 15).Append(" b: ").Append(chunk.Func_48490_a(l7 & 0xf, k9 & 0xf, mc.TheWorld.GetWorldChunkManager()).BiomeName).Append(" bl: ").Append(chunk.GetSavedLightValue(SkyBlock.Block, l7 & 0xf, l8, k9 & 0xf)).Append(" sl: ").Append(chunk.GetSavedLightValue(SkyBlock.Sky, l7 & 0xf, l8, k9 & 0xf)).Append(" rl: ").Append(chunk.GetBlockLightValue(l7 & 0xf, l8, k9 & 0xf, 0)).ToString(), 2, 96, 0xe0e0e0);
                }

                if (!mc.TheWorld.IsRemote)
                {
                    DrawString(fontrenderer, (new StringBuilder()).Append("Seed: ").Append(mc.TheWorld.GetSeed()).ToString(), 2, 112, 0xe0e0e0);
                }

                //GL.PopMatrix();
            }

            if (recordPlayingUpFor > 0)
            {
                float f2 = (float)recordPlayingUpFor - par1;
                int l1 = (int)((f2 * 256F) / 20F);

                if (l1 > 255)
                {
                    l1 = 255;
                }

                if (l1 > 0)
                {
                    //GL.PushMatrix();
                    //GL.Translate(i / 2, j - 48, 0.0F);
                    //GL.Enable(EnableCap.Blend);
                    //GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
                    int j2 = 0xffffff;
                    /*
                    if (recordIsPlaying)
                    {
                        j2 = Color.HSBtoRGB(f2 / 50F, 0.7F, 0.6F) & 0xffffff;
                    }
                    */
                    fontrenderer.DrawString(recordPlaying, - (int)fontrenderer.GetStringWidth(recordPlaying) / 2, -4, j2 + (l1 << 24));
                    //GL.Disable(EnableCap.Blend);
                    //GL.PopMatrix();
                }
            }

            //GL.Enable(EnableCap.Blend);
            //GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            //GL.Disable(EnableCap.AlphaTest);
            //GL.PushMatrix();
            //GL.Translate(0.0F, j - 48, 0.0F);
            Func_50010_a(fontrenderer);
            //GL.PopMatrix();

            if ((mc.ThePlayer is EntityClientPlayerMP) && mc.GameSettings.KeyBindPlayerList.Pressed)
            {
                NetClientHandler netclienthandler = ((EntityClientPlayerMP)mc.ThePlayer).SendQueue;
                List<GuiPlayerInfo> list = netclienthandler.PlayerNames;
                int k2 = netclienthandler.CurrentServerMaxPlayers;
                int j3 = k2;
                int i4 = 1;

                for (; j3 > 20; j3 = ((k2 + i4) - 1) / i4)
                {
                    i4++;
                }

                int k4 = 300 / i4;

                if (k4 > 150)
                {
                    k4 = 150;
                }

                int i5 = (i - i4 * k4) / 2;
                byte byte0 = 10;
                DrawRect(i5 - 1, byte0 - 1, i5 + k4 * i4, byte0 + 9 * j3, 0x8000000);

                for (int k6 = 0; k6 < k2; k6++)
                {
                    int j7 = i5 + (k6 % i4) * k4;
                    int i8 = byte0 + (k6 / i4) * 9;
                    DrawRect(j7, i8, (j7 + k4) - 1, i8 + 8, 0x20ffffff);
                    //GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
                    //GL.Enable(EnableCap.AlphaTest);

                    if (k6 >= list.Count)
                    {
                        continue;
                    }

                    GuiPlayerInfo guiplayerinfo = (GuiPlayerInfo)list[k6];
                    fontrenderer.DrawStringWithShadow(guiplayerinfo.Name, j7, i8, 0xffffff);
                    mc.RenderEngineOld.BindTexture(mc.RenderEngineOld.GetTexture("/gui/icons.png"));
                    int l9 = 0;
                    byte byte1 = 0;

                    if (guiplayerinfo.ResponseTime < 0)
                    {
                        byte1 = 5;
                    }
                    else if (guiplayerinfo.ResponseTime < 150)
                    {
                        byte1 = 0;
                    }
                    else if (guiplayerinfo.ResponseTime < 300)
                    {
                        byte1 = 1;
                    }
                    else if (guiplayerinfo.ResponseTime < 600)
                    {
                        byte1 = 2;
                    }
                    else if (guiplayerinfo.ResponseTime < 1000)
                    {
                        byte1 = 3;
                    }
                    else
                    {
                        byte1 = 4;
                    }

                    ZLevel += 100F;
                    DrawTexturedModalRect((j7 + k4) - 12, i8, 0 + l9 * 10, 176 + byte1 * 8, 10, 8);
                    ZLevel -= 100F;
                }
            }

            //GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
            //GL.Disable(EnableCap.Lighting);
            //GL.Enable(EnableCap.AlphaTest);
        }

        private void Func_50010_a(FontRenderer par1FontRenderer)
        {
            byte byte0 = 10;
            bool flag = false;
            int i = 0;
            int j = chatMessageList.Count;

            if (j <= 0)
            {
                return;
            }

            if (IsChatOpen())
            {
                byte0 = 20;
                flag = true;
            }

            for (int k = 0; k + field_50017_n < chatMessageList.Count && k < byte0; k++)
            {
                if (chatMessageList[k].UpdateCounter >= 200 && !flag)
                {
                    continue;
                }

                ChatLine chatline = chatMessageList[k + field_50017_n];
                double d = (double)chatline.UpdateCounter / 200D;
                d = 1.0D - d;
                d *= 10D;

                if (d < 0.0F)
                {
                    d = 0.0F;
                }

                if (d > 1.0D)
                {
                    d = 1.0D;
                }

                d *= d;
                int l1 = (int)(255D * d);

                if (flag)
                {
                    l1 = 255;
                }

                i++;

                if (l1 > 2)
                {
                    byte byte1 = 3;
                    int j2 = -k * 9;
                    String s = chatline.Message;
                    DrawRect(byte1, j2 - 1, byte1 + 320 + 4, j2 + 8, l1 / 2 << 24);
                    //GL.Enable(EnableCap.Blend);
                    par1FontRenderer.DrawStringWithShadow(s, byte1, j2, 0xffffff + (l1 << 24));
                }
            }

            if (flag)
            {
                //GL.Translate(0.0F, par1FontRenderer.FontHeight, 0.0F);
                int l = j * par1FontRenderer.FontHeight + j;
                int i1 = i * par1FontRenderer.FontHeight + i;
                int j1 = (field_50017_n * i1) / j;
                int k1 = (i1 * i1) / l;

                if (l != i1)
                {
                    int c = j1 <= 0 ? '`' : 252;
                    int i2 = field_50018_o ? 0xcc3333 : 0x3333aa;
                    DrawRect(0, -j1, 2, -j1 - k1, i2 + (c << 24));
                    DrawRect(2, -j1, 1, -j1 - k1, 0xcccccc + (c << 24));
                }
            }
        }

        ///<summary>
        /// Renders dragon's (boss) health on the HUD
        ///</summary>
        private void RenderBossHealth()
        {
            if (RenderDragon.EntityDragon == null)
            {
                return;
            }

            EntityDragon entitydragon = RenderDragon.EntityDragon;
            RenderDragon.EntityDragon = null;
            FontRendererOld fontrenderer = mc.FontRendererOld;
            ScaledResolution scaledresolution = new ScaledResolution(mc.GameSettings, mc.DisplayWidth, mc.DisplayHeight);
            int i = scaledresolution.GetScaledWidth();
            int c = 266;
            int j = i / 2 - c / 2;
            int k = (int)(((float)entitydragon.Func_41010_ax() / (float)entitydragon.GetMaxHealth()) * (float)(c + 1));
            byte byte0 = 12;
            DrawTexturedModalRect(j, byte0, 0, 74, c, 5);
            DrawTexturedModalRect(j, byte0, 0, 74, c, 5);

            if (k > 0)
            {
                DrawTexturedModalRect(j, byte0, 0, 79, k, 5);
            }

            string s = "Boss health";
            fontrenderer.DrawStringWithShadow(s, i / 2 - fontrenderer.GetStringWidth(s) / 2, byte0 - 10, 0xff00ff);
            //GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
            //GL.BindTexture(TextureTarget.Texture2D, mc.RenderEngineOld.GetTexture("/gui/icons.png"));
        }

        private void RenderPumpkinBlur(int par1, int par2)
        {
            //GL.Disable(EnableCap.DepthTest);
            //GL.DepthMask(false);
            //GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            //GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
            //GL.Disable(EnableCap.AlphaTest);
            //GL.BindTexture(TextureTarget.Texture2D, mc.RenderEngineOld.GetTexture("%blur%/misc/pumpkinblur.png"));
            Tessellator tessellator = Tessellator.Instance;
            tessellator.StartDrawingQuads();
            tessellator.AddVertexWithUV(0.0F, par2, -90D, 0.0F, 1.0D);
            tessellator.AddVertexWithUV(par1, par2, -90D, 1.0D, 1.0D);
            tessellator.AddVertexWithUV(par1, 0.0F, -90D, 1.0D, 0.0F);
            tessellator.AddVertexWithUV(0.0F, 0.0F, -90D, 0.0F, 0.0F);
            tessellator.Draw();
            //GL.DepthMask(true);
            //GL.Enable(EnableCap.DepthTest);
            //GL.Enable(EnableCap.AlphaTest);
            //GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
        }

        ///<summary>
        /// Renders the vignette. Args: vignetteBrightness, width, height
        ///</summary>
        private void RenderVignette(float par1, int par2, int par3)
        {
            par1 = 1.0F - par1;

            if (par1 < 0.0F)
            {
                par1 = 0.0F;
            }

            if (par1 > 1.0F)
            {
                par1 = 1.0F;
            }

            PrevVignetteBrightness += par1 - PrevVignetteBrightness * 0.01F;
            //GL.Disable(EnableCap.DepthTest);
            //GL.DepthMask(false);
            //GL.BlendFunc(BlendingFactorSrc.Zero, BlendingFactorDest.OneMinusSrcColor);
            //GL.Color4(PrevVignetteBrightness, PrevVignetteBrightness, PrevVignetteBrightness, 1.0F);
            //GL.BindTexture(TextureTarget.Texture2D, mc.RenderEngineOld.GetTexture("%blur%/misc/vignette.png"));
            RenderEngine.Instance.BindTexture("misc.vignette.png", TextureMode.Blur);
            Tessellator tessellator = Tessellator.Instance;
            tessellator.StartDrawingQuads();
            tessellator.AddVertexWithUV(0.0F, par3, -90D, 0.0F, 1.0D);
            tessellator.AddVertexWithUV(par2, par3, -90D, 1.0D, 1.0D);
            tessellator.AddVertexWithUV(par2, 0.0F, -90D, 1.0D, 0.0F);
            tessellator.AddVertexWithUV(0.0F, 0.0F, -90D, 0.0F, 0.0F);
            tessellator.Draw();
            //GL.DepthMask(true);
            //GL.Enable(EnableCap.DepthTest);
            //GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
            //GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
        }

        ///<summary>
        /// Renders the portal overlay. Args: portalStrength, width, height
        ///</summary>
        private void RenderPortalOverlay(float par1, int par2, int par3)
        {
            if (par1 < 1.0F)
            {
                par1 *= par1;
                par1 *= par1;
                par1 = par1 * 0.8F + 0.2F;
            }

            //GL.Disable(EnableCap.AlphaTest);
            //GL.Disable(EnableCap.DepthTest);
            //GL.DepthMask(false);
            //GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            //GL.Color4(1.0F, 1.0F, 1.0F, par1);
            //GL.BindTexture(TextureTarget.Texture2D, mc.RenderEngineOld.GetTexture("/terrain.png"));
            RenderEngine.Instance.BindTexture("terrain.png");
            float f = (float)(Block.Portal.BlockIndexInTexture % 16) / 16F;
            float f1 = (float)(Block.Portal.BlockIndexInTexture / 16) / 16F;
            float f2 = (float)(Block.Portal.BlockIndexInTexture % 16 + 1) / 16F;
            float f3 = (float)(Block.Portal.BlockIndexInTexture / 16 + 1) / 16F;
            Tessellator tessellator = Tessellator.Instance;
            tessellator.StartDrawingQuads();
            tessellator.AddVertexWithUV(0.0F, par3, -90D, f, f3);
            tessellator.AddVertexWithUV(par2, par3, -90D, f2, f3);
            tessellator.AddVertexWithUV(par2, 0.0F, -90D, f2, f1);
            tessellator.AddVertexWithUV(0.0F, 0.0F, -90D, f, f1);
            tessellator.Draw();
            //GL.DepthMask(true);
            //GL.Enable(EnableCap.DepthTest);
            //GL.Enable(EnableCap.AlphaTest);
            //GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
        }

        ///<summary>
        /// Renders the specified item of the inventory slot at the specified location. Args: slot, x, y, partialTick
        ///</summary>
        private void RenderInventorySlot(int par1, int par2, int par3, float par4)
        {
            ItemStack itemstack = mc.ThePlayer.Inventory.MainInventory[par1];

            if (itemstack == null)
            {
                return;
            }

            float f = (float)itemstack.AnimationsToGo - par4;

            if (f > 0.0F)
            {
                //GL.PushMatrix();
                float f1 = 1.0F + f / 5F;
                //GL.Translate(par2 + 8, par3 + 12, 0.0F);
                //GL.Scale(1.0F / f1, (f1 + 1.0F) / 2.0F, 1.0F);
                //GL.Translate(-(par2 + 8), -(par3 + 12), 0.0F);
            }

            itemRenderer.RenderItemIntoGUI(mc.FontRenderer, mc.RenderEngineOld, itemstack, par2, par3);

            if (f > 0.0F)
            {
                //GL.PopMatrix();
            }

            itemRenderer.RenderItemOverlayIntoGUI(mc.FontRenderer, mc.RenderEngineOld, itemstack, par2, par3);
        }

        ///<summary>
        /// The update tick for the ingame UI
        ///</summary>
        public void UpdateTick()
        {
            if (recordPlayingUpFor > 0)
            {
                recordPlayingUpFor--;
            }

            updateCounter++;

            for (int i = 0; i < chatMessageList.Count; i++)
            {
                chatMessageList[i].UpdateCounter++;
            }
        }

        ///<summary>
        /// Clear all chat messages.
        ///</summary>
        public void ClearChatMessages()
        {
            chatMessageList.Clear();
            field_50016_f.Clear();
        }

        ///<summary>
        /// Adds a chat message to the list of chat messages. Args: msg
        ///</summary>
        public void AddChatMessage(String par1Str)
        {
            bool flag = IsChatOpen();
            bool flag1 = true;
            string s;

            for (IEnumerator<string> iterator = mc.FontRendererOld.Func_50108_c(par1Str, 320).GetEnumerator(); iterator.MoveNext(); chatMessageList.Insert(0, new ChatLine(s)))
            {
                s = iterator.Current;

                if (flag && field_50017_n > 0)
                {
                    field_50018_o = true;
                    Func_50011_a(1);
                }

                if (!flag1)
                {
                    s = (new StringBuilder()).Append(" ").Append(s).ToString();
                }

                flag1 = false;
            }

            for (; chatMessageList.Count > 100; chatMessageList.RemoveAt(chatMessageList.Count - 1)) { }
        }

        public List<string> Func_50013_c()
        {
            return field_50016_f;
        }

        public void Func_50014_d()
        {
            field_50017_n = 0;
            field_50018_o = false;
        }

        public void Func_50011_a(int par1)
        {
            field_50017_n += par1;
            int i = chatMessageList.Count;

            if (field_50017_n > i - 20)
            {
                field_50017_n = i - 20;
            }

            if (field_50017_n <= 0)
            {
                field_50017_n = 0;
                field_50018_o = false;
            }
        }

        public ChatClickData Func_50012_a(int par1, int par2)
        {
            if (!IsChatOpen())
            {
                return null;
            }

            ScaledResolution scaledresolution = new ScaledResolution(mc.GameSettings, mc.DisplayWidth, mc.DisplayHeight);
            par2 = par2 / scaledresolution.ScaleFactor - 40;
            par1 = par1 / scaledresolution.ScaleFactor - 3;

            if (par1 < 0 || par2 < 0)
            {
                return null;
            }

            int i = Math.Min(20, chatMessageList.Count);

            if (par1 <= 320 && par2 < mc.FontRendererOld.FontHeight * i + i)
            {
                int j = par2 / (mc.FontRendererOld.FontHeight + 1) + field_50017_n;
                return new ChatClickData(mc.FontRendererOld, chatMessageList[j], par1, (par2 - (j - field_50017_n) * mc.FontRendererOld.FontHeight) + j);
            }
            else
            {
                return null;
            }
        }

        public void SetRecordPlayingMessage(String par1Str)
        {
            recordPlaying = (new StringBuilder()).Append("Now playing: ").Append(par1Str).ToString();
            recordPlayingUpFor = 60;
            recordIsPlaying = true;
        }

        ///<summary>
        /// Return true if chat gui is open
        ///</summary>
        public bool IsChatOpen()
        {
            return mc.CurrentScreen is GuiChat;
        }

        ///<summary>
        /// Adds the string to chat message after translate it with the language file.
        ///</summary>
        public void AddChatMessageTranslate(String par1Str)
        {
            StringTranslate stringtranslate = StringTranslate.GetInstance();
            string s = stringtranslate.TranslateKey(par1Str);
            AddChatMessage(s);
        }
    }
}