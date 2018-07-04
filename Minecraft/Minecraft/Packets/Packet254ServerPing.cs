using System.IO;
using System.Net.Sockets;

namespace net.minecraft.src
{
	public class Packet254ServerPing : Packet
	{
		public Packet254ServerPing()
		{
		}

		/// <summary>
		/// Abstract. Reads the raw packet data from the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void readPacketData(NetworkStream datainputstream) throws IOException
		public override void ReadPacketData(BinaryReader datainputstream)
		{
		}

		/// <summary>
		/// Abstract. Writes the raw packet data to the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void writePacketData(DataOutputStream dataoutputstream) throws IOException
		public override void WritePacketData(BinaryWriter dataoutputstream)
		{
		}

		/// <summary>
		/// Passes this Packet on to the NetHandler for processing.
		/// </summary>
		public override void ProcessPacket(NetHandler par1NetHandler)
		{
			par1NetHandler.HandleServerPing(this);
		}

		/// <summary>
		/// Abstract. Return the size of the packet (not counting the header).
		/// </summary>
		public override int GetPacketSize()
		{
			return 0;
		}
	}
}