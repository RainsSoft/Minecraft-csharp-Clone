namespace net.minecraft.src
{
	public class EntitySmallFireball : EntityFireball
	{
		public EntitySmallFireball(World par1World) : base(par1World)
		{
			SetSize(0.3125F, 0.3125F);
		}

        public EntitySmallFireball(World par1World, EntityLiving par2EntityLiving, float par3, float par5, float par7)
            : base(par1World, par2EntityLiving, par3, par5, par7)
		{
			SetSize(0.3125F, 0.3125F);
		}

        public EntitySmallFireball(World par1World, float par2, float par4, float par6, float par8, float par10, float par12)
            : base(par1World, par2, par4, par6, par8, par10, par12)
		{
			SetSize(0.3125F, 0.3125F);
		}

		protected override void Func_40071_a(MovingObjectPosition par1MovingObjectPosition)
		{
			if (!WorldObj.IsRemote)
			{
				if (par1MovingObjectPosition.EntityHit != null)
				{
					if (!par1MovingObjectPosition.EntityHit.IsImmuneToFire() && par1MovingObjectPosition.EntityHit.AttackEntityFrom(DamageSource.CauseFireballDamage(this, ShootingEntity), 5))
					{
						par1MovingObjectPosition.EntityHit.SetFire(5);
					}
				}
				else
				{
					int i = par1MovingObjectPosition.BlockX;
					int j = par1MovingObjectPosition.BlockY;
					int k = par1MovingObjectPosition.BlockZ;

					switch (par1MovingObjectPosition.SideHit)
					{
						case 1:
							j++;
							break;

						case 0:
							j--;
							break;

						case 2:
							k--;
							break;

						case 3:
							k++;
							break;

						case 5:
							i++;
							break;

						case 4:
							i--;
							break;
					}

					if (WorldObj.IsAirBlock(i, j, k))
					{
						WorldObj.SetBlockWithNotify(i, j, k, Block.Fire.BlockID);
					}
				}

				SetDead();
			}
		}

		/// <summary>
		/// Returns true if other Entities should be prevented from moving through this Entity.
		/// </summary>
		public override bool CanBeCollidedWith()
		{
			return false;
		}

		/// <summary>
		/// Called when the entity is attacked.
		/// </summary>
		public override bool AttackEntityFrom(DamageSource par1DamageSource, int par2)
		{
			return false;
		}
	}
}