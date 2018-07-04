namespace net.minecraft.src
{
	public class NibbleArray
	{
		public readonly byte[] Data;

		/// <summary>
		/// Log base 2 of the chunk height (128); applied as a shift on Z coordinate
		/// </summary>
		private readonly int DepthBits;

		/// <summary>
		/// Log base 2 of the chunk height (128) * width (16); applied as a shift on X coordinate
		/// </summary>
		private readonly int DepthBitsPlusFour;

		public NibbleArray(int par1, int par2)
		{
			Data = new byte[par1 >> 1];
			DepthBits = par2;
			DepthBitsPlusFour = par2 + 4;
		}

		public NibbleArray(byte[] par1ArrayOfByte, int par2)
		{
			Data = par1ArrayOfByte;
			DepthBits = par2;
			DepthBitsPlusFour = par2 + 4;
		}

		/// <summary>
		/// Returns the nibble of data corresponding to the passed in x, y, z. y is at most 6 bits, z is at most 4.
		/// </summary>
		public virtual int Get(int par1, int par2, int par3)
		{
			int i = par2 << DepthBitsPlusFour | par3 << DepthBits | par1;
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

		/// <summary>
		/// Arguments are x, y, z, val. Sets the nibble of data at x << 11 | z << 7 | y to val.
		/// </summary>
		public virtual void Set(int par1, int par2, int par3, int par4)
		{
			int i = par2 << DepthBitsPlusFour | par3 << DepthBits | par1;
			int j = i >> 1;
			int k = i & 1;

			if (k == 0)
			{
				Data[j] = (byte)(Data[j] & 0xf0 | par4 & 0xf);
			}
			else
			{
				Data[j] = (byte)(Data[j] & 0xf | (par4 & 0xf) << 4);
			}
		}
	}
}