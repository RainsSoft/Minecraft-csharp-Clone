using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public abstract class MapGenStructure : MapGenBase
	{
		protected Dictionary<long, StructureStart> CoordMap;

		public MapGenStructure()
		{
            CoordMap = new Dictionary<long, StructureStart>();
		}

		public override void Generate(IChunkProvider par1IChunkProvider, World par2World, int par3, int par4, byte[] par5ArrayOfByte)
		{
			base.Generate(par1IChunkProvider, par2World, par3, par4, par5ArrayOfByte);
		}

		/// <summary>
		/// Recursively called by generate() (generate) and optionally by itself.
		/// </summary>
		protected override void RecursiveGenerate(World par1World, int par2, int par3, int par4, int par5, byte[] par6ArrayOfByte)
		{
			if (CoordMap.ContainsKey(ChunkCoordIntPair.ChunkXZ2Int(par2, par3)))
			{
				return;
			}

			Rand.Next();

			if (CanSpawnStructureAtCoords(par2, par3))
			{
				StructureStart structurestart = GetStructureStart(par2, par3);
				CoordMap[ChunkCoordIntPair.ChunkXZ2Int(par2, par3)] = structurestart;
			}
		}

		/// <summary>
		/// Generates structures in specified chunk next to existing structures. Does *not* generate StructureStarts.
		/// </summary>
		public virtual bool GenerateStructuresInChunk(World par1World, Random par2Random, int par3, int par4)
		{
			int i = (par3 << 4) + 8;
			int j = (par4 << 4) + 8;
			bool flag = false;
			IEnumerator<StructureStart> iterator = CoordMap.Values.GetEnumerator();

			do
			{
				if (!iterator.MoveNext())
				{
					break;
				}

				StructureStart structurestart = iterator.Current;

				if (structurestart.IsSizeableStructure() && structurestart.GetBoundingBox().IntersectsWith(i, j, i + 15, j + 15))
				{
					structurestart.GenerateStructure(par1World, par2Random, new StructureBoundingBox(i, j, i + 15, j + 15));
					flag = true;
				}
			}
			while (true);

			return flag;
		}

		public virtual bool Func_40483_a(int par1, int par2, int par3)
		{
			IEnumerator<StructureStart> iterator = CoordMap.Values.GetEnumerator();
			label0:

			do
			{
				if (iterator.MoveNext())
				{
					StructureStart structurestart = iterator.Current;

					if (!structurestart.IsSizeableStructure() || !structurestart.GetBoundingBox().IntersectsWith(par1, par3, par1, par3))
					{
						continue;
					}

					IEnumerator<StructureComponent> iterator1 = structurestart.GetComponents().GetEnumerator();
					StructureComponent structurecomponent;

					do
					{
						if (!iterator1.MoveNext())
						{
							goto label0;
						}

						structurecomponent = iterator1.Current;
					}
					while (!structurecomponent.GetBoundingBox().IsVecInside(par1, par2, par3));

					break;
				}
				else
				{
					return false;
				}
			}
			while (true);

			return true;
		}

		public virtual ChunkPosition GetNearestInstance(World par1World, int par2, int par3, int par4)
		{
			WorldObj = par1World;
            Rand.SetSeed((int)par1World.GetSeed());
			long l = Rand.Next();
			long l1 = Rand.Next();
			long l2 = (long)(par2 >> 4) * l;
			long l3 = (long)(par4 >> 4) * l1;
            Rand.SetSeed((int)(l2 ^ l3 ^ par1World.GetSeed()));
			RecursiveGenerate(par1World, par2 >> 4, par4 >> 4, 0, 0, null);
			double d = double.MaxValue;
			ChunkPosition chunkposition = null;

			IEnumerator<StructureStart> iterator = CoordMap.Values.GetEnumerator();

			do
			{
				if (!iterator.MoveNext())
				{
					break;
				}

				StructureStart structurestart = iterator.Current;

				if (structurestart.IsSizeableStructure())
				{
					StructureComponent structurecomponent = structurestart.GetComponents()[0];
					ChunkPosition chunkposition2 = structurecomponent.GetCenter();
					int i = chunkposition2.x - par2;
					int k = chunkposition2.y - par3;
					int j1 = chunkposition2.z - par4;
					double d1 = i + i * k * k + j1 * j1;

					if (d1 < d)
					{
						d = d1;
						chunkposition = chunkposition2;
					}
				}
			}
			while (true);

			if (chunkposition != null)
			{
				return chunkposition;
			}

			List<ChunkPosition> list = Func_40482_a();

			if (iterator != null)
			{
				ChunkPosition chunkposition1 = null;
				IEnumerator<ChunkPosition> iterator2 = list.GetEnumerator();

				do
				{
					if (!iterator2.MoveNext())
					{
						break;
					}

					ChunkPosition chunkposition3 = iterator2.Current;
					int j = chunkposition3.x - par2;
					int i1 = chunkposition3.y - par3;
					int k1 = chunkposition3.z - par4;
					double d2 = j + j * i1 * i1 + k1 * k1;

					if (d2 < d)
					{
						d = d2;
						chunkposition1 = chunkposition3;
					}
				}
				while (true);

				return chunkposition1;
			}
			else
			{
				return null;
			}
		}

		protected virtual List<ChunkPosition> Func_40482_a()
		{
			return null;
		}

		protected abstract bool CanSpawnStructureAtCoords(int i, int j);

		protected abstract StructureStart GetStructureStart(int i, int j);
	}
}