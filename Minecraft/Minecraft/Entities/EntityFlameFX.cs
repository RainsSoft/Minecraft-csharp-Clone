using System;

namespace net.minecraft.src
{
	public class EntityFlameFX : EntityFX
	{
		/// <summary>
		/// the scale of the flame FX </summary>
		private float FlameScale;

        public EntityFlameFX(World par1World, float par2, float par4, float par6, float par8, float par10, float par12)
            : base(par1World, par2, par4, par6, par8, par10, par12)
		{
			MotionX = MotionX * 0.0099999997764825821F + par8;
			MotionY = MotionY * 0.0099999997764825821F + par10;
			MotionZ = MotionZ * 0.0099999997764825821F + par12;
			par2 += (Rand.NextFloat() - Rand.NextFloat()) * 0.05F;
			par4 += (Rand.NextFloat() - Rand.NextFloat()) * 0.05F;
			par6 += (Rand.NextFloat() - Rand.NextFloat()) * 0.05F;
			FlameScale = ParticleScale;
			ParticleRed = ParticleGreen = ParticleBlue = 1.0F;
			ParticleMaxAge = (int)(8D / ((new Random(1)).NextDouble() * 0.80000000000000004D + 0.20000000000000001D)) + 4;
			NoClip = true;
			SetParticleTextureIndex(48);
		}

		public override void RenderParticle(Tessellator par1Tessellator, float par2, float par3, float par4, float par5, float par6, float par7)
		{
			float f = ((float)ParticleAge + par2) / (float)ParticleMaxAge;
			ParticleScale = FlameScale * (1.0F - f * f * 0.5F);
			base.RenderParticle(par1Tessellator, par2, par3, par4, par5, par6, par7);
		}

		public override int GetBrightnessForRender(float par1)
		{
			float f = ((float)ParticleAge + par1) / (float)ParticleMaxAge;

			if (f < 0.0F)
			{
				f = 0.0F;
			}

			if (f > 1.0F)
			{
				f = 1.0F;
			}

			int i = base.GetBrightnessForRender(par1);
			int j = i & 0xff;
			int k = i >> 16 & 0xff;
			j += (int)(f * 15F * 16F);

			if (j > 240)
			{
				j = 240;
			}

			return j | k << 16;
		}

		/// <summary>
		/// Gets how bright this entity is.
		/// </summary>
		public override float GetBrightness(float par1)
		{
			float f = ((float)ParticleAge + par1) / (float)ParticleMaxAge;

			if (f < 0.0F)
			{
				f = 0.0F;
			}

			if (f > 1.0F)
			{
				f = 1.0F;
			}

			float f1 = base.GetBrightness(par1);
			return f1 * f + (1.0F - f);
		}

		/// <summary>
		/// Called to update the entity's position/logic.
		/// </summary>
		public override void OnUpdate()
		{
			PrevPosX = PosX;
			PrevPosY = PosY;
			PrevPosZ = PosZ;

			if (ParticleAge++ >= ParticleMaxAge)
			{
				SetDead();
			}

			MoveEntity(MotionX, MotionY, MotionZ);
			MotionX *= 0.95999997854232788F;
			MotionY *= 0.95999997854232788F;
			MotionZ *= 0.95999997854232788F;

			if (OnGround)
			{
				MotionX *= 0.69999998807907104F;
				MotionZ *= 0.69999998807907104F;
			}
		}
	}
}