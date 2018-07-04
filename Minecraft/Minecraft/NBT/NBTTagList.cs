using System.Collections.Generic;
using System.IO;
using System.Text;

namespace net.minecraft.src
{
	public class NBTTagList : NBTBase
	{
		/// <summary>
		/// The array list containing the tags encapsulated in this list. </summary>
		private List<NBTBase> TagList;

		/// <summary>
		/// The type byte for the tags in the list - they must all be of the same type.
		/// </summary>
		private byte TagType;

		public NBTTagList() : base("")
		{
			TagList = new List<NBTBase>();
		}

		public NBTTagList(string par1Str) : base(par1Str)
		{
			TagList = new List<NBTBase>();
		}

		/// <summary>
		/// Write the actual data contents of the tag, implemented in NBT extension classes
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: void write(DataOutput par1DataOutput) throws IOException
        public override void Write(BinaryWriter par1DataOutput)
		{
			if (TagList.Count > 0)
			{
				TagType = TagList[0].GetId();
			}
			else
			{
				TagType = 1;
			}

			par1DataOutput.Write(TagType);
			par1DataOutput.Write(TagList.Count);

			for (int i = 0; i < TagList.Count; i++)
			{
				TagList[i].Write(par1DataOutput);
			}
		}

		/// <summary>
		/// Read the actual data contents of the tag, implemented in NBT extension classes
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: void load(DataInput par1DataInput) throws IOException
        public override void Load(BinaryReader par1DataInput)
		{
			TagType = par1DataInput.ReadByte();
			int i = par1DataInput.ReadInt32();
			TagList = new List<NBTBase>();

			for (int j = 0; j < i; j++)
			{
				NBTBase nbtbase = NBTBase.NewTag(TagType, null);
				nbtbase.Load(par1DataInput);
				TagList.Add(nbtbase);
			}
		}

		/// <summary>
		/// Gets the type byte for the tag.
		/// </summary>
		public override byte GetId()
		{
			return 9;
		}

		public virtual string ToString()
		{
			return (new StringBuilder()).Append("").Append(TagList.Count).Append(" entries of type ").Append(NBTBase.GetTagName(TagType)).ToString();
		}

		/// <summary>
		/// Adds the provided tag to the end of the list. There is no check to veriy this tag is of the same type as any
		/// previous tag.
		/// </summary>
		public virtual void AppendTag(NBTBase par1NBTBase)
		{
			TagType = par1NBTBase.GetId();
			TagList.Add(par1NBTBase);
		}

		/// <summary>
		/// Retrieves the tag at the specified index from the list.
		/// </summary>
		public virtual NBTBase TagAt(int par1)
		{
			return (NBTBase)TagList[par1];
		}

		/// <summary>
		/// Returns the number of tags in the list.
		/// </summary>
		public virtual int TagCount()
		{
			return TagList.Count;
		}

		/// <summary>
		/// Creates a clone of the tag.
		/// </summary>
		public override NBTBase Copy()
		{
			NBTTagList nbttaglist = new NBTTagList(GetName());
			nbttaglist.TagType = TagType;
			NBTBase nbtbase1;

			for (IEnumerator<NBTBase> iterator = TagList.GetEnumerator(); iterator.MoveNext(); nbttaglist.TagList.Add(nbtbase1))
			{
				NBTBase nbtbase = iterator.Current;
				nbtbase1 = nbtbase.Copy();
			}

			return nbttaglist;
		}

		public override bool Equals(object par1Obj)
		{
			if (base.Equals(par1Obj))
			{
				NBTTagList nbttaglist = (NBTTagList)par1Obj;

				if (TagType == nbttaglist.TagType)
				{
					return TagList.Equals(nbttaglist.TagList);
				}
			}

			return false;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode() ^ TagList.GetHashCode();
		}
	}
}