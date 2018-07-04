using System;

namespace net.minecraft.src
{
	using Microsoft.Xna.Framework;

	public class RenderEndPortal : TileEntitySpecialRenderer
	{
		//FloatBuffer Field_40448_a;

		public RenderEndPortal()
		{
			//Field_40448_a = GLAllocation.CreateDirectFloatBuffer(16);
		}

		public virtual void Func_40446_a(TileEntityEndPortal par1TileEntityEndPortal, double par2, double par4, double par6, float par8)
		{
			float f = (float)TileEntityRenderer.PlayerX;
			float f1 = (float)TileEntityRenderer.PlayerY;
			float f2 = (float)TileEntityRenderer.PlayerZ;
			//GL.Disable(EnableCap.Lighting);
			Random random = new Random(31100);
			float f3 = 0.75F;

			for (int i = 0; i < 16; i++)
			{
				//GL.PushMatrix();
				float f4 = 16 - i;
				float f5 = 0.0625F;
				float f6 = 1.0F / (f4 + 1.0F);

				if (i == 0)
				{
					BindTextureByName("/misc/tunnel.png");
					f6 = 0.1F;
					f4 = 65F;
					f5 = 0.125F;
					//GL.Enable(EnableCap.Blend);
					//GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
				}

				if (i == 1)
				{
					BindTextureByName("/misc/particlefield.png");
					//GL.Enable(EnableCap.Blend);
					//GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.One);
					f5 = 0.5F;
				}

				float f7 = (float)(-(par4 + (double)f3));
				float f8 = f7 + ActiveRenderInfo.ObjectY;
				float f9 = f7 + f4 + ActiveRenderInfo.ObjectY;
				float f10 = f8 / f9;
				f10 = (float)(par4 + (double)f3) + f10;
				//GL.Translate(f, f10, f2);
				//GL.TexGen(TextureCoordName.S, TextureGenParameter.TextureGenMode, (int)TextureGenMode.ObjectLinear);
                //GL.TexGen(TextureCoordName.T, TextureGenParameter.TextureGenMode, (int)TextureGenMode.ObjectLinear);
                //GL.TexGen(TextureCoordName.R, TextureGenParameter.TextureGenMode, (int)TextureGenMode.ObjectLinear);
                //GL.TexGen(TextureCoordName.Q, TextureGenParameter.TextureGenMode, (int)TextureGenMode.EyeLinear);/*
                //GL.TexGen(TextureCoordName.S, TextureGenParameter.ObjectPlane, Func_40447_a(1.0F, 0.0F, 0.0F, 0.0F));
                //GL.TexGen(TextureCoordName.T, TextureGenParameter.ObjectPlane, Func_40447_a(0.0F, 0.0F, 1.0F, 0.0F));
                //GL.TexGen(TextureCoordName.R, TextureGenParameter.ObjectPlane, Func_40447_a(0.0F, 0.0F, 0.0F, 1.0F));
                //GL.TexGen(TextureCoordName.Q, TextureGenParameter.EyePlane, Func_40447_a(0.0F, 1.0F, 0.0F, 0.0F));*/
				//GL.Enable(EnableCap.TextureGenS);
				//GL.Enable(EnableCap.TextureGenT);
				//GL.Enable(EnableCap.TextureGenR);
				//GL.Enable(EnableCap.TextureGenQ);
				//GL.PopMatrix();
				//GL.MatrixMode(MatrixMode.Texture);
				//GL.PushMatrix();
				//GL.LoadIdentity();
				//GL.Translate(0.0F, (float)(JavaHelper.CurrentTimeMillis() % 0xaae60L) / 700000F, 0.0F);
				//GL.Scale(f5, f5, f5);
				//GL.Translate(0.5F, 0.5F, 0.0F);
				//GL.Rotate((float)(i * i * 4321 + i * 9) * 2.0F, 0.0F, 0.0F, 1.0F);
				//GL.Translate(-0.5F, -0.5F, 0.0F);
				//GL.Translate(-f, -f2, -f1);
				f8 = f7 + ActiveRenderInfo.ObjectY;
				//GL.Translate((ActiveRenderInfo.ObjectX * f4) / f8, (ActiveRenderInfo.ObjectZ * f4) / f8, -f1);
				Tessellator tessellator = Tessellator.Instance;
				tessellator.StartDrawingQuads();
				f10 = random.NextFloat() * 0.5F + 0.1F;
				float f11 = random.NextFloat() * 0.5F + 0.4F;
				float f12 = random.NextFloat() * 0.5F + 0.5F;

				if (i == 0)
				{
					f10 = f11 = f12 = 1.0F;
				}

				tessellator.SetColorRGBA_F(f10 * f6, f11 * f6, f12 * f6, 1.0F);
				tessellator.AddVertex(par2, par4 + (double)f3, par6);
				tessellator.AddVertex(par2, par4 + (double)f3, par6 + 1.0D);
				tessellator.AddVertex(par2 + 1.0D, par4 + (double)f3, par6 + 1.0D);
				tessellator.AddVertex(par2 + 1.0D, par4 + (double)f3, par6);
				tessellator.Draw();
				//GL.PopMatrix();
				//GL.MatrixMode(MatrixMode.Modelview);
			}

			//GL.Disable(EnableCap.Blend);
			//GL.Disable(EnableCap.TextureGenS);
			//GL.Disable(EnableCap.TextureGenT);
			//GL.Disable(EnableCap.TextureGenR);
			//GL.Disable(EnableCap.TextureGenQ);
			//GL.Enable(EnableCap.Lighting);
		}
        /*
		private FloatBuffer Func_40447_a(float par1, float par2, float par3, float par4)
		{
			Field_40448_a.clear();
			Field_40448_a.put(par1).put(par2).put(par3).put(par4);
			Field_40448_a.flip();
			return Field_40448_a;
		}
        */
        public override void RenderTileEntityAt(TileEntity par1TileEntity, float par2, float par4, float par6, float par8)
		{
			Func_40446_a((TileEntityEndPortal)par1TileEntity, par2, par4, par6, par8);
		}
	}
}