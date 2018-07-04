namespace net.minecraft.src
{
	public class EntityBreakingFX : EntityFX
	{
        public EntityBreakingFX(World par1World, float par2, float par4, float par6, Item par8Item)
            : base(par1World, par2, par4, par6, 0.0F, 0.0F, 0.0F)
		{
			SetParticleTextureIndex(par8Item.GetIconFromDamage(0));
			ParticleRed = ParticleGreen = ParticleBlue = 1.0F;
			ParticleGravity = Block.BlockSnow.BlockParticleGravity;
			ParticleScale /= 2.0F;
		}

        public EntityBreakingFX(World par1World, float par2, float par4, float par6, float par8, float par10, float par12, Item par14Item)
            : this(par1World, par2, par4, par6, par14Item)
		{
			MotionX *= 0.10000000149011612F;
			MotionY *= 0.10000000149011612F;
			MotionZ *= 0.10000000149011612F;
			MotionX += par8;
			MotionY += par10;
			MotionZ += par12;
		}

		public override int GetFXLayer()
		{
			return 2;
		}

		public override void RenderParticle(Tessellator par1Tessellator, float par2, float par3, float par4, float par5, float par6, float par7)
		{
			float f = ((float)(GetParticleTextureIndex() % 16) + ParticleTextureJitterX / 4F) / 16F;
			float f1 = f + 0.01560938F;
			float f2 = ((float)(GetParticleTextureIndex() / 16) + ParticleTextureJitterY / 4F) / 16F;
			float f3 = f2 + 0.01560938F;
			float f4 = 0.1F * ParticleScale;
			float f5 = (float)((PrevPosX + (PosX - PrevPosX) * (double)par2) - InterpPosX);
			float f6 = (float)((PrevPosY + (PosY - PrevPosY) * (double)par2) - InterpPosY);
			float f7 = (float)((PrevPosZ + (PosZ - PrevPosZ) * (double)par2) - InterpPosZ);
			float f8 = 1.0F;
			par1Tessellator.SetColorOpaque_F(f8 * ParticleRed, f8 * ParticleGreen, f8 * ParticleBlue);
			par1Tessellator.AddVertexWithUV(f5 - par3 * f4 - par6 * f4, f6 - par4 * f4, f7 - par5 * f4 - par7 * f4, f, f3);
			par1Tessellator.AddVertexWithUV((f5 - par3 * f4) + par6 * f4, f6 + par4 * f4, (f7 - par5 * f4) + par7 * f4, f, f2);
			par1Tessellator.AddVertexWithUV(f5 + par3 * f4 + par6 * f4, f6 + par4 * f4, f7 + par5 * f4 + par7 * f4, f1, f2);
			par1Tessellator.AddVertexWithUV((f5 + par3 * f4) - par6 * f4, f6 - par4 * f4, (f7 + par5 * f4) - par7 * f4, f1, f3);
		}
	}
}