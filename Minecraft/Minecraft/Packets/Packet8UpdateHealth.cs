using System.Net.Sockets;
using System.IO;

namespace net.minecraft.src
{
	public class Packet8UpdateHealth : Packet
	{
		/// <summary>
		/// Variable used for incoming health packets </summary>
		public int HealthMP;
		public int Food;

		/// <summary>
		/// Players logging on get a saturation of 5.0. Eating food increases the saturation as well as the food bar.
		/// </summary>
		public float FoodSaturation;

		public Packet8UpdateHealth()
		{
		}

		/// <summary>
		/// Abstract. Reads the raw packet data from the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void readPacketData(BinaryReader par1NetworkStream) throws IOException
		public override void ReadPacketData(BinaryReader par1NetworkStream)
		{
			HealthMP = par1NetworkStream.ReadInt16();
			Food = par1NetworkStream.ReadInt16();
			FoodSaturation = par1NetworkStream.ReadSingle();
		}

		/// <summary>
		/// Abstract. Writes the raw packet data to the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void writePacketData(DataOutputStream par1DataOutputStream) throws IOException
        public override void WritePacketData(BinaryWriter par1DataOutputStream)
		{
			par1DataOutputStream.Write(HealthMP);
			par1DataOutputStream.Write(Food);
			par1DataOutputStream.Write(FoodSaturation);
		}

		/// <summary>
		/// Passes this Packet on to the NetHandler for processing.
		/// </summary>
		public override void ProcessPacket(NetHandler par1NetHandler)
		{
			par1NetHandler.HandleUpdateHealth(this);
		}

		/// <summary>
		/// Abstract. Return the size of the packet (not counting the header).
		/// </summary>
		public override int GetPacketSize()
		{
			return 8;
		}
	}
}