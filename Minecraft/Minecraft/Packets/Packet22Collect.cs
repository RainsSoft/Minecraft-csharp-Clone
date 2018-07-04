using System.IO;
using System.Net.Sockets;

namespace net.minecraft.src
{
	public class Packet22Collect : Packet
	{
		/// <summary>
		/// The entity on the ground that was picked up. </summary>
		public int CollectedEntityId;

		/// <summary>
		/// The entity that picked up the one from the ground. </summary>
		public int CollectorEntityId;

		public Packet22Collect()
		{
		}

		/// <summary>
		/// Abstract. Reads the raw packet data from the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void readPacketData(BinaryReader par1NetworkStream) throws IOException
		public override void ReadPacketData(BinaryReader par1NetworkStream)
		{
			CollectedEntityId = par1NetworkStream.ReadInt32();
			CollectorEntityId = par1NetworkStream.ReadInt32();
		}

		/// <summary>
		/// Abstract. Writes the raw packet data to the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void writePacketData(DataOutputStream par1DataOutputStream) throws IOException
        public override void WritePacketData(BinaryWriter par1DataOutputStream)
		{
			par1DataOutputStream.Write(CollectedEntityId);
			par1DataOutputStream.Write(CollectorEntityId);
		}

		/// <summary>
		/// Passes this Packet on to the NetHandler for processing.
		/// </summary>
		public override void ProcessPacket(NetHandler par1NetHandler)
		{
			par1NetHandler.HandleCollect(this);
		}

		/// <summary>
		/// Abstract. Return the size of the packet (not counting the header).
		/// </summary>
		public override int GetPacketSize()
		{
			return 8;
		}
	}
}