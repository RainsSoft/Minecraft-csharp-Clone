using System;

namespace net.minecraft.src
{

	public class LongHashMap
	{
		[NonSerialized]
		private LongHashMapEntry[] HashArray;

		/// <summary>
		/// the number of elements in the hash array </summary>
		[NonSerialized]
		private int NumHashElements;

		/// <summary>
		/// the maximum amount of elements in the hash (probably 3/4 the size due to meh hashing function)
		/// </summary>
		private int Capacity;

		/// <summary>
		/// percent of the hasharray that can be used without hash colliding probably
		/// </summary>
		private readonly float PercentUseable = 0.75F;

		/// <summary>
		/// count of times elements have been added/removed </summary>
		[NonSerialized]
		private volatile int ModCount;

		public LongHashMap()
		{
			Capacity = 12;
			HashArray = new LongHashMapEntry[16];
		}

		/// <summary>
		/// returns the hashed key given the original key
		/// </summary>
		private static int GetHashedKey(long par0)
		{
			return Hash((int)(par0 ^ (long)((ulong)par0 >> 32)));
		}

		/// <summary>
		/// the hash function
		/// </summary>
		private static int Hash(int par0)
		{
			par0 ^= (int)((uint)par0 >> 20 ^ par0) >> 12;
			return par0 ^ (int)((uint)par0 >> 7 ^ par0) >> 4;
		}

		/// <summary>
		/// gets the index in the hash given the array length and the hashed key
		/// </summary>
		private static int GetHashIndex(int par0, int par1)
		{
			return par0 & par1 - 1;
		}

		public virtual int GetNumHashElements()
		{
			return NumHashElements;
		}

		/// <summary>
		/// get the value from the map given the key
		/// </summary>
		public virtual object GetValueByKey(long par1)
		{
			int i = GetHashedKey(par1);

			for (LongHashMapEntry longhashmapentry = HashArray[GetHashIndex(i, HashArray.Length)]; longhashmapentry != null; longhashmapentry = longhashmapentry.NextEntry)
			{
				if (longhashmapentry.Key == par1)
				{
					return longhashmapentry.Value;
				}
			}

			return null;
		}

		public virtual bool ContainsItem(long par1)
		{
			return GetEntry(par1) != null;
		}

		LongHashMapEntry GetEntry(long par1)
		{
			int i = GetHashedKey(par1);

			for (LongHashMapEntry longhashmapentry = HashArray[GetHashIndex(i, HashArray.Length)]; longhashmapentry != null; longhashmapentry = longhashmapentry.NextEntry)
			{
				if (longhashmapentry.Key == par1)
				{
					return longhashmapentry;
				}
			}

			return null;
		}

		/// <summary>
		/// add the key value pair to the list
		/// </summary>
		public virtual void Add(long par1, object par3Obj)
		{
			int i = GetHashedKey(par1);
			int j = GetHashIndex(i, HashArray.Length);

			for (LongHashMapEntry longhashmapentry = HashArray[j]; longhashmapentry != null; longhashmapentry = longhashmapentry.NextEntry)
			{
				if (longhashmapentry.Key == par1)
				{
					longhashmapentry.Value = par3Obj;
				}
			}

			ModCount++;
			CreateKey(i, par1, par3Obj, j);
		}

		/// <summary>
		/// resizes the table
		/// </summary>
		private void ResizeTable(int par1)
		{
			LongHashMapEntry[] alonghashmapentry = HashArray;
			int i = alonghashmapentry.Length;

			if (i == 0x40000000)
			{
				Capacity = 0x7fffffff;
				return;
			}
			else
			{
				LongHashMapEntry[] alonghashmapentry1 = new LongHashMapEntry[par1];
				CopyHashTableTo(alonghashmapentry1);
				HashArray = alonghashmapentry1;
				Capacity = (int)((float)par1 * PercentUseable);
				return;
			}
		}

		/// <summary>
		/// copies the hash table to the specified array
		/// </summary>
		private void CopyHashTableTo(LongHashMapEntry[] par1ArrayOfLongHashMapEntry)
		{
			LongHashMapEntry[] alonghashmapentry = HashArray;
			int i = par1ArrayOfLongHashMapEntry.Length;

			for (int j = 0; j < alonghashmapentry.Length; j++)
			{
				LongHashMapEntry longhashmapentry = alonghashmapentry[j];

				if (longhashmapentry == null)
				{
					continue;
				}

				alonghashmapentry[j] = null;

				do
				{
					LongHashMapEntry longhashmapentry1 = longhashmapentry.NextEntry;
					int k = GetHashIndex(longhashmapentry.Hash, i);
					longhashmapentry.NextEntry = par1ArrayOfLongHashMapEntry[k];
					par1ArrayOfLongHashMapEntry[k] = longhashmapentry;
					longhashmapentry = longhashmapentry1;
				}
				while (longhashmapentry != null);
			}
		}

		/// <summary>
		/// calls the removeKey method and returns removed object
		/// </summary>
		public virtual object Remove(long par1)
		{
			LongHashMapEntry longhashmapentry = RemoveKey(par1);
			return longhashmapentry != null ? longhashmapentry.Value : null;
		}

		/// <summary>
		/// removes the key from the hash linked list
		/// </summary>
		LongHashMapEntry RemoveKey(long par1)
		{
			int i = GetHashedKey(par1);
			int j = GetHashIndex(i, HashArray.Length);
			LongHashMapEntry longhashmapentry = HashArray[j];
			LongHashMapEntry longhashmapentry1;
			LongHashMapEntry longhashmapentry2;

			for (longhashmapentry1 = longhashmapentry; longhashmapentry1 != null; longhashmapentry1 = longhashmapentry2)
			{
				longhashmapentry2 = longhashmapentry1.NextEntry;

				if (longhashmapentry1.Key == par1)
				{
					ModCount++;
					NumHashElements--;

					if (longhashmapentry == longhashmapentry1)
					{
						HashArray[j] = longhashmapentry2;
					}
					else
					{
						longhashmapentry.NextEntry = longhashmapentry2;
					}

					return longhashmapentry1;
				}

				longhashmapentry = longhashmapentry1;
			}

			return longhashmapentry1;
		}

		/// <summary>
		/// creates the key in the hash table
		/// </summary>
		private void CreateKey(int par1, long par2, object par4Obj, int par5)
		{
			LongHashMapEntry longhashmapentry = HashArray[par5];
			HashArray[par5] = new LongHashMapEntry(par1, par2, par4Obj, longhashmapentry);

			if (NumHashElements++ >= Capacity)
			{
				ResizeTable(2 * HashArray.Length);
			}
		}

		/// <summary>
		/// public method to get the hashed key(hashCode)
		/// </summary>
		public static int GetHashCode(long par0)
		{
			return GetHashedKey(par0);
		}
	}
}