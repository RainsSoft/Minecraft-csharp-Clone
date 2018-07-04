using System.Net.Sockets;
using System.IO;

namespace net.minecraft.src
{
	public class Packet70Bed : Packet
	{
		public static readonly string[] BedChat = { "tile.bed.notValid", null, null, "gameMode.changed" };

		/// <summary>
		/// Either 1 or 2. 1 indicates to begin raining, 2 indicates to stop raining.
		/// </summary>
		public int BedState;

		/// <summary>
		/// Used only when reason = 3. 0 is survival, 1 is creative. </summary>
		public int GameMode;

		public Packet70Bed()
		{
		}

		/// <summary>
		/// Abstract. Reads the raw packet data from the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void readPacketData(BinaryReader par1NetworkStream) throws IOException
		public override void ReadPacketData(BinaryReader par1NetworkStream)
		{
			BedState = par1NetworkStream.ReadByte();
			GameMode = par1NetworkStream.ReadByte();
		}

		/// <summary>
		/// Abstract. Writes the raw packet data to the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void writePacketData(DataOutputStream par1DataOutputStream) throws IOException
        public override void WritePacketData(BinaryWriter par1DataOutputStream)
		{
			par1DataOutputStream.Write(BedState);
			par1DataOutputStream.Write(GameMode);
		}

		/// <summary>
		/// Passes this Packet on to the NetHandler for processing.
		/// </summary>
		public override void ProcessPacket(NetHandler par1NetHandler)
		{
			par1NetHandler.HandleBed(this);
		}

		/// <summary>
		/// Abstract. Return the size of the packet (not counting the header).
		/// </summary>
		public override int GetPacketSize()
		{
			return 2;
		}
	}
}