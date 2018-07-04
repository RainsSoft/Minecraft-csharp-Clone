using System.Net.Sockets;
using System.IO;

namespace net.minecraft.src
{
	public class Packet26EntityExpOrb : Packet
	{
		/// <summary>
		/// Entity ID for the XP Orb </summary>
		public int EntityId;
		public int PosX;
		public int PosY;
		public int PosZ;

		/// <summary>
		/// The Orbs Experience points value. </summary>
		public int XpValue;

		public Packet26EntityExpOrb()
		{
		}

		public Packet26EntityExpOrb(EntityXPOrb par1EntityXPOrb)
		{
			EntityId = par1EntityXPOrb.EntityId;
			PosX = MathHelper2.Floor_double(par1EntityXPOrb.PosX * 32D);
			PosY = MathHelper2.Floor_double(par1EntityXPOrb.PosY * 32D);
			PosZ = MathHelper2.Floor_double(par1EntityXPOrb.PosZ * 32D);
			XpValue = par1EntityXPOrb.GetXpValue();
		}

		/// <summary>
		/// Abstract. Reads the raw packet data from the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void readPacketData(BinaryReader par1NetworkStream) throws IOException
		public override void ReadPacketData(BinaryReader par1NetworkStream)
		{
			EntityId = par1NetworkStream.ReadInt32();
			PosX = par1NetworkStream.ReadInt32();
			PosY = par1NetworkStream.ReadInt32();
			PosZ = par1NetworkStream.ReadInt32();
			XpValue = par1NetworkStream.ReadInt16();
		}

		/// <summary>
		/// Abstract. Writes the raw packet data to the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void writePacketData(DataOutputStream par1DataOutputStream) throws IOException
        public override void WritePacketData(BinaryWriter par1DataOutputStream)
		{
			par1DataOutputStream.Write(EntityId);
			par1DataOutputStream.Write(PosX);
			par1DataOutputStream.Write(PosY);
			par1DataOutputStream.Write(PosZ);
			par1DataOutputStream.Write(XpValue);
		}

		/// <summary>
		/// Passes this Packet on to the NetHandler for processing.
		/// </summary>
		public override void ProcessPacket(NetHandler par1NetHandler)
		{
			par1NetHandler.HandleEntityExpOrb(this);
		}

		/// <summary>
		/// Abstract. Return the size of the packet (not counting the header).
		/// </summary>
		public override int GetPacketSize()
		{
			return 18;
		}
	}
}