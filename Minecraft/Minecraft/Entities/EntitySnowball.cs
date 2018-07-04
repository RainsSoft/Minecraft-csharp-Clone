namespace net.minecraft.src
{
	public class EntitySnowball : EntityThrowable
	{
		public EntitySnowball(World par1World) : base(par1World)
		{
		}

		public EntitySnowball(World par1World, EntityLiving par2EntityLiving) : base(par1World, par2EntityLiving)
		{
		}

        public EntitySnowball(World par1World, float par2, float par4, float par6)
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
				sbyte byte0 = 0;

				if (par1MovingObjectPosition.EntityHit is EntityBlaze)
				{
					byte0 = 3;
				}

				if (!par1MovingObjectPosition.EntityHit.AttackEntityFrom(DamageSource.CauseThrownDamage(this, Thrower), byte0))
				{
					;
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