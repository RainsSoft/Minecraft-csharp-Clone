using System;
using Microsoft.Xna.Framework;

namespace net.minecraft.src
{
	public class ItemRenderer
	{
		/// <summary>
		/// A reference to the Minecraft object. </summary>
		private Minecraft Mc;
		private ItemStack ItemToRender;

		/// <summary>
		/// How far the current item has been equipped (0 disequipped and 1 fully up)
		/// </summary>
		private float EquippedProgress;
		private float PrevEquippedProgress;

		/// <summary>
		/// Instance of RenderBlocks. </summary>
		private RenderBlocks RenderBlocksInstance;
		private MapItemRenderer MapItemRenderer;

		/// <summary>
		/// The index of the currently held item (0-8, or -1 if not yet updated) </summary>
		private int EquippedItemSlot;

		public ItemRenderer(Minecraft par1Minecraft)
		{
			ItemToRender = null;
			EquippedProgress = 0.0F;
			PrevEquippedProgress = 0.0F;
			RenderBlocksInstance = new RenderBlocks();
			EquippedItemSlot = -1;
			Mc = par1Minecraft;
			MapItemRenderer = new MapItemRenderer(par1Minecraft.FontRendererOld, par1Minecraft.GameSettings, par1Minecraft.RenderEngineOld);
		}

		/// <summary>
		/// Renders the item stack for being in an entity's hand Args: itemStack
		/// </summary>
		public virtual void RenderItem(EntityLiving par1EntityLiving, ItemStack par2ItemStack, int par3)
		{
			//GL.PushMatrix();

			if (par2ItemStack.ItemID < 256 && RenderBlocks.RenderItemIn3d(Block.BlocksList[par2ItemStack.ItemID].GetRenderType()))
			{
				//GL.BindTexture(TextureTarget.Texture2D, Mc.RenderEngineOld.GetTexture("/terrain.png"));
				RenderBlocksInstance.RenderBlockAsItem(Block.BlocksList[par2ItemStack.ItemID], par2ItemStack.GetItemDamage(), 1.0F);
			}
			else
			{
				if (par2ItemStack.ItemID < 256)
				{
					//GL.BindTexture(TextureTarget.Texture2D, Mc.RenderEngineOld.GetTexture("/terrain.png"));
				}
				else
				{
					//GL.BindTexture(TextureTarget.Texture2D, Mc.RenderEngineOld.GetTexture("/gui/items.png"));
				}

				Tessellator tessellator = Tessellator.Instance;
				int i = par1EntityLiving.GetItemIcon(par2ItemStack, par3);
				float f = ((float)((i % 16) * 16) + 0.0F) / 256F;
				float f1 = ((float)((i % 16) * 16) + 15.99F) / 256F;
				float f2 = ((float)((i / 16) * 16) + 0.0F) / 256F;
				float f3 = ((float)((i / 16) * 16) + 15.99F) / 256F;
			//	float f4 = 0.0F;
			//	float f5 = 0.3F;
				//GL.Enable(EnableCap.RescaleNormal);
				//GL.Translate(-f4, -f5, 0.0F);
			//	float f6 = 1.5F;
				//GL.Scale(f6, f6, f6);
				//GL.Rotate(50F, 0.0F, 1.0F, 0.0F);
				//GL.Rotate(335F, 0.0F, 0.0F, 1.0F);
				//GL.Translate(-0.9375F, -0.0625F, 0.0F);
				RenderItemIn2D(tessellator, f1, f2, f, f3);

				if (par2ItemStack != null && par2ItemStack.HasEffect() && par3 == 0)
				{
					//GL.DepthFunc(DepthFunction.Equal);
					//GL.Disable(EnableCap.Lighting);
					Mc.RenderEngineOld.BindTexture(Mc.RenderEngineOld.GetTexture("%blur%/misc/glint.png"));
					//GL.Enable(EnableCap.Blend);
					//GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.One);
				//	float f7 = 0.76F;
					//GL.Color4(0.5F * f7, 0.25F * f7, 0.8F * f7, 1.0F);
					//GL.MatrixMode(MatrixMode.Texture);
					//GL.PushMatrix();
				//	float f8 = 0.125F;
					//GL.Scale(f8, f8, f8);
					float f9 = ((float)(JavaHelper.CurrentTimeMillis() % 3000L) / 3000F) * 8F;
					//GL.Translate(f9, 0.0F, 0.0F);
					//GL.Rotate(-50F, 0.0F, 0.0F, 1.0F);
					RenderItemIn2D(tessellator, 0.0F, 0.0F, 1.0F, 1.0F);
					//GL.PopMatrix();
					//GL.PushMatrix();
					//GL.Scale(f8, f8, f8);
					f9 = ((float)(JavaHelper.CurrentTimeMillis() % 4873L) / 4873F) * 8F;
					//GL.Translate(-f9, 0.0F, 0.0F);
					//GL.Rotate(10F, 0.0F, 0.0F, 1.0F);
					RenderItemIn2D(tessellator, 0.0F, 0.0F, 1.0F, 1.0F);
					//GL.PopMatrix();
					//GL.MatrixMode(MatrixMode.Modelview);
					//GL.Disable(EnableCap.Blend);
					//GL.Enable(EnableCap.Lighting);
					//GL.DepthFunc(DepthFunction.Lequal);
				}

				//GL.Disable(EnableCap.RescaleNormal);
			}

			//GL.PopMatrix();
		}

