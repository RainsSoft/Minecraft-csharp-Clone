using System.Net.Sockets;
using System.Text;
using System.IO;

namespace net.minecraft.src
{
	public class Packet2Handshake : Packet
	{
		/// <summary>
		/// The username of the player attempting to connect. </summary>
		public string Username;

		public Packet2Handshake()
		{
		}

		public Packet2Handshake(string par1Str)
		{
			Username = par1Str;
		}

		public Packet2Handshake(string par1Str, string par2Str, int par3)
		{
			Username = (new StringBuilder()).Append(par1Str).Append(";").Append(par2Str).Append(":").Append(par3).ToString();
		}

		/// <summary>
		/// Abstract. Reads the raw packet data from the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void readPacketData(BinaryReader par1NetworkStream) throws IOException
        public override void ReadPacketData(BinaryReader par1NetworkStream)
		{
			Username = ReadString(par1NetworkStream, 64);
		}

		/// <summary>
		/// Abstract. Writes the raw packet data to the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void writePacketData(DataOutputStream par1DataOutputStream) throws IOException
        public override void WritePacketData(BinaryWriter par1DataOutputStream)
		{
			WriteString(Username, par1DataOutputStream);
		}

		/// <summary>
		/// Passes this Packet on to the NetHandler for processing.
		/// </summary>
		public override void ProcessPacket(NetHandler par1NetHandler)
		{
			par1NetHandler.HandleHandshake(this);
		}

		/// <summary>
		/// Abstract. Return the size of the packet (not counting the header).
		/// </summary>
		public override int GetPacketSize()
		{
			return 4 + Username.Length + 4;
		}
	}
}