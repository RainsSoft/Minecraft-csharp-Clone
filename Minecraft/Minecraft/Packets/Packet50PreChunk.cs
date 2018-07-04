using System.Net.Sockets;
using System.IO;

namespace net.minecraft.src
{
	public class Packet50PreChunk : Packet
	{
		/// <summary>
		/// The X position of the chunk. </summary>
		public int XPosition;

		/// <summary>
		/// The Y position of the chunk. </summary>
		public int YPosition;

		/// <summary>
		/// If mode is true (1) the client will initialise the chunk. If it is false (0) the client will unload the chunk.
		/// </summary>
		public bool Mode;

		public Packet50PreChunk()
		{
			IsChunkDataPacket = false;
		}

		/// <summary>
		/// Abstract. Reads the raw packet data from the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void readPacketData(BinaryReader par1NetworkStream) throws IOException
        public override void ReadPacketData(BinaryReader par1NetworkStream)
		{
			XPosition = par1NetworkStream.ReadInt32();
			YPosition = par1NetworkStream.ReadInt32();
			Mode = par1NetworkStream.Read() != 0;
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
			par1DataOutputStream.Write(Mode ? 1 : 0);
		}

		/// <summary>
		/// Passes this Packet on to the NetHandler for processing.
		/// </summary>
		public override void ProcessPacket(NetHandler par1NetHandler)
		{
			par1NetHandler.HandlePreChunk(this);
		}

		/// <summary>
		/// Abstract. Return the size of the packet (not counting the header).
		/// </summary>
		public override int GetPacketSize()
		{
			return 9;
		}
	}
}