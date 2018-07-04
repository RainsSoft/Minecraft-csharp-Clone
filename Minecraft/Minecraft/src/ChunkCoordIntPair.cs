using System.Text;

namespace net.minecraft.src
{

	public class ChunkCoordIntPair
	{
		/// <summary>
		/// The X position of this Chunk Coordinate Pair </summary>
		public readonly int ChunkXPos;

		/// <summary>
		/// The Z position of this Chunk Coordinate Pair </summary>
		public readonly int ChunkZPos;

		public ChunkCoordIntPair(int par1, int par2)
		{
			ChunkXPos = par1;
			ChunkZPos = par2;
		}

		/// <summary>
		/// converts a chunk coordinate pair to an integer (suitable for hashing)
		/// </summary>
		public static long ChunkXZ2Int(int par0, int par1)
		{
			long l = par0;
			long l1 = par1;
			return l & 0xffffffffL | (l1 & 0xffffffffL) << 32;
		}

		public virtual int GetHashCode()
		{
			long l = ChunkXZ2Int(ChunkXPos, ChunkZPos);
			int i = (int)l;
			int j = (int)(l >> 32);
			return i ^ j;
		}

		public virtual bool Equals(object par1Obj)
		{
			ChunkCoordIntPair chunkcoordintpair = (ChunkCoordIntPair)par1Obj;
			return chunkcoordintpair.ChunkXPos == ChunkXPos && chunkcoordintpair.ChunkZPos == ChunkZPos;
		}

		public virtual int GetCenterXPos()
		{
			return (ChunkXPos << 4) + 8;
		}

		public virtual int GetCenterZPos()
		{
			return (ChunkZPos << 4) + 8;
		}

		public virtual ChunkPosition GetChunkPosition(int par1)
		{
			return new ChunkPosition(GetCenterXPos(), par1, GetCenterZPos());
		}

		public virtual string ToString()
		{
			return (new StringBuilder()).Append("[").Append(ChunkXPos).Append(", ").Append(ChunkZPos).Append("]").ToString();
		}
	}

}