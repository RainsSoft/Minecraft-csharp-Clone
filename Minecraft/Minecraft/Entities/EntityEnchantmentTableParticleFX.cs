using System;

namespace net.minecraft.src
{
	public class EntityEnchantmentTableParticleFX : EntityFX
	{
		private float Field_40107_a;
        private float Field_40109_aw;
        private float Field_40108_ax;
        private float Field_40106_ay;

        public EntityEnchantmentTableParticleFX(World par1World, float par2, float par4, float par6, float par8, float par10, float par12)
            : base(par1World, par2, par4, par6, par8, par10, par12)
		{
			MotionX = par8;
			MotionY = par10;
			MotionZ = par12;
			Field_40109_aw = PosX = par2;
			Field_40108_ax = PosY = par4;
			Field_40106_ay = PosZ = par6;
			float f = Rand.NextFloat() * 0.6F + 0.4F;
			Field_40107_a = ParticleScale = Rand.NextFloat() * 0.5F + 0.2F;
			ParticleRed = ParticleGreen = ParticleBlue = 1.0F * f;
			ParticleGreen *= 0.9F;
			ParticleRed *= 0.9F;
			ParticleMaxAge = (int)((new Random(1)).NextDouble() * 10D) + 30;
			NoClip = true;
			SetParticleTextureIndex((int)((new Random(2)).NextDouble() * 26D + 1.0D + 224D));
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
			f1 *= f1;
			f1 *= f1;
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
			f = 1.0F - f;
			float f1 = 1.0F - f;
			f1 *= f1;
			f1 *= f1;
			PosX = Field_40109_aw + MotionX * f;
			PosY = (Field_40108_ax + MotionY * f) - (f1 * 1.2F);
			PosZ = Field_40106_ay + MotionZ * f;

			if (ParticleAge++ >= ParticleMaxAge)
			{
				SetDead();
			}
		}
	}
}