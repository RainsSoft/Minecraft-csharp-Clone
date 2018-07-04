using System.Net.Sockets;
using System.IO;

namespace net.minecraft.src
{
	public class Packet103SetSlot : Packet
	{
		/// <summary>
		/// The window which is being updated. 0 for player inventory </summary>
		public int WindowId;

		/// <summary>
		/// Slot that should be updated </summary>
		public int ItemSlot;

		/// <summary>
		/// Item stack </summary>
		public ItemStack MyItemStack;

		public Packet103SetSlot()
		{
		}

		/// <summary>
		/// Passes this Packet on to the NetHandler for processing.
		/// </summary>
		public override void ProcessPacket(NetHandler par1NetHandler)
		{
			par1NetHandler.HandleSetSlot(this);
		}

		/// <summary>
		/// Abstract. Reads the raw packet data from the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void readPacketData(BinaryReader par1NetworkStream) throws IOException
        public override void ReadPacketData(BinaryReader par1NetworkStream)
		{
			WindowId = par1NetworkStream.ReadByte();
			ItemSlot = par1NetworkStream.ReadInt16();
			MyItemStack = ReadItemStack(par1NetworkStream);
		}

		/// <summary>
		/// Abstract. Writes the raw packet data to the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void writePacketData(DataOutputStream par1DataOutputStream) throws IOException
        public override void WritePacketData(BinaryWriter par1DataOutputStream)
		{
			par1DataOutputStream.Write(WindowId);
			par1DataOutputStream.Write(ItemSlot);
			WriteItemStack(MyItemStack, par1DataOutputStream);
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