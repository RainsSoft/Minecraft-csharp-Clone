using System.Net.Sockets;
using System.IO;

namespace net.minecraft.src
{
	public class Packet9Respawn : Packet
	{
		public int RespawnDimension;

		/// <summary>
		/// 0 thru 3 for Peaceful, Easy, Normal, Hard. 1 is always sent c->s </summary>
		public int Difficulty;

		/// <summary>
		/// Defaults to 128 </summary>
		public int WorldHeight;

		/// <summary>
		/// 0 for survival, 1 for creative </summary>
		public int CreativeMode;
		public WorldType TerrainType;

		public Packet9Respawn()
		{
		}

		public Packet9Respawn(int par1, sbyte par2, WorldType par3WorldType, int par4, int par5)
		{
			RespawnDimension = par1;
			Difficulty = par2;
			WorldHeight = par4;
			CreativeMode = par5;
			TerrainType = par3WorldType;
		}

		/// <summary>
		/// Passes this Packet on to the NetHandler for processing.
		/// </summary>
		public override void ProcessPacket(NetHandler par1NetHandler)
		{
			par1NetHandler.HandleRespawn(this);
		}

		/// <summary>
		/// Abstract. Reads the raw packet data from the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void readPacketData(BinaryReader par1NetworkStream) throws IOException
        public override void ReadPacketData(BinaryReader par1NetworkStream)
		{
			RespawnDimension = par1NetworkStream.ReadInt32();
			Difficulty = par1NetworkStream.ReadByte();
			CreativeMode = par1NetworkStream.ReadByte();
			WorldHeight = par1NetworkStream.ReadInt16();
			string s = ReadString(par1NetworkStream, 16);
			TerrainType = WorldType.ParseWorldType(s);

			if (TerrainType == null)
			{
				TerrainType = WorldType.DEFAULT;
			}
		}

		/// <summary>
		/// Abstract. Writes the raw packet data to the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void writePacketData(DataOutputStream par1DataOutputStream) throws IOException
        public override void WritePacketData(BinaryWriter par1DataOutputStream)
		{
			par1DataOutputStream.Write(RespawnDimension);
			par1DataOutputStream.Write(Difficulty);
			par1DataOutputStream.Write(CreativeMode);
			par1DataOutputStream.Write(WorldHeight);
			WriteString(TerrainType.Func_48628_a(), par1DataOutputStream);
		}

		/// <summary>
		/// Abstract. Return the size of the packet (not counting the header).
		/// </summary>
		public override int GetPacketSize()
		{
			return 8 + TerrainType.Func_48628_a().Length;
		}
	}
}