using System;
using System.Text;

namespace net.minecraft.src
{
	public class StructureBoundingBox
	{
		/// <summary>
		/// The first x coordinate of a bounding box. </summary>
		public int MinX;

		/// <summary>
		/// The first y coordinate of a bounding box. </summary>
		public int MinY;

		/// <summary>
		/// The first z coordinate of a bounding box. </summary>
		public int MinZ;

		/// <summary>
		/// The second x coordinate of a bounding box. </summary>
		public int MaxX;

		/// <summary>
		/// The second y coordinate of a bounding box. </summary>
		public int MaxY;

		/// <summary>
		/// The second z coordinate of a bounding box. </summary>
		public int MaxZ;

		public StructureBoundingBox()
		{
		}

		/// <summary>
		/// 'returns a new StructureBoundingBox with MAX values'
		/// </summary>
		public static StructureBoundingBox GetNewBoundingBox()
		{
			return new StructureBoundingBox(0x7fffffff, 0x7fffffff, 0x7fffffff, 0x8000000, 0x8000000, 0x8000000);
		}

		/// <summary>
		/// 'used to project a possible new component Bounding Box - to check if it would cut anything already spawned'
		/// </summary>
		public static StructureBoundingBox GetComponentToAddBoundingBox(int par0, int par1, int par2, int par3, int par4, int par5, int par6, int par7, int par8, int par9)
		{
			switch (par9)
			{
				default:
					return new StructureBoundingBox(par0 + par3, par1 + par4, par2 + par5, ((par0 + par6) - 1) + par3, ((par1 + par7) - 1) + par4, ((par2 + par8) - 1) + par5);

				case 2:
					return new StructureBoundingBox(par0 + par3, par1 + par4, (par2 - par8) + 1 + par5, ((par0 + par6) - 1) + par3, ((par1 + par7) - 1) + par4, par2 + par5);

				case 0:
					return new StructureBoundingBox(par0 + par3, par1 + par4, par2 + par5, ((par0 + par6) - 1) + par3, ((par1 + par7) - 1) + par4, ((par2 + par8) - 1) + par5);

				case 1:
					return new StructureBoundingBox((par0 - par8) + 1 + par5, par1 + par4, par2 + par3, par0 + par5, ((par1 + par7) - 1) + par4, ((par2 + par6) - 1) + par3);

				case 3:
					return new StructureBoundingBox(par0 + par5, par1 + par4, par2 + par3, ((par0 + par8) - 1) + par5, ((par1 + par7) - 1) + par4, ((par2 + par6) - 1) + par3);
			}
		}

		public StructureBoundingBox(StructureBoundingBox par1StructureBoundingBox)
		{
			MinX = par1StructureBoundingBox.MinX;
			MinY = par1StructureBoundingBox.MinY;
			MinZ = par1StructureBoundingBox.MinZ;
			MaxX = par1StructureBoundingBox.MaxX;
			MaxY = par1StructureBoundingBox.MaxY;
			MaxZ = par1StructureBoundingBox.MaxZ;
		}

		public StructureBoundingBox(int par1, int par2, int par3, int par4, int par5, int par6)
		{
			MinX = par1;
			MinY = par2;
			MinZ = par3;
			MaxX = par4;
			MaxY = par5;
			MaxZ = par6;
		}

		public StructureBoundingBox(int par1, int par2, int par3, int par4)
		{
			MinX = par1;
			MinZ = par2;
			MaxX = par3;
			MaxZ = par4;
			MinY = 1;
			MaxY = 512;
		}

		/// <summary>
		/// Returns whether the given bounding box intersects with this one. Args: structureboundingbox
		/// </summary>
		public bool IntersectsWith(StructureBoundingBox par1StructureBoundingBox)
		{
			return MaxX >= par1StructureBoundingBox.MinX && MinX <= par1StructureBoundingBox.MaxX && MaxZ >= par1StructureBoundingBox.MinZ && MinZ <= par1StructureBoundingBox.MaxZ && MaxY >= par1StructureBoundingBox.MinY && MinY <= par1StructureBoundingBox.MaxY;
		}

		/// <summary>
		/// Discover if a coordinate is inside the bounding box area.
		/// </summary>
		public bool IntersectsWith(int par1, int par2, int par3, int par4)
		{
			return MaxX >= par1 && MinX <= par3 && MaxZ >= par2 && MinZ <= par4;
		}

		/// <summary>
		/// Expands a bounding box's dimensions to include the supplied bounding box.
		/// </summary>
		public void ExpandTo(StructureBoundingBox par1StructureBoundingBox)
		{
			MinX = Math.Min(MinX, par1StructureBoundingBox.MinX);
			MinY = Math.Min(MinY, par1StructureBoundingBox.MinY);
			MinZ = Math.Min(MinZ, par1StructureBoundingBox.MinZ);
			MaxX = Math.Max(MaxX, par1StructureBoundingBox.MaxX);
			MaxY = Math.Max(MaxY, par1StructureBoundingBox.MaxY);
			MaxZ = Math.Max(MaxZ, par1StructureBoundingBox.MaxZ);
		}

		/// <summary>
		/// Offsets the current bounding box by the specified coordinates. Args: x, y, z
		/// </summary>
		public void Offset(int par1, int par2, int par3)
		{
			MinX += par1;
			MinY += par2;
			MinZ += par3;
			MaxX += par1;
			MaxY += par2;
			MaxZ += par3;
		}

		/// <summary>
		/// Returns true if block is inside bounding box
		/// </summary>
		public bool IsVecInside(int par1, int par2, int par3)
		{
			return par1 >= MinX && par1 <= MaxX && par3 >= MinZ && par3 <= MaxZ && par2 >= MinY && par2 <= MaxY;
		}

		/// <summary>
		/// Returns width of a bounding box
		/// </summary>
		public int GetXSize()
		{
			return (MaxX - MinX) + 1;
		}

		/// <summary>
		/// Returns height of a bounding box
		/// </summary>
		public int GetYSize()
		{
			return (MaxY - MinY) + 1;
		}

		/// <summary>
		/// Returns length of a bounding box
		/// </summary>
		public int GetZSize()
		{
			return (MaxZ - MinZ) + 1;
		}

		public int GetCenterX()
		{
			return MinX + ((MaxX - MinX) + 1) / 2;
		}

		public int GetCenterY()
		{
			return MinY + ((MaxY - MinY) + 1) / 2;
		}

		public int GetCenterZ()
		{
			return MinZ + ((MaxZ - MinZ) + 1) / 2;
		}

		public override string ToString()
		{
			return (new StringBuilder()).Append("(").Append(MinX).Append(", ").Append(MinY).Append(", ").Append(MinZ).Append("; ").Append(MaxX).Append(", ").Append(MaxY).Append(", ").Append(MaxZ).Append(")").ToString();
		}
	}
}