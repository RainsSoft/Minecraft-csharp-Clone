using System.Collections.Generic;
using System.IO;
using System.Text;

namespace net.minecraft.src
{
	public class NBTTagCompound : NBTBase
	{
		/// <summary>
		/// The key-value pairs for the tag. Each key is a UTF string, each value is a tag.
		/// </summary>
		private Dictionary<string, NBTBase> TagMap;

		public NBTTagCompound()
            : base("")
		{
			TagMap = new Dictionary<string, NBTBase>();
		}

		public NBTTagCompound(string par1Str)
            : base(par1Str)
		{
			TagMap = new Dictionary<string, NBTBase>();
		}

		/// <summary>
		/// Write the actual data contents of the tag, implemented in NBT extension classes
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: void write(DataOutput par1DataOutput) throws IOException
        public override void Write(BinaryWriter par1DataOutput)
		{
			NBTBase nbtbase;

			for (IEnumerator<NBTBase> iterator = TagMap.Values.GetEnumerator(); iterator.MoveNext(); NBTBase.WriteNamedTag(nbtbase, par1DataOutput))
			{
				nbtbase = iterator.Current;
			}

			par1DataOutput.Write(0);
		}

		/// <summary>
		/// Read the actual data contents of the tag, implemented in NBT extension classes
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: void load(DataInput par1DataInput) throws IOException
        public override void Load(BinaryReader par1DataInput)
		{
			TagMap.Clear();
			NBTBase nbtbase;

            if ((nbtbase = NBTBase.ReadNamedTag(par1DataInput)).GetId() != 0)
                TagMap[nbtbase.GetName()] = nbtbase;
		}

		/// <summary>
		/// Returns all the values in the tagMap HashMap.
		/// </summary>
		public virtual List<NBTBase> GetTags()
		{
			return new List<NBTBase>(TagMap.Values);
		}

		/// <summary>
		/// Gets the type byte for the tag.
		/// </summary>
		public override byte GetId()
		{
			return 10;
		}

		/// <summary>
		/// Stores the given tag into the map with the given string key. This is mostly used to store tag lists.
		/// </summary>
		public virtual void SetTag(string par1Str, NBTBase par2NBTBase)
		{
			TagMap[par1Str] = par2NBTBase.SetName(par1Str);
		}

		/// <summary>
		/// Stores a new NBTTagByte with the given byte value into the map with the given string key.
		/// </summary>
		public virtual void SetByte(string par1Str, byte par2)
		{
			TagMap[par1Str] = new NBTTagByte(par1Str, par2);
		}

		/// <summary>
		/// Stores a new NBTTagShort with the given short value into the map with the given string key.
		/// </summary>
		public virtual void SetShort(string par1Str, short par2)
		{
			TagMap[par1Str] = new NBTTagShort(par1Str, par2);
		}

		/// <summary>
		/// Stores a new NBTTagInt with the given integer value into the map with the given string key.
		/// </summary>
		public virtual void SetInteger(string par1Str, int par2)
		{
			TagMap[par1Str] = new NBTTagInt(par1Str, par2);
		}

		/// <summary>
		/// Stores a new NBTTagLong with the given long value into the map with the given string key.
		/// </summary>
		public virtual void SetLong(string par1Str, long par2)
		{
			TagMap[par1Str] = new NBTTagLong(par1Str, par2);
		}

		/// <summary>
		/// Stores a new NBTTagFloat with the given float value into the map with the given string key.
		/// </summary>
		public virtual void SetFloat(string par1Str, float par2)
		{
			TagMap[par1Str] = new NBTTagFloat(par1Str, par2);
		}

		/// <summary>
		/// Stores a new NBTTagDouble with the given double value into the map with the given string key.
		/// </summary>
		public virtual void SetDouble(string par1Str, double par2)
		{
			TagMap[par1Str] = new NBTTagDouble(par1Str, par2);
		}

		/// <summary>
		/// Stores a new NBTTagString with the given string value into the map with the given string key.
		/// </summary>
		public virtual void SetString(string par1Str, string par2Str)
		{
			TagMap[par1Str] = new NBTTagString(par1Str, par2Str);
		}

		/// <summary>
		/// Stores a new NBTTagByteArray with the given array as data into the map with the given string key.
		/// </summary>
		public virtual void SetByteArray(string par1Str, byte[] par2ArrayOfByte)
		{
			TagMap[par1Str] = new NBTTagByteArray(par1Str, par2ArrayOfByte);
		}

		public virtual void Func_48183_a(string par1Str, int[] par2ArrayOfInteger)
		{
			TagMap[par1Str] = new NBTTagIntArray(par1Str, par2ArrayOfInteger);
		}

		/// <summary>
		/// Stores the given NBTTagCompound into the map with the given string key.
		/// </summary>
		public virtual void SetCompoundTag(string par1Str, NBTTagCompound par2NBTTagCompound)
		{
			TagMap[par1Str] = par2NBTTagCompound.SetName(par1Str);
		}

		/// <summary>
		/// Stores the given bool value as a NBTTagByte, storing 1 for true and 0 for false, using the given string key.
		/// </summary>
		public virtual void Setbool(string par1Str, bool par2)
		{
			SetByte(par1Str, (byte)(par2 ? 1 : 0));
		}

		/// <summary>
		/// gets a generic tag with the specified name
		/// </summary>
		public virtual NBTBase GetTag(string par1Str)
		{
			return (NBTBase)TagMap[par1Str];
		}

