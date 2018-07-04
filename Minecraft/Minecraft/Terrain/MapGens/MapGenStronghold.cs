using System;
using System.Collections.Generic;
using System.Text;

namespace net.minecraft.src
{
	public class MapGenStronghold : MapGenStructure
	{
		private BiomeGenBase[] AllowedBiomeGenBases;

		/// <summary>
		/// is spawned false and set true once the defined BiomeGenBases were compared with the present ones
		/// </summary>
		private bool RanBiomeCheck;
		private ChunkCoordIntPair[] StructureCoords;

		public MapGenStronghold()
		{
			AllowedBiomeGenBases = (new BiomeGenBase[] { BiomeGenBase.Desert, BiomeGenBase.Forest, BiomeGenBase.ExtremeHills, BiomeGenBase.Swampland, BiomeGenBase.Taiga, BiomeGenBase.IcePlains, BiomeGenBase.IceMountains, BiomeGenBase.DesertHills, BiomeGenBase.ForestHills, BiomeGenBase.ExtremeHillsEdge, BiomeGenBase.Jungle, BiomeGenBase.JungleHills });
			StructureCoords = new ChunkCoordIntPair[3];
		}

		protected override bool CanSpawnStructureAtCoords(int par1, int par2)
		{
			if (!RanBiomeCheck)
			{
				Random random = new Random();
				random.SetSeed((int)WorldObj.GetSeed());
				double d = random.NextDouble() * Math.PI * 2D;

				for (int k = 0; k < StructureCoords.Length; k++)
				{
					double d1 = (1.25D + random.NextDouble()) * 32D;
					int l = (int)Math.Round(Math.Cos(d) * d1);
					int i1 = (int)Math.Round(Math.Sin(d) * d1);
                    List<BiomeGenBase> arraylist = new List<BiomeGenBase>();
					BiomeGenBase[] abiomegenbase = AllowedBiomeGenBases;
					int j1 = abiomegenbase.Length;

					for (int k1 = 0; k1 < j1; k1++)
					{
						BiomeGenBase biomegenbase = abiomegenbase[k1];
						arraylist.Add(biomegenbase);
					}

					ChunkPosition chunkposition = WorldObj.GetWorldChunkManager().FindBiomePosition((l << 4) + 8, (i1 << 4) + 8, 112, arraylist, random);

					if (chunkposition != null)
					{
						l = chunkposition.x >> 4;
						i1 = chunkposition.z >> 4;
					}
					else
					{
						Console.WriteLine((new StringBuilder()).Append("Placed stronghold in INVALID biome at (").Append(l).Append(", ").Append(i1).Append(")").ToString());
					}

					StructureCoords[k] = new ChunkCoordIntPair(l, i1);
					d += (Math.PI * 2D) / (double)StructureCoords.Length;
				}

				RanBiomeCheck = true;
			}

			ChunkCoordIntPair[] achunkcoordintpair = StructureCoords;
			int i = achunkcoordintpair.Length;

			for (int j = 0; j < i; j++)
			{
				ChunkCoordIntPair chunkcoordintpair = achunkcoordintpair[j];

				if (par1 == chunkcoordintpair.ChunkXPos && par2 == chunkcoordintpair.ChunkZPos)
				{
					Console.WriteLine((new StringBuilder()).Append(par1).Append(", ").Append(par2).ToString());
					return true;
				}
			}

			return false;
		}

		protected override List<ChunkPosition> Func_40482_a()
		{
            List<ChunkPosition> arraylist = new List<ChunkPosition>();
			ChunkCoordIntPair[] achunkcoordintpair = StructureCoords;
			int i = achunkcoordintpair.Length;

			for (int j = 0; j < i; j++)
			{
				ChunkCoordIntPair chunkcoordintpair = achunkcoordintpair[j];

				if (chunkcoordintpair != null)
				{
					arraylist.Add(chunkcoordintpair.GetChunkPosition(64));
				}
			}

			return arraylist;
		}

		protected override StructureStart GetStructureStart(int par1, int par2)
		{
			StructureStrongholdStart structurestrongholdstart;

			for (structurestrongholdstart = new StructureStrongholdStart(WorldObj, Rand, par1, par2); structurestrongholdstart.GetComponents().Count == 0 || ((ComponentStrongholdStairs2)structurestrongholdstart.GetComponents()[0]).PortalRoom == null; structurestrongholdstart = new StructureStrongholdStart(WorldObj, Rand, par1, par2))
			{
			}

			return structurestrongholdstart;
		}
	}
}