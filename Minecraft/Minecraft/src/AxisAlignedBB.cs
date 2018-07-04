using System.Collections.Generic;
using System.Text;

namespace net.minecraft.src
{
	public class AxisAlignedBB
	{
		/// <summary>
		/// List of bounding boxes (not all necessarily being actively used) </summary>
        private static List<AxisAlignedBB> BoundingBoxes = new List<AxisAlignedBB>();

		/// <summary>
		/// Tracks how many bounding boxes are being used </summary>
		private static int NumBoundingBoxesInUse = 0;
		public float MinX;
        public float MinY;
        public float MinZ;
        public float MaxX;
        public float MaxY;
        public float MaxZ;

		/// <summary>
		/// Returns a bounding box with the specified bounds. Args: minX, minY, minZ, maxX, MaxY, maxZ
		/// </summary>
        public static AxisAlignedBB GetBoundingBox(float par0, float par2, float par4, float par6, float par8, float par10)
		{
			return new AxisAlignedBB(par0, par2, par4, par6, par8, par10);
		}

		public static void ClearBoundingBoxes()
		{
			BoundingBoxes.Clear();
			NumBoundingBoxesInUse = 0;
		}

		/// <summary>
		/// Sets the number of bounding boxes in use from the pool to 0 so they will be reused
		/// </summary>
		public static void ClearBoundingBoxPool()
		{
			NumBoundingBoxesInUse = 0;
		}

		/// <summary>
		/// Returns a bounding box with the specified bounds from the pool.  Args: minX, minY, minZ, maxX, MaxY, maxZ
		/// </summary>
        public static AxisAlignedBB GetBoundingBoxFromPool(float par0, float par2, float par4, float par6, float par8, float par10)
		{
			if (NumBoundingBoxesInUse >= BoundingBoxes.Count)
			{
				BoundingBoxes.Add(GetBoundingBox(0.0F, 0.0F, 0.0F, 0.0F, 0.0F, 0.0F));
			}

			return BoundingBoxes[NumBoundingBoxesInUse++].SetBounds(par0, par2, par4, par6, par8, par10);
		}

        private AxisAlignedBB(float par1, float par3, float par5, float par7, float par9, float par11)
		{
			MinX = par1;
			MinY = par3;
			MinZ = par5;
			MaxX = par7;
			MaxY = par9;
			MaxZ = par11;
		}

		/// <summary>
		/// Sets the bounds of the bounding box. Args: minX, minY, minZ, maxX, MaxY, maxZ
		/// </summary>
        public AxisAlignedBB SetBounds(float par1, float par3, float par5, float par7, float par9, float par11)
		{
			MinX = par1;
			MinY = par3;
			MinZ = par5;
			MaxX = par7;
			MaxY = par9;
			MaxZ = par11;
			return this;
		}

		/// <summary>
		/// Adds the coordinates to the bounding box extending it if the point lies outside the current ranges. Args: x, y, z
		/// </summary>
        public AxisAlignedBB AddCoord(float par1, float par3, float par5)
		{
            float d = MinX;
            float d1 = MinY;
            float d2 = MinZ;
            float d3 = MaxX;
            float d4 = MaxY;
            float d5 = MaxZ;

			if (par1 < 0.0F)
			{
				d += par1;
			}

			if (par1 > 0.0F)
			{
				d3 += par1;
			}

			if (par3 < 0.0F)
			{
				d1 += par3;
			}

			if (par3 > 0.0F)
			{
				d4 += par3;
			}

			if (par5 < 0.0F)
			{
				d2 += par5;
			}

			if (par5 > 0.0F)
			{
				d5 += par5;
			}

			return GetBoundingBoxFromPool(d, d1, d2, d3, d4, d5);
		}

		/// <summary>
		/// Returns a bounding box expanded by the specified vector (if negative numbers are given it will shrink). Args: x,
		/// y, z
		/// </summary>
        public AxisAlignedBB Expand(float par1, float par3, float par5)
		{
            float d = MinX - par1;
            float d1 = MinY - par3;
            float d2 = MinZ - par5;
            float d3 = MaxX + par1;
            float d4 = MaxY + par3;
            float d5 = MaxZ + par5;
			return GetBoundingBoxFromPool(d, d1, d2, d3, d4, d5);
		}

