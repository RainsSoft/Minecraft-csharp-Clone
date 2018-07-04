using System.Net.Sockets;
using System.IO;

namespace net.minecraft.src
{
	public class Packet14BlockDig : Packet
	{
		/// <summary>
		/// Block X position. </summary>
		public int XPosition;

		/// <summary>
		/// Block Y position. </summary>
		public int YPosition;

		/// <summary>
		/// Block Z position. </summary>
		public int ZPosition;

		/// <summary>
		/// Punched face of the block. </summary>
		public int Face;

		/// <summary>
		/// Status of the digging (started, ongoing, broken). </summary>
		public int Status;

		public Packet14BlockDig()
		{
		}

		public Packet14BlockDig(int par1, int par2, int par3, int par4, int par5)
		{
			Status = par1;
			XPosition = par2;
			YPosition = par3;
			ZPosition = par4;
			Face = par5;
		}

		/// <summary>
		/// Abstract. Reads the raw packet data from the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void readPacketData(BinaryReader par1NetworkStream) throws IOException
		public override void ReadPacketData(BinaryReader par1NetworkStream)
		{
			Status = par1NetworkStream.Read();
			XPosition = par1NetworkStream.ReadInt32();
			YPosition = par1NetworkStream.Read();
			ZPosition = par1NetworkStream.ReadInt32();
			Face = par1NetworkStream.Read();
		}

		/// <summary>
		/// Abstract. Writes the raw packet data to the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void writePacketData(DataOutputStream par1DataOutputStream) throws IOException
        public override void WritePacketData(BinaryWriter par1DataOutputStream)
		{
			par1DataOutputStream.Write(Status);
			par1DataOutputStream.Write(XPosition);
			par1DataOutputStream.Write(YPosition);
			par1DataOutputStream.Write(ZPosition);
			par1DataOutputStream.Write(Face);
		}

		/// <summary>
		/// Passes this Packet on to the NetHandler for processing.
		/// </summary>
		public override void ProcessPacket(NetHandler par1NetHandler)
		{
			par1NetHandler.HandleBlockDig(this);
		}

		/// <summary>
		/// Abstract. Return the size of the packet (not counting the header).
		/// </summary>
		public override int GetPacketSize()
		{
			return 11;
		}
	}
}