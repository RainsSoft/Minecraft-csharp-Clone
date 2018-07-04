using System.Net.Sockets;
using System.IO;

namespace net.minecraft.src
{
	public class Packet13PlayerLookMove : Packet10Flying
	{
		public Packet13PlayerLookMove()
		{
			Rotating = true;
			Moving = true;
		}

		public Packet13PlayerLookMove(double par1, double par3, double par5, double par7, float par9, float par10, bool par11)
		{
			XPosition = par1;
			YPosition = par3;
			Stance = par5;
			ZPosition = par7;
			Yaw = par9;
			Pitch = par10;
			OnGround = par11;
			Rotating = true;
			Moving = true;
		}

		/// <summary>
		/// Abstract. Reads the raw packet data from the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void readPacketData(BinaryReader par1NetworkStream) throws IOException
		public override void ReadPacketData(BinaryReader par1NetworkStream)
		{
			XPosition = par1NetworkStream.ReadDouble();
			YPosition = par1NetworkStream.ReadDouble();
			Stance = par1NetworkStream.ReadDouble();
			ZPosition = par1NetworkStream.ReadDouble();
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
			par1DataOutputStream.Write(XPosition);
			par1DataOutputStream.Write(YPosition);
			par1DataOutputStream.Write(Stance);
			par1DataOutputStream.Write(ZPosition);
			par1DataOutputStream.Write(Yaw);
			par1DataOutputStream.Write(Pitch);
			base.WritePacketData(par1DataOutputStream);
		}

		/// <summary>
		/// Abstract. Return the size of the packet (not counting the header).
		/// </summary>
		public override int GetPacketSize()
		{
			return 41;
		}
	}
}