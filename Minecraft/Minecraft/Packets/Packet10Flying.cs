using System.Net.Sockets;
using System.IO;

namespace net.minecraft.src
{
	public class Packet10Flying : Packet
	{
		/// <summary>
		/// The player's X position. </summary>
		public double XPosition;

		/// <summary>
		/// The player's Y position. </summary>
		public double YPosition;

		/// <summary>
		/// The player's Z position. </summary>
		public double ZPosition;

		/// <summary>
		/// The player's stance. (boundingBox.MinY) </summary>
		public double Stance;

		/// <summary>
		/// The player's yaw rotation. </summary>
		public float Yaw;

		/// <summary>
		/// The player's pitch rotation. </summary>
		public float Pitch;

		/// <summary>
		/// True if the client is on the ground. </summary>
		public bool OnGround;

		/// <summary>
		/// bool set to true if the player is moving. </summary>
		public bool Moving;

		/// <summary>
		/// bool set to true if the player is rotating. </summary>
		public bool Rotating;

		public Packet10Flying()
		{
		}

		public Packet10Flying(bool par1)
		{
			OnGround = par1;
		}

		/// <summary>
		/// Passes this Packet on to the NetHandler for processing.
		/// </summary>
		public override void ProcessPacket(NetHandler par1NetHandler)
		{
			par1NetHandler.HandleFlying(this);
		}

		/// <summary>
		/// Abstract. Reads the raw packet data from the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void readPacketData(BinaryReader par1NetworkStream) throws IOException
        public override void ReadPacketData(BinaryReader par1NetworkStream)
		{
			OnGround = par1NetworkStream.Read() != 0;
		}

		/// <summary>
		/// Abstract. Writes the raw packet data to the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void writePacketData(DataOutputStream par1DataOutputStream) throws IOException
        public override void WritePacketData(BinaryWriter par1DataOutputStream)
		{
			par1DataOutputStream.Write(OnGround ? 1 : 0);
		}

		/// <summary>
		/// Abstract. Return the size of the packet (not counting the header).
		/// </summary>
		public override int GetPacketSize()
		{
			return 1;
		}
	}
}