using Microsoft.Xna.Framework;

namespace net.minecraft.src
{
	public class EntityLargeExplodeFX : EntityFX
	{
		private int Field_35130_a;
		private int Field_35129_ay;
		private RenderEngine Field_35128_az;
		private float Field_35131_aA;

        public EntityLargeExplodeFX(RenderEngine par1RenderEngine, World par2World, float par3, float par5, float par7, float par9, float par11, float par13)
            : base(par2World, par3, par5, par7, 0.0F, 0.0F, 0.0F)
		{
			Field_35130_a = 0;
			Field_35129_ay = 0;
			Field_35128_az = par1RenderEngine;
			Field_35129_ay = 6 + Rand.Next(4);
			ParticleRed = ParticleGreen = ParticleBlue = Rand.NextFloat() * 0.6F + 0.4F;
			Field_35131_aA = 1.0F - (float)par9 * 0.5F;
		}

		public override void RenderParticle(Tessellator par1Tessellator, float par2, float par3, float par4, float par5, float par6, float par7)
		{
			int i = (int)((((float)Field_35130_a + par2) * 15F) / (float)Field_35129_ay);

			if (i > 15)
			{
				return;
			}
			else
			{
				Field_35128_az.BindTexture("misc.explosion.png");
				float f = (float)(i % 4) / 4F;
				float f1 = f + 0.24975F;
				float f2 = (float)(i / 4) / 4F;
				float f3 = f2 + 0.24975F;
				float f4 = 2.0F * Field_35131_aA;
				float f5 = (float)((PrevPosX + (PosX - PrevPosX) * (double)par2) - InterpPosX);
				float f6 = (float)((PrevPosY + (PosY - PrevPosY) * (double)par2) - InterpPosY);
				float f7 = (float)((PrevPosZ + (PosZ - PrevPosZ) * (double)par2) - InterpPosZ);
				//GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
				//GL.Disable(EnableCap.Lighting);
				RenderHelper.DisableStandardItemLighting();
				par1Tessellator.StartDrawingQuads();
				par1Tessellator.SetColorRGBA_F(ParticleRed, ParticleGreen, ParticleBlue, 1.0F);
				par1Tessellator.SetNormal(0.0F, 1.0F, 0.0F);
				par1Tessellator.SetBrightness(240);
				par1Tessellator.AddVertexWithUV(f5 - par3 * f4 - par6 * f4, f6 - par4 * f4, f7 - par5 * f4 - par7 * f4, f1, f3);
				par1Tessellator.AddVertexWithUV((f5 - par3 * f4) + par6 * f4, f6 + par4 * f4, (f7 - par5 * f4) + par7 * f4, f1, f2);
				par1Tessellator.AddVertexWithUV(f5 + par3 * f4 + par6 * f4, f6 + par4 * f4, f7 + par5 * f4 + par7 * f4, f, f2);
				par1Tessellator.AddVertexWithUV((f5 + par3 * f4) - par6 * f4, f6 - par4 * f4, (f7 + par5 * f4) - par7 * f4, f, f3);
				par1Tessellator.Draw();
				//GL.PolygonOffset(0.0F, 0.0F);
				//GL.Enable(EnableCap.Lighting);
				return;
			}
		}

		public override int GetBrightnessForRender(float par1)
		{
			return 61680;
		}

		/// <summary>
		/// Called to update the entity's position/logic.
		/// </summary>
		public override void OnUpdate()
		{
			PrevPosX = PosX;
			PrevPosY = PosY;
			PrevPosZ = PosZ;
			Field_35130_a++;

			if (Field_35130_a == Field_35129_ay)
			{
				SetDead();
			}
		}

		public override int GetFXLayer()
		{
			return 3;
		}
	}
}