		/// <summary>
		/// Renders an item held in hand as a 2D texture with thickness
		/// </summary>
		private void RenderItemIn2D(Tessellator par1Tessellator, float par2, float par3, float par4, float par5)
		{
			float f = 1.0F;
			float f1 = 0.0625F;
			par1Tessellator.StartDrawingQuads();
			par1Tessellator.SetNormal(0.0F, 0.0F, 1.0F);
			par1Tessellator.AddVertexWithUV(0.0F, 0.0F, 0.0F, par2, par5);
			par1Tessellator.AddVertexWithUV(f, 0.0F, 0.0F, par4, par5);
			par1Tessellator.AddVertexWithUV(f, 1.0D, 0.0F, par4, par3);
			par1Tessellator.AddVertexWithUV(0.0F, 1.0D, 0.0F, par2, par3);
			par1Tessellator.Draw();
			par1Tessellator.StartDrawingQuads();
			par1Tessellator.SetNormal(0.0F, 0.0F, -1F);
			par1Tessellator.AddVertexWithUV(0.0F, 1.0D, 0.0F - f1, par2, par3);
			par1Tessellator.AddVertexWithUV(f, 1.0D, 0.0F - f1, par4, par3);
			par1Tessellator.AddVertexWithUV(f, 0.0F, 0.0F - f1, par4, par5);
			par1Tessellator.AddVertexWithUV(0.0F, 0.0F, 0.0F - f1, par2, par5);
			par1Tessellator.Draw();
			par1Tessellator.StartDrawingQuads();
			par1Tessellator.SetNormal(-1F, 0.0F, 0.0F);

			for (int i = 0; i < 16; i++)
			{
				float f2 = (float)i / 16F;
				float f6 = (par2 + (par4 - par2) * f2) - 0.001953125F;
				float f10 = f * f2;
				par1Tessellator.AddVertexWithUV(f10, 0.0F, 0.0F - f1, f6, par5);
				par1Tessellator.AddVertexWithUV(f10, 0.0F, 0.0F, f6, par5);
				par1Tessellator.AddVertexWithUV(f10, 1.0D, 0.0F, f6, par3);
				par1Tessellator.AddVertexWithUV(f10, 1.0D, 0.0F - f1, f6, par3);
			}

			par1Tessellator.Draw();
			par1Tessellator.StartDrawingQuads();
			par1Tessellator.SetNormal(1.0F, 0.0F, 0.0F);

			for (int j = 0; j < 16; j++)
			{
				float f3 = (float)j / 16F;
				float f7 = (par2 + (par4 - par2) * f3) - 0.001953125F;
				float f11 = f * f3 + 0.0625F;
				par1Tessellator.AddVertexWithUV(f11, 1.0D, 0.0F - f1, f7, par3);
				par1Tessellator.AddVertexWithUV(f11, 1.0D, 0.0F, f7, par3);
				par1Tessellator.AddVertexWithUV(f11, 0.0F, 0.0F, f7, par5);
				par1Tessellator.AddVertexWithUV(f11, 0.0F, 0.0F - f1, f7, par5);
			}

			par1Tessellator.Draw();
			par1Tessellator.StartDrawingQuads();
			par1Tessellator.SetNormal(0.0F, 1.0F, 0.0F);

			for (int k = 0; k < 16; k++)
			{
				float f4 = (float)k / 16F;
				float f8 = (par5 + (par3 - par5) * f4) - 0.001953125F;
				float f12 = f * f4 + 0.0625F;
				par1Tessellator.AddVertexWithUV(0.0F, f12, 0.0F, par2, f8);
				par1Tessellator.AddVertexWithUV(f, f12, 0.0F, par4, f8);
				par1Tessellator.AddVertexWithUV(f, f12, 0.0F - f1, par4, f8);
				par1Tessellator.AddVertexWithUV(0.0F, f12, 0.0F - f1, par2, f8);
			}

			par1Tessellator.Draw();
			par1Tessellator.StartDrawingQuads();
			par1Tessellator.SetNormal(0.0F, -1F, 0.0F);

			for (int l = 0; l < 16; l++)
			{
				float f5 = (float)l / 16F;
				float f9 = (par5 + (par3 - par5) * f5) - 0.001953125F;
				float f13 = f * f5;
				par1Tessellator.AddVertexWithUV(f, f13, 0.0F, par4, f9);
				par1Tessellator.AddVertexWithUV(0.0F, f13, 0.0F, par2, f9);
				par1Tessellator.AddVertexWithUV(0.0F, f13, 0.0F - f1, par2, f9);
				par1Tessellator.AddVertexWithUV(f, f13, 0.0F - f1, par4, f9);
			}

			par1Tessellator.Draw();
		}

