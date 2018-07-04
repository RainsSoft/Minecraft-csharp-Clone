namespace net.minecraft.src
{
	public class EntityEnderPearl : EntityThrowable
	{
		public EntityEnderPearl(World par1World) : base(par1World)
		{
		}

		public EntityEnderPearl(World par1World, EntityLiving par2EntityLiving) : base(par1World, par2EntityLiving)
		{
		}

        public EntityEnderPearl(World par1World, float par2, float par4, float par6)
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

			for (int i = 0; i < 32; i++)
			{
				WorldObj.SpawnParticle("portal", PosX, PosY + Rand.NextFloat() * 2, PosZ, Rand.NextGaussian(), 0.0F, Rand.NextGaussian());
			}

			if (!WorldObj.IsRemote)
			{
				if (Thrower != null)
				{
					Thrower.SetPositionAndUpdate(PosX, PosY, PosZ);
					Thrower.FallDistance = 0.0F;
					Thrower.AttackEntityFrom(DamageSource.Fall, 5);
				}

				SetDead();
			}
		}
	}
}