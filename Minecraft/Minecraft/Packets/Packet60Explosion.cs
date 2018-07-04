using System.Collections.Generic;
using System.Net.Sockets;
using System.IO;

namespace net.minecraft.src
{
	public class Packet60Explosion : Packet
	{
		public double ExplosionX;
		public double ExplosionY;
		public double ExplosionZ;
		public float ExplosionSize;
        public List<ChunkPosition> DestroyedBlockPositions;

		public Packet60Explosion()
		{
		}

		/// <summary>
		/// Abstract. Reads the raw packet data from the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void readPacketData(BinaryReader par1NetworkStream) throws IOException
		public override void ReadPacketData(BinaryReader par1NetworkStream)
		{
			ExplosionX = par1NetworkStream.ReadDouble();
			ExplosionY = par1NetworkStream.ReadDouble();
			ExplosionZ = par1NetworkStream.ReadDouble();
			ExplosionSize = par1NetworkStream.ReadSingle();
			int i = par1NetworkStream.ReadInt32();
            DestroyedBlockPositions = new List<ChunkPosition>();
			int j = (int)ExplosionX;
			int k = (int)ExplosionY;
			int l = (int)ExplosionZ;

			for (int i1 = 0; i1 < i; i1++)
			{
				int j1 = par1NetworkStream.ReadByte() + j;
				int k1 = par1NetworkStream.ReadByte() + k;
				int l1 = par1NetworkStream.ReadByte() + l;
				DestroyedBlockPositions.Add(new ChunkPosition(j1, k1, l1));
			}
		}

		/// <summary>
		/// Abstract. Writes the raw packet data to the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void writePacketData(DataOutputStream par1DataOutputStream) throws IOException
		public override void WritePacketData(BinaryWriter par1DataOutputStream)
		{
			par1DataOutputStream.Write(ExplosionX);
			par1DataOutputStream.Write(ExplosionY);
			par1DataOutputStream.Write(ExplosionZ);
			par1DataOutputStream.Write(ExplosionSize);
			par1DataOutputStream.Write(DestroyedBlockPositions.Count);
			int i = (int)ExplosionX;
			int j = (int)ExplosionY;
			int k = (int)ExplosionZ;
			int j1;

			for (IEnumerator<ChunkPosition> iterator = DestroyedBlockPositions.GetEnumerator(); iterator.MoveNext(); par1DataOutputStream.Write(j1))
			{
				ChunkPosition chunkposition = iterator.Current;
				int l = chunkposition.x - i;
				int i1 = chunkposition.y - j;
				j1 = chunkposition.z - k;
				par1DataOutputStream.Write(l);
				par1DataOutputStream.Write(i1);
			}
		}

		/// <summary>
		/// Passes this Packet on to the NetHandler for processing.
		/// </summary>
		public override void ProcessPacket(NetHandler par1NetHandler)
		{
			par1NetHandler.HandleExplosion(this);
		}

		/// <summary>
		/// Abstract. Return the size of the packet (not counting the header).
		/// </summary>
		public override int GetPacketSize()
		{
			return 32 + DestroyedBlockPositions.Count * 3;
		}
	}
}