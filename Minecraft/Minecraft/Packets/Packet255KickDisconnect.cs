using System.IO;
using System.Net.Sockets;

namespace net.minecraft.src
{
	public class Packet255KickDisconnect : Packet
	{
		/// <summary>
		/// Displayed to the client when the connection terminates. </summary>
		public string Reason;

		public Packet255KickDisconnect()
		{
		}

		public Packet255KickDisconnect(string par1Str)
		{
			Reason = par1Str;
		}

		/// <summary>
		/// Abstract. Reads the raw packet data from the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void readPacketData(BinaryReader par1NetworkStream) throws IOException
        public override void ReadPacketData(BinaryReader par1NetworkStream)
		{
			Reason = ReadString(par1NetworkStream, 256);
		}

		/// <summary>
		/// Abstract. Writes the raw packet data to the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void writePacketData(DataOutputStream par1DataOutputStream) throws IOException
        public override void WritePacketData(BinaryWriter par1DataOutputStream)
		{
			WriteString(Reason, par1DataOutputStream);
		}

		/// <summary>
		/// Passes this Packet on to the NetHandler for processing.
		/// </summary>
		public override void ProcessPacket(NetHandler par1NetHandler)
		{
			par1NetHandler.HandleKickDisconnect(this);
		}

		/// <summary>
		/// Abstract. Return the size of the packet (not counting the header).
		/// </summary>
		public override int GetPacketSize()
		{
			return Reason.Length;
		}
	}
}