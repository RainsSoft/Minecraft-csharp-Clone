using System.IO;
using System.Text;

namespace net.minecraft.src
{
	public class NBTTagFloat : NBTBase
	{
		/// <summary>
		/// The float value for the tag. </summary>
		public float Data;

		public NBTTagFloat(string par1Str) : base(par1Str)
		{
		}

		public NBTTagFloat(string par1Str, float par2) : base(par1Str)
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
			Data = par1DataInput.ReadSingle();
		}

		/// <summary>
		/// Gets the type byte for the tag.
		/// </summary>
		public override byte GetId()
		{
			return 5;
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
			return new NBTTagFloat(GetName(), Data);
		}

		public override bool Equals(object par1Obj)
		{
			if (base.Equals(par1Obj))
			{
				NBTTagFloat nbttagfloat = (NBTTagFloat)par1Obj;
				return Data == nbttagfloat.Data;
			}
			else
			{
				return false;
			}
		}

		public override int GetHashCode()
		{
			return base.GetHashCode() ^ (int)Data;
		}
	}
}