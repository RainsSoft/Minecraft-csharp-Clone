namespace net.minecraft.src
{
	public class BiomeGenEnd : BiomeGenBase
	{
		public BiomeGenEnd(int par1) : base(par1)
		{
			SpawnableMonsterList.Clear();
			SpawnableCreatureList.Clear();
			SpawnableWaterCreatureList.Clear();
			SpawnableMonsterList.Add(new SpawnListEntry(typeof(net.minecraft.src.EntityEnderman), 10, 4, 4));
			TopBlock = (byte)Block.Dirt.BlockID;
			FillerBlock = (byte)Block.Dirt.BlockID;
			BiomeDecorator = new BiomeEndDecorator(this);
		}

		/// <summary>
		/// takes temperature, returns color
		/// </summary>
		public override int GetSkyColorByTemp(float par1)
		{
			return 0;
		}
	}
}