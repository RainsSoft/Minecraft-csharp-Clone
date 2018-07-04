using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class SoundPool
	{
		/// <summary>
		/// The RNG used by SoundPool. </summary>
		private Random Rand;

		/// <summary>
		/// Maps a name (can be sound/newsound/streaming/music/newmusic) to a list of SoundPoolEntry's.
		/// </summary>
		private Dictionary<string, List<SoundPoolEntry>> NameToSoundPoolEntriesMapping;

		/// <summary>
		/// A list of all SoundPoolEntries that have been loaded. </summary>
		private List<SoundPoolEntry> AllSoundPoolEntries;

		/// <summary>
		/// The number of soundPoolEntry's. This value is computed but never used (should be equal to
		/// allSoundPoolEntries.size()).
		/// </summary>
		public int NumberOfSoundPoolEntries;
		public bool IsGetRandomSound;

		public SoundPool()
		{
			Rand = new Random();
            NameToSoundPoolEntriesMapping = new Dictionary<string, List<SoundPoolEntry>>();
            AllSoundPoolEntries = new List<SoundPoolEntry>();
			NumberOfSoundPoolEntries = 0;
			IsGetRandomSound = true;
		}

		/// <summary>
		/// Adds a sound to this sound pool.
		/// </summary>
		public virtual SoundPoolEntry AddSound(string par1Str, string par2File)
		{
			try
			{
				string s = par1Str;
				par1Str = par1Str.Substring(0, par1Str.IndexOf("."));

				if (IsGetRandomSound)
				{
					for (; char.IsDigit(par1Str[par1Str.Length - 1]); par1Str = par1Str.Substring(0, par1Str.Length - 1))
					{
					}
				}

				par1Str = par1Str.Replace("/", ".");

				if (!NameToSoundPoolEntriesMapping.ContainsKey(par1Str))
				{
					NameToSoundPoolEntriesMapping[par1Str] = new List<SoundPoolEntry>();
				}

				SoundPoolEntry soundpoolentry = new SoundPoolEntry(s, par2File);
				NameToSoundPoolEntriesMapping[par1Str].Add(soundpoolentry);
				AllSoundPoolEntries.Add(soundpoolentry);
				NumberOfSoundPoolEntries++;
				return soundpoolentry;
			}
			catch (Exception malformedurlexception)
			{
                Utilities.LogException(malformedurlexception);

				throw malformedurlexception;
			}
		}

		/// <summary>
		/// gets a random sound from the specified (by name, can be sound/newsound/streaming/music/newmusic) sound pool.
		/// </summary>
		public virtual SoundPoolEntry GetRandomSoundFromSoundPool(string par1Str)
		{
            if (NameToSoundPoolEntriesMapping.ContainsKey(par1Str))
            {
                List<SoundPoolEntry> list = NameToSoundPoolEntriesMapping[par1Str];
                return list[Rand.Next(list.Count)];
            }

			return null;
		}

		/// <summary>
		/// Gets a random SoundPoolEntry.
		/// </summary>
		public virtual SoundPoolEntry GetRandomSound()
		{
			if (AllSoundPoolEntries.Count == 0)
			{
				return null;
			}
			else
			{
				return AllSoundPoolEntries[Rand.Next(AllSoundPoolEntries.Count)];
			}
		}
	}
}