		/// <summary>
		/// Renders the active item in the player's hand when in first person mode. Args: partialTickTime
		/// </summary>
		public virtual void RenderItemInFirstPerson(float par1)
		{
			float f = PrevEquippedProgress + (EquippedProgress - PrevEquippedProgress) * par1;
			EntityPlayerSP entityplayersp = Mc.ThePlayer;
			float f1 = ((EntityPlayer)(entityplayersp)).PrevRotationPitch + (((EntityPlayer)(entityplayersp)).RotationPitch - ((EntityPlayer)(entityplayersp)).PrevRotationPitch) * par1;
			//GL.PushMatrix();
			//GL.Rotate(f1, 1.0F, 0.0F, 0.0F);
			//GL.Rotate(((EntityPlayer)(entityplayersp)).PrevRotationYaw + (((EntityPlayer)(entityplayersp)).RotationYaw - ((EntityPlayer)(entityplayersp)).PrevRotationYaw) * par1, 0.0F, 1.0F, 0.0F);
			RenderHelper.EnableStandardItemLighting();
			//GL.PopMatrix();

			if (entityplayersp is EntityPlayerSP)
			{
				EntityPlayerSP entityplayersp1 = (EntityPlayerSP)entityplayersp;
				float f2 = entityplayersp1.PrevRenderArmPitch + (entityplayersp1.RenderArmPitch - entityplayersp1.PrevRenderArmPitch) * par1;
				float f4 = entityplayersp1.PrevRenderArmYaw + (entityplayersp1.RenderArmYaw - entityplayersp1.PrevRenderArmYaw) * par1;
				//GL.Rotate((((EntityPlayer)(entityplayersp)).RotationPitch - f2) * 0.1F, 1.0F, 0.0F, 0.0F);
				//GL.Rotate((((EntityPlayer)(entityplayersp)).RotationYaw - f4) * 0.1F, 0.0F, 1.0F, 0.0F);
			}

			ItemStack itemstack = ItemToRender;
			float f3 = Mc.TheWorld.GetLightBrightness(MathHelper2.Floor_double(((EntityPlayer)(entityplayersp)).PosX), MathHelper2.Floor_double(((EntityPlayer)(entityplayersp)).PosY), MathHelper2.Floor_double(((EntityPlayer)(entityplayersp)).PosZ));
			f3 = 1.0F;
			int i = Mc.TheWorld.GetLightBrightnessForSkyBlocks(MathHelper2.Floor_double(((EntityPlayer)(entityplayersp)).PosX), MathHelper2.Floor_double(((EntityPlayer)(entityplayersp)).PosY), MathHelper2.Floor_double(((EntityPlayer)(entityplayersp)).PosZ), 0);
			int k = i % 0x10000;
			int l = i / 0x10000;
			OpenGlHelper.SetLightmapTextureCoords(OpenGlHelper.LightmapTexUnit, (float)k / 1.0F, (float)l / 1.0F);
			//GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);

			if (itemstack != null)
			{
				int j = Item.ItemsList[itemstack.ItemID].GetColorFromDamage(itemstack.GetItemDamage(), 0);
				float f8 = (float)(j >> 16 & 0xff) / 255F;
				float f13 = (float)(j >> 8 & 0xff) / 255F;
				float f19 = (float)(j & 0xff) / 255F;
				//GL.Color4(f3 * f8, f3 * f13, f3 * f19, 1.0F);
			}
			else
			{
				//GL.Color4(f3, f3, f3, 1.0F);
			}

			if (itemstack != null && itemstack.ItemID == Item.Map.ShiftedIndex)
			{
				//GL.PushMatrix();
			//	float f5 = 0.8F;
				float f9 = entityplayersp.GetSwingProgress(par1);
				float f14 = MathHelper2.Sin(f9 * (float)Math.PI);
				float f20 = MathHelper2.Sin(MathHelper2.Sqrt_float(f9) * (float)Math.PI);
				//GL.Translate(-f20 * 0.4F, MathHelper.Sin(MathHelper.Sqrt_float(f9) * (float)Math.PI * 2.0F) * 0.2F, -f14 * 0.2F);
				f9 = (1.0F - f1 / 45F) + 0.1F;

				if (f9 < 0.0F)
				{
					f9 = 0.0F;
				}

				if (f9 > 1.0F)
				{
					f9 = 1.0F;
				}

				f9 = -MathHelper2.Cos(f9 * (float)Math.PI) * 0.5F + 0.5F;
				//GL.Translate(0.0F, (0.0F * f5 - (1.0F - f) * 1.2F - f9 * 0.5F) + 0.04F, -0.9F * f5);
				//GL.Rotate(90F, 0.0F, 1.0F, 0.0F);
				//GL.Rotate(f9 * -85F, 0.0F, 0.0F, 1.0F);
				//GL.Enable(EnableCap.RescaleNormal);
				//GL.BindTexture(TextureTarget.Texture2D, Mc.RenderEngineOld.GetTextureForDownloadableImage(Mc.ThePlayer.SkinUrl, Mc.ThePlayer.GetTexture()));

				for (f14 = 0; f14 < 2; f14++)
				{
					f20 = f14 * 2 - 1;
					//GL.PushMatrix();
					//GL.Translate(-0F, -0.6F, 1.1F * (float)f20);
					//GL.Rotate(-45 * f20, 1.0F, 0.0F, 0.0F);
					//GL.Rotate(-90F, 0.0F, 0.0F, 1.0F);
					//GL.Rotate(59F, 0.0F, 0.0F, 1.0F);
					//GL.Rotate(-65 * f20, 0.0F, 1.0F, 0.0F);
					Render render1 = RenderManager.Instance.GetEntityRenderObject(Mc.ThePlayer);
					RenderPlayer renderplayer1 = (RenderPlayer)render1;
				//	float f34 = 1.0F;
					//GL.Scale(f34, f34, f34);
					renderplayer1.DrawFirstPersonHand();
					//GL.PopMatrix();
				}

				f14 = entityplayersp.GetSwingProgress(par1);
				f20 = MathHelper2.Sin(f14 * f14 * (float)Math.PI);
				float f27 = MathHelper2.Sin(MathHelper2.Sqrt_float(f14) * (float)Math.PI);
				//GL.Rotate(-f20 * 20F, 0.0F, 1.0F, 0.0F);
				//GL.Rotate(-f27 * 20F, 0.0F, 0.0F, 1.0F);
				//GL.Rotate(-f27 * 80F, 1.0F, 0.0F, 0.0F);
				f14 = 0.38F;
				//GL.Scale(f14, f14, f14);
				//GL.Rotate(90F, 0.0F, 1.0F, 0.0F);
				//GL.Rotate(180F, 0.0F, 0.0F, 1.0F);
				//GL.Translate(-1F, -1F, 0.0F);
				f20 = 0.015625F;
				//GL.Scale(f20, f20, f20);
				Mc.RenderEngineOld.BindTexture(Mc.RenderEngineOld.GetTexture("/misc/mapbg.png"));
				Tessellator tessellator = Tessellator.Instance;
				//GL.Normal3(0.0F, 0.0F, -1F);
				tessellator.StartDrawingQuads();
				sbyte byte0 = 7;
				tessellator.AddVertexWithUV(0 - byte0, 128 + byte0, 0.0F, 0.0F, 1.0D);
				tessellator.AddVertexWithUV(128 + byte0, 128 + byte0, 0.0F, 1.0D, 1.0D);
				tessellator.AddVertexWithUV(128 + byte0, 0 - byte0, 0.0F, 1.0D, 0.0F);
				tessellator.AddVertexWithUV(0 - byte0, 0 - byte0, 0.0F, 0.0F, 0.0F);
				tessellator.Draw();
				MapData mapdata = Item.Map.GetMapData(itemstack, Mc.TheWorld);
				MapItemRenderer.RenderMap(Mc.ThePlayer, Mc.RenderEngineOld, mapdata);
				//GL.PopMatrix();
			}
			else if (itemstack != null)
			{
				//GL.PushMatrix();
			//	float f6 = 0.8F;

				if (entityplayersp.GetItemInUseCount() > 0)
				{
					EnumAction enumaction = itemstack.GetItemUseAction();

					if (enumaction == EnumAction.Eat || enumaction == EnumAction.Drink)
					{
						float f15 = ((float)entityplayersp.GetItemInUseCount() - par1) + 1.0F;
						float f21 = 1.0F - f15 / (float)itemstack.GetMaxItemUseDuration();
						float f28 = f21;
						float f31 = 1.0F - f28;
						f31 = f31 * f31 * f31;
						f31 = f31 * f31 * f31;
						f31 = f31 * f31 * f31;
						float f35 = 1.0F - f31;
						//GL.Translate(0.0F, MathHelper.Abs(MathHelper.Cos((f15 / 4F) * (float)Math.PI) * 0.1F) * (float)((double)f28 <= 0.20000000000000001D ? 0 : 1), 0.0F);
						//GL.Translate(f35 * 0.6F, -f35 * 0.5F, 0.0F);
						//GL.Rotate(f35 * 90F, 0.0F, 1.0F, 0.0F);
						//GL.Rotate(f35 * 10F, 1.0F, 0.0F, 0.0F);
						//GL.Rotate(f35 * 30F, 0.0F, 0.0F, 1.0F);
					}
				}
				else
				{
					float f10 = entityplayersp.GetSwingProgress(par1);
					float f16 = MathHelper2.Sin(f10 * (float)Math.PI);
					float f22 = MathHelper2.Sin(MathHelper2.Sqrt_float(f10) * (float)Math.PI);
					//GL.Translate(-f22 * 0.4F, MathHelper.Sin(MathHelper.Sqrt_float(f10) * (float)Math.PI * 2.0F) * 0.2F, -f16 * 0.2F);
				}

				//GL.Translate(0.7F * f6, -0.65F * f6 - (1.0F - f) * 0.6F, -0.9F * f6);
				//GL.Rotate(45F, 0.0F, 1.0F, 0.0F);
				//GL.Enable(EnableCap.RescaleNormal);
				float f11 = entityplayersp.GetSwingProgress(par1);
				float f17 = MathHelper2.Sin(f11 * f11 * (float)Math.PI);
				float f23 = MathHelper2.Sin(MathHelper2.Sqrt_float(f11) * (float)Math.PI);
				//GL.Rotate(-f17 * 20F, 0.0F, 1.0F, 0.0F);
				//GL.Rotate(-f23 * 20F, 0.0F, 0.0F, 1.0F);
				//GL.Rotate(-f23 * 80F, 1.0F, 0.0F, 0.0F);
				f11 = 0.4F;
				//GL.Scale(f11, f11, f11);

				if (entityplayersp.GetItemInUseCount() > 0)
				{
					EnumAction enumaction1 = itemstack.GetItemUseAction();

					if (enumaction1 == EnumAction.Block)
					{
						//GL.Translate(-0.5F, 0.2F, 0.0F);
						//GL.Rotate(30F, 0.0F, 1.0F, 0.0F);
						//GL.Rotate(-80F, 1.0F, 0.0F, 0.0F);
						//GL.Rotate(60F, 0.0F, 1.0F, 0.0F);
					}
					else if (enumaction1 == EnumAction.Bow)
					{
						//GL.Rotate(-18F, 0.0F, 0.0F, 1.0F);
						//GL.Rotate(-12F, 0.0F, 1.0F, 0.0F);
						//GL.Rotate(-8F, 1.0F, 0.0F, 0.0F);
						//GL.Translate(-0.9F, 0.2F, 0.0F);
						float f24 = (float)itemstack.GetMaxItemUseDuration() - (((float)entityplayersp.GetItemInUseCount() - par1) + 1.0F);
						float f29 = f24 / 20F;
						f29 = (f29 * f29 + f29 * 2.0F) / 3F;

						if (f29 > 1.0F)
						{
							f29 = 1.0F;
						}

						if (f29 > 0.1F)
						{
							//GL.Translate(0.0F, MathHelper.Sin((f24 - 0.1F) * 1.3F) * 0.01F * (f29 - 0.1F), 0.0F);
						}

						//GL.Translate(0.0F, 0.0F, f29 * 0.1F);
						//GL.Rotate(-335F, 0.0F, 0.0F, 1.0F);
						//GL.Rotate(-50F, 0.0F, 1.0F, 0.0F);
						//GL.Translate(0.0F, 0.5F, 0.0F);
						float f32 = 1.0F + f29 * 0.2F;
						//GL.Scale(1.0F, 1.0F, f32);
						//GL.Translate(0.0F, -0.5F, 0.0F);
						//GL.Rotate(50F, 0.0F, 1.0F, 0.0F);
						//GL.Rotate(335F, 0.0F, 0.0F, 1.0F);
					}
				}

				if (itemstack.GetItem().ShouldRotateAroundWhenRendering())
				{
					//GL.Rotate(180F, 0.0F, 1.0F, 0.0F);
				}

				if (itemstack.GetItem().Func_46058_c())
				{
					RenderItem(entityplayersp, itemstack, 0);
					int i1 = Item.ItemsList[itemstack.ItemID].GetColorFromDamage(itemstack.GetItemDamage(), 1);
					float f25 = (float)(i1 >> 16 & 0xff) / 255F;
					float f30 = (float)(i1 >> 8 & 0xff) / 255F;
					float f33 = (float)(i1 & 0xff) / 255F;
					//GL.Color4(f3 * f25, f3 * f30, f3 * f33, 1.0F);
					RenderItem(entityplayersp, itemstack, 1);
				}
				else
				{
					RenderItem(entityplayersp, itemstack, 0);
				}

				//GL.PopMatrix();
			}
			else
			{
				//GL.PushMatrix();
			//	float f7 = 0.8F;
				float f12 = entityplayersp.GetSwingProgress(par1);
				float f18 = MathHelper2.Sin(f12 * (float)Math.PI);
				float f26 = MathHelper2.Sin(MathHelper2.Sqrt_float(f12) * (float)Math.PI);
				//GL.Translate(-f26 * 0.3F, MathHelper.Sin(MathHelper.Sqrt_float(f12) * (float)Math.PI * 2.0F) * 0.4F, -f18 * 0.4F);
				//GL.Translate(0.8F * f7, -0.75F * f7 - (1.0F - f) * 0.6F, -0.9F * f7);
				//GL.Rotate(45F, 0.0F, 1.0F, 0.0F);
				//GL.Enable(EnableCap.RescaleNormal);
				f12 = entityplayersp.GetSwingProgress(par1);
				f18 = MathHelper2.Sin(f12 * f12 * (float)Math.PI);
				f26 = MathHelper2.Sin(MathHelper2.Sqrt_float(f12) * (float)Math.PI);
				//GL.Rotate(f26 * 70F, 0.0F, 1.0F, 0.0F);
				//GL.Rotate(-f18 * 20F, 0.0F, 0.0F, 1.0F);
				//GL.BindTexture(TextureTarget.Texture2D, Mc.RenderEngineOld.GetTextureForDownloadableImage(Mc.ThePlayer.SkinUrl, Mc.ThePlayer.GetTexture()));
				//GL.Translate(-1F, 3.6F, 3.5F);
				//GL.Rotate(120F, 0.0F, 0.0F, 1.0F);
				//GL.Rotate(200F, 1.0F, 0.0F, 0.0F);
				//GL.Rotate(-135F, 0.0F, 1.0F, 0.0F);
				//GL.Scale(1.0F, 1.0F, 1.0F);
				//GL.Translate(5.6F, 0.0F, 0.0F);
				Render render = RenderManager.Instance.GetEntityRenderObject(Mc.ThePlayer);
				RenderPlayer renderplayer = (RenderPlayer)render;
				f26 = 1.0F;
				//GL.Scale(f26, f26, f26);
				renderplayer.DrawFirstPersonHand();
				//GL.PopMatrix();
			}

			//GL.Disable(EnableCap.RescaleNormal);
			RenderHelper.DisableStandardItemLighting();
		}

