using System;
using System.Drawing;
using System.IO;

namespace net.minecraft.src
{
	public class TextureWatchFX : TextureFX
	{
		/// <summary>
		/// Holds the game instance to retrieve information like world provider and time.
		/// </summary>
		private Minecraft Mc;
		private int[] WatchIconImageData;
		private int[] DialImageData;
		private double Field_4222_j;
		private double Field_4221_k;

		public TextureWatchFX(Minecraft par1Minecraft)
            : base(Item.PocketSundial.GetIconFromDamage(0))
		{
			WatchIconImageData = new int[256];
			DialImageData = new int[256];
			Mc = par1Minecraft;
			TileImage = 1;

			try
			{
				//Bitmap bufferedimage = ImageIO.read((typeof(Minecraft)).GetResource("/gui/items.png"));
				int i = (IconIndex % 16) * 16;
				int j = (IconIndex / 16) * 16;/*
				bufferedimage.getRGB(i, j, 16, 16, WatchIconImageData, 0, 16);
				bufferedimage = ImageIO.read((typeof(Minecraft)).GetResource("/misc/dial.png"));
				bufferedimage.getRGB(0, 0, 16, 16, DialImageData, 0, 16);*/
			}
			catch (IOException ioexception)
			{
				Console.WriteLine(ioexception.ToString());
				Console.Write(ioexception.StackTrace);
			}
		}

		public override void OnTick()
		{
			double d = 0.0F;

			if (Mc.TheWorld != null && Mc.ThePlayer != null)
			{
				float f = Mc.TheWorld.GetCelestialAngle(1.0F);
				d = -f * (float)Math.PI * 2.0F;

				if (!Mc.TheWorld.WorldProvider.Func_48217_e())
				{
					d = (new Random(1)).NextDouble() * Math.PI * 2D;
				}
			}

			double d1;

			for (d1 = d - Field_4222_j; d1 < -Math.PI; d1 += (Math.PI * 2D))
			{
			}

			for (; d1 >= Math.PI; d1 -= (Math.PI * 2D))
			{
			}

			if (d1 < -1D)
			{
				d1 = -1D;
			}

			if (d1 > 1.0D)
			{
				d1 = 1.0D;
			}

			Field_4221_k += d1 * 0.10000000000000001D;
			Field_4221_k *= 0.80000000000000004D;
			Field_4222_j += Field_4221_k;
			double d2 = Math.Sin(Field_4222_j);
			double d3 = Math.Cos(Field_4222_j);

			for (int i = 0; i < 256; i++)
			{
				int j = WatchIconImageData[i] >> 24 & 0xff;
				int k = WatchIconImageData[i] >> 16 & 0xff;
				int l = WatchIconImageData[i] >> 8 & 0xff;
				int i1 = WatchIconImageData[i] >> 0 & 0xff;

				if (k == i1 && l == 0 && i1 > 0)
				{
					double d4 = -((double)(i % 16) / 15D - 0.5D);
					double d5 = (double)(i / 16) / 15D - 0.5D;
					int i2 = k;
					int j2 = (int)((d4 * d3 + d5 * d2 + 0.5D) * 16D);
					int k2 = (int)(((d5 * d3 - d4 * d2) + 0.5D) * 16D);
					int l2 = (j2 & 0xf) + (k2 & 0xf) * 16;
					j = DialImageData[l2] >> 24 & 0xff;
					k = ((DialImageData[l2] >> 16 & 0xff) * i2) / 255;
					l = ((DialImageData[l2] >> 8 & 0xff) * i2) / 255;
					i1 = ((DialImageData[l2] >> 0 & 0xff) * i2) / 255;
				}

				if (AnaglyphEnabled)
				{
					int j1 = (k * 30 + l * 59 + i1 * 11) / 100;
					int k1 = (k * 30 + l * 70) / 100;
					int l1 = (k * 30 + i1 * 70) / 100;
					k = j1;
					l = k1;
					i1 = l1;
				}

				ImageData[i * 4 + 0] = (byte)k;
				ImageData[i * 4 + 1] = (byte)l;
				ImageData[i * 4 + 2] = (byte)i1;
				ImageData[i * 4 + 3] = (byte)j;
			}
		}
	}
}