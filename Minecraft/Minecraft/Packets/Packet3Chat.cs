using System.Net.Sockets;
using System.IO;

namespace net.minecraft.src
{
	public class Packet3Chat : Packet
	{
		public static int Field_52010_b = 119;

		/// <summary>
		/// The message being sent. </summary>
		public string Message;

		public Packet3Chat()
		{
		}

		public Packet3Chat(string par1Str)
		{
			if (par1Str.Length > Field_52010_b)
			{
				par1Str = par1Str.Substring(0, Field_52010_b);
			}

			Message = par1Str;
		}

		/// <summary>
		/// Abstract. Reads the raw packet data from the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void readPacketData(BinaryReader par1NetworkStream) throws IOException
        public override void ReadPacketData(BinaryReader par1NetworkStream)
		{
			Message = ReadString(par1NetworkStream, Field_52010_b);
		}

		/// <summary>
		/// Abstract. Writes the raw packet data to the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void writePacketData(DataOutputStream par1DataOutputStream) throws IOException
        public override void WritePacketData(BinaryWriter par1DataOutputStream)
		{
			WriteString(Message, par1DataOutputStream);
		}

		/// <summary>
		/// Passes this Packet on to the NetHandler for processing.
		/// </summary>
		public override void ProcessPacket(NetHandler par1NetHandler)
		{
			par1NetHandler.HandleChat(this);
		}

		/// <summary>
		/// Abstract. Return the size of the packet (not counting the header).
		/// </summary>
		public override int GetPacketSize()
		{
			return 2 + Message.Length * 2;
		}
	}
}