using Microsoft.Xna.Framework;

namespace net.minecraft.src
{
	public abstract class Render
	{
		protected RenderManager RenderManager;
		private ModelBase ModelBase;
		protected RenderBlocks RenderBlocks;
		protected float ShadowSize;

		/// <summary>
		/// Determines the darkness of the object's shadow. Higher value makes a darker shadow.
		/// </summary>
		protected float ShadowOpaque;

		public Render()
		{
			ModelBase = new ModelBiped();
			RenderBlocks = new RenderBlocks();
			ShadowSize = 0.0F;
			ShadowOpaque = 1.0F;
		}

		/// <summary>
		/// Actually renders the given argument. This is a synthetic bridge method, always casting down its argument and then
		/// handing it off to a worker function which does the actual work. In all probabilty, the class Render is generic
		/// (Render<T extends Entity) and this method has signature public void doRender(T entity, double d, double d1,
		/// double d2, float f, float f1). But JAD is pre 1.5 so doesn't do that.
		/// </summary>
		public abstract void DoRender(Entity entity, double d, double d1, double d2, float f, float f1);

		/// <summary>
		/// loads the specified texture
		/// </summary>
		protected virtual void LoadTexture(string par1Str)
		{
			RenderEngine renderengine = RenderManager.RenderEngine;
			renderengine.BindTexture(par1Str);
		}

