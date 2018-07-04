using System;

namespace net.minecraft.src
{
	public class EntityPortalFX : EntityFX
	{
		private float PortalParticleScale;
        private float PortalPosX;
        private float PortalPosY;
        private float PortalPosZ;

        public EntityPortalFX(World par1World, float par2, float par4, float par6, float par8, float par10, float par12)
            : base(par1World, par2, par4, par6, par8, par10, par12)
		{
			MotionX = par8;
			MotionY = par10;
			MotionZ = par12;
			PortalPosX = PosX = par2;
			PortalPosY = PosY = par4;
			PortalPosZ = PosZ = par6;
			float f = Rand.NextFloat() * 0.6F + 0.4F;
			PortalParticleScale = ParticleScale = Rand.NextFloat() * 0.2F + 0.5F;
			ParticleRed = ParticleGreen = ParticleBlue = 1.0F * f;
			ParticleGreen *= 0.3F;
			ParticleRed *= 0.9F;
			ParticleMaxAge = (int)((new Random(1)).NextDouble() * 10D) + 40;
			NoClip = true;
			SetParticleTextureIndex((int)((new Random(2)).NextDouble() * 8D));
		}

		public override void RenderParticle(Tessellator par1Tessellator, float par2, float par3, float par4, float par5, float par6, float par7)
		{
			float f = ((float)ParticleAge + par2) / (float)ParticleMaxAge;
			f = 1.0F - f;
			f *= f;
			f = 1.0F - f;
			ParticleScale = PortalParticleScale * f;
			base.RenderParticle(par1Tessellator, par2, par3, par4, par5, par6, par7);
		}

		public override int GetBrightnessForRender(float par1)
		{
			int i = base.GetBrightnessForRender(par1);
			float f = (float)ParticleAge / (float)ParticleMaxAge;
			f *= f;
			f *= f;
			int j = i & 0xff;
			int k = i >> 16 & 0xff;
			k += (int)(f * 15F * 16F);

			if (k > 240)
			{
				k = 240;
			}

			return j | k << 16;
		}

		/// <summary>
		/// Gets how bright this entity is.
		/// </summary>
		public override float GetBrightness(float par1)
		{
			float f = base.GetBrightness(par1);
			float f1 = (float)ParticleAge / (float)ParticleMaxAge;
			f1 = f1 * f1 * f1 * f1;
			return f * (1.0F - f1) + f1;
		}

		/// <summary>
		/// Called to update the entity's position/logic.
		/// </summary>
		public override void OnUpdate()
		{
			PrevPosX = PosX;
			PrevPosY = PosY;
			PrevPosZ = PosZ;
			float f = (float)ParticleAge / (float)ParticleMaxAge;
			float f1 = f;
			f = -f + f * f * 2.0F;
			f = 1.0F - f;
			PosX = PortalPosX + MotionX * f;
			PosY = PortalPosY + MotionY * f + (1.0F - f1);
			PosZ = PortalPosZ + MotionZ * f;

			if (ParticleAge++ >= ParticleMaxAge)
			{
				SetDead();
			}
		}
	}
}