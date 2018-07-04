using System.Net.Sockets;
using System.IO;

namespace net.minecraft.src
{
	public class Packet12PlayerLook : Packet10Flying
	{
		public Packet12PlayerLook()
		{
			Rotating = true;
		}

		public Packet12PlayerLook(float par1, float par2, bool par3)
		{
			Yaw = par1;
			Pitch = par2;
			OnGround = par3;
			Rotating = true;
		}

		/// <summary>
		/// Abstract. Reads the raw packet data from the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void readPacketData(BinaryReader par1NetworkStream) throws IOException
		public override void ReadPacketData(BinaryReader par1NetworkStream)
		{
			Yaw = par1NetworkStream.ReadSingle();
			Pitch = par1NetworkStream.ReadSingle();
			base.ReadPacketData(par1NetworkStream);
		}

		/// <summary>
		/// Abstract. Writes the raw packet data to the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void writePacketData(DataOutputStream par1DataOutputStream) throws IOException
        public override void WritePacketData(BinaryWriter par1DataOutputStream)
		{
			par1DataOutputStream.Write(Yaw);
			par1DataOutputStream.Write(Pitch);
			base.WritePacketData(par1DataOutputStream);
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