namespace net.minecraft.src
{
	public class RenderSilverfish : RenderLiving
	{
		public RenderSilverfish() : base(new ModelSilverfish(), 0.3F)
		{
		}

		/// <summary>
		/// Return the silverfish's maximum death rotation.
		/// </summary>
		protected virtual float GetSilverfishDeathRotation(EntitySilverfish par1EntitySilverfish)
		{
			return 180F;
		}

		/// <summary>
		/// Renders the silverfish.
		/// </summary>
		public virtual void DoRenderSilverfish(EntitySilverfish par1EntitySilverfish, double par2, double par4, double par6, float par8, float par9)
		{
			base.DoRenderLiving(par1EntitySilverfish, par2, par4, par6, par8, par9);
		}

		/// <summary>
		/// Disallows the silverfish to render the renderPassModel.
		/// </summary>
		protected virtual int ShouldSilverfishRenderPass(EntitySilverfish par1EntitySilverfish, int par2, float par3)
		{
			return -1;
		}

		protected override float GetDeathMaxRotation(EntityLiving par1EntityLiving)
		{
			return GetSilverfishDeathRotation((EntitySilverfish)par1EntityLiving);
		}

		/// <summary>
		/// Queries whether should render the specified pass or not.
		/// </summary>
		protected override int ShouldRenderPass(EntityLiving par1EntityLiving, int par2, float par3)
		{
			return ShouldSilverfishRenderPass((EntitySilverfish)par1EntityLiving, par2, par3);
		}

		public override void DoRenderLiving(EntityLiving par1EntityLiving, double par2, double par4, double par6, float par8, float par9)
		{
			DoRenderSilverfish((EntitySilverfish)par1EntityLiving, par2, par4, par6, par8, par9);
		}

		/// <summary>
		/// Actually renders the given argument. This is a synthetic bridge method, always casting down its argument and then
		/// handing it off to a worker function which does the actual work. In all probabilty, the class Render is generic
		/// (Render<T extends Entity) and this method has signature public void doRender(T entity, double d, double d1,
		/// double d2, float f, float f1). But JAD is pre 1.5 so doesn't do that.
		/// </summary>
		public override void DoRender(Entity par1Entity, double par2, double par4, double par6, float par8, float par9)
		{
			DoRenderSilverfish((EntitySilverfish)par1Entity, par2, par4, par6, par8, par9);
		}
	}
}