		/// <summary>
		/// Returns a bounding box offseted by the specified vector (if negative numbers are given it will shrink). Args: x,
		/// y, z
		/// </summary>
        public AxisAlignedBB GetOffsetBoundingBox(float par1, float par3, float par5)
		{
			return GetBoundingBoxFromPool(MinX + par1, MinY + par3, MinZ + par5, MaxX + par1, MaxY + par3, MaxZ + par5);
		}

		/// <summary>
		/// if instance and the argument bounding boxes overlap in the Y and Z dimensions, calculate the offset between them
		/// in the X dimension.  return var2 if the bounding boxes do not overlap or if var2 is closer to 0 then the
		/// calculated offset.  Otherwise return the calculated offset.
		/// </summary>
        public float CalculateXOffset(AxisAlignedBB par1AxisAlignedBB, float par2)
		{
			if (par1AxisAlignedBB.MaxY <= MinY || par1AxisAlignedBB.MinY >= MaxY)
			{
				return par2;
			}

			if (par1AxisAlignedBB.MaxZ <= MinZ || par1AxisAlignedBB.MinZ >= MaxZ)
			{
				return par2;
			}

			if (par2 > 0.0F && par1AxisAlignedBB.MaxX <= MinX)
			{
                float d = MinX - par1AxisAlignedBB.MaxX;

				if (d < par2)
				{
					par2 = d;
				}
			}

			if (par2 < 0.0F && par1AxisAlignedBB.MinX >= MaxX)
			{
                float d1 = MaxX - par1AxisAlignedBB.MinX;

				if (d1 > par2)
				{
					par2 = d1;
				}
			}

			return par2;
		}

		/// <summary>
		/// if instance and the argument bounding boxes overlap in the X and Z dimensions, calculate the offset between them
		/// in the Y dimension.  return var2 if the bounding boxes do not overlap or if var2 is closer to 0 then the
		/// calculated offset.  Otherwise return the calculated offset.
		/// </summary>
        public float CalculateYOffset(AxisAlignedBB par1AxisAlignedBB, float par2)
		{
			if (par1AxisAlignedBB.MaxX <= MinX || par1AxisAlignedBB.MinX >= MaxX)
			{
				return par2;
			}

			if (par1AxisAlignedBB.MaxZ <= MinZ || par1AxisAlignedBB.MinZ >= MaxZ)
			{
				return par2;
			}

			if (par2 > 0.0F && par1AxisAlignedBB.MaxY <= MinY)
			{
                float d = MinY - par1AxisAlignedBB.MaxY;

				if (d < par2)
				{
					par2 = d;
				}
			}

			if (par2 < 0.0F && par1AxisAlignedBB.MinY >= MaxY)
			{
                float d1 = MaxY - par1AxisAlignedBB.MinY;

				if (d1 > par2)
				{
					par2 = d1;
				}
			}

			return par2;
		}

		/// <summary>
		/// if instance and the argument bounding boxes overlap in the Y and X dimensions, calculate the offset between them
		/// in the Z dimension.  return var2 if the bounding boxes do not overlap or if var2 is closer to 0 then the
		/// calculated offset.  Otherwise return the calculated offset.
		/// </summary>
        public float CalculateZOffset(AxisAlignedBB par1AxisAlignedBB, float par2)
		{
			if (par1AxisAlignedBB.MaxX <= MinX || par1AxisAlignedBB.MinX >= MaxX)
			{
				return par2;
			}

			if (par1AxisAlignedBB.MaxY <= MinY || par1AxisAlignedBB.MinY >= MaxY)
			{
				return par2;
			}

			if (par2 > 0.0F && par1AxisAlignedBB.MaxZ <= MinZ)
			{
                float d = MinZ - par1AxisAlignedBB.MaxZ;

				if (d < par2)
				{
					par2 = d;
				}
			}

			if (par2 < 0.0F && par1AxisAlignedBB.MinZ >= MaxZ)
			{
                float d1 = MaxZ - par1AxisAlignedBB.MinZ;

				if (d1 > par2)
				{
					par2 = d1;
				}
			}

			return par2;
		}

