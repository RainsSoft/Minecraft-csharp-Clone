namespace net.minecraft.src
{
	using Microsoft.Xna.Framework;

	public class RenderTNTPrimed : Render
	{
		private RenderBlocks BlockRenderer;

		public RenderTNTPrimed()
		{
			BlockRenderer = new RenderBlocks();
			ShadowSize = 0.5F;
		}

		public virtual void Func_153_a(EntityTNTPrimed par1EntityTNTPrimed, double par2, double par4, double par6, float par8, float par9)
		{
			//GL.PushMatrix();
			//GL.Translate((float)par2, (float)par4, (float)par6);

			if (((float)par1EntityTNTPrimed.Fuse - par9) + 1.0F < 10F)
			{
				float f = 1.0F - (((float)par1EntityTNTPrimed.Fuse - par9) + 1.0F) / 10F;

				if (f < 0.0F)
				{
					f = 0.0F;
				}

				if (f > 1.0F)
				{
					f = 1.0F;
				}

				f *= f;
				f *= f;
				float f2 = 1.0F + f * 0.3F;
				//GL.Scale(f2, f2, f2);
			}

			float f1 = (1.0F - (((float)par1EntityTNTPrimed.Fuse - par9) + 1.0F) / 100F) * 0.8F;
			LoadTexture("/terrain.png");
			BlockRenderer.RenderBlockAsItem(Block.Tnt, 0, par1EntityTNTPrimed.GetBrightness(par9));

			if ((par1EntityTNTPrimed.Fuse / 5) % 2 == 0)
			{
				//GL.Disable(EnableCap.Texture2D);
				//GL.Disable(EnableCap.Lighting);
				//GL.Enable(EnableCap.Blend);
				//GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.DstAlpha);
				//GL.Color4(1.0F, 1.0F, 1.0F, f1);
				BlockRenderer.RenderBlockAsItem(Block.Tnt, 0, 1.0F);
				//GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
				//GL.Disable(EnableCap.Blend);
				//GL.Enable(EnableCap.Lighting);
				//GL.Enable(EnableCap.Texture2D);
			}

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
			Func_153_a((EntityTNTPrimed)par1Entity, par2, par4, par6, par8, par9);
		}
	}
}