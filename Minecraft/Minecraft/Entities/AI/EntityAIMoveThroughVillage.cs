using System.Collections.Generic;

namespace net.minecraft.src
{
	public class EntityAIMoveThroughVillage : EntityAIBase
	{
		private EntityCreature TheEntity;
		private float Field_48290_b;
		private PathEntity Field_48291_c;
		private VillageDoorInfo DoorInfo;
		private bool Field_48289_e;
		private List<VillageDoorInfo> DoorList;

		public EntityAIMoveThroughVillage(EntityCreature par1EntityCreature, float par2, bool par3)
		{
			DoorList = new List<VillageDoorInfo>();
			TheEntity = par1EntityCreature;
			Field_48290_b = par2;
			Field_48289_e = par3;
			SetMutexBits(1);
		}

		/// <summary>
		/// Returns whether the EntityAIBase should begin execution.
		/// </summary>
		public override bool ShouldExecute()
		{
			Func_48286_h();

			if (Field_48289_e && TheEntity.WorldObj.IsDaytime())
			{
				return false;
			}

			Village village = TheEntity.WorldObj.VillageCollectionObj.FindNearestVillage(MathHelper2.Floor_double(TheEntity.PosX), MathHelper2.Floor_double(TheEntity.PosY), MathHelper2.Floor_double(TheEntity.PosZ), 0);

			if (village == null)
			{
				return false;
			}

			DoorInfo = Func_48284_a(village);

			if (DoorInfo == null)
			{
				return false;
			}

			bool flag = TheEntity.GetNavigator().Func_48665_b();
			TheEntity.GetNavigator().SetBreakDoors(false);
			Field_48291_c = TheEntity.GetNavigator().GetPathToXYZ(DoorInfo.PosX, DoorInfo.PosY, DoorInfo.PosZ);
			TheEntity.GetNavigator().SetBreakDoors(flag);

			if (Field_48291_c != null)
			{
				return true;
			}

			Vec3D vec3d = RandomPositionGenerator.Func_48620_a(TheEntity, 10, 7, Vec3D.CreateVector(DoorInfo.PosX, DoorInfo.PosY, DoorInfo.PosZ));

			if (vec3d == null)
			{
				return false;
			}
			else
			{
				TheEntity.GetNavigator().SetBreakDoors(false);
				Field_48291_c = TheEntity.GetNavigator().GetPathToXYZ(vec3d.XCoord, vec3d.YCoord, vec3d.ZCoord);
				TheEntity.GetNavigator().SetBreakDoors(flag);
				return Field_48291_c != null;
			}
		}

		/// <summary>
		/// Returns whether an in-progress EntityAIBase should continue executing
		/// </summary>
		public override bool ContinueExecuting()
		{
			if (TheEntity.GetNavigator().NoPath())
			{
				return false;
			}
			else
			{
				float f = TheEntity.Width + 4F;
				return TheEntity.GetDistanceSq(DoorInfo.PosX, DoorInfo.PosY, DoorInfo.PosZ) > (double)(f * f);
			}
		}

		/// <summary>
		/// Execute a one shot task or start executing a continuous task
		/// </summary>
		public override void StartExecuting()
		{
			TheEntity.GetNavigator().SetPath(Field_48291_c, Field_48290_b);
		}

		/// <summary>
		/// Resets the task
		/// </summary>
		public override void ResetTask()
		{
			if (TheEntity.GetNavigator().NoPath() || TheEntity.GetDistanceSq(DoorInfo.PosX, DoorInfo.PosY, DoorInfo.PosZ) < 16D)
			{
				DoorList.Add(DoorInfo);
			}
		}

		private VillageDoorInfo Func_48284_a(Village par1Village)
		{
			VillageDoorInfo villagedoorinfo = null;
			int i = 0x7fffffff;
			List<VillageDoorInfo> list = par1Village.GetVillageDoorInfoList();
			IEnumerator<VillageDoorInfo> iterator = list.GetEnumerator();

			do
			{
				if (!iterator.MoveNext())
				{
					break;
				}

				VillageDoorInfo villagedoorinfo1 = iterator.Current;
				int j = villagedoorinfo1.GetDistanceSquared(MathHelper2.Floor_double(TheEntity.PosX), MathHelper2.Floor_double(TheEntity.PosY), MathHelper2.Floor_double(TheEntity.PosZ));

				if (j < i && !Func_48285_a(villagedoorinfo1))
				{
					villagedoorinfo = villagedoorinfo1;
					i = j;
				}
			}
			while (true);

			return villagedoorinfo;
		}

		private bool Func_48285_a(VillageDoorInfo par1VillageDoorInfo)
		{
			for (IEnumerator<VillageDoorInfo> iterator = DoorList.GetEnumerator(); iterator.MoveNext();)
			{
				VillageDoorInfo villagedoorinfo = iterator.Current;

				if (par1VillageDoorInfo.PosX == villagedoorinfo.PosX && par1VillageDoorInfo.PosY == villagedoorinfo.PosY && par1VillageDoorInfo.PosZ == villagedoorinfo.PosZ)
				{
					return true;
				}
			}

			return false;
		}

		private void Func_48286_h()
		{
			if (DoorList.Count > 15)
			{
				DoorList.RemoveAt(0);
			}
		}
	}
}