namespace net.minecraft.src
{
	public class NibbleArrayReader
	{
		public readonly byte[] Data;
		private readonly int DepthBits;
		private readonly int DepthBitsPlusFour;

		public NibbleArrayReader(byte[] par1ArrayOfByte, int par2)
		{
			Data = par1ArrayOfByte;
			DepthBits = par2;
			DepthBitsPlusFour = par2 + 4;
		}

		public virtual int Get(int par1, int par2, int par3)
		{
			int i = par1 << DepthBitsPlusFour | par3 << DepthBits | par2;
			int j = i >> 1;
			int k = i & 1;

			if (k == 0)
			{
				return Data[j] & 0xf;
			}
			else
			{
				return Data[j] >> 4 & 0xf;
			}
		}
	}
}