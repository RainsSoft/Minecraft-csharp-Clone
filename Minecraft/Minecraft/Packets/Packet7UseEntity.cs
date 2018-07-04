using System.Net.Sockets;
using System.IO;

namespace net.minecraft.src
{
	public class Packet7UseEntity : Packet
	{
		/// <summary>
		/// The entity of the player (ignored by the server) </summary>
		public int PlayerEntityId;

		/// <summary>
		/// The entity the player is interacting with </summary>
		public int TargetEntity;

		/// <summary>
		/// Seems to be true when the player is pointing at an entity and left-clicking and false when right-clicking.
		/// </summary>
		public int IsLeftClick;

		public Packet7UseEntity()
		{
		}

		public Packet7UseEntity(int par1, int par2, int par3)
		{
			PlayerEntityId = par1;
			TargetEntity = par2;
			IsLeftClick = par3;
		}

		/// <summary>
		/// Abstract. Reads the raw packet data from the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void readPacketData(BinaryReader par1NetworkStream) throws IOException
		public override void ReadPacketData(BinaryReader par1NetworkStream)
		{
			PlayerEntityId = par1NetworkStream.ReadInt32();
			TargetEntity = par1NetworkStream.ReadInt32();
			IsLeftClick = par1NetworkStream.ReadByte();
		}

		/// <summary>
		/// Abstract. Writes the raw packet data to the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void writePacketData(DataOutputStream par1DataOutputStream) throws IOException
        public override void WritePacketData(BinaryWriter par1DataOutputStream)
		{
			par1DataOutputStream.Write(PlayerEntityId);
			par1DataOutputStream.Write(TargetEntity);
			par1DataOutputStream.Write(IsLeftClick);
		}

		/// <summary>
		/// Passes this Packet on to the NetHandler for processing.
		/// </summary>
		public override void ProcessPacket(NetHandler par1NetHandler)
		{
			par1NetHandler.HandleUseEntity(this);
		}

		/// <summary>
		/// Abstract. Return the size of the packet (not counting the header).
		/// </summary>
		public override int GetPacketSize()
		{
			return 9;
		}
	}
}