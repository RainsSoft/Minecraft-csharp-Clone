using System.Net.Sockets;
using System.IO;

namespace net.minecraft.src
{
	public class Packet71Weather : Packet
	{
		public int EntityID;
		public int PosX;
		public int PosY;
		public int PosZ;
		public int IsLightningBolt;

		public Packet71Weather()
		{
		}

		public Packet71Weather(Entity par1Entity)
		{
			EntityID = par1Entity.EntityId;
			PosX = MathHelper2.Floor_double(par1Entity.PosX * 32D);
			PosY = MathHelper2.Floor_double(par1Entity.PosY * 32D);
			PosZ = MathHelper2.Floor_double(par1Entity.PosZ * 32D);

			if (par1Entity is EntityLightningBolt)
			{
				IsLightningBolt = 1;
			}
		}

		/// <summary>
		/// Abstract. Reads the raw packet data from the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void readPacketData(BinaryReader par1NetworkStream) throws IOException
		public override void ReadPacketData(BinaryReader par1NetworkStream)
		{
			EntityID = par1NetworkStream.ReadInt32();
			IsLightningBolt = par1NetworkStream.ReadByte();
			PosX = par1NetworkStream.ReadInt32();
			PosY = par1NetworkStream.ReadInt32();
			PosZ = par1NetworkStream.ReadInt32();
		}

		/// <summary>
		/// Abstract. Writes the raw packet data to the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void writePacketData(DataOutputStream par1DataOutputStream) throws IOException
        public override void WritePacketData(BinaryWriter par1DataOutputStream)
		{
			par1DataOutputStream.Write(EntityID);
			par1DataOutputStream.Write(IsLightningBolt);
			par1DataOutputStream.Write(PosX);
			par1DataOutputStream.Write(PosY);
			par1DataOutputStream.Write(PosZ);
		}

		/// <summary>
		/// Passes this Packet on to the NetHandler for processing.
		/// </summary>
		public override void ProcessPacket(NetHandler par1NetHandler)
		{
			par1NetHandler.HandleWeather(this);
		}

		/// <summary>
		/// Abstract. Return the size of the packet (not counting the header).
		/// </summary>
		public override int GetPacketSize()
		{
			return 17;
		}
	}
}