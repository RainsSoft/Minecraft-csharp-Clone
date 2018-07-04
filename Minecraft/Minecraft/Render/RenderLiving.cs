using System;
using Microsoft.Xna.Framework;

namespace net.minecraft.src
{
	public class RenderLiving : Render
	{
		protected ModelBase MainModel;

		/// <summary>
		/// The model to be used during the render passes. </summary>
		protected ModelBase RenderPassModel;

		public RenderLiving(ModelBase par1ModelBase, float par2)
		{
			MainModel = par1ModelBase;
			ShadowSize = par2;
		}

		/// <summary>
		/// Sets the model to be used in the current render pass (the first render pass is done after the primary model is
		/// rendered) Args: model
		/// </summary>
		public virtual void SetRenderPassModel(ModelBase par1ModelBase)
		{
			RenderPassModel = par1ModelBase;
		}

		private float Func_48418_a(float par1, float par2, float par3)
		{
			float f;

			for (f = par2 - par1; f < -180F; f += 360F)
			{
			}

			for (; f >= 180F; f -= 360F)
			{
			}

			return par1 + par3 * f;
		}

		public virtual void DoRenderLiving(EntityLiving par1EntityLiving, double par2, double par4, double par6, float par8, float par9)
		{
			//GL.PushMatrix();
			//GL.Disable(EnableCap.CullFace);
			MainModel.OnGround = RenderSwingProgress(par1EntityLiving, par9);

			if (RenderPassModel != null)
			{
				RenderPassModel.OnGround = MainModel.OnGround;
			}

			MainModel.IsRiding = par1EntityLiving.IsRiding();

			if (RenderPassModel != null)
			{
				RenderPassModel.IsRiding = MainModel.IsRiding;
			}

			MainModel.IsChild = par1EntityLiving.IsChild();

			if (RenderPassModel != null)
			{
				RenderPassModel.IsChild = MainModel.IsChild;
			}

			try
			{
				float f = Func_48418_a(par1EntityLiving.PrevRenderYawOffset, par1EntityLiving.RenderYawOffset, par9);
				float f1 = Func_48418_a(par1EntityLiving.PrevRotationYawHead, par1EntityLiving.RotationYawHead, par9);
				float f2 = par1EntityLiving.PrevRotationPitch + (par1EntityLiving.RotationPitch - par1EntityLiving.PrevRotationPitch) * par9;
				RenderLivingAt(par1EntityLiving, par2, par4, par6);
				float f3 = HandleRotationFloat(par1EntityLiving, par9);
				RotateCorpse(par1EntityLiving, f3, f, par9);
				float f4 = 0.0625F;
				//GL.Enable(EnableCap.RescaleNormal);
				//GL.Scale(-1F, -1F, 1.0F);
				PreRenderCallback(par1EntityLiving, par9);
				//GL.Translate(0.0F, -24F * f4 - 0.0078125F, 0.0F);
				float f5 = par1EntityLiving.Field_705_Q + (par1EntityLiving.Field_704_R - par1EntityLiving.Field_705_Q) * par9;
				float f6 = par1EntityLiving.Field_703_S - par1EntityLiving.Field_704_R * (1.0F - par9);

				if (par1EntityLiving.IsChild())
				{
					f6 *= 3F;
				}

				if (f5 > 1.0F)
				{
					f5 = 1.0F;
				}

				//GL.Enable(EnableCap.AlphaTest);
				MainModel.SetLivingAnimations(par1EntityLiving, f6, f5, par9);
				RenderModel(par1EntityLiving, f6, f5, f3, f1 - f, f2, f4);

				for (int i = 0; i < 4; i++)
				{
					int j = ShouldRenderPass(par1EntityLiving, i, par9);

					if (j <= 0)
					{
						continue;
					}

					RenderPassModel.SetLivingAnimations(par1EntityLiving, f6, f5, par9);
					RenderPassModel.Render(par1EntityLiving, f6, f5, f3, f1 - f, f2, f4);

					if (j == 15)
					{
						float f8 = (float)par1EntityLiving.TicksExisted + par9;
						LoadTexture("%blur%/misc/glint.png");
						//GL.Enable(EnableCap.Blend);
						float f10 = 0.5F;
						//GL.Color4(f10, f10, f10, 1.0F);
						//GL.DepthFunc(DepthFunction.Equal);
						//GL.DepthMask(false);

						for (int i1 = 0; i1 < 2; i1++)
						{
							//GL.Disable(EnableCap.Lighting);
							float f13 = 0.76F;
							//GL.Color4(0.5F * f13, 0.25F * f13, 0.8F * f13, 1.0F);
							//GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.One);
							//GL.MatrixMode(MatrixMode.Texture);
							//GL.LoadIdentity();
							float f15 = f8 * (0.001F + (float)i1 * 0.003F) * 20F;
							float f16 = 0.3333333F;
							//GL.Scale(f16, f16, f16);
							//GL.Rotate(30F - (float)i1 * 60F, 0.0F, 0.0F, 1.0F);
							//GL.Translate(0.0F, f15, 0.0F);
							//GL.MatrixMode(MatrixMode.Modelview);
							RenderPassModel.Render(par1EntityLiving, f6, f5, f3, f1 - f, f2, f4);
						}

						//GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
						//GL.MatrixMode(MatrixMode.Texture);
						//GL.DepthMask(true);
						//GL.LoadIdentity();
						//GL.MatrixMode(MatrixMode.Modelview);
						//GL.Enable(EnableCap.Lighting);
						//GL.Disable(EnableCap.Blend);
						//GL.DepthFunc(DepthFunction.Lequal);
					}

					//GL.Disable(EnableCap.Blend);
					//GL.Enable(EnableCap.AlphaTest);
				}

				RenderEquippedItems(par1EntityLiving, par9);
				float f7 = par1EntityLiving.GetBrightness(par9);
				int k = GetColorMultiplier(par1EntityLiving, f7, par9);
				OpenGlHelper.SetActiveTexture(OpenGlHelper.LightmapTexUnit);
				//GL.Disable(EnableCap.Texture2D);
				OpenGlHelper.SetActiveTexture(OpenGlHelper.DefaultTexUnit);

				if ((k >> 24 & 0xff) > 0 || par1EntityLiving.HurtTime > 0 || par1EntityLiving.DeathTime > 0)
				{
					//GL.Disable(EnableCap.Texture2D);
					//GL.Disable(EnableCap.AlphaTest);
					//GL.Enable(EnableCap.Blend);
					//GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
					//GL.DepthFunc(DepthFunction.Equal);

					if (par1EntityLiving.HurtTime > 0 || par1EntityLiving.DeathTime > 0)
					{
						//GL.Color4(f7, 0.0F, 0.0F, 0.4F);
						MainModel.Render(par1EntityLiving, f6, f5, f3, f1 - f, f2, f4);

						for (int l = 0; l < 4; l++)
						{
							if (InheritRenderPass(par1EntityLiving, l, par9) >= 0)
							{
								//GL.Color4(f7, 0.0F, 0.0F, 0.4F);
								RenderPassModel.Render(par1EntityLiving, f6, f5, f3, f1 - f, f2, f4);
							}
						}
					}

					if ((k >> 24 & 0xff) > 0)
					{
						float f9 = (float)(k >> 16 & 0xff) / 255F;
						float f11 = (float)(k >> 8 & 0xff) / 255F;
						float f12 = (float)(k & 0xff) / 255F;
						float f14 = (float)(k >> 24 & 0xff) / 255F;
						//GL.Color4(f9, f11, f12, f14);
						MainModel.Render(par1EntityLiving, f6, f5, f3, f1 - f, f2, f4);

						for (int j1 = 0; j1 < 4; j1++)
						{
							if (InheritRenderPass(par1EntityLiving, j1, par9) >= 0)
							{
								//GL.Color4(f9, f11, f12, f14);
								RenderPassModel.Render(par1EntityLiving, f6, f5, f3, f1 - f, f2, f4);
							}
						}
					}

					//GL.DepthFunc(DepthFunction.Lequal);
					//GL.Disable(EnableCap.Blend);
					//GL.Enable(EnableCap.AlphaTest);
					//GL.Enable(EnableCap.Texture2D);
				}

				//GL.Disable(EnableCap.RescaleNormal);
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception.ToString());
				Console.Write(exception.StackTrace);
			}

