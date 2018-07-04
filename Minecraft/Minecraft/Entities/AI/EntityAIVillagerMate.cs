using System;

namespace net.minecraft.src
{
	public class EntityAIVillagerMate : EntityAIBase
	{
		private EntityVillager VillagerObj;
		private EntityVillager Mate;
		private World WorldObj;
		private int MatingTimeout;
		Village VillageObj;

		public EntityAIVillagerMate(EntityVillager par1EntityVillager)
		{
			MatingTimeout = 0;
			VillagerObj = par1EntityVillager;
			WorldObj = par1EntityVillager.WorldObj;
			SetMutexBits(3);
		}

		/// <summary>
		/// Returns whether the EntityAIBase should begin execution.
		/// </summary>
		public override bool ShouldExecute()
		{
			if (VillagerObj.GetGrowingAge() != 0)
			{
				return false;
			}

			if (VillagerObj.GetRNG().Next(500) != 0)
			{
				return false;
			}

			VillageObj = WorldObj.VillageCollectionObj.FindNearestVillage(MathHelper2.Floor_double(VillagerObj.PosX), MathHelper2.Floor_double(VillagerObj.PosY), MathHelper2.Floor_double(VillagerObj.PosZ), 0);

			if (VillageObj == null)
			{
				return false;
			}

			if (!CheckSufficientDoorsPresentForNewVillager())
			{
				return false;
			}

			Entity entity = WorldObj.FindNearestEntityWithinAABB(typeof(net.minecraft.src.EntityVillager), VillagerObj.BoundingBox.Expand(8, 3, 8), VillagerObj);

			if (entity == null)
			{
				return false;
			}

			Mate = (EntityVillager)entity;
			return Mate.GetGrowingAge() == 0;
		}

		/// <summary>
		/// Execute a one shot task or start executing a continuous task
		/// </summary>
		public override void StartExecuting()
		{
			MatingTimeout = 300;
			VillagerObj.SetIsMatingFlag(true);
		}

		/// <summary>
		/// Resets the task
		/// </summary>
		public override void ResetTask()
		{
			VillageObj = null;
			Mate = null;
			VillagerObj.SetIsMatingFlag(false);
		}

		/// <summary>
		/// Returns whether an in-progress EntityAIBase should continue executing
		/// </summary>
		public override bool ContinueExecuting()
		{
			return MatingTimeout >= 0 && CheckSufficientDoorsPresentForNewVillager() && VillagerObj.GetGrowingAge() == 0;
		}

		/// <summary>
		/// Updates the task
		/// </summary>
		public override void UpdateTask()
		{
			MatingTimeout--;
			VillagerObj.GetLookHelper().SetLookPositionWithEntity(Mate, 10F, 30F);

			if (VillagerObj.GetDistanceSqToEntity(Mate) > 2.25D)
			{
				VillagerObj.GetNavigator().Func_48667_a(Mate, 0.25F);
			}
			else if (MatingTimeout == 0 && Mate.GetIsMatingFlag())
			{
				GiveBirth();
			}

			if (VillagerObj.GetRNG().Next(35) == 0)
			{
				SpawnHeartParticles(VillagerObj);
			}
		}

		private bool CheckSufficientDoorsPresentForNewVillager()
		{
			int i = (int)((double)(float)VillageObj.GetNumVillageDoors() * 0.34999999999999998D);
			return VillageObj.GetNumVillagers() < i;
		}

		private void GiveBirth()
		{
			EntityVillager entityvillager = new EntityVillager(WorldObj);
			Mate.SetGrowingAge(6000);
			VillagerObj.SetGrowingAge(6000);
			entityvillager.SetGrowingAge(-24000);
			entityvillager.SetProfession(VillagerObj.GetRNG().Next(5));
			entityvillager.SetLocationAndAngles(VillagerObj.PosX, VillagerObj.PosY, VillagerObj.PosZ, 0.0F, 0.0F);
			WorldObj.SpawnEntityInWorld(entityvillager);
			SpawnHeartParticles(entityvillager);
		}

		private void SpawnHeartParticles(EntityLiving par1EntityLiving)
		{
			Random random = par1EntityLiving.GetRNG();

			for (int i = 0; i < 5; i++)
			{
				double d = random.NextGaussian() * 0.02D;
				double d1 = random.NextGaussian() * 0.02D;
				double d2 = random.NextGaussian() * 0.02D;
				WorldObj.SpawnParticle("heart", (par1EntityLiving.PosX + (double)(random.NextFloat() * par1EntityLiving.Width * 2.0F)) - (double)par1EntityLiving.Width, par1EntityLiving.PosY + 1.0D + (double)(random.NextFloat() * par1EntityLiving.Height), (par1EntityLiving.PosZ + (double)(random.NextFloat() * par1EntityLiving.Width * 2.0F)) - (double)par1EntityLiving.Width, d, d1, d2);
			}
		}
	}
}