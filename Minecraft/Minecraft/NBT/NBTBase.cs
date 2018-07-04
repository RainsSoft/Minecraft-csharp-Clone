using System.IO;

namespace net.minecraft.src
{
	public abstract class NBTBase
	{
		/// <summary>
		/// The UTF string key used to lookup values. </summary>
		private string Name;

		/// <summary>
		/// Write the actual data contents of the tag, implemented in NBT extension classes
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: abstract void write(DataOutput dataoutput) throws IOException;
        public abstract void Write(BinaryWriter dataoutput);

		/// <summary>
		/// Read the actual data contents of the tag, implemented in NBT extension classes
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: abstract void load(DataInput datainput) throws IOException;
        public abstract void Load(BinaryReader datainput);

		/// <summary>
		/// Gets the type byte for the tag.
		/// </summary>
		public abstract byte GetId();

		protected NBTBase(string par1Str)
		{
			if (par1Str == null)
			{
				Name = "";
			}
			else
			{
				Name = par1Str;
			}
		}

		/// <summary>
		/// Sets the name for this tag and returns this for convenience.
		/// </summary>
		public virtual NBTBase SetName(string par1Str)
		{
			if (par1Str == null)
			{
				Name = "";
			}
			else
			{
				Name = par1Str;
			}

			return this;
		}

		/// <summary>
		/// Gets the name corresponding to the tag, or an empty string if none set.
		/// </summary>
		public virtual string GetName()
		{
			if (Name == null)
			{
				return "";
			}
			else
			{
				return Name;
			}
		}

		/// <summary>
		/// Reads and returns a tag from the given DataInput, or the End tag if no tag could be read.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static NBTBase readNamedTag(DataInput par0DataInput) throws IOException
		public static NBTBase ReadNamedTag(BinaryReader par0DataInput)
		{
			byte byte0 = par0DataInput.ReadByte();

			if (byte0 == 0)
			{
				return new NBTTagEnd();
			}
			else
			{
				string s = par0DataInput.ReadString();
				NBTBase nbtbase = NewTag(byte0, s);
				nbtbase.Load(par0DataInput);
				return nbtbase;
			}
		}

		/// <summary>
		/// Writes the specified tag to the given DataOutput, writing the type byte, the UTF string key and then calling the
		/// tag to write its data.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static void writeNamedTag(NBTBase par0NBTBase, DataOutput par1DataOutput) throws IOException
		public static void WriteNamedTag(NBTBase par0NBTBase, BinaryWriter par1DataOutput)
		{
			par1DataOutput.Write(par0NBTBase.GetId());

			if (par0NBTBase.GetId() == 0)
			{
				return;
			}
			else
			{
				par1DataOutput.Write(par0NBTBase.GetName());
				par0NBTBase.Write(par1DataOutput);
				return;
			}
		}

		/// <summary>
		/// Creates and returns a new tag of the specified type, or null if invalid.
		/// </summary>
		public static NBTBase NewTag(byte par0, string par1Str)
		{
			switch (par0)
			{
				case 0:
					return new NBTTagEnd();

				case 1:
					return new NBTTagByte(par1Str);

				case 2:
					return new NBTTagShort(par1Str);

				case 3:
					return new NBTTagInt(par1Str);

				case 4:
					return new NBTTagLong(par1Str);

				case 5:
					return new NBTTagFloat(par1Str);

				case 6:
					return new NBTTagDouble(par1Str);

				case 7:
					return new NBTTagByteArray(par1Str);

				case 11:
					return new NBTTagIntArray(par1Str);

				case 8:
					return new NBTTagString(par1Str);

				case 9:
					return new NBTTagList(par1Str);

				case 10:
					return new NBTTagCompound(par1Str);
			}

			return null;
		}

		/// <summary>
		/// Returns the string name of a tag with the specified type, or 'UNKNOWN' if invalid.
		/// </summary>
		public static string GetTagName(byte par0)
		{
			switch (par0)
			{
				case 0:
					return "TAG_End";

				case 1:
					return "TAG_Byte";

				case 2:
					return "TAG_Short";

				case 3:
					return "TAG_Int";

				case 4:
					return "TAG_Long";

				case 5:
					return "TAG_Float";

				case 6:
					return "TAG_Double";

				case 7:
					return "TAG_Byte_Array";

				case 11:
					return "TAG_Int_Array";

				case 8:
					return "TAG_String";

				case 9:
					return "TAG_List";

				case 10:
					return "TAG_Compound";
			}

			return "UNKNOWN";
		}

		/// <summary>
		/// Creates a clone of the tag.
		/// </summary>
		public abstract NBTBase Copy();

		public virtual bool Equals(object par1Obj)
		{
			if (par1Obj == null || !(par1Obj is NBTBase))
			{
				return false;
			}

			NBTBase nbtbase = (NBTBase)par1Obj;

			if (GetId() != nbtbase.GetId())
			{
				return false;
			}

			if (Name == null && nbtbase.Name != null || Name != null && nbtbase.Name == null)
			{
				return false;
			}

			return Name == null || Name.Equals(nbtbase.Name);
		}

		public virtual int GetHashCode()
		{
			return Name.GetHashCode() ^ GetId();
		}
	}
}