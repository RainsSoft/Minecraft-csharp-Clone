using System;
using System.Text;

namespace net.minecraft.src
{
	class LongHashMapEntry
	{
		/// <summary>
		/// the key as a long (for playerInstances it is the x in the most significant 32 bits and then y)
		/// </summary>
		public readonly long Key;

		/// <summary>
		/// the value held by the hash at the specified key </summary>
		public object Value;

		/// <summary>
		/// the next hashentry in the table </summary>
		public LongHashMapEntry NextEntry;
		public readonly int Hash;

		public LongHashMapEntry(int par1, long par2, object par4Obj, LongHashMapEntry par5LongHashMapEntry)
		{
			Value = par4Obj;
			NextEntry = par5LongHashMapEntry;
			Key = par2;
			Hash = par1;
		}

		public long GetKey()
		{
			return Key;
		}

		public object GetValue()
		{
			return Value;
		}

		public bool Equals(object par1Obj)
		{
			if (!(par1Obj is LongHashMapEntry))
			{
				return false;
			}

			LongHashMapEntry longhashmapentry = (LongHashMapEntry)par1Obj;
			long? long1 = Convert.ToInt64(GetKey());
			long? long2 = Convert.ToInt64(longhashmapentry.GetKey());

			if (long1 == long2 || long1 != null && long1.Equals(long2))
			{
				object obj = GetValue();
				object obj1 = longhashmapentry.GetValue();

				if (obj == obj1 || obj != null && obj.Equals(obj1))
				{
					return true;
				}
			}

			return false;
		}

		public int GetHashCode()
		{
			return LongHashMap.GetHashCode(Key);
		}

		public string ToString()
		{
			return (new StringBuilder()).Append(GetKey()).Append("=").Append(GetValue()).ToString();
		}
	}
}