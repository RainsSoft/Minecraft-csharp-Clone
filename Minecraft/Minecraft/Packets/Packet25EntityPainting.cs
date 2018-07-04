using System.Net.Sockets;
using System.IO;

namespace net.minecraft.src
{
	public class Packet25EntityPainting : Packet
	{
		public int EntityId;
		public int XPosition;
		public int YPosition;
		public int ZPosition;
		public int Direction;
		public string Title;

		public Packet25EntityPainting()
		{
		}

		public Packet25EntityPainting(EntityPainting par1EntityPainting)
		{
			EntityId = par1EntityPainting.EntityId;
			XPosition = par1EntityPainting.XPosition;
			YPosition = par1EntityPainting.YPosition;
			ZPosition = par1EntityPainting.ZPosition;
			Direction = par1EntityPainting.Direction;
			Title = par1EntityPainting.Art.Title;
		}

		/// <summary>
		/// Abstract. Reads the raw packet data from the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void readPacketData(BinaryReader par1NetworkStream) throws IOException
        public override void ReadPacketData(BinaryReader par1NetworkStream)
		{
			EntityId = par1NetworkStream.ReadInt32();
			Title = ReadString(par1NetworkStream, Art.MaxArtTitleLength);
			XPosition = par1NetworkStream.ReadInt32();
			YPosition = par1NetworkStream.ReadInt32();
			ZPosition = par1NetworkStream.ReadInt32();
			Direction = par1NetworkStream.ReadInt32();
		}

		/// <summary>
		/// Abstract. Writes the raw packet data to the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void writePacketData(DataOutputStream par1DataOutputStream) throws IOException
        public override void WritePacketData(BinaryWriter par1DataOutputStream)
		{
			par1DataOutputStream.Write(EntityId);
			WriteString(Title, par1DataOutputStream);
			par1DataOutputStream.Write(XPosition);
			par1DataOutputStream.Write(YPosition);
			par1DataOutputStream.Write(ZPosition);
			par1DataOutputStream.Write(Direction);
		}

		/// <summary>
		/// Passes this Packet on to the NetHandler for processing.
		/// </summary>
		public override void ProcessPacket(NetHandler par1NetHandler)
		{
			par1NetHandler.HandleEntityPainting(this);
		}

		/// <summary>
		/// Abstract. Return the size of the packet (not counting the header).
		/// </summary>
		public override int GetPacketSize()
		{
			return 24;
		}
	}
}