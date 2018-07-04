using System.Collections.Generic;

namespace net.minecraft.src
{
	public interface ISaveFormat
	{
		string GetFormatName();

		/// <summary>
		/// Returns back a loader for the specified save directory
		/// </summary>
		ISaveHandler GetSaveLoader(string s, bool flag);

		List<SaveFormatComparator> GetSaveList();

		void FlushCache();

		/// <summary>
		/// gets the world info
		/// </summary>
		WorldInfo GetWorldInfo(string s);

		/// <summary>
		/// @args: Takes one argument - the name of the directory of the world to delete. @desc: Delete the world by deleting
		/// the associated directory recursively.
		/// </summary>
		void DeleteWorldDirectory(string s);

		/// <summary>
		/// @args: Takes two arguments - first the name of the directory containing the world and second the new name for
		/// that world. @desc: Renames the world by storing the new name in level.dat. It does *not* rename the directory
		/// containing the world data.
		/// </summary>
		void RenameWorld(string s, string s1);

		/// <summary>
		/// Checks if the save directory uses the old map format
		/// </summary>
		bool IsOldMapFormat(string s);

		/// <summary>
		/// Converts the specified map to the new map format. Args: worldName, loadingScreen
		/// </summary>
		bool ConvertMapFormat(string s, IProgressUpdate iprogressupdate);
	}
}