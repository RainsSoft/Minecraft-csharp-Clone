using System.Net.Sockets;
using System.IO;

namespace net.minecraft.src
{
	public class Packet108EnchantItem : Packet
	{
		public int WindowId;

		/// <summary>
		/// The position of the enchantment on the enchantment table window, starting with 0 as the topmost one.
		/// </summary>
		public int Enchantment;

		public Packet108EnchantItem()
		{
		}

		public Packet108EnchantItem(int par1, int par2)
		{
			WindowId = par1;
			Enchantment = par2;
		}

		/// <summary>
		/// Passes this Packet on to the NetHandler for processing.
		/// </summary>
		public override void ProcessPacket(NetHandler par1NetHandler)
		{
			par1NetHandler.HandleEnchantItem(this);
		}

		/// <summary>
		/// Abstract. Reads the raw packet data from the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void readPacketData(BinaryReader par1NetworkStream) throws IOException
		public override void ReadPacketData(BinaryReader par1NetworkStream)
		{
			WindowId = par1NetworkStream.ReadByte();
			Enchantment = par1NetworkStream.ReadByte();
		}

		/// <summary>
		/// Abstract. Writes the raw packet data to the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void writePacketData(DataOutputStream par1DataOutputStream) throws IOException
        public override void WritePacketData(BinaryWriter par1DataOutputStream)
		{
			par1DataOutputStream.Write(WindowId);
			par1DataOutputStream.Write(Enchantment);
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