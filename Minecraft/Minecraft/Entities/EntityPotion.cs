using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class EntityPotion : EntityThrowable
	{
		/// <summary>
		/// The damage value of the thrown potion that this EntityPotion represents.
		/// </summary>
		private int PotionDamage;

		public EntityPotion(World par1World) : base(par1World)
		{
		}

		public EntityPotion(World par1World, EntityLiving par2EntityLiving, int par3) : base(par1World, par2EntityLiving)
		{
			PotionDamage = par3;
		}

        public EntityPotion(World par1World, float par2, float par4, float par6, int par8)
            : base(par1World, par2, par4, par6)
		{
			PotionDamage = par8;
		}

		protected override float Func_40075_e()
		{
			return 0.05F;
		}

		protected override float Func_40077_c()
		{
			return 0.5F;
		}

		protected override float Func_40074_d()
		{
			return -20F;
		}

		/// <summary>
		/// Returns the damage value of the thrown potion that this EntityPotion represents.
		/// </summary>
		public virtual int GetPotionDamage()
		{
			return PotionDamage;
		}

		/// <summary>
		/// Called when the throwable hits a block or entity.
		/// </summary>
		protected override void OnImpact(MovingObjectPosition par1MovingObjectPosition)
		{
			if (!WorldObj.IsRemote)
			{
				List<PotionEffect> list = Item.Potion.GetEffects(PotionDamage);

				if (list != null && list.Count > 0)
				{
					AxisAlignedBB axisalignedbb = BoundingBox.Expand(4, 2, 4);
					List<Entity> list1 = WorldObj.GetEntitiesWithinAABB(typeof(net.minecraft.src.EntityLiving), axisalignedbb);

					if (list1 != null && list1.Count > 0)
					{
						for (IEnumerator<Entity> iterator = list1.GetEnumerator(); iterator.MoveNext();)
						{
							Entity entity = iterator.Current;
							double d = GetDistanceSqToEntity(entity);

							if (d < 16D)
							{
								double d1 = 1.0D - Math.Sqrt(d) / 4D;

								if (entity == par1MovingObjectPosition.EntityHit)
								{
									d1 = 1.0D;
								}

								IEnumerator<PotionEffect> iterator1 = list.GetEnumerator();

								while (iterator1.MoveNext())
								{
									PotionEffect potioneffect = iterator1.Current;
									int i = potioneffect.GetPotionID();

									if (Potion.PotionTypes[i].IsInstant())
									{
										Potion.PotionTypes[i].AffectEntity(Thrower, (EntityLiving)entity, potioneffect.GetAmplifier(), d1);
									}
									else
									{
										int j = (int)(d1 * (double)potioneffect.GetDuration() + 0.5D);

										if (j > 20)
										{
											((EntityLiving)entity).AddPotionEffect(new PotionEffect(i, j, potioneffect.GetAmplifier()));
										}
									}
								}
							}
						}
					}
				}

				WorldObj.PlayAuxSFX(2002, (int)Math.Round(PosX), (int)Math.Round(PosY), (int)Math.Round(PosZ), PotionDamage);
				SetDead();
			}
		}
	}
}