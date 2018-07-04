using System;
using Microsoft.Xna.Framework;

namespace net.minecraft.src
{
	public class RenderDragon : RenderLiving
	{
		/// <summary>
		/// The entity instance of the dragon. Note: This is a static field in RenderDragon because there is only supposed to
		/// be one dragon
		/// </summary>
		public static EntityDragon EntityDragon;
		private static int Field_40284_d = 0;

		/// <summary>
		/// An instance of the dragon model in RenderDragon </summary>
		protected ModelDragon ModelDragon;

		public RenderDragon() : base(new ModelDragon(0.0F), 0.5F)
		{
			ModelDragon = (ModelDragon)MainModel;
			SetRenderPassModel(MainModel);
		}

		/// <summary>
		/// Used to rotate the dragon as a whole in RenderDragon. It's called in the rotateCorpse method.
		/// </summary>
		protected virtual void RotateDragonBody(EntityDragon par1EntityDragon, float par2, float par3, float par4)
		{
			float f = (float)par1EntityDragon.Func_40160_a(7, par4)[0];
			float f1 = (float)(par1EntityDragon.Func_40160_a(5, par4)[1] - par1EntityDragon.Func_40160_a(10, par4)[1]);
			//GL.Rotate(-f, 0.0F, 1.0F, 0.0F);
			//GL.Rotate(f1 * 10F, 1.0F, 0.0F, 0.0F);
			//GL.Translate(0.0F, 0.0F, 1.0F);

			if (par1EntityDragon.DeathTime > 0)
			{
				float f2 = ((((float)par1EntityDragon.DeathTime + par4) - 1.0F) / 20F) * 1.6F;
				f2 = MathHelper2.Sqrt_float(f2);

				if (f2 > 1.0F)
				{
					f2 = 1.0F;
				}

				//GL.Rotate(f2 * GetDeathMaxRotation(par1EntityDragon), 0.0F, 0.0F, 1.0F);
			}
		}

		protected virtual void Func_40280_a(EntityDragon par1EntityDragon, float par2, float par3, float par4, float par5, float par6, float par7)
		{
			if (par1EntityDragon.Field_40178_aA > 0)
			{
				float f = (float)par1EntityDragon.Field_40178_aA / 200F;
				//GL.DepthFunc(DepthFunction.Lequal);
                //GL.Enable(EnableCap.AlphaTest);
				//GL.AlphaFunc(AlphaFunction.Greater, f);
				LoadDownloadableImageTexture(par1EntityDragon.SkinUrl, "/mob/enderdragon/shuffle.png");
				MainModel.Render(par1EntityDragon, par2, par3, par4, par5, par6, par7);
				//GL.AlphaFunc(AlphaFunction.Greater, 0.1F);
				//GL.DepthFunc(DepthFunction.Equal);
			}

			LoadDownloadableImageTexture(par1EntityDragon.SkinUrl, par1EntityDragon.GetTexture());
			MainModel.Render(par1EntityDragon, par2, par3, par4, par5, par6, par7);

			if (par1EntityDragon.HurtTime > 0)
			{
				//GL.DepthFunc(DepthFunction.Equal);
				//GL.Disable(EnableCap.Texture2D);
                //GL.Enable(EnableCap.Blend);
				//GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
				//GL.Color4(1.0F, 0.0F, 0.0F, 0.5F);
				MainModel.Render(par1EntityDragon, par2, par3, par4, par5, par6, par7);
				//GL.Enable(EnableCap.Texture2D);
                //GL.Disable(EnableCap.Blend);
				//GL.DepthFunc(DepthFunction.Lequal);
			}
		}

