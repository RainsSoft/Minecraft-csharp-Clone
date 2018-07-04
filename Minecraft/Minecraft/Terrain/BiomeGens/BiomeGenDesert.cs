using System;

namespace net.minecraft.src
{
	public class BiomeGenDesert : BiomeGenBase
	{
		public BiomeGenDesert(int par1) : base(par1)
		{
			SpawnableCreatureList.Clear();
			TopBlock = (byte)Block.Sand.BlockID;
			FillerBlock = (byte)Block.Sand.BlockID;
			BiomeDecorator.TreesPerChunk = -999;
			BiomeDecorator.DeadBushPerChunk = 2;
			BiomeDecorator.ReedsPerChunk = 50;
			BiomeDecorator.CactiPerChunk = 10;
		}

		public override void Decorate(World par1World, Random par2Random, int par3, int par4)
		{
			base.Decorate(par1World, par2Random, par3, par4);

			if (par2Random.Next(1000) == 0)
			{
				int i = par3 + par2Random.Next(16) + 8;
				int j = par4 + par2Random.Next(16) + 8;
				WorldGenDesertWells worldgendesertwells = new WorldGenDesertWells();
				worldgendesertwells.Generate(par1World, par2Random, i, par1World.GetHeightValue(i, j) + 1, j);
			}
		}
	}
}