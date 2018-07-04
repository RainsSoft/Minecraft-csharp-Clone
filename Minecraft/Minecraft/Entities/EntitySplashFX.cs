namespace net.minecraft.src
{
	public class EntitySplashFX : EntityRainFX
	{
        public EntitySplashFX(World par1World, float par2, float par4, float par6, float par8, float par10, float par12)
            : base(par1World, par2, par4, par6)
		{
			ParticleGravity = 0.04F;
			SetParticleTextureIndex(GetParticleTextureIndex() + 1);

			if (par10 == 0.0F && (par8 != 0.0F || par12 != 0.0F))
			{
				MotionX = par8;
				MotionY = par10 + 0.10000000000000001F;
				MotionZ = par12;
			}
		}
	}
}