namespace net.minecraft.src
{

	public interface IProgressUpdate
	{
		/// <summary>
		/// Shows the 'Saving level' string.
		/// </summary>
		void DisplaySavingString(string s);

		/// <summary>
		/// Displays a string on the loading screen supposed to indicate what is being done currently.
		/// </summary>
		void DisplayLoadingString(string s);

		/// <summary>
		/// Updates the progress bar on the loading screen to the specified amount. Args: loadProgress
		/// </summary>
		void SetLoadingProgress(int i);
	}

}