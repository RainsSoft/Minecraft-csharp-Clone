using System;
using System.IO;
using System.Text;

namespace net.minecraft.src
{
	public class NBTTagByteArray : NBTBase
	{
		public byte[] ByteArray;

		public NBTTagByteArray(string par1Str) : base(par1Str)
		{
		}

		public NBTTagByteArray(string par1Str, byte[] par2ArrayOfByte) : base(par1Str)
		{
			ByteArray = par2ArrayOfByte;
		}

		/// <summary>
		/// Write the actual data contents of the tag, implemented in NBT extension classes
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: void write(DataOutput par1DataOutput) throws IOException
        public override void Write(BinaryWriter par1DataOutput)
		{
			par1DataOutput.Write(ByteArray.Length);
			par1DataOutput.Write(ByteArray);
		}

		/// <summary>
		/// Read the actual data contents of the tag, implemented in NBT extension classes
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: void load(DataInput par1DataInput) throws IOException
        public override void Load(BinaryReader par1DataInput)
		{
			int i = par1DataInput.ReadInt32();
			ByteArray = new byte[i];
			par1DataInput.Read(ByteArray, (int)par1DataInput.BaseStream.Position, (int)par1DataInput.BaseStream.Length - (int)par1DataInput.BaseStream.Position);
		}

		/// <summary>
		/// Gets the type byte for the tag.
		/// </summary>
		public override byte GetId()
		{
			return 7;
		}

		public virtual string ToString()
		{
			return (new StringBuilder()).Append("[").Append(ByteArray.Length).Append(" bytes]").ToString();
		}

		/// <summary>
		/// Creates a clone of the tag.
		/// </summary>
		public override NBTBase Copy()
		{
			byte[] abyte0 = new byte[ByteArray.Length];
			Array.Copy(ByteArray, 0, abyte0, 0, ByteArray.Length);
			return new NBTTagByteArray(GetName(), abyte0);
		}

		public override bool Equals(object par1Obj)
		{
			if (base.Equals(par1Obj))
			{
				return Array.Equals(ByteArray, ((NBTTagByteArray)par1Obj).ByteArray);
			}
			else
			{
				return false;
			}
		}

		public override int GetHashCode()
		{
			return base.GetHashCode() ^ ByteArray.GetHashCode();
		}
	}
}