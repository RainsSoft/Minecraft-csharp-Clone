using System.Net.Sockets;
using System.IO;

namespace net.minecraft.src
{
	public class Packet17Sleep : Packet
	{
		public int EntityID;
		public int BedX;
		public int BedY;
		public int BedZ;
		public int Field_22046_e;

		public Packet17Sleep()
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
			Field_22046_e = par1NetworkStream.ReadByte();
			BedX = par1NetworkStream.ReadInt32();
			BedY = par1NetworkStream.ReadByte();
			BedZ = par1NetworkStream.ReadInt32();
		}

		/// <summary>
		/// Abstract. Writes the raw packet data to the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void writePacketData(DataOutputStream par1DataOutputStream) throws IOException
        public override void WritePacketData(BinaryWriter par1DataOutputStream)
		{
			par1DataOutputStream.Write(EntityID);
			par1DataOutputStream.Write(Field_22046_e);
			par1DataOutputStream.Write(BedX);
			par1DataOutputStream.Write(BedY);
			par1DataOutputStream.Write(BedZ);
		}

		/// <summary>
		/// Passes this Packet on to the NetHandler for processing.
		/// </summary>
		public override void ProcessPacket(NetHandler par1NetHandler)
		{
			par1NetHandler.HandleSleep(this);
		}

		/// <summary>
		/// Abstract. Return the size of the packet (not counting the header).
		/// </summary>
		public override int GetPacketSize()
		{
			return 14;
		}
	}
}