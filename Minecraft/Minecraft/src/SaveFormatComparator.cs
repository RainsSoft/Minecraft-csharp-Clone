using System;

namespace net.minecraft.src
{
	public class SaveFormatComparator : IComparable
	{
		/// <summary>
		/// the file name of this save </summary>
		private readonly string FileName;

		/// <summary>
		/// the displayed name of this save file </summary>
		private readonly string DisplayName;
		private readonly long LastTimePlayed;
		private readonly long SizeOnDisk;
		private readonly bool requiresConversion_Renamed;
		private readonly int GameType;
		private readonly bool Hardcore;

		public SaveFormatComparator(string par1Str, string par2Str, long par3, long par5, int par7, bool par8, bool par9)
		{
			FileName = par1Str;
			DisplayName = par2Str;
			LastTimePlayed = par3;
			SizeOnDisk = par5;
			GameType = par7;
			requiresConversion_Renamed = par8;
			Hardcore = par9;
		}

		/// <summary>
		/// return the file name
		/// </summary>
		public virtual string GetFileName()
		{
			return FileName;
		}

		/// <summary>
		/// return the display name of the save
		/// </summary>
		public virtual string GetDisplayName()
		{
			return DisplayName;
		}

		public virtual bool RequiresConversion()
		{
			return requiresConversion_Renamed;
		}

		public virtual long GetLastTimePlayed()
		{
			return LastTimePlayed;
		}

		public virtual int CompareTo(SaveFormatComparator par1SaveFormatComparator)
		{
			if (LastTimePlayed < par1SaveFormatComparator.LastTimePlayed)
			{
				return 1;
			}

			if (LastTimePlayed > par1SaveFormatComparator.LastTimePlayed)
			{
				return -1;
			}
			else
			{
				return FileName.CompareTo(par1SaveFormatComparator.FileName);
			}
		}

		public virtual int GetGameType()
		{
			return GameType;
		}

		public virtual bool IsHardcoreModeEnabled()
		{
			return Hardcore;
		}

		public virtual int CompareTo(object par1Obj)
		{
			return CompareTo((SaveFormatComparator)par1Obj);
		}
	}
}