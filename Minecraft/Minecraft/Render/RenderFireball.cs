using Microsoft.Xna.Framework;

namespace net.minecraft.src
{
	public class RenderFireball : Render
	{
		private float Field_40269_a;

		public RenderFireball(float par1)
		{
			Field_40269_a = par1;
		}

		public virtual void DoRenderFireball(EntityFireball par1EntityFireball, double par2, double par4, double par6, float par8, float par9)
		{
			//GL.PushMatrix();
			//GL.Translate((float)par2, (float)par4, (float)par6);
			//GL.Enable(EnableCap.RescaleNormal);
			float f = Field_40269_a;
			//GL.Scale(f / 1.0F, f / 1.0F, f / 1.0F);
			sbyte byte0 = 46;
			LoadTexture("/gui/items.png");
			Tessellator tessellator = Tessellator.Instance;
			float f1 = (float)((byte0 % 16) * 16 + 0) / 256F;
			float f2 = (float)((byte0 % 16) * 16 + 16) / 256F;
			float f3 = (float)((byte0 / 16) * 16 + 0) / 256F;
			float f4 = (float)((byte0 / 16) * 16 + 16) / 256F;
			float f5 = 1.0F;
			float f6 = 0.5F;
			float f7 = 0.25F;
			//GL.Rotate(180F - RenderManager.PlayerViewY, 0.0F, 1.0F, 0.0F);
			//GL.Rotate(-RenderManager.PlayerViewX, 1.0F, 0.0F, 0.0F);
			tessellator.StartDrawingQuads();
			tessellator.SetNormal(0.0F, 1.0F, 0.0F);
			tessellator.AddVertexWithUV(0.0F - f6, 0.0F - f7, 0.0F, f1, f4);
			tessellator.AddVertexWithUV(f5 - f6, 0.0F - f7, 0.0F, f2, f4);
			tessellator.AddVertexWithUV(f5 - f6, 1.0F - f7, 0.0F, f2, f3);
			tessellator.AddVertexWithUV(0.0F - f6, 1.0F - f7, 0.0F, f1, f3);
			tessellator.Draw();
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
			DoRenderFireball((EntityFireball)par1Entity, par2, par4, par6, par8, par9);
		}
	}
}