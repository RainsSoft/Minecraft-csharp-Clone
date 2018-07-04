using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class WorldChunkManager
	{
		private GenLayer GenBiomes;

		/// <summary>
		/// A GenLayer containing the indices into BiomeGenBase.biomeList[] </summary>
		private GenLayer BiomeIndexLayer;

		/// <summary>
		/// The BiomeCache object for this world. </summary>
		private BiomeCache BiomeCache;

		/// <summary>
		/// A list of biomes that the player can spawn in. </summary>
        private List<BiomeGenBase> BiomesToSpawnIn;

		protected WorldChunkManager()
		{
			BiomeCache = new BiomeCache(this);
			BiomesToSpawnIn = new List<BiomeGenBase>();
			BiomesToSpawnIn.Add(BiomeGenBase.Forest);
			BiomesToSpawnIn.Add(BiomeGenBase.Plains);
			BiomesToSpawnIn.Add(BiomeGenBase.Taiga);
			BiomesToSpawnIn.Add(BiomeGenBase.TaigaHills);
			BiomesToSpawnIn.Add(BiomeGenBase.ForestHills);
			BiomesToSpawnIn.Add(BiomeGenBase.Jungle);
			BiomesToSpawnIn.Add(BiomeGenBase.JungleHills);
		}

		public WorldChunkManager(long par1, WorldType par3WorldType) : this()
		{
			GenLayer[] agenlayer = GenLayer.Func_48425_a(par1, par3WorldType);
			GenBiomes = agenlayer[0];
			BiomeIndexLayer = agenlayer[1];
		}

		public WorldChunkManager(World par1World)
            : this(par1World.GetSeed(), par1World.GetWorldInfo().GetTerrainType())
		{
		}

		/// <summary>
		/// Gets the list of valid biomes for the player to spawn in.
		/// </summary>
        public virtual List<BiomeGenBase> GetBiomesToSpawnIn()
		{
			return BiomesToSpawnIn;
		}

		/// <summary>
		/// Returns the BiomeGenBase related to the x, z position on the world.
		/// </summary>
		public virtual BiomeGenBase GetBiomeGenAt(int par1, int par2)
		{
			return BiomeCache.GetBiomeGenAt(par1, par2);
		}

		/// <summary>
		/// Returns a list of rainfall values for the specified blocks. Args: listToReuse, x, z, width, length.
		/// </summary>
		public virtual float[] GetRainfall(float[] par1ArrayOfFloat, int par2, int par3, int par4, int par5)
		{
			IntCache.ResetIntCache();

			if (par1ArrayOfFloat == null || par1ArrayOfFloat.Length < par4 * par5)
			{
				par1ArrayOfFloat = new float[par4 * par5];
			}

			int[] ai = BiomeIndexLayer.GetInts(par2, par3, par4, par5);

			for (int i = 0; i < par4 * par5; i++)
			{
				float f = (float)BiomeGenBase.BiomeList[ai[i]].GetIntRainfall() / 65536F;

				if (f > 1.0F)
				{
					f = 1.0F;
				}

				par1ArrayOfFloat[i] = f;
			}

			return par1ArrayOfFloat;
		}

		/// <summary>
		/// Return an adjusted version of a given temperature based on the y height
		/// </summary>
		public virtual float GetTemperatureAtHeight(float par1, int par2)
		{
			return par1;
		}

		/// <summary>
		/// Returns a list of temperatures to use for the specified blocks.  Args: listToReuse, x, y, width, length
		/// </summary>
		public virtual float[] GetTemperatures(float[] par1ArrayOfFloat, int par2, int par3, int par4, int par5)
		{
			IntCache.ResetIntCache();

			if (par1ArrayOfFloat == null || par1ArrayOfFloat.Length < par4 * par5)
			{
				par1ArrayOfFloat = new float[par4 * par5];
			}

			int[] ai = BiomeIndexLayer.GetInts(par2, par3, par4, par5);

			for (int i = 0; i < par4 * par5; i++)
			{
				float f = (float)BiomeGenBase.BiomeList[ai[i]].GetIntTemperature() / 65536F;

				if (f > 1.0F)
				{
					f = 1.0F;
				}

				par1ArrayOfFloat[i] = f;
			}

			return par1ArrayOfFloat;
		}

		/// <summary>
		/// Returns an array of biomes for the location input.
		/// </summary>
		public virtual BiomeGenBase[] GetBiomesForGeneration(BiomeGenBase[] par1ArrayOfBiomeGenBase, int par2, int par3, int par4, int par5)
		{
			IntCache.ResetIntCache();

			if (par1ArrayOfBiomeGenBase == null || par1ArrayOfBiomeGenBase.Length < par4 * par5)
			{
				par1ArrayOfBiomeGenBase = new BiomeGenBase[par4 * par5];
			}

			int[] ai = GenBiomes.GetInts(par2, par3, par4, par5);

			for (int i = 0; i < par4 * par5; i++)
			{
				par1ArrayOfBiomeGenBase[i] = BiomeGenBase.BiomeList[ai[i]];
			}

			return par1ArrayOfBiomeGenBase;
		}

		/// <summary>
		/// Returns biomes to use for the blocks and loads the other data like temperature and humidity onto the
		/// WorldChunkManager Args: oldBiomeList, x, z, width, depth
		/// </summary>
		public virtual BiomeGenBase[] LoadBlockGeneratorData(BiomeGenBase[] par1ArrayOfBiomeGenBase, int par2, int par3, int par4, int par5)
		{
			return GetBiomeGenAt(par1ArrayOfBiomeGenBase, par2, par3, par4, par5, true);
		}

		/// <summary>
		/// Return a list of biomes for the specified blocks. Args: listToReuse, x, y, width, length, cacheFlag (if false,
		/// don't check biomeCache to avoid infinite loop in BiomeCacheBlock)
		/// </summary>
		public virtual BiomeGenBase[] GetBiomeGenAt(BiomeGenBase[] par1ArrayOfBiomeGenBase, int par2, int par3, int par4, int par5, bool par6)
		{
			IntCache.ResetIntCache();

			if (par1ArrayOfBiomeGenBase == null || par1ArrayOfBiomeGenBase.Length < par4 * par5)
			{
				par1ArrayOfBiomeGenBase = new BiomeGenBase[par4 * par5];
			}

			if (par6 && par4 == 16 && par5 == 16 && (par2 & 0xf) == 0 && (par3 & 0xf) == 0)
			{
				BiomeGenBase[] abiomegenbase = BiomeCache.GetCachedBiomes(par2, par3);
				Array.Copy(abiomegenbase, 0, par1ArrayOfBiomeGenBase, 0, par4 * par5);
				return par1ArrayOfBiomeGenBase;
			}

			int[] ai = BiomeIndexLayer.GetInts(par2, par3, par4, par5);

			for (int i = 0; i < par4 * par5; i++)
			{
				par1ArrayOfBiomeGenBase[i] = BiomeGenBase.BiomeList[ai[i]];
			}

			return par1ArrayOfBiomeGenBase;
		}

		/// <summary>
		/// checks given Chunk's Biomes against List of allowed ones
		/// </summary>
		public virtual bool AreBiomesViable(int par1, int par2, int par3, List<BiomeGenBase> par4List)
		{
			int i = par1 - par3 >> 2;
			int j = par2 - par3 >> 2;
			int k = par1 + par3 >> 2;
			int l = par2 + par3 >> 2;
			int i1 = (k - i) + 1;
			int j1 = (l - j) + 1;
			int[] ai = GenBiomes.GetInts(i, j, i1, j1);

			for (int k1 = 0; k1 < i1 * j1; k1++)
			{
				BiomeGenBase biomegenbase = BiomeGenBase.BiomeList[ai[k1]];

				if (!par4List.Contains(biomegenbase))
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Finds a valid position within a range, that is once of the listed biomes.
		/// </summary>
        public virtual ChunkPosition FindBiomePosition(int par1, int par2, int par3, List<BiomeGenBase> par4List, Random par5Random)
		{
			int i = par1 - par3 >> 2;
			int j = par2 - par3 >> 2;
			int k = par1 + par3 >> 2;
			int l = par2 + par3 >> 2;
			int i1 = (k - i) + 1;
			int j1 = (l - j) + 1;
			int[] ai = GenBiomes.GetInts(i, j, i1, j1);
			ChunkPosition chunkposition = null;
			int k1 = 0;

			for (int l1 = 0; l1 < ai.Length; l1++)
			{
				int i2 = i + l1 % i1 << 2;
				int j2 = j + l1 / i1 << 2;
				BiomeGenBase biomegenbase = BiomeGenBase.BiomeList[ai[l1]];

				if (par4List.Contains(biomegenbase) && (chunkposition == null || par5Random.Next(k1 + 1) == 0))
				{
					chunkposition = new ChunkPosition(i2, 0, j2);
					k1++;
				}
			}

			return chunkposition;
		}

		/// <summary>
		/// Calls the WorldChunkManager's biomeCache.cleanupCache()
		/// </summary>
		public virtual void CleanupCache()
		{
			BiomeCache.CleanupCache();
		}
	}
}