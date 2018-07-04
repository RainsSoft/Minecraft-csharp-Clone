namespace net.minecraft.src
{

	public class ColorizerGrass
	{
		private static int[] GrassBuffer = new int[0x10000];

		public ColorizerGrass()
		{
		}

		public static void SetGrassBiomeColorizer(int[] par0ArrayOfInteger)
		{
			GrassBuffer = par0ArrayOfInteger;
		}

		/// <summary>
		/// Gets grass color from temperature and humidity. Args: temperature, humidity
		/// </summary>
		public static int GetGrassColor(double par0, double par2)
		{
			par2 *= par0;
			int i = (int)((1.0D - par0) * 255D);
			int j = (int)((1.0D - par2) * 255D);
			return GrassBuffer[j << 8 | i];
		}
	}

}