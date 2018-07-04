using System;

namespace net.minecraft.src
{
	public class BiomeDecorator
	{
		/// <summary>
		/// The world the BiomeDecorator is currently decorating </summary>
		protected World CurrentWorld;

		/// <summary>
		/// The Biome Decorator's random number generator. </summary>
		protected Random RandomGenerator;

		/// <summary>
		/// The X-coordinate of the chunk currently being decorated </summary>
		protected int Chunk_X;

		/// <summary>
		/// The Z-coordinate of the chunk currently being decorated </summary>
		protected int Chunk_Z;

		/// <summary>
		/// The biome generator object. </summary>
		protected BiomeGenBase Biome;

		/// <summary>
		/// The clay generator. </summary>
		protected WorldGenerator ClayGen;

		/// <summary>
		/// The sand generator. </summary>
		protected WorldGenerator SandGen;

		/// <summary>
		/// The gravel generator. </summary>
		protected WorldGenerator GravelAsSandGen;

		/// <summary>
		/// The dirt generator. </summary>
		protected WorldGenerator DirtGen;
		protected WorldGenerator GravelGen;
		protected WorldGenerator CoalGen;
		protected WorldGenerator IronGen;

		/// <summary>
		/// Field that holds gold WorldGenMinable </summary>
		protected WorldGenerator GoldGen;

		/// <summary>
		/// Field that holds redstone WorldGenMinable </summary>
		protected WorldGenerator RedstoneGen;

		/// <summary>
		/// Field that holds diamond WorldGenMinable </summary>
		protected WorldGenerator DiamondGen;

		/// <summary>
		/// Field that holds Lapis WorldGenMinable </summary>
		protected WorldGenerator LapisGen;

		/// <summary>
		/// Field that holds one of the plantYellow WorldGenFlowers </summary>
		protected WorldGenerator PlantYellowGen;

		/// <summary>
		/// Field that holds one of the plantRed WorldGenFlowers </summary>
		protected WorldGenerator PlantRedGen;

		/// <summary>
		/// Field that holds mushroomBrown WorldGenFlowers </summary>
		protected WorldGenerator MushroomBrownGen;

		/// <summary>
		/// Field that holds mushroomRed WorldGenFlowers </summary>
		protected WorldGenerator MushroomRedGen;

		/// <summary>
		/// Field that holds big mushroom generator </summary>
		protected WorldGenerator BigMushroomGen;

		/// <summary>
		/// Field that holds WorldGenReed </summary>
		protected WorldGenerator ReedGen;

		/// <summary>
		/// Field that holds WorldGenCactus </summary>
		protected WorldGenerator CactusGen;

		/// <summary>
		/// The water lily generation! </summary>
		protected WorldGenerator WaterlilyGen;

		/// <summary>
		/// Amount of waterlilys per chunk. </summary>
		public int WaterlilyPerChunk;

		/// <summary>
		/// The number of trees to attempt to generate per chunk. Up to 10 in forests, none in deserts.
		/// </summary>
		public int TreesPerChunk;

		/// <summary>
		/// The number of yellow flower patches to generate per chunk. The game generates much less than this number, since
		/// it attempts to generate them at a random altitude.
		/// </summary>
		public int FlowersPerChunk;

		/// <summary>
		/// The amount of tall grass to generate per chunk. </summary>
		public int GrassPerChunk;

		/// <summary>
		/// The number of dead bushes to generate per chunk. Used in deserts and swamps.
		/// </summary>
		public int DeadBushPerChunk;

		/// <summary>
		/// The number of extra mushroom patches per chunk. It generates 1/4 this number in brown mushroom patches, and 1/8
		/// this number in red mushroom patches. These mushrooms go beyond the default base number of mushrooms.
		/// </summary>
		public int MushroomsPerChunk;

		/// <summary>
		/// The number of reeds to generate per chunk. Reeds won't generate if the randomly selected placement is unsuitable.
		/// </summary>
		public int ReedsPerChunk;

		/// <summary>
		/// The number of cactus plants to generate per chunk. Cacti only work on sand.
		/// </summary>
		public int CactiPerChunk;

		/// <summary>
		/// The number of sand patches to generate per chunk. Sand patches only generate when part of it is underwater.
		/// </summary>
		public int SandPerChunk;

		/// <summary>
		/// The number of sand patches to generate per chunk. Sand patches only generate when part of it is underwater. There
		/// appear to be two separate fields for this.
		/// </summary>
		public int SandPerChunk2;

		/// <summary>
		/// The number of clay patches to generate per chunk. Only generates when part of it is underwater.
		/// </summary>
		public int ClayPerChunk;

