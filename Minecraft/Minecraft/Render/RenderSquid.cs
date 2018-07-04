namespace net.minecraft.src
{

	using Microsoft.Xna.Framework;

	public class RenderSquid : RenderLiving
	{
		public RenderSquid(ModelBase par1ModelBase, float par2) : base(par1ModelBase, par2)
		{
		}

		public virtual void Func_21008_a(EntitySquid par1EntitySquid, double par2, double par4, double par6, float par8, float par9)
		{
			base.DoRenderLiving(par1EntitySquid, par2, par4, par6, par8, par9);
		}

		protected virtual void Func_21007_a(EntitySquid par1EntitySquid, float par2, float par3, float par4)
		{
			float f = par1EntitySquid.Field_21088_b + (par1EntitySquid.Field_21089_a - par1EntitySquid.Field_21088_b) * par4;
			float f1 = par1EntitySquid.Field_21086_f + (par1EntitySquid.Field_21087_c - par1EntitySquid.Field_21086_f) * par4;
			//GL.Translate(0.0F, 0.5F, 0.0F);
			//GL.Rotate(180F - par3, 0.0F, 1.0F, 0.0F);
			//GL.Rotate(f, 1.0F, 0.0F, 0.0F);
			//GL.Rotate(f1, 0.0F, 1.0F, 0.0F);
			//GL.Translate(0.0F, -1.2F, 0.0F);
		}

		protected virtual void Func_21005_a(EntitySquid entitysquid, float f)
		{
		}

		protected virtual float HandleRotationFloat(EntitySquid par1EntitySquid, float par2)
		{
			float f = par1EntitySquid.LastTentacleAngle + (par1EntitySquid.TentacleAngle - par1EntitySquid.LastTentacleAngle) * par2;
			return f;
		}

		/// <summary>
		/// Allows the render to do any OpenGL state modifications necessary before the model is rendered. Args:
		/// entityLiving, partialTickTime
		/// </summary>
		protected override void PreRenderCallback(EntityLiving par1EntityLiving, float par2)
		{
			Func_21005_a((EntitySquid)par1EntityLiving, par2);
		}

		/// <summary>
		/// Defines what float the third param in setRotationAngles of ModelBase is
		/// </summary>
		protected override float HandleRotationFloat(EntityLiving par1EntityLiving, float par2)
		{
			return HandleRotationFloat((EntitySquid)par1EntityLiving, par2);
		}

		protected override void RotateCorpse(EntityLiving par1EntityLiving, float par2, float par3, float par4)
		{
			Func_21007_a((EntitySquid)par1EntityLiving, par2, par3, par4);
		}

		public override void DoRenderLiving(EntityLiving par1EntityLiving, double par2, double par4, double par6, float par8, float par9)
		{
			Func_21008_a((EntitySquid)par1EntityLiving, par2, par4, par6, par8, par9);
		}

		/// <summary>
		/// Actually renders the given argument. This is a synthetic bridge method, always casting down its argument and then
		/// handing it off to a worker function which does the actual work. In all probabilty, the class Render is generic
		/// (Render<T extends Entity) and this method has signature public void doRender(T entity, double d, double d1,
		/// double d2, float f, float f1). But JAD is pre 1.5 so doesn't do that.
		/// </summary>
		public override void DoRender(Entity par1Entity, double par2, double par4, double par6, float par8, float par9)
		{
			Func_21008_a((EntitySquid)par1Entity, par2, par4, par6, par8, par9);
		}
	}

}