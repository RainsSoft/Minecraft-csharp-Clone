using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.IO;

namespace net.minecraft.src
{
	public class Packet24MobSpawn : Packet
	{
		/// <summary>
		/// The entity ID. </summary>
		public int EntityId;

		/// <summary>
		/// The type of mob. </summary>
		public int Type;

		/// <summary>
		/// The X position of the entity. </summary>
		public int XPosition;

		/// <summary>
		/// The Y position of the entity. </summary>
		public int YPosition;

		/// <summary>
		/// The Z position of the entity. </summary>
		public int ZPosition;

		/// <summary>
		/// The yaw of the entity. </summary>
		public sbyte Yaw;

		/// <summary>
		/// The pitch of the entity. </summary>
		public sbyte Pitch;
		public sbyte Field_48169_h;

		/// <summary>
		/// Indexed metadata for Mob, terminated by 0x7F </summary>
		private DataWatcher MetaData;
        private List<WatchableObject> ReceivedMetadata;

		public Packet24MobSpawn()
		{
		}

		public Packet24MobSpawn(EntityLiving par1EntityLiving)
		{
			EntityId = par1EntityLiving.EntityId;
			Type = (sbyte)EntityList.GetEntityID(par1EntityLiving);
			XPosition = MathHelper2.Floor_double(par1EntityLiving.PosX * 32D);
			YPosition = MathHelper2.Floor_double(par1EntityLiving.PosY * 32D);
			ZPosition = MathHelper2.Floor_double(par1EntityLiving.PosZ * 32D);
			Yaw = (sbyte)(int)((par1EntityLiving.RotationYaw * 256F) / 360F);
			Pitch = (sbyte)(int)((par1EntityLiving.RotationPitch * 256F) / 360F);
			Field_48169_h = (sbyte)(int)((par1EntityLiving.RotationYawHead * 256F) / 360F);
			MetaData = par1EntityLiving.GetDataWatcher();
		}

		/// <summary>
		/// Abstract. Reads the raw packet data from the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void readPacketData(BinaryReader par1NetworkStream) throws IOException
        public override void ReadPacketData(BinaryReader par1NetworkStream)
		{
			EntityId = par1NetworkStream.ReadInt32();
			Type = par1NetworkStream.ReadByte() & 0xff;
			XPosition = par1NetworkStream.ReadInt32();
			YPosition = par1NetworkStream.ReadInt32();
			ZPosition = par1NetworkStream.ReadInt32();
			Yaw = (sbyte)par1NetworkStream.ReadByte();
			Pitch = (sbyte)par1NetworkStream.ReadByte();
			Field_48169_h = (sbyte)par1NetworkStream.ReadByte();
			ReceivedMetadata = DataWatcher.ReadWatchableObjects(par1NetworkStream);
		}

		/// <summary>
		/// Abstract. Writes the raw packet data to the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void writePacketData(DataOutputStream par1DataOutputStream) throws IOException
        public override void WritePacketData(BinaryWriter par1DataOutputStream)
		{
			par1DataOutputStream.Write(EntityId);
			par1DataOutputStream.Write(Type & 0xff);
			par1DataOutputStream.Write(XPosition);
			par1DataOutputStream.Write(YPosition);
			par1DataOutputStream.Write(ZPosition);
			par1DataOutputStream.Write(Yaw);
			par1DataOutputStream.Write(Pitch);
			par1DataOutputStream.Write(Field_48169_h);
			MetaData.WriteWatchableObjects(par1DataOutputStream);
		}

		/// <summary>
		/// Passes this Packet on to the NetHandler for processing.
		/// </summary>
		public override void ProcessPacket(NetHandler par1NetHandler)
		{
			par1NetHandler.HandleMobSpawn(this);
		}

		/// <summary>
		/// Abstract. Return the size of the packet (not counting the header).
		/// </summary>
		public override int GetPacketSize()
		{
			return 20;
		}

        public virtual List<WatchableObject> GetMetadata()
		{
			return ReceivedMetadata;
		}
	}
}