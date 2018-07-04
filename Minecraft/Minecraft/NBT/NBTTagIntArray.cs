using System;
using System.IO;
using System.Text;

namespace net.minecraft.src
{
	public class NBTTagIntArray : NBTBase
	{
		public int[] Field_48181_a;

		public NBTTagIntArray(string par1Str) : base(par1Str)
		{
		}

		public NBTTagIntArray(string par1Str, int[] par2ArrayOfInteger) : base(par1Str)
		{
			Field_48181_a = par2ArrayOfInteger;
		}

		/// <summary>
		/// Write the actual data contents of the tag, implemented in NBT extension classes
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: void write(DataOutput par1DataOutput) throws IOException
        public override void Write(BinaryWriter par1DataOutput)
		{
			par1DataOutput.Write(Field_48181_a.Length);

			for (int i = 0; i < Field_48181_a.Length; i++)
			{
				par1DataOutput.Write(Field_48181_a[i]);
			}
		}

		/// <summary>
		/// Read the actual data contents of the tag, implemented in NBT extension classes
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: void load(DataInput par1DataInput) throws IOException
        public override void Load(BinaryReader par1DataInput)
		{
			int i = par1DataInput.ReadInt32();
			Field_48181_a = new int[i];

			for (int j = 0; j < i; j++)
			{
				Field_48181_a[j] = par1DataInput.ReadInt32();
			}
		}

		/// <summary>
		/// Gets the type byte for the tag.
		/// </summary>
		public override byte GetId()
		{
			return 11;
		}

		public virtual string ToString()
		{
			return (new StringBuilder()).Append("[").Append(Field_48181_a.Length).Append(" bytes]").ToString();
		}

		/// <summary>
		/// Creates a clone of the tag.
		/// </summary>
		public override NBTBase Copy()
		{
			int[] ai = new int[Field_48181_a.Length];
			Array.Copy(Field_48181_a, 0, ai, 0, Field_48181_a.Length);
			return new NBTTagIntArray(GetName(), ai);
		}

		public override bool Equals(object par1Obj)
		{
			if (base.Equals(par1Obj))
			{
				NBTTagIntArray nbttagintarray = (NBTTagIntArray)par1Obj;
				return Field_48181_a == null && nbttagintarray.Field_48181_a == null || Field_48181_a != null && Field_48181_a.Equals(nbttagintarray.Field_48181_a);
			}
			else
			{
				return false;
			}
		}

		public override int GetHashCode()
		{
			return base.GetHashCode() ^ Field_48181_a.GetHashCode();
		}
	}
}