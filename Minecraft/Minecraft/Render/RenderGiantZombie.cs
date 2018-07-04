namespace net.minecraft.src
{

	using Microsoft.Xna.Framework;

	public class RenderGiantZombie : RenderLiving
	{
		/// <summary>
		/// Scale of the model to use </summary>
		private float Scale;

		public RenderGiantZombie(ModelBase par1ModelBase, float par2, float par3) : base(par1ModelBase, par2 * par3)
		{
			Scale = par3;
		}

		/// <summary>
		/// Applies the scale to the transform matrix
		/// </summary>
		protected virtual void PreRenderScale(EntityGiantZombie par1EntityGiantZombie, float par2)
		{
			//GL.Scale(Scale, Scale, Scale);
		}

		/// <summary>
		/// Allows the render to do any OpenGL state modifications necessary before the model is rendered. Args:
		/// entityLiving, partialTickTime
		/// </summary>
		protected override void PreRenderCallback(EntityLiving par1EntityLiving, float par2)
		{
			PreRenderScale((EntityGiantZombie)par1EntityLiving, par2);
		}
	}

}