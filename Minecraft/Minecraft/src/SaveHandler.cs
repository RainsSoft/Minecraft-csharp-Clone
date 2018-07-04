using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace net.minecraft.src
{
    using IOPath = System.IO.Path;

	public class SaveHandler : ISaveHandler
	{
		/// <summary>
		/// Reference to the logger. </summary>
		//private static readonly Logger Logger = Logger.getLogger("Minecraft");

		/// <summary>
		/// The path to the current savegame directory </summary>
		private readonly string SaveDirectory;

		/// <summary>
		/// The directory in which to save player information </summary>
        private readonly string PlayersDirectory;
        private readonly string MapDataDir;

		/// <summary>
		/// The time in milliseconds when this field was initialized. Stored in the session lock file.
		/// </summary>
		private readonly long InitializationTime = JavaHelper.CurrentTimeMillis();

		/// <summary>
		/// The directory name of the world </summary>
		private readonly string SaveDirectoryName;

		public SaveHandler(string par1File, string par2Str, bool par3)
		{
            SaveDirectory = IOPath.Combine(par1File, par2Str);
            Directory.CreateDirectory(SaveDirectory);
            PlayersDirectory = IOPath.Combine(SaveDirectory, "players");
            MapDataDir = IOPath.Combine(SaveDirectory, "data");
			Directory.CreateDirectory(MapDataDir);
			SaveDirectoryName = par2Str;

			if (par3)
			{
				Directory.CreateDirectory(PlayersDirectory);
			}

			SetSessionLock();
		}

		/// <summary>
		/// Creates a session lock file for this process
		/// </summary>
		private void SetSessionLock()
		{
			try
			{
                string file = IOPath.Combine(SaveDirectory, "session.lock");
				BinaryWriter dataoutputstream = new BinaryWriter(new FileStream(file, FileMode.Create));

				try
				{
					dataoutputstream.Write(InitializationTime);
				}
				finally
				{
					dataoutputstream.Close();
				}
			}
			catch (IOException ioexception)
			{
				Console.WriteLine(ioexception.ToString());
				Console.Write(ioexception.StackTrace);

				throw new Exception("Failed to check session lock, aborting");
			}
		}

		/// <summary>
		/// gets the File object corresponding to the base directory of this save (saves/404 for a save called 404 etc)
		/// </summary>
		protected virtual string GetSaveDirectory()
		{
			return SaveDirectory;
		}

		/// <summary>
		/// Checks the session lock to prevent save collisions
		/// </summary>
		public virtual void CheckSessionLock()
		{
            try
            {
                string file = IOPath.Combine(SaveDirectory, "session.lock");
                BinaryReader datainputstream = new BinaryReader(new FileStream(file, FileMode.Open));

                try
                {
                    if (datainputstream.ReadInt64() != InitializationTime)
                    {
                        throw new MinecraftException("The save is being accessed from another location, aborting");
                    }
                }
                finally
                {
                    datainputstream.Close();
                }
            }
            catch (IOException ioexception)
            {
                Console.WriteLine(ioexception.ToString());
                Console.WriteLine();

                throw new MinecraftException("Failed to check session lock, aborting");
            }
		}

		/// <summary>
		/// Returns the chunk loader with the provided world provider
		/// </summary>
		public virtual IChunkLoader GetChunkLoader(WorldProvider par1WorldProvider)
		{
			throw new Exception("Old Chunk Storage is no longer supported.");
		}

		/// <summary>
		/// Loads and returns the world info
		/// </summary>
		public virtual WorldInfo LoadWorldInfo()
		{
            string file = IOPath.Combine(SaveDirectory, "level.dat");

			if (File.Exists(file))
			{
				try
				{
					NBTTagCompound nbttagcompound = CompressedStreamTools.ReadCompressed(new FileStream(file, FileMode.Open));
					NBTTagCompound nbttagcompound2 = nbttagcompound.GetCompoundTag("Data");
					return new WorldInfo(nbttagcompound2);
				}
				catch (Exception exception)
				{
					Console.WriteLine(exception.ToString());
					Console.Write(exception.StackTrace);
				}
			}

            file = IOPath.Combine(SaveDirectory, "level.dat_old");

			if (File.Exists(file))
			{
				try
				{
					NBTTagCompound nbttagcompound1 = CompressedStreamTools.ReadCompressed(new FileStream(file, FileMode.Open));
					NBTTagCompound nbttagcompound3 = nbttagcompound1.GetCompoundTag("Data");
					return new WorldInfo(nbttagcompound3);
				}
				catch (Exception exception1)
				{
					Console.WriteLine(exception1.ToString());
					Console.Write(exception1.StackTrace);
				}
			}

			return null;
		}

		/// <summary>
		/// saves level.dat and backs up the existing one to level.dat_old
		/// </summary>
		public virtual void SaveWorldInfoAndPlayer(WorldInfo par1WorldInfo, List<EntityPlayer> par2List)
		{
			NBTTagCompound nbttagcompound = par1WorldInfo.GetNBTTagCompoundWithPlayers(par2List);
			NBTTagCompound nbttagcompound1 = new NBTTagCompound();
			nbttagcompound1.SetTag("Data", nbttagcompound);

			try
			{
                string file = IOPath.Combine(SaveDirectory, "level.dat_new");
                string file1 = IOPath.Combine(SaveDirectory, "level.dat_old");
                string file2 = IOPath.Combine(SaveDirectory, "level.dat");
				CompressedStreamTools.WriteCompressed(nbttagcompound1, new FileStream(file, FileMode.Create));

				if (File.Exists(file1))
				{
					File.Delete(file1);
				}

				File.Move(file2, file1);

				if (File.Exists(file2))
				{
					File.Delete(file2);
				}

				File.Move(file, file2);

				if (File.Exists(file))
				{
					File.Delete(file);
				}
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception.ToString());
				Console.Write(exception.StackTrace);
			}
		}

		/// <summary>
		/// Saves the passed in world info.
		/// </summary>
		public virtual void SaveWorldInfo(WorldInfo par1WorldInfo)
		{
			NBTTagCompound nbttagcompound = par1WorldInfo.GetNBTTagCompound();
			NBTTagCompound nbttagcompound1 = new NBTTagCompound();
			nbttagcompound1.SetTag("Data", nbttagcompound);

			try
			{
                string file = IOPath.Combine(SaveDirectory, "level.dat_new");
                string file1 = IOPath.Combine(SaveDirectory, "level.dat_old");
                string file2 = IOPath.Combine(SaveDirectory, "level.dat");
				CompressedStreamTools.WriteCompressed(nbttagcompound1, new FileStream(file, FileMode.Create));

				if (File.Exists(file1))
				{
					File.Delete(file1);
				}

				File.Move(file2, file1);

				if (File.Exists(file2))
				{
					File.Delete(file2);
				}

				File.Move(file, file2);

				if (File.Exists(file))
				{
					File.Delete(file);
				}
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception.ToString());
				Console.Write(exception.StackTrace);
			}
		}

		/// <summary>
		/// Gets the file location of the given map
		/// </summary>
		public virtual string GetMapFileFromName(string par1Str)
		{
            return IOPath.Combine(MapDataDir, (new StringBuilder()).Append(par1Str).Append(".dat").ToString());
		}

		/// <summary>
		/// Returns the name of the directory where world information is saved
		/// </summary>
		public virtual string GetSaveDirectoryName()
		{
			return SaveDirectoryName;
		}
	}
}