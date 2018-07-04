namespace net.minecraft.src
{

	public class ColorizerWater
	{
		private static int[] WaterBuffer = new int[0x10000];

		public ColorizerWater()
		{
		}

		public static void SetWaterBiomeColorizer(int[] par0ArrayOfInteger)
		{
			WaterBuffer = par0ArrayOfInteger;
		}
	}

}