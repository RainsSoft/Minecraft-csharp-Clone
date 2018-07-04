using System;

namespace net.minecraft.src
{
	public class EntityCritFX : EntityFX
	{
		float Field_35137_a;

        public EntityCritFX(World par1World, float par2, float par4, float par6, float par8, float par10, float par12)
            : this(par1World, par2, par4, par6, par8, par10, par12, 1.0F)
		{
		}

        public EntityCritFX(World par1World, float par2, float par4, float par6, float par8, float par10, float par12, float par14)
            : base(par1World, par2, par4, par6, 0.0F, 0.0F, 0.0F)
		{
			MotionX *= 0.10000000149011612F;
			MotionY *= 0.10000000149011612F;
			MotionZ *= 0.10000000149011612F;
			MotionX += par8 * 0.40000000000000002F;
			MotionY += par10 * 0.40000000000000002F;
			MotionZ += par12 * 0.40000000000000002F;
			ParticleRed = ParticleGreen = ParticleBlue = (float)((new Random(1)).NextDouble() * 0.30000001192092896D + 0.60000002384185791D);
			ParticleScale *= 0.75F;
			ParticleScale *= par14;
			Field_35137_a = ParticleScale;
			ParticleMaxAge = (int)(6D / ((new Random(2)).NextDouble() * 0.80000000000000004D + 0.59999999999999998D));
			ParticleMaxAge *= (int)par14;
			NoClip = false;
			SetParticleTextureIndex(65);
			OnUpdate();
		}

		public override void RenderParticle(Tessellator par1Tessellator, float par2, float par3, float par4, float par5, float par6, float par7)
		{
			float f = (((float)ParticleAge + par2) / (float)ParticleMaxAge) * 32F;

			if (f < 0.0F)
			{
				f = 0.0F;
			}

			if (f > 1.0F)
			{
				f = 1.0F;
			}

			ParticleScale = Field_35137_a * f;
			base.RenderParticle(par1Tessellator, par2, par3, par4, par5, par6, par7);
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
			ParticleGreen *= 0.95999999999999996F;
			ParticleBlue *= 0.90000000000000002F;
			MotionX *= 0.69999998807907104F;
			MotionY *= 0.69999998807907104F;
			MotionZ *= 0.69999998807907104F;
			MotionY -= 0.019999999552965164F;

			if (OnGround)
			{
				MotionX *= 0.69999998807907104F;
				MotionZ *= 0.69999998807907104F;
			}
		}
	}
}