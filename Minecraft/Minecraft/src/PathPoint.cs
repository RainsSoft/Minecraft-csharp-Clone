using System.Text;

namespace net.minecraft.src
{
	public class PathPoint
	{
		/// <summary>
		/// The x coordinate of this point </summary>
		public readonly int XCoord;

		/// <summary>
		/// The y coordinate of this point </summary>
		public readonly int YCoord;

		/// <summary>
		/// The z coordinate of this point </summary>
		public readonly int ZCoord;

		/// <summary>
		/// A hash of the coordinates used to identify this point </summary>
		private readonly int Hash;

		/// <summary>
		/// The index of this point in its assigned path </summary>
		public int Index;

		/// <summary>
		/// The distance along the path to this point </summary>
		public float TotalPathDistance;

		/// <summary>
		/// The linear distance to the next point </summary>
		public float DistanceToNext;

		/// <summary>
		/// The distance to the target </summary>
		public float DistanceToTarget;

		/// <summary>
		/// The point preceding this in its assigned path </summary>
		public PathPoint Previous;

		/// <summary>
		/// Indicates this is the origin </summary>
		public bool IsFirst;

		public PathPoint(int par1, int par2, int par3)
		{
			Index = -1;
			IsFirst = false;
			XCoord = par1;
			YCoord = par2;
			ZCoord = par3;
			Hash = MakeHash(par1, par2, par3);
		}

		public static int MakeHash(int par0, int par1, int par2)
		{
			return par1 & 0xff | (par0 & 0x7fff) << 8 | (par2 & 0x7fff) << 24 | (par0 >= 0 ? 0 : 0x8000000) | (par2 >= 0 ? 0 : 0x8000);
		}

		/// <summary>
		/// Returns the linear distance to another path point
		/// </summary>
		public virtual float DistanceTo(PathPoint par1PathPoint)
		{
			float f = par1PathPoint.XCoord - XCoord;
			float f1 = par1PathPoint.YCoord - YCoord;
			float f2 = par1PathPoint.ZCoord - ZCoord;
			return MathHelper2.Sqrt_float(f * f + f1 * f1 + f2 * f2);
		}

		public virtual bool Equals(object par1Obj)
		{
			if (par1Obj is PathPoint)
			{
				PathPoint pathpoint = (PathPoint)par1Obj;
				return Hash == pathpoint.Hash && XCoord == pathpoint.XCoord && YCoord == pathpoint.YCoord && ZCoord == pathpoint.ZCoord;
			}
			else
			{
				return false;
			}
		}

		public virtual int GetHashCode()
		{
			return Hash;
		}

		/// <summary>
		/// Returns true if this point has already been assigned to a path
		/// </summary>
		public virtual bool IsAssigned()
		{
			return Index >= 0;
		}

		public override string ToString()
		{
			return (new StringBuilder()).Append(XCoord).Append(", ").Append(YCoord).Append(", ").Append(ZCoord).ToString();
		}
	}
}