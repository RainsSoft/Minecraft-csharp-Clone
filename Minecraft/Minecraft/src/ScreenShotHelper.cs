using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework;
using IOPath = System.IO.Path;

namespace net.minecraft.src
{
	public class ScreenShotHelper
	{
		private static string DateFormat = "yyyy-MM-dd_HH.mm.ss";
		//private static Buffer Buffer;
		private static sbyte[] PixelData;
		private static int[] ImageData;

		/// <summary>
		/// Takes a screenshot and saves it to the screenshots directory. Returns the filename of the screenshot.
		/// </summary>
		public static string SaveScreenshot(string par0File, int width, int height)
		{
			return SaveScreenshot(par0File, null, width, height);
		}

		public static string SaveScreenshot(string par0File, string par1Str, int width, int height)
		{
			try
			{
				string file = IOPath.Combine(par0File, "screenshots");
				Directory.CreateDirectory(file);

				//GL.PixelStore(PixelStoreParameter.PackAlignment, 1);
				//GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);
                /*
				Bitmap bufferedimage = new Bitmap(width, height);
                BitmapData data = bufferedimage.LockBits(new Rectangle(0, 0, bufferedimage.Width, bufferedimage.Height), ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

				//GL.ReadPixels(0, 0, width, height, PixelFormat.Rgb, PixelType.UnsignedByte, data.Scan0);
                bufferedimage.UnlockBits(data);
                bufferedimage.RotateFlip(RotateFlipType.RotateNoneFlipY);
                */
				string s = new StringBuilder().Append("").Append(DateTime.Now.ToString(DateFormat)).ToString();
				string file1;

				if (par1Str == null)
				{
					for (int i = 1; File.Exists(file1 = IOPath.Combine(file, new StringBuilder().Append(s).Append(i != 1 ? new StringBuilder().Append("_").Append(i).ToString() : "").Append(".png").ToString())); i++)
					{
					}
				}
				else
				{
					file1 = IOPath.Combine(file, par1Str);
				}

                //bufferedimage.Save(file, ImageFormat.Png);

				return new StringBuilder().Append("Saved screenshot as ").Append(IOPath.GetFileName(file1)).ToString();
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception.ToString());
				Console.Write(exception.StackTrace);
				return (new StringBuilder()).Append("Failed to save: ").Append(exception).ToString();
			}
		}
	}
}