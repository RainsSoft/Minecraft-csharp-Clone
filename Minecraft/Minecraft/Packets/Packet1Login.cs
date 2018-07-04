using System.Net.Sockets;
using System.IO;

namespace net.minecraft.src
{
	public class Packet1Login : Packet
	{
		/// <summary>
		/// The protocol version in use. Current version is 2. </summary>
		public int ProtocolVersion;

		/// <summary>
		/// The name of the user attempting to login. </summary>
		public string Username;
		public WorldType TerrainType;

		/// <summary>
		/// 0 for survival, 1 for creative </summary>
		public int ServerMode;
		public int Field_48170_e;

		/// <summary>
		/// The difficulty setting byte. </summary>
		public byte DifficultySetting;

		/// <summary>
		/// Defaults to 128 </summary>
		public byte WorldHeight;

		/// <summary>
		/// The maximum players. </summary>
		public byte MaxPlayers;

		public Packet1Login()
		{
		}

		public Packet1Login(string par1Str, int par2)
		{
			Username = par1Str;
			ProtocolVersion = par2;
		}

		/// <summary>
		/// Abstract. Reads the raw packet data from the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void readPacketData(BinaryReader par1NetworkStream) throws IOException
        public override void ReadPacketData(BinaryReader par1NetworkStream)
		{
			ProtocolVersion = par1NetworkStream.ReadInt32();
			Username = ReadString(par1NetworkStream, 16);
			string s = ReadString(par1NetworkStream, 16);
			TerrainType = WorldType.ParseWorldType(s);

			if (TerrainType == null)
			{
				TerrainType = WorldType.DEFAULT;
			}

			ServerMode = par1NetworkStream.ReadInt32();
			Field_48170_e = par1NetworkStream.ReadInt32();
			DifficultySetting = par1NetworkStream.ReadByte();
			WorldHeight = par1NetworkStream.ReadByte();
			MaxPlayers = par1NetworkStream.ReadByte();
		}

		/// <summary>
		/// Abstract. Writes the raw packet data to the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void writePacketData(DataOutputStream par1DataOutputStream) throws IOException
        public override void WritePacketData(BinaryWriter par1DataOutputStream)
		{
			par1DataOutputStream.Write(ProtocolVersion);
			WriteString(Username, par1DataOutputStream);

			if (TerrainType == null)
			{
				WriteString("", par1DataOutputStream);
			}
			else
			{
				WriteString(TerrainType.Func_48628_a(), par1DataOutputStream);
			}

			par1DataOutputStream.Write(ServerMode);
			par1DataOutputStream.Write(Field_48170_e);
			par1DataOutputStream.Write(DifficultySetting);
			par1DataOutputStream.Write(WorldHeight);
			par1DataOutputStream.Write(MaxPlayers);
		}

		/// <summary>
		/// Passes this Packet on to the NetHandler for processing.
		/// </summary>
		public override void ProcessPacket(NetHandler par1NetHandler)
		{
			par1NetHandler.HandleLogin(this);
		}

		/// <summary>
		/// Abstract. Return the size of the packet (not counting the header).
		/// </summary>
		public override int GetPacketSize()
		{
			int i = 0;

			if (TerrainType != null)
			{
				i = TerrainType.Func_48628_a().Length;
			}

			return 4 + Username.Length + 4 + 7 + 7 + i;
		}
	}
}