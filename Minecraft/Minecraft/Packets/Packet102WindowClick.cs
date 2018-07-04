using System.Net.Sockets;
using System.IO;

namespace net.minecraft.src
{
	public class Packet102WindowClick : Packet
	{
		/// <summary>
		/// The id of the window which was clicked. 0 for player inventory. </summary>
		public int Window_Id;

		/// <summary>
		/// The clicked slot (-999 is outside of inventory) </summary>
		public int InventorySlot;

		/// <summary>
		/// 1 when right-clicking and otherwise 0 - I'm not sure... </summary>
		public int MouseClick;

		/// <summary>
		/// A unique number for the action, used for transaction handling </summary>
		public short Action;

		/// <summary>
		/// Item stack for inventory </summary>
		public ItemStack ItemStack;
		public bool HoldingShift;

		public Packet102WindowClick()
		{
		}

		public Packet102WindowClick(int par1, int par2, int par3, bool par4, ItemStack par5ItemStack, short par6)
		{
			Window_Id = par1;
			InventorySlot = par2;
			MouseClick = par3;
			ItemStack = par5ItemStack;
			Action = par6;
			HoldingShift = par4;
		}

		/// <summary>
		/// Passes this Packet on to the NetHandler for processing.
		/// </summary>
		public override void ProcessPacket(NetHandler par1NetHandler)
		{
			par1NetHandler.HandleWindowClick(this);
		}

		/// <summary>
		/// Abstract. Reads the raw packet data from the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void readPacketData(BinaryReader par1NetworkStream) throws IOException
        public override void ReadPacketData(BinaryReader par1NetworkStream)
		{
			Window_Id = par1NetworkStream.ReadByte();
			InventorySlot = par1NetworkStream.ReadInt16();
			MouseClick = par1NetworkStream.ReadByte();
			Action = par1NetworkStream.ReadInt16();
			HoldingShift = par1NetworkStream.ReadBoolean();
			ItemStack = ReadItemStack(par1NetworkStream);
		}

		/// <summary>
		/// Abstract. Writes the raw packet data to the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void writePacketData(DataOutputStream par1DataOutputStream) throws IOException
        public override void WritePacketData(BinaryWriter par1DataOutputStream)
		{
			par1DataOutputStream.Write(Window_Id);
			par1DataOutputStream.Write(InventorySlot);
			par1DataOutputStream.Write(MouseClick);
			par1DataOutputStream.Write(Action);
			par1DataOutputStream.Write(HoldingShift);
			WriteItemStack(ItemStack, par1DataOutputStream);
		}

		/// <summary>
		/// Abstract. Return the size of the packet (not counting the header).
		/// </summary>
		public override int GetPacketSize()
		{
			return 11;
		}
	}
}