using System;
using System.Text;

namespace net.minecraft.src
{
	class IntHashMapEntry
	{
		/// <summary>
		/// The hash code of this entry </summary>
		public readonly int HashEntry;

		/// <summary>
		/// The object stored in this entry </summary>
		public object ValueEntry;

		/// <summary>
		/// The next entry in this slot </summary>
		public IntHashMapEntry NextEntry;

		/// <summary>
		/// The id of the hash slot computed from the hash </summary>
		public readonly int SlotHash;

		public IntHashMapEntry(int par1, int par2, object par3Obj, IntHashMapEntry par4IntHashMapEntry)
		{
			ValueEntry = par3Obj;
			NextEntry = par4IntHashMapEntry;
			HashEntry = par2;
			SlotHash = par1;
		}

		/// <summary>
		/// Returns the hash code for this entry
		/// </summary>
		public int GetHash()
		{
			return HashEntry;
		}

		/// <summary>
		/// Returns the object stored in this entry
		/// </summary>
		public object GetValue()
		{
			return ValueEntry;
		}

		public bool Equals(object par1Obj)
		{
			if (!(par1Obj is IntHashMapEntry))
			{
				return false;
			}

			IntHashMapEntry inthashmapentry = (IntHashMapEntry)par1Obj;
			int? integer = Convert.ToInt32(GetHash());
			int? integer1 = Convert.ToInt32(inthashmapentry.GetHash());

			if (integer == integer1 || integer != null && integer.Equals(integer1))
			{
				object obj = GetValue();
				object obj1 = inthashmapentry.GetValue();

				if (obj == obj1 || obj != null && obj.Equals(obj1))
				{
					return true;
				}
			}

			return false;
		}

		public int GetHashCode()
		{
			return IntHashMap.GetHash(HashEntry);
		}

		public string ToString()
		{
			return (new StringBuilder()).Append(GetHash()).Append("=").Append(GetValue()).ToString();
		}
	}
}