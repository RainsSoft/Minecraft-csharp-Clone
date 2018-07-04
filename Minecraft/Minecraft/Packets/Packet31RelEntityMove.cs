using System.Net.Sockets;
using System.IO;

namespace net.minecraft.src
{
	public class Packet31RelEntityMove : Packet30Entity
	{
		public Packet31RelEntityMove()
		{
		}

		/// <summary>
		/// Abstract. Reads the raw packet data from the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void readPacketData(BinaryReader par1NetworkStream) throws IOException
		public override void ReadPacketData(BinaryReader par1NetworkStream)
		{
			base.ReadPacketData(par1NetworkStream);
			XPosition = (sbyte)par1NetworkStream.ReadByte();
            YPosition = (sbyte)par1NetworkStream.ReadByte();
            ZPosition = (sbyte)par1NetworkStream.ReadByte();
		}

		/// <summary>
		/// Abstract. Writes the raw packet data to the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void writePacketData(DataOutputStream par1DataOutputStream) throws IOException
        public override void WritePacketData(BinaryWriter par1DataOutputStream)
		{
			base.WritePacketData(par1DataOutputStream);
			par1DataOutputStream.Write((byte)XPosition);
            par1DataOutputStream.Write((byte)YPosition);
            par1DataOutputStream.Write((byte)ZPosition);
		}

		/// <summary>
		/// Abstract. Return the size of the packet (not counting the header).
		/// </summary>
		public override int GetPacketSize()
		{
			return 7;
		}
	}
}