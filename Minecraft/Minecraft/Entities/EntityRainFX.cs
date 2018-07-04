using System;

namespace net.minecraft.src
{
	public class EntityRainFX : EntityFX
	{
        public EntityRainFX(World par1World, float par2, float par4, float par6)
            : base(par1World, par2, par4, par6, 0.0F, 0.0F, 0.0F)
		{
			MotionX *= 0.30000001192092896F;
			MotionY = (float)(new Random(1)).NextDouble() * 0.2F + 0.1F;
			MotionZ *= 0.30000001192092896F;
			ParticleRed = 1.0F;
			ParticleGreen = 1.0F;
			ParticleBlue = 1.0F;
			SetParticleTextureIndex(19 + Rand.Next(4));
			SetSize(0.01F, 0.01F);
			ParticleGravity = 0.06F;
			ParticleMaxAge = (int)(8D / ((new Random(2)).NextDouble() * 0.80000000000000004D + 0.20000000000000001D));
		}

		/// <summary>
		/// Called to update the entity's position/logic.
		/// </summary>
		public override void OnUpdate()
		{
			PrevPosX = PosX;
			PrevPosY = PosY;
			PrevPosZ = PosZ;
			MotionY -= ParticleGravity;
			MoveEntity(MotionX, MotionY, MotionZ);
			MotionX *= 0.98000001907348633F;
			MotionY *= 0.98000001907348633F;
			MotionZ *= 0.98000001907348633F;

			if (ParticleMaxAge-- <= 0)
			{
				SetDead();
			}

			if (OnGround)
			{
				if ((new Random(1)).NextDouble() < 0.5D)
				{
					SetDead();
				}

				MotionX *= 0.69999998807907104F;
				MotionZ *= 0.69999998807907104F;
			}

			Material material = WorldObj.GetBlockMaterial(MathHelper2.Floor_double(PosX), MathHelper2.Floor_double(PosY), MathHelper2.Floor_double(PosZ));

			if (material.IsLiquid() || material.IsSolid())
			{
				double d = (float)(MathHelper2.Floor_double(PosY) + 1) - BlockFluid.GetFluidHeightPercent(WorldObj.GetBlockMetadata(MathHelper2.Floor_double(PosX), MathHelper2.Floor_double(PosY), MathHelper2.Floor_double(PosZ)));

				if (PosY < d)
				{
					SetDead();
				}
			}
		}
	}
}