using System.IO;
using System.Text;

namespace net.minecraft.src
{
	public class NBTTagString : NBTBase
	{
		/// <summary>
		/// The string value for the tag (cannot be empty). </summary>
		public string Data;

		public NBTTagString(string par1Str) : base(par1Str)
		{
		}

		public NBTTagString(string par1Str, string par2Str) : base(par1Str)
		{
			Data = par2Str;

			if (par2Str == null)
			{
				throw new System.ArgumentException("Empty string not allowed");
			}
			else
			{
				return;
			}
		}

		/// <summary>
		/// Write the actual data contents of the tag, implemented in NBT extension classes
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: void write(DataOutput par1DataOutput) throws IOException
        public override void Write(BinaryWriter par1DataOutput)
		{
			par1DataOutput.Write(Data);
		}

		/// <summary>
		/// Read the actual data contents of the tag, implemented in NBT extension classes
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: void load(DataInput par1DataInput) throws IOException
        public override void Load(BinaryReader par1DataInput)
		{
			Data = par1DataInput.ReadString();
		}

		/// <summary>
		/// Gets the type byte for the tag.
		/// </summary>
		public override byte GetId()
		{
			return 8;
		}

		public virtual string ToString()
		{
			return (new StringBuilder()).Append("").Append(Data).ToString();
		}

		/// <summary>
		/// Creates a clone of the tag.
		/// </summary>
		public override NBTBase Copy()
		{
			return new NBTTagString(GetName(), Data);
		}

		public override bool Equals(object par1Obj)
		{
			if (base.Equals(par1Obj))
			{
				NBTTagString nbttagstring = (NBTTagString)par1Obj;
				return Data == null && nbttagstring.Data == null || Data != null && Data.Equals(nbttagstring.Data);
			}
			else
			{
				return false;
			}
		}

		public override int GetHashCode()
		{
			return base.GetHashCode() ^ Data.GetHashCode();
		}
	}
}