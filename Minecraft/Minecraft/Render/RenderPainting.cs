using System;

namespace net.minecraft.src
{
	using Microsoft.Xna.Framework;

	public class RenderPainting : Render
	{
		/// <summary>
		/// RNG. </summary>
		private Random Rand;

		public RenderPainting()
		{
			Rand = new Random();
		}

		public virtual void Func_158_a(EntityPainting par1EntityPainting, double par2, double par4, double par6, float par8, float par9)
		{
			Rand.SetSeed(187);
			//GL.PushMatrix();
			//GL.Translate((float)par2, (float)par4, (float)par6);
			//GL.Rotate(par8, 0.0F, 1.0F, 0.0F);
			//GL.Enable(EnableCap.RescaleNormal);
			LoadTexture("/art/kz.png");
			Art enumart = par1EntityPainting.Art;
			float f = 0.0625F;
			//GL.Scale(f, f, f);
			Func_159_a(par1EntityPainting, enumart.SizeX, enumart.SizeY, enumart.OffsetX, enumart.OffsetY);
			//GL.Disable(EnableCap.RescaleNormal);
			//GL.PopMatrix();
		}

		private void Func_159_a(EntityPainting par1EntityPainting, int par2, int par3, int par4, int par5)
		{
			float f = (float)(-par2) / 2.0F;
			float f1 = (float)(-par3) / 2.0F;
			float f2 = -0.5F;
			float f3 = 0.5F;

			for (int i = 0; i < par2 / 16; i++)
			{
				for (int j = 0; j < par3 / 16; j++)
				{
					float f4 = f + (float)((i + 1) * 16);
					float f5 = f + (float)(i * 16);
					float f6 = f1 + (float)((j + 1) * 16);
					float f7 = f1 + (float)(j * 16);
					Func_160_a(par1EntityPainting, (f4 + f5) / 2.0F, (f6 + f7) / 2.0F);
					float f8 = (float)((par4 + par2) - i * 16) / 256F;
					float f9 = (float)((par4 + par2) - (i + 1) * 16) / 256F;
					float f10 = (float)((par5 + par3) - j * 16) / 256F;
					float f11 = (float)((par5 + par3) - (j + 1) * 16) / 256F;
					float f12 = 0.75F;
					float f13 = 0.8125F;
					float f14 = 0.0F;
					float f15 = 0.0625F;
					float f16 = 0.75F;
					float f17 = 0.8125F;
					float f18 = 0.001953125F;
					float f19 = 0.001953125F;
					float f20 = 0.7519531F;
					float f21 = 0.7519531F;
					float f22 = 0.0F;
					float f23 = 0.0625F;
					Tessellator tessellator = Tessellator.Instance;
					tessellator.StartDrawingQuads();
					tessellator.SetNormal(0.0F, 0.0F, -1F);
					tessellator.AddVertexWithUV(f4, f7, f2, f9, f10);
					tessellator.AddVertexWithUV(f5, f7, f2, f8, f10);
					tessellator.AddVertexWithUV(f5, f6, f2, f8, f11);
					tessellator.AddVertexWithUV(f4, f6, f2, f9, f11);
					tessellator.SetNormal(0.0F, 0.0F, 1.0F);
					tessellator.AddVertexWithUV(f4, f6, f3, f12, f14);
					tessellator.AddVertexWithUV(f5, f6, f3, f13, f14);
					tessellator.AddVertexWithUV(f5, f7, f3, f13, f15);
					tessellator.AddVertexWithUV(f4, f7, f3, f12, f15);
					tessellator.SetNormal(0.0F, 1.0F, 0.0F);
					tessellator.AddVertexWithUV(f4, f6, f2, f16, f18);
					tessellator.AddVertexWithUV(f5, f6, f2, f17, f18);
					tessellator.AddVertexWithUV(f5, f6, f3, f17, f19);
					tessellator.AddVertexWithUV(f4, f6, f3, f16, f19);
					tessellator.SetNormal(0.0F, -1F, 0.0F);
					tessellator.AddVertexWithUV(f4, f7, f3, f16, f18);
					tessellator.AddVertexWithUV(f5, f7, f3, f17, f18);
					tessellator.AddVertexWithUV(f5, f7, f2, f17, f19);
					tessellator.AddVertexWithUV(f4, f7, f2, f16, f19);
					tessellator.SetNormal(-1F, 0.0F, 0.0F);
					tessellator.AddVertexWithUV(f4, f6, f3, f21, f22);
					tessellator.AddVertexWithUV(f4, f7, f3, f21, f23);
					tessellator.AddVertexWithUV(f4, f7, f2, f20, f23);
					tessellator.AddVertexWithUV(f4, f6, f2, f20, f22);
					tessellator.SetNormal(1.0F, 0.0F, 0.0F);
					tessellator.AddVertexWithUV(f5, f6, f2, f21, f22);
					tessellator.AddVertexWithUV(f5, f7, f2, f21, f23);
					tessellator.AddVertexWithUV(f5, f7, f3, f20, f23);
					tessellator.AddVertexWithUV(f5, f6, f3, f20, f22);
					tessellator.Draw();
				}
			}
		}

		private void Func_160_a(EntityPainting par1EntityPainting, float par2, float par3)
		{
			int i = MathHelper2.Floor_double(par1EntityPainting.PosX);
			int j = MathHelper2.Floor_double(par1EntityPainting.PosY + (double)(par3 / 16F));
			int k = MathHelper2.Floor_double(par1EntityPainting.PosZ);

			if (par1EntityPainting.Direction == 0)
			{
				i = MathHelper2.Floor_double(par1EntityPainting.PosX + (double)(par2 / 16F));
			}

			if (par1EntityPainting.Direction == 1)
			{
				k = MathHelper2.Floor_double(par1EntityPainting.PosZ - (double)(par2 / 16F));
			}

			if (par1EntityPainting.Direction == 2)
			{
				i = MathHelper2.Floor_double(par1EntityPainting.PosX - (double)(par2 / 16F));
			}

			if (par1EntityPainting.Direction == 3)
			{
				k = MathHelper2.Floor_double(par1EntityPainting.PosZ + (double)(par2 / 16F));
			}

			int l = RenderManager.WorldObj.GetLightBrightnessForSkyBlocks(i, j, k, 0);
			int i1 = l % 0x10000;
			int j1 = l / 0x10000;
			OpenGlHelper.SetLightmapTextureCoords(OpenGlHelper.LightmapTexUnit, i1, j1);
			//GL.Color3(1.0F, 1.0F, 1.0F);
		}

		/// <summary>
		/// Actually renders the given argument. This is a synthetic bridge method, always casting down its argument and then
		/// handing it off to a worker function which does the actual work. In all probabilty, the class Render is generic
		/// (Render<T extends Entity) and this method has signature public void doRender(T entity, double d, double d1,
		/// double d2, float f, float f1). But JAD is pre 1.5 so doesn't do that.
		/// </summary>
		public override void DoRender(Entity par1Entity, double par2, double par4, double par6, float par8, float par9)
		{
			Func_158_a((EntityPainting)par1Entity, par2, par4, par6, par8, par9);
		}
	}
}