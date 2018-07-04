using System.Net.Sockets;
using System.IO;

namespace net.minecraft.src
{
	public class Packet250CustomPayload : Packet
	{
		/// <summary>
		/// Name of the 'channel' used to send data </summary>
		public string Channel;

		/// <summary>
		/// Length of the data to be read </summary>
		public int Length;
		public byte[] Data;

		public Packet250CustomPayload()
		{
		}

		/// <summary>
		/// Abstract. Reads the raw packet data from the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void readPacketData(BinaryReader par1NetworkStream) throws IOException
		public override void ReadPacketData(BinaryReader par1NetworkStream)
		{
			Channel = ReadString(par1NetworkStream, 16);
			Length = par1NetworkStream.ReadInt16();

			if (Length > 0 && Length < 32767)
			{
				Data = new byte[Length];
				par1NetworkStream.Read(Data, 0, Data.Length);
			}
		}

		/// <summary>
		/// Abstract. Writes the raw packet data to the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void writePacketData(DataOutputStream par1DataOutputStream) throws IOException
        public override void WritePacketData(BinaryWriter par1DataOutputStream)
		{
			WriteString(Channel, par1DataOutputStream);
			par1DataOutputStream.Write((short)Length);

			if (Data != null)
			{
				par1DataOutputStream.Write(Data);
			}
		}

		/// <summary>
		/// Passes this Packet on to the NetHandler for processing.
		/// </summary>
		public override void ProcessPacket(NetHandler par1NetHandler)
		{
			par1NetHandler.HandleCustomPayload(this);
		}

		/// <summary>
		/// Abstract. Return the size of the packet (not counting the header).
		/// </summary>
		public override int GetPacketSize()
		{
			return 2 + Channel.Length * 2 + 2 + Length;
		}
	}
}