		/// <summary>
		/// Returns whether the given string has been previously stored as a key in the map.
		/// </summary>
		public virtual bool HasKey(string par1Str)
		{
			return TagMap.ContainsKey(par1Str);
		}

		/// <summary>
		/// Retrieves a byte value using the specified key, or 0 if no such key was stored.
		/// </summary>
		public virtual byte GetByte(string par1Str)
		{
			if (!TagMap.ContainsKey(par1Str))
			{
				return 0;
			}
			else
			{
				return ((NBTTagByte)TagMap[par1Str]).Data;
			}
		}

		/// <summary>
		/// Retrieves a short value using the specified key, or 0 if no such key was stored.
		/// </summary>
		public virtual short GetShort(string par1Str)
		{
            if (!TagMap.ContainsKey(par1Str))
			{
				return 0;
			}
			else
			{
				return ((NBTTagShort)TagMap[par1Str]).Data;
			}
		}

		/// <summary>
		/// Retrieves an integer value using the specified key, or 0 if no such key was stored.
		/// </summary>
		public virtual int GetInteger(string par1Str)
		{
            if (!TagMap.ContainsKey(par1Str))
			{
				return 0;
			}
			else
			{
				return ((NBTTagInt)TagMap[par1Str]).Data;
			}
		}

		/// <summary>
		/// Retrieves a long value using the specified key, or 0 if no such key was stored.
		/// </summary>
		public virtual long GetLong(string par1Str)
		{
            if (!TagMap.ContainsKey(par1Str))
			{
				return 0L;
			}
			else
			{
				return ((NBTTagLong)TagMap[par1Str]).Data;
			}
		}

		/// <summary>
		/// Retrieves a float value using the specified key, or 0 if no such key was stored.
		/// </summary>
		public virtual float GetFloat(string par1Str)
		{
            if (!TagMap.ContainsKey(par1Str))
			{
				return 0.0F;
			}
			else
			{
				return ((NBTTagFloat)TagMap[par1Str]).Data;
			}
		}

		/// <summary>
		/// Retrieves a double value using the specified key, or 0 if no such key was stored.
		/// </summary>
		public virtual double GetDouble(string par1Str)
		{
            if (!TagMap.ContainsKey(par1Str))
			{
				return 0.0D;
			}
			else
			{
				return ((NBTTagDouble)TagMap[par1Str]).Data;
			}
		}

		/// <summary>
		/// Retrieves a string value using the specified key, or an empty string if no such key was stored.
		/// </summary>
		public virtual string GetString(string par1Str)
		{
            if (!TagMap.ContainsKey(par1Str))
			{
				return "";
			}
			else
			{
				return ((NBTTagString)TagMap[par1Str]).Data;
			}
		}

		/// <summary>
		/// Retrieves a byte array using the specified key, or a zero-length array if no such key was stored.
		/// </summary>
		public virtual byte[] GetByteArray(string par1Str)
		{
            if (!TagMap.ContainsKey(par1Str))
			{
				return new byte[0];
			}
			else
			{
				return ((NBTTagByteArray)TagMap[par1Str]).ByteArray;
			}
		}

		public virtual int[] Func_48182_l(string par1Str)
		{
            if (!TagMap.ContainsKey(par1Str))
			{
				return new int[0];
			}
			else
			{
				return ((NBTTagIntArray)TagMap[par1Str]).Field_48181_a;
			}
		}

		/// <summary>
		/// Retrieves a NBTTagCompound subtag matching the specified key, or a new empty NBTTagCompound if no such key was
		/// stored.
		/// </summary>
		public virtual NBTTagCompound GetCompoundTag(string par1Str)
		{
            if (!TagMap.ContainsKey(par1Str))
			{
				return new NBTTagCompound(par1Str);
			}
			else
			{
				return (NBTTagCompound)TagMap[par1Str];
			}
		}

		/// <summary>
		/// Retrieves a NBTTagList subtag matching the specified key, or a new empty NBTTagList if no such key was stored.
		/// </summary>
		public virtual NBTTagList GetTagList(string par1Str)
		{
            if (!TagMap.ContainsKey(par1Str))
			{
				return new NBTTagList(par1Str);
			}
			else
			{
				return (NBTTagList)TagMap[par1Str];
			}
		}

		/// <summary>
		/// Retrieves a bool value using the specified key, or false if no such key was stored. This uses the getByte
		/// method.
		/// </summary>
		public virtual bool Getbool(string par1Str)
		{
			return GetByte(par1Str) != 0;
		}

		public virtual string ToString()
		{
			return (new StringBuilder()).Append("").Append(TagMap.Count).Append(" entries").ToString();
		}

		/// <summary>
		/// Creates a clone of the tag.
		/// </summary>
		public override NBTBase Copy()
		{
			NBTTagCompound nbttagcompound = new NBTTagCompound(GetName());
			string s;

			for (IEnumerator<string> iterator = TagMap.Keys.GetEnumerator(); iterator.MoveNext(); nbttagcompound.SetTag(s, ((NBTBase)TagMap[s]).Copy()))
			{
				s = (string)iterator.Current;
			}

			return nbttagcompound;
		}

		public override bool Equals(object par1Obj)
		{
			if (base.Equals(par1Obj))
			{
				NBTTagCompound nbttagcompound = (NBTTagCompound)par1Obj;
				return TagMap.Equals(nbttagcompound.TagMap);
			}
			else
			{
				return false;
			}
		}

		public override int GetHashCode()
		{
			return base.GetHashCode() ^ TagMap.GetHashCode();
		}
	}
}