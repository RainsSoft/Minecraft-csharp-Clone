using System;

namespace net.minecraft.src
{
	public class Path
	{
		private PathPoint[] PathPoints;

		/// <summary>
		/// The number of points in this path </summary>
		private int Count;

		public Path()
		{
			PathPoints = new PathPoint[1024];
			Count = 0;
		}

		/// <summary>
		/// Adds a point to the path
		/// </summary>
		public virtual PathPoint AddPoint(PathPoint par1PathPoint)
		{
			if (par1PathPoint.Index >= 0)
			{
				throw new InvalidOperationException("OW KNOWS!");
			}

			if (Count == PathPoints.Length)
			{
				PathPoint[] apathpoint = new PathPoint[Count << 1];
				Array.Copy(PathPoints, 0, apathpoint, 0, Count);
				PathPoints = apathpoint;
			}

			PathPoints[Count] = par1PathPoint;
			par1PathPoint.Index = Count;
			SortBack(Count++);
			return par1PathPoint;
		}

		/// <summary>
		/// Clears the path
		/// </summary>
		public virtual void ClearPath()
		{
			Count = 0;
		}

		/// <summary>
		/// Returns and removes the first point in the path
		/// </summary>
		public virtual PathPoint Dequeue()
		{
			PathPoint pathpoint = PathPoints[0];
			PathPoints[0] = PathPoints[--Count];
			PathPoints[Count] = null;

			if (Count > 0)
			{
				SortForward(0);
			}

			pathpoint.Index = -1;
			return pathpoint;
		}

		/// <summary>
		/// Changes the provided point's distance to target
		/// </summary>
		public virtual void ChangeDistance(PathPoint par1PathPoint, float par2)
		{
			float f = par1PathPoint.DistanceToTarget;
			par1PathPoint.DistanceToTarget = par2;

			if (par2 < f)
			{
				SortBack(par1PathPoint.Index);
			}
			else
			{
				SortForward(par1PathPoint.Index);
			}
		}

		/// <summary>
		/// Sorts a point to the left
		/// </summary>
		private void SortBack(int par1)
		{
			PathPoint pathpoint = PathPoints[par1];
			float f = pathpoint.DistanceToTarget;

			do
			{
				if (par1 <= 0)
				{
					break;
				}

				int i = par1 - 1 >> 1;
				PathPoint pathpoint1 = PathPoints[i];

				if (f >= pathpoint1.DistanceToTarget)
				{
					break;
				}

				PathPoints[par1] = pathpoint1;
				pathpoint1.Index = par1;
				par1 = i;
			}
			while (true);

			PathPoints[par1] = pathpoint;
			pathpoint.Index = par1;
		}

		/// <summary>
		/// Sorts a point to the right
		/// </summary>
		private void SortForward(int par1)
		{
			PathPoint pathpoint = PathPoints[par1];
			float f = pathpoint.DistanceToTarget;

			do
			{
				int i = 1 + (par1 << 1);
				int j = i + 1;

				if (i >= Count)
				{
					break;
				}

				PathPoint pathpoint1 = PathPoints[i];
				float f1 = pathpoint1.DistanceToTarget;
				PathPoint pathpoint2;
				float f2;

				if (j >= Count)
				{
					pathpoint2 = null;
					f2 = (1.0F / 0.0F);
				}
				else
				{
					pathpoint2 = PathPoints[j];
					f2 = pathpoint2.DistanceToTarget;
				}

				if (f1 < f2)
				{
					if (f1 >= f)
					{
						break;
					}

					PathPoints[par1] = pathpoint1;
					pathpoint1.Index = par1;
					par1 = i;
					continue;
				}

				if (f2 >= f)
				{
					break;
				}

				PathPoints[par1] = pathpoint2;
				pathpoint2.Index = par1;
				par1 = j;
			}
			while (true);

			PathPoints[par1] = pathpoint;
			pathpoint.Index = par1;
		}

		/// <summary>
		/// Returns true if this path Contains no points
		/// </summary>
		public virtual bool IsPathEmpty()
		{
			return Count == 0;
		}
	}
}