		/// <summary>
		/// Renders all the overlays that are in first person mode. Args: partialTickTime
		/// </summary>
		public virtual void RenderOverlays(float par1)
		{
			//GL.Disable(EnableCap.AlphaTest);

			if (Mc.ThePlayer.IsBurning())
			{
				int i = Mc.RenderEngineOld.GetTexture("/terrain.png");
				//GL.BindTexture(TextureTarget.Texture2D, i);
				RenderFireInFirstPerson(par1);
			}

			if (Mc.ThePlayer.IsEntityInsideOpaqueBlock())
			{
				int j = MathHelper2.Floor_double(Mc.ThePlayer.PosX);
				int l = MathHelper2.Floor_double(Mc.ThePlayer.PosY);
				int i1 = MathHelper2.Floor_double(Mc.ThePlayer.PosZ);
				int j1 = Mc.RenderEngineOld.GetTexture("/terrain.png");
				//GL.BindTexture(TextureTarget.Texture2D, j1);
				int k1 = Mc.TheWorld.GetBlockId(j, l, i1);

				if (Mc.TheWorld.IsBlockNormalCube(j, l, i1))
				{
					RenderInsideOfBlock(par1, Block.BlocksList[k1].GetBlockTextureFromSide(2));
				}
				else
				{
					for (int l1 = 0; l1 < 8; l1++)
					{
						float f = ((float)((l1 >> 0) % 2) - 0.5F) * Mc.ThePlayer.Width * 0.9F;
						float f1 = ((float)((l1 >> 1) % 2) - 0.5F) * Mc.ThePlayer.Height * 0.2F;
						float f2 = ((float)((l1 >> 2) % 2) - 0.5F) * Mc.ThePlayer.Width * 0.9F;
						int i2 = MathHelper2.Floor_float((float)j + f);
						int j2 = MathHelper2.Floor_float((float)l + f1);
						int k2 = MathHelper2.Floor_float((float)i1 + f2);

						if (Mc.TheWorld.IsBlockNormalCube(i2, j2, k2))
						{
							k1 = Mc.TheWorld.GetBlockId(i2, j2, k2);
						}
					}
				}

				if (Block.BlocksList[k1] != null)
				{
					RenderInsideOfBlock(par1, Block.BlocksList[k1].GetBlockTextureFromSide(2));
				}
			}

			if (Mc.ThePlayer.IsInsideOfMaterial(Material.Water))
			{
				int k = Mc.RenderEngineOld.GetTexture("/misc/water.png");
				//GL.BindTexture(TextureTarget.Texture2D, k);
				RenderWarpedTextureOverlay(par1);
			}

			//GL.Enable(EnableCap.AlphaTest);
		}

