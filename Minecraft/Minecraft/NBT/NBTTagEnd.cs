using System.IO;

namespace net.minecraft.src
{
	public class NBTTagEnd : NBTBase
	{
		public NBTTagEnd() : base(null)
		{
		}

		/// <summary>
		/// Read the actual data contents of the tag, implemented in NBT extension classes
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: void load(DataInput datainput) throws IOException
        public override void Load(BinaryReader datainput)
		{
		}

		/// <summary>
		/// Write the actual data contents of the tag, implemented in NBT extension classes
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: void write(DataOutput dataoutput) throws IOException
        public override void Write(BinaryWriter dataoutput)
		{
		}

		/// <summary>
		/// Gets the type byte for the tag.
		/// </summary>
		public override byte GetId()
		{
			return 0;
		}

		public virtual string ToString()
		{
			return "END";
		}

		/// <summary>
		/// Creates a clone of the tag.
		/// </summary>
		public override NBTBase Copy()
		{
			return new NBTTagEnd();
		}
	}
}