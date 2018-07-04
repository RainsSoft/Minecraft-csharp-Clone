namespace net.minecraft.src
{
	public class EntityAIBeg : EntityAIBase
	{
		private EntityWolf TheWolf;
		private EntityPlayer Field_48348_b;
		private World Field_48349_c;
		private float Field_48346_d;
		private int Field_48347_e;

		public EntityAIBeg(EntityWolf par1EntityWolf, float par2)
		{
			TheWolf = par1EntityWolf;
			Field_48349_c = par1EntityWolf.WorldObj;
			Field_48346_d = par2;
			SetMutexBits(2);
		}

		/// <summary>
		/// Returns whether the EntityAIBase should begin execution.
		/// </summary>
		public override bool ShouldExecute()
		{
			Field_48348_b = Field_48349_c.GetClosestPlayerToEntity(TheWolf, Field_48346_d);

			if (Field_48348_b == null)
			{
				return false;
			}
			else
			{
				return Func_48345_a(Field_48348_b);
			}
		}

		/// <summary>
		/// Returns whether an in-progress EntityAIBase should continue executing
		/// </summary>
		public override bool ContinueExecuting()
		{
			if (!Field_48348_b.IsEntityAlive())
			{
				return false;
			}

			if (TheWolf.GetDistanceSqToEntity(Field_48348_b) > (double)(Field_48346_d * Field_48346_d))
			{
				return false;
			}
			else
			{
				return Field_48347_e > 0 && Func_48345_a(Field_48348_b);
			}
		}

		/// <summary>
		/// Execute a one shot task or start executing a continuous task
		/// </summary>
		public override void StartExecuting()
		{
			TheWolf.Func_48150_h(true);
			Field_48347_e = 40 + TheWolf.GetRNG().Next(40);
		}

		/// <summary>
		/// Resets the task
		/// </summary>
		public override void ResetTask()
		{
			TheWolf.Func_48150_h(false);
			Field_48348_b = null;
		}

		/// <summary>
		/// Updates the task
		/// </summary>
		public override void UpdateTask()
		{
			TheWolf.GetLookHelper().SetLookPosition(Field_48348_b.PosX, Field_48348_b.PosY + (double)Field_48348_b.GetEyeHeight(), Field_48348_b.PosZ, 10F, TheWolf.GetVerticalFaceSpeed());
			Field_48347_e--;
		}

		private bool Func_48345_a(EntityPlayer par1EntityPlayer)
		{
			ItemStack itemstack = par1EntityPlayer.Inventory.GetCurrentItem();

			if (itemstack == null)
			{
				return false;
			}

			if (!TheWolf.IsTamed() && itemstack.ItemID == Item.Bone.ShiftedIndex)
			{
				return true;
			}
			else
			{
				return TheWolf.IsWheat(itemstack);
			}
		}
	}

}