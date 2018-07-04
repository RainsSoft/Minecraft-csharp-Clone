namespace net.minecraft.src
{
	public class EntityEgg : EntityThrowable
	{
		public EntityEgg(World par1World) : base(par1World)
		{
		}

		public EntityEgg(World par1World, EntityLiving par2EntityLiving) : base(par1World, par2EntityLiving)
		{
		}

        public EntityEgg(World par1World, float par2, float par4, float par6)
            : base(par1World, par2, par4, par6)
		{
		}

		/// <summary>
		/// Called when the throwable hits a block or entity.
		/// </summary>
		protected override void OnImpact(MovingObjectPosition par1MovingObjectPosition)
		{
			if (par1MovingObjectPosition.EntityHit != null)
			{
				if (!par1MovingObjectPosition.EntityHit.AttackEntityFrom(DamageSource.CauseThrownDamage(this, Thrower), 0))
				{
					;
				}
			}

			if (!WorldObj.IsRemote && Rand.Next(8) == 0)
			{
				sbyte byte0 = 1;

				if (Rand.Next(32) == 0)
				{
					byte0 = 4;
				}

				for (int j = 0; j < byte0; j++)
				{
					EntityChicken entitychicken = new EntityChicken(WorldObj);
					entitychicken.SetGrowingAge(-24000);
					entitychicken.SetLocationAndAngles(PosX, PosY, PosZ, RotationYaw, 0.0F);
					WorldObj.SpawnEntityInWorld(entitychicken);
				}
			}

			for (int i = 0; i < 8; i++)
			{
				WorldObj.SpawnParticle("snowballpoof", PosX, PosY, PosZ, 0.0F, 0.0F, 0.0F);
			}

			if (!WorldObj.IsRemote)
			{
				SetDead();
			}
		}
	}
}