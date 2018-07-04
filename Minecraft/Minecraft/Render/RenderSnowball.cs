using Microsoft.Xna.Framework;

namespace net.minecraft.src
{
	public class RenderSnowball : Render
	{
		/// <summary>
		/// Have the icon index (in items.png) that will be used to render the image. Currently, eggs and snowballs uses this
		/// classes.
		/// </summary>
		private int ItemIconIndex;

		public RenderSnowball(int par1)
		{
			ItemIconIndex = par1;
		}

		/// <summary>
		/// Actually renders the given argument. This is a synthetic bridge method, always casting down its argument and then
		/// handing it off to a worker function which does the actual work. In all probabilty, the class Render is generic
		/// (Render<T extends Entity) and this method has signature public void doRender(T entity, double d, double d1,
		/// double d2, float f, float f1). But JAD is pre 1.5 so doesn't do that.
		/// </summary>
		public override void DoRender(Entity par1Entity, double par2, double par4, double par6, float par8, float par9)
		{
			//GL.PushMatrix();
			//GL.Translate((float)par2, (float)par4, (float)par6);
			//GL.Enable(EnableCap.RescaleNormal);
			//GL.Scale(0.5F, 0.5F, 0.5F);
			LoadTexture("/gui/items.png");
			Tessellator tessellator = Tessellator.Instance;

			if (ItemIconIndex == 154)
			{
				int i = PotionHelper.Func_40358_a(((EntityPotion)par1Entity).GetPotionDamage(), false);
				float f = (float)(i >> 16 & 0xff) / 255F;
				float f1 = (float)(i >> 8 & 0xff) / 255F;
				float f2 = (float)(i & 0xff) / 255F;
				//GL.Color3(f, f1, f2);
				//GL.PushMatrix();
				Func_40265_a(tessellator, 141);
				//GL.PopMatrix();
				//GL.Color3(1.0F, 1.0F, 1.0F);
			}

			Func_40265_a(tessellator, ItemIconIndex);
			//GL.Disable(EnableCap.RescaleNormal);
			//GL.PopMatrix();
		}

		private void Func_40265_a(Tessellator par1Tessellator, int par2)
		{
			float f = (float)((par2 % 16) * 16 + 0) / 256F;
			float f1 = (float)((par2 % 16) * 16 + 16) / 256F;
			float f2 = (float)((par2 / 16) * 16 + 0) / 256F;
			float f3 = (float)((par2 / 16) * 16 + 16) / 256F;
			float f4 = 1.0F;
			float f5 = 0.5F;
			float f6 = 0.25F;
			//GL.Rotate(180F - RenderManager.PlayerViewY, 0.0F, 1.0F, 0.0F);
			//GL.Rotate(-RenderManager.PlayerViewX, 1.0F, 0.0F, 0.0F);
			par1Tessellator.StartDrawingQuads();
			par1Tessellator.SetNormal(0.0F, 1.0F, 0.0F);
			par1Tessellator.AddVertexWithUV(0.0F - f5, 0.0F - f6, 0.0F, f, f3);
			par1Tessellator.AddVertexWithUV(f4 - f5, 0.0F - f6, 0.0F, f1, f3);
			par1Tessellator.AddVertexWithUV(f4 - f5, f4 - f6, 0.0F, f1, f2);
			par1Tessellator.AddVertexWithUV(0.0F - f5, f4 - f6, 0.0F, f, f2);
			par1Tessellator.Draw();
		}
	}
}