using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace net.minecraft.src
{
    using IOPath = System.IO.Path;

	public class SaveFormatOld : ISaveFormat
	{
		/// <summary>
		/// Reference to the File object representing the directory for the world saves
		/// </summary>
		protected readonly string SavesDirectory;

		public SaveFormatOld(string par1File)
		{
			if (!Directory.Exists(par1File))
			{
				Directory.CreateDirectory(par1File);
			}

			SavesDirectory = par1File;
		}

		public virtual string GetFormatName()
		{
			return "Old Format";
		}

		public virtual List<SaveFormatComparator> GetSaveList()
		{
			List<SaveFormatComparator> arraylist = new List<SaveFormatComparator>();

			for (int i = 0; i < 5; i++)
			{
				string s = (new StringBuilder()).Append("World").Append(i + 1).ToString();
				WorldInfo worldinfo = GetWorldInfo(s);

				if (worldinfo != null)
				{
					arraylist.Add(new SaveFormatComparator(s, "", worldinfo.GetLastTimePlayed(), worldinfo.GetSizeOnDisk(), worldinfo.GetGameType(), false, worldinfo.IsHardcoreModeEnabled()));
				}
			}

			return arraylist;
		}

		public virtual void FlushCache()
		{
		}

		/// <summary>
		/// gets the world info
		/// </summary>
		public virtual WorldInfo GetWorldInfo(string par1Str)
		{
			string file = IOPath.Combine(SavesDirectory, par1Str);

			if (!Directory.Exists(file))
			{
				return null;
			}

            string file1 = IOPath.Combine(file, "level.dat");

			if (File.Exists(file1))
			{
				//try
				{
                    MemoryStream stream = new MemoryStream();
                    using (var fs = new FileStream(file1, FileMode.Open))
                    {
                        fs.CopyTo(stream);
                    }
                    stream.Seek(0, SeekOrigin.Begin);

					NBTTagCompound nbttagcompound = CompressedStreamTools.ReadCompressed(stream);
					NBTTagCompound nbttagcompound2 = nbttagcompound.GetCompoundTag("Data");
					return new WorldInfo(nbttagcompound2);
				}/*
				catch (Exception exception)
				{
                    Utilities.LogException(exception);
				}*/
			}

            file1 = IOPath.Combine(file, "level.dat_old");

			if (File.Exists(file1))
			{
				try
				{
					NBTTagCompound nbttagcompound1 = CompressedStreamTools.ReadCompressed(new FileStream(file1, FileMode.Open));
					NBTTagCompound nbttagcompound3 = nbttagcompound1.GetCompoundTag("Data");
					return new WorldInfo(nbttagcompound3);
				}
				catch (Exception exception1)
				{
                    Utilities.LogException(exception1);
				}
			}

			return null;
		}

		/// <summary>
		/// @args: Takes two arguments - first the name of the directory containing the world and second the new name for
		/// that world. @desc: Renames the world by storing the new name in level.dat. It does *not* rename the directory
		/// containing the world data.
		/// </summary>
		public virtual void RenameWorld(string par1Str, string par2Str)
		{
            string file = IOPath.Combine(SavesDirectory, par1Str);

			if (!File.Exists(file))
			{
				return;
			}

            string file1 = IOPath.Combine(file, "level.dat");

			if (File.Exists(file1))
			{
				try
				{
					NBTTagCompound nbttagcompound = CompressedStreamTools.ReadCompressed(new FileStream(file1, FileMode.Open));
					NBTTagCompound nbttagcompound1 = nbttagcompound.GetCompoundTag("Data");
					nbttagcompound1.SetString("LevelName", par2Str);
					CompressedStreamTools.WriteCompressed(nbttagcompound, new FileStream(file1, FileMode.Create));
				}
				catch (Exception exception)
				{
					Console.WriteLine(exception.ToString());
					Console.Write(exception.StackTrace);
				}
			}
		}

		/// <summary>
		/// @args: Takes one argument - the name of the directory of the world to delete. @desc: Delete the world by deleting
		/// the associated directory recursively.
		/// </summary>
		public virtual void DeleteWorldDirectory(string par1Str)
		{
            string file = IOPath.Combine(SavesDirectory, par1Str);

			if (!Directory.Exists(file))
			{
				return;
			}
			else
			{
                Directory.Delete(file, true);
				return;
			}
		}

		/// <summary>
		/// Returns back a loader for the specified save directory
		/// </summary>
		public virtual ISaveHandler GetSaveLoader(string par1Str, bool par2)
		{
			return new SaveHandler(SavesDirectory, par1Str, par2);
		}

		/// <summary>
		/// Checks if the save directory uses the old map format
		/// </summary>
		public virtual bool IsOldMapFormat(string par1Str)
		{
			return false;
		}

		/// <summary>
		/// Converts the specified map to the new map format. Args: worldName, loadingScreen
		/// </summary>
		public virtual bool ConvertMapFormat(string par1Str, IProgressUpdate par2IProgressUpdate)
		{
			return false;
		}
	}
}