		/// <summary>
		/// Renders the texture of the block the player is inside as an overlay. Args: partialTickTime, blockTextureIndex
		/// </summary>
		private void RenderInsideOfBlock(float par1, int par2)
		{
			Tessellator tessellator = Tessellator.Instance;
			float f = Mc.ThePlayer.GetBrightness(par1);
			f = 0.1F;
			//GL.Color4(f, f, f, 0.5F);
			//GL.PushMatrix();
			float f1 = -1F;
			float f2 = 1.0F;
			float f3 = -1F;
			float f4 = 1.0F;
			float f5 = -0.5F;
			float f6 = 0.0078125F;
			float f7 = (float)(par2 % 16) / 256F - f6;
			float f8 = ((float)(par2 % 16) + 15.99F) / 256F + f6;
			float f9 = (float)(par2 / 16) / 256F - f6;
			float f10 = ((float)(par2 / 16) + 15.99F) / 256F + f6;
			tessellator.StartDrawingQuads();
			tessellator.AddVertexWithUV(f1, f3, f5, f8, f10);
			tessellator.AddVertexWithUV(f2, f3, f5, f7, f10);
			tessellator.AddVertexWithUV(f2, f4, f5, f7, f9);
			tessellator.AddVertexWithUV(f1, f4, f5, f8, f9);
			tessellator.Draw();
			//GL.PopMatrix();
			//GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
		}