		/// <summary>
		/// loads the specified downloadable texture or alternative built in texture
		/// </summary>
		protected virtual bool LoadDownloadableImageTexture(string par1Str, string par2Str)
		{
			RenderEngine renderengine = RenderManager.RenderEngine;
			string i = renderengine.GetTextureForDownloadableImage(par1Str, par2Str);

			if (i == "")
			{
				renderengine.BindTexture(i);
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Renders fire on top of the entity. Args: entity, x, y, z, partialTickTime
		/// </summary>
		private void RenderEntityOnFire(Entity par1Entity, double par2, double par4, double par6, float par8)
		{
			//GL.Disable(EnableCap.Lighting);
			int i = Block.Fire.BlockIndexInTexture;
			int j = (i & 0xf) << 4;
			int k = i & 0xf0;
			float f = (float)j / 256F;
			float f2 = ((float)j + 15.99F) / 256F;
			float f4 = (float)k / 256F;
			float f6 = ((float)k + 15.99F) / 256F;
			//GL.PushMatrix();
			//GL.Translate((float)par2, (float)par4, (float)par6);
			float f8 = par1Entity.Width * 1.4F;
			//GL.Scale(f8, f8, f8);
			LoadTexture("terrain.png");
			Tessellator tessellator = Tessellator.Instance;
			float f9 = 0.5F;
			float f10 = 0.0F;
			float f11 = par1Entity.Height / f8;
			float f12 = (float)(par1Entity.PosY - par1Entity.BoundingBox.MinY);
			//GL.Rotate(-RenderManager.PlayerViewY, 0.0F, 1.0F, 0.0F);
			//GL.Translate(0.0F, 0.0F, -0.3F + (float)(int)f11 * 0.02F);
			//GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
			float f13 = 0.0F;
			int l = 0;
			tessellator.StartDrawingQuads();

			while (f11 > 0.0F)
			{
				float f1;
				float f3;
				float f5;
				float f7;

				if (l % 2 == 0)
				{
					f1 = (float)j / 256F;
					f3 = ((float)j + 15.99F) / 256F;
					f5 = (float)k / 256F;
					f7 = ((float)k + 15.99F) / 256F;
				}
				else
				{
					f1 = (float)j / 256F;
					f3 = ((float)j + 15.99F) / 256F;
					f5 = (float)(k + 16) / 256F;
					f7 = ((float)(k + 16) + 15.99F) / 256F;
				}

				if ((l / 2) % 2 == 0)
				{
					float f14 = f3;
					f3 = f1;
					f1 = f14;
				}

				tessellator.AddVertexWithUV(f9 - f10, 0.0F - f12, f13, f3, f7);
				tessellator.AddVertexWithUV(-f9 - f10, 0.0F - f12, f13, f1, f7);
				tessellator.AddVertexWithUV(-f9 - f10, 1.4F - f12, f13, f1, f5);
				tessellator.AddVertexWithUV(f9 - f10, 1.4F - f12, f13, f3, f5);
				f11 -= 0.45F;
				f12 -= 0.45F;
				f9 *= 0.9F;
				f13 += 0.03F;
				l++;
			}

			tessellator.Draw();
			//GL.PopMatrix();
			//GL.Enable(EnableCap.Lighting);
		}

		/// <summary>
		/// Renders the entity shadows at the position, shadow alpha and partialTickTime. Args: entity, x, y, z, shadowAlpha,
		/// partialTickTime
		/// </summary>
		private void RenderShadow(Entity par1Entity, double par2, double par4, double par6, float par8, float par9)
		{
			//GL.Enable(EnableCap.Blend);
			//GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			RenderEngine renderengine = RenderManager.RenderEngine;
			renderengine.BindTexture("misc.shadow.png", TextureMode.Clamp);
			World world = GetWorldFromRenderManager();
			//GL.DepthMask(false);
			float f = ShadowSize;

			if (par1Entity is EntityLiving)
			{
				EntityLiving entityliving = (EntityLiving)par1Entity;
				f *= entityliving.GetRenderSizeModifier();

				if (entityliving is EntityAnimal)
				{
					EntityAnimal entityanimal = (EntityAnimal)entityliving;

					if (entityanimal.IsChild())
					{
						f *= 0.5F;
					}
				}
			}

			double d = par1Entity.LastTickPosX + (par1Entity.PosX - par1Entity.LastTickPosX) * (double)par9;
			double d1 = par1Entity.LastTickPosY + (par1Entity.PosY - par1Entity.LastTickPosY) * (double)par9 + (double)par1Entity.GetShadowSize();
			double d2 = par1Entity.LastTickPosZ + (par1Entity.PosZ - par1Entity.LastTickPosZ) * (double)par9;
			int i = MathHelper2.Floor_double(d - (double)f);
			int j = MathHelper2.Floor_double(d + (double)f);
			int k = MathHelper2.Floor_double(d1 - (double)f);
			int l = MathHelper2.Floor_double(d1);
			int i1 = MathHelper2.Floor_double(d2 - (double)f);
			int j1 = MathHelper2.Floor_double(d2 + (double)f);
			double d3 = par2 - d;
			double d4 = par4 - d1;
			double d5 = par6 - d2;
			Tessellator tessellator = Tessellator.Instance;
			tessellator.StartDrawingQuads();

			for (int k1 = i; k1 <= j; k1++)
			{
				for (int l1 = k; l1 <= l; l1++)
				{
					for (int i2 = i1; i2 <= j1; i2++)
					{
						int j2 = world.GetBlockId(k1, l1 - 1, i2);

						if (j2 > 0 && world.GetBlockLightValue(k1, l1, i2) > 3)
						{
							RenderShadowOnBlock(Block.BlocksList[j2], par2, par4 + (double)par1Entity.GetShadowSize(), par6, k1, l1, i2, par8, f, d3, d4 + (double)par1Entity.GetShadowSize(), d5);
						}
					}
				}
			}

			tessellator.Draw();
			//GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
			//GL.Disable(EnableCap.Blend);
			//GL.DepthMask(true);
		}

		/// <summary>
		/// Returns the render manager's world object
		/// </summary>
		private World GetWorldFromRenderManager()
		{
			return RenderManager.WorldObj;
		}

		/// <summary>
		/// Renders a shadow projected down onto the specified block. Brightness of the block plus how far away on the Y axis
		/// determines the alpha of the shadow.  Args: block, centerX, centerY, centerZ, blockX, blockY, blockZ, baseAlpha,
		/// shadowSize, xOffset, yOffset, zOffset
		/// </summary>
		private void RenderShadowOnBlock(Block par1Block, double par2, double par4, double par6, int par8, int par9, int par10, float par11, float par12, double par13, double par15, double par17)
		{
			Tessellator tessellator = Tessellator.Instance;

			if (!par1Block.RenderAsNormalBlock())
			{
				return;
			}

			double d = ((double)par11 - (par4 - ((double)par9 + par15)) / 2D) * 0.5D * (double)GetWorldFromRenderManager().GetLightBrightness(par8, par9, par10);

			if (d < 0.0F)
			{
				return;
			}

			if (d > 1.0D)
			{
				d = 1.0D;
			}

			tessellator.SetColorRGBA_F(1.0F, 1.0F, 1.0F, (float)d);
			double d1 = (double)par8 + par1Block.MinX + par13;
			double d2 = (double)par8 + par1Block.MaxX + par13;
			double d3 = (double)par9 + par1Block.MinY + par15 + 0.015625D;
			double d4 = (double)par10 + par1Block.MinZ + par17;
			double d5 = (double)par10 + par1Block.MaxZ + par17;
			float f = (float)((par2 - d1) / 2D / (double)par12 + 0.5D);
			float f1 = (float)((par2 - d2) / 2D / (double)par12 + 0.5D);
			float f2 = (float)((par6 - d4) / 2D / (double)par12 + 0.5D);
			float f3 = (float)((par6 - d5) / 2D / (double)par12 + 0.5D);
			tessellator.AddVertexWithUV(d1, d3, d4, f, f2);
			tessellator.AddVertexWithUV(d1, d3, d5, f, f3);
			tessellator.AddVertexWithUV(d2, d3, d5, f1, f3);
			tessellator.AddVertexWithUV(d2, d3, d4, f1, f2);
		}

		/// <summary>
		/// Renders a white box with the bounds of the AABB translated by the offset. Args: aabb, x, y, z
		/// </summary>
		public static void RenderOffsetAABB(AxisAlignedBB par0AxisAlignedBB, double par1, double par3, double par5)
		{
			//GL.Disable(EnableCap.Texture2D);
			Tessellator tessellator = Tessellator.Instance;
			//GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
			tessellator.StartDrawingQuads();
			tessellator.SetTranslation(par1, par3, par5);
			tessellator.SetNormal(0.0F, 0.0F, -1F);
			tessellator.AddVertex(par0AxisAlignedBB.MinX, par0AxisAlignedBB.MaxY, par0AxisAlignedBB.MinZ);
			tessellator.AddVertex(par0AxisAlignedBB.MaxX, par0AxisAlignedBB.MaxY, par0AxisAlignedBB.MinZ);
			tessellator.AddVertex(par0AxisAlignedBB.MaxX, par0AxisAlignedBB.MinY, par0AxisAlignedBB.MinZ);
			tessellator.AddVertex(par0AxisAlignedBB.MinX, par0AxisAlignedBB.MinY, par0AxisAlignedBB.MinZ);
			tessellator.SetNormal(0.0F, 0.0F, 1.0F);
			tessellator.AddVertex(par0AxisAlignedBB.MinX, par0AxisAlignedBB.MinY, par0AxisAlignedBB.MaxZ);
			tessellator.AddVertex(par0AxisAlignedBB.MaxX, par0AxisAlignedBB.MinY, par0AxisAlignedBB.MaxZ);
			tessellator.AddVertex(par0AxisAlignedBB.MaxX, par0AxisAlignedBB.MaxY, par0AxisAlignedBB.MaxZ);
			tessellator.AddVertex(par0AxisAlignedBB.MinX, par0AxisAlignedBB.MaxY, par0AxisAlignedBB.MaxZ);
			tessellator.SetNormal(0.0F, -1F, 0.0F);
			tessellator.AddVertex(par0AxisAlignedBB.MinX, par0AxisAlignedBB.MinY, par0AxisAlignedBB.MinZ);
			tessellator.AddVertex(par0AxisAlignedBB.MaxX, par0AxisAlignedBB.MinY, par0AxisAlignedBB.MinZ);
			tessellator.AddVertex(par0AxisAlignedBB.MaxX, par0AxisAlignedBB.MinY, par0AxisAlignedBB.MaxZ);
			tessellator.AddVertex(par0AxisAlignedBB.MinX, par0AxisAlignedBB.MinY, par0AxisAlignedBB.MaxZ);
			tessellator.SetNormal(0.0F, 1.0F, 0.0F);
			tessellator.AddVertex(par0AxisAlignedBB.MinX, par0AxisAlignedBB.MaxY, par0AxisAlignedBB.MaxZ);
			tessellator.AddVertex(par0AxisAlignedBB.MaxX, par0AxisAlignedBB.MaxY, par0AxisAlignedBB.MaxZ);
			tessellator.AddVertex(par0AxisAlignedBB.MaxX, par0AxisAlignedBB.MaxY, par0AxisAlignedBB.MinZ);
			tessellator.AddVertex(par0AxisAlignedBB.MinX, par0AxisAlignedBB.MaxY, par0AxisAlignedBB.MinZ);
			tessellator.SetNormal(-1F, 0.0F, 0.0F);
			tessellator.AddVertex(par0AxisAlignedBB.MinX, par0AxisAlignedBB.MinY, par0AxisAlignedBB.MaxZ);
			tessellator.AddVertex(par0AxisAlignedBB.MinX, par0AxisAlignedBB.MaxY, par0AxisAlignedBB.MaxZ);
			tessellator.AddVertex(par0AxisAlignedBB.MinX, par0AxisAlignedBB.MaxY, par0AxisAlignedBB.MinZ);
			tessellator.AddVertex(par0AxisAlignedBB.MinX, par0AxisAlignedBB.MinY, par0AxisAlignedBB.MinZ);
			tessellator.SetNormal(1.0F, 0.0F, 0.0F);
			tessellator.AddVertex(par0AxisAlignedBB.MaxX, par0AxisAlignedBB.MinY, par0AxisAlignedBB.MinZ);
			tessellator.AddVertex(par0AxisAlignedBB.MaxX, par0AxisAlignedBB.MaxY, par0AxisAlignedBB.MinZ);
			tessellator.AddVertex(par0AxisAlignedBB.MaxX, par0AxisAlignedBB.MaxY, par0AxisAlignedBB.MaxZ);
			tessellator.AddVertex(par0AxisAlignedBB.MaxX, par0AxisAlignedBB.MinY, par0AxisAlignedBB.MaxZ);
			tessellator.SetTranslation(0.0F, 0.0F, 0.0F);
			tessellator.Draw();
			//GL.Enable(EnableCap.Texture2D);
		}

		/// <summary>
		/// Adds to the tesselator a box using the aabb for the bounds. Args: aabb
		/// </summary>
		public static void RenderAABB(AxisAlignedBB par0AxisAlignedBB)
		{
			Tessellator tessellator = Tessellator.Instance;
			tessellator.StartDrawingQuads();
			tessellator.AddVertex(par0AxisAlignedBB.MinX, par0AxisAlignedBB.MaxY, par0AxisAlignedBB.MinZ);
			tessellator.AddVertex(par0AxisAlignedBB.MaxX, par0AxisAlignedBB.MaxY, par0AxisAlignedBB.MinZ);
			tessellator.AddVertex(par0AxisAlignedBB.MaxX, par0AxisAlignedBB.MinY, par0AxisAlignedBB.MinZ);
			tessellator.AddVertex(par0AxisAlignedBB.MinX, par0AxisAlignedBB.MinY, par0AxisAlignedBB.MinZ);
			tessellator.AddVertex(par0AxisAlignedBB.MinX, par0AxisAlignedBB.MinY, par0AxisAlignedBB.MaxZ);
			tessellator.AddVertex(par0AxisAlignedBB.MaxX, par0AxisAlignedBB.MinY, par0AxisAlignedBB.MaxZ);
			tessellator.AddVertex(par0AxisAlignedBB.MaxX, par0AxisAlignedBB.MaxY, par0AxisAlignedBB.MaxZ);
			tessellator.AddVertex(par0AxisAlignedBB.MinX, par0AxisAlignedBB.MaxY, par0AxisAlignedBB.MaxZ);
			tessellator.AddVertex(par0AxisAlignedBB.MinX, par0AxisAlignedBB.MinY, par0AxisAlignedBB.MinZ);
			tessellator.AddVertex(par0AxisAlignedBB.MaxX, par0AxisAlignedBB.MinY, par0AxisAlignedBB.MinZ);
			tessellator.AddVertex(par0AxisAlignedBB.MaxX, par0AxisAlignedBB.MinY, par0AxisAlignedBB.MaxZ);
			tessellator.AddVertex(par0AxisAlignedBB.MinX, par0AxisAlignedBB.MinY, par0AxisAlignedBB.MaxZ);
			tessellator.AddVertex(par0AxisAlignedBB.MinX, par0AxisAlignedBB.MaxY, par0AxisAlignedBB.MaxZ);
			tessellator.AddVertex(par0AxisAlignedBB.MaxX, par0AxisAlignedBB.MaxY, par0AxisAlignedBB.MaxZ);
			tessellator.AddVertex(par0AxisAlignedBB.MaxX, par0AxisAlignedBB.MaxY, par0AxisAlignedBB.MinZ);
			tessellator.AddVertex(par0AxisAlignedBB.MinX, par0AxisAlignedBB.MaxY, par0AxisAlignedBB.MinZ);
			tessellator.AddVertex(par0AxisAlignedBB.MinX, par0AxisAlignedBB.MinY, par0AxisAlignedBB.MaxZ);
			tessellator.AddVertex(par0AxisAlignedBB.MinX, par0AxisAlignedBB.MaxY, par0AxisAlignedBB.MaxZ);
			tessellator.AddVertex(par0AxisAlignedBB.MinX, par0AxisAlignedBB.MaxY, par0AxisAlignedBB.MinZ);
			tessellator.AddVertex(par0AxisAlignedBB.MinX, par0AxisAlignedBB.MinY, par0AxisAlignedBB.MinZ);
			tessellator.AddVertex(par0AxisAlignedBB.MaxX, par0AxisAlignedBB.MinY, par0AxisAlignedBB.MinZ);
			tessellator.AddVertex(par0AxisAlignedBB.MaxX, par0AxisAlignedBB.MaxY, par0AxisAlignedBB.MinZ);
			tessellator.AddVertex(par0AxisAlignedBB.MaxX, par0AxisAlignedBB.MaxY, par0AxisAlignedBB.MaxZ);
			tessellator.AddVertex(par0AxisAlignedBB.MaxX, par0AxisAlignedBB.MinY, par0AxisAlignedBB.MaxZ);
			tessellator.Draw();
		}

		/// <summary>
		/// Sets the RenderManager.
		/// </summary>
		public virtual void SetRenderManager(RenderManager par1RenderManager)
		{
			RenderManager = par1RenderManager;
		}

		/// <summary>
		/// Renders the entity's shadow and fire (if its on fire). Args: entity, x, y, z, yaw, partialTickTime
		/// </summary>
		public virtual void DoRenderShadowAndFire(Entity par1Entity, double par2, double par4, double par6, float par8, float par9)
		{
			if (RenderManager.Options.FancyGraphics && ShadowSize > 0.0F)
			{
				double d = RenderManager.GetDistanceToCamera(par1Entity.PosX, par1Entity.PosY, par1Entity.PosZ);
				float f = (float)((1.0D - d / 256D) * (double)ShadowOpaque);

				if (f > 0.0F)
				{
					RenderShadow(par1Entity, par2, par4, par6, f, par9);
				}
			}

			if (par1Entity.IsBurning())
			{
				RenderEntityOnFire(par1Entity, par2, par4, par6, par9);
			}
		}

		/// <summary>
		/// Returns the font renderer from the set render manager
		/// </summary>
		public virtual FontRenderer GetFontRendererFromRenderManager()
		{
			return RenderManager.GetFontRenderer();
		}
	}
}