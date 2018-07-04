using System.Net.Sockets;
using System.IO;

namespace net.minecraft.src
{
	public class Packet104WindowItems : Packet
	{
		/// <summary>
		/// The id of window which items are being sent for. 0 for player inventory.
		/// </summary>
		public int WindowId;
		public ItemStack[] ItemStack;

		public Packet104WindowItems()
		{
		}

		/// <summary>
		/// Abstract. Reads the raw packet data from the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void readPacketData(BinaryReader par1NetworkStream) throws IOException
        public override void ReadPacketData(BinaryReader par1NetworkStream)
		{
			WindowId = par1NetworkStream.ReadByte();
			short word0 = par1NetworkStream.ReadInt16();
			ItemStack = new ItemStack[word0];

			for (int i = 0; i < word0; i++)
			{
				ItemStack[i] = ReadItemStack(par1NetworkStream);
			}
		}

		/// <summary>
		/// Abstract. Writes the raw packet data to the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void writePacketData(DataOutputStream par1DataOutputStream) throws IOException
        public override void WritePacketData(BinaryWriter par1DataOutputStream)
		{
			par1DataOutputStream.Write(WindowId);
			par1DataOutputStream.Write(ItemStack.Length);

			for (int i = 0; i < ItemStack.Length; i++)
			{
				WriteItemStack(ItemStack[i], par1DataOutputStream);
			}
		}

		/// <summary>
		/// Passes this Packet on to the NetHandler for processing.
		/// </summary>
		public override void ProcessPacket(NetHandler par1NetHandler)
		{
			par1NetHandler.HandleWindowItems(this);
		}

		/// <summary>
		/// Abstract. Return the size of the packet (not counting the header).
		/// </summary>
		public override int GetPacketSize()
		{
			return 3 + ItemStack.Length * 5;
		}
	}
}