using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace net.minecraft.src
{
    public class FontRendererOld
    {
        //private static readonly Pattern field_52015_r = Pattern.compile("(?i)\\u00A7[0-9A-FK-OR]");
        private int[] charWidth;
        public string FontTextureName;

        /// <summary>
        /// the height in pixels of default text </summary>
        public int FontHeight;

        public Random FontRandom;
        private byte[] glyphWidth;
        private readonly string[] glyphTextureName;
        private int[] colorCode;

        /// <summary>
        /// The currently bound GL texture ID. Avoids unnecessary BindTexture() for the same texture if it's already bound.
        /// </summary>
        private string boundTextureName;

        /// <summary>
        /// The RenderEngine used to load and setup glyph textures. </summary>
        private readonly RenderEngine renderEngine;

        /// <summary>
        /// Current X coordinate at which to draw the next character. </summary>
        private float posX;

        /// <summary>
        /// Current Y coordinate at which to draw the next character. </summary>
        private float posY;

        /// <summary>
        /// If true, strings should be rendered with Unicode fonts instead of the default.png font
        /// </summary>
        private bool unicodeFlag;

        /// <summary>
        /// If true, the Unicode Bidirectional Algorithm should be run before rendering any string.
        /// </summary>
        private bool bidiFlag;
        private float field_50115_n;
        private float field_50116_o;
        private float field_50118_p;
        private float field_50117_q;

        FontRendererOld()
        {
            charWidth = new int[256];
            FontTextureName = "";
            FontHeight = 8;
            FontRandom = new Random();
            glyphWidth = new byte[0x10000];
            glyphTextureName = new string[256];
            colorCode = new int[32];
            renderEngine = null;
        }

        public FontRendererOld(GameSettings par1GameSettings, string par2Str, RenderEngine par3RenderEngine, bool par4)
        {
            charWidth = new int[256];
            FontTextureName = "";
            FontHeight = 8;
            FontRandom = new Random();
            glyphWidth = new byte[0x10000];
            glyphTextureName = new string[256];
            colorCode = new int[32];
            renderEngine = par3RenderEngine;
            unicodeFlag = par4;
            Texture2D bufferedimage;

            try
            {
                bufferedimage = RenderEngine.Instance.GetTexture(par2Str);
                Stream inputstream = Minecraft.GetResourceStream("font.glyph_sizes.bin");
                inputstream.Read(glyphWidth, 0, 0x10000);
            }
            catch (IOException ioexception)
            {
                Console.WriteLine(ioexception.ToString());
                Console.WriteLine();

                throw ioexception;
            }

            int i = bufferedimage.Width;
            int j = bufferedimage.Height;
            int[] ai = new int[i * j];
            bufferedimage.GetData<int>(ai);
            //bufferedimage.getRGB(0, 0, i, j, ai, 0, i);
            
            for (int k = 0; k < 256; k++)
            {
                int i1 = k % 16;
                int k1 = k / 16;
                int i2 = 7;
                /*
                do
                {
                    if (i2 < 0)
                    {
                        break;
                    }

                    int k2 = i1 * 8 + i2;
                    bool flag = true;

                    for (int j3 = 0; j3 < 8 && flag; j3++)
                    {
                        int l3 = (k1 * 8 + j3) * i;
                        int j4 = ai[k2 + l3] & 0xff;

                        if (j4 > 0)
                        {
                            flag = false;
                        }
                    }

                    if (!flag)
                    {
                        break;
                    }

                    i2--;
                }
                while (true);
                */
                if (k == 32)
                {
                    i2 = 2;
                }

                charWidth[k] = i2 + 2;
            }
            
            FontTextureName = par3RenderEngine.AllocateTexture(bufferedimage);
            
            for (int l = 0; l < 32; l++)
            {
                int j1 = (l >> 3 & 1) * 85;
                int l1 = (l >> 2 & 1) * 170 + j1;
                int j2 = (l >> 1 & 1) * 170 + j1;
                int l2 = (l >> 0 & 1) * 170 + j1;

                if (l == 6)
                {
                    l1 += 85;
                }

                if (par1GameSettings.Anaglyph)
                {
                    int i3 = (l1 * 30 + j2 * 59 + l2 * 11) / 100;
                    int k3 = (l1 * 30 + j2 * 70) / 100;
                    int i4 = (l1 * 30 + l2 * 70) / 100;
                    l1 = i3;
                    j2 = k3;
                    l2 = i4;
                }

                if (l >= 16)
                {
                    l1 /= 4;
                    j2 /= 4;
                    l2 /= 4;
                }

                colorCode[l] = (l1 & 0xff) << 16 | (j2 & 0xff) << 8 | l2 & 0xff;
            }
        }

        private float Func_50112_a(int par1, char par2, bool par3)
        {
            if (par2 == ' ')
            {
                return 4F;
            }

            if (par1 > 0 && !unicodeFlag)
            {
                return Func_50106_a(par1 + 32, par3);
            }
            else
            {
                return Func_50111_a(par2, par3);
            }
        }

        private float Func_50106_a(int par1, bool par2)
        {
            float f = (par1 % 16) * 8;
            float f1 = (par1 / 16) * 8;
            float f2 = par2 ? 1.0F : 0.0F;

            if (boundTextureName != FontTextureName)
            {
                //GL.BindTexture(TextureTarget.Texture2D, FontTextureName);
                RenderEngine.Instance.BindTexture(FontTextureName);
                boundTextureName = FontTextureName;
            }

            float f3 = (float)charWidth[par1] - 0.01F;
            //GL.Begin(BeginMode.TriangleStrip);
            //GL.TexCoord2(f / 128F, f1 / 128F);
            //GL.Vertex3(posX + f2, posY, 0.0F);
            //GL.TexCoord2(f / 128F, (f1 + 7.99F) / 128F);
            //GL.Vertex3(posX - f2, posY + 7.99F, 0.0F);
            //GL.TexCoord2((f + f3) / 128F, f1 / 128F);
            //GL.Vertex3(posX + f3 + f2, posY, 0.0F);
            //GL.TexCoord2((f + f3) / 128F, (f1 + 7.99F) / 128F);
            //GL.Vertex3((posX + f3) - f2, posY + 7.99F, 0.0F);
            //GL.End();
            //renderEngine.RenderSprite(new Rectangle((int)posX, (int)posY, (int)f2, 8), new Rectangle((int)f, (int)f1, (int)f3, 16));
            renderEngine.RenderSprite(new Rectangle(-(int)posX, 100, 9, 8), new RectangleF(f, f1, f3, 16));

            return (float)charWidth[par1];
        }

        /// <summary>
        /// Load one of the /font/glyph_XX.png into a new GL texture and store the texture ID in glyphTextureName array.
        /// </summary>
        private void LoadGlyphTexture(int par1)
        {
            string s = string.Format("font.glyph_{0:X2}.png", par1);
            Texture2D bufferedimage;

            try
            {
                bufferedimage = renderEngine.GetTexture(s);
            }
            catch (IOException ioexception)
            {
                Console.WriteLine(ioexception.ToString());
                Console.WriteLine();

                throw ioexception;
            }

            glyphTextureName[par1] = renderEngine.AllocateTexture(bufferedimage);
            boundTextureName = glyphTextureName[par1];
        }

        private float Func_50111_a(char par1, bool par2)
        {
            if (glyphWidth[par1] == 0)
            {
                return 0.0F;
            }

            int i = par1 / 256;

            if (glyphTextureName[i] == "")
            {
                LoadGlyphTexture(i);
            }

            if (boundTextureName != glyphTextureName[i])
            {
                //GL.BindTexture(TextureTarget.Texture2D, glyphTextureName[i]);
                renderEngine.BindTexture(glyphTextureName[i]);
                boundTextureName = glyphTextureName[i];
            }

            int j = (int)((uint)glyphWidth[par1] >> 4);
            int k = glyphWidth[par1] & 0xf;
            float f = j;
            float f1 = k + 1;
            float f2 = (float)((par1 % 16) * 16) + f;
            float f3 = ((par1 & 0xff) / 16) * 16;
            float f4 = f1 - f - 0.02F;
            float f5 = par2 ? 1.0F : 0.0F;/*
            GL.Begin(BeginMode.TriangleStrip);
            GL.TexCoord2(f2 / 256F, f3 / 256F);
            GL.Vertex3(posX + f5, posY, 0.0F);
            GL.TexCoord2(f2 / 256F, (f3 + 15.98F) / 256F);
            GL.Vertex3(posX - f5, posY + 7.99F, 0.0F);
            GL.TexCoord2((f2 + f4) / 256F, f3 / 256F);
            GL.Vertex3(posX + f4 / 2.0F + f5, posY, 0.0F);
            GL.TexCoord2((f2 + f4) / 256F, (f3 + 15.98F) / 256F);
            GL.Vertex3((posX + f4 / 2.0F) - f5, posY + 7.99F, 0.0F);
            GL.End();*/
            renderEngine.RenderSprite(new Rectangle((int)posX, (int)posY, (int)f5, 8), new RectangleF(f2, f3, f4, 16));

            return (f1 - f) / 2.0F + 1.0F;
        }

        /// <summary>
        /// Draws the specified string with a shadow.
        /// </summary>
        public virtual int DrawStringWithShadow(string par1Str, int par2, int par3, int par4)
        {
            if (bidiFlag)
            {
                par1Str = BidiReorder(par1Str);
            }

            int i = Func_50101_a(par1Str, par2 + 1, par3 + 1, par4, true);
            i = Math.Max(i, Func_50101_a(par1Str, par2, par3, par4, false));
            return i;
        }

        /// <summary>
        /// Draws the specified string.
        /// </summary>
        public virtual void DrawString(string par1Str, int par2, int par3, int par4)
        {
            if (bidiFlag)
            {
                par1Str = BidiReorder(par1Str);
            }

            Func_50101_a(par1Str, par2, par3, par4, false);
        }

        /// <summary>
        /// Apply Unicode Bidirectional Algorithm to string and return a new possibly reordered string for visual rendering.
        /// </summary>
        private string BidiReorder(string par1Str)
        {/*
            if (par1Str == null || !Bidi.requiresBidi(par1Str.ToCharArray(), 0, par1Str.Length))
            {
                return par1Str;
            }

            Bidi bidi = new Bidi(par1Str, -2);
            sbyte[] abyte0 = new sbyte[bidi.getRunCount()];
            string[] @as = new string[abyte0.Length];

            for (int i = 0; i < abyte0.Length; i++)
            {
                int j = bidi.getRunStart(i);
                int k = bidi.getRunLimit(i);
                int i1 = bidi.getRunLevel(i);
                string s = par1Str.Substring(j, k - j);
                abyte0[i] = (sbyte)i1;
                @as[i] = s;
            }
            
            string[] as1 = (string[])@as.Clone();
            Bidi.reorderVisually(abyte0, 0, @as, 0, abyte0.Length);
            StringBuilder stringbuilder = new StringBuilder();
        label0:

            for (int l = 0; l < @as.Length; l++)
            {
                sbyte byte0 = abyte0[l];
                int j1 = 0;

                do
                {
                    if (j1 >= as1.Length)
                    {
                        break;
                    }

                    if (as1[j1].Equals(@as[l]))
                    {
                        byte0 = abyte0[j1];
                        break;
                    }

                    j1++;
                }
                while (true);

                if ((byte0 & 1) == 0)
                {
                    stringbuilder.Append(@as[l]);
                    continue;
                }

                j1 = @as[l].Length - 1;

                do
                {
                    if (j1 < 0)
                    {
                        goto label0;
                    }

                    char c = @as[l][j1];

                    if (c == '(')
                    {
                        c = ')';
                    }
                    else if (c == ')')
                    {
                        c = '(';
                    }

                    stringbuilder.Append(c);
                    j1--;
                }
                while (true);
            }

            return stringbuilder.ToString();*/
            return "";
        }

        /// <summary>
        /// Render a single line string at the current (posX, posY) and update posX
        /// </summary>
        private void RenderStringAtPos(string par1Str, bool par2)
        {
            bool flag = false;
            bool flag1 = false;
            bool flag2 = false;
            bool flag3 = false;
            bool flag4 = false;

            for (int i = 0; i < par1Str.Length; i++)
            {
                char c = par1Str[i];

                if (c == 247 && i + 1 < par1Str.Length)
                {
                    int j = "0123456789abcdefklmnor".IndexOf(par1Str.ToLower()[i + 1]);

                    if (j < 16)
                    {
                        flag = false;
                        flag1 = false;
                        flag4 = false;
                        flag3 = false;
                        flag2 = false;

                        if (j < 0 || j > 15)
                        {
                            j = 15;
                        }

                        if (par2)
                        {
                            j += 16;
                        }

                        int l = colorCode[j];
                        //GL.Color3((float)(l >> 16) / 255F, (float)(l >> 8 & 0xff) / 255F, (float)(l & 0xff) / 255F);
                    }
                    else if (j == 16)
                    {
                        flag = true;
                    }
                    else if (j == 17)
                    {
                        flag1 = true;
                    }
                    else if (j == 18)
                    {
                        flag4 = true;
                    }
                    else if (j == 19)
                    {
                        flag3 = true;
                    }
                    else if (j == 20)
                    {
                        flag2 = true;
                    }
                    else if (j == 21)
                    {
                        flag = false;
                        flag1 = false;
                        flag4 = false;
                        flag3 = false;
                        flag2 = false;
                        //GL.Color4(field_50115_n, field_50116_o, field_50118_p, field_50117_q);
                    }

                    i++;
                    continue;
                }

                int k = ChatAllowedCharacters.AllowedCharacters.IndexOf(c);

                if (flag && k > 0)
                {
                    int i1;

                    do
                    {
                        i1 = FontRandom.Next(ChatAllowedCharacters.AllowedCharacters.Length);
                    }
                    while (charWidth[k + 32] != charWidth[i1 + 32]);

                    k = i1;
                }

                float f = Func_50112_a(k, c, flag2);

                if (flag1)
                {
                    posX++;
                    Func_50112_a(k, c, flag2);
                    posX--;
                    f++;
                }

                if (flag4)
                {/*
                    Tessellator tessellator = Tessellator.Instance;
                    GL.Disable(EnableCap.Texture2D);
                    tessellator.StartDrawingQuads();
                    tessellator.AddVertex(posX, posY + (float)(FontHeight / 2), 0.0F);
                    tessellator.AddVertex(posX + f, posY + (float)(FontHeight / 2), 0.0F);
                    tessellator.AddVertex(posX + f, (posY + (float)(FontHeight / 2)) - 1.0F, 0.0F);
                    tessellator.AddVertex(posX, (posY + (float)(FontHeight / 2)) - 1.0F, 0.0F);
                    tessellator.Draw();
                    GL.Enable(EnableCap.Texture2D);*/

                    renderEngine.RenderSprite(new Rectangle((int)posX, (int)posY + FontHeight / 2, 1, (int)f), null);
                }

                if (flag3)
                {/*
                    Tessellator tessellator1 = Tessellator.Instance;
                    GL.Disable(EnableCap.Texture2D);
                    tessellator1.StartDrawingQuads();*/
                    int j1 = flag3 ? -1 : 0;/*
                    tessellator1.AddVertex(posX + (float)j1, posY + (float)FontHeight, 0.0F);
                    tessellator1.AddVertex(posX + f, posY + (float)FontHeight, 0.0F);
                    tessellator1.AddVertex(posX + f, (posY + (float)FontHeight) - 1.0F, 0.0F);
                    tessellator1.AddVertex(posX + (float)j1, (posY + (float)FontHeight) - 1.0F, 0.0F);
                    tessellator1.Draw();
                    GL.Enable(EnableCap.Texture2D);*/

                    renderEngine.RenderSprite(new Rectangle((int)posX, (int)posY + FontHeight, (int)(f - j1), (int)f), null);
                }

                posX += f;
            }
        }

        public int Func_50101_a(string par1Str, int par2, int par3, int par4, bool par5)
        {
            if (par1Str != null)
            {
                boundTextureName = "";

                if ((par4 & 0xfc000000) == 0)
                {
                    par4 |= 0xff00000;
                }

                if (par5)
                {
                    par4 = (par4 & 0xfcfcfc) >> 2 | par4 & 0xff00000;
                }

                field_50115_n = (float)(par4 >> 16 & 0xff) / 255F;
                field_50116_o = (float)(par4 >> 8 & 0xff) / 255F;
                field_50118_p = (float)(par4 & 0xff) / 255F;
                field_50117_q = (float)(par4 >> 24 & 0xff) / 255F;
                //GL.Color4(field_50115_n, field_50116_o, field_50118_p, field_50117_q);
                posX = par2;
                posY = par3;
                RenderStringAtPos(par1Str, par5);
                return (int)posX;
            }
            else
            {
                return 0;
            }
        }

        ///<summary>
        // Returns the width of this string. Equivalent of FontMetrics.stringWidth(String s).
        ///</summary>
        public int GetStringWidth(string par1Str)
        {
            if (par1Str == null)
            {
                return 0;
            }

            int i = 0;
            bool flag = false;

            for (int j = 0; j < par1Str.Length; j++)
            {
                char c = par1Str[j];
                int k = Func_50105_a(c);

                if (k < 0 && j < par1Str.Length - 1)
                {
                    char c1 = par1Str[++j];

                    if (c1 == 'l' || c1 == 'L')
                    {
                        flag = true;
                    }
                    else if (c1 == 'r' || c1 == 'R')
                    {
                        flag = false;
                    }

                    k = Func_50105_a(c1);
                }

                i += k;

                if (flag)
                {
                    i++;
                }
            }

            return i;
        }

        public int Func_50105_a(int par1)
        {
            if (par1 == 247)
            {
                return -1;
            }

            int i = ChatAllowedCharacters.AllowedCharacters.IndexOf((char)par1);

            if (i >= 0 && !unicodeFlag)
            {
                return charWidth[i + 32];
            }

            if (glyphWidth[par1] != 0)
            {
                int j = glyphWidth[par1] >> 4;
                int k = glyphWidth[par1] & 0xf;

                if (k > 7)
                {
                    k = 15;
                    j = 0;
                }

                return (++k - j) / 2 + 1;
            }
            else
            {
                return 0;
            }
        }

        public string Func_50107_a(string par1Str, int par2)
        {
            return Func_50104_a(par1Str, par2, false);
        }

        public string Func_50104_a(string par1Str, int par2, bool par3)
        {
            StringBuilder stringbuilder = new StringBuilder();
            int i = 0;
            int j = par3 ? par1Str.Length - 1 : 0;
            byte byte0 = (byte)(par3 ? -1 : 1);
            bool flag = false;
            bool flag1 = false;

            for (int k = j; k >= 0 && k < par1Str.Length && i < par2; k += byte0)
            {
                char c = par1Str[k];
                int l = Func_50105_a(c);

                if (flag)
                {
                    flag = false;

                    if (c == 'l' || c == 'L')
                    {
                        flag1 = true;
                    }
                    else if (c == 'r' || c == 'R')
                    {
                        flag1 = false;
                    }
                }
                else if (l < 0)
                {
                    flag = true;
                }
                else
                {
                    i += l;

                    if (flag1)
                    {
                        i++;
                    }
                }

                if (i > par2)
                {
                    break;
                }

                if (par3)
                {
                    stringbuilder.Insert(0, c);
                }
                else
                {
                    stringbuilder.Append(c);
                }
            }

            return stringbuilder.ToString();
        }

        ///<summary>
        /// Splits and draws a String with wordwrap (maximum length is parameter k)
        ///</summary>
        public void DrawSplitString(string par1Str, int par2, int par3, int par4, int par5)
        {/*
            if (bidiFlag)
            {
                par1Str = bidiReorder(par1Str);
            }
            */
            RenderSplitStringNoShadow(par1Str, par2, par3, par4, par5);
        }

        ///<summary>
        /// renders a multi-line string with wordwrap (maximum length is parameter k) by means of renderSplitString
        ///</summary>
        private void RenderSplitStringNoShadow(string par1Str, int par2, int par3, int par4, int par5)
        {
            RenderSplitString(par1Str, par2, par3, par4, par5, false);
        }

        ///<summary>
        /// Perform actual work of rendering a multi-line string with wordwrap (maximum length is parameter k) and with
        /// darkre drop shadow color if flag is set
        ///</summary>
        private void RenderSplitString(string par1Str, int par2, int par3, int par4, int par5, bool par6)
        {
            string[] as0 = par1Str.Split('\n');

            if (as0.Length > 1)
            {
                for (int i = 0; i < as0.Length; i++)
                {
                    RenderSplitStringNoShadow(as0[i], par2, par3, par4, par5);
                    par3 += SplitStringWidth(as0[i], par4);
                }

                return;
            }

            string[] as1 = par1Str.Split(' ');
            int j = 0;
            string s = "";

            do
            {
                if (j >= as1.Length)
                {
                    break;
                }

                string s1;

                for (s1 = (new StringBuilder()).Append(s).Append(as1[j++]).Append(" ").ToString();
                    j < as1.Length && GetStringWidth((new StringBuilder()).Append(s1).Append(as1[j]).ToString()) < par4;
                    s1 = (new StringBuilder()).Append(s1).Append(as1[j++]).Append(" ").ToString()) { }

                int k;

                for (; GetStringWidth(s1) > par4; s1 = (new StringBuilder()).Append(s).Append(s1.Substring(k)).ToString())
                {
                    for (k = 0; GetStringWidth(s1.Substring(0, k + 1)) <= par4; k++) { }

                    if (s1.Substring(0, k).Trim().Length <= 0)
                    {
                        continue;
                    }

                    string s2 = s1.Substring(0, k);

                    if (s2.LastIndexOf((char)247) >= 0)
                    {
                        s = (new StringBuilder()).Append((char)247).Append(s2[s2.LastIndexOf((char)247) + 1]).ToString();
                    }

                    Func_50101_a(s2, par2, par3, par5, par6);
                    par3 += FontHeight;
                }

                if (GetStringWidth(s1.Trim()) > 0)
                {
                    if (s1.LastIndexOf((char)247) >= 0)
                    {
                        s = (new StringBuilder()).Append((char)247).Append(s1[s1.LastIndexOf((char)247) + 1]).ToString();
                    }

                    Func_50101_a(s1, par2, par3, par5, par6);
                    par3 += FontHeight;
                }
            }
            while (true);
        }

        ///<summary>
        /// Returns the width of the wordwrapped String (maximum length is parameter k)
        ///</summary>
        public int SplitStringWidth(string par1Str, int par2)
        {
            string[] as0 = par1Str.Split('\n');

            if (as0.Length > 1)
            {
                int i = 0;

                for (int j = 0; j < as0.Length; j++)
                {
                    i += SplitStringWidth(as0[j], par2);
                }

                return i;
            }

            string[] as1 = par1Str.Split(' ');
            int k = 0;
            int l = 0;

            do
            {
                if (k >= as1.Length)
                {
                    break;
                }

                string s;

                for (s = (new StringBuilder()).Append(as1[k++]).Append(" ").ToString(); k < as1.Length && GetStringWidth((new StringBuilder()).Append(s).Append(as1[k]).ToString()) < par2; s = (new StringBuilder()).Append(s).Append(as1[k++]).Append(" ").ToString()) { }

                int i1;

                for (; GetStringWidth(s) > par2; s = s.Substring(i1))
                {
                    for (i1 = 0; GetStringWidth(s.Substring(0, i1 + 1)) <= par2; i1++) { }

                    if (s.Substring(0, i1).Trim().Length > 0)
                    {
                        l += FontHeight;
                    }
                }

                if (s.Trim().Length > 0)
                {
                    l += FontHeight;
                }
            }
            while (true);

            if (l < FontHeight)
            {
                l += FontHeight;
            }

            return l;
        }

        ///<summary>
        /// Set unicodeFlag controlling whether strings should be rendered with Unicode fonts instead of the default.png
        /// font.
        ///</summary>
        public void SetUnicodeFlag(bool par1)
        {
            unicodeFlag = par1;
        }

        ///<summary>
        /// Set bidiFlag to control if the Unicode Bidirectional Algorithm should be run before rendering any string.
        ///</summary>
        public void SetBidiFlag(bool par1)
        {
            bidiFlag = par1;
        }

        public List<string> Func_50108_c(string par1Str, int par2)
        {
            return Func_50113_d(par1Str, par2).Split('\n').ToList();
        }

        string Func_50113_d(string par1Str, int par2)
        {
            int i = Func_50102_e(par1Str, par2);

            if (par1Str.Length <= i)
            {
                return par1Str;
            }
            else
            {
                string s = par1Str.Substring(0, i);
                string s1 = (new StringBuilder()).Append(Func_50114_c(s)).Append(par1Str.Substring(i + (par1Str[i] != ' ' ? 0 : 1))).ToString();
                return (new StringBuilder()).Append(s).Append("\n").Append(Func_50113_d(s1, par2)).ToString();
            }
        }

        private int Func_50102_e(string par1Str, int par2)
        {
            int i = par1Str.Length;
            int j = 0;
            int k = 0;
            int l = -1;
            bool flag = false;

            do
            {
                if (k >= i)
                {
                    break;
                }

                int c = par1Str[k];

                switch (c)
                {
                    case 167:
                        if (k != i)
                        {
                            char c1 = par1Str[++k];

                            if (c1 == 'l' || c1 == 'L')
                            {
                                flag = true;
                            }
                            else if (c1 == 'r' || c1 == 'R')
                            {
                                flag = false;
                            }
                        }

                        break;

                    case 32:
                        l = k;
                        break;

                    default:
                        j += Func_50105_a(c);

                        if (flag)
                        {
                            j++;
                        }

                        break;
                }

                if (c == '\n')
                {
                    l = ++k;
                    break;
                }

                if (j > par2)
                {
                    break;
                }

                k++;
            }
            while (true);

            if (k != i && l != -1 && l < k)
            {
                return l;
            }
            else
            {
                return k;
            }
        }

        private static bool Func_50110_b(char par0)
        {
            return par0 >= '0' && par0 <= '9' || par0 >= 'a' && par0 <= 'f' || par0 >= 'A' && par0 <= 'F';
        }

        private static bool Func_50109_c(char par0)
        {
            return par0 >= 'k' && par0 <= 'o' || par0 >= 'K' && par0 <= 'O' || par0 == 'r' || par0 == 'R';
        }

        private static string Func_50114_c(string par0Str)
        {
            string s = "";
            int i = -1;
            int j = par0Str.Length;

            do
            {
                if ((i = par0Str.IndexOf((char)247, i + 1)) == -1)
                {
                    break;
                }

                if (i < j - 1)
                {
                    char c = par0Str[i + 1];

                    if (Func_50110_b(c))
                    {
                        s = (new StringBuilder()).Append((char)247).Append(c).ToString();
                    }
                    else if (Func_50109_c(c))
                    {
                        s = (new StringBuilder()).Append(s).Append((char)247).Append(c).ToString();
                    }
                }
            }
            while (true);

            return s;
        }

        public static string Func_52014_d(string par0Str)
        {
            return "";// field_52015_r.matcher(par0Str).replaceAll("");
        }
    }
}