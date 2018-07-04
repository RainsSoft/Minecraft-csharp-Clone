using System.Net.Sockets;
using System.IO;

namespace net.minecraft.src
{
	public class Packet61DoorChange : Packet
	{
		public int SfxID;
		public int AuxData;
		public int PosX;
		public int PosY;
		public int PosZ;

		public Packet61DoorChange()
		{
		}

		/// <summary>
		/// Abstract. Reads the raw packet data from the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void readPacketData(BinaryReader par1NetworkStream) throws IOException
		public override void ReadPacketData(BinaryReader par1NetworkStream)
		{
			SfxID = par1NetworkStream.ReadInt32();
			PosX = par1NetworkStream.ReadInt32();
			PosY = par1NetworkStream.ReadByte() & 0xff;
			PosZ = par1NetworkStream.ReadInt32();
			AuxData = par1NetworkStream.ReadInt32();
		}

		/// <summary>
		/// Abstract. Writes the raw packet data to the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void writePacketData(DataOutputStream par1DataOutputStream) throws IOException
        public override void WritePacketData(BinaryWriter par1DataOutputStream)
		{
			par1DataOutputStream.Write(SfxID);
			par1DataOutputStream.Write(PosX);
			par1DataOutputStream.Write(PosY & 0xff);
			par1DataOutputStream.Write(PosZ);
			par1DataOutputStream.Write(AuxData);
		}

		/// <summary>
		/// Passes this Packet on to the NetHandler for processing.
		/// </summary>
		public override void ProcessPacket(NetHandler par1NetHandler)
		{
			par1NetHandler.HandleDoorChange(this);
		}

		/// <summary>
		/// Abstract. Return the size of the packet (not counting the header).
		/// </summary>
		public override int GetPacketSize()
		{
			return 20;
		}
	}
}