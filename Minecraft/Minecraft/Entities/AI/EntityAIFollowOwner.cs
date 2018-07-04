namespace net.minecraft.src
{
	public class EntityAIFollowOwner : EntityAIBase
	{
		private EntityTameable ThePet;
		private EntityLiving TheOwner;
		World TheWorld;
		private float Field_48303_f;
		private PathNavigate PetPathfinder;
		private int Field_48310_h;
		float MaxDist;
		float MinDist;
		private bool Field_48311_i;

		public EntityAIFollowOwner(EntityTameable par1EntityTameable, float par2, float par3, float par4)
		{
			ThePet = par1EntityTameable;
			TheWorld = par1EntityTameable.WorldObj;
			Field_48303_f = par2;
			PetPathfinder = par1EntityTameable.GetNavigator();
			MinDist = par3;
			MaxDist = par4;
			SetMutexBits(3);
		}

		/// <summary>
		/// Returns whether the EntityAIBase should begin execution.
		/// </summary>
		public override bool ShouldExecute()
		{
			EntityLiving entityliving = ThePet.GetOwner();

			if (entityliving == null)
			{
				return false;
			}

			if (ThePet.IsSitting())
			{
				return false;
			}

			if (ThePet.GetDistanceSqToEntity(entityliving) < (double)(MinDist * MinDist))
			{
				return false;
			}
			else
			{
				TheOwner = entityliving;
				return true;
			}
		}

		/// <summary>
		/// Returns whether an in-progress EntityAIBase should continue executing
		/// </summary>
		public override bool ContinueExecuting()
		{
			return !PetPathfinder.NoPath() && ThePet.GetDistanceSqToEntity(TheOwner) > (double)(MaxDist * MaxDist) && !ThePet.IsSitting();
		}

		/// <summary>
		/// Execute a one shot task or start executing a continuous task
		/// </summary>
		public override void StartExecuting()
		{
			Field_48310_h = 0;
			Field_48311_i = ThePet.GetNavigator().Func_48658_a();
			ThePet.GetNavigator().Func_48664_a(false);
		}

		/// <summary>
		/// Resets the task
		/// </summary>
		public override void ResetTask()
		{
			TheOwner = null;
			PetPathfinder.ClearPathEntity();
			ThePet.GetNavigator().Func_48664_a(Field_48311_i);
		}

		/// <summary>
		/// Updates the task
		/// </summary>
		public override void UpdateTask()
		{
			ThePet.GetLookHelper().SetLookPositionWithEntity(TheOwner, 10F, ThePet.GetVerticalFaceSpeed());

			if (ThePet.IsSitting())
			{
				return;
			}

			if (--Field_48310_h > 0)
			{
				return;
			}

			Field_48310_h = 10;

			if (PetPathfinder.Func_48667_a(TheOwner, Field_48303_f))
			{
				return;
			}

			if (ThePet.GetDistanceSqToEntity(TheOwner) < 144D)
			{
				return;
			}

			int i = MathHelper2.Floor_double(TheOwner.PosX) - 2;
			int j = MathHelper2.Floor_double(TheOwner.PosZ) - 2;
			int k = MathHelper2.Floor_double(TheOwner.BoundingBox.MinY);

			for (int l = 0; l <= 4; l++)
			{
				for (int i1 = 0; i1 <= 4; i1++)
				{
					if ((l < 1 || i1 < 1 || l > 3 || i1 > 3) && TheWorld.IsBlockNormalCube(i + l, k - 1, j + i1) && !TheWorld.IsBlockNormalCube(i + l, k, j + i1) && !TheWorld.IsBlockNormalCube(i + l, k + 1, j + i1))
					{
						ThePet.SetLocationAndAngles((float)(i + l) + 0.5F, k, (float)(j + i1) + 0.5F, ThePet.RotationYaw, ThePet.RotationPitch);
						PetPathfinder.ClearPathEntity();
						return;
					}
				}
			}
		}
	}
}