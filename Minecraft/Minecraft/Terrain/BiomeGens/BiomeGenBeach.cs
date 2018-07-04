namespace net.minecraft.src
{
	public class BiomeGenBeach : BiomeGenBase
	{
		public BiomeGenBeach(int par1) : base(par1)
		{
			SpawnableCreatureList.Clear();
			TopBlock = (byte)Block.Sand.BlockID;
			FillerBlock = (byte)Block.Sand.BlockID;
			BiomeDecorator.TreesPerChunk = -999;
			BiomeDecorator.DeadBushPerChunk = 0;
			BiomeDecorator.ReedsPerChunk = 0;
			BiomeDecorator.CactiPerChunk = 0;
		}
	}
}