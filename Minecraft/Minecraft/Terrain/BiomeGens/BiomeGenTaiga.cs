using System;

namespace net.minecraft.src
{
	public class BiomeGenTaiga : BiomeGenBase
	{
		public BiomeGenTaiga(int par1) : base(par1)
		{
			SpawnableCreatureList.Add(new SpawnListEntry(typeof(net.minecraft.src.EntityWolf), 8, 4, 4));
			BiomeDecorator.TreesPerChunk = 10;
			BiomeDecorator.GrassPerChunk = 1;
		}

		/// <summary>
		/// Gets a WorldGen appropriate for this biome.
		/// </summary>
		public override WorldGenerator GetRandomWorldGenForTrees(Random par1Random)
		{
			if (par1Random.Next(3) == 0)
			{
				return new WorldGenTaiga1();
			}
			else
			{
				return new WorldGenTaiga2(false);
			}
		}
	}

}