using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class VillageCollection
	{
		private World WorldObj;

		/// <summary>
		/// This is a black hole. You can add data to this list through a public interface, but you can't query that
		/// information in any way and it's not used internally either.
		/// </summary>
        private readonly List<ChunkCoordinates> VillagerPositionsList = new List<ChunkCoordinates>();
        private readonly List<VillageDoorInfo> NewDoors = new List<VillageDoorInfo>();
        private readonly List<Village> VillageList = new List<Village>();
		private int TickCounter;

		public VillageCollection(World par1World)
		{
			TickCounter = 0;
			WorldObj = par1World;
		}

		/// <summary>
		/// This is a black hole. You can add data to this list through a public interface, but you can't query that
		/// information in any way and it's not used internally either.
		/// </summary>
		public virtual void AddVillagerPosition(int par1, int par2, int par3)
		{
			if (VillagerPositionsList.Count > 64)
			{
				return;
			}

			if (!IsVillagerPositionPresent(par1, par2, par3))
			{
				VillagerPositionsList.Add(new ChunkCoordinates(par1, par2, par3));
			}
		}

		/// <summary>
		/// Runs a single tick for the village collection
		/// </summary>
		public virtual void Tick()
		{
			TickCounter++;
			Village village;

			for (IEnumerator<Village> iterator = VillageList.GetEnumerator(); iterator.MoveNext(); village.Tick(TickCounter))
			{
				village = iterator.Current;
			}

			RemoveAnnihilatedVillages();
			DropOldestVillagerPosition();
			AddNewDoorsToVillageOrCreateVillage();
		}

		private void RemoveAnnihilatedVillages()
		{
            for (int i = VillageList.Count; i > 0; i--)
            {
                Village village = VillageList[i];

                if (village.IsAnnihilated())
                {
                    VillageList.RemoveAt(i);
                }
            }
		}

		public virtual List<Village> Func_48554_b()
		{
			return VillageList;
		}

		/// <summary>
		/// Finds the nearest village, but only the given coordinates are withing it's bounding box plus the given the
		/// distance.
		/// </summary>
		public virtual Village FindNearestVillage(int par1, int par2, int par3, int par4)
		{
			Village village = null;
	//JAVA TO C# CONVERTER TODO TASK: Octal literals cannot be represented in C#:
			float f = 3.402823E+038F;
			IEnumerator<Village> iterator = VillageList.GetEnumerator();

			do
			{
				if (!iterator.MoveNext())
				{
					break;
				}

				Village village1 = iterator.Current;
				float f1 = village1.GetCenter().GetDistanceSquared(par1, par2, par3);

				if (f1 < f)
				{
					int i = par4 + village1.GetVillageRadius();

					if (f1 <= (float)(i * i))
					{
						village = village1;
						f = f1;
					}
				}
			}
			while (true);

			return village;
		}

		private void DropOldestVillagerPosition()
		{
			if (VillagerPositionsList.Count == 0)
			{
				return;
			}
			else
			{
				AddUnassignedWoodenDoorsAroundToNewDoorsList((ChunkCoordinates)VillagerPositionsList[0]);
                VillagerPositionsList.RemoveAt(0);
				return;
			}
		}

		private void AddNewDoorsToVillageOrCreateVillage()
		{
			for (int i = 0; i < NewDoors.Count; i++)
			{
				VillageDoorInfo villagedoorinfo = (VillageDoorInfo)NewDoors[i];
				bool flag = false;
				IEnumerator<Village> iterator = VillageList.GetEnumerator();

				do
				{
					if (!iterator.MoveNext())
					{
						break;
					}

					Village village1 = iterator.Current;
					int j = (int)village1.GetCenter().GetEuclideanDistanceTo(villagedoorinfo.PosX, villagedoorinfo.PosY, villagedoorinfo.PosZ);

					if (j > 32 + village1.GetVillageRadius())
					{
						continue;
					}

					village1.AddVillageDoorInfo(villagedoorinfo);
					flag = true;
					break;
				}
				while (true);

				if (!flag)
				{
					Village village = new Village(WorldObj);
					village.AddVillageDoorInfo(villagedoorinfo);
					VillageList.Add(village);
				}
			}

			NewDoors.Clear();
		}

		private void AddUnassignedWoodenDoorsAroundToNewDoorsList(ChunkCoordinates par1ChunkCoordinates)
		{
			sbyte byte0 = 16;
			sbyte byte1 = 4;
			sbyte byte2 = 16;

			for (int i = par1ChunkCoordinates.PosX - byte0; i < par1ChunkCoordinates.PosX + byte0; i++)
			{
				for (int j = par1ChunkCoordinates.PosY - byte1; j < par1ChunkCoordinates.PosY + byte1; j++)
				{
					for (int k = par1ChunkCoordinates.PosZ - byte2; k < par1ChunkCoordinates.PosZ + byte2; k++)
					{
						if (!IsWoodenDoorAt(i, j, k))
						{
							continue;
						}

						VillageDoorInfo villagedoorinfo = GetVillageDoorAt(i, j, k);

						if (villagedoorinfo == null)
						{
							AddDoorToNewListIfAppropriate(i, j, k);
						}
						else
						{
							villagedoorinfo.LastActivityTimestamp = TickCounter;
						}
					}
				}
			}
		}

		private VillageDoorInfo GetVillageDoorAt(int par1, int par2, int par3)
		{
			for (IEnumerator<VillageDoorInfo> iterator = NewDoors.GetEnumerator(); iterator.MoveNext();)
			{
				VillageDoorInfo villagedoorinfo = iterator.Current;

				if (villagedoorinfo.PosX == par1 && villagedoorinfo.PosZ == par3 && Math.Abs(villagedoorinfo.PosY - par2) <= 1)
				{
					return villagedoorinfo;
				}
			}

			for (IEnumerator<Village> iterator1 = VillageList.GetEnumerator(); iterator1.MoveNext();)
			{
				Village village = iterator1.Current;
				VillageDoorInfo villagedoorinfo1 = village.GetVillageDoorAt(par1, par2, par3);

				if (villagedoorinfo1 != null)
				{
					return villagedoorinfo1;
				}
			}

			return null;
		}

		private void AddDoorToNewListIfAppropriate(int par1, int par2, int par3)
		{
			int i = ((BlockDoor)Block.DoorWood).GetDoorOrientation(WorldObj, par1, par2, par3);

			if (i == 0 || i == 2)
			{
				int j = 0;

				for (int l = -5; l < 0; l++)
				{
					if (WorldObj.CanBlockSeeTheSky(par1 + l, par2, par3))
					{
						j--;
					}
				}

				for (int i1 = 1; i1 <= 5; i1++)
				{
					if (WorldObj.CanBlockSeeTheSky(par1 + i1, par2, par3))
					{
						j++;
					}
				}

				if (j != 0)
				{
					NewDoors.Add(new VillageDoorInfo(par1, par2, par3, j <= 0 ? 2 : -2, 0, TickCounter));
				}
			}
			else
			{
				int k = 0;

				for (int j1 = -5; j1 < 0; j1++)
				{
					if (WorldObj.CanBlockSeeTheSky(par1, par2, par3 + j1))
					{
						k--;
					}
				}

				for (int k1 = 1; k1 <= 5; k1++)
				{
					if (WorldObj.CanBlockSeeTheSky(par1, par2, par3 + k1))
					{
						k++;
					}
				}

				if (k != 0)
				{
					NewDoors.Add(new VillageDoorInfo(par1, par2, par3, 0, k <= 0 ? 2 : -2, TickCounter));
				}
			}
		}

		private bool IsVillagerPositionPresent(int par1, int par2, int par3)
		{
			for (IEnumerator<ChunkCoordinates> iterator = VillagerPositionsList.GetEnumerator(); iterator.MoveNext();)
			{
				ChunkCoordinates chunkcoordinates = iterator.Current;

				if (chunkcoordinates.PosX == par1 && chunkcoordinates.PosY == par2 && chunkcoordinates.PosZ == par3)
				{
					return true;
				}
			}

			return false;
		}

		private bool IsWoodenDoorAt(int par1, int par2, int par3)
		{
			int i = WorldObj.GetBlockId(par1, par2, par3);
			return i == Block.DoorWood.BlockID;
		}
	}
}