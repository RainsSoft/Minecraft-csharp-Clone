using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class IntHashMap
	{
		[NonSerialized]
		private IntHashMapEntry[] Slots;

		/// <summary>
		/// The number of items stored in this map </summary>
		[NonSerialized]
		private int Count;

		/// <summary>
		/// The grow threshold </summary>
		private int Threshold;

		/// <summary>
		/// The scale factor used to determine when to grow the table </summary>
		private readonly float GrowFactor = 0.75F;

		/// <summary>
		/// A serial stamp used to mark changes </summary>
		[NonSerialized]
		private volatile int VersionStamp;

		/// <summary>
		/// The set of all the keys stored in this MCHash object </summary>
		private List<int> KeySet;

		public IntHashMap()
		{
            KeySet = new List<int>();
			Threshold = 12;
			Slots = new IntHashMapEntry[16];
		}

		/// <summary>
		/// Makes the passed in integer suitable for hashing by a number of shifts
		/// </summary>
		private static int ComputeHash(int par0)
		{
			par0 ^= (int)((uint)par0 >> 20 ^ par0 >> 12);
			return par0 ^ (int)((uint)par0 >> 7 ^ par0 >> 4);
		}

		/// <summary>
		/// Computes the index of the slot for the hash and slot count passed in.
		/// </summary>
		private static int GetSlotIndex(int par0, int par1)
		{
			return par0 & par1 - 1;
		}

		/// <summary>
		/// Returns the object associated to a key
		/// </summary>
		public virtual object Lookup(int par1)
		{
			int i = ComputeHash(par1);

			for (IntHashMapEntry inthashmapentry = Slots[GetSlotIndex(i, Slots.Length)]; inthashmapentry != null; inthashmapentry = inthashmapentry.NextEntry)
			{
				if (inthashmapentry.HashEntry == par1)
				{
					return inthashmapentry.ValueEntry;
				}
			}

			return null;
		}

		/// <summary>
		/// Return true if an object is associated with the given key
		/// </summary>
		public virtual bool ContainsItem(int par1)
		{
			return LookupEntry(par1) != null;
		}

		/// <summary>
		/// Returns the key/object mapping for a given key as a MCHashEntry
		/// </summary>
		IntHashMapEntry LookupEntry(int par1)
		{
			int i = ComputeHash(par1);

			for (IntHashMapEntry inthashmapentry = Slots[GetSlotIndex(i, Slots.Length)]; inthashmapentry != null; inthashmapentry = inthashmapentry.NextEntry)
			{
				if (inthashmapentry.HashEntry == par1)
				{
					return inthashmapentry;
				}
			}

			return null;
		}

		/// <summary>
		/// Adds a key and associated value to this map
		/// </summary>
		public virtual void AddKey(int par1, object par2Obj)
		{
			KeySet.Add(par1);
			int i = ComputeHash(par1);
			int j = GetSlotIndex(i, Slots.Length);

			for (IntHashMapEntry inthashmapentry = Slots[j]; inthashmapentry != null; inthashmapentry = inthashmapentry.NextEntry)
			{
				if (inthashmapentry.HashEntry == par1)
				{
					inthashmapentry.ValueEntry = par2Obj;
				}
			}

			VersionStamp++;
			Insert(i, par1, par2Obj, j);
		}

		/// <summary>
		/// Increases the number of hash slots
		/// </summary>
		private void Grow(int par1)
		{
			IntHashMapEntry[] ainthashmapentry = Slots;
			int i = ainthashmapentry.Length;

			if (i == 0x40000000)
			{
				Threshold = 0x7fffffff;
				return;
			}
			else
			{
				IntHashMapEntry[] ainthashmapentry1 = new IntHashMapEntry[par1];
				CopyTo(ainthashmapentry1);
				Slots = ainthashmapentry1;
				Threshold = (int)((float)par1 * GrowFactor);
				return;
			}
		}

		/// <summary>
		/// Copies the hash slots to a new array
		/// </summary>
		private void CopyTo(IntHashMapEntry[] par1ArrayOfIntHashMapEntry)
		{
			IntHashMapEntry[] ainthashmapentry = Slots;
			int i = par1ArrayOfIntHashMapEntry.Length;

			for (int j = 0; j < ainthashmapentry.Length; j++)
			{
				IntHashMapEntry inthashmapentry = ainthashmapentry[j];

				if (inthashmapentry == null)
				{
					continue;
				}

				ainthashmapentry[j] = null;

				do
				{
					IntHashMapEntry inthashmapentry1 = inthashmapentry.NextEntry;
					int k = GetSlotIndex(inthashmapentry.SlotHash, i);
					inthashmapentry.NextEntry = par1ArrayOfIntHashMapEntry[k];
					par1ArrayOfIntHashMapEntry[k] = inthashmapentry;
					inthashmapentry = inthashmapentry1;
				}
				while (inthashmapentry != null);
			}
		}

		/// <summary>
		/// Removes the specified object from the map and returns it
		/// </summary>
		public virtual object RemoveObject(int par1)
		{
			KeySet.Remove(par1);
			IntHashMapEntry inthashmapentry = RemoveEntry(par1);
			return inthashmapentry != null ? inthashmapentry.ValueEntry : null;
		}

		/// <summary>
		/// Removes the specified entry from the map and returns it
		/// </summary>
		IntHashMapEntry RemoveEntry(int par1)
		{
			int i = ComputeHash(par1);
			int j = GetSlotIndex(i, Slots.Length);
			IntHashMapEntry inthashmapentry = Slots[j];
			IntHashMapEntry inthashmapentry1;
			IntHashMapEntry inthashmapentry2;

			for (inthashmapentry1 = inthashmapentry; inthashmapentry1 != null; inthashmapentry1 = inthashmapentry2)
			{
				inthashmapentry2 = inthashmapentry1.NextEntry;

				if (inthashmapentry1.HashEntry == par1)
				{
					VersionStamp++;
					Count--;

					if (inthashmapentry == inthashmapentry1)
					{
						Slots[j] = inthashmapentry2;
					}
					else
					{
						inthashmapentry.NextEntry = inthashmapentry2;
					}

					return inthashmapentry1;
				}

				inthashmapentry = inthashmapentry1;
			}

			return inthashmapentry1;
		}

		/// <summary>
		/// Removes all entries from the map
		/// </summary>
		public virtual void ClearMap()
		{
			VersionStamp++;
			IntHashMapEntry[] ainthashmapentry = Slots;

			for (int i = 0; i < ainthashmapentry.Length; i++)
			{
				ainthashmapentry[i] = null;
			}

			Count = 0;
		}

		/// <summary>
		/// Adds an object to a slot
		/// </summary>
		private void Insert(int par1, int par2, object par3Obj, int par4)
		{
			IntHashMapEntry inthashmapentry = Slots[par4];
			Slots[par4] = new IntHashMapEntry(par1, par2, par3Obj, inthashmapentry);

			if (Count++ >= Threshold)
			{
				Grow(2 * Slots.Length);
			}
		}

		/// <summary>
		/// Return the Set of all keys stored in this MCHash object
		/// </summary>
        public virtual List<int> GetKeySet()
		{
			return KeySet;
		}

		/// <summary>
		/// Returns the hash code for a key
		/// </summary>
		public static int GetHash(int par0)
		{
			return ComputeHash(par0);
		}
	}
}