namespace net.minecraft.src
{
	public class EntityCaveSpider : EntitySpider
	{
		public EntityCaveSpider(World par1World) : base(par1World)
		{
			Texture = "/mob/cavespider.png";
			SetSize(0.7F, 0.5F);
		}

		public override int GetMaxHealth()
		{
			return 12;
		}

		/// <summary>
		/// How large the spider should be scaled.
		/// </summary>
		public override float SpiderScaleAmount()
		{
			return 0.7F;
		}

		public override bool AttackEntityAsMob(Entity par1Entity)
		{
			if (base.AttackEntityAsMob(par1Entity))
			{
				if (par1Entity is EntityLiving)
				{
					sbyte byte0 = 0;

					if (WorldObj.DifficultySetting > 1)
					{
						if (WorldObj.DifficultySetting == 2)
						{
							byte0 = 7;
						}
						else if (WorldObj.DifficultySetting == 3)
						{
							byte0 = 15;
						}
					}

					if (byte0 > 0)
					{
						((EntityLiving)par1Entity).AddPotionEffect(new PotionEffect(Potion.Poison.Id, byte0 * 20, 0));
					}
				}

				return true;
			}
			else
			{
				return false;
			}
		}
	}
}