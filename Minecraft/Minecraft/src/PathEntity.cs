namespace net.minecraft.src
{
	public class PathEntity
	{
		private readonly PathPoint[] Points;

		/// <summary>
		/// PathEntity Array Index the Entity is currently targeting </summary>
		private int CurrentPathIndex;

		/// <summary>
		/// The total length of the path </summary>
		private int PathLength;

		public PathEntity(PathPoint[] par1ArrayOfPathPoint)
		{
			Points = par1ArrayOfPathPoint;
			PathLength = par1ArrayOfPathPoint.Length;
		}

		/// <summary>
		/// Directs this path to the next point in its array
		/// </summary>
		public virtual void IncrementPathIndex()
		{
			CurrentPathIndex++;
		}

		/// <summary>
		/// Returns true if this path has reached the end
		/// </summary>
		public virtual bool IsFinished()
		{
			return CurrentPathIndex >= PathLength;
		}

		/// <summary>
		/// returns the last PathPoint of the Array
		/// </summary>
		public virtual PathPoint GetFinalPathPoint()
		{
			if (PathLength > 0)
			{
				return Points[PathLength - 1];
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// return the PathPoint located at the specified PathIndex, usually the current one
		/// </summary>
		public virtual PathPoint GetPathPointFromIndex(int par1)
		{
			return Points[par1];
		}

		public virtual int GetCurrentPathLength()
		{
			return PathLength;
		}

		public virtual void SetCurrentPathLength(int par1)
		{
			PathLength = par1;
		}

		public virtual int GetCurrentPathIndex()
		{
			return CurrentPathIndex;
		}

		public virtual void SetCurrentPathIndex(int par1)
		{
			CurrentPathIndex = par1;
		}

		/// <summary>
		/// Gets the vector of the PathPoint associated with the given index.
		/// </summary>
		public virtual Vec3D GetVectorFromIndex(Entity par1Entity, int par2)
		{
			double d = (double)Points[par2].XCoord + (double)(int)(par1Entity.Width + 1.0F) * 0.5D;
			double d1 = Points[par2].YCoord;
			double d2 = (double)Points[par2].ZCoord + (double)(int)(par1Entity.Width + 1.0F) * 0.5D;
			return Vec3D.CreateVector(d, d1, d2);
		}

		/// <summary>
		/// returns the current PathEntity target node as Vec3D
		/// </summary>
		public virtual Vec3D GetCurrentNodeVec3d(Entity par1Entity)
		{
			return GetVectorFromIndex(par1Entity, CurrentPathIndex);
		}

		public virtual bool Func_48647_a(PathEntity par1PathEntity)
		{
			if (par1PathEntity == null)
			{
				return false;
			}

			if (par1PathEntity.Points.Length != Points.Length)
			{
				return false;
			}

			for (int i = 0; i < Points.Length; i++)
			{
				if (Points[i].XCoord != par1PathEntity.Points[i].XCoord || Points[i].YCoord != par1PathEntity.Points[i].YCoord || Points[i].ZCoord != par1PathEntity.Points[i].ZCoord)
				{
					return false;
				}
			}

			return true;
		}

		public virtual bool Func_48639_a(Vec3D par1Vec3D)
		{
			PathPoint pathpoint = GetFinalPathPoint();

			if (pathpoint == null)
			{
				return false;
			}
			else
			{
				return pathpoint.XCoord == (int)par1Vec3D.XCoord && pathpoint.ZCoord == (int)par1Vec3D.ZCoord;
			}
		}
	}
}