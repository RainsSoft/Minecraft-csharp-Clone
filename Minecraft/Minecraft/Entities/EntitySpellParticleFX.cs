using System;

namespace net.minecraft.src
{
	public class EntitySpellParticleFX : EntityFX
	{
		private int Field_40111_a;

        public EntitySpellParticleFX(World par1World, float par2, float par4, float par6, float par8, float par10, float par12)
            : base(par1World, par2, par4, par6, par8, par10, par12)
		{
			Field_40111_a = 128;
			MotionY *= 0.20000000298023224F;

			if (par8 == 0.0F && par12 == 0.0F)
			{
				MotionX *= 0.10000000149011612F;
				MotionZ *= 0.10000000149011612F;
			}

			ParticleScale *= 0.75F;
			ParticleMaxAge = (int)(8D / ((new Random(1)).NextDouble() * 0.80000000000000004D + 0.20000000000000001D));
			NoClip = false;
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

			SetParticleTextureIndex(Field_40111_a + (7 - (ParticleAge * 8) / ParticleMaxAge));
			MotionY += 0.0040000000000000001F;
			MoveEntity(MotionX, MotionY, MotionZ);

			if (PosY == PrevPosY)
			{
				MotionX *= 1.1000000000000001F;
				MotionZ *= 1.1000000000000001F;
			}

			MotionX *= 0.95999997854232788F;
			MotionY *= 0.95999997854232788F;
			MotionZ *= 0.95999997854232788F;

			if (OnGround)
			{
				MotionX *= 0.69999998807907104F;
				MotionZ *= 0.69999998807907104F;
			}
		}

		public virtual void Func_40110_b(int par1)
		{
			Field_40111_a = par1;
		}
	}
}