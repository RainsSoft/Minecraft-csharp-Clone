using System;

namespace net.minecraft.src
{
    public class EntityLavaFX : EntityFX
    {
        private float lavaParticleScale;

        public EntityLavaFX(World par1World, float par2, float par4, float par6)
            : base(par1World, par2, par4, par6, 0.0F, 0.0F, 0.0F)
        {
            MotionX *= 0.80000001192092896F;
            MotionY *= 0.80000001192092896F;
            MotionZ *= 0.80000001192092896F;
            MotionY = (float)Rand.NextDouble() * 0.4F + 0.05F;
            ParticleRed = ParticleGreen = ParticleBlue = 1.0F;
            ParticleScale *= (float)Rand.NextDouble() * 2.0F + 0.2F;
            lavaParticleScale = ParticleScale;
            ParticleMaxAge = (int)(16D / ((new Random(1)).NextDouble() * 0.80000000000000004D + 0.20000000000000001D));
            NoClip = false;
            SetParticleTextureIndex(49);
        }

        public override int GetBrightnessForRender(float par1)
        {
            float f = ((float)ParticleAge + par1) / (float)ParticleMaxAge;

            if (f < 0.0F)
            {
                f = 0.0F;
            }

            if (f > 1.0F)
            {
                f = 1.0F;
            }

            int i = base.GetBrightnessForRender(par1);
            int c = 360;
            int j = i >> 16 & 0xff;
            return c | j << 16;
        }

        ///<summary>
        /// Gets how bright this entity is.
        ///</summary>
        public float GetBrightness(float par1)
        {
            return 1.0F;
        }

        public void RenderParticle(Tessellator par1Tessellator, float par2, float par3, float par4, float par5, float par6, float par7)
        {
            float f = ((float)ParticleAge + par2) / (float)ParticleMaxAge;
            ParticleScale = lavaParticleScale * (1.0F - f * f);
            base.RenderParticle(par1Tessellator, par2, par3, par4, par5, par6, par7);
        }

        ///<summary>
        /// Called to update the entity's position/logic.
        ///</summary>
        public void OnUpdate()
        {
            PrevPosX = PosX;
            PrevPosY = PosY;
            PrevPosZ = PosZ;

            if (ParticleAge++ >= ParticleMaxAge)
            {
                SetDead();
            }

            float f = (float)ParticleAge / (float)ParticleMaxAge;

            if ((float)Rand.NextDouble() > f)
            {
                WorldObj.SpawnParticle("smoke", PosX, PosY, PosZ, MotionX, MotionY, MotionZ);
            }

            MotionY -= 0.029999999999999999F;
            MoveEntity(MotionX, MotionY, MotionZ);
            MotionX *= 0.99900001287460327F;
            MotionY *= 0.99900001287460327F;
            MotionZ *= 0.99900001287460327F;

            if (OnGround)
            {
                MotionX *= 0.69999998807907104F;
                MotionZ *= 0.69999998807907104F;
            }
        }
    }
}