using System;
using System.Net.Sockets;
using System.IO;

namespace net.minecraft.src
{
	public class Packet23VehicleSpawn : Packet
	{
		/// <summary>
		/// Entity ID of the object. </summary>
		public int EntityId;

		/// <summary>
		/// The X position of the object. </summary>
		public int XPosition;

		/// <summary>
		/// The Y position of the object. </summary>
		public int YPosition;

		/// <summary>
		/// The Z position of the object. </summary>
		public int ZPosition;

		/// <summary>
		/// Not sent if the thrower entity ID is 0. The speed of this fireball along the X axis.
		/// </summary>
		public int SpeedX;

		/// <summary>
		/// Not sent if the thrower entity ID is 0. The speed of this fireball along the Y axis.
		/// </summary>
		public int SpeedY;

		/// <summary>
		/// Not sent if the thrower entity ID is 0. The speed of this fireball along the Z axis.
		/// </summary>
		public int SpeedZ;

		/// <summary>
		/// The type of object. </summary>
		public int Type;

		/// <summary>
		/// 0 if not a fireball. Otherwise, this is the Entity ID of the thrower. </summary>
		public int ThrowerEntityId;

		public Packet23VehicleSpawn()
		{
		}

		/// <summary>
		/// Abstract. Reads the raw packet data from the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void readPacketData(BinaryReader par1NetworkStream) throws IOException
        public override void ReadPacketData(BinaryReader par1NetworkStream)
		{
			EntityId = par1NetworkStream.ReadInt32();
			Type = par1NetworkStream.ReadByte();
			XPosition = par1NetworkStream.ReadInt32();
			YPosition = par1NetworkStream.ReadInt32();
			ZPosition = par1NetworkStream.ReadInt32();
			ThrowerEntityId = par1NetworkStream.ReadInt32();

			if (ThrowerEntityId > 0)
			{
				SpeedX = par1NetworkStream.ReadInt16();
				SpeedY = par1NetworkStream.ReadInt16();
				SpeedZ = par1NetworkStream.ReadInt16();
			}
		}

		/// <summary>
		/// Abstract. Writes the raw packet data to the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void writePacketData(DataOutputStream par1DataOutputStream) throws IOException
        public override void WritePacketData(BinaryWriter par1DataOutputStream)
		{
			par1DataOutputStream.Write(EntityId);
			par1DataOutputStream.Write(Type);
			par1DataOutputStream.Write(XPosition);
			par1DataOutputStream.Write(YPosition);
			par1DataOutputStream.Write(ZPosition);
			par1DataOutputStream.Write(ThrowerEntityId);

			if (ThrowerEntityId > 0)
			{
				par1DataOutputStream.Write(SpeedX);
				par1DataOutputStream.Write(SpeedY);
				par1DataOutputStream.Write(SpeedZ);
			}
		}

		/// <summary>
		/// Passes this Packet on to the NetHandler for processing.
		/// </summary>
		public override void ProcessPacket(NetHandler par1NetHandler)
		{
			par1NetHandler.HandleVehicleSpawn(this);
		}

		/// <summary>
		/// Abstract. Return the size of the packet (not counting the header).
		/// </summary>
		public override int GetPacketSize()
		{
			return 21 + ThrowerEntityId <= 0 ? 0 : 6;
		}
	}
}