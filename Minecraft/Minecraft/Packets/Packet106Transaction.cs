using System.Net.Sockets;
using System.IO;

namespace net.minecraft.src
{
	public class Packet106Transaction : Packet
	{
		/// <summary>
		/// The id of the window that the action occurred in. </summary>
		public int WindowId;
		public short ShortWindowId;
		public bool Accepted;

		public Packet106Transaction()
		{
		}

		public Packet106Transaction(int par1, short par2, bool par3)
		{
			WindowId = par1;
			ShortWindowId = par2;
			Accepted = par3;
		}

		/// <summary>
		/// Passes this Packet on to the NetHandler for processing.
		/// </summary>
		public override void ProcessPacket(NetHandler par1NetHandler)
		{
			par1NetHandler.HandleTransaction(this);
		}

		/// <summary>
		/// Abstract. Reads the raw packet data from the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void readPacketData(BinaryReader par1NetworkStream) throws IOException
		public override void ReadPacketData(BinaryReader par1NetworkStream)
		{
			WindowId = par1NetworkStream.ReadByte();
			ShortWindowId = par1NetworkStream.ReadInt16();
			Accepted = par1NetworkStream.ReadByte() != 0;
		}

		/// <summary>
		/// Abstract. Writes the raw packet data to the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void writePacketData(DataOutputStream par1DataOutputStream) throws IOException
        public override void WritePacketData(BinaryWriter par1DataOutputStream)
		{
			par1DataOutputStream.Write(WindowId);
			par1DataOutputStream.Write(ShortWindowId);
			par1DataOutputStream.Write(Accepted ? 1 : 0);
		}

		/// <summary>
		/// Abstract. Return the size of the packet (not counting the header).
		/// </summary>
		public override int GetPacketSize()
		{
			return 4;
		}
	}
}