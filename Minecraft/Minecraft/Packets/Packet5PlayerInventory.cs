using System.Net.Sockets;
using System.IO;

namespace net.minecraft.src
{
	public class Packet5PlayerInventory : Packet
	{
		/// <summary>
		/// Entity ID of the object. </summary>
		public int EntityID;

		/// <summary>
		/// Equipment slot: 0=held, 1-4=armor slot </summary>
		public int Slot;

		/// <summary>
		/// Equipped item (-1 for empty slot). </summary>
		public int ItemID;

		/// <summary>
		/// The health of the item. </summary>
		public int ItemDamage;

		public Packet5PlayerInventory()
		{
		}

		/// <summary>
		/// Abstract. Reads the raw packet data from the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void readPacketData(BinaryReader par1NetworkStream) throws IOException
        public override void ReadPacketData(BinaryReader par1NetworkStream)
		{
			EntityID = par1NetworkStream.ReadInt32();
			Slot = par1NetworkStream.ReadInt16();
			ItemID = par1NetworkStream.ReadInt16();
			ItemDamage = par1NetworkStream.ReadInt16();
		}

		/// <summary>
		/// Abstract. Writes the raw packet data to the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void writePacketData(DataOutputStream par1DataOutputStream) throws IOException
        public override void WritePacketData(BinaryWriter par1DataOutputStream)
		{
			par1DataOutputStream.Write(EntityID);
			par1DataOutputStream.Write(Slot);
			par1DataOutputStream.Write(ItemID);
			par1DataOutputStream.Write(ItemDamage);
		}

		/// <summary>
		/// Passes this Packet on to the NetHandler for processing.
		/// </summary>
		public override void ProcessPacket(NetHandler par1NetHandler)
		{
			par1NetHandler.HandlePlayerInventory(this);
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