namespace net.minecraft.src
{
	public class BiomeGenMushroomIsland : BiomeGenBase
	{
		public BiomeGenMushroomIsland(int par1) : base(par1)
		{
			BiomeDecorator.TreesPerChunk = -100;
			BiomeDecorator.FlowersPerChunk = -100;
			BiomeDecorator.GrassPerChunk = -100;
			BiomeDecorator.MushroomsPerChunk = 1;
			BiomeDecorator.BigMushroomsPerChunk = 1;
			TopBlock = (byte)Block.Mycelium.BlockID;
			SpawnableMonsterList.Clear();
			SpawnableCreatureList.Clear();
			SpawnableWaterCreatureList.Clear();
			SpawnableCreatureList.Add(new SpawnListEntry(typeof(net.minecraft.src.EntityMooshroom), 8, 4, 8));
		}
	}
}