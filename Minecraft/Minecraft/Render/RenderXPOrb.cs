using Microsoft.Xna.Framework;

namespace net.minecraft.src
{
	public class RenderXPOrb : Render
	{
		private RenderBlocks Field_35439_b;
		public bool Field_35440_a;

		public RenderXPOrb()
		{
			Field_35439_b = new RenderBlocks();
			Field_35440_a = true;
			ShadowSize = 0.15F;
			ShadowOpaque = 0.75F;
		}

		public virtual void Func_35438_a(EntityXPOrb par1EntityXPOrb, double par2, double par4, double par6, float par8, float par9)
		{
			//GL.PushMatrix();
			//GL.Translate((float)par2, (float)par4, (float)par6);
			int i = par1EntityXPOrb.GetTextureByXP();
			LoadTexture("/item/xporb.png");
			Tessellator tessellator = Tessellator.Instance;
			float f = (float)((i % 4) * 16 + 0) / 64F;
			float f1 = (float)((i % 4) * 16 + 16) / 64F;
			float f2 = (float)((i / 4) * 16 + 0) / 64F;
			float f3 = (float)((i / 4) * 16 + 16) / 64F;
			float f4 = 1.0F;
			float f5 = 0.5F;
			float f6 = 0.25F;
			int i7 = par1EntityXPOrb.GetBrightnessForRender(par9);
			float f8 = i7 % 0x10000;
			int j = i7 / 0x10000;
			OpenGlHelper.SetLightmapTextureCoords(OpenGlHelper.LightmapTexUnit, (float)f8 / 1.0F, (float)j / 1.0F);
			//GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
			float f7 = 255F;
			f8 = ((float)par1EntityXPOrb.XpColor + par9) / 2.0F;
			j = (int)((MathHelper2.Sin(f8 + 0.0F) + 1.0F) * 0.5F * f7);
			int k = (int)f7;
			int l = (int)((MathHelper2.Sin(f8 + 4.18879F) + 1.0F) * 0.1F * f7);
			int i1 = j << 16 | k << 8 | l;
			//GL.Rotate(180F - RenderManager.PlayerViewY, 0.0F, 1.0F, 0.0F);
			//GL.Rotate(-RenderManager.PlayerViewX, 1.0F, 0.0F, 0.0F);
			float f9 = 0.3F;
			//GL.Scale(f9, f9, f9);
			tessellator.StartDrawingQuads();
			tessellator.SetColorRGBA_I(i1, 128);
			tessellator.SetNormal(0.0F, 1.0F, 0.0F);
			tessellator.AddVertexWithUV(0.0F - f5, 0.0F - f6, 0.0F, f, f3);
			tessellator.AddVertexWithUV(f4 - f5, 0.0F - f6, 0.0F, f1, f3);
			tessellator.AddVertexWithUV(f4 - f5, 1.0F - f6, 0.0F, f1, f2);
			tessellator.AddVertexWithUV(0.0F - f5, 1.0F - f6, 0.0F, f, f2);
			tessellator.Draw();
			//GL.Disable(EnableCap.Blend);
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
			Func_35438_a((EntityXPOrb)par1Entity, par2, par4, par6, par8, par9);
		}
	}
}