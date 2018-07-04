using System;

namespace net.minecraft.src
{
	public class BiomeGenForest : BiomeGenBase
	{
		public BiomeGenForest(int par1) : base(par1)
		{
			SpawnableCreatureList.Add(new SpawnListEntry(typeof(net.minecraft.src.EntityWolf), 5, 4, 4));
			BiomeDecorator.TreesPerChunk = 10;
			BiomeDecorator.GrassPerChunk = 2;
		}

		/// <summary>
		/// Gets a WorldGen appropriate for this biome.
		/// </summary>
		public override WorldGenerator GetRandomWorldGenForTrees(Random par1Random)
		{
			if (par1Random.Next(5) == 0)
			{
				return WorldGenForest;
			}

			if (par1Random.Next(10) == 0)
			{
				return WorldGenBigTree;
			}
			else
			{
				return WorldGenTrees;
			}
		}
	}

}