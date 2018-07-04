using System;

namespace net.minecraft.src
{
	public class EntityExplodeFX : EntityFX
	{
        public EntityExplodeFX(World par1World, float par2, float par4, float par6, float par8, float par10, float par12)
            : base(par1World, par2, par4, par6, par8, par10, par12)
		{
			MotionX = par8 + ((float)((new Random(1)).NextDouble() * 2 - 1) * 0.05F);
			MotionY = par10 + ((float)((new Random(2)).NextDouble() * 2 - 1) * 0.05F);
			MotionZ = par12 + ((float)((new Random(3)).NextDouble() * 2 - 1) * 0.05F);
			ParticleRed = ParticleGreen = ParticleBlue = Rand.NextFloat() * 0.3F + 0.7F;
			ParticleScale = Rand.NextFloat() * Rand.NextFloat() * 6F + 1.0F;
			ParticleMaxAge = (int)(16D / ((double)Rand.NextFloat() * 0.80000000000000004D + 0.20000000000000001D)) + 2;
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

			SetParticleTextureIndex(7 - (ParticleAge * 8) / ParticleMaxAge);
			MotionY += 0.0040000000000000001F;
			MoveEntity(MotionX, MotionY, MotionZ);
			MotionX *= 0.89999997615814209F;
			MotionY *= 0.89999997615814209F;
			MotionZ *= 0.89999997615814209F;

			if (OnGround)
			{
				MotionX *= 0.69999998807907104F;
				MotionZ *= 0.69999998807907104F;
			}
		}
	}
}