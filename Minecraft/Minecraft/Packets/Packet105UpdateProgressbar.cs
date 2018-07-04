using System.Net.Sockets;
using System.IO;

namespace net.minecraft.src
{
	public class Packet105UpdateProgressbar : Packet
	{
		/// <summary>
		/// The id of the window that the progress bar is in. </summary>
		public int WindowId;

		/// <summary>
		/// Which of the progress bars that should be updated. (For furnaces, 0 = progress arrow, 1 = fire icon)
		/// </summary>
		public int ProgressBar;

		/// <summary>
		/// The value of the progress bar. The maximum values vary depending on the progress bar. Presumably the values are
		/// specified as in-game ticks. Some progress bar values increase, while others decrease. For furnaces, 0 is empty,
		/// full progress arrow = about 180, full fire icon = about 250)
		/// </summary>
		public int ProgressBarValue;

		public Packet105UpdateProgressbar()
		{
		}

		/// <summary>
		/// Passes this Packet on to the NetHandler for processing.
		/// </summary>
		public override void ProcessPacket(NetHandler par1NetHandler)
		{
			par1NetHandler.HandleUpdateProgressbar(this);
		}

		/// <summary>
		/// Abstract. Reads the raw packet data from the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void readPacketData(BinaryReader par1NetworkStream) throws IOException
        public override void ReadPacketData(BinaryReader par1NetworkStream)
		{
			WindowId = par1NetworkStream.ReadByte();
			ProgressBar = par1NetworkStream.ReadInt16();
			ProgressBarValue = par1NetworkStream.ReadInt16();
		}

		/// <summary>
		/// Abstract. Writes the raw packet data to the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void writePacketData(DataOutputStream par1DataOutputStream) throws IOException
        public override void WritePacketData(BinaryWriter par1DataOutputStream)
		{
			par1DataOutputStream.Write(WindowId);
			par1DataOutputStream.Write(ProgressBar);
			par1DataOutputStream.Write(ProgressBarValue);
		}

		/// <summary>
		/// Abstract. Return the size of the packet (not counting the header).
		/// </summary>
		public override int GetPacketSize()
		{
			return 5;
		}
	}
}