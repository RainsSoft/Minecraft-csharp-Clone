namespace net.minecraft.src
{
	using Microsoft.Xna.Framework;

	public class RenderGhast : RenderLiving
	{
		public RenderGhast() : base(new ModelGhast(), 0.5F)
		{
		}

		protected virtual void Func_4014_a(EntityGhast par1EntityGhast, float par2)
		{
			EntityGhast entityghast = par1EntityGhast;
			float f = ((float)entityghast.PrevAttackCounter + (float)(entityghast.AttackCounter - entityghast.PrevAttackCounter) * par2) / 20F;

			if (f < 0.0F)
			{
				f = 0.0F;
			}

			f = 1.0F / (f * f * f * f * f * 2.0F + 1.0F);
			float f1 = (8F + f) / 2.0F;
			float f2 = (8F + 1.0F / f) / 2.0F;
			//GL.Scale(f2, f1, f2);
			//GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
		}

		/// <summary>
		/// Allows the render to do any OpenGL state modifications necessary before the model is rendered. Args:
		/// entityLiving, partialTickTime
		/// </summary>
		protected override void PreRenderCallback(EntityLiving par1EntityLiving, float par2)
		{
			Func_4014_a((EntityGhast)par1EntityLiving, par2);
		}
	}
}