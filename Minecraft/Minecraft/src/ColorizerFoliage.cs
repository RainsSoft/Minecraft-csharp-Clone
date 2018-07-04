namespace net.minecraft.src
{

	public class ColorizerFoliage
	{
		private static int[] FoliageBuffer = new int[0x10000];

		public ColorizerFoliage()
		{
		}

		public static void GetFoilageBiomeColorizer(int[] par0ArrayOfInteger)
		{
			FoliageBuffer = par0ArrayOfInteger;
		}

		/// <summary>
		/// Gets foliage color from temperature and humidity. Args: temperature, humidity
		/// </summary>
		public static int GetFoliageColor(double par0, double par2)
		{
			par2 *= par0;
			int i = (int)((1.0D - par0) * 255D);
			int j = (int)((1.0D - par2) * 255D);
			return FoliageBuffer[j << 8 | i];
		}

		/// <summary>
		/// Gets the foliage color for pine type (metadata 1) trees
		/// </summary>
		public static int GetFoliageColorPine()
		{
			return 0x619961;
		}

		/// <summary>
		/// Gets the foliage color for birch type (metadata 2) trees
		/// </summary>
		public static int GetFoliageColorBirch()
		{
			return 0x80a755;
		}

		public static int GetFoliageColorBasic()
		{
			return 0x48b518;
		}
	}

}