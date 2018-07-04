namespace net.minecraft.src
{
	public class EntityDiggingFX : EntityFX
	{
		private Block BlockInstance;

        public EntityDiggingFX(World par1World, float par2, float par4, float par6, float par8, float par10, float par12, Block par14Block, int par15, int par16)
            : base(par1World, par2, par4, par6, par8, par10, par12)
		{
			BlockInstance = par14Block;
			SetParticleTextureIndex(par14Block.GetBlockTextureFromSideAndMetadata(0, par16));
			ParticleGravity = par14Block.BlockParticleGravity;
			ParticleRed = ParticleGreen = ParticleBlue = 0.6F;
			ParticleScale /= 2.0F;
		}

		public virtual EntityDiggingFX Func_4041_a(int par1, int par2, int par3)
		{
			if (BlockInstance == Block.Grass)
			{
				return this;
			}
			else
			{
				int i = BlockInstance.ColorMultiplier(WorldObj, par1, par2, par3);
				ParticleRed *= (float)(i >> 16 & 0xff) / 255F;
				ParticleGreen *= (float)(i >> 8 & 0xff) / 255F;
				ParticleBlue *= (float)(i & 0xff) / 255F;
				return this;
			}
		}

		public override int GetFXLayer()
		{
			return 1;
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