using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace net.minecraft.src
{
	public class ChunkProviderLoadOrGenerate : IChunkProvider
	{
		/// <summary>
		/// A completely empty Chunk, used by ChunkProviderLoadOrGenerate when there's no ChunkProvider.
		/// </summary>
		private Chunk BlankChunk;

		/// <summary>
		/// The parent IChunkProvider for this ChunkProviderLoadOrGenerate. </summary>
		private IChunkProvider ChunkProvider;

		/// <summary>
		/// The IChunkLoader used by this ChunkProviderLoadOrGenerate. </summary>
		private IChunkLoader ChunkLoader;
		private Chunk[] Chunks;

		/// <summary>
		/// Reference to the World object. </summary>
		private World WorldObj;

		/// <summary>
		/// The last X position of a chunk that was returned from setRecordPlayingMessage </summary>
		int LastQueriedChunkXPos;

		/// <summary>
		/// The last Z position of a chunk that was returned from setRecordPlayingMessage </summary>
		int LastQueriedChunkZPos;

		/// <summary>
		/// The last Chunk that was returned from setRecordPlayingMessage </summary>
		private Chunk LastQueriedChunk;

		/// <summary>
		/// The current chunk the player is over </summary>
		private int CurChunkX;

		/// <summary>
		/// The current chunk the player is over </summary>
		private int CurChunkY;

		/// <summary>
		/// This is the chunk that the player is currently standing over. Args: chunkX, chunkZ
		/// </summary>
		public virtual void SetCurrentChunkOver(int par1, int par2)
		{
			CurChunkX = par1;
			CurChunkY = par2;
		}

		/// <summary>
		/// Checks if the chunk coordinate could actually be stored within the chunk cache. Args: chunkX, chunkZ
		/// </summary>
		public virtual bool CanChunkExist(int par1, int par2)
		{
			sbyte byte0 = 15;
			return par1 >= CurChunkX - byte0 && par2 >= CurChunkY - byte0 && par1 <= CurChunkX + byte0 && par2 <= CurChunkY + byte0;
		}

		/// <summary>
		/// Checks to see if a chunk exists at x, y
		/// </summary>
		public virtual bool ChunkExists(int par1, int par2)
		{
			if (!CanChunkExist(par1, par2))
			{
				return false;
			}

			if (par1 == LastQueriedChunkXPos && par2 == LastQueriedChunkZPos && LastQueriedChunk != null)
			{
				return true;
			}
			else
			{
				int i = par1 & 0x1f;
				int j = par2 & 0x1f;
				int k = i + j * 32;
				return Chunks[k] != null && (Chunks[k] == BlankChunk || Chunks[k].IsAtLocation(par1, par2));
			}
		}

		/// <summary>
		/// loads or generates the chunk at the chunk location specified
		/// </summary>
		public virtual Chunk LoadChunk(int par1, int par2)
		{
			return ProvideChunk(par1, par2);
		}

		/// <summary>
		/// Will return back a chunk, if it doesn't exist and its not a MP client it will generates all the blocks for the
		/// specified chunk from the map seed and chunk seed
		/// </summary>
		public virtual Chunk ProvideChunk(int par1, int par2)
		{
			if (par1 == LastQueriedChunkXPos && par2 == LastQueriedChunkZPos && LastQueriedChunk != null)
			{
				return LastQueriedChunk;
			}

			if (!WorldObj.FindingSpawnPoint && !CanChunkExist(par1, par2))
			{
				return BlankChunk;
			}

			int i = par1 & 0x1f;
			int j = par2 & 0x1f;
			int k = i + j * 32;

			if (!ChunkExists(par1, par2))
			{
				if (Chunks[k] != null)
				{
					Chunks[k].OnChunkUnload();
					SaveChunk(Chunks[k]);
					SaveExtraChunkData(Chunks[k]);
				}

				Chunk chunk = Func_542_c(par1, par2);

				if (chunk == null)
				{
					if (ChunkProvider == null)
					{
						chunk = BlankChunk;
					}
					else
					{
						chunk = ChunkProvider.ProvideChunk(par1, par2);
						chunk.RemoveUnknownBlocks();
					}
				}

				Chunks[k] = chunk;
				chunk.Func_4143_d();

				if (Chunks[k] != null)
				{
					Chunks[k].OnChunkLoad();
				}

				Chunks[k].PopulateChunk(this, this, par1, par2);
			}

			LastQueriedChunkXPos = par1;
			LastQueriedChunkZPos = par2;
			LastQueriedChunk = Chunks[k];
			return Chunks[k];
		}

		private Chunk Func_542_c(int par1, int par2)
		{
			if (ChunkLoader == null)
			{
				return BlankChunk;
			}

			try
			{
				Chunk chunk = ChunkLoader.LoadChunk(WorldObj, par1, par2);

				if (chunk != null)
				{
					chunk.LastSaveTime = WorldObj.GetWorldTime();
				}

				return chunk;
			}
			catch (Exception exception)
			{
                Utilities.LogException(exception);
			}

			return BlankChunk;
		}

		/// <summary>
		/// Save extra data associated with this Chunk not normally saved during autosave, only during chunk unload.
		/// Currently unused.
		/// </summary>
		private void SaveExtraChunkData(Chunk par1Chunk)
		{
			if (ChunkLoader == null)
			{
				return;
			}

			try
			{
				ChunkLoader.SaveExtraChunkData(WorldObj, par1Chunk);
			}
			catch (Exception exception)
            {
                Utilities.LogException(exception);
			}
		}

		/// <summary>
		/// Save a given Chunk, recording the time in lastSaveTime
		/// </summary>
		private void SaveChunk(Chunk par1Chunk)
		{
			if (ChunkLoader == null)
			{
				return;
			}

			try
			{
				par1Chunk.LastSaveTime = WorldObj.GetWorldTime();
				ChunkLoader.SaveChunk(WorldObj, par1Chunk);
			}
			catch (IOException ioexception)
            {
                Utilities.LogException(ioexception);
			}
		}

		/// <summary>
		/// Populates chunk with ores etc etc
		/// </summary>
		public virtual void Populate(IChunkProvider par1IChunkProvider, int par2, int par3)
		{
			Chunk chunk = ProvideChunk(par2, par3);

			if (!chunk.IsTerrainPopulated)
			{
				chunk.IsTerrainPopulated = true;

				if (ChunkProvider != null)
				{
					ChunkProvider.Populate(par1IChunkProvider, par2, par3);
					chunk.SetChunkModified();
				}
			}
		}

		/// <summary>
		/// Two modes of operation: if passed true, save all Chunks in one go.  If passed false, save up to two chunks.
		/// Return true if all chunks have been saved.
		/// </summary>
		public virtual bool SaveChunks(bool par1, IProgressUpdate par2IProgressUpdate)
		{
			int i = 0;
			int j = 0;

			if (par2IProgressUpdate != null)
			{
				for (int k = 0; k < Chunks.Length; k++)
				{
					if (Chunks[k] != null && Chunks[k].NeedsSaving(par1))
					{
						j++;
					}
				}
			}

			int l = 0;

			for (int i1 = 0; i1 < Chunks.Length; i1++)
			{
				if (Chunks[i1] == null)
				{
					continue;
				}

				if (par1)
				{
					SaveExtraChunkData(Chunks[i1]);
				}

				if (!Chunks[i1].NeedsSaving(par1))
				{
					continue;
				}

				SaveChunk(Chunks[i1]);
				Chunks[i1].IsModified = false;

				if (++i == 2 && !par1)
				{
					return false;
				}

				if (par2IProgressUpdate != null && ++l % 10 == 0)
				{
					par2IProgressUpdate.SetLoadingProgress((l * 100) / j);
				}
			}

			if (par1)
			{
				if (ChunkLoader == null)
				{
					return true;
				}

				ChunkLoader.SaveExtraData();
			}

			return true;
		}

		/// <summary>
		/// Unloads the 100 oldest chunks from memory, due to a bug with chunkSet.Add() never being called it thinks the list
		/// is always empty and will not remove any chunks.
		/// </summary>
		public virtual bool Unload100OldestChunks()
		{
			if (ChunkLoader != null)
			{
				ChunkLoader.ChunkTick();
			}

			return ChunkProvider.Unload100OldestChunks();
		}

		/// <summary>
		/// Returns if the IChunkProvider supports saving.
		/// </summary>
		public virtual bool CanSave()
		{
			return true;
		}

		/// <summary>
		/// Converts the instance data to a readable string.
		/// </summary>
		public virtual string MakeString()
		{
			return (new StringBuilder()).Append("ChunkCache: ").Append(Chunks.Length).ToString();
		}

		/// <summary>
		/// Returns a list of creatures of the specified type that can spawn at the given location.
		/// </summary>
		public virtual List<SpawnListEntry> GetPossibleCreatures(CreatureType par1EnumCreatureType, int par2, int par3, int par4)
		{
			return ChunkProvider.GetPossibleCreatures(par1EnumCreatureType, par2, par3, par4);
		}

		/// <summary>
		/// Returns the location of the closest structure of the specified type. If not found returns null.
		/// </summary>
		public virtual ChunkPosition FindClosestStructure(World par1World, string par2Str, int par3, int par4, int par5)
		{
			return ChunkProvider.FindClosestStructure(par1World, par2Str, par3, par4, par5);
		}
	}
}