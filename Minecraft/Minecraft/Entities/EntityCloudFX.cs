using System;

namespace net.minecraft.src
{
	public class EntityCloudFX : EntityFX
	{
		float Field_35135_a;

        public EntityCloudFX(World par1World, float par2, float par4, float par6, float par8, float par10, float par12)
            : base(par1World, par2, par4, par6, 0.0F, 0.0F, 0.0F)
		{
			float f = 2.5F;
			MotionX *= 0.10000000149011612F;
			MotionY *= 0.10000000149011612F;
			MotionZ *= 0.10000000149011612F;
			MotionX += par8;
			MotionY += par10;
			MotionZ += par12;
			ParticleRed = ParticleGreen = ParticleBlue = 1.0F - (float)((new Random(1)).NextDouble() * 0.30000001192092896D);
			ParticleScale *= 0.75F;
			ParticleScale *= f;
			Field_35135_a = ParticleScale;
			ParticleMaxAge = (int)(8D / ((new Random(2)).NextDouble() * 0.80000000000000004D + 0.29999999999999999D));
			ParticleMaxAge *= (int)f;
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

			ParticleScale = Field_35135_a * f;
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

			SetParticleTextureIndex(7 - (ParticleAge * 8) / ParticleMaxAge);
			MoveEntity(MotionX, MotionY, MotionZ);
			MotionX *= 0.95999997854232788F;
			MotionY *= 0.95999997854232788F;
			MotionZ *= 0.95999997854232788F;
			EntityPlayer entityplayer = WorldObj.GetClosestPlayerToEntity(this, 2);

			if (entityplayer != null && PosY > entityplayer.BoundingBox.MinY)
			{
				PosY += (entityplayer.BoundingBox.MinY - PosY) * 0.20000000000000001F;
				MotionY += (entityplayer.MotionY - MotionY) * 0.20000000000000001F;
				SetPosition(PosX, PosY, PosZ);
			}

			if (OnGround)
			{
				MotionX *= 0.69999998807907104F;
				MotionZ *= 0.69999998807907104F;
			}
		}
	}
}