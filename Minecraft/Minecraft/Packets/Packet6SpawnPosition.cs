using System.Net.Sockets;
using System.IO;

namespace net.minecraft.src
{
	public class Packet6SpawnPosition : Packet
	{
		/// <summary>
		/// X coordinate of spawn. </summary>
		public int XPosition;

		/// <summary>
		/// Y coordinate of spawn. </summary>
		public int YPosition;

		/// <summary>
		/// Z coordinate of spawn. </summary>
		public int ZPosition;

		public Packet6SpawnPosition()
		{
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
			ZPosition = par1NetworkStream.ReadInt32();
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
		}

		/// <summary>
		/// Passes this Packet on to the NetHandler for processing.
		/// </summary>
		public override void ProcessPacket(NetHandler par1NetHandler)
		{
			par1NetHandler.HandleSpawnPosition(this);
		}

		/// <summary>
		/// Abstract. Return the size of the packet (not counting the header).
		/// </summary>
		public override int GetPacketSize()
		{
			return 12;
		}
	}
}