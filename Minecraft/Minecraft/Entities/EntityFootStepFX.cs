using Microsoft.Xna.Framework;

namespace net.minecraft.src
{
	public class EntityFootStepFX : EntityFX
	{
		private int Field_27018_a;
		private int Field_27020_o;
		private RenderEngine CurrentFootSteps;

        public EntityFootStepFX(RenderEngine par1RenderEngine, World par2World, float par3, float par5, float par7)
            : base(par2World, par3, par5, par7, 0.0F, 0.0F, 0.0F)
		{
			Field_27018_a = 0;
			Field_27020_o = 0;
			CurrentFootSteps = par1RenderEngine;
			MotionX = MotionY = MotionZ = 0.0F;
			Field_27020_o = 200;
		}

		public override void RenderParticle(Tessellator par1Tessellator, float par2, float par3, float par4, float par5, float par6, float par7)
		{
			float f = ((float)Field_27018_a + par2) / (float)Field_27020_o;
			f *= f;
			float f1 = 2.0F - f * 2.0F;

			if (f1 > 1.0F)
			{
				f1 = 1.0F;
			}

			f1 *= 0.2F;
			//GL.Disable(EnableCap.Lighting);
			float f2 = 0.125F;
			float f3 = (float)(PosX - InterpPosX);
			float f4 = (float)(PosY - InterpPosY);
			float f5 = (float)(PosZ - InterpPosZ);
			float f6 = WorldObj.GetLightBrightness(MathHelper2.Floor_double(PosX), MathHelper2.Floor_double(PosY), MathHelper2.Floor_double(PosZ));
			//CurrentFootSteps.BindTexture(CurrentFootSteps.GetTexture("/misc/footprint.png"));
            CurrentFootSteps.BindTexture("misc.footprint.png");
			//GL.Enable(EnableCap.Blend);
			//GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			par1Tessellator.StartDrawingQuads();
			par1Tessellator.SetColorRGBA_F(f6, f6, f6, f1);
			par1Tessellator.AddVertexWithUV(f3 - f2, f4, f5 + f2, 0.0F, 1.0D);
			par1Tessellator.AddVertexWithUV(f3 + f2, f4, f5 + f2, 1.0D, 1.0D);
			par1Tessellator.AddVertexWithUV(f3 + f2, f4, f5 - f2, 1.0D, 0.0F);
			par1Tessellator.AddVertexWithUV(f3 - f2, f4, f5 - f2, 0.0F, 0.0F);
			par1Tessellator.Draw();
			//GL.Disable(EnableCap.Blend);
			//GL.Enable(EnableCap.Lighting);
		}

		/// <summary>
		/// Called to update the entity's position/logic.
		/// </summary>
		public override void OnUpdate()
		{
			Field_27018_a++;

			if (Field_27018_a == Field_27020_o)
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