		/// <summary>
		/// Amount of big mushrooms per chunk </summary>
		public int BigMushroomsPerChunk;

		/// <summary>
		/// True if decorator should generate surface lava & water </summary>
		public bool GenerateLakes;

		public BiomeDecorator(BiomeGenBase par1BiomeGenBase)
		{
			ClayGen = new WorldGenClay(4);
			SandGen = new WorldGenSand(7, Block.Sand.BlockID);
			GravelAsSandGen = new WorldGenSand(6, Block.Gravel.BlockID);
			DirtGen = new WorldGenMinable(Block.Dirt.BlockID, 32);
			GravelGen = new WorldGenMinable(Block.Gravel.BlockID, 32);
			CoalGen = new WorldGenMinable(Block.OreCoal.BlockID, 16);
			IronGen = new WorldGenMinable(Block.OreIron.BlockID, 8);
			GoldGen = new WorldGenMinable(Block.OreGold.BlockID, 8);
			RedstoneGen = new WorldGenMinable(Block.OreRedstone.BlockID, 7);
			DiamondGen = new WorldGenMinable(Block.OreDiamond.BlockID, 7);
			LapisGen = new WorldGenMinable(Block.OreLapis.BlockID, 6);
			PlantYellowGen = new WorldGenFlowers(Block.PlantYellow.BlockID);
			PlantRedGen = new WorldGenFlowers(Block.PlantRed.BlockID);
			MushroomBrownGen = new WorldGenFlowers(Block.MushroomBrown.BlockID);
			MushroomRedGen = new WorldGenFlowers(Block.MushroomRed.BlockID);
			BigMushroomGen = new WorldGenBigMushroom();
			ReedGen = new WorldGenReed();
			CactusGen = new WorldGenCactus();
			WaterlilyGen = new WorldGenWaterlily();
			WaterlilyPerChunk = 0;
			TreesPerChunk = 0;
			FlowersPerChunk = 2;
			GrassPerChunk = 1;
			DeadBushPerChunk = 0;
			MushroomsPerChunk = 0;
			ReedsPerChunk = 0;
			CactiPerChunk = 0;
			SandPerChunk = 1;
			SandPerChunk2 = 3;
			ClayPerChunk = 1;
			BigMushroomsPerChunk = 0;
			GenerateLakes = true;
			Biome = par1BiomeGenBase;
		}

		/// <summary>
		/// Decorates the world. Calls code that was formerly (pre-1.8) in ChunkProviderGenerate.populate
		/// </summary>
		public virtual void Decorate(World par1World, Random par2Random, int par3, int par4)
		{
			if (CurrentWorld != null)
			{
				throw new Exception("Already decorating!!");
			}
			else
			{
				CurrentWorld = par1World;
				RandomGenerator = par2Random;
				Chunk_X = par3;
				Chunk_Z = par4;
				Decorate();
				CurrentWorld = null;
				RandomGenerator = null;
				return;
			}
		}

