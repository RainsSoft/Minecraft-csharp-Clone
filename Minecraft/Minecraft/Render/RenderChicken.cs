namespace net.minecraft.src
{
	public class RenderChicken : RenderLiving
	{
		public RenderChicken(ModelBase par1ModelBase, float par2) : base(par1ModelBase, par2)
		{
		}

		public virtual void DoRenderChicken(EntityChicken par1EntityChicken, double par2, double par4, double par6, float par8, float par9)
		{
			base.DoRenderLiving(par1EntityChicken, par2, par4, par6, par8, par9);
		}

		protected virtual float GetWingRotation(EntityChicken par1EntityChicken, float par2)
		{
			float f = par1EntityChicken.Field_756_e + (par1EntityChicken.Field_752_b - par1EntityChicken.Field_756_e) * par2;
			float f1 = par1EntityChicken.Field_757_d + (par1EntityChicken.DestPos - par1EntityChicken.Field_757_d) * par2;
			return (MathHelper2.Sin(f) + 1.0F) * f1;
		}

		/// <summary>
		/// Defines what float the third param in setRotationAngles of ModelBase is
		/// </summary>
		protected override float HandleRotationFloat(EntityLiving par1EntityLiving, float par2)
		{
			return GetWingRotation((EntityChicken)par1EntityLiving, par2);
		}

		public override void DoRenderLiving(EntityLiving par1EntityLiving, double par2, double par4, double par6, float par8, float par9)
		{
			DoRenderChicken((EntityChicken)par1EntityLiving, par2, par4, par6, par8, par9);
		}

		/// <summary>
		/// Actually renders the given argument. This is a synthetic bridge method, always casting down its argument and then
		/// handing it off to a worker function which does the actual work. In all probabilty, the class Render is generic
		/// (Render<T extends Entity) and this method has signature public void doRender(T entity, double d, double d1,
		/// double d2, float f, float f1). But JAD is pre 1.5 so doesn't do that.
		/// </summary>
		public override void DoRender(Entity par1Entity, double par2, double par4, double par6, float par8, float par9)
		{
			DoRenderChicken((EntityChicken)par1Entity, par2, par4, par6, par8, par9);
		}
	}
}