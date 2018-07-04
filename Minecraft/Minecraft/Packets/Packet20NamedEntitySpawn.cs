using System.Net.Sockets;
using System.IO;

namespace net.minecraft.src
{
	public class Packet20NamedEntitySpawn : Packet
	{
		/// <summary>
		/// The entity ID, in this case it's the player ID. </summary>
		public int EntityId;

		/// <summary>
		/// The player's name. </summary>
		public string Name;

		/// <summary>
		/// The player's X position. </summary>
		public int XPosition;

		/// <summary>
		/// The player's Y position. </summary>
		public int YPosition;

		/// <summary>
		/// The player's Z position. </summary>
		public int ZPosition;

		/// <summary>
		/// The player's rotation. </summary>
		public sbyte Rotation;

		/// <summary>
		/// The player's pitch. </summary>
		public sbyte Pitch;

		/// <summary>
		/// The current item the player is holding. </summary>
		public int CurrentItem;

		public Packet20NamedEntitySpawn()
		{
		}

		public Packet20NamedEntitySpawn(EntityPlayer par1EntityPlayer)
		{
			EntityId = par1EntityPlayer.EntityId;
			Name = par1EntityPlayer.Username;
			XPosition = MathHelper2.Floor_double(par1EntityPlayer.PosX * 32D);
			YPosition = MathHelper2.Floor_double(par1EntityPlayer.PosY * 32D);
			ZPosition = MathHelper2.Floor_double(par1EntityPlayer.PosZ * 32D);
			Rotation = (sbyte)(int)((par1EntityPlayer.RotationYaw * 256F) / 360F);
			Pitch = (sbyte)(int)((par1EntityPlayer.RotationPitch * 256F) / 360F);
			ItemStack itemstack = par1EntityPlayer.Inventory.GetCurrentItem();
			CurrentItem = itemstack != null ? itemstack.ItemID : 0;
		}

		/// <summary>
		/// Abstract. Reads the raw packet data from the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void readPacketData(BinaryReader par1NetworkStream) throws IOException
        public override void ReadPacketData(BinaryReader par1NetworkStream)
		{
			EntityId = par1NetworkStream.ReadInt32();
			Name = ReadString(par1NetworkStream, 16);
			XPosition = par1NetworkStream.ReadInt32();
			YPosition = par1NetworkStream.ReadInt32();
			ZPosition = par1NetworkStream.ReadInt32();
			Rotation = par1NetworkStream.ReadSByte();
			Pitch = par1NetworkStream.ReadSByte();
			CurrentItem = par1NetworkStream.ReadInt16();
		}

		/// <summary>
		/// Abstract. Writes the raw packet data to the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void writePacketData(DataOutputStream par1DataOutputStream) throws IOException
        public override void WritePacketData(BinaryWriter par1DataOutputStream)
		{
			par1DataOutputStream.Write(EntityId);
			WriteString(Name, par1DataOutputStream);
			par1DataOutputStream.Write(XPosition);
			par1DataOutputStream.Write(YPosition);
			par1DataOutputStream.Write(ZPosition);
			par1DataOutputStream.Write(Rotation);
			par1DataOutputStream.Write(Pitch);
			par1DataOutputStream.Write(CurrentItem);
		}

		/// <summary>
		/// Passes this Packet on to the NetHandler for processing.
		/// </summary>
		public override void ProcessPacket(NetHandler par1NetHandler)
		{
			par1NetHandler.HandleNamedEntitySpawn(this);
		}

		/// <summary>
		/// Abstract. Return the size of the packet (not counting the header).
		/// </summary>
		public override int GetPacketSize()
		{
			return 28;
		}
	}
}