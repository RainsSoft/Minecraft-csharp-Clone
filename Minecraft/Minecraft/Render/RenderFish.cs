using System;
using Microsoft.Xna.Framework;

namespace net.minecraft.src
{
	public class RenderFish : Render
	{
		public RenderFish()
		{
		}

		/// <summary>
		/// Actually renders the fishing line and hook
		/// </summary>
		public virtual void DoRenderFishHook(EntityFishHook par1EntityFishHook, double par2, double par4, double par6, float par8, float par9)
		{
			//GL.PushMatrix();
			//GL.Translate((float)par2, (float)par4, (float)par6);
			//GL.Enable(EnableCap.RescaleNormal);
			//GL.Scale(0.5F, 0.5F, 0.5F);
			int i = 1;
			sbyte byte0 = 2;
			LoadTexture("/particles.png");
			Tessellator tessellator = Tessellator.Instance;
			float f = (float)(i * 8 + 0) / 128F;
			float f1 = (float)(i * 8 + 8) / 128F;
			float f2 = (float)(byte0 * 8 + 0) / 128F;
			float f3 = (float)(byte0 * 8 + 8) / 128F;
			float f4 = 1.0F;
			float f5 = 0.5F;
			float f6 = 0.5F;
			//GL.Rotate(180F - RenderManager.PlayerViewY, 0.0F, 1.0F, 0.0F);
			//GL.Rotate(-RenderManager.PlayerViewX, 1.0F, 0.0F, 0.0F);
			tessellator.StartDrawingQuads();
			tessellator.SetNormal(0.0F, 1.0F, 0.0F);
			tessellator.AddVertexWithUV(0.0F - f5, 0.0F - f6, 0.0F, f, f3);
			tessellator.AddVertexWithUV(f4 - f5, 0.0F - f6, 0.0F, f1, f3);
			tessellator.AddVertexWithUV(f4 - f5, 1.0F - f6, 0.0F, f1, f2);
			tessellator.AddVertexWithUV(0.0F - f5, 1.0F - f6, 0.0F, f, f2);
			tessellator.Draw();
			//GL.Disable(EnableCap.RescaleNormal);
			//GL.PopMatrix();

			if (par1EntityFishHook.Angler != null)
			{
				float f7 = ((par1EntityFishHook.Angler.PrevRotationYaw + (par1EntityFishHook.Angler.RotationYaw - par1EntityFishHook.Angler.PrevRotationYaw) * par9) * (float)Math.PI) / 180F;
				double d = MathHelper2.Sin(f7);
				double d2 = MathHelper2.Cos(f7);
				float f9 = par1EntityFishHook.Angler.GetSwingProgress(par9);
				float f10 = MathHelper2.Sin(MathHelper2.Sqrt_float(f9) * (float)Math.PI);
				Vec3D vec3d = Vec3D.CreateVector(-0.5D, 0.029999999999999999D, 0.80000000000000004D);
				vec3d.RotateAroundX((-(par1EntityFishHook.Angler.PrevRotationPitch + (par1EntityFishHook.Angler.RotationPitch - par1EntityFishHook.Angler.PrevRotationPitch) * par9) * (float)Math.PI) / 180F);
				vec3d.RotateAroundY((-(par1EntityFishHook.Angler.PrevRotationYaw + (par1EntityFishHook.Angler.RotationYaw - par1EntityFishHook.Angler.PrevRotationYaw) * par9) * (float)Math.PI) / 180F);
				vec3d.RotateAroundY(f10 * 0.5F);
				vec3d.RotateAroundX(-f10 * 0.7F);
				double d4 = par1EntityFishHook.Angler.PrevPosX + (par1EntityFishHook.Angler.PosX - par1EntityFishHook.Angler.PrevPosX) * (double)par9 + vec3d.XCoord;
				double d5 = par1EntityFishHook.Angler.PrevPosY + (par1EntityFishHook.Angler.PosY - par1EntityFishHook.Angler.PrevPosY) * (double)par9 + vec3d.YCoord;
				double d6 = par1EntityFishHook.Angler.PrevPosZ + (par1EntityFishHook.Angler.PosZ - par1EntityFishHook.Angler.PrevPosZ) * (double)par9 + vec3d.ZCoord;

				if (RenderManager.Options.ThirdPersonView > 0)
				{
					float f8 = ((par1EntityFishHook.Angler.PrevRenderYawOffset + (par1EntityFishHook.Angler.RenderYawOffset - par1EntityFishHook.Angler.PrevRenderYawOffset) * par9) * (float)Math.PI) / 180F;
					double d1 = MathHelper2.Sin(f8);
					double d3 = MathHelper2.Cos(f8);
					d4 = (par1EntityFishHook.Angler.PrevPosX + (par1EntityFishHook.Angler.PosX - par1EntityFishHook.Angler.PrevPosX) * (double)par9) - d3 * 0.34999999999999998D - d1 * 0.84999999999999998D;
					d5 = (par1EntityFishHook.Angler.PrevPosY + (par1EntityFishHook.Angler.PosY - par1EntityFishHook.Angler.PrevPosY) * (double)par9) - 0.45000000000000001D;
					d6 = ((par1EntityFishHook.Angler.PrevPosZ + (par1EntityFishHook.Angler.PosZ - par1EntityFishHook.Angler.PrevPosZ) * (double)par9) - d1 * 0.34999999999999998D) + d3 * 0.84999999999999998D;
				}

				double d7 = par1EntityFishHook.PrevPosX + (par1EntityFishHook.PosX - par1EntityFishHook.PrevPosX) * (double)par9;
				double d8 = par1EntityFishHook.PrevPosY + (par1EntityFishHook.PosY - par1EntityFishHook.PrevPosY) * (double)par9 + 0.25D;
				double d9 = par1EntityFishHook.PrevPosZ + (par1EntityFishHook.PosZ - par1EntityFishHook.PrevPosZ) * (double)par9;
				double d10 = (float)(d4 - d7);
				double d11 = (float)(d5 - d8);
				double d12 = (float)(d6 - d9);
				//GL.Disable(EnableCap.Texture2D);
				//GL.Disable(EnableCap.Lighting);
				tessellator.StartDrawing(3);
				tessellator.SetColorOpaque_I(0);
				int j = 16;

				for (int k = 0; k <= j; k++)
				{
					float f11 = (float)k / (float)j;
					tessellator.AddVertex(par2 + d10 * (double)f11, par4 + d11 * (double)(f11 * f11 + f11) * 0.5D + 0.25D, par6 + d12 * (double)f11);
				}

				tessellator.Draw();
				//GL.Enable(EnableCap.Lighting);
				//GL.Enable(EnableCap.Texture2D);
			}
		}

		/// <summary>
		/// Actually renders the given argument. This is a synthetic bridge method, always casting down its argument and then
		/// handing it off to a worker function which does the actual work. In all probabilty, the class Render is generic
		/// (Render<T extends Entity) and this method has signature public void doRender(T entity, double d, double d1,
		/// double d2, float f, float f1). But JAD is pre 1.5 so doesn't do that.
		/// </summary>
		public override void DoRender(Entity par1Entity, double par2, double par4, double par6, float par8, float par9)
		{
			DoRenderFishHook((EntityFishHook)par1Entity, par2, par4, par6, par8, par9);
		}
	}
}