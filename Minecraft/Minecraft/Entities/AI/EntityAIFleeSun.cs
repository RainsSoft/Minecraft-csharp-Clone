using System;

namespace net.minecraft.src
{
	public class EntityAIFleeSun : EntityAIBase
	{
		private EntityCreature TheCreature;
		private double ShelterX;
		private double ShelterY;
		private double ShelterZ;
		private float Field_48299_e;
		private World TheWorld;

		public EntityAIFleeSun(EntityCreature par1EntityCreature, float par2)
		{
			TheCreature = par1EntityCreature;
			Field_48299_e = par2;
			TheWorld = par1EntityCreature.WorldObj;
			SetMutexBits(1);
		}

		/// <summary>
		/// Returns whether the EntityAIBase should begin execution.
		/// </summary>
		public override bool ShouldExecute()
		{
			if (!TheWorld.IsDaytime())
			{
				return false;
			}

			if (!TheCreature.IsBurning())
			{
				return false;
			}

			if (!TheWorld.CanBlockSeeTheSky(MathHelper2.Floor_double(TheCreature.PosX), (int)TheCreature.BoundingBox.MinY, MathHelper2.Floor_double(TheCreature.PosZ)))
			{
				return false;
			}

			Vec3D vec3d = FindPossibleShelter();

			if (vec3d == null)
			{
				return false;
			}
			else
			{
				ShelterX = vec3d.XCoord;
				ShelterY = vec3d.YCoord;
				ShelterZ = vec3d.ZCoord;
				return true;
			}
		}

		/// <summary>
		/// Returns whether an in-progress EntityAIBase should continue executing
		/// </summary>
		public override bool ContinueExecuting()
		{
			return !TheCreature.GetNavigator().NoPath();
		}

		/// <summary>
		/// Execute a one shot task or start executing a continuous task
		/// </summary>
		public override void StartExecuting()
		{
			TheCreature.GetNavigator().Func_48666_a(ShelterX, ShelterY, ShelterZ, Field_48299_e);
		}

		private Vec3D FindPossibleShelter()
		{
			Random random = TheCreature.GetRNG();

			for (int i = 0; i < 10; i++)
			{
				int j = MathHelper2.Floor_double((TheCreature.PosX + (double)random.Next(20)) - 10D);
				int k = MathHelper2.Floor_double((TheCreature.BoundingBox.MinY + (double)random.Next(6)) - 3D);
				int l = MathHelper2.Floor_double((TheCreature.PosZ + (double)random.Next(20)) - 10D);

				if (!TheWorld.CanBlockSeeTheSky(j, k, l) && TheCreature.GetBlockPathWeight(j, k, l) < 0.0F)
				{
					return Vec3D.CreateVector(j, k, l);
				}
			}

			return null;
		}
	}
}