namespace net.minecraft.src
{
	using Microsoft.Xna.Framework;

	public class RenderBoat : Render
	{
		/// <summary>
		/// instance of ModelBoat for rendering </summary>
		protected ModelBase ModelBoat;

		public RenderBoat()
		{
			ShadowSize = 0.5F;
			ModelBoat = new ModelBoat();
		}

		/// <summary>
		/// The render method used in RenderBoat that renders the boat model.
		/// </summary>
		public virtual void DoRenderBoat(EntityBoat par1EntityBoat, double par2, double par4, double par6, float par8, float par9)
		{
			//GL.PushMatrix();
			//GL.Translate((float)par2, (float)par4, (float)par6);
			//GL.Rotate(180F - par8, 0.0F, 1.0F, 0.0F);
			float f = (float)par1EntityBoat.GetTimeSinceHit() - par9;
			float f1 = (float)par1EntityBoat.GetDamageTaken() - par9;

			if (f1 < 0.0F)
			{
				f1 = 0.0F;
			}

			if (f > 0.0F)
			{
				//GL.Rotate(((MathHelper.Sin(f) * f * f1) / 10F) * (float)par1EntityBoat.GetForwardDirection(), 1.0F, 0.0F, 0.0F);
			}

			LoadTexture("/terrain.png");
			float f2 = 0.75F;
			//GL.Scale(f2, f2, f2);
			//GL.Scale(1.0F / f2, 1.0F / f2, 1.0F / f2);
			LoadTexture("/item/boat.png");
			//GL.Scale(-1F, -1F, 1.0F);
			ModelBoat.Render(par1EntityBoat, 0.0F, 0.0F, -0.1F, 0.0F, 0.0F, 0.0625F);
			//GL.PopMatrix();
		}

		/// <summary>
		/// Actually renders the given argument. This is a synthetic bridge method, always casting down its argument and then
		/// handing it off to a worker function which does the actual work. In all probabilty, the class Render is generic
		/// (Render<T extends Entity) and this method has signature public void doRender(T entity, double d, double d1,
		/// double d2, float f, float f1). But JAD is pre 1.5 so doesn't do that.
		/// </summary>
		public override void DoRender(Entity par1Entity, double par2, double par4, double par6, float par8, float par9)
		{
			DoRenderBoat((EntityBoat)par1Entity, par2, par4, par6, par8, par9);
		}
	}
}