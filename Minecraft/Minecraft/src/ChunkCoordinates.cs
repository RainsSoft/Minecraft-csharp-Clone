using System;

namespace net.minecraft.src
{

	public class ChunkCoordinates : IComparable
	{
		public int PosX;

		/// <summary>
		/// the y coordinate </summary>
		public int PosY;

		/// <summary>
		/// the z coordinate </summary>
		public int PosZ;

		public ChunkCoordinates()
		{
		}

		public ChunkCoordinates(int par1, int par2, int par3)
		{
			PosX = par1;
			PosY = par2;
			PosZ = par3;
		}

		public ChunkCoordinates(ChunkCoordinates par1ChunkCoordinates)
		{
			PosX = par1ChunkCoordinates.PosX;
			PosY = par1ChunkCoordinates.PosY;
			PosZ = par1ChunkCoordinates.PosZ;
		}

		public virtual bool Equals(object par1Obj)
		{
			if (!(par1Obj is ChunkCoordinates))
			{
				return false;
			}
			else
			{
				ChunkCoordinates chunkcoordinates = (ChunkCoordinates)par1Obj;
				return PosX == chunkcoordinates.PosX && PosY == chunkcoordinates.PosY && PosZ == chunkcoordinates.PosZ;
			}
		}

		public virtual int GetHashCode()
		{
			return PosX + PosZ << 8 + PosY << 16;
		}

		/// <summary>
		/// Compare the coordinate with another coordinate
		/// </summary>
		public virtual int CompareChunkCoordinate(ChunkCoordinates par1ChunkCoordinates)
		{
			if (PosY == par1ChunkCoordinates.PosY)
			{
				if (PosZ == par1ChunkCoordinates.PosZ)
				{
					return PosX - par1ChunkCoordinates.PosX;
				}
				else
				{
					return PosZ - par1ChunkCoordinates.PosZ;
				}
			}
			else
			{
				return PosY - par1ChunkCoordinates.PosY;
			}
		}

		public virtual void Set(int par1, int par2, int par3)
		{
			PosX = par1;
			PosY = par2;
			PosZ = par3;
		}

		/// <summary>
		/// Returns the euclidean distance of the chunk coordinate to the x, y, z parameters passed.
		/// </summary>
		public virtual double GetEuclideanDistanceTo(int par1, int par2, int par3)
		{
			int i = PosX - par1;
			int j = PosY - par2;
			int k = PosZ - par3;
			return Math.Sqrt(i * i + j * j + k * k);
		}

		/// <summary>
		/// Returns the squared distance between this coordinates and the coordinates given as argument.
		/// </summary>
		public virtual float GetDistanceSquared(int par1, int par2, int par3)
		{
			int i = PosX - par1;
			int j = PosY - par2;
			int k = PosZ - par3;
			return (float)(i * i + j * j + k * k);
		}

		public virtual int CompareTo(object par1Obj)
		{
			return CompareChunkCoordinate((ChunkCoordinates)par1Obj);
		}
	}

}