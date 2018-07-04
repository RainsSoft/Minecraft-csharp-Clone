using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class EntityAIAvoidEntity : EntityAIBase
	{
		/// <summary>
		/// The entity we are attached to </summary>
		private EntityCreature TheEntity;
		private float Field_48242_b;
		private float Field_48243_c;
		private Entity Field_48240_d;
		private float Field_48241_e;
		private PathEntity Field_48238_f;

		/// <summary>
		/// The PathNavigate of our entity </summary>
		private PathNavigate EntityPathNavigate;

		/// <summary>
		/// The class of the entity we should avoid </summary>
		private Type TargetEntityClass;

		public EntityAIAvoidEntity(EntityCreature par1EntityCreature, Type par2Class, float par3, float par4, float par5)
		{
			TheEntity = par1EntityCreature;
			TargetEntityClass = par2Class;
			Field_48241_e = par3;
			Field_48242_b = par4;
			Field_48243_c = par5;
			EntityPathNavigate = par1EntityCreature.GetNavigator();
			SetMutexBits(1);
		}

		/// <summary>
		/// Returns whether the EntityAIBase should begin execution.
		/// </summary>
		public override bool ShouldExecute()
		{
			if (TargetEntityClass == (typeof(net.minecraft.src.EntityPlayer)))
			{
				if ((TheEntity is EntityTameable) && ((EntityTameable)TheEntity).IsTamed())
				{
					return false;
				}

				Field_48240_d = TheEntity.WorldObj.GetClosestPlayerToEntity(TheEntity, Field_48241_e);

				if (Field_48240_d == null)
				{
					return false;
				}
			}
			else
			{
				List<Entity> list = TheEntity.WorldObj.GetEntitiesWithinAABB(TargetEntityClass, TheEntity.BoundingBox.Expand(Field_48241_e, 3, Field_48241_e));

				if (list.Count == 0)
				{
					return false;
				}

				Field_48240_d = list[0];
			}

			if (!TheEntity.Func_48090_aM().CanSee(Field_48240_d))
			{
				return false;
			}

			Vec3D vec3d = RandomPositionGenerator.Func_48623_b(TheEntity, 16, 7, Vec3D.CreateVector(Field_48240_d.PosX, Field_48240_d.PosY, Field_48240_d.PosZ));

			if (vec3d == null)
			{
				return false;
			}

            if (Field_48240_d.GetDistanceSq((float)vec3d.XCoord, (float)vec3d.YCoord, (float)vec3d.ZCoord) < Field_48240_d.GetDistanceSqToEntity(TheEntity))
			{
				return false;
			}

			Field_48238_f = EntityPathNavigate.GetPathToXYZ(vec3d.XCoord, vec3d.YCoord, vec3d.ZCoord);

			if (Field_48238_f == null)
			{
				return false;
			}

			return Field_48238_f.Func_48639_a(vec3d);
		}

		/// <summary>
		/// Returns whether an in-progress EntityAIBase should continue executing
		/// </summary>
		public override bool ContinueExecuting()
		{
			return !EntityPathNavigate.NoPath();
		}

		/// <summary>
		/// Execute a one shot task or start executing a continuous task
		/// </summary>
		public override void StartExecuting()
		{
			EntityPathNavigate.SetPath(Field_48238_f, Field_48242_b);
		}

		/// <summary>
		/// Resets the task
		/// </summary>
		public override void ResetTask()
		{
			Field_48240_d = null;
		}

		/// <summary>
		/// Updates the task
		/// </summary>
		public override void UpdateTask()
		{
			if (TheEntity.GetDistanceSqToEntity(Field_48240_d) < 49D)
			{
				TheEntity.GetNavigator().SetSpeed(Field_48243_c);
			}
			else
			{
				TheEntity.GetNavigator().SetSpeed(Field_48242_b);
			}
		}
	}
}