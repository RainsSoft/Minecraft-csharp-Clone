using System.Collections.Generic;
using System.Text;

namespace net.minecraft.src
{
	public class Vec3D
	{
		/// <summary>
		/// ArrayList of all created vectors </summary>
        private static List<Vec3D> VectorList = new List<Vec3D>();

		/// <summary>
		/// Next empty index in the vectorList. We don't ever seem to remove vectors from the list, however.
		/// </summary>
		private static int NextVector = 0;

		/// <summary>
		/// X coordinate of Vec3D </summary>
		public double XCoord;

		/// <summary>
		/// Y coordinate of Vec3D </summary>
		public double YCoord;

		/// <summary>
		/// Z coordinate of Vec3D </summary>
		public double ZCoord;

		/// <summary>
		/// Static method for creating a new Vec3D given the three x,y,z values. This is only called from the other static
		/// method which creates and places it in the list.
		/// </summary>
		public static Vec3D CreateVectorHelper(double par0, double par2, double par4)
		{
			return new Vec3D(par0, par2, par4);
		}

		/// <summary>
		/// Clears the vector list.
		/// </summary>
		public static void ClearVectorList()
		{
			VectorList.Clear();
			NextVector = 0;
		}

		/// <summary>
		/// Initializes the next empty vector slot in the list to 0.
		/// </summary>
		public static void Initialize()
		{
			NextVector = 0;
		}

		/// <summary>
		/// Static method to create a new vector in the vector list and return it.
		/// </summary>
		public static Vec3D CreateVector(double par0, double par2, double par4)
		{
			if (NextVector >= VectorList.Count)
			{
				VectorList.Add(CreateVectorHelper(0.0F, 0.0F, 0.0F));
			}

			return VectorList[NextVector++].SetComponents(par0, par2, par4);
		}

		private Vec3D(double par1, double par3, double par5)
		{
			if (par1 == -0D)
			{
				par1 = 0.0F;
			}

			if (par3 == -0D)
			{
				par3 = 0.0F;
			}

			if (par5 == -0D)
			{
				par5 = 0.0F;
			}

			XCoord = par1;
			YCoord = par3;
			ZCoord = par5;
		}

		/// <summary>
		/// Sets the x,y,z components of the vector as specified.
		/// </summary>
		private Vec3D SetComponents(double par1, double par3, double par5)
		{
			XCoord = par1;
			YCoord = par3;
			ZCoord = par5;
			return this;
		}

		/// <summary>
		/// Returns a new vector with the result of the specified vector minus this.
		/// </summary>
		public virtual Vec3D Subtract(Vec3D par1Vec3D)
		{
			return CreateVector(par1Vec3D.XCoord - XCoord, par1Vec3D.YCoord - YCoord, par1Vec3D.ZCoord - ZCoord);
		}

		/// <summary>
		/// Normalizes the vector to a length of 1 (except if it is the zero vector)
		/// </summary>
		public virtual Vec3D Normalize()
		{
			double d = MathHelper2.Sqrt_double(XCoord * XCoord + YCoord * YCoord + ZCoord * ZCoord);

			if (d < 0.0001D)
			{
				return CreateVector(0.0F, 0.0F, 0.0F);
			}
			else
			{
				return CreateVector(XCoord / d, YCoord / d, ZCoord / d);
			}
		}

		public virtual double DotProduct(Vec3D par1Vec3D)
		{
			return XCoord * par1Vec3D.XCoord + YCoord * par1Vec3D.YCoord + ZCoord * par1Vec3D.ZCoord;
		}

		/// <summary>
		/// Returns a new vector with the result of this vector x the specified vector.
		/// </summary>
		public virtual Vec3D CrossProduct(Vec3D par1Vec3D)
		{
			return CreateVector(YCoord * par1Vec3D.ZCoord - ZCoord * par1Vec3D.YCoord, ZCoord * par1Vec3D.XCoord - XCoord * par1Vec3D.ZCoord, XCoord * par1Vec3D.YCoord - YCoord * par1Vec3D.XCoord);
		}

		/// <summary>
		/// Adds the specified x,y,z vector components to this vector and returns the resulting vector. Does not change this
		/// vector.
		/// </summary>
		public virtual Vec3D AddVector(double par1, double par3, double par5)
		{
			return CreateVector(XCoord + par1, YCoord + par3, ZCoord + par5);
		}

		/// <summary>
		/// Euclidean distance between this and the specified vector, returned as double.
		/// </summary>
		public virtual double DistanceTo(Vec3D par1Vec3D)
		{
			double d = par1Vec3D.XCoord - XCoord;
			double d1 = par1Vec3D.YCoord - YCoord;
			double d2 = par1Vec3D.ZCoord - ZCoord;
			return (double)MathHelper2.Sqrt_double(d * d + d1 * d1 + d2 * d2);
		}

