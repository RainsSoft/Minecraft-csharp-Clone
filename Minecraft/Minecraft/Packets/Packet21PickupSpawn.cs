using System.Net.Sockets;
using System.IO;

namespace net.minecraft.src
{
	public class Packet21PickupSpawn : Packet
	{
		/// <summary>
		/// Unique entity ID. </summary>
		public int EntityId;

		/// <summary>
		/// The item X position. </summary>
		public int XPosition;

		/// <summary>
		/// The item Y position. </summary>
		public int YPosition;

		/// <summary>
		/// The item Z position. </summary>
		public int ZPosition;

		/// <summary>
		/// The item rotation. </summary>
		public sbyte Rotation;

		/// <summary>
		/// The item pitch. </summary>
		public sbyte Pitch;

		/// <summary>
		/// The item roll. </summary>
		public sbyte Roll;
		public int ItemID;

		/// <summary>
		/// The number of items. </summary>
		public int Count;

		/// <summary>
		/// The health of the item. </summary>
		public int ItemDamage;

		public Packet21PickupSpawn()
		{
		}

		public Packet21PickupSpawn(EntityItem par1EntityItem)
		{
			EntityId = par1EntityItem.EntityId;
			ItemID = par1EntityItem.ItemStack.ItemID;
			Count = par1EntityItem.ItemStack.StackSize;
			ItemDamage = par1EntityItem.ItemStack.GetItemDamage();
			XPosition = MathHelper2.Floor_double(par1EntityItem.PosX * 32D);
			YPosition = MathHelper2.Floor_double(par1EntityItem.PosY * 32D);
			ZPosition = MathHelper2.Floor_double(par1EntityItem.PosZ * 32D);
			Rotation = (sbyte)(int)(par1EntityItem.MotionX * 128D);
			Pitch = (sbyte)(int)(par1EntityItem.MotionY * 128D);
			Roll = (sbyte)(int)(par1EntityItem.MotionZ * 128D);
		}

		/// <summary>
		/// Abstract. Reads the raw packet data from the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void readPacketData(BinaryReader par1NetworkStream) throws IOException
		public override void ReadPacketData(BinaryReader par1NetworkStream)
		{
			EntityId = par1NetworkStream.ReadInt32();
			ItemID = par1NetworkStream.ReadInt16();
			Count = par1NetworkStream.ReadByte();
			ItemDamage = par1NetworkStream.ReadInt16();
			XPosition = par1NetworkStream.ReadInt32();
			YPosition = par1NetworkStream.ReadInt32();
			ZPosition = par1NetworkStream.ReadInt32();
			Rotation = par1NetworkStream.ReadSByte();
			Pitch = par1NetworkStream.ReadSByte();
			Roll = par1NetworkStream.ReadSByte();
		}

		/// <summary>
		/// Abstract. Writes the raw packet data to the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void writePacketData(DataOutputStream par1DataOutputStream) throws IOException
        public override void WritePacketData(BinaryWriter par1DataOutputStream)
		{
			par1DataOutputStream.Write(EntityId);
			par1DataOutputStream.Write(ItemID);
			par1DataOutputStream.Write(Count);
			par1DataOutputStream.Write(ItemDamage);
			par1DataOutputStream.Write(XPosition);
			par1DataOutputStream.Write(YPosition);
			par1DataOutputStream.Write(ZPosition);
			par1DataOutputStream.Write(Rotation);
			par1DataOutputStream.Write(Pitch);
			par1DataOutputStream.Write(Roll);
		}

		/// <summary>
		/// Passes this Packet on to the NetHandler for processing.
		/// </summary>
		public override void ProcessPacket(NetHandler par1NetHandler)
		{
			par1NetHandler.HandlePickupSpawn(this);
		}

		/// <summary>
		/// Abstract. Return the size of the packet (not counting the header).
		/// </summary>
		public override int GetPacketSize()
		{
			return 24;
		}
	}
}