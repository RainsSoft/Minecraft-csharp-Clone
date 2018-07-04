using System;

namespace net.minecraft.src
{
	using Microsoft.Xna.Framework;

	public class RenderEnderman : RenderLiving
	{
		/// <summary>
		/// The model of the enderman </summary>
		private ModelEnderman EndermanModel;
		private Random Rnd;

		public RenderEnderman() : base(new ModelEnderman(), 0.5F)
		{
			Rnd = new Random();
			EndermanModel = (ModelEnderman)base.MainModel;
			SetRenderPassModel(EndermanModel);
		}

		/// <summary>
		/// Renders the enderman
		/// </summary>
		public virtual void DoRenderEnderman(EntityEnderman par1EntityEnderman, double par2, double par4, double par6, float par8, float par9)
		{
			EndermanModel.IsCarrying = par1EntityEnderman.GetCarried() > 0;
			EndermanModel.IsAttacking = par1EntityEnderman.IsAttacking;

			if (par1EntityEnderman.IsAttacking)
			{
				double d = 0.02D;
				par2 += Rnd.NextGaussian() * d;
				par6 += Rnd.NextGaussian() * d;
			}

			base.DoRenderLiving(par1EntityEnderman, par2, par4, par6, par8, par9);
		}

		/// <summary>
		/// Render the block an enderman is carrying
		/// </summary>
		protected virtual void RenderCarrying(EntityEnderman par1EntityEnderman, float par2)
		{
			base.RenderEquippedItems(par1EntityEnderman, par2);

			if (par1EntityEnderman.GetCarried() > 0)
			{
				//GL.Enable(EnableCap.RescaleNormal);
				//GL.PushMatrix();
				float f = 0.5F;
				//GL.Translate(0.0F, 0.6875F, -0.75F);
				f *= 1.0F;
				//GL.Rotate(20F, 1.0F, 0.0F, 0.0F);
				//GL.Rotate(45F, 0.0F, 1.0F, 0.0F);
				//GL.Scale(f, -f, f);
				int i = par1EntityEnderman.GetBrightnessForRender(par2);
				int j = i % 0x10000;
				int k = i / 0x10000;
				OpenGlHelper.SetLightmapTextureCoords(OpenGlHelper.LightmapTexUnit, (float)j / 1.0F, (float)k / 1.0F);
				//GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
				//GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
				LoadTexture("/terrain.png");
				RenderBlocks.RenderBlockAsItem(Block.BlocksList[par1EntityEnderman.GetCarried()], par1EntityEnderman.GetCarryingData(), 1.0F);
				//GL.PopMatrix();
				//GL.Disable(EnableCap.RescaleNormal);
			}
		}

		/// <summary>
		/// Render the endermans eyes
		/// </summary>
		protected virtual int RenderEyes(EntityEnderman par1EntityEnderman, int par2, float par3)
		{
			if (par2 != 0)
			{
				return -1;
			}
			else
			{
				LoadTexture("/mob/enderman_eyes.png");
				float f = 1.0F;
				//GL.Enable(EnableCap.Blend);
                //GL.Disable(EnableCap.AlphaTest);
				//GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.One);
                //GL.Disable(EnableCap.Lighting);
				int i = 61680;
				int j = i % 0x10000;
				int k = i / 0x10000;
				OpenGlHelper.SetLightmapTextureCoords(OpenGlHelper.LightmapTexUnit, (float)j / 1.0F, (float)k / 1.0F);
				//GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
                //GL.Enable(EnableCap.Lighting);
				//GL.Color4(1.0F, 1.0F, 1.0F, f);
				return 1;
			}
		}

		/// <summary>
		/// Queries whether should render the specified pass or not.
		/// </summary>
		protected override int ShouldRenderPass(EntityLiving par1EntityLiving, int par2, float par3)
		{
			return RenderEyes((EntityEnderman)par1EntityLiving, par2, par3);
		}

		protected override void RenderEquippedItems(EntityLiving par1EntityLiving, float par2)
		{
			RenderCarrying((EntityEnderman)par1EntityLiving, par2);
		}

		public override void DoRenderLiving(EntityLiving par1EntityLiving, double par2, double par4, double par6, float par8, float par9)
		{
			DoRenderEnderman((EntityEnderman)par1EntityLiving, par2, par4, par6, par8, par9);
		}

		/// <summary>
		/// Actually renders the given argument. This is a synthetic bridge method, always casting down its argument and then
		/// handing it off to a worker function which does the actual work. In all probabilty, the class Render is generic
		/// (Render<T extends Entity) and this method has signature public void doRender(T entity, double d, double d1,
		/// double d2, float f, float f1). But JAD is pre 1.5 so doesn't do that.
		/// </summary>
		public override void DoRender(Entity par1Entity, double par2, double par4, double par6, float par8, float par9)
		{
			DoRenderEnderman((EntityEnderman)par1Entity, par2, par4, par6, par8, par9);
		}
	}
}