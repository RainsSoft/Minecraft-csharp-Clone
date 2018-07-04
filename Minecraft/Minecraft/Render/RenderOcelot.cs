namespace net.minecraft.src
{

	using Microsoft.Xna.Framework;

	public class RenderOcelot : RenderLiving
	{
		public RenderOcelot(ModelBase par1ModelBase, float par2) : base(par1ModelBase, par2)
		{
		}

		public virtual void Func_48424_a(EntityOcelot par1EntityOcelot, double par2, double par4, double par6, float par8, float par9)
		{
			base.DoRenderLiving(par1EntityOcelot, par2, par4, par6, par8, par9);
		}

		protected virtual void Func_48423_a(EntityOcelot par1EntityOcelot, float par2)
		{
			base.PreRenderCallback(par1EntityOcelot, par2);

			if (par1EntityOcelot.IsTamed())
			{
				//GL.Scale(0.8F, 0.8F, 0.8F);
			}
		}

		/// <summary>
		/// Allows the render to do any OpenGL state modifications necessary before the model is rendered. Args:
		/// entityLiving, partialTickTime
		/// </summary>
		protected override void PreRenderCallback(EntityLiving par1EntityLiving, float par2)
		{
			Func_48423_a((EntityOcelot)par1EntityLiving, par2);
		}

		public override void DoRenderLiving(EntityLiving par1EntityLiving, double par2, double par4, double par6, float par8, float par9)
		{
			Func_48424_a((EntityOcelot)par1EntityLiving, par2, par4, par6, par8, par9);
		}

		/// <summary>
		/// Actually renders the given argument. This is a synthetic bridge method, always casting down its argument and then
		/// handing it off to a worker function which does the actual work. In all probabilty, the class Render is generic
		/// (Render<T extends Entity) and this method has signature public void doRender(T entity, double d, double d1,
		/// double d2, float f, float f1). But JAD is pre 1.5 so doesn't do that.
		/// </summary>
		public override void DoRender(Entity par1Entity, double par2, double par4, double par6, float par8, float par9)
		{
			Func_48424_a((EntityOcelot)par1Entity, par2, par4, par6, par8, par9);
		}
	}

}