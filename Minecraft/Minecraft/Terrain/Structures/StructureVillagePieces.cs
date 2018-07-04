using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class StructureVillagePieces
	{
		public StructureVillagePieces()
		{
		}

        public static List<StructureVillagePieceWeight> GetStructureVillageWeightedPieceList(Random par0Random, int par1)
		{
            List<StructureVillagePieceWeight> arraylist = new List<StructureVillagePieceWeight>();
			arraylist.Add(new StructureVillagePieceWeight(typeof(net.minecraft.src.ComponentVillageHouse4_Garden), 4, MathHelper2.GetRandomIntegerInRange(par0Random, 2 + par1, 4 + par1 * 2)));
			arraylist.Add(new StructureVillagePieceWeight(typeof(net.minecraft.src.ComponentVillageChurch), 20, MathHelper2.GetRandomIntegerInRange(par0Random, 0 + par1, 1 + par1)));
			arraylist.Add(new StructureVillagePieceWeight(typeof(net.minecraft.src.ComponentVillageHouse1), 20, MathHelper2.GetRandomIntegerInRange(par0Random, 0 + par1, 2 + par1)));
			arraylist.Add(new StructureVillagePieceWeight(typeof(net.minecraft.src.ComponentVillageWoodHut), 3, MathHelper2.GetRandomIntegerInRange(par0Random, 2 + par1, 5 + par1 * 3)));
			arraylist.Add(new StructureVillagePieceWeight(typeof(net.minecraft.src.ComponentVillageHall), 15, MathHelper2.GetRandomIntegerInRange(par0Random, 0 + par1, 2 + par1)));
			arraylist.Add(new StructureVillagePieceWeight(typeof(net.minecraft.src.ComponentVillageField), 3, MathHelper2.GetRandomIntegerInRange(par0Random, 1 + par1, 4 + par1)));
			arraylist.Add(new StructureVillagePieceWeight(typeof(net.minecraft.src.ComponentVillageField2), 3, MathHelper2.GetRandomIntegerInRange(par0Random, 2 + par1, 4 + par1 * 2)));
			arraylist.Add(new StructureVillagePieceWeight(typeof(net.minecraft.src.ComponentVillageHouse2), 15, MathHelper2.GetRandomIntegerInRange(par0Random, 0, 1 + par1)));
			arraylist.Add(new StructureVillagePieceWeight(typeof(net.minecraft.src.ComponentVillageHouse3), 8, MathHelper2.GetRandomIntegerInRange(par0Random, 0 + par1, 3 + par1 * 2)));
            /*
			IEnumerator<StructureVillagePieceWeight> iterator = arraylist.GetEnumerator();

			do
			{
				if (!iterator.MoveNext())
				{
					break;
				}

				if (iterator.Current.VillagePiecesLimit == 0)
				{
					iterator.Remove();
				}
			}
			while (true);
            */
			return arraylist;
		}

		private static int GetAvailablePieceWeight(List<StructureVillagePieceWeight> par0ArrayList)
		{
			bool flag = false;
			int i = 0;

			for (IEnumerator<StructureVillagePieceWeight> iterator = par0ArrayList.GetEnumerator(); iterator.MoveNext();)
			{
				StructureVillagePieceWeight structurevillagepieceweight = iterator.Current;

				if (structurevillagepieceweight.VillagePiecesLimit > 0 && structurevillagepieceweight.VillagePiecesSpawned < structurevillagepieceweight.VillagePiecesLimit)
				{
					flag = true;
				}

				i += structurevillagepieceweight.VillagePieceWeight;
			}

			return flag ? i : -1;
		}

        private static ComponentVillage GetVillageComponentFromWeightedPiece(StructureVillagePieceWeight par0StructureVillagePieceWeight, List<StructureComponent> par1List, Random par2Random, int par3, int par4, int par5, int par6, int par7)
		{
			Type class1 = par0StructureVillagePieceWeight.VillagePieceClass;
			object obj = null;

			if (class1 == (typeof(net.minecraft.src.ComponentVillageHouse4_Garden)))
			{
				obj = ComponentVillageHouse4_Garden.FindValidPlacement(par1List, par2Random, par3, par4, par5, par6, par7);
			}
			else if (class1 == (typeof(net.minecraft.src.ComponentVillageChurch)))
			{
				obj = ComponentVillageChurch.FindValidPlacement(par1List, par2Random, par3, par4, par5, par6, par7);
			}
			else if (class1 == (typeof(net.minecraft.src.ComponentVillageHouse1)))
			{
				obj = ComponentVillageHouse1.FindValidPlacement(par1List, par2Random, par3, par4, par5, par6, par7);
			}
			else if (class1 == (typeof(net.minecraft.src.ComponentVillageWoodHut)))
			{
				obj = ComponentVillageWoodHut.FindValidPlacement(par1List, par2Random, par3, par4, par5, par6, par7);
			}
			else if (class1 == (typeof(net.minecraft.src.ComponentVillageHall)))
			{
				obj = ComponentVillageHall.FindValidPlacement(par1List, par2Random, par3, par4, par5, par6, par7);
			}
			else if (class1 == (typeof(net.minecraft.src.ComponentVillageField)))
			{
				obj = ComponentVillageField.FindValidPlacement(par1List, par2Random, par3, par4, par5, par6, par7);
			}
			else if (class1 == (typeof(net.minecraft.src.ComponentVillageField2)))
			{
				obj = ComponentVillageField2.FindValidPlacement(par1List, par2Random, par3, par4, par5, par6, par7);
			}
			else if (class1 == (typeof(net.minecraft.src.ComponentVillageHouse2)))
			{
				obj = ComponentVillageHouse2.FindValidPlacement(par1List, par2Random, par3, par4, par5, par6, par7);
			}
			else if (class1 == (typeof(net.minecraft.src.ComponentVillageHouse3)))
			{
				obj = ComponentVillageHouse3.FindValidPlacement(par1List, par2Random, par3, par4, par5, par6, par7);
			}

			return ((ComponentVillage)(obj));
		}

		/// <summary>
		/// 'attempts to find a next Village Component to be spawned'
		/// </summary>
        private static ComponentVillage GetNextVillageComponent(ComponentVillageStartPiece par0ComponentVillageStartPiece, List<StructureComponent> par1List, Random par2Random, int par3, int par4, int par5, int par6, int par7)
		{
			int var8 = GetAvailablePieceWeight(par0ComponentVillageStartPiece.StructureVillageWeightedPieceList);

			if (var8 <= 0)
			{
				return null;
			}
			else
			{
				int var9 = 0;

				while (var9 < 5)
				{
					++var9;
					int var10 = par2Random.Next(var8);
					IEnumerator<StructureVillagePieceWeight> var11 = par0ComponentVillageStartPiece.StructureVillageWeightedPieceList.GetEnumerator();

					while (var11.MoveNext())
					{
						StructureVillagePieceWeight var12 = var11.Current;
						var10 -= var12.VillagePieceWeight;

						if (var10 < 0)
						{
							if (!var12.CanSpawnMoreVillagePiecesOfType(par7) || var12 == par0ComponentVillageStartPiece.StructVillagePieceWeight && par0ComponentVillageStartPiece.StructureVillageWeightedPieceList.Count > 1)
							{
								break;
							}

							ComponentVillage var13 = GetVillageComponentFromWeightedPiece(var12, par1List, par2Random, par3, par4, par5, par6, par7);

							if (var13 != null)
							{
								++var12.VillagePiecesSpawned;
								par0ComponentVillageStartPiece.StructVillagePieceWeight = var12;

								if (!var12.CanSpawnMoreVillagePieces())
								{
									par0ComponentVillageStartPiece.StructureVillageWeightedPieceList.Remove(var12);
								}

								return var13;
							}
						}
					}
				}

				StructureBoundingBox var14 = ComponentVillageTorch.FindValidPlacement(par1List, par2Random, par3, par4, par5, par6);

				if (var14 != null)
				{
					return new ComponentVillageTorch(par7, par2Random, var14, par6);
				}
				else
				{
					return null;
				}
			}
		}

		/// <summary>
		/// 'attempts to find a next Structure Component to be spawned, private Village function'
		/// </summary>
        private static StructureComponent GetNextVillageStructureComponent(ComponentVillageStartPiece par0ComponentVillageStartPiece, List<StructureComponent> par1List, Random par2Random, int par3, int par4, int par5, int par6, int par7)
		{
			if (par7 > 50)
			{
				return null;
			}

			if (Math.Abs(par3 - par0ComponentVillageStartPiece.GetBoundingBox().MinX) > 112 || Math.Abs(par5 - par0ComponentVillageStartPiece.GetBoundingBox().MinZ) > 112)
			{
				return null;
			}

			ComponentVillage componentvillage = GetNextVillageComponent(par0ComponentVillageStartPiece, par1List, par2Random, par3, par4, par5, par6, par7 + 1);

			if (componentvillage != null)
			{
				int i = (((StructureComponent)(componentvillage)).BoundingBox.MinX + ((StructureComponent)(componentvillage)).BoundingBox.MaxX) / 2;
				int j = (((StructureComponent)(componentvillage)).BoundingBox.MinZ + ((StructureComponent)(componentvillage)).BoundingBox.MaxZ) / 2;
				int k = ((StructureComponent)(componentvillage)).BoundingBox.MaxX - ((StructureComponent)(componentvillage)).BoundingBox.MinX;
				int l = ((StructureComponent)(componentvillage)).BoundingBox.MaxZ - ((StructureComponent)(componentvillage)).BoundingBox.MinZ;
				int i1 = k <= l ? l : k;

				if (par0ComponentVillageStartPiece.GetWorldChunkManager().AreBiomesViable(i, j, i1 / 2 + 4, MapGenVillage.VillageSpawnBiomes))
				{
					par1List.Add(componentvillage);
					par0ComponentVillageStartPiece.Field_35108_e.Add(componentvillage);
					return componentvillage;
				}
			}

			return null;
		}

        private static StructureComponent GetNextComponentVillagePath(ComponentVillageStartPiece par0ComponentVillageStartPiece, List<StructureComponent> par1List, Random par2Random, int par3, int par4, int par5, int par6, int par7)
		{
			if (par7 > 3 + par0ComponentVillageStartPiece.TerrainType)
			{
				return null;
			}

			if (Math.Abs(par3 - par0ComponentVillageStartPiece.GetBoundingBox().MinX) > 112 || Math.Abs(par5 - par0ComponentVillageStartPiece.GetBoundingBox().MinZ) > 112)
			{
				return null;
			}

			StructureBoundingBox structureboundingbox = ComponentVillagePathGen.Func_35087_a(par0ComponentVillageStartPiece, par1List, par2Random, par3, par4, par5, par6);

			if (structureboundingbox != null && structureboundingbox.MinY > 10)
			{
				ComponentVillagePathGen componentvillagepathgen = new ComponentVillagePathGen(par7, par2Random, structureboundingbox, par6);
				int i = (((StructureComponent)(componentvillagepathgen)).BoundingBox.MinX + ((StructureComponent)(componentvillagepathgen)).BoundingBox.MaxX) / 2;
				int j = (((StructureComponent)(componentvillagepathgen)).BoundingBox.MinZ + ((StructureComponent)(componentvillagepathgen)).BoundingBox.MaxZ) / 2;
				int k = ((StructureComponent)(componentvillagepathgen)).BoundingBox.MaxX - ((StructureComponent)(componentvillagepathgen)).BoundingBox.MinX;
				int l = ((StructureComponent)(componentvillagepathgen)).BoundingBox.MaxZ - ((StructureComponent)(componentvillagepathgen)).BoundingBox.MinZ;
				int i1 = k <= l ? l : k;

				if (par0ComponentVillageStartPiece.GetWorldChunkManager().AreBiomesViable(i, j, i1 / 2 + 4, MapGenVillage.VillageSpawnBiomes))
				{
					par1List.Add(componentvillagepathgen);
					par0ComponentVillageStartPiece.Field_35106_f.Add(componentvillagepathgen);
					return componentvillagepathgen;
				}
			}

			return null;
		}

		/// <summary>
		/// 'attempts to find a next Structure Component to be spawned'
		/// </summary>
        public static StructureComponent GetNextStructureComponent(ComponentVillageStartPiece par0ComponentVillageStartPiece, List<StructureComponent> par1List, Random par2Random, int par3, int par4, int par5, int par6, int par7)
		{
			return GetNextVillageStructureComponent(par0ComponentVillageStartPiece, par1List, par2Random, par3, par4, par5, par6, par7);
		}

        public static StructureComponent GetNextStructureComponentVillagePath(ComponentVillageStartPiece par0ComponentVillageStartPiece, List<StructureComponent> par1List, Random par2Random, int par3, int par4, int par5, int par6, int par7)
		{
			return GetNextComponentVillagePath(par0ComponentVillageStartPiece, par1List, par2Random, par3, par4, par5, par6, par7);
		}
	}
}