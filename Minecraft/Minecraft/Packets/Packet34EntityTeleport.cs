using System.Net.Sockets;
using System.IO;

namespace net.minecraft.src
{
	public class Packet34EntityTeleport : Packet
	{
		/// <summary>
		/// ID of the entity. </summary>
		public int EntityId;

		/// <summary>
		/// X position of the entity. </summary>
		public int XPosition;

		/// <summary>
		/// Y position of the entity. </summary>
		public int YPosition;

		/// <summary>
		/// Z position of the entity. </summary>
		public int ZPosition;

		/// <summary>
		/// Yaw of the entity. </summary>
		public sbyte Yaw;

		/// <summary>
		/// Pitch of the entity. </summary>
		public sbyte Pitch;

		public Packet34EntityTeleport()
		{
		}

		public Packet34EntityTeleport(Entity par1Entity)
		{
			EntityId = par1Entity.EntityId;
			XPosition = MathHelper2.Floor_double(par1Entity.PosX * 32D);
			YPosition = MathHelper2.Floor_double(par1Entity.PosY * 32D);
			ZPosition = MathHelper2.Floor_double(par1Entity.PosZ * 32D);
			Yaw = (sbyte)(int)((par1Entity.RotationYaw * 256F) / 360F);
			Pitch = (sbyte)(int)((par1Entity.RotationPitch * 256F) / 360F);
		}

		/// <summary>
		/// Abstract. Reads the raw packet data from the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void readPacketData(BinaryReader par1NetworkStream) throws IOException
		public override void ReadPacketData(BinaryReader par1NetworkStream)
		{
			EntityId = par1NetworkStream.ReadInt32();
			XPosition = par1NetworkStream.ReadInt32();
			YPosition = par1NetworkStream.ReadInt32();
			ZPosition = par1NetworkStream.ReadInt32();
			Yaw = par1NetworkStream.ReadSByte();
			Pitch = par1NetworkStream.ReadSByte();
		}

		/// <summary>
		/// Abstract. Writes the raw packet data to the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void writePacketData(DataOutputStream par1DataOutputStream) throws IOException
        public override void WritePacketData(BinaryWriter par1DataOutputStream)
		{
			par1DataOutputStream.Write(EntityId);
			par1DataOutputStream.Write(XPosition);
			par1DataOutputStream.Write(YPosition);
			par1DataOutputStream.Write(ZPosition);
			par1DataOutputStream.Write(Yaw);
			par1DataOutputStream.Write(Pitch);
		}

		/// <summary>
		/// Passes this Packet on to the NetHandler for processing.
		/// </summary>
		public override void ProcessPacket(NetHandler par1NetHandler)
		{
			par1NetHandler.HandleEntityTeleport(this);
		}

		/// <summary>
		/// Abstract. Return the size of the packet (not counting the header).
		/// </summary>
		public override int GetPacketSize()
		{
			return 34;
		}
	}
}