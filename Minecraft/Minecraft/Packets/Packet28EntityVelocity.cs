using System.Net.Sockets;
using System.IO;

namespace net.minecraft.src
{
	public class Packet28EntityVelocity : Packet
	{
		public int EntityId;
		public int MotionX;
		public int MotionY;
		public int MotionZ;

		public Packet28EntityVelocity()
		{
		}

		public Packet28EntityVelocity(Entity par1Entity) : this(par1Entity.EntityId, par1Entity.MotionX, par1Entity.MotionY, par1Entity.MotionZ)
		{
		}

		public Packet28EntityVelocity(int par1, double par2, double par4, double par6)
		{
			EntityId = par1;
			double d = 3.8999999999999999D;

			if (par2 < -d)
			{
				par2 = -d;
			}

			if (par4 < -d)
			{
				par4 = -d;
			}

			if (par6 < -d)
			{
				par6 = -d;
			}

			if (par2 > d)
			{
				par2 = d;
			}

			if (par4 > d)
			{
				par4 = d;
			}

			if (par6 > d)
			{
				par6 = d;
			}

			MotionX = (int)(par2 * 8000D);
			MotionY = (int)(par4 * 8000D);
			MotionZ = (int)(par6 * 8000D);
		}

		/// <summary>
		/// Abstract. Reads the raw packet data from the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void readPacketData(BinaryReader par1NetworkStream) throws IOException
		public override void ReadPacketData(BinaryReader par1NetworkStream)
		{
			EntityId = par1NetworkStream.ReadInt32();
			MotionX = par1NetworkStream.ReadInt16();
			MotionY = par1NetworkStream.ReadInt16();
			MotionZ = par1NetworkStream.ReadInt16();
		}

		/// <summary>
		/// Abstract. Writes the raw packet data to the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void writePacketData(DataOutputStream par1DataOutputStream) throws IOException
        public override void WritePacketData(BinaryWriter par1DataOutputStream)
		{
			par1DataOutputStream.Write(EntityId);
			par1DataOutputStream.Write(MotionX);
			par1DataOutputStream.Write(MotionY);
			par1DataOutputStream.Write(MotionZ);
		}

		/// <summary>
		/// Passes this Packet on to the NetHandler for processing.
		/// </summary>
		public override void ProcessPacket(NetHandler par1NetHandler)
		{
			par1NetHandler.HandleEntityVelocity(this);
		}

		/// <summary>
		/// Abstract. Return the size of the packet (not counting the header).
		/// </summary>
		public override int GetPacketSize()
		{
			return 10;
		}
	}
}