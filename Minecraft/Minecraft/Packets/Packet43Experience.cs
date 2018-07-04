using System.Net.Sockets;
using System.IO;

namespace net.minecraft.src
{
	public class Packet43Experience : Packet
	{
		/// <summary>
		/// The current experience bar points. </summary>
		public float Experience;

		/// <summary>
		/// The total experience points. </summary>
		public int ExperienceTotal;

		/// <summary>
		/// The experience level. </summary>
		public int ExperienceLevel;

		public Packet43Experience()
		{
		}

		/// <summary>
		/// Abstract. Reads the raw packet data from the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void readPacketData(BinaryReader par1NetworkStream) throws IOException
		public override void ReadPacketData(BinaryReader par1NetworkStream)
		{
			Experience = par1NetworkStream.ReadSingle();
			ExperienceLevel = par1NetworkStream.ReadInt16();
			ExperienceTotal = par1NetworkStream.ReadInt16();
		}

		/// <summary>
		/// Abstract. Writes the raw packet data to the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void writePacketData(DataOutputStream par1DataOutputStream) throws IOException
        public override void WritePacketData(BinaryWriter par1DataOutputStream)
		{
			par1DataOutputStream.Write(Experience);
			par1DataOutputStream.Write(ExperienceLevel);
			par1DataOutputStream.Write(ExperienceTotal);
		}

		/// <summary>
		/// Passes this Packet on to the NetHandler for processing.
		/// </summary>
		public override void ProcessPacket(NetHandler par1NetHandler)
		{
			par1NetHandler.HandleExperience(this);
		}

		/// <summary>
		/// Abstract. Return the size of the packet (not counting the header).
		/// </summary>
		public override int GetPacketSize()
		{
			return 4;
		}
	}
}