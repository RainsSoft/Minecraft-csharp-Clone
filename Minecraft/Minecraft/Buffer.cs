using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace net.minecraft.src
{
    public struct Buffer<T> where T : struct
    {
        int position;

        public int Position
        {
            get { return position; }
            set
            {
                if (value > Length)
                    throw new ArgumentOutOfRangeException();

                position = value;
            }
        }

        public int Length { get { return Data.Length; } }

        public int Remaining { get { return Length - Position; } }

        public T[] Data;

        public Buffer(int capacity)
        {
            Data = new T[capacity];
            position = 0;
        }

        public T Get(int index)
        {
            return Data[index];
        }

        public void Put(T value)
        {
            if (Data.Length > Position)
            {
                Data[Position] = value;
            }
            else
            {
                Data[Position] = value;
            }

            Position++;
        }

        public void Put(T[] values, int offset, int length)
        {
            if (length > Remaining)
                throw new ArgumentOutOfRangeException("length");

            for (int i = offset; i < offset + length; i++)
                Put(values[i]);
        }

        public void Put(T[] array)
        {
            Put(array, 0, array.Length);
        }

        public void Flip()
        {
            Data.Reverse();
        }

        public void Limit(int length)
        {
            Array.Resize<T>(ref Data, length);
        }

        public void Clear()
        {
            for (int i = 0; i < Data.Length; i++)
            {
                Data[i] = default(T);
            }

            Position = 0;
        }

        public static implicit operator T[](Buffer<T> buffer)
        {
            return buffer.Data;
        }
    }
}
