namespace net.minecraft.src
{
	using Microsoft.Xna.Framework;

	public class RenderSpider : RenderLiving
	{
		public RenderSpider() : base(new ModelSpider(), 1.0F)
		{
			SetRenderPassModel(new ModelSpider());
		}

		protected virtual float SetSpiderDeathMaxRotation(EntitySpider par1EntitySpider)
		{
			return 180F;
		}

		/// <summary>
		/// Sets the spider's glowing eyes
		/// </summary>
		protected virtual int SetSpiderEyeBrightness(EntitySpider par1EntitySpider, int par2, float par3)
		{
			if (par2 != 0)
			{
				return -1;
			}
			else
			{
				LoadTexture("/mob/spider_eyes.png");
				float f = 1.0F;
				//GL.Enable(EnableCap.Blend);
				//GL.Disable(EnableCap.AlphaTest);
				//GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.One);
				int i = 61680;
				int j = i % 0x10000;
				int k = i / 0x10000;
				OpenGlHelper.SetLightmapTextureCoords(OpenGlHelper.LightmapTexUnit, (float)j / 1.0F, (float)k / 1.0F);
				//GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
				//GL.Color4(1.0F, 1.0F, 1.0F, f);
				return 1;
			}
		}

		protected virtual void ScaleSpider(EntitySpider par1EntitySpider, float par2)
		{
			float f = par1EntitySpider.SpiderScaleAmount();
			//GL.Scale(f, f, f);
		}

		/// <summary>
		/// Allows the render to do any OpenGL state modifications necessary before the model is rendered. Args:
		/// entityLiving, partialTickTime
		/// </summary>
		protected override void PreRenderCallback(EntityLiving par1EntityLiving, float par2)
		{
			ScaleSpider((EntitySpider)par1EntityLiving, par2);
		}

		protected override float GetDeathMaxRotation(EntityLiving par1EntityLiving)
		{
			return SetSpiderDeathMaxRotation((EntitySpider)par1EntityLiving);
		}

		/// <summary>
		/// Queries whether should render the specified pass or not.
		/// </summary>
		protected override int ShouldRenderPass(EntityLiving par1EntityLiving, int par2, float par3)
		{
			return SetSpiderEyeBrightness((EntitySpider)par1EntityLiving, par2, par3);
		}
	}
}