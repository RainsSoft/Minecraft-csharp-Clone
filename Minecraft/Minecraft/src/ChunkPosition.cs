namespace net.minecraft.src
{

	public class ChunkPosition
	{
		/// <summary>
		/// The x coordinate of this ChunkPosition </summary>
		public readonly int x;

		/// <summary>
		/// The y coordinate of this ChunkPosition </summary>
		public readonly int y;

		/// <summary>
		/// The z coordinate of this ChunkPosition </summary>
		public readonly int z;

		public ChunkPosition(int par1, int par2, int par3)
		{
			x = par1;
			y = par2;
			z = par3;
		}

		public ChunkPosition(Vec3D par1Vec3D) : this(MathHelper2.Floor_double(par1Vec3D.XCoord), MathHelper2.Floor_double(par1Vec3D.YCoord), MathHelper2.Floor_double(par1Vec3D.ZCoord))
		{
		}

		public virtual bool Equals(object par1Obj)
		{
			if (par1Obj is ChunkPosition)
			{
				ChunkPosition chunkposition = (ChunkPosition)par1Obj;
				return chunkposition.x == x && chunkposition.y == y && chunkposition.z == z;
			}
			else
			{
				return false;
			}
		}

		public virtual int GetHashCode()
		{
			return x * 0x88f9fa + y * 0xef88b + z;
		}
	}

}