		/// <summary>
		/// Renders a texture that warps around based on the direction the player is looking. Texture needs to be bound
		/// before being called. Used for the water overlay. Args: parialTickTime
		/// </summary>
		private void RenderWarpedTextureOverlay(float par1)
		{
			Tessellator tessellator = Tessellator.Instance;
			float f = Mc.ThePlayer.GetBrightness(par1);
			//GL.Color4(f, f, f, 0.5F);
			//GL.Enable(EnableCap.Blend);
			//GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			//GL.PushMatrix();
			float f1 = 4F;
			float f2 = -1F;
			float f3 = 1.0F;
			float f4 = -1F;
			float f5 = 1.0F;
			float f6 = -0.5F;
			float f7 = -Mc.ThePlayer.RotationYaw / 64F;
			float f8 = Mc.ThePlayer.RotationPitch / 64F;
			tessellator.StartDrawingQuads();
			tessellator.AddVertexWithUV(f2, f4, f6, f1 + f7, f1 + f8);
			tessellator.AddVertexWithUV(f3, f4, f6, 0.0F + f7, f1 + f8);
			tessellator.AddVertexWithUV(f3, f5, f6, 0.0F + f7, 0.0F + f8);
			tessellator.AddVertexWithUV(f2, f5, f6, f1 + f7, 0.0F + f8);
			tessellator.Draw();
			//GL.PopMatrix();
			//GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
			//GL.Disable(EnableCap.Blend);
		}

