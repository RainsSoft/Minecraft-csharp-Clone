using System;

namespace net.minecraft.src
{
	public class MathHelper2
	{
		private static float[] SIN_TABLE;

		public MathHelper2()
		{
		}

		/// <summary>
		/// sin looked up in a table
		/// </summary>
		public static float Sin(float par0)
		{
			return SIN_TABLE[(int)(par0 * 10430.38F) & 0xffff];
		}

		/// <summary>
		/// cos looked up in the sin table with the appropriate offset
		/// </summary>
		public static float Cos(float par0)
		{
			return SIN_TABLE[(int)(par0 * 10430.38F + 16384F) & 0xffff];
		}

		public static float Sqrt_float(float par0)
		{
			return (float)Math.Sqrt(par0);
		}

		public static float Sqrt_double(double par0)
		{
			return (float)Math.Sqrt(par0);
		}

		/// <summary>
		/// Returns the greatest integer less than or equal to the float argument
		/// </summary>
		public static int Floor_float(float par0)
		{
			int i = (int)par0;
			return par0 >= (float)i ? i : i - 1;
		}

		public static int Func_40346_b(double par0)
		{
			return (int)(par0 + 1024D) - 1024;
		}

		/// <summary>
		/// Returns the greatest integer less than or equal to the double argument
		/// </summary>
		public static int Floor_double(double par0)
		{
			int i = (int)par0;
			return par0 >= (double)i ? i : i - 1;
		}

		/// <summary>
		/// Long version of floor_double
		/// </summary>
		public static long Floor_double_long(double par0)
		{
			long l = (long)par0;
			return par0 >= (double)l ? l : l - 1L;
		}

		public static float Abs(float par0)
		{
			return par0 < 0.0F ? - par0 : par0;
		}

		/// <summary>
		/// Returns the value of the first parameter, clamped to be within the lower and upper limits given by the second and
		/// third parameters.
		/// </summary>
		public static int Clamp_int(int par0, int par1, int par2)
		{
			if (par0 < par1)
			{
				return par1;
			}

			if (par0 > par2)
			{
				return par2;
			}
			else
			{
				return par0;
			}
		}

		/// <summary>
		/// Returns the value of the first parameter, clamped to be within the lower and upper limits given by the second and
		/// third parameters
		/// </summary>
		public static float Clamp_float(float par0, float par1, float par2)
		{
			if (par0 < par1)
			{
				return par1;
			}

			if (par0 > par2)
			{
				return par2;
			}
			else
			{
				return par0;
			}
		}

		/// <summary>
		/// Maximum of the absolute value of two numbers.
		/// </summary>
		public static double Abs_max(double par0, double par2)
		{
			if (par0 < 0.0F)
			{
				par0 = -par0;
			}

			if (par2 < 0.0F)
			{
				par2 = -par2;
			}

			return par0 <= par2 ? par2 : par0;
		}

		/// <summary>
		/// Buckets an integer with specifed bucket sizes.  Args: i, bucketSize
		/// </summary>
		public static int BucketInt(int par0, int par1)
		{
			if (par0 < 0)
			{
				return -((-par0 - 1) / par1) - 1;
			}
			else
			{
				return par0 / par1;
			}
		}

		/// <summary>
		/// Tests if a string is null or of length zero
		/// </summary>
		public static bool StringNullOrLengthZero(string par0Str)
		{
			return par0Str == null || par0Str.Length == 0;
		}

		public static int GetRandomIntegerInRange(Random par0Random, int par1, int par2)
		{
			if (par1 >= par2)
			{
				return par1;
			}
			else
			{
				return par0Random.Next((par2 - par1) + 1) + par1;
			}
		}

		static MathHelper2()
		{
			SIN_TABLE = new float[0x10000];

			for (int i = 0; i < 0x10000; i++)
			{
				SIN_TABLE[i] = (float)Math.Sin(((double)i * Math.PI * 2D) / 65536D);
			}
		}
	}
}