		/// <summary>
		/// The square of the Euclidean distance between this and the specified vector.
		/// </summary>
		public virtual double SquareDistanceTo(Vec3D par1Vec3D)
		{
			double d = par1Vec3D.XCoord - XCoord;
			double d1 = par1Vec3D.YCoord - YCoord;
			double d2 = par1Vec3D.ZCoord - ZCoord;
			return d * d + d1 * d1 + d2 * d2;
		}

		/// <summary>
		/// The square of the Euclidean distance between this and the vector of x,y,z components passed in.
		/// </summary>
		public virtual double SquareDistanceTo(double par1, double par3, double par5)
		{
			double d = par1 - XCoord;
			double d1 = par3 - YCoord;
			double d2 = par5 - ZCoord;
			return d * d + d1 * d1 + d2 * d2;
		}

		/// <summary>
		/// Returns the length of the vector.
		/// </summary>
		public virtual double LengthVector()
		{
			return (double)MathHelper2.Sqrt_double(XCoord * XCoord + YCoord * YCoord + ZCoord * ZCoord);
		}

		/// <summary>
		/// Returns a new vector with x value equal to the second parameter, along the line between this vector and the
		/// passed in vector, or null if not possible.
		/// </summary>
		public virtual Vec3D GetIntermediateWithXValue(Vec3D par1Vec3D, double par2)
		{
			double d = par1Vec3D.XCoord - XCoord;
			double d1 = par1Vec3D.YCoord - YCoord;
			double d2 = par1Vec3D.ZCoord - ZCoord;

	//JAVA TO C# CONVERTER TODO TASK: Octal literals cannot be represented in C#:
			if (d * d < 1.0000000116860974E-007D)
			{
				return null;
			}

			double d3 = (par2 - XCoord) / d;

			if (d3 < 0.0F || d3 > 1.0D)
			{
				return null;
			}
			else
			{
				return CreateVector(XCoord + d * d3, YCoord + d1 * d3, ZCoord + d2 * d3);
			}
		}

		/// <summary>
		/// Returns a new vector with y value equal to the second parameter, along the line between this vector and the
		/// passed in vector, or null if not possible.
		/// </summary>
		public virtual Vec3D GetIntermediateWithYValue(Vec3D par1Vec3D, double par2)
		{
			double d = par1Vec3D.XCoord - XCoord;
			double d1 = par1Vec3D.YCoord - YCoord;
			double d2 = par1Vec3D.ZCoord - ZCoord;

	//JAVA TO C# CONVERTER TODO TASK: Octal literals cannot be represented in C#:
			if (d1 * d1 < 1.0000000116860974E-007D)
			{
				return null;
			}

			double d3 = (par2 - YCoord) / d1;

			if (d3 < 0.0F || d3 > 1.0D)
			{
				return null;
			}
			else
			{
				return CreateVector(XCoord + d * d3, YCoord + d1 * d3, ZCoord + d2 * d3);
			}
		}

		/// <summary>
		/// Returns a new vector with z value equal to the second parameter, along the line between this vector and the
		/// passed in vector, or null if not possible.
		/// </summary>
		public virtual Vec3D GetIntermediateWithZValue(Vec3D par1Vec3D, double par2)
		{
			double d = par1Vec3D.XCoord - XCoord;
			double d1 = par1Vec3D.YCoord - YCoord;
			double d2 = par1Vec3D.ZCoord - ZCoord;

	//JAVA TO C# CONVERTER TODO TASK: Octal literals cannot be represented in C#:
			if (d2 * d2 < 1.0000000116860974E-007D)
			{
				return null;
			}

			double d3 = (par2 - ZCoord) / d2;

			if (d3 < 0.0F || d3 > 1.0D)
			{
				return null;
			}
			else
			{
				return CreateVector(XCoord + d * d3, YCoord + d1 * d3, ZCoord + d2 * d3);
			}
		}

		public override string ToString()
		{
			return (new StringBuilder()).Append("(").Append(XCoord).Append(", ").Append(YCoord).Append(", ").Append(ZCoord).Append(")").ToString();
		}

		/// <summary>
		/// Rotates the vector around the x axis by the specified angle.
		/// </summary>
		public virtual void RotateAroundX(float par1)
		{
			float f = MathHelper2.Cos(par1);
			float f1 = MathHelper2.Sin(par1);
			double d = XCoord;
			double d1 = YCoord * (double)f + ZCoord * (double)f1;
			double d2 = ZCoord * (double)f - YCoord * (double)f1;
			XCoord = d;
			YCoord = d1;
			ZCoord = d2;
		}

		/// <summary>
		/// Rotates the vector around the y axis by the specified angle.
		/// </summary>
		public virtual void RotateAroundY(float par1)
		{
			float f = MathHelper2.Cos(par1);
			float f1 = MathHelper2.Sin(par1);
			double d = XCoord * (double)f + ZCoord * (double)f1;
			double d1 = YCoord;
			double d2 = ZCoord * (double)f - XCoord * (double)f1;
			XCoord = d;
			YCoord = d1;
			ZCoord = d2;
		}
	}
}