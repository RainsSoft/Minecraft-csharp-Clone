using System.Net.Sockets;
using System.IO;

namespace net.minecraft.src
{
	public class Packet201PlayerInfo : Packet
	{
		/// <summary>
		/// The player's name. </summary>
		public string PlayerName;

		/// <summary>
		/// Byte that tells whether the player is connected. </summary>
		public bool IsConnected;
		public int Ping;

		public Packet201PlayerInfo()
		{
		}

		/// <summary>
		/// Abstract. Reads the raw packet data from the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void readPacketData(BinaryReader par1NetworkStream) throws IOException
        public override void ReadPacketData(BinaryReader par1NetworkStream)
		{
			PlayerName = ReadString(par1NetworkStream, 16);
			IsConnected = par1NetworkStream.ReadByte() != 0;
			Ping = par1NetworkStream.ReadInt16();
		}

		/// <summary>
		/// Abstract. Writes the raw packet data to the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void writePacketData(DataOutputStream par1DataOutputStream) throws IOException
        public override void WritePacketData(BinaryWriter par1DataOutputStream)
		{
			WriteString(PlayerName, par1DataOutputStream);
			par1DataOutputStream.Write(IsConnected ? 1 : 0);
			par1DataOutputStream.Write(Ping);
		}

		/// <summary>
		/// Passes this Packet on to the NetHandler for processing.
		/// </summary>
		public override void ProcessPacket(NetHandler par1NetHandler)
		{
			par1NetHandler.HandlePlayerInfo(this);
		}

		/// <summary>
		/// Abstract. Return the size of the packet (not counting the header).
		/// </summary>
		public override int GetPacketSize()
		{
			return PlayerName.Length + 2 + 1 + 2;
		}
	}
}