using System.Net.Sockets;
using System.IO;

namespace net.minecraft.src
{
	public class Packet15Place : Packet
	{
		public int XPosition;
		public int YPosition;
		public int ZPosition;

		/// <summary>
		/// The offset to use for block/item placement. </summary>
		public int Direction;
		public ItemStack ItemStack;

		public Packet15Place()
		{
		}

		public Packet15Place(int par1, int par2, int par3, int par4, ItemStack par5ItemStack)
		{
			XPosition = par1;
			YPosition = par2;
			ZPosition = par3;
			Direction = par4;
			ItemStack = par5ItemStack;
		}

		/// <summary>
		/// Abstract. Reads the raw packet data from the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void readPacketData(BinaryReader par1NetworkStream) throws IOException
        public override void ReadPacketData(BinaryReader par1NetworkStream)
		{
			XPosition = par1NetworkStream.ReadInt32();
			YPosition = par1NetworkStream.Read();
			ZPosition = par1NetworkStream.ReadInt32();
			Direction = par1NetworkStream.Read();
			ItemStack = ReadItemStack(par1NetworkStream);
		}

		/// <summary>
		/// Abstract. Writes the raw packet data to the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void writePacketData(DataOutputStream par1DataOutputStream) throws IOException
        public override void WritePacketData(BinaryWriter par1DataOutputStream)
		{
			par1DataOutputStream.Write(XPosition);
			par1DataOutputStream.Write(YPosition);
			par1DataOutputStream.Write(ZPosition);
			par1DataOutputStream.Write(Direction);
			WriteItemStack(ItemStack, par1DataOutputStream);
		}

		/// <summary>
		/// Passes this Packet on to the NetHandler for processing.
		/// </summary>
		public override void ProcessPacket(NetHandler par1NetHandler)
		{
			par1NetHandler.HandlePlace(this);
		}

		/// <summary>
		/// Abstract. Return the size of the packet (not counting the header).
		/// </summary>
		public override int GetPacketSize()
		{
			return 15;
		}
	}
}