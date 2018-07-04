using System.Net.Sockets;
using System.IO;

namespace net.minecraft.src
{
	public class Packet30Entity : Packet
	{
		/// <summary>
		/// The ID of this entity. </summary>
		public int EntityId;

		/// <summary>
		/// The X axis relative movement. </summary>
		public sbyte XPosition;

		/// <summary>
		/// The Y axis relative movement. </summary>
		public sbyte YPosition;

		/// <summary>
		/// The Z axis relative movement. </summary>
		public sbyte ZPosition;

		/// <summary>
		/// The X axis rotation. </summary>
		public sbyte Yaw;

		/// <summary>
		/// The Y axis rotation. </summary>
		public sbyte Pitch;

		/// <summary>
		/// bool set to true if the entity is rotating. </summary>
		public bool Rotating;

		public Packet30Entity()
		{
			Rotating = false;
		}

		/// <summary>
		/// Abstract. Reads the raw packet data from the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void readPacketData(BinaryReader par1NetworkStream) throws IOException
		public override void ReadPacketData(BinaryReader par1NetworkStream)
		{
			EntityId = par1NetworkStream.ReadInt32();
		}

		/// <summary>
		/// Abstract. Writes the raw packet data to the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void writePacketData(DataOutputStream par1DataOutputStream) throws IOException
        public override void WritePacketData(BinaryWriter par1DataOutputStream)
		{
			par1DataOutputStream.Write(EntityId);
		}

		/// <summary>
		/// Passes this Packet on to the NetHandler for processing.
		/// </summary>
		public override void ProcessPacket(NetHandler par1NetHandler)
		{
			par1NetHandler.HandleEntity(this);
		}

		/// <summary>
		/// Abstract. Return the size of the packet (not counting the header).
		/// </summary>
		public override int GetPacketSize()
		{
			return 4;
		}
	}
}