namespace net.minecraft.src
{
	public class BiomeGenHell : BiomeGenBase
	{
		public BiomeGenHell(int par1) : base(par1)
		{
			SpawnableMonsterList.Clear();
			SpawnableCreatureList.Clear();
			SpawnableWaterCreatureList.Clear();
			SpawnableMonsterList.Add(new SpawnListEntry(typeof(net.minecraft.src.EntityGhast), 50, 4, 4));
			SpawnableMonsterList.Add(new SpawnListEntry(typeof(net.minecraft.src.EntityPigZombie), 100, 4, 4));
			SpawnableMonsterList.Add(new SpawnListEntry(typeof(net.minecraft.src.EntityMagmaCube), 1, 4, 4));
		}
	}

}