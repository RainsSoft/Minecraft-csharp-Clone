using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class WorldChunkManagerHell : WorldChunkManager
	{
		/// <summary>
		/// The biome generator object. </summary>
		private BiomeGenBase BiomeGenerator;
		private float HellTemperature;

		/// <summary>
		/// The rainfall in the world </summary>
		private float Rainfall;

		public WorldChunkManagerHell(BiomeGenBase par1BiomeGenBase, float par2, float par3)
		{
			BiomeGenerator = par1BiomeGenBase;
			HellTemperature = par2;
			Rainfall = par3;
		}

		/// <summary>
		/// Returns the BiomeGenBase related to the x, z position on the world.
		/// </summary>
		public override BiomeGenBase GetBiomeGenAt(int par1, int par2)
		{
			return BiomeGenerator;
		}

		/// <summary>
		/// Returns an array of biomes for the location input.
		/// </summary>
		public override BiomeGenBase[] GetBiomesForGeneration(BiomeGenBase[] par1ArrayOfBiomeGenBase, int par2, int par3, int par4, int par5)
		{
			if (par1ArrayOfBiomeGenBase == null || par1ArrayOfBiomeGenBase.Length < par4 * par5)
			{
				par1ArrayOfBiomeGenBase = new BiomeGenBase[par4 * par5];
			}

			JavaHelper.FillArray(par1ArrayOfBiomeGenBase, 0, par4 * par5, BiomeGenerator);
			return par1ArrayOfBiomeGenBase;
		}

		/// <summary>
		/// Returns a list of temperatures to use for the specified blocks.  Args: listToReuse, x, y, width, length
		/// </summary>
		public override float[] GetTemperatures(float[] par1ArrayOfFloat, int par2, int par3, int par4, int par5)
		{
			if (par1ArrayOfFloat == null || par1ArrayOfFloat.Length < par4 * par5)
			{
				par1ArrayOfFloat = new float[par4 * par5];
			}

			JavaHelper.FillArray(par1ArrayOfFloat, 0, par4 * par5, HellTemperature);
			return par1ArrayOfFloat;
		}

		/// <summary>
		/// Returns a list of rainfall values for the specified blocks. Args: listToReuse, x, z, width, length.
		/// </summary>
		public override float[] GetRainfall(float[] par1ArrayOfFloat, int par2, int par3, int par4, int par5)
		{
			if (par1ArrayOfFloat == null || par1ArrayOfFloat.Length < par4 * par5)
			{
				par1ArrayOfFloat = new float[par4 * par5];
			}

			JavaHelper.FillArray(par1ArrayOfFloat, 0, par4 * par5, Rainfall);
			return par1ArrayOfFloat;
		}

		/// <summary>
		/// Returns biomes to use for the blocks and loads the other data like temperature and humidity onto the
		/// WorldChunkManager Args: oldBiomeList, x, z, width, depth
		/// </summary>
		public override BiomeGenBase[] LoadBlockGeneratorData(BiomeGenBase[] par1ArrayOfBiomeGenBase, int par2, int par3, int par4, int par5)
		{
			if (par1ArrayOfBiomeGenBase == null || par1ArrayOfBiomeGenBase.Length < par4 * par5)
			{
				par1ArrayOfBiomeGenBase = new BiomeGenBase[par4 * par5];
			}

			JavaHelper.FillArray(par1ArrayOfBiomeGenBase, 0, par4 * par5, BiomeGenerator);
			return par1ArrayOfBiomeGenBase;
		}

		/// <summary>
		/// Return a list of biomes for the specified blocks. Args: listToReuse, x, y, width, length, cacheFlag (if false,
		/// don't check biomeCache to avoid infinite loop in BiomeCacheBlock)
		/// </summary>
		public override BiomeGenBase[] GetBiomeGenAt(BiomeGenBase[] par1ArrayOfBiomeGenBase, int par2, int par3, int par4, int par5, bool par6)
		{
			return LoadBlockGeneratorData(par1ArrayOfBiomeGenBase, par2, par3, par4, par5);
		}

		/// <summary>
		/// Finds a valid position within a range, that is once of the listed biomes.
		/// </summary>
		public override ChunkPosition FindBiomePosition(int par1, int par2, int par3, List<BiomeGenBase> par4List, Random par5Random)
		{
			if (par4List.Contains(BiomeGenerator))
			{
				return new ChunkPosition((par1 - par3) + par5Random.Next(par3 * 2 + 1), 0, (par2 - par3) + par5Random.Next(par3 * 2 + 1));
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// checks given Chunk's Biomes against List of allowed ones
		/// </summary>
		public override bool AreBiomesViable(int par1, int par2, int par3, List<BiomeGenBase> par4List)
		{
			return par4List.Contains(BiomeGenerator);
		}
	}
}