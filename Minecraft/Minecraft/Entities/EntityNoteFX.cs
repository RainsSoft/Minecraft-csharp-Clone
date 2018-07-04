using System;

namespace net.minecraft.src
{
	public class EntityNoteFX : EntityFX
	{
		float NoteParticleScale;

        public EntityNoteFX(World par1World, float par2, float par4, float par6, float par8, float par10, float par12)
            : this(par1World, par2, par4, par6, par8, par10, par12, 2.0F)
		{
		}

        public EntityNoteFX(World par1World, float par2, float par4, float par6, float par8, float par10, float par12, float par14)
            : base(par1World, par2, par4, par6, 0.0F, 0.0F, 0.0F)
		{
			MotionX *= 0.0099999997764825821F;
			MotionY *= 0.0099999997764825821F;
			MotionZ *= 0.0099999997764825821F;
			MotionY += 0.20000000000000001F;
			ParticleRed = MathHelper2.Sin(((float)par8 + 0.0F) * (float)Math.PI * 2.0F) * 0.65F + 0.35F;
			ParticleGreen = MathHelper2.Sin(((float)par8 + 0.3333333F) * (float)Math.PI * 2.0F) * 0.65F + 0.35F;
			ParticleBlue = MathHelper2.Sin(((float)par8 + 0.6666667F) * (float)Math.PI * 2.0F) * 0.65F + 0.35F;
			ParticleScale *= 0.75F;
			ParticleScale *= par14;
			NoteParticleScale = ParticleScale;
			ParticleMaxAge = 6;
			NoClip = false;
			SetParticleTextureIndex(64);
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

			ParticleScale = NoteParticleScale * f;
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

			if (PosY == PrevPosY)
			{
				MotionX *= 1.1000000000000001F;
				MotionZ *= 1.1000000000000001F;
			}

			MotionX *= 0.6600000262260437F;
			MotionY *= 0.6600000262260437F;
			MotionZ *= 0.6600000262260437F;

			if (OnGround)
			{
				MotionX *= 0.69999998807907104F;
				MotionZ *= 0.69999998807907104F;
			}
		}
	}
}