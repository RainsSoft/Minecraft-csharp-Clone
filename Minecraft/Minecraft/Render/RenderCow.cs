namespace net.minecraft.src
{
	public class RenderCow : RenderLiving
	{
		public RenderCow(ModelBase par1ModelBase, float par2) : base(par1ModelBase, par2)
		{
		}

		public virtual void DoRenderCow(EntityCow par1EntityCow, double par2, double par4, double par6, float par8, float par9)
		{
			base.DoRenderLiving(par1EntityCow, par2, par4, par6, par8, par9);
		}

		public override void DoRenderLiving(EntityLiving par1EntityLiving, double par2, double par4, double par6, float par8, float par9)
		{
			DoRenderCow((EntityCow)par1EntityLiving, par2, par4, par6, par8, par9);
		}

		/// <summary>
		/// Actually renders the given argument. This is a synthetic bridge method, always casting down its argument and then
		/// handing it off to a worker function which does the actual work. In all probabilty, the class Render is generic
		/// (Render<T extends Entity) and this method has signature public void doRender(T entity, double d, double d1,
		/// double d2, float f, float f1). But JAD is pre 1.5 so doesn't do that.
		/// </summary>
		public override void DoRender(Entity par1Entity, double par2, double par4, double par6, float par8, float par9)
		{
			DoRenderCow((EntityCow)par1Entity, par2, par4, par6, par8, par9);
		}
	}
}