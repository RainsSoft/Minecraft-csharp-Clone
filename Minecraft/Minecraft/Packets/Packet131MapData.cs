using System.Net.Sockets;
using System.IO;

namespace net.minecraft.src
{
	public class Packet131MapData : Packet
	{
		public short ItemID;

		/// <summary>
		/// Contains a unique ID for the item that this packet will be populating.
		/// </summary>
		public short UniqueID;
		public byte[] ItemData;

		public Packet131MapData()
		{
			IsChunkDataPacket = true;
		}

		/// <summary>
		/// Abstract. Reads the raw packet data from the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void readPacketData(BinaryReader par1NetworkStream) throws IOException
		public override void ReadPacketData(BinaryReader par1NetworkStream)
		{
			ItemID = par1NetworkStream.ReadInt16();
			UniqueID = par1NetworkStream.ReadInt16();
			ItemData = new byte[par1NetworkStream.ReadByte() & 0xff];
			par1NetworkStream.Read(ItemData, 0, ItemData.Length);
		}

		/// <summary>
		/// Abstract. Writes the raw packet data to the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void writePacketData(DataOutputStream par1DataOutputStream) throws IOException
        public override void WritePacketData(BinaryWriter par1DataOutputStream)
		{
			par1DataOutputStream.Write(ItemID);
			par1DataOutputStream.Write(UniqueID);
			par1DataOutputStream.Write(ItemData.Length);
			par1DataOutputStream.Write(ItemData);
		}

		/// <summary>
		/// Passes this Packet on to the NetHandler for processing.
		/// </summary>
		public override void ProcessPacket(NetHandler par1NetHandler)
		{
			par1NetHandler.HandleMapData(this);
		}

		/// <summary>
		/// Abstract. Return the size of the packet (not counting the header).
		/// </summary>
		public override int GetPacketSize()
		{
			return 4 + ItemData.Length;
		}
	}
}