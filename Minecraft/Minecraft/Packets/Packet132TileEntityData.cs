using System.Net.Sockets;
using System.IO;

namespace net.minecraft.src
{
	public class Packet132TileEntityData : Packet
	{
		/// <summary>
		/// The X position of the tile entity to update. </summary>
		public int XPosition;

		/// <summary>
		/// The Y position of the tile entity to update. </summary>
		public int YPosition;

		/// <summary>
		/// The Z position of the tile entity to update. </summary>
		public int ZPosition;

		/// <summary>
		/// The type of update to perform on the tile entity. </summary>
		public int ActionType;

		/// <summary>
		/// Custom parameter 1 passed to the tile entity on update. </summary>
		public int CustomParam1;

		/// <summary>
		/// Custom parameter 2 passed to the tile entity on update. </summary>
		public int CustomParam2;

		/// <summary>
		/// Custom parameter 3 passed to the tile entity on update. </summary>
		public int CustomParam3;

		public Packet132TileEntityData()
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
			XPosition = par1NetworkStream.ReadInt32();
			YPosition = par1NetworkStream.ReadInt16();
			ZPosition = par1NetworkStream.ReadInt32();
			ActionType = par1NetworkStream.ReadByte();
			CustomParam1 = par1NetworkStream.ReadInt32();
			CustomParam2 = par1NetworkStream.ReadInt32();
			CustomParam3 = par1NetworkStream.ReadInt32();
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
			par1DataOutputStream.Write(ActionType);
			par1DataOutputStream.Write(CustomParam1);
			par1DataOutputStream.Write(CustomParam2);
			par1DataOutputStream.Write(CustomParam3);
		}

		/// <summary>
		/// Passes this Packet on to the NetHandler for processing.
		/// </summary>
		public override void ProcessPacket(NetHandler par1NetHandler)
		{
			par1NetHandler.HandleTileEntityData(this);
		}

		/// <summary>
		/// Abstract. Return the size of the packet (not counting the header).
		/// </summary>
		public override int GetPacketSize()
		{
			return 25;
		}
	}
}