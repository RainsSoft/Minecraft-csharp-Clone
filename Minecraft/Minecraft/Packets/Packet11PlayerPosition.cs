using System.Net.Sockets;
using System.IO;

namespace net.minecraft.src
{
	public class Packet11PlayerPosition : Packet10Flying
	{
		public Packet11PlayerPosition()
		{
			Moving = true;
		}

		public Packet11PlayerPosition(double par1, double par3, double par5, double par7, bool par9)
		{
			XPosition = par1;
			YPosition = par3;
			Stance = par5;
			ZPosition = par7;
			OnGround = par9;
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
			base.WritePacketData(par1DataOutputStream);
		}

		/// <summary>
		/// Abstract. Return the size of the packet (not counting the header).
		/// </summary>
		public override int GetPacketSize()
		{
			return 33;
		}
	}
}