using System;
using Microsoft.Xna.Framework;

namespace net.minecraft.src
{
	public class RenderMinecart : Render
	{
		/// <summary>
		/// instance of ModelMinecart for rendering </summary>
		protected ModelBase ModelMinecart;

		public RenderMinecart()
		{
			ShadowSize = 0.5F;
			ModelMinecart = new ModelMinecart();
		}

		public virtual void Func_152_a(EntityMinecart par1EntityMinecart, double par2, double par4, double par6, float par8, float par9)
		{
			//GL.PushMatrix();
			long l = (long)par1EntityMinecart.EntityId * 0x1d66f537L;
			l = l * l * 0x105cb26d1L + l * 0x181c9L;
			float f = (((float)(l >> 16 & 7L) + 0.5F) / 8F - 0.5F) * 0.004F;
			float f1 = (((float)(l >> 20 & 7L) + 0.5F) / 8F - 0.5F) * 0.004F;
			float f2 = (((float)(l >> 24 & 7L) + 0.5F) / 8F - 0.5F) * 0.004F;
			//GL.Translate(f, f1, f2);
			double d = par1EntityMinecart.LastTickPosX + (par1EntityMinecart.PosX - par1EntityMinecart.LastTickPosX) * (double)par9;
			double d1 = par1EntityMinecart.LastTickPosY + (par1EntityMinecart.PosY - par1EntityMinecart.LastTickPosY) * (double)par9;
			double d2 = par1EntityMinecart.LastTickPosZ + (par1EntityMinecart.PosZ - par1EntityMinecart.LastTickPosZ) * (double)par9;
			double d3 = 0.30000001192092896D;
			Vec3D vec3d = par1EntityMinecart.Func_514_g(d, d1, d2);
			float f3 = par1EntityMinecart.PrevRotationPitch + (par1EntityMinecart.RotationPitch - par1EntityMinecart.PrevRotationPitch) * par9;

			if (vec3d != null)
			{
				Vec3D vec3d1 = par1EntityMinecart.Func_515_a(d, d1, d2, d3);
				Vec3D vec3d2 = par1EntityMinecart.Func_515_a(d, d1, d2, -d3);

				if (vec3d1 == null)
				{
					vec3d1 = vec3d;
				}

				if (vec3d2 == null)
				{
					vec3d2 = vec3d;
				}

				par2 += vec3d.XCoord - d;
				par4 += (vec3d1.YCoord + vec3d2.YCoord) / 2D - d1;
				par6 += vec3d.ZCoord - d2;
				Vec3D vec3d3 = vec3d2.AddVector(-vec3d1.XCoord, -vec3d1.YCoord, -vec3d1.ZCoord);

				if (vec3d3.LengthVector() != 0.0F)
				{
					vec3d3 = vec3d3.Normalize();
					par8 = (float)((Math.Atan2(vec3d3.ZCoord, vec3d3.XCoord) * 180D) / Math.PI);
					f3 = (float)(Math.Atan(vec3d3.YCoord) * 73D);
				}
			}

			//GL.Translate((float)par2, (float)par4, (float)par6);
			//GL.Rotate(180F - par8, 0.0F, 1.0F, 0.0F);
			//GL.Rotate(-f3, 0.0F, 0.0F, 1.0F);
			float f4 = (float)par1EntityMinecart.Func_41023_l() - par9;
			float f5 = (float)par1EntityMinecart.Func_41025_i() - par9;

			if (f5 < 0.0F)
			{
				f5 = 0.0F;
			}

			if (f4 > 0.0F)
			{
				//GL.Rotate(((MathHelper.Sin(f4) * f4 * f5) / 10F) * (float)par1EntityMinecart.Func_41030_m(), 1.0F, 0.0F, 0.0F);
			}

			if (par1EntityMinecart.MinecartType != 0)
			{
				LoadTexture("/terrain.png");
				float f6 = 0.75F;
				//GL.Scale(f6, f6, f6);

				if (par1EntityMinecart.MinecartType == 1)
				{
					//GL.Translate(-0.5F, 0.0F, 0.5F);
					//GL.Rotate(90F, 0.0F, 1.0F, 0.0F);
					(new RenderBlocks()).RenderBlockAsItem(Block.Chest, 0, par1EntityMinecart.GetBrightness(par9));
					//GL.Rotate(-90F, 0.0F, 1.0F, 0.0F);
					//GL.Translate(0.5F, 0.0F, -0.5F);
					//GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
				}
				else if (par1EntityMinecart.MinecartType == 2)
				{
					//GL.Translate(0.0F, 0.3125F, 0.0F);
					//GL.Rotate(90F, 0.0F, 1.0F, 0.0F);
					(new RenderBlocks()).RenderBlockAsItem(Block.StoneOvenIdle, 0, par1EntityMinecart.GetBrightness(par9));
					//GL.Rotate(-90F, 0.0F, 1.0F, 0.0F);
					//GL.Translate(0.0F, -0.3125F, 0.0F);
					//GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
				}

				//GL.Scale(1.0F / f6, 1.0F / f6, 1.0F / f6);
			}

			LoadTexture("/item/cart.png");
			//GL.Scale(-1F, -1F, 1.0F);
			ModelMinecart.Render(par1EntityMinecart, 0.0F, 0.0F, -0.1F, 0.0F, 0.0F, 0.0625F);
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
			Func_152_a((EntityMinecart)par1Entity, par2, par4, par6, par8, par9);
		}
	}
}