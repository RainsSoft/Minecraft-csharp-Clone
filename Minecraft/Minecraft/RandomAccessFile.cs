using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace net.minecraft.src
{
    public enum FileOptions { Read, Write, ReadWrite }

    public class RandomAccessFile
    {
        FileStream stream;

        public long Length { get { return stream.Length; } }

        public RandomAccessFile(string path, FileOptions options)
        {
            stream = new FileStream(path, FileMode.OpenOrCreate);
        }

        public void Seek(long index)
        {
            stream.Seek(index, SeekOrigin.Begin);
        }

        public void Read(byte[] array, int offset, int count)
        {
            stream.Read(array, offset, count);
        }

        public void Read(byte[] array)
        {
            Read(array, 0, array.Length);
        }

        public byte ReadByte()
        {
            return (byte)stream.ReadByte();
        }

        public int ReadInt()
        {
            var array = new byte[4];
            return BitConverter.ToInt32(array, 0);
        }

        public void Write(byte[] array, int offset, int length)
        {
            stream.Write(array, offset, length);
        }

        public void Write(byte[] array)
        {
            Write(array, 0, array.Length);
        }

        public void Write(byte value)
        {
            stream.WriteByte(value);
        }

        public void Write(int value)
        {
            stream.Write(BitConverter.GetBytes(value), 0, 4);
        }

        public void Close()
        {
            stream.Flush();
            stream.Close();
        }
    }
}
