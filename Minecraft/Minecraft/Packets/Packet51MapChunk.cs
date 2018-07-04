using System.Net.Sockets;
using System.IO;
using ICSharpCode.SharpZipLib.Zip.Compression;

namespace net.minecraft.src
{
	public class Packet51MapChunk : Packet
	{
		/// <summary>
		/// The x-position of the transmitted chunk, in chunk coordinates. </summary>
		public int XCh;

		/// <summary>
		/// The z-position of the transmitted chunk, in chunk coordinates. </summary>
		public int ZCh;

		/// <summary>
		/// The y-position of the lowest chunk Section in the transmitted chunk, in chunk coordinates.
		/// </summary>
		public int YChMin;

		/// <summary>
		/// The y-position of the highest chunk Section in the transmitted chunk, in chunk coordinates.
		/// </summary>
		public int YChMax;
		public byte[] ChunkData;

		/// <summary>
		/// Whether to initialize the Chunk before applying the effect of the Packet51MapChunk.
		/// </summary>
		public bool IncludeInitialize;

		/// <summary>
		/// The length of the compressed chunk data byte array. </summary>
		private int TempLength;
		private int Field_48178_h;
		private static byte[] Temp = new byte[0];

		public Packet51MapChunk()
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
			XCh = par1NetworkStream.ReadInt32();
			ZCh = par1NetworkStream.ReadInt32();
			IncludeInitialize = par1NetworkStream.ReadBoolean();
			YChMin = par1NetworkStream.ReadInt16();
			YChMax = par1NetworkStream.ReadInt16();
			TempLength = par1NetworkStream.ReadInt32();
			Field_48178_h = par1NetworkStream.ReadInt32();

			if (Temp.Length < TempLength)
			{
				Temp = new byte[TempLength];
			}

			par1NetworkStream.Read(Temp, 0, TempLength);
			int i = 0;

			for (int j = 0; j < 16; j++)
			{
				i += YChMin >> j & 1;
			}

			int k = 12288 * i;

			if (IncludeInitialize)
			{
				k += 256;
			}

			ChunkData = new byte[k];
			Inflater inflater = new Inflater();
			inflater.SetInput(Temp, 0, TempLength);
            
			//try
			{
				inflater.Inflate(ChunkData);
			}/*
			catch (DataFormatException dataformatexception)
			{
				throw new IOException("Bad compressed data format");
			}
			finally
			{
				inflater.End();
			}*/
		}

		/// <summary>
		/// Abstract. Writes the raw packet data to the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void writePacketData(DataOutputStream par1DataOutputStream) throws IOException
        public override void WritePacketData(BinaryWriter par1DataOutputStream)
		{
			par1DataOutputStream.Write(XCh);
			par1DataOutputStream.Write(ZCh);
			par1DataOutputStream.Write(IncludeInitialize);
			par1DataOutputStream.Write((short)(YChMin & 0xffff));
			par1DataOutputStream.Write((short)(YChMax & 0xffff));
			par1DataOutputStream.Write(TempLength);
			par1DataOutputStream.Write(Field_48178_h);
			par1DataOutputStream.Write(ChunkData, 0, TempLength);
		}

		/// <summary>
		/// Passes this Packet on to the NetHandler for processing.
		/// </summary>
		public override void ProcessPacket(NetHandler par1NetHandler)
		{
			par1NetHandler.Func_48487_a(this);
		}

		/// <summary>
		/// Abstract. Return the size of the packet (not counting the header).
		/// </summary>
		public override int GetPacketSize()
		{
			return 17 + TempLength;
		}
	}
}