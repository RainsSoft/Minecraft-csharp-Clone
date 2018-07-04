using System;

namespace net.minecraft.src
{
	public class EntityExpBottle : EntityThrowable
	{
		public EntityExpBottle(World par1World) : base(par1World)
		{
		}

		public EntityExpBottle(World par1World, EntityLiving par2EntityLiving) : base(par1World, par2EntityLiving)
		{
		}

        public EntityExpBottle(World par1World, float par2, float par4, float par6)
            : base(par1World, par2, par4, par6)
		{
		}

		protected override float Func_40075_e()
		{
			return 0.07F;
		}

		protected override float Func_40077_c()
		{
			return 0.7F;
		}

		protected override float Func_40074_d()
		{
			return -20F;
		}

		/// <summary>
		/// Called when the throwable hits a block or entity.
		/// </summary>
		protected override void OnImpact(MovingObjectPosition par1MovingObjectPosition)
		{
			if (!WorldObj.IsRemote)
			{
				WorldObj.PlayAuxSFX(2002, (int)Math.Round(PosX), (int)Math.Round(PosY), (int)Math.Round(PosZ), 0);

				for (int i = 3 + WorldObj.Rand.Next(5) + WorldObj.Rand.Next(5); i > 0;)
				{
					int j = EntityXPOrb.GetXPSplit(i);
					i -= j;
					WorldObj.SpawnEntityInWorld(new EntityXPOrb(WorldObj, PosX, PosY, PosZ, j));
				}

				SetDead();
			}
		}
	}

}