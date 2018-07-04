using System.Collections.Generic;

namespace net.minecraft.src
{
	public class SaveHandlerMP : ISaveHandler
	{
		public SaveHandlerMP()
		{
		}

		/// <summary>
		/// Loads and returns the world info
		/// </summary>
		public virtual WorldInfo LoadWorldInfo()
		{
			return null;
		}

		/// <summary>
		/// Checks the session lock to prevent save collisions
		/// </summary>
		public virtual void CheckSessionLock()
		{
		}

		/// <summary>
		/// Returns the chunk loader with the provided world provider
		/// </summary>
		public virtual IChunkLoader GetChunkLoader(WorldProvider par1WorldProvider)
		{
			return null;
		}

		/// <summary>
		/// saves level.dat and backs up the existing one to level.dat_old
		/// </summary>
		public virtual void SaveWorldInfoAndPlayer(WorldInfo worldinfo, List<EntityPlayer> list)
		{
		}

		/// <summary>
		/// Saves the passed in world info.
		/// </summary>
		public virtual void SaveWorldInfo(WorldInfo worldinfo)
		{
		}

		/// <summary>
		/// Gets the file location of the given map
		/// </summary>
		public virtual string GetMapFileFromName(string par1Str)
		{
			return null;
		}

		/// <summary>
		/// Returns the name of the directory where world information is saved
		/// </summary>
		public virtual string GetSaveDirectoryName()
		{
			return "none";
		}
	}
}