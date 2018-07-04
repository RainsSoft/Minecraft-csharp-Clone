using System.Net.Sockets;
using System.IO;

namespace net.minecraft.src
{
	public class Packet100OpenWindow : Packet
	{
		public int WindowId;
		public int InventoryType;
		public string WindowTitle;
		public int SlotsCount;

		public Packet100OpenWindow()
		{
		}

		/// <summary>
		/// Passes this Packet on to the NetHandler for processing.
		/// </summary>
		public override void ProcessPacket(NetHandler par1NetHandler)
		{
			par1NetHandler.HandleOpenWindow(this);
		}

		/// <summary>
		/// Abstract. Reads the raw packet data from the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void readPacketData(BinaryReader par1NetworkStream) throws IOException
        public override void ReadPacketData(BinaryReader par1NetworkStream)
		{
			WindowId = par1NetworkStream.ReadByte() & 0xff;
			InventoryType = par1NetworkStream.ReadByte() & 0xff;
			WindowTitle = ReadString(par1NetworkStream, 32);
			SlotsCount = par1NetworkStream.ReadByte() & 0xff;
		}

		/// <summary>
		/// Abstract. Writes the raw packet data to the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void writePacketData(DataOutputStream par1DataOutputStream) throws IOException
        public override void WritePacketData(BinaryWriter par1DataOutputStream)
		{
			par1DataOutputStream.Write(WindowId & 0xff);
			par1DataOutputStream.Write(InventoryType & 0xff);
			WriteString(WindowTitle, par1DataOutputStream);
			par1DataOutputStream.Write(SlotsCount & 0xff);
		}

		/// <summary>
		/// Abstract. Return the size of the packet (not counting the header).
		/// </summary>
		public override int GetPacketSize()
		{
			return 3 + WindowTitle.Length;
		}
	}
}