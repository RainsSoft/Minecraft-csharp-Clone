namespace net.minecraft.src
{
	using Microsoft.Xna.Framework;

	public class RenderSlime : RenderLiving
	{
		private ModelBase ScaleAmount;

		public RenderSlime(ModelBase par1ModelBase, ModelBase par2ModelBase, float par3) : base(par1ModelBase, par3)
		{
			ScaleAmount = par2ModelBase;
		}

		protected virtual int Func_40287_a(EntitySlime par1EntitySlime, int par2, float par3)
		{
			if (par2 == 0)
			{
				SetRenderPassModel(ScaleAmount);
				//GL.Enable(EnableCap.Normalize);
				//GL.Enable(EnableCap.Blend);
				//GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
				return 1;
			}

			if (par2 == 1)
			{
				//GL.Disable(EnableCap.Blend);
				//GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
			}

			return -1;
		}

		/// <summary>
		/// sets the scale for the slime based on getSlimeSize in EntitySlime
		/// </summary>
		protected virtual void ScaleSlime(EntitySlime par1EntitySlime, float par2)
		{
			int i = par1EntitySlime.GetSlimeSize();
			float f = (par1EntitySlime.Field_767_b + (par1EntitySlime.Field_768_a - par1EntitySlime.Field_767_b) * par2) / ((float)i * 0.5F + 1.0F);
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
			ScaleSlime((EntitySlime)par1EntityLiving, par2);
		}

		/// <summary>
		/// Queries whether should render the specified pass or not.
		/// </summary>
		protected override int ShouldRenderPass(EntityLiving par1EntityLiving, int par2, float par3)
		{
			return Func_40287_a((EntitySlime)par1EntityLiving, par2, par3);
		}
	}
}