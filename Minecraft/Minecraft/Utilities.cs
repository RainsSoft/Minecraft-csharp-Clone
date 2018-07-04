using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace net.minecraft.src
{
    public static class Utilities
    {
        public static Rectangle FitToRectangle(this Rectangle r, Rectangle bounds)
        {
            Rectangle result = new Rectangle();

            float sourceAspect = (float)r.Width / r.Height;
            float destAspect = (float)bounds.Width / bounds.Height;

            if (destAspect > sourceAspect)
            {
                result.Height = bounds.Height;
                result.Width = (int)(result.Height * sourceAspect);
            }

            result.X = (bounds.Width - result.Width) / 2;

            return result;
        }

        public static string[] Split(this string s, char separator, bool keepDelimiter)
        {
            if (!keepDelimiter) return s.Split(separator);

            List<string> strings = new List<string>();

            int splitIndex = 0;

            for (int i = 0; i < s.Length; i++)
            {
                var c = s[i];

                if (c == separator)
                {
                    var sub = s.Substring(splitIndex, i - splitIndex);
                    if (sub.Length > 0)
                        strings.Add(sub);
                    splitIndex = i;
                }
            }

            if (splitIndex != s.Length)
            {
                strings.Add(s.Substring(splitIndex, s.Length - splitIndex));
            }

            return strings.ToArray();
        }

        /// <summary>
        /// Creates an ARGB hex string representation of the <see cref="Color"/> value.
        /// </summary>
        /// <param name="color">The <see cref="Color"/> value to parse.</param>
        /// <param name="includeHash">Determines whether to include the hash mark (#) character in the string.</param>
        /// <returns>A hex string representation of the specified <see cref="Color"/> value.</returns>
        public static string ToHex(this Color color, bool includeHash)
        {
            string[] argb =
            {
                color.A.ToString("X2"),
                color.R.ToString("X2"),
                color.G.ToString("X2"),
                color.B.ToString("X2"),
            };

            return (includeHash ? "#" : string.Empty) + string.Join(string.Empty, argb);
        }

        /// Creates a <see cref="Color"/> value from an ARGB or RGB hex string.  The string may
        /// begin with or without the hash mark (#) character.
        /// </summary>
        /// <param name="hexString">The ARGB hex string to parse.</param>
        /// <returns>
        /// A <see cref="Color"/> value as defined by the ARGB or RGB hex string.
        /// </returns>
        /// <exception cref="InvalidOperationException">Thrown if the string is not a valid ARGB or RGB hex value.</exception>
        public static Color ColorFromString(this string hexString)
        {
            if (hexString.StartsWith("#"))
                hexString = hexString.Substring(1);

            uint hex = uint.Parse(hexString, NumberStyles.HexNumber, CultureInfo.InvariantCulture);

            Color color = Color.White;
            if (hexString.Length == 8)
            {
                color.A = (byte)(hex >> 24);
                color.R = (byte)(hex >> 16);
                color.G = (byte)(hex >> 8);
                color.B = (byte)(hex);
            }
            else if (hexString.Length == 6)
            {
                color.R = (byte)(hex >> 16);
                color.G = (byte)(hex >> 8);
                color.B = (byte)(hex);
            }
            else
            {
                throw new InvalidOperationException("Invald hex representation of an ARGB or RGB color value.");
            }
            return color;
        }

        public static Color ColorFromHex(int hexcode)
        {
            Color color = Color.White;
            //if (hexcode.Length == 8)
            {
                color.A = (byte)(hexcode >> 24);
                color.R = (byte)(hexcode >> 16);
                color.G = (byte)(hexcode >> 8);
                color.B = (byte)(hexcode);
            }/*
            else if (hexcode.Length == 6)
            {
                color.R = (byte)(hex >> 16);
                color.G = (byte)(hex >> 8);
                color.B = (byte)(hex);
            }
            else
            {
                throw new InvalidOperationException("Invald hex representation of an ARGB or RGB color value.");
            }*/
            return color;
        }

        public static void LogException(Exception e)
        {
            Console.WriteLine(e.ToString());
            Console.WriteLine("<<<<<<< Stack Trace >>>>>>>");
            Console.WriteLine(e.StackTrace);
            Console.WriteLine();
        }
    }
}
