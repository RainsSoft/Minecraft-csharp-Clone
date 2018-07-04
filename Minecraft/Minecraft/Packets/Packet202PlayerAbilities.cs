using System.Net.Sockets;
using System.IO;

namespace net.minecraft.src
{
	public class Packet202PlayerAbilities : Packet
	{
		public bool Field_50072_a;
		public bool Field_50070_b;
		public bool Field_50071_c;
		public bool Field_50069_d;

		public Packet202PlayerAbilities()
		{
			Field_50072_a = false;
			Field_50070_b = false;
			Field_50071_c = false;
			Field_50069_d = false;
		}

		public Packet202PlayerAbilities(PlayerCapabilities par1PlayerCapabilities)
		{
			Field_50072_a = false;
			Field_50070_b = false;
			Field_50071_c = false;
			Field_50069_d = false;
			Field_50072_a = par1PlayerCapabilities.DisableDamage;
			Field_50070_b = par1PlayerCapabilities.IsFlying;
			Field_50071_c = par1PlayerCapabilities.AllowFlying;
			Field_50069_d = par1PlayerCapabilities.IsCreativeMode;
		}

		/// <summary>
		/// Abstract. Reads the raw packet data from the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void readPacketData(BinaryReader par1NetworkStream) throws IOException
		public override void ReadPacketData(BinaryReader par1NetworkStream)
		{
			Field_50072_a = par1NetworkStream.ReadBoolean();
			Field_50070_b = par1NetworkStream.ReadBoolean();
			Field_50071_c = par1NetworkStream.ReadBoolean();
			Field_50069_d = par1NetworkStream.ReadBoolean();
		}

		/// <summary>
		/// Abstract. Writes the raw packet data to the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void writePacketData(DataOutputStream par1DataOutputStream) throws IOException
        public override void WritePacketData(BinaryWriter par1DataOutputStream)
		{
			par1DataOutputStream.Write(Field_50072_a);
			par1DataOutputStream.Write(Field_50070_b);
			par1DataOutputStream.Write(Field_50071_c);
			par1DataOutputStream.Write(Field_50069_d);
		}

		/// <summary>
		/// Passes this Packet on to the NetHandler for processing.
		/// </summary>
		public override void ProcessPacket(NetHandler par1NetHandler)
		{
			par1NetHandler.Func_50100_a(this);
		}

		/// <summary>
		/// Abstract. Return the size of the packet (not counting the header).
		/// </summary>
		public override int GetPacketSize()
		{
			return 1;
		}
	}
}