using System;

namespace net.minecraft.src
{
	public class ScaledResolution
	{
		private int ScaledWidth;
		private int ScaledHeight;
		public float ScaledWidthD;
		public float ScaledHeightD;
		public int ScaleFactor;

		public ScaledResolution(GameSettings par1GameSettings, int par2, int par3)
		{
			ScaledWidth = par2;
			ScaledHeight = par3;
			ScaleFactor = 1;
			int i = par1GameSettings.GuiScale;

			if (i == 0)
			{
				i = 1000;
			}

			for (; ScaleFactor < i && ScaledWidth / (ScaleFactor + 1) >= 320 && ScaledHeight / (ScaleFactor + 1) >= 240; ScaleFactor++)
			{
			}

			ScaledWidthD = (float)ScaledWidth / ScaleFactor;
			ScaledHeightD = (float)ScaledHeight / ScaleFactor;
			ScaledWidth = (int)Math.Ceiling(ScaledWidthD);
			ScaledHeight = (int)Math.Ceiling(ScaledHeightD);
		}

		public virtual int GetScaledWidth()
		{
			return ScaledWidth;
		}

		public virtual int GetScaledHeight()
		{
			return ScaledHeight;
		}
	}
}