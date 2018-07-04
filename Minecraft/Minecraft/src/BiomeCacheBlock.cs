namespace net.minecraft.src
{

	public class BiomeCacheBlock
	{
		public float[] TemperatureValues;
		public float[] RainfallValues;
		public BiomeGenBase[] Biomes;

		/// <summary>
		/// The x coordinate of the BiomeCacheBlock. </summary>
		public int XPosition;

		/// <summary>
		/// The z coordinate of the BiomeCacheBlock. </summary>
		public int ZPosition;

		/// <summary>
		/// The last time this BiomeCacheBlock was accessed, in milliseconds. </summary>
		public long LastAccessTime;

		/// <summary>
		/// The BiomeCache objevt that Contains this BiomeCacheBlock </summary>
		readonly BiomeCache BiomeCache;

		public BiomeCacheBlock(BiomeCache par1BiomeCache, int par2, int par3)
		{
			BiomeCache = par1BiomeCache;
			TemperatureValues = new float[256];
			RainfallValues = new float[256];
			Biomes = new BiomeGenBase[256];
			XPosition = par2;
			ZPosition = par3;
			BiomeCache.GetChunkManager(par1BiomeCache).GetTemperatures(TemperatureValues, par2 << 4, par3 << 4, 16, 16);
			BiomeCache.GetChunkManager(par1BiomeCache).GetRainfall(RainfallValues, par2 << 4, par3 << 4, 16, 16);
			BiomeCache.GetChunkManager(par1BiomeCache).GetBiomeGenAt(Biomes, par2 << 4, par3 << 4, 16, 16, false);
		}

		/// <summary>
		/// Returns the BiomeGenBase related to the x, z position from the cache block.
		/// </summary>
		public virtual BiomeGenBase GetBiomeGenAt(int par1, int par2)
		{
			return Biomes[par1 & 0xf | (par2 & 0xf) << 4];
		}
	}
}