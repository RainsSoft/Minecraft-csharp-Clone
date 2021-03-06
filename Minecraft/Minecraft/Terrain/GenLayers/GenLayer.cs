namespace net.minecraft.src
{

	public abstract class GenLayer
	{
		/// <summary>
		/// seed from World#getWorldSeed that is used in the LCG prng </summary>
		private long WorldGenSeed;

		/// <summary>
		/// parent GenLayer that was provided via the constructor </summary>
		protected GenLayer Parent;

		/// <summary>
		/// final part of the LCG prng that uses the chunk X, Z coords along with the other two seeds to generate
		/// pseudorandom numbers
		/// </summary>
		private long ChunkSeed;

		/// <summary>
		/// base seed to the LCG prng provided via the constructor </summary>
		private long BaseSeed;

		public static GenLayer[] Func_48425_a(long par0, WorldType par2WorldType)
		{
			GenLayer obj = new GenLayerIsland(1L);
			obj = new GenLayerFuzzyZoom(2000L, ((GenLayer)(obj)));
			obj = new GenLayerAddIsland(1L, ((GenLayer)(obj)));
			obj = new GenLayerZoom(2001L, ((GenLayer)(obj)));
			obj = new GenLayerAddIsland(2L, ((GenLayer)(obj)));
			obj = new GenLayerAddSnow(2L, ((GenLayer)(obj)));
			obj = new GenLayerZoom(2002L, ((GenLayer)(obj)));
			obj = new GenLayerAddIsland(3L, ((GenLayer)(obj)));
			obj = new GenLayerZoom(2003L, ((GenLayer)(obj)));
			obj = new GenLayerAddIsland(4L, ((GenLayer)(obj)));
			obj = new GenLayerAddMushroomIsland(5L, ((GenLayer)(obj)));
			sbyte byte0 = 4;
			GenLayer obj1 = obj;
			obj1 = GenLayerZoom.Func_35515_a(1000L, ((GenLayer)(obj1)), 0);
			obj1 = new GenLayerRiverInit(100L, ((GenLayer)(obj1)));
			obj1 = GenLayerZoom.Func_35515_a(1000L, ((GenLayer)(obj1)), byte0 + 2);
			obj1 = new GenLayerRiver(1L, ((GenLayer)(obj1)));
			obj1 = new GenLayerSmooth(1000L, ((GenLayer)(obj1)));
			GenLayer obj2 = obj;
			obj2 = GenLayerZoom.Func_35515_a(1000L, ((GenLayer)(obj2)), 0);
			obj2 = new GenLayerBiome(200L, ((GenLayer)(obj2)), par2WorldType);
			obj2 = GenLayerZoom.Func_35515_a(1000L, ((GenLayer)(obj2)), 2);
			obj2 = new GenLayerHills(1000L, ((GenLayer)(obj2)));

			for (int i = 0; i < byte0; i++)
			{
				obj2 = new GenLayerZoom(1000 + i, ((GenLayer)(obj2)));

				if (i == 0)
				{
					obj2 = new GenLayerAddIsland(3L, ((GenLayer)(obj2)));
				}

				if (i == 1)
				{
					obj2 = new GenLayerShore(1000L, ((GenLayer)(obj2)));
				}

				if (i == 1)
				{
					obj2 = new GenLayerSwampRivers(1000L, ((GenLayer)(obj2)));
				}
			}

			obj2 = new GenLayerSmooth(1000L, ((GenLayer)(obj2)));
			obj2 = new GenLayerRiverMix(100L, ((GenLayer)(obj2)), ((GenLayer)(obj1)));
			GenLayerRiverMix genlayerrivermix = ((GenLayerRiverMix)(obj2));
			GenLayerVoronoiZoom genlayervoronoizoom = new GenLayerVoronoiZoom(10L, ((GenLayer)(obj2)));
			((GenLayer)(obj2)).InitWorldGenSeed(par0);
			genlayervoronoizoom.InitWorldGenSeed(par0);
			return (new GenLayer[] { obj2, genlayervoronoizoom, genlayerrivermix });
		}

		public GenLayer(long par1)
		{
			BaseSeed = par1;
			BaseSeed *= BaseSeed * 0x5851f42d4c957f2dL + 0x14057b7ef767814fL;
			BaseSeed += par1;
			BaseSeed *= BaseSeed * 0x5851f42d4c957f2dL + 0x14057b7ef767814fL;
			BaseSeed += par1;
			BaseSeed *= BaseSeed * 0x5851f42d4c957f2dL + 0x14057b7ef767814fL;
			BaseSeed += par1;
		}

		/// <summary>
		/// Initialize layer's local worldGenSeed based on its own baseSeed and the world's global seed (passed in as an
		/// argument).
		/// </summary>
		public virtual void InitWorldGenSeed(long par1)
		{
			WorldGenSeed = par1;

			if (Parent != null)
			{
				Parent.InitWorldGenSeed(par1);
			}

			WorldGenSeed *= WorldGenSeed * 0x5851f42d4c957f2dL + 0x14057b7ef767814fL;
			WorldGenSeed += BaseSeed;
			WorldGenSeed *= WorldGenSeed * 0x5851f42d4c957f2dL + 0x14057b7ef767814fL;
			WorldGenSeed += BaseSeed;
			WorldGenSeed *= WorldGenSeed * 0x5851f42d4c957f2dL + 0x14057b7ef767814fL;
			WorldGenSeed += BaseSeed;
		}

		/// <summary>
		/// Initialize layer's current chunkSeed based on the local worldGenSeed and the (x,z) chunk coordinates.
		/// </summary>
		public virtual void InitChunkSeed(long par1, long par3)
		{
			ChunkSeed = WorldGenSeed;
			ChunkSeed *= ChunkSeed * 0x5851f42d4c957f2dL + 0x14057b7ef767814fL;
			ChunkSeed += par1;
			ChunkSeed *= ChunkSeed * 0x5851f42d4c957f2dL + 0x14057b7ef767814fL;
			ChunkSeed += par3;
			ChunkSeed *= ChunkSeed * 0x5851f42d4c957f2dL + 0x14057b7ef767814fL;
			ChunkSeed += par1;
			ChunkSeed *= ChunkSeed * 0x5851f42d4c957f2dL + 0x14057b7ef767814fL;
			ChunkSeed += par3;
		}

		/// <summary>
		/// returns a LCG pseudo random number from [0, x). Args: int x
		/// </summary>
		protected virtual int NextInt(int par1)
		{
			int i = (int)((ChunkSeed >> 24) % (long)par1);

			if (i < 0)
			{
				i += par1;
			}

			ChunkSeed *= ChunkSeed * 0x5851f42d4c957f2dL + 0x14057b7ef767814fL;
			ChunkSeed += WorldGenSeed;
			return i;
		}

		/// <summary>
		/// Returns a list of integer values generated by this layer. These may be interpreted as temperatures, rainfall
		/// amounts, or biomeList[] indices based on the particular GenLayer subclass.
		/// </summary>
		public abstract int[] GetInts(int i, int j, int k, int l);
	}

}