		/// <summary>
		/// Renders the fire on the screen for first person mode. Arg: partialTickTime
		/// </summary>
		private void RenderFireInFirstPerson(float par1)
		{
			Tessellator tessellator = Tessellator.Instance;
			//GL.Color4(1.0F, 1.0F, 1.0F, 0.9F);
			//GL.Enable(EnableCap.Blend);
			//GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			float f = 1.0F;

			for (int i = 0; i < 2; i++)
			{
				//GL.PushMatrix();
				int j = Block.Fire.BlockIndexInTexture + i * 16;
				int k = (j & 0xf) << 4;
				int l = j & 0xf0;
				float f1 = (float)k / 256F;
				float f2 = ((float)k + 15.99F) / 256F;
				float f3 = (float)l / 256F;
				float f4 = ((float)l + 15.99F) / 256F;
				float f5 = (0.0F - f) / 2.0F;
				float f6 = f5 + f;
				float f7 = 0.0F - f / 2.0F;
				float f8 = f7 + f;
				float f9 = -0.5F;
				//GL.Translate((float)(-(i * 2 - 1)) * 0.24F, -0.3F, 0.0F);
				//GL.Rotate((float)(i * 2 - 1) * 10F, 0.0F, 1.0F, 0.0F);
				tessellator.StartDrawingQuads();
				tessellator.AddVertexWithUV(f5, f7, f9, f2, f4);
				tessellator.AddVertexWithUV(f6, f7, f9, f1, f4);
				tessellator.AddVertexWithUV(f6, f8, f9, f1, f3);
				tessellator.AddVertexWithUV(f5, f8, f9, f2, f3);
				tessellator.Draw();
				//GL.PopMatrix();
			}

			//GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
			//GL.Disable(EnableCap.Blend);
		}

