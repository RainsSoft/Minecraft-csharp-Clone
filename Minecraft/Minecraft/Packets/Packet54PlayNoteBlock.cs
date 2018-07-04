using System.IO;
using System.Net.Sockets;

namespace net.minecraft.src
{
	public class Packet54PlayNoteBlock : Packet
	{
		public int XLocation;
		public int YLocation;
		public int ZLocation;

		/// <summary>
		/// 1=Double Bass, 2=Snare Drum, 3=Clicks / Sticks, 4=Bass Drum, 5=Harp </summary>
		public int InstrumentType;

		/// <summary>
		/// The pitch of the note (between 0-24 inclusive where 0 is the lowest and 24 is the highest).
		/// </summary>
		public int Pitch;

		public Packet54PlayNoteBlock()
		{
		}

		/// <summary>
		/// Abstract. Reads the raw packet data from the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void readPacketData(BinaryReader par1NetworkStream) throws IOException
		public override void ReadPacketData(BinaryReader par1NetworkStream)
		{
			XLocation = par1NetworkStream.ReadInt32();
			YLocation = par1NetworkStream.ReadInt16();
			ZLocation = par1NetworkStream.ReadInt32();
			InstrumentType = par1NetworkStream.Read();
			Pitch = par1NetworkStream.Read();
		}

		/// <summary>
		/// Abstract. Writes the raw packet data to the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void writePacketData(BinaryWriterStream par1BinaryWriterStream) throws IOException
		public override void WritePacketData(BinaryWriter par1OutputStream)
		{
            par1OutputStream.Write(XLocation);
            par1OutputStream.Write(YLocation);
            par1OutputStream.Write(ZLocation);
            par1OutputStream.Write(InstrumentType);
            par1OutputStream.Write(Pitch);
		}

		/// <summary>
		/// Passes this Packet on to the NetHandler for processing.
		/// </summary>
		public override void ProcessPacket(NetHandler par1NetHandler)
		{
			par1NetHandler.HandlePlayNoteBlock(this);
		}

		/// <summary>
		/// Abstract. Return the size of the packet (not counting the header).
		/// </summary>
		public override int GetPacketSize()
		{
			return 12;
		}
	}
}