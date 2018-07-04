namespace net.minecraft.src
{
	public interface IChunkLoader
	{
		/// <summary>
		/// Loads the specified(XZ) chunk into the specified world.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public abstract Chunk loadChunk(World world, int i, int j) throws java.io.IOException;
		Chunk LoadChunk(World world, int i, int j);

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public abstract void saveChunk(World world, Chunk chunk) throws java.io.IOException;
		void SaveChunk(World world, Chunk chunk);

		/// <summary>
		/// Save extra data associated with this Chunk not normally saved during autosave, only during chunk unload.
		/// Currently unused.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public abstract void saveExtraChunkData(World world, Chunk chunk) throws java.io.IOException;
		void SaveExtraChunkData(World world, Chunk chunk);

		/// <summary>
		/// Called every World.tick()
		/// </summary>
		void ChunkTick();

		/// <summary>
		/// Save extra data not associated with any Chunk.  Not saved during autosave, only during world unload.  Currently
		/// unused.
		/// </summary>
		void SaveExtraData();
	}
}