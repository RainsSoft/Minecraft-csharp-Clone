using System.Net.Sockets;
using System.IO;

namespace net.minecraft.src
{
	public class Packet130UpdateSign : Packet
	{
		public int XPosition;
		public int YPosition;
		public int ZPosition;
		public string[] SignLines;

		public Packet130UpdateSign()
		{
			IsChunkDataPacket = true;
		}

		public Packet130UpdateSign(int par1, int par2, int par3, string[] par4ArrayOfStr)
		{
			IsChunkDataPacket = true;
			XPosition = par1;
			YPosition = par2;
			ZPosition = par3;
			SignLines = par4ArrayOfStr;
		}

		/// <summary>
		/// Abstract. Reads the raw packet data from the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void readPacketData(BinaryReader par1NetworkStream) throws IOException
        public override void ReadPacketData(BinaryReader par1NetworkStream)
		{
			XPosition = par1NetworkStream.ReadInt32();
			YPosition = par1NetworkStream.ReadInt16();
			ZPosition = par1NetworkStream.ReadInt32();
			SignLines = new string[4];

			for (int i = 0; i < 4; i++)
			{
				SignLines[i] = ReadString(par1NetworkStream, 15);
			}
		}

		/// <summary>
		/// Abstract. Writes the raw packet data to the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void writePacketData(DataOutputStream par1DataOutputStream) throws IOException
        public override void WritePacketData(BinaryWriter par1DataOutputStream)
		{
			par1DataOutputStream.Write(XPosition);
			par1DataOutputStream.Write(YPosition);
			par1DataOutputStream.Write(ZPosition);

			for (int i = 0; i < 4; i++)
			{
				WriteString(SignLines[i], par1DataOutputStream);
			}
		}

		/// <summary>
		/// Passes this Packet on to the NetHandler for processing.
		/// </summary>
		public override void ProcessPacket(NetHandler par1NetHandler)
		{
			par1NetHandler.HandleUpdateSign(this);
		}

		/// <summary>
		/// Abstract. Return the size of the packet (not counting the header).
		/// </summary>
		public override int GetPacketSize()
		{
			int i = 0;

			for (int j = 0; j < 4; j++)
			{
				i += SignLines[j].Length;
			}

			return i;
		}
	}
}