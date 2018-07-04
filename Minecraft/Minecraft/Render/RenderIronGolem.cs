using System;

namespace net.minecraft.src
{
	using Microsoft.Xna.Framework;

	public class RenderIronGolem : RenderLiving
	{
		private ModelIronGolem Field_48422_c;

		public RenderIronGolem() : base(new ModelIronGolem(), 0.5F)
		{
			Field_48422_c = (ModelIronGolem)MainModel;
		}

		public virtual void Func_48421_a(EntityIronGolem par1EntityIronGolem, double par2, double par4, double par6, float par8, float par9)
		{
			base.DoRenderLiving(par1EntityIronGolem, par2, par4, par6, par8, par9);
		}

		protected virtual void Func_48420_a(EntityIronGolem par1EntityIronGolem, float par2, float par3, float par4)
		{
			base.RotateCorpse(par1EntityIronGolem, par2, par3, par4);

			if ((double)par1EntityIronGolem.Field_704_R < 0.01D)
			{
				return;
			}
			else
			{
				float f = 13F;
				float f1 = (par1EntityIronGolem.Field_703_S - par1EntityIronGolem.Field_704_R * (1.0F - par4)) + 6F;
				float f2 = (Math.Abs(f1 % f - f * 0.5F) - f * 0.25F) / (f * 0.25F);
				//GL.Rotate(6.5F * f2, 0.0F, 0.0F, 1.0F);
				return;
			}
		}

		protected virtual void Func_48419_a(EntityIronGolem par1EntityIronGolem, float par2)
		{
			base.RenderEquippedItems(par1EntityIronGolem, par2);

			if (par1EntityIronGolem.Func_48117_D_() == 0)
			{
				return;
			}
			else
			{
				//GL.Enable(EnableCap.RescaleNormal);
				//GL.PushMatrix();
				//GL.Rotate(5F + (180F * Field_48422_c.Field_48233_c.RotateAngleX) / (float)Math.PI, 1.0F, 0.0F, 0.0F);
				//GL.Translate(-0.6875F, 1.25F, -0.9375F);
				//GL.Rotate(90F, 1.0F, 0.0F, 0.0F);
				float f = 0.8F;
				//GL.Scale(f, -f, f);
				int i = par1EntityIronGolem.GetBrightnessForRender(par2);
				int j = i % 0x10000;
				int k = i / 0x10000;
				OpenGlHelper.SetLightmapTextureCoords(OpenGlHelper.LightmapTexUnit, (float)j / 1.0F, (float)k / 1.0F);
				//GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
				//GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
				LoadTexture("/terrain.png");
				RenderBlocks.RenderBlockAsItem(Block.PlantRed, 0, 1.0F);
				//GL.PopMatrix();
				//GL.Disable(EnableCap.RescaleNormal);
				return;
			}
		}

		protected override void RenderEquippedItems(EntityLiving par1EntityLiving, float par2)
		{
			Func_48419_a((EntityIronGolem)par1EntityLiving, par2);
		}

		protected override void RotateCorpse(EntityLiving par1EntityLiving, float par2, float par3, float par4)
		{
			Func_48420_a((EntityIronGolem)par1EntityLiving, par2, par3, par4);
		}

		public override void DoRenderLiving(EntityLiving par1EntityLiving, double par2, double par4, double par6, float par8, float par9)
		{
			Func_48421_a((EntityIronGolem)par1EntityLiving, par2, par4, par6, par8, par9);
		}

		/// <summary>
		/// Actually renders the given argument. This is a synthetic bridge method, always casting down its argument and then
		/// handing it off to a worker function which does the actual work. In all probabilty, the class Render is generic
		/// (Render<T extends Entity) and this method has signature public void doRender(T entity, double d, double d1,
		/// double d2, float f, float f1). But JAD is pre 1.5 so doesn't do that.
		/// </summary>
		public override void DoRender(Entity par1Entity, double par2, double par4, double par6, float par8, float par9)
		{
			Func_48421_a((EntityIronGolem)par1Entity, par2, par4, par6, par8, par9);
		}
	}
}