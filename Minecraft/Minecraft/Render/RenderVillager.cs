namespace net.minecraft.src
{
	using Microsoft.Xna.Framework;

	public class RenderVillager : RenderLiving
	{
		protected ModelVillager Field_40295_c;

		public RenderVillager() : base(new ModelVillager(0.0F), 0.5F)
		{
			Field_40295_c = (ModelVillager)MainModel;
		}

		protected virtual int Func_40293_a(EntityVillager par1EntityVillager, int par2, float par3)
		{
			return -1;
		}

		public virtual void DoRenderVillager(EntityVillager par1EntityVillager, double par2, double par4, double par6, float par8, float par9)
		{
			base.DoRenderLiving(par1EntityVillager, par2, par4, par6, par8, par9);
		}

		protected virtual void Func_40290_a(EntityVillager entityvillager, double d, double d1, double d2)
		{
		}

		protected virtual void Func_40291_a(EntityVillager par1EntityVillager, float par2)
		{
			base.RenderEquippedItems(par1EntityVillager, par2);
		}

		protected virtual void Func_40292_b(EntityVillager par1EntityVillager, float par2)
		{
			float f = 0.9375F;

			if (par1EntityVillager.GetGrowingAge() < 0)
			{
				f = (float)((double)f * 0.5D);
				ShadowSize = 0.25F;
			}
			else
			{
				ShadowSize = 0.5F;
			}

			//GL.Scale(f, f, f);
		}

		/// <summary>
		/// Passes the specialRender and renders it
		/// </summary>
		protected override void PassSpecialRender(EntityLiving par1EntityLiving, double par2, double par4, double par6)
		{
			Func_40290_a((EntityVillager)par1EntityLiving, par2, par4, par6);
		}

		/// <summary>
		/// Allows the render to do any OpenGL state modifications necessary before the model is rendered. Args:
		/// entityLiving, partialTickTime
		/// </summary>
		protected override void PreRenderCallback(EntityLiving par1EntityLiving, float par2)
		{
			Func_40292_b((EntityVillager)par1EntityLiving, par2);
		}

		/// <summary>
		/// Queries whether should render the specified pass or not.
		/// </summary>
		protected override int ShouldRenderPass(EntityLiving par1EntityLiving, int par2, float par3)
		{
			return Func_40293_a((EntityVillager)par1EntityLiving, par2, par3);
		}

		protected override void RenderEquippedItems(EntityLiving par1EntityLiving, float par2)
		{
			Func_40291_a((EntityVillager)par1EntityLiving, par2);
		}

		public override void DoRenderLiving(EntityLiving par1EntityLiving, double par2, double par4, double par6, float par8, float par9)
		{
			DoRenderVillager((EntityVillager)par1EntityLiving, par2, par4, par6, par8, par9);
		}

		/// <summary>
		/// Actually renders the given argument. This is a synthetic bridge method, always casting down its argument and then
		/// handing it off to a worker function which does the actual work. In all probabilty, the class Render is generic
		/// (Render<T extends Entity) and this method has signature public void doRender(T entity, double d, double d1,
		/// double d2, float f, float f1). But JAD is pre 1.5 so doesn't do that.
		/// </summary>
		public override void DoRender(Entity par1Entity, double par2, double par4, double par6, float par8, float par9)
		{
			DoRenderVillager((EntityVillager)par1Entity, par2, par4, par6, par8, par9);
		}
	}
}