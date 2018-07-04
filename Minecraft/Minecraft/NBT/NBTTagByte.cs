using System.IO;
using System.Text;

namespace net.minecraft.src
{
	public class NBTTagByte : NBTBase
	{
		/// <summary>
		/// The byte value for the tag. </summary>
		public byte Data;

		public NBTTagByte(string par1Str) : base(par1Str)
		{
		}

		public NBTTagByte(string par1Str, byte par2) : base(par1Str)
		{
			Data = par2;
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
			Data = par1DataInput.ReadByte();
		}

		/// <summary>
		/// Gets the type byte for the tag.
		/// </summary>
		public override byte GetId()
		{
			return 1;
		}

		public virtual string ToString()
		{
			return new StringBuilder().Append("").Append(Data).ToString();
		}

		/// <summary>
		/// Creates a clone of the tag.
		/// </summary>
		public override NBTBase Copy()
		{
			return new NBTTagByte(GetName(), Data);
		}

		public override bool Equals(object par1Obj)
		{
			if (base.Equals(par1Obj))
			{
				NBTTagByte nbttagbyte = (NBTTagByte)par1Obj;
				return Data == nbttagbyte.Data;
			}
			else
			{
				return false;
			}
		}

		public override int GetHashCode()
		{
			return base.GetHashCode() ^ Data;
		}
	}
}