using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class Village
	{
		private readonly World WorldObj;

		/// <summary>
		/// list of VillageDoorInfo objects </summary>
		private readonly List<VillageDoorInfo> VillageDoorInfoList = new List<VillageDoorInfo>();

		/// <summary>
		/// This is the sum of all door coordinates and used to calculate the actual village center by dividing by the number
		/// of doors.
		/// </summary>
		private readonly ChunkCoordinates CenterHelper = new ChunkCoordinates(0, 0, 0);

		/// <summary>
		/// This is the actual village center. </summary>
		private readonly ChunkCoordinates Center = new ChunkCoordinates(0, 0, 0);
		private int VillageRadius;
		private int LastAddDoorTimestamp;
		private int TickCounter;
		private int NumVillagers;
		private List<VillageAgressor> VillageAgressors;
		private int NumIronGolems;

		public Village(World par1World)
		{
			VillageRadius = 0;
			LastAddDoorTimestamp = 0;
			TickCounter = 0;
			NumVillagers = 0;
            VillageAgressors = new List<VillageAgressor>();
			NumIronGolems = 0;
			WorldObj = par1World;
		}

		/// <summary>
		/// Called periodically by VillageCollection
		/// </summary>
		public virtual void Tick(int par1)
		{
			TickCounter = par1;
			RemoveDeadAndOutOfRangeDoors();
			RemoveDeadAndOldAgressors();

			if (par1 % 20 == 0)
			{
				UpdateNumVillagers();
			}

			if (par1 % 30 == 0)
			{
				UpdateNumIronGolems();
			}

			int i = NumVillagers / 16;

			if (NumIronGolems < i && VillageDoorInfoList.Count > 20 && WorldObj.Rand.Next(7000) == 0)
			{
				Vec3D vec3d = TryGetIronGolemSpawningLocation(MathHelper2.Floor_float(Center.PosX), MathHelper2.Floor_float(Center.PosY), MathHelper2.Floor_float(Center.PosZ), 2, 4, 2);

				if (vec3d != null)
				{
					EntityIronGolem entityirongolem = new EntityIronGolem(WorldObj);
                    entityirongolem.SetPosition((float)vec3d.XCoord, (float)vec3d.YCoord, (float)vec3d.ZCoord);
					WorldObj.SpawnEntityInWorld(entityirongolem);
					NumIronGolems++;
				}
			}
		}

		/// <summary>
		/// Tries up to 10 times to get a valid spawning location before eventually failing and returning null.
		/// </summary>
		private Vec3D TryGetIronGolemSpawningLocation(int par1, int par2, int par3, int par4, int par5, int par6)
		{
			for (int i = 0; i < 10; i++)
			{
				int j = (par1 + WorldObj.Rand.Next(16)) - 8;
				int k = (par2 + WorldObj.Rand.Next(6)) - 3;
				int l = (par3 + WorldObj.Rand.Next(16)) - 8;

				if (IsInRange(j, k, l) && IsValidIronGolemSpawningLocation(j, k, l, par4, par5, par6))
				{
					return Vec3D.CreateVector(j, k, l);
				}
			}

			return null;
		}

		private bool IsValidIronGolemSpawningLocation(int par1, int par2, int par3, int par4, int par5, int par6)
		{
			if (!WorldObj.IsBlockNormalCube(par1, par2 - 1, par3))
			{
				return false;
			}

			int i = par1 - par4 / 2;
			int j = par3 - par6 / 2;

			for (int k = i; k < i + par4; k++)
			{
				for (int l = par2; l < par2 + par5; l++)
				{
					for (int i1 = j; i1 < j + par6; i1++)
					{
						if (WorldObj.IsBlockNormalCube(k, l, i1))
						{
							return false;
						}
					}
				}
			}

			return true;
		}

		private void UpdateNumIronGolems()
		{
            List<Entity> list = WorldObj.GetEntitiesWithinAABB(typeof(net.minecraft.src.EntityIronGolem), AxisAlignedBB.GetBoundingBoxFromPool(Center.PosX - VillageRadius, Center.PosY - 4, Center.PosZ - VillageRadius, Center.PosX + VillageRadius, Center.PosY + 4, Center.PosZ + VillageRadius));
			NumIronGolems = list.Count;
		}

		private void UpdateNumVillagers()
		{
            List<Entity> list = WorldObj.GetEntitiesWithinAABB(typeof(net.minecraft.src.EntityVillager), AxisAlignedBB.GetBoundingBoxFromPool(Center.PosX - VillageRadius, Center.PosY - 4, Center.PosZ - VillageRadius, Center.PosX + VillageRadius, Center.PosY + 4, Center.PosZ + VillageRadius));
			NumVillagers = list.Count;
		}

		public virtual ChunkCoordinates GetCenter()
		{
			return Center;
		}

		public virtual int GetVillageRadius()
		{
			return VillageRadius;
		}

		/// <summary>
		/// Actually get num village door info entries, but that boils down to number of doors. Called by
		/// EntityAIVillagerMate and VillageSiege
		/// </summary>
		public virtual int GetNumVillageDoors()
		{
			return VillageDoorInfoList.Count;
		}

		public virtual int GetTicksSinceLastDoorAdding()
		{
			return TickCounter - LastAddDoorTimestamp;
		}

		public virtual int GetNumVillagers()
		{
			return NumVillagers;
		}

		/// <summary>
		/// Returns true, if the given coordinates are within the bounding box of the village.
		/// </summary>
		public virtual bool IsInRange(int par1, int par2, int par3)
		{
			return Center.GetDistanceSquared(par1, par2, par3) < (float)(VillageRadius * VillageRadius);
		}

		/// <summary>
		/// called only by class EntityAIMoveThroughVillage
		/// </summary>
		public virtual List<VillageDoorInfo> GetVillageDoorInfoList()
		{
			return VillageDoorInfoList;
		}

		public virtual VillageDoorInfo FindNearestDoor(int par1, int par2, int par3)
		{
			VillageDoorInfo villagedoorinfo = null;
			int i = 0x7fffffff;
			IEnumerator<VillageDoorInfo> iterator = VillageDoorInfoList.GetEnumerator();

			do
			{
				if (!iterator.MoveNext())
				{
					break;
				}

				VillageDoorInfo villagedoorinfo1 = iterator.Current;
				int j = villagedoorinfo1.GetDistanceSquared(par1, par2, par3);

				if (j < i)
				{
					villagedoorinfo = villagedoorinfo1;
					i = j;
				}
			}
			while (true);

			return villagedoorinfo;
		}

		/// <summary>
		/// Find a door suitable for shelter. If there are more doors in a distance of 16 blocks, then the least restricted
		/// one (i.e. the one protecting the lowest number of villagers) of them is chosen, else the nearest one regardless
		/// of restriction.
		/// </summary>
		public virtual VillageDoorInfo FindNearestDoorUnrestricted(int par1, int par2, int par3)
		{
			VillageDoorInfo villagedoorinfo = null;
			int i = 0x7fffffff;
			IEnumerator<VillageDoorInfo> iterator = VillageDoorInfoList.GetEnumerator();

			do
			{
				if (!iterator.MoveNext())
				{
					break;
				}

				VillageDoorInfo villagedoorinfo1 = iterator.Current;
				int j = villagedoorinfo1.GetDistanceSquared(par1, par2, par3);

				if (j > 256)
				{
					j *= 1000;
				}
				else
				{
					j = villagedoorinfo1.GetDoorOpeningRestrictionCounter();
				}

				if (j < i)
				{
					villagedoorinfo = villagedoorinfo1;
					i = j;
				}
			}
			while (true);

			return villagedoorinfo;
		}

		public virtual VillageDoorInfo GetVillageDoorAt(int par1, int par2, int par3)
		{
			if (Center.GetDistanceSquared(par1, par2, par3) > (float)(VillageRadius * VillageRadius))
			{
				return null;
			}

			for (IEnumerator<VillageDoorInfo> iterator = VillageDoorInfoList.GetEnumerator(); iterator.MoveNext();)
			{
				VillageDoorInfo villagedoorinfo = iterator.Current;

				if (villagedoorinfo.PosX == par1 && villagedoorinfo.PosZ == par3 && Math.Abs(villagedoorinfo.PosY - par2) <= 1)
				{
					return villagedoorinfo;
				}
			}

			return null;
		}

		public virtual void AddVillageDoorInfo(VillageDoorInfo par1VillageDoorInfo)
		{
			VillageDoorInfoList.Add(par1VillageDoorInfo);
			CenterHelper.PosX += par1VillageDoorInfo.PosX;
			CenterHelper.PosY += par1VillageDoorInfo.PosY;
			CenterHelper.PosZ += par1VillageDoorInfo.PosZ;
			UpdateVillageRadiusAndCenter();
			LastAddDoorTimestamp = par1VillageDoorInfo.LastActivityTimestamp;
		}

		/// <summary>
		/// Returns true, if there is not a single village door left. Called by VillageCollection
		/// </summary>
		public virtual bool IsAnnihilated()
		{
			return VillageDoorInfoList.Count == 0;
		}

		public virtual void AddOrRenewAgressor(EntityLiving par1EntityLiving)
		{
			for (IEnumerator<VillageAgressor> iterator = VillageAgressors.GetEnumerator(); iterator.MoveNext();)
			{
				VillageAgressor villageagressor = iterator.Current;

				if (villageagressor.Agressor == par1EntityLiving)
				{
					villageagressor.AgressionTime = TickCounter;
					return;
				}
			}

			VillageAgressors.Add(new VillageAgressor(this, par1EntityLiving, TickCounter));
		}

		public virtual EntityLiving FindNearestVillageAggressor(EntityLiving par1EntityLiving)
		{
			double d = double.MaxValue;
			VillageAgressor villageagressor = null;

			for (int i = 0; i < VillageAgressors.Count; i++)
			{
				VillageAgressor villageagressor1 = (VillageAgressor)VillageAgressors[i];
				double d1 = villageagressor1.Agressor.GetDistanceSqToEntity(par1EntityLiving);

				if (d1 <= d)
				{
					villageagressor = villageagressor1;
					d = d1;
				}
			}

			return villageagressor == null ? null : villageagressor.Agressor;
		}

		private void RemoveDeadAndOldAgressors()
		{
            for (int i = VillageAgressors.Count; i > 0; i--)
            {
				VillageAgressor villageagressor = VillageAgressors[i];

				if (!villageagressor.Agressor.IsEntityAlive() || Math.Abs(TickCounter - villageagressor.AgressionTime) > 300)
				{
					VillageAgressors.RemoveAt(i);
				}
            }
		}

		private void RemoveDeadAndOutOfRangeDoors()
		{
			bool flag = false;
			bool flag1 = WorldObj.Rand.Next(50) == 0;

            for (int i = VillageDoorInfoList.Count; i > 0; i--)
            {
                var doorInfo = VillageDoorInfoList[i];

                if (!IsBlockDoor(doorInfo.PosX, doorInfo.PosY, doorInfo.PosZ) || Math.Abs(TickCounter - doorInfo.LastActivityTimestamp) > 1200)
                {
                    CenterHelper.PosX -= doorInfo.PosX;
                    CenterHelper.PosY -= doorInfo.PosY;
                    CenterHelper.PosZ -= doorInfo.PosZ;
                    flag = true;
                    doorInfo.IsDetachedFromVillageFlag = true;
                    VillageDoorInfoList.RemoveAt(i);
                }
            }

			if (flag)
			{
				UpdateVillageRadiusAndCenter();
			}
		}

		private bool IsBlockDoor(int par1, int par2, int par3)
		{
			int i = WorldObj.GetBlockId(par1, par2, par3);

			if (i <= 0)
			{
				return false;
			}
			else
			{
				return i == Block.DoorWood.BlockID;
			}
		}

		private void UpdateVillageRadiusAndCenter()
		{
			int i = VillageDoorInfoList.Count;

			if (i == 0)
			{
				Center.Set(0, 0, 0);
				VillageRadius = 0;
				return;
			}

			Center.Set(CenterHelper.PosX / i, CenterHelper.PosY / i, CenterHelper.PosZ / i);
			int j = 0;

			for (IEnumerator<VillageDoorInfo> iterator = VillageDoorInfoList.GetEnumerator(); iterator.MoveNext();)
			{
				VillageDoorInfo villagedoorinfo = iterator.Current;
				j = Math.Max(villagedoorinfo.GetDistanceSquared(Center.PosX, Center.PosY, Center.PosZ), j);
			}

			VillageRadius = Math.Max(32, (int)Math.Sqrt(j) + 1);
		}
	}
}