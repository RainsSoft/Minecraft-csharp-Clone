using System.Collections.Generic;

namespace net.minecraft.src
{
	public interface IChunkProvider
	{
		/// <summary>
		/// Checks to see if a chunk exists at x, y
		/// </summary>
		bool ChunkExists(int i, int j);

		/// <summary>
		/// Will return back a chunk, if it doesn't exist and its not a MP client it will generates all the blocks for the
		/// specified chunk from the map seed and chunk seed
		/// </summary>
		Chunk ProvideChunk(int i, int j);

		/// <summary>
		/// loads or generates the chunk at the chunk location specified
		/// </summary>
		Chunk LoadChunk(int i, int j);

		/// <summary>
		/// Populates chunk with ores etc etc
		/// </summary>
		void Populate(IChunkProvider ichunkprovider, int i, int j);

		/// <summary>
		/// Two modes of operation: if passed true, save all Chunks in one go.  If passed false, save up to two chunks.
		/// Return true if all chunks have been saved.
		/// </summary>
		bool SaveChunks(bool flag, IProgressUpdate iprogressupdate);

		/// <summary>
		/// Unloads the 100 oldest chunks from memory, due to a bug with chunkSet.Add() never being called it thinks the list
		/// is always empty and will not remove any chunks.
		/// </summary>
		bool Unload100OldestChunks();

		/// <summary>
		/// Returns if the IChunkProvider supports saving.
		/// </summary>
		bool CanSave();

		/// <summary>
		/// Converts the instance data to a readable string.
		/// </summary>
		string MakeString();

		/// <summary>
		/// Returns a list of creatures of the specified type that can spawn at the given location.
		/// </summary>
		List<SpawnListEntry> GetPossibleCreatures(CreatureType enumcreaturetype, int i, int j, int k);

		/// <summary>
		/// Returns the location of the closest structure of the specified type. If not found returns null.
		/// </summary>
		ChunkPosition FindClosestStructure(World world, string s, int i, int j, int k);
	}
}