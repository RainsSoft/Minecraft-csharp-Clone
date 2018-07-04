using System.Net.Sockets;
using System.IO;

namespace net.minecraft.src
{
	public class Packet32EntityLook : Packet30Entity
	{
		public Packet32EntityLook()
		{
			Rotating = true;
		}

		/// <summary>
		/// Abstract. Reads the raw packet data from the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void readPacketData(BinaryReader par1NetworkStream) throws IOException
		public override void ReadPacketData(BinaryReader par1NetworkStream)
		{
			base.ReadPacketData(par1NetworkStream);
			Yaw = (sbyte)par1NetworkStream.ReadByte();
			Pitch = (sbyte)par1NetworkStream.ReadByte();
		}

		/// <summary>
		/// Abstract. Writes the raw packet data to the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void writePacketData(DataOutputStream par1DataOutputStream) throws IOException
        public override void WritePacketData(BinaryWriter par1DataOutputStream)
		{
			base.WritePacketData(par1DataOutputStream);
			par1DataOutputStream.Write(Yaw);
			par1DataOutputStream.Write(Pitch);
		}

		/// <summary>
		/// Abstract. Return the size of the packet (not counting the header).
		/// </summary>
		public override int GetPacketSize()
		{
			return 6;
		}
	}
}