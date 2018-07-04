using System.Net.Sockets;
using System.IO;

namespace net.minecraft.src
{
	public class Packet107CreativeSetSlot : Packet
	{
		public int Slot;
		public ItemStack ItemStack;

		public Packet107CreativeSetSlot()
		{
		}

		public Packet107CreativeSetSlot(int par1, ItemStack par2ItemStack)
		{
			Slot = par1;
			ItemStack = par2ItemStack;
		}

		/// <summary>
		/// Passes this Packet on to the NetHandler for processing.
		/// </summary>
		public override void ProcessPacket(NetHandler par1NetHandler)
		{
			par1NetHandler.HandleCreativeSetSlot(this);
		}

		/// <summary>
		/// Abstract. Reads the raw packet data from the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void readPacketData(BinaryReader par1NetworkStream) throws IOException
        public override void ReadPacketData(BinaryReader par1NetworkStream)
		{
			Slot = par1NetworkStream.ReadInt16();
			ItemStack = ReadItemStack(par1NetworkStream);
		}

		/// <summary>
		/// Abstract. Writes the raw packet data to the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void writePacketData(DataOutputStream par1DataOutputStream) throws IOException
        public override void WritePacketData(BinaryWriter par1DataOutputStream)
		{
			par1DataOutputStream.Write(Slot);
			WriteItemStack(ItemStack, par1DataOutputStream);
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