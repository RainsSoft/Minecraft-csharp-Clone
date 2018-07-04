using System;

namespace net.minecraft.src
{
	public class EntitySuspendFX : EntityFX
	{
        public EntitySuspendFX(World par1World, float par2, float par4, float par6, float par8, float par10, float par12)
            : base(par1World, par2, par4 - 0.125F, par6, par8, par10, par12)
		{
			ParticleRed = 0.4F;
			ParticleGreen = 0.4F;
			ParticleBlue = 0.7F;
			SetParticleTextureIndex(0);
			SetSize(0.01F, 0.01F);
			ParticleScale = ParticleScale * (Rand.NextFloat() * 0.6F + 0.2F);
			MotionX = par8 * 0.0F;
			MotionY = par10 * 0.0F;
			MotionZ = par12 * 0.0F;
			ParticleMaxAge = (int)(16D / ((new Random(1)).NextDouble() * 0.80000000000000004D + 0.20000000000000001D));
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