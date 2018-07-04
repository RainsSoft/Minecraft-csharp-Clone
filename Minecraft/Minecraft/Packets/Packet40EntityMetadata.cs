using System.Collections.Generic;
using System.Net.Sockets;
using System.IO;

namespace net.minecraft.src
{
	public class Packet40EntityMetadata : Packet
	{
		public int EntityId;
        private List<WatchableObject> Metadata;

		public Packet40EntityMetadata()
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
			Metadata = DataWatcher.ReadWatchableObjects(par1NetworkStream);
		}

		/// <summary>
		/// Abstract. Writes the raw packet data to the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void writePacketData(DataOutputStream par1DataOutputStream) throws IOException
        public override void WritePacketData(BinaryWriter par1DataOutputStream)
		{
			par1DataOutputStream.Write(EntityId);
			DataWatcher.WriteObjectsInListToStream(Metadata, par1DataOutputStream);
		}

		/// <summary>
		/// Passes this Packet on to the NetHandler for processing.
		/// </summary>
		public override void ProcessPacket(NetHandler par1NetHandler)
		{
			par1NetHandler.HandleEntityMetadata(this);
		}

		/// <summary>
		/// Abstract. Return the size of the packet (not counting the header).
		/// </summary>
		public override int GetPacketSize()
		{
			return 5;
		}

        public virtual List<WatchableObject> GetMetadata()
		{
			return Metadata;
		}
	}
}