using System;
using System.IO;

namespace net.minecraft.src
{
	class MusFileStream : FileStream
	{
		private int Hash;
		private FileStream FileStream;
		byte[] Buffer;
		readonly CodecMus Codec;

		public MusFileStream(CodecMus par1CodecMus, string par2URL, FileStream par3FileStream)
            : base(par2URL, FileMode.Open)
		{
			Codec = par1CodecMus;
			Buffer = new byte[1];
			FileStream = par3FileStream;
			string s = par2URL;
			s = s.Substring(s.LastIndexOf("/") + 1);
			Hash = s.GetHashCode();
		}

		public virtual int Read()
		{
			int i = Read(Buffer, 0, 1);

			if (i < 0)
			{
				return i;
			}
			else
			{
				return Buffer[0];
			}
		}

		public virtual int Read(byte[] par1ArrayOfByte, int par2, int par3)
		{
            try
            {
                par3 = FileStream.Read(par1ArrayOfByte, par2, par3);
            }
            catch (Exception t)
            {
                Console.WriteLine(t.ToString());
                Console.WriteLine();

                return 0;
            }

			for (int i = 0; i < par3; i++)
			{
				byte byte0 = par1ArrayOfByte[par2 + i] ^= (byte)(Hash >> 8);
				Hash = Hash * 0x1dba038f + 0x14ee3 * byte0;
			}

			return par3;
		}
	}
}