		public virtual void UpdateEquippedItem()
		{
			PrevEquippedProgress = EquippedProgress;
			EntityPlayerSP entityplayersp = Mc.ThePlayer;
			ItemStack itemstack = ((EntityPlayer)(entityplayersp)).Inventory.GetCurrentItem();
			ItemStack itemstack1 = itemstack;
			bool flag = EquippedItemSlot == ((EntityPlayer)(entityplayersp)).Inventory.CurrentItem && itemstack1 == ItemToRender;

			if (ItemToRender == null && itemstack1 == null)
			{
				flag = true;
			}

			if (itemstack1 != null && ItemToRender != null && itemstack1 != ItemToRender && itemstack1.ItemID == ItemToRender.ItemID && itemstack1.GetItemDamage() == ItemToRender.GetItemDamage())
			{
				ItemToRender = itemstack1;
				flag = true;
			}

			float f = 0.4F;
			float f1 = flag ? 1.0F : 0.0F;
			float f2 = f1 - EquippedProgress;

			if (f2 < -f)
			{
				f2 = -f;
			}

			if (f2 > f)
			{
				f2 = f;
			}

			EquippedProgress += f2;

			if (EquippedProgress < 0.1F)
			{
				ItemToRender = itemstack1;
				EquippedItemSlot = ((EntityPlayer)(entityplayersp)).Inventory.CurrentItem;
			}
		}

		public virtual void Func_9449_b()
		{
			EquippedProgress = 0.0F;
		}

		public virtual void Func_9450_c()
		{
			EquippedProgress = 0.0F;
		}
	}
}