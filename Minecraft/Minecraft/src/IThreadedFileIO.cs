namespace net.minecraft.src
{

	public interface IThreadedFileIO
	{
		/// <summary>
		/// Returns a bool stating if the write was unsuccessful.
		/// </summary>
		bool WriteNextIO();
	}

}