		/// <summary>
		/// Renders the dragon, along with its dying animation
		/// </summary>
		public virtual void DoRenderDragon(EntityDragon par1EntityDragon, double par2, double par4, double par6, float par8, float par9)
		{
			EntityDragon = par1EntityDragon;

			if (Field_40284_d != 4)
			{
				MainModel = new ModelDragon(0.0F);
				Field_40284_d = 4;
			}

			base.DoRenderLiving(par1EntityDragon, par2, par4, par6, par8, par9);

			if (par1EntityDragon.HealingEnderCrystal != null)
			{
				float f = (float)par1EntityDragon.HealingEnderCrystal.InnerRotation + par9;
				float f1 = MathHelper2.Sin(f * 0.2F) / 2.0F + 0.5F;
				f1 = (f1 * f1 + f1) * 0.2F;
				float f2 = (float)(par1EntityDragon.HealingEnderCrystal.PosX - par1EntityDragon.PosX - (par1EntityDragon.PrevPosX - par1EntityDragon.PosX) * (double)(1.0F - par9));
				float f3 = (float)(((double)f1 + par1EntityDragon.HealingEnderCrystal.PosY) - 1.0D - par1EntityDragon.PosY - (par1EntityDragon.PrevPosY - par1EntityDragon.PosY) * (double)(1.0F - par9));
				float f4 = (float)(par1EntityDragon.HealingEnderCrystal.PosZ - par1EntityDragon.PosZ - (par1EntityDragon.PrevPosZ - par1EntityDragon.PosZ) * (double)(1.0F - par9));
				float f5 = MathHelper2.Sqrt_float(f2 * f2 + f4 * f4);
				float f6 = MathHelper2.Sqrt_float(f2 * f2 + f3 * f3 + f4 * f4);
				//GL.PushMatrix();
				//GL.Translate((float)par2, (float)par4 + 2.0F, (float)par6);
				//GL.Rotate(((float)(-Math.Atan2(f4, f2)) * 180F) / (float)Math.PI - 90F, 0.0F, 1.0F, 0.0F);
				//GL.Rotate(((float)(-Math.Atan2(f5, f3)) * 180F) / (float)Math.PI - 90F, 1.0F, 0.0F, 0.0F);
				Tessellator tessellator = Tessellator.Instance;
				RenderHelper.DisableStandardItemLighting();
                //GL.Disable(EnableCap.CullFace);
				LoadTexture("/mob/enderdragon/beam.png");
				//GL.ShadeModel(ShadingModel.Smooth);
				float f7 = 0.0F - ((float)par1EntityDragon.TicksExisted + par9) * 0.01F;
				float f8 = MathHelper2.Sqrt_float(f2 * f2 + f3 * f3 + f4 * f4) / 32F - ((float)par1EntityDragon.TicksExisted + par9) * 0.01F;
				tessellator.StartDrawing(5);
				int i = 8;

				for (int j = 0; j <= i; j++)
				{
					float f9 = MathHelper2.Sin(((float)(j % i) * (float)Math.PI * 2.0F) / (float)i) * 0.75F;
					float f10 = MathHelper2.Cos(((float)(j % i) * (float)Math.PI * 2.0F) / (float)i) * 0.75F;
					float f11 = ((float)(j % i) * 1.0F) / (float)i;
					tessellator.SetColorOpaque_I(0);
					tessellator.AddVertexWithUV(f9 * 0.2F, f10 * 0.2F, 0.0F, f11, f8);
					tessellator.SetColorOpaque_I(0xffffff);
					tessellator.AddVertexWithUV(f9, f10, f6, f11, f7);
				}

				tessellator.Draw();
                //GL.Enable(EnableCap.CullFace);
				//GL.ShadeModel(ShadingModel.Flat);
				RenderHelper.EnableStandardItemLighting();
				//GL.PopMatrix();
			}
		}

