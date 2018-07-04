using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace net.minecraft.src
{
    public struct ColoredString
    {
        public Color Color;

        public string Text;

        public ColoredString(string text, Color color)
        {
            Text = text;
            Color = color;
        }
    }

    public class FontRenderer
    {
        Minecraft mc;

        RenderEngine renderEngine;

        SpriteFont font;

        public static char SpecialChar = (char)167;

        int[] colorCodes;

        public Random FontRandom;

        public int FontHeight;

        public FontRenderer(Minecraft mc)
        {
            this.mc = mc;
            font = mc.Content.Load<SpriteFont>("VolterGoldfish");
            renderEngine = mc.RenderEngine;
            FontRandom = new Random();
            FontHeight = font.LineSpacing;
            colorCodes = new int[32];

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

                if (mc.GameSettings.Anaglyph)
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

                colorCodes[l] = (l1 & 0xff) << 16 | (j2 & 0xff) << 8 | l2 & 0xff;
            }
        }

        public string SanitizeString(string text)
        {
            foreach (char c in text)
            {
                if (!font.Characters.Contains(c))
                    text.Replace(c, ' ');
            }

            return text;
        }

        public string RemoveColorCodes(string text)
        {
            var array = text.Split(SpecialChar, true);

            for (int i = 0; i < array.Length; i++)
            {
                array[i] = array[i].Substring(2);
            }

            return string.Concat(array);
        }

        public ColoredString[] ExtractColors(string text)
        {
            var strings = new List<ColoredString>();

            var array = text.Split(SpecialChar, true);

            foreach (string s in array)
            {
                if (s[0] == SpecialChar)
                {
                    var colorChar = s[1];

                    strings.Add(new ColoredString(s.Substring(2), GetCharacterColor(colorChar)));
                }
            }

            return strings.ToArray();
        }

        public float GetTextScale()
        {
            if (mc.GameSettings.GuiScale == 0) return 1;
            return mc.GameSettings.GuiScale / 2F;
        }

        public int GetStringWidth(string text)
        {
            if (text.Contains(SpecialChar))
                text = RemoveColorCodes(text);
            return (int)font.MeasureString(SanitizeString(text)).X / 2;
        }

        public Color GetCharacterColor(char c)
        {
            int j = "0123456789abcdefklmnor".IndexOf(c);

            if (j < 16)
            {
                if (j < 0 || j > 15)
                {
                    j = 15;
                }

                int l = colorCodes[j];
                return new Color((float)(l >> 16) / 255F, (float)(l >> 8 & 0xff) / 255F, (float)(l & 0xff) / 255F);
            }

            return Color.White;
        }

        ///<summary>
        /// Returns the width of the wordwrapped String (maximum length is parameter k)
        ///</summary>
        public int SplitStringWidth(string par1Str, int par2)
        {
            var stringar = par1Str.Split('\n');

            if (stringar.Length > 0)
            {
                int width = 0;

                for (int i = 0; i < stringar.Length; i++)
                {
                    width = Math.Max(width, GetStringWidth(stringar[i]));
                }

                return width;
            }

            return GetStringWidth(par1Str);
        }

        private void DrawColoredText(ColoredString[] strings, int x, int y)
        {
            float totalLength = 0;

            foreach (ColoredString s in strings)
            {
                DrawString(s.Text, x + totalLength, y, s.Color);

                totalLength += GetStringWidth(s.Text);
            }
        }

        private void DrawString(string text, float x, float y, Color color)
        {
            var scaler = renderEngine.GetDisplayScaler();

            renderEngine.SpriteBatch.DrawString(font, SanitizeString(text), new Vector2(x, y) * scaler, color, 0, new Vector2(0, 4), GetTextScale(), SpriteEffects.None, 0);
        }

        public void DrawString(string text, int x, int y, int color)
        {
            if (text.Contains(SpecialChar))
            {
                DrawColoredText(ExtractColors(text), x, y);
            }
            else DrawString(text, x, y, Utilities.ColorFromHex(color));
        }

        public void DrawStringWithShadow(string text, int x, int y, int color)
        {
            if (text.Contains(SpecialChar))
                DrawString(RemoveColorCodes(text), x + 0.5f, y + 0.5f, Color.Black);
            else DrawString(text, x + 0.5f, y + 0.5f, Color.Black);

            DrawString(text, x, y, color);
        }

        public void DrawSplitString(string text, int x, int y, int other, int other2)
        {
        }

        public int Func_50101_a(string par1Str, int par2, int par3, int par4, bool par5)
        {/*
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
            else*/
            {
                return 0;
            }
        }

        public int Func_50105_a(int par1)
        {
            if (par1 == 247)
            {
                return -1;
            }

            int i = ChatAllowedCharacters.AllowedCharacters.IndexOf((char)par1);
            /*
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
            }*/
            return 0;
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
    }
}