		/// <summary>
		/// The method that does the work of actually decorating chunks
		/// </summary>
		protected virtual void Decorate()
		{
			GenerateOres();

			for (int i = 0; i < SandPerChunk2; i++)
			{
				int i1 = Chunk_X + RandomGenerator.Next(16) + 8;
				int k5 = Chunk_Z + RandomGenerator.Next(16) + 8;
				SandGen.Generate(CurrentWorld, RandomGenerator, i1, CurrentWorld.GetTopSolidOrLiquidBlock(i1, k5), k5);
			}

			for (int j = 0; j < ClayPerChunk; j++)
			{
				int j1 = Chunk_X + RandomGenerator.Next(16) + 8;
				int l5 = Chunk_Z + RandomGenerator.Next(16) + 8;
				ClayGen.Generate(CurrentWorld, RandomGenerator, j1, CurrentWorld.GetTopSolidOrLiquidBlock(j1, l5), l5);
			}

			for (int k = 0; k < SandPerChunk; k++)
			{
				int k1 = Chunk_X + RandomGenerator.Next(16) + 8;
				int i6 = Chunk_Z + RandomGenerator.Next(16) + 8;
				SandGen.Generate(CurrentWorld, RandomGenerator, k1, CurrentWorld.GetTopSolidOrLiquidBlock(k1, i6), i6);
			}

			int l = TreesPerChunk;

			if (RandomGenerator.Next(10) == 0)
			{
				l++;
			}

			for (int l1 = 0; l1 < l; l1++)
			{
				int j6 = Chunk_X + RandomGenerator.Next(16) + 8;
				int k10 = Chunk_Z + RandomGenerator.Next(16) + 8;
				WorldGenerator worldgenerator = Biome.GetRandomWorldGenForTrees(RandomGenerator);
				worldgenerator.SetScale(1.0D, 1.0D, 1.0D);
				worldgenerator.Generate(CurrentWorld, RandomGenerator, j6, CurrentWorld.GetHeightValue(j6, k10), k10);
			}

			for (int i2 = 0; i2 < BigMushroomsPerChunk; i2++)
			{
				int k6 = Chunk_X + RandomGenerator.Next(16) + 8;
				int l10 = Chunk_Z + RandomGenerator.Next(16) + 8;
				BigMushroomGen.Generate(CurrentWorld, RandomGenerator, k6, CurrentWorld.GetHeightValue(k6, l10), l10);
			}

			for (int j2 = 0; j2 < FlowersPerChunk; j2++)
			{
				int l6 = Chunk_X + RandomGenerator.Next(16) + 8;
				int i11 = RandomGenerator.Next(128);
				int l14 = Chunk_Z + RandomGenerator.Next(16) + 8;
				PlantYellowGen.Generate(CurrentWorld, RandomGenerator, l6, i11, l14);

				if (RandomGenerator.Next(4) == 0)
				{
					int i7 = Chunk_X + RandomGenerator.Next(16) + 8;
					int j11 = RandomGenerator.Next(128);
					int i15 = Chunk_Z + RandomGenerator.Next(16) + 8;
					PlantRedGen.Generate(CurrentWorld, RandomGenerator, i7, j11, i15);
				}
			}

			for (int k2 = 0; k2 < GrassPerChunk; k2++)
			{
				int j7 = Chunk_X + RandomGenerator.Next(16) + 8;
				int k11 = RandomGenerator.Next(128);
				int j15 = Chunk_Z + RandomGenerator.Next(16) + 8;
				WorldGenerator worldgenerator1 = Biome.Func_48410_b(RandomGenerator);
				worldgenerator1.Generate(CurrentWorld, RandomGenerator, j7, k11, j15);
			}

			for (int l2 = 0; l2 < DeadBushPerChunk; l2++)
			{
				int k7 = Chunk_X + RandomGenerator.Next(16) + 8;
				int l11 = RandomGenerator.Next(128);
				int k15 = Chunk_Z + RandomGenerator.Next(16) + 8;
				(new WorldGenDeadBush(Block.DeadBush.BlockID)).Generate(CurrentWorld, RandomGenerator, k7, l11, k15);
			}

			for (int i3 = 0; i3 < WaterlilyPerChunk; i3++)
			{
				int l7 = Chunk_X + RandomGenerator.Next(16) + 8;
				int i12 = Chunk_Z + RandomGenerator.Next(16) + 8;
				int l15;

				for (l15 = RandomGenerator.Next(128); l15 > 0 && CurrentWorld.GetBlockId(l7, l15 - 1, i12) == 0; l15--)
				{
				}

				WaterlilyGen.Generate(CurrentWorld, RandomGenerator, l7, l15, i12);
			}

			for (int j3 = 0; j3 < MushroomsPerChunk; j3++)
			{
				if (RandomGenerator.Next(4) == 0)
				{
					int i8 = Chunk_X + RandomGenerator.Next(16) + 8;
					int j12 = Chunk_Z + RandomGenerator.Next(16) + 8;
					int i16 = CurrentWorld.GetHeightValue(i8, j12);
					MushroomBrownGen.Generate(CurrentWorld, RandomGenerator, i8, i16, j12);
				}

				if (RandomGenerator.Next(8) == 0)
				{
					int j8 = Chunk_X + RandomGenerator.Next(16) + 8;
					int k12 = Chunk_Z + RandomGenerator.Next(16) + 8;
					int j16 = RandomGenerator.Next(128);
					MushroomRedGen.Generate(CurrentWorld, RandomGenerator, j8, j16, k12);
				}
			}

			if (RandomGenerator.Next(4) == 0)
			{
				int k3 = Chunk_X + RandomGenerator.Next(16) + 8;
				int k8 = RandomGenerator.Next(128);
				int l12 = Chunk_Z + RandomGenerator.Next(16) + 8;
				MushroomBrownGen.Generate(CurrentWorld, RandomGenerator, k3, k8, l12);
			}

			if (RandomGenerator.Next(8) == 0)
			{
				int l3 = Chunk_X + RandomGenerator.Next(16) + 8;
				int l8 = RandomGenerator.Next(128);
				int i13 = Chunk_Z + RandomGenerator.Next(16) + 8;
				MushroomRedGen.Generate(CurrentWorld, RandomGenerator, l3, l8, i13);
			}

			for (int i4 = 0; i4 < ReedsPerChunk; i4++)
			{
				int i9 = Chunk_X + RandomGenerator.Next(16) + 8;
				int j13 = Chunk_Z + RandomGenerator.Next(16) + 8;
				int k16 = RandomGenerator.Next(128);
				ReedGen.Generate(CurrentWorld, RandomGenerator, i9, k16, j13);
			}

			for (int j4 = 0; j4 < 10; j4++)
			{
				int j9 = Chunk_X + RandomGenerator.Next(16) + 8;
				int k13 = RandomGenerator.Next(128);
				int l16 = Chunk_Z + RandomGenerator.Next(16) + 8;
				ReedGen.Generate(CurrentWorld, RandomGenerator, j9, k13, l16);
			}

			if (RandomGenerator.Next(32) == 0)
			{
				int k4 = Chunk_X + RandomGenerator.Next(16) + 8;
				int k9 = RandomGenerator.Next(128);
				int l13 = Chunk_Z + RandomGenerator.Next(16) + 8;
				(new WorldGenPumpkin()).Generate(CurrentWorld, RandomGenerator, k4, k9, l13);
			}

			for (int l4 = 0; l4 < CactiPerChunk; l4++)
			{
				int l9 = Chunk_X + RandomGenerator.Next(16) + 8;
				int i14 = RandomGenerator.Next(128);
				int i17 = Chunk_Z + RandomGenerator.Next(16) + 8;
				CactusGen.Generate(CurrentWorld, RandomGenerator, l9, i14, i17);
			}

			if (GenerateLakes)
			{
				for (int i5 = 0; i5 < 50; i5++)
				{
					int i10 = Chunk_X + RandomGenerator.Next(16) + 8;
					int j14 = RandomGenerator.Next(RandomGenerator.Next(120) + 8);
					int j17 = Chunk_Z + RandomGenerator.Next(16) + 8;
					(new WorldGenLiquids(Block.WaterMoving.BlockID)).Generate(CurrentWorld, RandomGenerator, i10, j14, j17);
				}

				for (int j5 = 0; j5 < 20; j5++)
				{
					int j10 = Chunk_X + RandomGenerator.Next(16) + 8;
					int k14 = RandomGenerator.Next(RandomGenerator.Next(RandomGenerator.Next(112) + 8) + 8);
					int k17 = Chunk_Z + RandomGenerator.Next(16) + 8;
					(new WorldGenLiquids(Block.LavaMoving.BlockID)).Generate(CurrentWorld, RandomGenerator, j10, k14, k17);
				}
			}
		}

