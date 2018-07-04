namespace net.minecraft.src
{
	public class BiomeGenPlains : BiomeGenBase
	{
		public BiomeGenPlains(int par1) : base(par1)
		{
			BiomeDecorator.TreesPerChunk = -999;
			BiomeDecorator.FlowersPerChunk = 4;
			BiomeDecorator.GrassPerChunk = 10;
		}
	}
}