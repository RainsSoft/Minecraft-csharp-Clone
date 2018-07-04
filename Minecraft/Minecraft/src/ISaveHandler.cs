using System.Collections.Generic;

namespace net.minecraft.src
{
	public interface ISaveHandler
	{
		/// <summary>
		/// Loads and returns the world info
		/// </summary>
		WorldInfo LoadWorldInfo();

		/// <summary>
		/// Checks the session lock to prevent save collisions
		/// </summary>
		void CheckSessionLock();

		/// <summary>
		/// Returns the chunk loader with the provided world provider
		/// </summary>
		IChunkLoader GetChunkLoader(WorldProvider worldprovider);

		/// <summary>
		/// saves level.dat and backs up the existing one to level.dat_old
		/// </summary>
		void SaveWorldInfoAndPlayer(WorldInfo worldinfo, List<EntityPlayer> list);

		/// <summary>
		/// Saves the passed in world info.
		/// </summary>
		void SaveWorldInfo(WorldInfo worldinfo);

		/// <summary>
		/// Gets the file location of the given map
		/// </summary>
		string GetMapFileFromName(string s);

		/// <summary>
		/// Returns the name of the directory where world information is saved
		/// </summary>
		string GetSaveDirectoryName();
	}
}