		/// <summary>
		/// Standard ore generation helper. Generates most ores.
		/// </summary>
		protected virtual void GenStandardOre1(int par1, WorldGenerator par2WorldGenerator, int par3, int par4)
		{
			for (int i = 0; i < par1; i++)
			{
				int j = Chunk_X + RandomGenerator.Next(16);
				int k = RandomGenerator.Next(par4 - par3) + par3;
				int l = Chunk_Z + RandomGenerator.Next(16);
				par2WorldGenerator.Generate(CurrentWorld, RandomGenerator, j, k, l);
			}
		}

		/// <summary>
		/// Standard ore generation helper. Generates Lapis Lazuli.
		/// </summary>
		protected virtual void GenStandardOre2(int par1, WorldGenerator par2WorldGenerator, int par3, int par4)
		{
			for (int i = 0; i < par1; i++)
			{
				int j = Chunk_X + RandomGenerator.Next(16);
				int k = RandomGenerator.Next(par4) + RandomGenerator.Next(par4) + (par3 - par4);
				int l = Chunk_Z + RandomGenerator.Next(16);
				par2WorldGenerator.Generate(CurrentWorld, RandomGenerator, j, k, l);
			}
		}

		/// <summary>
		/// Generates ores in the current chunk
		/// </summary>
		protected virtual void GenerateOres()
		{
			GenStandardOre1(20, DirtGen, 0, 128);
			GenStandardOre1(10, GravelGen, 0, 128);
			GenStandardOre1(20, CoalGen, 0, 128);
			GenStandardOre1(20, IronGen, 0, 64);
			GenStandardOre1(2, GoldGen, 0, 32);
			GenStandardOre1(8, RedstoneGen, 0, 16);
			GenStandardOre1(1, DiamondGen, 0, 16);
			GenStandardOre2(1, LapisGen, 16, 16);
		}
	}

}