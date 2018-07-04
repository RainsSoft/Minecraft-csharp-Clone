namespace net.minecraft.src
{
	public class BiomeEndDecorator : BiomeDecorator
	{
		protected WorldGenerator SpikeGen;

		public BiomeEndDecorator(BiomeGenBase par1BiomeGenBase) : base(par1BiomeGenBase)
		{
			SpikeGen = new WorldGenSpikes(Block.WhiteStone.BlockID);
		}

		/// <summary>
		/// The method that does the work of actually decorating chunks
		/// </summary>
		protected override void Decorate()
		{
			GenerateOres();

			if (RandomGenerator.Next(5) == 0)
			{
				int i = Chunk_X + RandomGenerator.Next(16) + 8;
				int j = Chunk_Z + RandomGenerator.Next(16) + 8;
				int k = CurrentWorld.GetTopSolidOrLiquidBlock(i, j);

				if (k <= 0)
				{
					;
				}

				SpikeGen.Generate(CurrentWorld, RandomGenerator, i, k, j);
			}

			if (Chunk_X == 0 && Chunk_Z == 0)
			{
				EntityDragon entitydragon = new EntityDragon(CurrentWorld);
				entitydragon.SetLocationAndAngles(0.0F, 128F, 0.0F, RandomGenerator.NextFloat() * 360F, 0.0F);
				CurrentWorld.SpawnEntityInWorld(entitydragon);
			}
		}
	}
}