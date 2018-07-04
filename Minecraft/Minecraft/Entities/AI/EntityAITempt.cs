using System;

namespace net.minecraft.src
{
	public class EntityAITempt : EntityAIBase
	{
		/// <summary>
		/// The entity using this AI that is tempted by the player. </summary>
		private EntityCreature TemptedEntity;
		private float Field_48275_b;
        private float Field_48276_c;
        private float Field_48273_d;
        private float Field_48274_e;
        private float Field_48271_f;
        private float Field_48272_g;

		/// <summary>
		/// The player that is tempting the entity that is using this AI. </summary>
		private EntityPlayer TemptingPlayer;

		/// <summary>
		/// A counter that is decremented each time the shouldExecute method is called. The shouldExecute method will always
		/// return false if delayTemptCounter is greater than 0.
		/// </summary>
		private int DelayTemptCounter;
		private bool Field_48280_j;

		/// <summary>
		/// This field saves the ID of the items that can be used to breed entities with this behaviour.
		/// </summary>
		private int BreedingFood;

		/// <summary>
		/// Whether the entity using this AI will be scared by the tempter's sudden movement.
		/// </summary>
		private bool ScaredByPlayerMovement;
		private bool Field_48279_m;

		public EntityAITempt(EntityCreature par1EntityCreature, float par2, int par3, bool par4)
		{
			DelayTemptCounter = 0;
			TemptedEntity = par1EntityCreature;
			Field_48275_b = par2;
			BreedingFood = par3;
			ScaredByPlayerMovement = par4;
			SetMutexBits(3);
		}

		/// <summary>
		/// Returns whether the EntityAIBase should begin execution.
		/// </summary>
		public override bool ShouldExecute()
		{
			if (DelayTemptCounter > 0)
			{
				DelayTemptCounter--;
				return false;
			}

			TemptingPlayer = TemptedEntity.WorldObj.GetClosestPlayerToEntity(TemptedEntity, 10);

			if (TemptingPlayer == null)
			{
				return false;
			}

			ItemStack itemstack = TemptingPlayer.GetCurrentEquippedItem();

			if (itemstack == null)
			{
				return false;
			}

			return itemstack.ItemID == BreedingFood;
		}

		/// <summary>
		/// Returns whether an in-progress EntityAIBase should continue executing
		/// </summary>
		public override bool ContinueExecuting()
		{
			if (ScaredByPlayerMovement)
			{
				if (TemptedEntity.GetDistanceSqToEntity(TemptingPlayer) < 36D)
				{
					if (TemptingPlayer.GetDistanceSq(Field_48276_c, Field_48273_d, Field_48274_e) > 0.010000000000000002F)
					{
						return false;
					}

					if (Math.Abs((double)TemptingPlayer.RotationPitch - Field_48271_f) > 5D || Math.Abs((double)TemptingPlayer.RotationYaw - Field_48272_g) > 5D)
					{
						return false;
					}
				}
				else
				{
					Field_48276_c = TemptingPlayer.PosX;
					Field_48273_d = TemptingPlayer.PosY;
					Field_48274_e = TemptingPlayer.PosZ;
				}

				Field_48271_f = TemptingPlayer.RotationPitch;
				Field_48272_g = TemptingPlayer.RotationYaw;
			}

			return ShouldExecute();
		}

		/// <summary>
		/// Execute a one shot task or start executing a continuous task
		/// </summary>
		public override void StartExecuting()
		{
			Field_48276_c = TemptingPlayer.PosX;
			Field_48273_d = TemptingPlayer.PosY;
			Field_48274_e = TemptingPlayer.PosZ;
			Field_48280_j = true;
			Field_48279_m = TemptedEntity.GetNavigator().Func_48658_a();
			TemptedEntity.GetNavigator().Func_48664_a(false);
		}

		/// <summary>
		/// Resets the task
		/// </summary>
		public override void ResetTask()
		{
			TemptingPlayer = null;
			TemptedEntity.GetNavigator().ClearPathEntity();
			DelayTemptCounter = 100;
			Field_48280_j = false;
			TemptedEntity.GetNavigator().Func_48664_a(Field_48279_m);
		}

		/// <summary>
		/// Updates the task
		/// </summary>
		public override void UpdateTask()
		{
			TemptedEntity.GetLookHelper().SetLookPositionWithEntity(TemptingPlayer, 30F, TemptedEntity.GetVerticalFaceSpeed());

			if (TemptedEntity.GetDistanceSqToEntity(TemptingPlayer) < 6.25D)
			{
				TemptedEntity.GetNavigator().ClearPathEntity();
			}
			else
			{
				TemptedEntity.GetNavigator().Func_48667_a(TemptingPlayer, Field_48275_b);
			}
		}

		public virtual bool Func_48270_h()
		{
			return Field_48280_j;
		}
	}
}