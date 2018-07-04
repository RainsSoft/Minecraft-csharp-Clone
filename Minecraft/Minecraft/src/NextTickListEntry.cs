using System;

namespace net.minecraft.src
{

	public class NextTickListEntry : IComparable
	{
		/// <summary>
		/// The id number for the next tick entry </summary>
		private static long NextTickEntryID = 0L;

		/// <summary>
		/// X position this tick is occuring at </summary>
		public int XCoord;

		/// <summary>
		/// Y position this tick is occuring at </summary>
		public int YCoord;

		/// <summary>
		/// Z position this tick is occuring at </summary>
		public int ZCoord;

		/// <summary>
		/// BlockID of the scheduled tick (ensures when the tick occurs its still for this block)
		/// </summary>
		public int BlockID;

		/// <summary>
		/// Time this tick is scheduled to occur at </summary>
		public long ScheduledTime;

		/// <summary>
		/// The id of the tick entry </summary>
		private long TickEntryID;

		public NextTickListEntry(int par1, int par2, int par3, int par4)
		{
			TickEntryID = NextTickEntryID++;
			XCoord = par1;
			YCoord = par2;
			ZCoord = par3;
			BlockID = par4;
		}

		public virtual bool Equals(object par1Obj)
		{
			if (par1Obj is NextTickListEntry)
			{
				NextTickListEntry nextticklistentry = (NextTickListEntry)par1Obj;
				return XCoord == nextticklistentry.XCoord && YCoord == nextticklistentry.YCoord && ZCoord == nextticklistentry.ZCoord && BlockID == nextticklistentry.BlockID;
			}
			else
			{
				return false;
			}
		}

		public virtual int GetHashCode()
		{
			return (XCoord * 1024 * 1024 + ZCoord * 1024 + YCoord) * 256 + BlockID;
		}

		/// <summary>
		/// Sets the scheduled time for this tick entry
		/// </summary>
		public virtual NextTickListEntry SetScheduledTime(long par1)
		{
			ScheduledTime = par1;
			return this;
		}

		/// <summary>
		/// Compares this tick entry to another tick entry for sorting purposes. Compared first based on the scheduled time
		/// and second based on tickEntryID.
		/// </summary>
		public virtual int Comparer(NextTickListEntry par1NextTickListEntry)
		{
			if (ScheduledTime < par1NextTickListEntry.ScheduledTime)
			{
				return -1;
			}

			if (ScheduledTime > par1NextTickListEntry.ScheduledTime)
			{
				return 1;
			}

			if (TickEntryID < par1NextTickListEntry.TickEntryID)
			{
				return -1;
			}

			return TickEntryID <= par1NextTickListEntry.TickEntryID ? 0 : 1;
		}

		public virtual int CompareTo(object par1Obj)
		{
			return Comparer((NextTickListEntry)par1Obj);
		}
	}

}