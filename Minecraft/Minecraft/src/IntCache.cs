using System.Collections.Generic;

namespace net.minecraft.src
{
	public class IntCache
	{
		private static int IntCacheSize = 256;

		/// <summary>
		/// A list of pre-allocated int[256] arrays that are currently unused and can be returned by getIntCache()
		/// </summary>
        private static List<int[]> FreeSmallArrays = new List<int[]>();

		/// <summary>
		/// A list of pre-allocated int[256] arrays that were previously returned by getIntCache() and which will not be re-
		/// used again until resetIntCache() is called.
		/// </summary>
        private static List<int[]> InUseSmallArrays = new List<int[]>();

		/// <summary>
		/// A list of pre-allocated int[cacheSize] arrays that are currently unused and can be returned by getIntCache()
		/// </summary>
        private static List<int[]> FreeLargeArrays = new List<int[]>();

		/// <summary>
		/// A list of pre-allocated int[cacheSize] arrays that were previously returned by getIntCache() and which will not
		/// be re-used again until resetIntCache() is called.
		/// </summary>
        private static List<int[]> InUseLargeArrays = new List<int[]>();

		public IntCache()
		{
		}

		public static int[] GetIntCache(int par0)
		{
			if (par0 <= 256)
			{
				if (FreeSmallArrays.Count == 0)
				{
					int[] ai = new int[256];
					InUseSmallArrays.Add(ai);
					return ai;
				}
				else
				{
                    int[] ai1 = FreeSmallArrays[FreeSmallArrays.Count - 1];
                    FreeSmallArrays.RemoveAt(FreeSmallArrays.Count - 1);
					InUseSmallArrays.Add(ai1);
					return ai1;
				}
			}

			if (par0 > IntCacheSize)
			{
				IntCacheSize = par0;
				FreeLargeArrays.Clear();
				InUseLargeArrays.Clear();
				int[] ai2 = new int[IntCacheSize];
				InUseLargeArrays.Add(ai2);
				return ai2;
			}

			if (FreeLargeArrays.Count == 0)
			{
				int[] ai3 = new int[IntCacheSize];
				InUseLargeArrays.Add(ai3);
				return ai3;
			}
			else
			{
				int[] ai4 = FreeLargeArrays[FreeLargeArrays.Count - 1];
                FreeLargeArrays.RemoveAt(FreeLargeArrays.Count - 1);
				InUseLargeArrays.Add(ai4);
				return ai4;
			}
		}

		/// <summary>
		/// Mark all pre-allocated arrays as available for re-use by moving them to the appropriate free lists.
		/// </summary>
		public static void ResetIntCache()
		{
			if (FreeLargeArrays.Count > 0)
			{
				FreeLargeArrays.RemoveAt(FreeLargeArrays.Count - 1);
			}

			if (FreeSmallArrays.Count > 0)
			{
				FreeSmallArrays.RemoveAt(FreeSmallArrays.Count - 1);
			}

			FreeLargeArrays.AddRange(InUseLargeArrays);
			FreeSmallArrays.AddRange(InUseSmallArrays);
			InUseLargeArrays.Clear();
			InUseSmallArrays.Clear();
		}
	}
}