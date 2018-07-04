using System;

namespace net.minecraft.src
{
	public class EntityBubbleFX : EntityFX
	{
        public EntityBubbleFX(World par1World, float par2, float par4, float par6, float par8, float par10, float par12)
            : base(par1World, par2, par4, par6, par8, par10, par12)
		{
			ParticleRed = 1.0F;
			ParticleGreen = 1.0F;
			ParticleBlue = 1.0F;
			SetParticleTextureIndex(32);
			SetSize(0.02F, 0.02F);
			ParticleScale = ParticleScale * (Rand.NextFloat() * 0.6F + 0.2F);
			MotionX = par8 * 0.20000000298023224F + ((float)((new Random(1)).NextDouble() * 2 - 1) * 0.02F);
			MotionY = par10 * 0.20000000298023224F + ((float)((new Random(2)).NextDouble() * 2 - 1) * 0.02F);
			MotionZ = par12 * 0.20000000298023224F + ((float)((new Random(3)).NextDouble() * 2 - 1) * 0.02F);
			ParticleMaxAge = (int)(8D / ((new Random(4)).NextDouble() * 0.80000000000000004D + 0.20000000000000001D));
		}

		/// <summary>
		/// Called to update the entity's position/logic.
		/// </summary>
		public override void OnUpdate()
		{
			PrevPosX = PosX;
			PrevPosY = PosY;
			PrevPosZ = PosZ;
			MotionY += 0.002F;
			MoveEntity(MotionX, MotionY, MotionZ);
			MotionX *= 0.85000002384185791F;
			MotionY *= 0.85000002384185791F;
			MotionZ *= 0.85000002384185791F;

			if (WorldObj.GetBlockMaterial(MathHelper2.Floor_double(PosX), MathHelper2.Floor_double(PosY), MathHelper2.Floor_double(PosZ)) != Material.Water)
			{
				SetDead();
			}

			if (ParticleMaxAge-- <= 0)
			{
				SetDead();
			}
		}
	}

}