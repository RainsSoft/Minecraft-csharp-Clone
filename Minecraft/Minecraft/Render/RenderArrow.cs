using Microsoft.Xna.Framework;

namespace net.minecraft.src
{
	public class RenderArrow : Render
	{
		public RenderArrow()
		{
		}

		public virtual void DoRenderArrow(EntityArrow par1EntityArrow, double par2, double par4, double par6, float par8, float par9)
		{
			LoadTexture("/item/arrows.png");
			//GL.PushMatrix();
			//GL.Translate((float)par2, (float)par4, (float)par6);
			//GL.Rotate((par1EntityArrow.PrevRotationYaw + (par1EntityArrow.RotationYaw - par1EntityArrow.PrevRotationYaw) * par9) - 90F, 0.0F, 1.0F, 0.0F);
			//GL.Rotate(par1EntityArrow.PrevRotationPitch + (par1EntityArrow.RotationPitch - par1EntityArrow.PrevRotationPitch) * par9, 0.0F, 0.0F, 1.0F);
			Tessellator tessellator = Tessellator.Instance;
			int i = 0;
			float f = 0.0F;
			float f1 = 0.5F;
			float f2 = (float)(0 + i * 10) / 32F;
			float f3 = (float)(5 + i * 10) / 32F;
			float f4 = 0.0F;
			float f5 = 0.15625F;
			float f6 = (float)(5 + i * 10) / 32F;
			float f7 = (float)(10 + i * 10) / 32F;
			float f8 = 0.05625F;
			//GL.Enable(EnableCap.RescaleNormal);
			float f9 = (float)par1EntityArrow.ArrowShake - par9;

			if (f9 > 0.0F)
			{
				float f10 = -MathHelper2.Sin(f9 * 3F) * f9;
				//GL.Rotate(f10, 0.0F, 0.0F, 1.0F);
			}

			//GL.Rotate(45F, 1.0F, 0.0F, 0.0F);
			//GL.Scale(f8, f8, f8);
			//GL.Translate(-4F, 0.0F, 0.0F);
			//GL.Normal3(f8, 0.0F, 0.0F);
			tessellator.StartDrawingQuads();
			tessellator.AddVertexWithUV(-7D, -2D, -2D, f4, f6);
			tessellator.AddVertexWithUV(-7D, -2D, 2D, f5, f6);
			tessellator.AddVertexWithUV(-7D, 2D, 2D, f5, f7);
			tessellator.AddVertexWithUV(-7D, 2D, -2D, f4, f7);
			tessellator.Draw();
			//GL.Normal3(-f8, 0.0F, 0.0F);
			tessellator.StartDrawingQuads();
			tessellator.AddVertexWithUV(-7D, 2D, -2D, f4, f6);
			tessellator.AddVertexWithUV(-7D, 2D, 2D, f5, f6);
			tessellator.AddVertexWithUV(-7D, -2D, 2D, f5, f7);
			tessellator.AddVertexWithUV(-7D, -2D, -2D, f4, f7);
			tessellator.Draw();

			for (int j = 0; j < 4; j++)
			{
				//GL.Rotate(90F, 1.0F, 0.0F, 0.0F);
				//GL.Normal3(0.0F, 0.0F, f8);
				tessellator.StartDrawingQuads();
				tessellator.AddVertexWithUV(-8D, -2D, 0.0F, f, f2);
				tessellator.AddVertexWithUV(8D, -2D, 0.0F, f1, f2);
				tessellator.AddVertexWithUV(8D, 2D, 0.0F, f1, f3);
				tessellator.AddVertexWithUV(-8D, 2D, 0.0F, f, f3);
				tessellator.Draw();
			}

			//GL.Disable(EnableCap.RescaleNormal);
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
			DoRenderArrow((EntityArrow)par1Entity, par2, par4, par6, par8, par9);
		}
	}
}