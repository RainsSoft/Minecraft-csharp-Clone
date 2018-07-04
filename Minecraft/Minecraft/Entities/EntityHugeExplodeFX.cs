namespace net.minecraft.src
{
	public class EntityHugeExplodeFX : EntityFX
	{
		private int TimeSinceStart;

		/// <summary>
		/// the maximum time for the explosion </summary>
		private int MaximumTime;

        public EntityHugeExplodeFX(World par1World, float par2, float par4, float par6, float par8, float par10, float par12)
            : base(par1World, par2, par4, par6, 0.0F, 0.0F, 0.0F)
		{
			TimeSinceStart = 0;
			MaximumTime = 0;
			MaximumTime = 8;
		}

		public override void RenderParticle(Tessellator tessellator, float f, float f1, float f2, float f3, float f4, float f5)
		{
		}

		/// <summary>
		/// Called to update the entity's position/logic.
		/// </summary>
		public override void OnUpdate()
		{
			for (int i = 0; i < 6; i++)
			{
                float d = PosX + (Rand.NextFloat() - Rand.NextFloat()) * 4F;
                float d1 = PosY + (Rand.NextFloat() - Rand.NextFloat()) * 4F;
                float d2 = PosZ + (Rand.NextFloat() - Rand.NextFloat()) * 4F;
				WorldObj.SpawnParticle("largeexplode", d, d1, d2, (float)TimeSinceStart / (float)MaximumTime, 0.0F, 0.0F);
			}

			TimeSinceStart++;

			if (TimeSinceStart == MaximumTime)
			{
				SetDead();
			}
		}

		public override int GetFXLayer()
		{
			return 1;
		}
	}
}