		/// <summary>
		/// Returns whether the given bounding box intersects with this one. Args: axisAlignedBB
		/// </summary>
		public bool IntersectsWith(AxisAlignedBB par1AxisAlignedBB)
		{
			if (par1AxisAlignedBB.MaxX <= MinX || par1AxisAlignedBB.MinX >= MaxX)
			{
				return false;
			}

			if (par1AxisAlignedBB.MaxY <= MinY || par1AxisAlignedBB.MinY >= MaxY)
			{
				return false;
			}

			return par1AxisAlignedBB.MaxZ > MinZ && par1AxisAlignedBB.MinZ < MaxZ;
		}

		/// <summary>
		/// Offsets the current bounding box by the specified coordinates. Args: x, y, z
		/// </summary>
        public AxisAlignedBB Offset(float par1, float par3, float par5)
		{
			MinX += par1;
			MinY += par3;
			MinZ += par5;
			MaxX += par1;
			MaxY += par3;
			MaxZ += par5;
			return this;
		}

		/// <summary>
		/// Returns if the supplied Vec3D is completely inside the bounding box
		/// </summary>
		public bool IsVecInside(Vec3D par1Vec3D)
		{
			if (par1Vec3D.XCoord <= MinX || par1Vec3D.XCoord >= MaxX)
			{
				return false;
			}

			if (par1Vec3D.YCoord <= MinY || par1Vec3D.YCoord >= MaxY)
			{
				return false;
			}

			return par1Vec3D.ZCoord > MinZ && par1Vec3D.ZCoord < MaxZ;
		}

		/// <summary>
		/// Returns the average length of the edges of the bounding box.
		/// </summary>
        public float GetAverageEdgeLength()
		{
            float d = MaxX - MinX;
            float d1 = MaxY - MinY;
            float d2 = MaxZ - MinZ;
			return (d + d1 + d2) / 3F;
		}

		/// <summary>
		/// Returns a bounding box that is inset by the specified amounts
		/// </summary>
        public AxisAlignedBB Contract(float par1, float par3, float par5)
		{
            float d = MinX + par1;
            float d1 = MinY + par3;
            float d2 = MinZ + par5;
            float d3 = MaxX - par1;
            float d4 = MaxY - par3;
            float d5 = MaxZ - par5;
			return GetBoundingBoxFromPool(d, d1, d2, d3, d4, d5);
		}

		/// <summary>
		/// Returns a copy of the bounding box.
		/// </summary>
		public AxisAlignedBB Copy()
		{
			return GetBoundingBoxFromPool(MinX, MinY, MinZ, MaxX, MaxY, MaxZ);
		}