		/// <summary>
		/// Renders the animation for when an enderdragon dies
		/// </summary>
		protected virtual void RenderDragonDying(EntityDragon par1EntityDragon, float par2)
		{
			base.RenderEquippedItems(par1EntityDragon, par2);
			Tessellator tessellator = Tessellator.Instance;

			if (par1EntityDragon.Field_40178_aA > 0)
			{
				RenderHelper.DisableStandardItemLighting();
				float f = ((float)par1EntityDragon.Field_40178_aA + par2) / 200F;
				float f1 = 0.0F;

				if (f > 0.8F)
				{
					f1 = (f - 0.8F) / 0.2F;
				}

				Random random = new Random(432);
				//GL.Disable(EnableCap.Texture2D);
				//GL.ShadeModel(ShadingModel.Smooth);
                //GL.Enable(EnableCap.Blend);
				//GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.One);
                //GL.Disable(EnableCap.AlphaTest);
                //GL.Enable(EnableCap.CullFace);
				//GL.DepthMask(false);
				//GL.PushMatrix();
				//GL.Translate(0.0F, -1F, -2F);

				for (int i = 0; (float)i < ((f + f * f) / 2.0F) * 60F; i++)
				{
					//GL.Rotate(random.NextDouble() * 360F, 1.0F, 0.0F, 0.0F);
                    //GL.Rotate(random.NextDouble() * 360F, 0.0F, 1.0F, 0.0F);
                    //GL.Rotate(random.NextDouble() * 360F, 0.0F, 0.0F, 1.0F);
                    //GL.Rotate(random.NextDouble() * 360F, 1.0F, 0.0F, 0.0F);
                    //GL.Rotate(random.NextDouble() * 360F, 0.0F, 1.0F, 0.0F);
                    //GL.Rotate(random.NextDouble() * 360F + f * 90F, 0.0F, 0.0F, 1.0F);
					tessellator.StartDrawing(6);
                    float f2 = (float)random.NextDouble() * 20F + 5F + f1 * 10F;
                    float f3 = (float)random.NextDouble() * 2.0F + 1.0F + f1 * 2.0F;
					tessellator.SetColorRGBA_I(0xffffff, (int)(255F * (1.0F - f1)));
					tessellator.AddVertex(0.0F, 0.0F, 0.0F);
					tessellator.SetColorRGBA_I(0xff00ff, 0);
					tessellator.AddVertex(-0.86599999999999999D * (double)f3, f2, -0.5F * f3);
					tessellator.AddVertex(0.86599999999999999D * (double)f3, f2, -0.5F * f3);
					tessellator.AddVertex(0.0F, f2, 1.0F * f3);
					tessellator.AddVertex(-0.86599999999999999D * (double)f3, f2, -0.5F * f3);
					tessellator.Draw();
				}

				//GL.PopMatrix();
				//GL.DepthMask(true);
                //GL.Disable(EnableCap.CullFace);
                //GL.Disable(EnableCap.Blend);
				//GL.ShadeModel(ShadingModel.Flat);
				//GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
				//GL.Enable(EnableCap.Texture2D);
                //GL.Enable(EnableCap.AlphaTest);
				RenderHelper.EnableStandardItemLighting();
			}
		}

		protected virtual int Func_40283_a(EntityDragon par1EntityDragon, int par2, float par3)
		{
			if (par2 == 1)
			{
				//GL.DepthFunc(DepthFunction.Lequal);
			}

			if (par2 != 0)
			{
				return -1;
			}
			else
			{
				LoadTexture("/mob/enderdragon/ender_eyes.png");
				float f = 1.0F;
                //GL.Enable(EnableCap.Blend);
                //GL.Disable(EnableCap.AlphaTest);
				//GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.One);
                //GL.Disable(EnableCap.Lighting);
				//GL.DepthFunc(DepthFunction.Equal);
				int i = 61680;
				int j = i % 0x10000;
				int k = i / 0x10000;
				OpenGlHelper.SetLightmapTextureCoords(OpenGlHelper.LightmapTexUnit, (float)j / 1.0F, (float)k / 1.0F);
				//GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
                //GL.Enable(EnableCap.Lighting);
				//GL.Color4(1.0F, 1.0F, 1.0F, f);
				return 1;
			}
		}

		/// <summary>
		/// Queries whether should render the specified pass or not.
		/// </summary>
		protected override int ShouldRenderPass(EntityLiving par1EntityLiving, int par2, float par3)
		{
			return Func_40283_a((EntityDragon)par1EntityLiving, par2, par3);
		}

		protected override void RenderEquippedItems(EntityLiving par1EntityLiving, float par2)
		{
			RenderDragonDying((EntityDragon)par1EntityLiving, par2);
		}

		protected override void RotateCorpse(EntityLiving par1EntityLiving, float par2, float par3, float par4)
		{
			RotateDragonBody((EntityDragon)par1EntityLiving, par2, par3, par4);
		}

		/// <summary>
		/// Renders the model in RenderLiving
		/// </summary>
		protected override void RenderModel(EntityLiving par1EntityLiving, float par2, float par3, float par4, float par5, float par6, float par7)
		{
			Func_40280_a((EntityDragon)par1EntityLiving, par2, par3, par4, par5, par6, par7);
		}

		public override void DoRenderLiving(EntityLiving par1EntityLiving, double par2, double par4, double par6, float par8, float par9)
		{
			DoRenderDragon((EntityDragon)par1EntityLiving, par2, par4, par6, par8, par9);
		}

		/// <summary>
		/// Actually renders the given argument. This is a synthetic bridge method, always casting down its argument and then
		/// handing it off to a worker function which does the actual work. In all probabilty, the class Render is generic
		/// (Render<T extends Entity) and this method has signature public void doRender(T entity, double d, double d1,
		/// double d2, float f, float f1). But JAD is pre 1.5 so doesn't do that.
		/// </summary>
		public override void DoRender(Entity par1Entity, double par2, double par4, double par6, float par8, float par9)
		{
			DoRenderDragon((EntityDragon)par1Entity, par2, par4, par6, par8, par9);
		}
	}
}