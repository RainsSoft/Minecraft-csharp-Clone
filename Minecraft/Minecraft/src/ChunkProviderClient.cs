using System.Collections.Generic;
using System.Text;

namespace net.minecraft.src
{
	public class ChunkProviderClient : IChunkProvider
	{
		/// <summary>
		/// The completely empty chunk used by ChunkProviderClient when chunkMapping doesn't contain the requested
		/// coordinates.
		/// </summary>
		private Chunk BlankChunk;

		/// <summary>
		/// The mapping between ChunkCoordinates and Chunks that ChunkProviderClient maintains.
		/// </summary>
		private LongHashMap ChunkMapping;
		private List<Chunk> Field_889_c;

		/// <summary>
		/// Reference to the World object. </summary>
		private World WorldObj;

		public ChunkProviderClient(World par1World)
		{
			ChunkMapping = new LongHashMap();
            Field_889_c = new List<Chunk>();
			BlankChunk = new EmptyChunk(par1World, 0, 0);
			WorldObj = par1World;
		}

		/// <summary>
		/// Checks to see if a chunk exists at x, y
		/// </summary>
		public virtual bool ChunkExists(int par1, int par2)
		{
			return true;
		}

		public virtual void Func_539_c(int par1, int par2)
		{
			Chunk chunk = ProvideChunk(par1, par2);

			if (!chunk.IsEmpty())
			{
				chunk.OnChunkUnload();
			}

			ChunkMapping.Remove(ChunkCoordIntPair.ChunkXZ2Int(par1, par2));
			Field_889_c.Remove(chunk);
		}

		/// <summary>
		/// loads or generates the chunk at the chunk location specified
		/// </summary>
		public virtual Chunk LoadChunk(int par1, int par2)
		{
			Chunk chunk = new Chunk(WorldObj, par1, par2);
			ChunkMapping.Add(ChunkCoordIntPair.ChunkXZ2Int(par1, par2), chunk);
			chunk.IsChunkLoaded = true;
			return chunk;
		}

		/// <summary>
		/// Will return back a chunk, if it doesn't exist and its not a MP client it will generates all the blocks for the
		/// specified chunk from the map seed and chunk seed
		/// </summary>
		public virtual Chunk ProvideChunk(int par1, int par2)
		{
			Chunk chunk = (Chunk)ChunkMapping.GetValueByKey(ChunkCoordIntPair.ChunkXZ2Int(par1, par2));

			if (chunk == null)
			{
				return BlankChunk;
			}
			else
			{
				return chunk;
			}
		}

		/// <summary>
		/// Two modes of operation: if passed true, save all Chunks in one go.  If passed false, save up to two chunks.
		/// Return true if all chunks have been saved.
		/// </summary>
		public virtual bool SaveChunks(bool par1, IProgressUpdate par2IProgressUpdate)
		{
			return true;
		}

		/// <summary>
		/// Unloads the 100 oldest chunks from memory, due to a bug with chunkSet.Add() never being called it thinks the list
		/// is always empty and will not remove any chunks.
		/// </summary>
		public virtual bool Unload100OldestChunks()
		{
			return false;
		}

		/// <summary>
		/// Returns if the IChunkProvider supports saving.
		/// </summary>
		public virtual bool CanSave()
		{
			return false;
		}

		/// <summary>
		/// Populates chunk with ores etc etc
		/// </summary>
		public virtual void Populate(IChunkProvider ichunkprovider, int i, int j)
		{
		}

		/// <summary>
		/// Converts the instance data to a readable string.
		/// </summary>
		public virtual string MakeString()
		{
			return (new StringBuilder()).Append("MultiplayerChunkCache: ").Append(ChunkMapping.GetNumHashElements()).ToString();
		}

		/// <summary>
		/// Returns a list of creatures of the specified type that can spawn at the given location.
		/// </summary>
		public virtual List<SpawnListEntry> GetPossibleCreatures(CreatureType par1EnumCreatureType, int par2, int par3, int i)
		{
			return null;
		}

		/// <summary>
		/// Returns the location of the closest structure of the specified type. If not found returns null.
		/// </summary>
		public virtual ChunkPosition FindClosestStructure(World par1World, string par2Str, int par3, int i, int j)
		{
			return null;
		}
	}
}