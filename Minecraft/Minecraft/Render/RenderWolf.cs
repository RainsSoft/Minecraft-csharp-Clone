namespace net.minecraft.src
{
	public class RenderWolf : RenderLiving
	{
		public RenderWolf(ModelBase par1ModelBase, float par2) : base(par1ModelBase, par2)
		{
		}

		public virtual void DoRenderWolf(EntityWolf par1EntityWolf, double par2, double par4, double par6, float par8, float par9)
		{
			base.DoRenderLiving(par1EntityWolf, par2, par4, par6, par8, par9);
		}

		protected virtual float GetTailRotation(EntityWolf par1EntityWolf, float par2)
		{
			return par1EntityWolf.GetTailRotation();
		}

		protected virtual void Func_25006_b(EntityWolf entitywolf, float f)
		{
		}

		/// <summary>
		/// Allows the render to do any OpenGL state modifications necessary before the model is rendered. Args:
		/// entityLiving, partialTickTime
		/// </summary>
		protected override void PreRenderCallback(EntityLiving par1EntityLiving, float par2)
		{
			Func_25006_b((EntityWolf)par1EntityLiving, par2);
		}

		/// <summary>
		/// Defines what float the third param in setRotationAngles of ModelBase is
		/// </summary>
		protected override float HandleRotationFloat(EntityLiving par1EntityLiving, float par2)
		{
			return GetTailRotation((EntityWolf)par1EntityLiving, par2);
		}

		public override void DoRenderLiving(EntityLiving par1EntityLiving, double par2, double par4, double par6, float par8, float par9)
		{
			DoRenderWolf((EntityWolf)par1EntityLiving, par2, par4, par6, par8, par9);
		}

		/// <summary>
		/// Actually renders the given argument. This is a synthetic bridge method, always casting down its argument and then
		/// handing it off to a worker function which does the actual work. In all probabilty, the class Render is generic
		/// (Render<T extends Entity) and this method has signature public void doRender(T entity, double d, double d1,
		/// double d2, float f, float f1). But JAD is pre 1.5 so doesn't do that.
		/// </summary>
		public override void DoRender(Entity par1Entity, double par2, double par4, double par6, float par8, float par9)
		{
			DoRenderWolf((EntityWolf)par1Entity, par2, par4, par6, par8, par9);
		}
	}
}