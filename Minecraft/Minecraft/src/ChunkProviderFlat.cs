using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class ChunkProviderFlat : IChunkProvider
	{
		private World WorldObj;
		private Random Random;
		private readonly bool UseStructures;
		private MapGenVillage VillageGen;

		public ChunkProviderFlat(World par1World, long par2, bool par4)
		{
			VillageGen = new MapGenVillage(1);
			WorldObj = par1World;
			UseStructures = par4;
			Random = new Random((int)par2);
		}

		private void Generate(byte[] par1ArrayOfByte)
		{
			int i = par1ArrayOfByte.Length / 256;

			for (int j = 0; j < 16; j++)
			{
				for (int k = 0; k < 16; k++)
				{
					for (int l = 0; l < i; l++)
					{
						int i1 = 0;

						if (l == 0)
						{
							i1 = Block.Bedrock.BlockID;
						}
						else if (l <= 2)
						{
							i1 = Block.Dirt.BlockID;
						}
						else if (l == 3)
						{
							i1 = Block.Grass.BlockID;
						}

						par1ArrayOfByte[j << 11 | k << 7 | l] = (byte)i1;
					}
				}
			}
		}

		/// <summary>
		/// loads or generates the chunk at the chunk location specified
		/// </summary>
		public Chunk LoadChunk(int par1, int par2)
		{
			return ProvideChunk(par1, par2);
		}

		/// <summary>
		/// Will return back a chunk, if it doesn't exist and its not a MP client it will generates all the blocks for the
		/// specified chunk from the map seed and chunk seed
		/// </summary>
		public Chunk ProvideChunk(int par1, int par2)
		{
			byte[] abyte0 = new byte[32768];
			Generate(abyte0);
			Chunk chunk = new Chunk(WorldObj, abyte0, par1, par2);

			if (UseStructures)
			{
				VillageGen.Generate(this, WorldObj, par1, par2, abyte0);
			}

			BiomeGenBase[] abiomegenbase = WorldObj.GetWorldChunkManager().LoadBlockGeneratorData(null, par1 * 16, par2 * 16, 16, 16);
			byte[] abyte1 = chunk.GetBiomeArray();

			for (int i = 0; i < abyte1.Length; i++)
			{
				abyte1[i] = (byte)abiomegenbase[i].BiomeID;
			}

			chunk.GenerateSkylightMap();
			return chunk;
		}

		/// <summary>
		/// Checks to see if a chunk exists at x, y
		/// </summary>
		public bool ChunkExists(int par1, int par2)
		{
			return true;
		}

		/// <summary>
		/// Populates chunk with ores etc etc
		/// </summary>
		public void Populate(IChunkProvider par1IChunkProvider, int par2, int par3)
		{
            Random.SetSeed((int)WorldObj.GetSeed());
			long l = (Random.Next() / 2L) * 2L + 1L;
			long l1 = (Random.Next() / 2L) * 2L + 1L;
            Random.SetSeed(par2 * (int)l + par3 * (int)l1 ^ (int)WorldObj.GetSeed());

			if (UseStructures)
			{
				VillageGen.GenerateStructuresInChunk(WorldObj, Random, par2, par3);
			}
		}

		/// <summary>
		/// Two modes of operation: if passed true, save all Chunks in one go.  If passed false, save up to two chunks.
		/// Return true if all chunks have been saved.
		/// </summary>
        public bool SaveChunks(bool par1, IProgressUpdate par2IProgressUpdate)
		{
			return true;
		}

		/// <summary>
		/// Unloads the 100 oldest chunks from memory, due to a bug with chunkSet.Add() never being called it thinks the list
		/// is always empty and will not remove any chunks.
		/// </summary>
        public bool Unload100OldestChunks()
		{
			return false;
		}

		/// <summary>
		/// Returns if the IChunkProvider supports saving.
		/// </summary>
        public bool CanSave()
		{
			return true;
		}

		/// <summary>
		/// Converts the instance data to a readable string.
		/// </summary>
        public string MakeString()
		{
			return "FlatLevelSource";
		}

		/// <summary>
		/// Returns a list of creatures of the specified type that can spawn at the given location.
		/// </summary>
        public List<SpawnListEntry> GetPossibleCreatures(CreatureType par1EnumCreatureType, int par2, int par3, int par4)
		{
			BiomeGenBase biomegenbase = WorldObj.GetBiomeGenForCoords(par2, par4);

			if (biomegenbase == null)
			{
				return null;
			}
			else
			{
				return biomegenbase.GetSpawnableList(par1EnumCreatureType);
			}
		}

		/// <summary>
		/// Returns the location of the closest structure of the specified type. If not found returns null.
		/// </summary>
        public ChunkPosition FindClosestStructure(World par1World, string par2Str, int par3, int i, int j)
		{
			return null;
		}
	}
}