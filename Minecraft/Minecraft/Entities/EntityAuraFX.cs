using System;

namespace net.minecraft.src
{
	public class EntityAuraFX : EntityFX
	{
        public EntityAuraFX(World par1World, float par2, float par4, float par6, float par8, float par10, float par12)
            : base(par1World, par2, par4, par6, par8, par10, par12)
		{
			float f = Rand.NextFloat() * 0.1F + 0.2F;
			ParticleRed = f;
			ParticleGreen = f;
			ParticleBlue = f;
			SetParticleTextureIndex(0);
			SetSize(0.02F, 0.02F);
			ParticleScale = ParticleScale * (Rand.NextFloat() * 0.6F + 0.5F);
			MotionX *= 0.019999999552965164F;
			MotionY *= 0.019999999552965164F;
			MotionZ *= 0.019999999552965164F;
			ParticleMaxAge = (int)(20D / ((new Random(1)).NextDouble() * 0.80000000000000004D + 0.20000000000000001D));
			NoClip = true;
		}

		/// <summary>
		/// Called to update the entity's position/logic.
		/// </summary>
		public override void OnUpdate()
		{
			PrevPosX = PosX;
			PrevPosY = PosY;
			PrevPosZ = PosZ;
			MoveEntity(MotionX, MotionY, MotionZ);
			MotionX *= 0.98999999999999999F;
			MotionY *= 0.98999999999999999F;
			MotionZ *= 0.98999999999999999F;

			if (ParticleMaxAge-- <= 0)
			{
				SetDead();
			}
		}
	}
}