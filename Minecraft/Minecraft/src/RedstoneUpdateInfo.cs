namespace net.minecraft.src
{
    public class RedstoneUpdateInfo
	{
		public int X;
        public int Y;
        public int Z;
        public long UpdateTime;

		public RedstoneUpdateInfo(int par1, int par2, int par3, long par4)
		{
			X = par1;
			Y = par2;
			Z = par3;
			UpdateTime = par4;
		}
	}
}