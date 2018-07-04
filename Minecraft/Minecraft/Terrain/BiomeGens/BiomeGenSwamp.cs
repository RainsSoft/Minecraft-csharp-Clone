using System;

namespace net.minecraft.src
{
	public class BiomeGenSwamp : BiomeGenBase
	{
		public BiomeGenSwamp(int par1) : base(par1)
		{
			BiomeDecorator.TreesPerChunk = 2;
			BiomeDecorator.FlowersPerChunk = -999;
			BiomeDecorator.DeadBushPerChunk = 1;
			BiomeDecorator.MushroomsPerChunk = 8;
			BiomeDecorator.ReedsPerChunk = 10;
			BiomeDecorator.ClayPerChunk = 1;
			BiomeDecorator.WaterlilyPerChunk = 4;
			WaterColorMultiplier = 0xe0ffae;
		}

		/// <summary>
		/// Gets a WorldGen appropriate for this biome.
		/// </summary>
		public override WorldGenerator GetRandomWorldGenForTrees(Random par1Random)
		{
			return WorldGenSwamp;
		}

		/// <summary>
		/// Provides the basic grass color based on the biome temperature and rainfall
		/// </summary>
		public override int GetBiomeGrassColor()
		{
			double d = GetFloatTemperature();
			double d1 = GetFloatRainfall();
			return ((ColorizerGrass.GetGrassColor(d, d1) & 0xfefefe) + 0x4e0e4e) / 2;
		}

		/// <summary>
		/// Provides the basic foliage color based on the biome temperature and rainfall
		/// </summary>
		public override int GetBiomeFoliageColor()
		{
			double d = GetFloatTemperature();
			double d1 = GetFloatRainfall();
			return ((ColorizerFoliage.GetFoliageColor(d, d1) & 0xfefefe) + 0x4e0e4e) / 2;
		}
	}
}