using System.Net.Sockets;
using System.IO;

namespace net.minecraft.src
{
	public class Packet52MultiBlockChange : Packet
	{
		/// <summary>
		/// Chunk X position. </summary>
		public int XPosition;

		/// <summary>
		/// Chunk Z position. </summary>
		public int ZPosition;
		public byte[] MetadataArray;

		/// <summary>
		/// The size of the arrays. </summary>
		public int Size;
		private static sbyte[] Field_48168_e = new sbyte[0];

		public Packet52MultiBlockChange()
		{
			IsChunkDataPacket = true;
		}

		/// <summary>
		/// Abstract. Reads the raw packet data from the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void readPacketData(BinaryReader par1NetworkStream) throws IOException
		public override void ReadPacketData(BinaryReader par1NetworkStream)
		{
			XPosition = par1NetworkStream.ReadInt32();
			ZPosition = par1NetworkStream.ReadInt32();
			Size = par1NetworkStream.ReadInt16() & 0xffff;
			int i = par1NetworkStream.ReadInt32();

			if (i > 0)
			{
				MetadataArray = new byte[i];
				par1NetworkStream.Read(MetadataArray, 0, MetadataArray.Length);
			}
		}

		/// <summary>
		/// Abstract. Writes the raw packet data to the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void writePacketData(DataOutputStream par1DataOutputStream) throws IOException
        public override void WritePacketData(BinaryWriter par1DataOutputStream)
		{
			par1DataOutputStream.Write(XPosition);
			par1DataOutputStream.Write(ZPosition);
			par1DataOutputStream.Write((short)Size);

			if (MetadataArray != null)
			{
				par1DataOutputStream.Write(MetadataArray.Length);
				par1DataOutputStream.Write(MetadataArray);
			}
			else
			{
				par1DataOutputStream.Write(0);
			}
		}

		/// <summary>
		/// Passes this Packet on to the NetHandler for processing.
		/// </summary>
		public override void ProcessPacket(NetHandler par1NetHandler)
		{
			par1NetHandler.HandleMultiBlockChange(this);
		}

		/// <summary>
		/// Abstract. Return the size of the packet (not counting the header).
		/// </summary>
		public override int GetPacketSize()
		{
			return 10 + Size * 4;
		}
	}
}