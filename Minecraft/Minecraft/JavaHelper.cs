using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace net.minecraft.src
{
    public static class JavaHelper
    {
        public static T[][] ReturnRectangularArray<T>(int Size1, int Size2)
        {
            T[][] Array = new T[Size1][];
            for (int Array1 = 0; Array1 < Size1; Array1++)
            {
                Array[Array1] = new T[Size2];
            }
            return Array;
        }

        public static void FillArray<T>(T[] array, T value)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            for (var i = 0; i < array.Length; i++)
            {
                array[i] = value;
            }
        }

        public static void FillArray<T>(T[] array, int fromIndex, int toIndex, T value)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            if (fromIndex < 0 || fromIndex > toIndex)
            {
                throw new ArgumentOutOfRangeException("fromIndex");
            }
            if (toIndex > array.Length)
            {
                throw new ArgumentOutOfRangeException("toIndex");
            }
            for (var i = fromIndex; i < toIndex; i++)
            {
                array[i] = value;
            }
        }

        private static readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static long CurrentTimeMillis()
        {
            return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
        }

        public static long NanoTime()
        {
            return CurrentTimeMillis() / 1000L;
        }

        public static bool NextBool(this Random rand)
        {
            return rand.Next(1) == 0;
        }

        public static float NextFloat(this Random rand)
        {
            return (float)rand.NextDouble();
        }

        public static float NextGaussian(this Random rand)
        {
            float v1, v2, s;
            do
            {
                v1 = 2 * rand.NextFloat() - 1;   // between -1.0 and 1.0
                v2 = 2 * rand.NextFloat() - 1;   // between -1.0 and 1.0
                s = v1 * v1 + v2 * v2;
            } while (s >= 1 || s == 0);
            float multiplier = (float)Math.Sqrt(-2 * Math.Log(s) / s);
            return v1 * multiplier;
        }

        public static void SetSeed(this Random rand, int seed)
        {
            rand = new Random(seed);
        }
    }
}
