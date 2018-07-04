namespace net.minecraft.src
{
	public class SkyBlock
	{
		public static SkyBlock Sky = new SkyBlock(15);

        public static SkyBlock Block = new SkyBlock(0);

		public int DefaultLightValue;

		private SkyBlock(int par3)
		{
			DefaultLightValue = par3;
		}
	}
}