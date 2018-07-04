using System;

namespace net.minecraft.src
{
	using Microsoft.Xna.Framework;

	public class RenderMagmaCube : RenderLiving
	{
		private int Field_40276_c;

		public RenderMagmaCube() : base(new ModelMagmaCube(), 0.25F)
		{
			Field_40276_c = ((ModelMagmaCube)MainModel).Func_40343_a();
		}

		public virtual void DoRenderMagmaCube(EntityMagmaCube par1EntityMagmaCube, double par2, double par4, double par6, float par8, float par9)
		{
			int i = ((ModelMagmaCube)MainModel).Func_40343_a();

			if (i != Field_40276_c)
			{
				Field_40276_c = i;
				MainModel = new ModelMagmaCube();
				Console.WriteLine("new lava slime model");
			}

			base.DoRenderLiving(par1EntityMagmaCube, par2, par4, par6, par8, par9);
		}

		protected virtual void ScaleMagmaCube(EntityMagmaCube par1EntityMagmaCube, float par2)
		{
			int i = par1EntityMagmaCube.GetSlimeSize();
			float f = (par1EntityMagmaCube.Field_767_b + (par1EntityMagmaCube.Field_768_a - par1EntityMagmaCube.Field_767_b) * par2) / ((float)i * 0.5F + 1.0F);
			float f1 = 1.0F / (f + 1.0F);
			float f2 = i;
			//GL.Scale(f1 * f2, (1.0F / f1) * f2, f1 * f2);
		}

		/// <summary>
		/// Allows the render to do any OpenGL state modifications necessary before the model is rendered. Args:
		/// entityLiving, partialTickTime
		/// </summary>
		protected override void PreRenderCallback(EntityLiving par1EntityLiving, float par2)
		{
			ScaleMagmaCube((EntityMagmaCube)par1EntityLiving, par2);
		}

		public override void DoRenderLiving(EntityLiving par1EntityLiving, double par2, double par4, double par6, float par8, float par9)
		{
			DoRenderMagmaCube((EntityMagmaCube)par1EntityLiving, par2, par4, par6, par8, par9);
		}

		/// <summary>
		/// Actually renders the given argument. This is a synthetic bridge method, always casting down its argument and then
		/// handing it off to a worker function which does the actual work. In all probabilty, the class Render is generic
		/// (Render<T extends Entity) and this method has signature public void doRender(T entity, double d, double d1,
		/// double d2, float f, float f1). But JAD is pre 1.5 so doesn't do that.
		/// </summary>
		public override void DoRender(Entity par1Entity, double par2, double par4, double par6, float par8, float par9)
		{
			DoRenderMagmaCube((EntityMagmaCube)par1Entity, par2, par4, par6, par8, par9);
		}
	}
}