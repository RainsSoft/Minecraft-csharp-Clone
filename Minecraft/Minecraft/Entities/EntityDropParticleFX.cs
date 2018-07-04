using System;

namespace net.minecraft.src
{
	public class EntityDropParticleFX : EntityFX
	{
		/// <summary>
		/// the material type for dropped items/blocks </summary>
		private Material MaterialType;

		/// <summary>
		/// The height of the current bob </summary>
		private int BobTimer;

        public EntityDropParticleFX(World par1World, float par2, float par4, float par6, Material par8Material)
            : base(par1World, par2, par4, par6, 0.0F, 0.0F, 0.0F)
		{
			MotionX = MotionY = MotionZ = 0.0F;

			if (par8Material == Material.Water)
			{
				ParticleRed = 0.0F;
				ParticleGreen = 0.0F;
				ParticleBlue = 1.0F;
			}
			else
			{
				ParticleRed = 1.0F;
				ParticleGreen = 0.0F;
				ParticleBlue = 0.0F;
			}

			SetParticleTextureIndex(113);
			SetSize(0.01F, 0.01F);
			ParticleGravity = 0.06F;
			MaterialType = par8Material;
			BobTimer = 40;
			ParticleMaxAge = (int)(64D / ((new Random(1)).NextDouble() * 0.80000000000000004D + 0.20000000000000001D));
			MotionX = MotionY = MotionZ = 0.0F;
		}

		public override int GetBrightnessForRender(float par1)
		{
			if (MaterialType == Material.Water)
			{
				return base.GetBrightnessForRender(par1);
			}
			else
			{
				return 257;
			}
		}

		/// <summary>
		/// Gets how bright this entity is.
		/// </summary>
		public override float GetBrightness(float par1)
		{
			if (MaterialType == Material.Water)
			{
				return base.GetBrightness(par1);
			}
			else
			{
				return 1.0F;
			}
		}

		/// <summary>
		/// Called to update the entity's position/logic.
		/// </summary>
		public override void OnUpdate()
		{
			PrevPosX = PosX;
			PrevPosY = PosY;
			PrevPosZ = PosZ;

			if (MaterialType == Material.Water)
			{
				ParticleRed = 0.2F;
				ParticleGreen = 0.3F;
				ParticleBlue = 1.0F;
			}
			else
			{
				ParticleRed = 1.0F;
				ParticleGreen = 16F / (float)((40 - BobTimer) + 16);
				ParticleBlue = 4F / (float)((40 - BobTimer) + 8);
			}

			MotionY -= ParticleGravity;

			if (BobTimer-- > 0)
			{
				MotionX *= 0.02F;
				MotionY *= 0.02F;
				MotionZ *= 0.02F;
				SetParticleTextureIndex(113);
			}
			else
			{
				SetParticleTextureIndex(112);
			}

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
				if (MaterialType == Material.Water)
				{
					SetDead();
					WorldObj.SpawnParticle("splash", PosX, PosY, PosZ, 0.0F, 0.0F, 0.0F);
				}
				else
				{
					SetParticleTextureIndex(114);
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