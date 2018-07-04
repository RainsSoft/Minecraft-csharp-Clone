using System.Net.Sockets;
using System.IO;

namespace net.minecraft.src
{
	public class Packet53BlockChange : Packet
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
		/// The new block type for the block. </summary>
		public int Type;

		/// <summary>
		/// Metadata of the block. </summary>
		public int Metadata;

		public Packet53BlockChange()
		{
			IsChunkDataPacket = true;
		}

		/// <summary>
		/// Abstract. Reads the raw packet data from the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void readPacketData(BinaryReader par1NetworkStream) throws IOException
		public override void ReadPacketData(BinaryReader par1NetworkStream)
		{
			XPosition = par1NetworkStream.ReadInt32();
			YPosition = par1NetworkStream.Read();
			ZPosition = par1NetworkStream.ReadInt32();
			Type = par1NetworkStream.Read();
			Metadata = par1NetworkStream.Read();
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
			par1DataOutputStream.Write(Type);
			par1DataOutputStream.Write(Metadata);
		}

		/// <summary>
		/// Passes this Packet on to the NetHandler for processing.
		/// </summary>
		public override void ProcessPacket(NetHandler par1NetHandler)
		{
			par1NetHandler.HandleBlockChange(this);
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