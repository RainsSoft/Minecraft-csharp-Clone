namespace net.minecraft.src
{
	public class EntityCrit2FX : EntityFX
	{
		private Entity Field_35134_a;
		private int CurrentLife;
		private int MaximumLife;
		private string ParticleName;

		public EntityCrit2FX(World par1World, Entity par2Entity)
            : this(par1World, par2Entity, "crit")
		{
		}

		public EntityCrit2FX(World par1World, Entity par2Entity, string par3Str)
            : base(par1World, par2Entity.PosX, par2Entity.BoundingBox.MinY + (par2Entity.Height / 2.0F), par2Entity.PosZ, par2Entity.MotionX, par2Entity.MotionY, par2Entity.MotionZ)
		{
			CurrentLife = 0;
			MaximumLife = 0;
			Field_35134_a = par2Entity;
			MaximumLife = 3;
			ParticleName = par3Str;
			OnUpdate();
		}

		public override void RenderParticle(Tessellator tessellator, float f, float f1, float f2, float f3, float f4, float f5)
		{
		}

		/// <summary>
		/// Called to update the entity's position/logic.
		/// </summary>
		public override void OnUpdate()
		{
			for (int i = 0; i < 16; i++)
			{
                float d = Rand.NextFloat() * 2.0F - 1.0F;
                float d1 = Rand.NextFloat() * 2.0F - 1.0F;
                float d2 = Rand.NextFloat() * 2.0F - 1.0F;

				if (d * d + d1 * d1 + d2 * d2 <= 1.0D)
				{
                    float d3 = Field_35134_a.PosX + (d * Field_35134_a.Width) / 4;
                    float d4 = Field_35134_a.BoundingBox.MinY + (Field_35134_a.Height / 2.0F) + (d1 * Field_35134_a.Height) / 4;
                    float d5 = Field_35134_a.PosZ + (d2 * Field_35134_a.Width) / 4;
					WorldObj.SpawnParticle(ParticleName, d3, d4, d5, d, d1 + 0.20000000000000001F, d2);
				}
			}

			CurrentLife++;

			if (CurrentLife >= MaximumLife)
			{
				SetDead();
			}
		}

		public override int GetFXLayer()
		{
			return 3;
		}
	}
}