		public MovingObjectPosition CalculateIntercept(Vec3D par1Vec3D, Vec3D par2Vec3D)
		{
			Vec3D vec3d = par1Vec3D.GetIntermediateWithXValue(par2Vec3D, MinX);
			Vec3D vec3d1 = par1Vec3D.GetIntermediateWithXValue(par2Vec3D, MaxX);
			Vec3D vec3d2 = par1Vec3D.GetIntermediateWithYValue(par2Vec3D, MinY);
			Vec3D vec3d3 = par1Vec3D.GetIntermediateWithYValue(par2Vec3D, MaxY);
			Vec3D vec3d4 = par1Vec3D.GetIntermediateWithZValue(par2Vec3D, MinZ);
			Vec3D vec3d5 = par1Vec3D.GetIntermediateWithZValue(par2Vec3D, MaxZ);

			if (!IsVecInYZ(vec3d))
			{
				vec3d = null;
			}

			if (!IsVecInYZ(vec3d1))
			{
				vec3d1 = null;
			}

			if (!IsVecInXZ(vec3d2))
			{
				vec3d2 = null;
			}

			if (!IsVecInXZ(vec3d3))
			{
				vec3d3 = null;
			}

			if (!IsVecInXY(vec3d4))
			{
				vec3d4 = null;
			}

			if (!IsVecInXY(vec3d5))
			{
				vec3d5 = null;
			}

			Vec3D vec3d6 = null;

			if (vec3d != null && (vec3d6 == null || par1Vec3D.SquareDistanceTo(vec3d) < par1Vec3D.SquareDistanceTo(vec3d6)))
			{
				vec3d6 = vec3d;
			}

			if (vec3d1 != null && (vec3d6 == null || par1Vec3D.SquareDistanceTo(vec3d1) < par1Vec3D.SquareDistanceTo(vec3d6)))
			{
				vec3d6 = vec3d1;
			}

			if (vec3d2 != null && (vec3d6 == null || par1Vec3D.SquareDistanceTo(vec3d2) < par1Vec3D.SquareDistanceTo(vec3d6)))
			{
				vec3d6 = vec3d2;
			}

			if (vec3d3 != null && (vec3d6 == null || par1Vec3D.SquareDistanceTo(vec3d3) < par1Vec3D.SquareDistanceTo(vec3d6)))
			{
				vec3d6 = vec3d3;
			}

			if (vec3d4 != null && (vec3d6 == null || par1Vec3D.SquareDistanceTo(vec3d4) < par1Vec3D.SquareDistanceTo(vec3d6)))
			{
				vec3d6 = vec3d4;
			}

			if (vec3d5 != null && (vec3d6 == null || par1Vec3D.SquareDistanceTo(vec3d5) < par1Vec3D.SquareDistanceTo(vec3d6)))
			{
				vec3d6 = vec3d5;
			}

			if (vec3d6 == null)
			{
				return null;
			}

			sbyte byte0 = -1;

			if (vec3d6 == vec3d)
			{
				byte0 = 4;
			}

			if (vec3d6 == vec3d1)
			{
				byte0 = 5;
			}

			if (vec3d6 == vec3d2)
			{
				byte0 = 0;
			}

			if (vec3d6 == vec3d3)
			{
				byte0 = 1;
			}

			if (vec3d6 == vec3d4)
			{
				byte0 = 2;
			}

			if (vec3d6 == vec3d5)
			{
				byte0 = 3;
			}

			return new MovingObjectPosition(0, 0, 0, byte0, vec3d6);
		}

		/// <summary>
		/// Checks if the specified vector is within the YZ dimensions of the bounding box. Args: Vec3D
		/// </summary>
		private bool IsVecInYZ(Vec3D par1Vec3D)
		{
			if (par1Vec3D == null)
			{
				return false;
			}
			else
			{
				return par1Vec3D.YCoord >= MinY && par1Vec3D.YCoord <= MaxY && par1Vec3D.ZCoord >= MinZ && par1Vec3D.ZCoord <= MaxZ;
			}
		}

		/// <summary>
		/// Checks if the specified vector is within the XZ dimensions of the bounding box. Args: Vec3D
		/// </summary>
		private bool IsVecInXZ(Vec3D par1Vec3D)
		{
			if (par1Vec3D == null)
			{
				return false;
			}
			else
			{
				return par1Vec3D.XCoord >= MinX && par1Vec3D.XCoord <= MaxX && par1Vec3D.ZCoord >= MinZ && par1Vec3D.ZCoord <= MaxZ;
			}
		}

		/// <summary>
		/// Checks if the specified vector is within the XY dimensions of the bounding box. Args: Vec3D
		/// </summary>
		private bool IsVecInXY(Vec3D par1Vec3D)
		{
			if (par1Vec3D == null)
			{
				return false;
			}
			else
			{
				return par1Vec3D.XCoord >= MinX && par1Vec3D.XCoord <= MaxX && par1Vec3D.YCoord >= MinY && par1Vec3D.YCoord <= MaxY;
			}
		}

		/// <summary>
		/// Sets the bounding box to the same bounds as the bounding box passed in. Args: axisAlignedBB
		/// </summary>
		public void SetBB(AxisAlignedBB par1AxisAlignedBB)
		{
			MinX = par1AxisAlignedBB.MinX;
			MinY = par1AxisAlignedBB.MinY;
			MinZ = par1AxisAlignedBB.MinZ;
			MaxX = par1AxisAlignedBB.MaxX;
			MaxY = par1AxisAlignedBB.MaxY;
			MaxZ = par1AxisAlignedBB.MaxZ;
		}

		public string ToString()
		{
			return (new StringBuilder()).Append("box[").Append(MinX).Append(", ").Append(MinY).Append(", ").Append(MinZ).Append(" -> ").Append(MaxX).Append(", ").Append(MaxY).Append(", ").Append(MaxZ).Append("]").ToString();
		}
	}
}