			OpenGlHelper.SetActiveTexture(OpenGlHelper.LightmapTexUnit);
			//GL.Enable(EnableCap.Texture2D);
			OpenGlHelper.SetActiveTexture(OpenGlHelper.DefaultTexUnit);
			//GL.Enable(EnableCap.CullFace);
			//GL.PopMatrix();
			PassSpecialRender(par1EntityLiving, par2, par4, par6);
		}

		/// <summary>
		/// Renders the model in RenderLiving
		/// </summary>
		protected virtual void RenderModel(EntityLiving par1EntityLiving, float par2, float par3, float par4, float par5, float par6, float par7)
		{
			LoadDownloadableImageTexture(par1EntityLiving.SkinUrl, par1EntityLiving.GetTexture());
			MainModel.Render(par1EntityLiving, par2, par3, par4, par5, par6, par7);
		}

		/// <summary>
		/// Sets a simple glTranslate on a LivingEntity.
		/// </summary>
		protected virtual void RenderLivingAt(EntityLiving par1EntityLiving, double par2, double par4, double par6)
		{
			//GL.Translate((float)par2, (float)par4, (float)par6);
		}

		protected virtual void RotateCorpse(EntityLiving par1EntityLiving, float par2, float par3, float par4)
		{
			//GL.Rotate(180F - par3, 0.0F, 1.0F, 0.0F);

			if (par1EntityLiving.DeathTime > 0)
			{
				float f = ((((float)par1EntityLiving.DeathTime + par4) - 1.0F) / 20F) * 1.6F;
				f = MathHelper2.Sqrt_float(f);

				if (f > 1.0F)
				{
					f = 1.0F;
				}

				//GL.Rotate(f * GetDeathMaxRotation(par1EntityLiving), 0.0F, 0.0F, 1.0F);
			}
		}

