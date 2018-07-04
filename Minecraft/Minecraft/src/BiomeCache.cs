using System.Collections.Generic;

namespace net.minecraft.src
{
	public class BiomeCache
	{
		/// <summary>
		/// Reference to the WorldChunkManager </summary>
		private readonly WorldChunkManager ChunkManager;

		/// <summary>
		/// The last time this BiomeCache was cleaned, in milliseconds. </summary>
		private long LastCleanupTime;

		/// <summary>
		/// The map of keys to BiomeCacheBlocks. Keys are based on the chunk x, z coordinates as (x | z << 32).
		/// </summary>
		private LongHashMap CacheMap;

		/// <summary>
		/// The list of cached BiomeCacheBlocks </summary>
		private List<BiomeCacheBlock> Cache;

		public BiomeCache(WorldChunkManager par1WorldChunkManager)
		{
			LastCleanupTime = 0L;
			CacheMap = new LongHashMap();
            Cache = new List<BiomeCacheBlock>();
			ChunkManager = par1WorldChunkManager;
		}

		/// <summary>
		/// Returns a biome cache block at location specified.
		/// </summary>
		public virtual BiomeCacheBlock GetBiomeCacheBlock(int par1, int par2)
		{
			par1 >>= 4;
			par2 >>= 4;
			long l = (long)par1 & 0xffffffffL | ((long)par2 & 0xffffffffL) << 32;
			BiomeCacheBlock biomecacheblock = (BiomeCacheBlock)CacheMap.GetValueByKey(l);

			if (biomecacheblock == null)
			{
				biomecacheblock = new BiomeCacheBlock(this, par1, par2);
				CacheMap.Add(l, biomecacheblock);
				Cache.Add(biomecacheblock);
			}

			biomecacheblock.LastAccessTime = JavaHelper.CurrentTimeMillis();
			return biomecacheblock;
		}

		/// <summary>
		/// Returns the BiomeGenBase related to the x, z position from the cache.
		/// </summary>
		public virtual BiomeGenBase GetBiomeGenAt(int par1, int par2)
		{
			return GetBiomeCacheBlock(par1, par2).GetBiomeGenAt(par1, par2);
		}

		/// <summary>
		/// Removes BiomeCacheBlocks from this cache that haven't been accessed in at least 30 seconds.
		/// </summary>
		public virtual void CleanupCache()
		{
			long l = JavaHelper.CurrentTimeMillis();
			long l1 = l - LastCleanupTime;

			if (l1 > 7500L || l1 < 0L)
			{
				LastCleanupTime = l;

				for (int i = 0; i < Cache.Count; i++)
				{
					BiomeCacheBlock biomecacheblock = (BiomeCacheBlock)Cache[i];
					long l2 = l - biomecacheblock.LastAccessTime;

					if (l2 > 30000L || l2 < 0L)
					{
						Cache.RemoveAt(i--);
						long l3 = (long)biomecacheblock.XPosition & 0xffffffffL | ((long)biomecacheblock.ZPosition & 0xffffffffL) << 32;
						CacheMap.Remove(l3);
					}
				}
			}
		}

		/// <summary>
		/// Returns the array of cached biome types in the BiomeCacheBlock at the given location.
		/// </summary>
		public virtual BiomeGenBase[] GetCachedBiomes(int par1, int par2)
		{
			return GetBiomeCacheBlock(par1, par2).Biomes;
		}

		/// <summary>
		/// Get the world chunk manager object for a biome list.
		/// </summary>
		public static WorldChunkManager GetChunkManager(BiomeCache par0BiomeCache)
		{
			return par0BiomeCache.ChunkManager;
		}
	}
}