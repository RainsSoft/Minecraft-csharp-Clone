using System;

namespace net.minecraft.src
{
	public class BiomeGenJungle : BiomeGenBase
	{
		public BiomeGenJungle(int par1) : base(par1)
		{
			BiomeDecorator.TreesPerChunk = 50;
			BiomeDecorator.GrassPerChunk = 25;
			BiomeDecorator.FlowersPerChunk = 4;
			SpawnableMonsterList.Add(new SpawnListEntry(typeof(net.minecraft.src.EntityOcelot), 2, 1, 1));
			SpawnableCreatureList.Add(new SpawnListEntry(typeof(net.minecraft.src.EntityChicken), 10, 4, 4));
		}

		/// <summary>
		/// Gets a WorldGen appropriate for this biome.
		/// </summary>
		public override WorldGenerator GetRandomWorldGenForTrees(Random par1Random)
		{
			if (par1Random.Next(10) == 0)
			{
				return WorldGenBigTree;
			}

			if (par1Random.Next(2) == 0)
			{
				return new WorldGenShrub(3, 0);
			}

			if (par1Random.Next(3) == 0)
			{
				return new WorldGenHugeTrees(false, 10 + par1Random.Next(20), 3, 3);
			}
			else
			{
				return new WorldGenTrees(false, 4 + par1Random.Next(7), 3, 3, true);
			}
		}

		public override WorldGenerator Func_48410_b(Random par1Random)
		{
			if (par1Random.Next(4) == 0)
			{
				return new WorldGenTallGrass(Block.TallGrass.BlockID, 2);
			}
			else
			{
				return new WorldGenTallGrass(Block.TallGrass.BlockID, 1);
			}
		}

		public override void Decorate(World par1World, Random par2Random, int par3, int par4)
		{
			base.Decorate(par1World, par2Random, par3, par4);
			WorldGenVines worldgenvines = new WorldGenVines();

			for (int i = 0; i < 50; i++)
			{
				int j = par3 + par2Random.Next(16) + 8;
				sbyte byte0 = 64;
				int k = par4 + par2Random.Next(16) + 8;
				worldgenvines.Generate(par1World, par2Random, j, byte0, k);
			}
		}
	}

}