		protected virtual float RenderSwingProgress(EntityLiving par1EntityLiving, float par2)
		{
			return par1EntityLiving.GetSwingProgress(par2);
		}

		/// <summary>
		/// Defines what float the third param in setRotationAngles of ModelBase is
		/// </summary>
		protected virtual float HandleRotationFloat(EntityLiving par1EntityLiving, float par2)
		{
			return (float)par1EntityLiving.TicksExisted + par2;
		}

		protected virtual void RenderEquippedItems(EntityLiving entityliving, float f)
		{
		}

		protected virtual int InheritRenderPass(EntityLiving par1EntityLiving, int par2, float par3)
		{
			return ShouldRenderPass(par1EntityLiving, par2, par3);
		}

		/// <summary>
		/// Queries whether should render the specified pass or not.
		/// </summary>
		protected virtual int ShouldRenderPass(EntityLiving par1EntityLiving, int par2, float par3)
		{
			return -1;
		}

		protected virtual float GetDeathMaxRotation(EntityLiving par1EntityLiving)
		{
			return 90F;
		}

		/// <summary>
		/// Returns an ARGB int color back. Args: entityLiving, lightBrightness, partialTickTime
		/// </summary>
		protected virtual int GetColorMultiplier(EntityLiving par1EntityLiving, float par2, float par3)
		{
			return 0;
		}

		/// <summary>
		/// Allows the render to do any OpenGL state modifications necessary before the model is rendered. Args:
		/// entityLiving, partialTickTime
		/// </summary>
		protected virtual void PreRenderCallback(EntityLiving entityliving, float f)
		{
		}

		/// <summary>
		/// Passes the specialRender and renders it
		/// </summary>
		protected virtual void PassSpecialRender(EntityLiving par1EntityLiving, double par2, double par4, double par6)
		{
			if (!Minecraft.IsDebugInfoEnabled())
			{
				;
			}
		}

		/// <summary>
		/// Draws the debug or playername text above a living
		/// </summary>
		protected virtual void RenderLivingLabel(EntityLiving par1EntityLiving, string par2Str, double par3, double par5, double par7, int par9)
		{
			float f = par1EntityLiving.GetDistanceToEntity(RenderManager.LivingPlayer);

			if (f > (float)par9)
			{
				return;
			}

			FontRenderer fontrenderer = GetFontRendererFromRenderManager();
			float f1 = 1.6F;
			float f2 = 0.01666667F * f1;
			//GL.PushMatrix();
			//GL.Translate((float)par3 + 0.0F, (float)par5 + 2.3F, (float)par7);
			//GL.Normal3(0.0F, 1.0F, 0.0F);
			//GL.Rotate(-RenderManager.PlayerViewY, 0.0F, 1.0F, 0.0F);
			//GL.Rotate(RenderManager.PlayerViewX, 1.0F, 0.0F, 0.0F);
			//GL.Scale(-f2, -f2, f2);
			//GL.Disable(EnableCap.Lighting);
			//GL.DepthMask(false);
			//GL.Disable(EnableCap.DepthTest);
			//GL.Enable(EnableCap.Blend);
			//GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			Tessellator tessellator = Tessellator.Instance;
			sbyte byte0 = 0;

			if (par2Str.Equals("deadmau5"))
			{
				byte0 = -10;
			}

			//GL.Disable(EnableCap.Texture2D);
			tessellator.StartDrawingQuads();
			int i = fontrenderer.GetStringWidth(par2Str) / 2;
			tessellator.SetColorRGBA_F(0.0F, 0.0F, 0.0F, 0.25F);
			tessellator.AddVertex(-i - 1, -1 + byte0, 0.0F);
			tessellator.AddVertex(-i - 1, 8 + byte0, 0.0F);
			tessellator.AddVertex(i + 1, 8 + byte0, 0.0F);
			tessellator.AddVertex(i + 1, -1 + byte0, 0.0F);
			tessellator.Draw();
			//GL.Enable(EnableCap.Texture2D);
			fontrenderer.DrawString(par2Str, -fontrenderer.GetStringWidth(par2Str) / 2, byte0, 0x20ffffff);
			//GL.Enable(EnableCap.DepthTest);
			//GL.DepthMask(true);
			fontrenderer.DrawString(par2Str, -fontrenderer.GetStringWidth(par2Str) / 2, byte0, -1);
			//GL.Enable(EnableCap.Lighting);
			//GL.Disable(EnableCap.Blend);
			//GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
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
			DoRenderLiving((EntityLiving)par1Entity, par2, par4, par6, par8, par9);
		}
	}
}