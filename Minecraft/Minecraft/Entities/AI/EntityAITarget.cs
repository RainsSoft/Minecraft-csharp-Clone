namespace net.minecraft.src
{
	public abstract class EntityAITarget : EntityAIBase
	{
		/// <summary>
		/// The entity that this task belongs to </summary>
		protected EntityLiving TaskOwner;
		protected float Field_48379_d;
		protected bool Field_48380_e;
		private bool Field_48383_a;
		private int Field_48381_b;
		private int Field_48377_f;
		private int Field_48378_g;

		public EntityAITarget(EntityLiving par1EntityLiving, float par2, bool par3) : this(par1EntityLiving, par2, par3, false)
		{
		}

		public EntityAITarget(EntityLiving par1EntityLiving, float par2, bool par3, bool par4)
		{
			Field_48381_b = 0;
			Field_48377_f = 0;
			Field_48378_g = 0;
			TaskOwner = par1EntityLiving;
			Field_48379_d = par2;
			Field_48380_e = par3;
			Field_48383_a = par4;
		}

		/// <summary>
		/// Returns whether an in-progress EntityAIBase should continue executing
		/// </summary>
		public override bool ContinueExecuting()
		{
			EntityLiving entityliving = TaskOwner.GetAttackTarget();

			if (entityliving == null)
			{
				return false;
			}

			if (!entityliving.IsEntityAlive())
			{
				return false;
			}

			if (TaskOwner.GetDistanceSqToEntity(entityliving) > (double)(Field_48379_d * Field_48379_d))
			{
				return false;
			}

			if (Field_48380_e)
			{
				if (!TaskOwner.Func_48090_aM().CanSee(entityliving))
				{
					if (++Field_48378_g > 60)
					{
						return false;
					}
				}
				else
				{
					Field_48378_g = 0;
				}
			}

			return true;
		}

		/// <summary>
		/// Execute a one shot task or start executing a continuous task
		/// </summary>
		public override void StartExecuting()
		{
			Field_48381_b = 0;
			Field_48377_f = 0;
			Field_48378_g = 0;
		}

		/// <summary>
		/// Resets the task
		/// </summary>
		public override void ResetTask()
		{
			TaskOwner.SetAttackTarget(null);
		}

		protected virtual bool Func_48376_a(EntityLiving par1EntityLiving, bool par2)
		{
			if (par1EntityLiving == null)
			{
				return false;
			}

			if (par1EntityLiving == TaskOwner)
			{
				return false;
			}

			if (!par1EntityLiving.IsEntityAlive())
			{
				return false;
			}

			if (par1EntityLiving.BoundingBox.MaxY <= TaskOwner.BoundingBox.MinY || par1EntityLiving.BoundingBox.MinY >= TaskOwner.BoundingBox.MaxY)
			{
				return false;
			}

			if (!TaskOwner.Func_48100_a(par1EntityLiving.GetType()))
			{
				return false;
			}

			if ((TaskOwner is EntityTameable) && ((EntityTameable)TaskOwner).IsTamed())
			{
				if ((par1EntityLiving is EntityTameable) && ((EntityTameable)par1EntityLiving).IsTamed())
				{
					return false;
				}

				if (par1EntityLiving == ((EntityTameable)TaskOwner).GetOwner())
				{
					return false;
				}
			}
			else if ((par1EntityLiving is EntityPlayer) && !par2 && ((EntityPlayer)par1EntityLiving).Capabilities.DisableDamage)
			{
				return false;
			}

			if (!TaskOwner.IsWithinHomeDistance(MathHelper2.Floor_double(par1EntityLiving.PosX), MathHelper2.Floor_double(par1EntityLiving.PosY), MathHelper2.Floor_double(par1EntityLiving.PosZ)))
			{
				return false;
			}

			if (Field_48380_e && !TaskOwner.Func_48090_aM().CanSee(par1EntityLiving))
			{
				return false;
			}

			if (Field_48383_a)
			{
				if (--Field_48377_f <= 0)
				{
					Field_48381_b = 0;
				}

				if (Field_48381_b == 0)
				{
					Field_48381_b = Func_48375_a(par1EntityLiving) ? 1 : 2;
				}

				if (Field_48381_b == 2)
				{
					return false;
				}
			}

			return true;
		}

		private bool Func_48375_a(EntityLiving par1EntityLiving)
		{
			Field_48377_f = 10 + TaskOwner.GetRNG().Next(5);
			PathEntity pathentity = TaskOwner.GetNavigator().Func_48679_a(par1EntityLiving);

			if (pathentity == null)
			{
				return false;
			}

			PathPoint pathpoint = pathentity.GetFinalPathPoint();

			if (pathpoint == null)
			{
				return false;
			}
			else
			{
				int i = pathpoint.XCoord - MathHelper2.Floor_double(par1EntityLiving.PosX);
				int j = pathpoint.ZCoord - MathHelper2.Floor_double(par1EntityLiving.PosZ);
				return (double)(i * i + j * j) <= 2.25D;
			}
		}
	}
}