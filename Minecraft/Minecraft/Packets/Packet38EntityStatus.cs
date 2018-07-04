using System.IO;
using System.Net.Sockets;

namespace net.minecraft.src
{
	public class Packet38EntityStatus : Packet
	{
		public int EntityId;

		/// <summary>
		/// 2 for hurt, 3 for dead </summary>
		public byte EntityStatus;

		public Packet38EntityStatus()
		{
		}

		/// <summary>
		/// Abstract. Reads the raw packet data from the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void readPacketData(BinaryReader par1NetworkStream) throws IOException
		public override void ReadPacketData(BinaryReader par1NetworkStream)
		{
			EntityId = par1NetworkStream.ReadInt32();
			EntityStatus = par1NetworkStream.ReadByte();
		}

		/// <summary>
		/// Abstract. Writes the raw packet data to the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void writePacketData(DataOutputStream par1DataOutputStream) throws IOException
		public override void WritePacketData(BinaryWriter par1DataOutputStream)
		{
			par1DataOutputStream.Write(EntityId);
			par1DataOutputStream.Write(EntityStatus);
		}

		/// <summary>
		/// Passes this Packet on to the NetHandler for processing.
		/// </summary>
		public override void ProcessPacket(NetHandler par1NetHandler)
		{
			par1NetHandler.HandleEntityStatus(this);
		}

		/// <summary>
		/// Abstract. Return the size of the packet (not counting the header).
		/// </summary>
		public override int GetPacketSize()
		{
			return 5;
		}
	}
}