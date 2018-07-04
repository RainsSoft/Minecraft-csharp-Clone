using System;
using System.Text;
using Microsoft.Xna.Framework;

namespace net.minecraft.src
{
	public class RenderPlayer : RenderLiving
	{
		private ModelBiped ModelBipedMain;
		private ModelBiped ModelArmorChestplate;
		private ModelBiped ModelArmor;
		private static readonly string[] ArmorFilenamePrefix = { "cloth", "chain", "iron", "diamond", "gold" };

		public RenderPlayer() : base(new ModelBiped(0.0F), 0.5F)
		{
			ModelBipedMain = (ModelBiped)MainModel;
			ModelArmorChestplate = new ModelBiped(1.0F);
			ModelArmor = new ModelBiped(0.5F);
		}

		/// <summary>
		/// Set the specified armor model as the player model. Args: player, armorSlot, partialTick
		/// </summary>
		protected virtual int SetArmorModel(EntityPlayer par1EntityPlayer, int par2, float par3)
		{
			ItemStack itemstack = par1EntityPlayer.Inventory.ArmorItemInSlot(3 - par2);

			if (itemstack != null)
			{
				Item item = itemstack.GetItem();

				if (item is ItemArmor)
				{
					ItemArmor itemarmor = (ItemArmor)item;
					LoadTexture((new StringBuilder()).Append("/armor/").Append(ArmorFilenamePrefix[itemarmor.RenderIndex]).Append("_").Append(par2 != 2 ? 1 : 2).Append(".png").ToString());
					ModelBiped modelbiped = par2 != 2 ? ModelArmorChestplate : ModelArmor;
					modelbiped.BipedHead.ShowModel = par2 == 0;
					modelbiped.BipedHeadwear.ShowModel = par2 == 0;
					modelbiped.BipedBody.ShowModel = par2 == 1 || par2 == 2;
					modelbiped.BipedRightArm.ShowModel = par2 == 1;
					modelbiped.BipedLeftArm.ShowModel = par2 == 1;
					modelbiped.BipedRightLeg.ShowModel = par2 == 2 || par2 == 3;
					modelbiped.BipedLeftLeg.ShowModel = par2 == 2 || par2 == 3;
					SetRenderPassModel(modelbiped);
					return !itemstack.IsItemEnchanted() ? 1 : 15;
				}
			}

			return -1;
		}

		public virtual void DoRenderPlayer(EntityPlayer par1EntityPlayer, double par2, double par4, double par6, float par8, float par9)
		{
			ItemStack itemstack = par1EntityPlayer.Inventory.GetCurrentItem();
			ModelArmorChestplate.HeldItemRight = ModelArmor.HeldItemRight = ModelBipedMain.HeldItemRight = itemstack == null ? 0 : 1;

			if (itemstack != null && par1EntityPlayer.GetItemInUseCount() > 0)
			{
				EnumAction enumaction = itemstack.GetItemUseAction();

				if (enumaction == EnumAction.Block)
				{
					ModelArmorChestplate.HeldItemRight = ModelArmor.HeldItemRight = ModelBipedMain.HeldItemRight = 3;
				}
				else if (enumaction == EnumAction.Bow)
				{
					ModelArmorChestplate.AimedBow = ModelArmor.AimedBow = ModelBipedMain.AimedBow = true;
				}
			}

			ModelArmorChestplate.IsSneak = ModelArmor.IsSneak = ModelBipedMain.IsSneak = par1EntityPlayer.IsSneaking();
			double d = par4 - (double)par1EntityPlayer.YOffset;

			if (par1EntityPlayer.IsSneaking() && !(par1EntityPlayer is EntityPlayerSP))
			{
				d -= 0.125D;
			}

			base.DoRenderLiving(par1EntityPlayer, par2, d, par6, par8, par9);
			ModelArmorChestplate.AimedBow = ModelArmor.AimedBow = ModelBipedMain.AimedBow = false;
			ModelArmorChestplate.IsSneak = ModelArmor.IsSneak = ModelBipedMain.IsSneak = false;
			ModelArmorChestplate.HeldItemRight = ModelArmor.HeldItemRight = ModelBipedMain.HeldItemRight = 0;
		}

		/// <summary>
		/// Used to render a player's name above their head
		/// </summary>
		protected virtual void RenderName(EntityPlayer par1EntityPlayer, double par2, double par4, double par6)
		{
			if (Minecraft.IsGuiEnabled() && par1EntityPlayer != RenderManager.LivingPlayer)
			{
				float f = 1.6F;
				float f1 = 0.01666667F * f;
				float f2 = par1EntityPlayer.GetDistanceToEntity(RenderManager.LivingPlayer);
				float f3 = par1EntityPlayer.IsSneaking() ? 32F : 64F;

				if (f2 < f3)
				{
					string s = par1EntityPlayer.Username;

					if (!par1EntityPlayer.IsSneaking())
					{
						if (par1EntityPlayer.IsPlayerSleeping())
						{
							RenderLivingLabel(par1EntityPlayer, s, par2, par4 - 1.5D, par6, 64);
						}
						else
						{
							RenderLivingLabel(par1EntityPlayer, s, par2, par4, par6, 64);
						}
					}
					else
					{
						FontRenderer fontrenderer = GetFontRendererFromRenderManager();
						//GL.PushMatrix();
						//GL.Translate((float)par2 + 0.0F, (float)par4 + 2.3F, (float)par6);
						//GL.Normal3(0.0F, 1.0F, 0.0F);
						//GL.Rotate(-RenderManager.PlayerViewY, 0.0F, 1.0F, 0.0F);
						//GL.Rotate(RenderManager.PlayerViewX, 1.0F, 0.0F, 0.0F);
						//GL.Scale(-f1, -f1, f1);
						//GL.Disable(EnableCap.Lighting);
						//GL.Translate(0.0F, 0.25F / f1, 0.0F);
						//GL.DepthMask(false);
						//GL.Enable(EnableCap.Blend);
						//GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
						Tessellator tessellator = Tessellator.Instance;
						//GL.Disable(EnableCap.Texture2D);
						tessellator.StartDrawingQuads();
						int i = fontrenderer.GetStringWidth(s) / 2;
						tessellator.SetColorRGBA_F(0.0F, 0.0F, 0.0F, 0.25F);
						tessellator.AddVertex(-i - 1, -1D, 0.0F);
						tessellator.AddVertex(-i - 1, 8D, 0.0F);
						tessellator.AddVertex(i + 1, 8D, 0.0F);
						tessellator.AddVertex(i + 1, -1D, 0.0F);
						tessellator.Draw();
						//GL.Enable(EnableCap.Texture2D);
						//GL.DepthMask(true);
						fontrenderer.DrawString(s, -fontrenderer.GetStringWidth(s) / 2, 0, 0x20ffffff);
						//GL.Enable(EnableCap.Lighting);
						//GL.Disable(EnableCap.Blend);
						//GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
						//GL.PopMatrix();
					}
				}
			}
		}

		/// <summary>
		/// Method for adding special render rules
		/// </summary>
		protected virtual void RenderSpecials(EntityPlayer par1EntityPlayer, float par2)
		{
			base.RenderEquippedItems(par1EntityPlayer, par2);
			ItemStack itemstack = par1EntityPlayer.Inventory.ArmorItemInSlot(3);

			if (itemstack != null && itemstack.GetItem().ShiftedIndex < 256)
			{
				//GL.PushMatrix();
				ModelBipedMain.BipedHead.PostRender(0.0625F);

				if (RenderBlocks.RenderItemIn3d(Block.BlocksList[itemstack.ItemID].GetRenderType()))
				{
					float f = 0.625F;
					//GL.Translate(0.0F, -0.25F, 0.0F);
					//GL.Rotate(180F, 0.0F, 1.0F, 0.0F);
					//GL.Scale(f, -f, f);
				}

				RenderManager.ItemRenderer.RenderItem(par1EntityPlayer, itemstack, 0);
				//GL.PopMatrix();
			}

			if (par1EntityPlayer.Username.Equals("deadmau5") && LoadDownloadableImageTexture(par1EntityPlayer.SkinUrl, null))
			{
				for (int i = 0; i < 2; i++)
				{
					float f1 = (par1EntityPlayer.PrevRotationYaw + (par1EntityPlayer.RotationYaw - par1EntityPlayer.PrevRotationYaw) * par2) - (par1EntityPlayer.PrevRenderYawOffset + (par1EntityPlayer.RenderYawOffset - par1EntityPlayer.PrevRenderYawOffset) * par2);
					float f2 = par1EntityPlayer.PrevRotationPitch + (par1EntityPlayer.RotationPitch - par1EntityPlayer.PrevRotationPitch) * par2;
					//GL.PushMatrix();
					//GL.Rotate(f1, 0.0F, 1.0F, 0.0F);
					//GL.Rotate(f2, 1.0F, 0.0F, 0.0F);
					//GL.Translate(0.375F * (float)(i * 2 - 1), 0.0F, 0.0F);
					//GL.Translate(0.0F, -0.375F, 0.0F);
					//GL.Rotate(-f2, 1.0F, 0.0F, 0.0F);
					//GL.Rotate(-f1, 0.0F, 1.0F, 0.0F);
					float f7 = 1.333333F;
					//GL.Scale(f7, f7, f7);
					ModelBipedMain.RenderEars(0.0625F);
					//GL.PopMatrix();
				}
			}

			if (LoadDownloadableImageTexture(par1EntityPlayer.PlayerCloakUrl, null))
			{
				//GL.PushMatrix();
				//GL.Translate(0.0F, 0.0F, 0.125F);
				double d = (par1EntityPlayer.Field_20066_r + (par1EntityPlayer.Field_20063_u - par1EntityPlayer.Field_20066_r) * (double)par2) - (par1EntityPlayer.PrevPosX + (par1EntityPlayer.PosX - par1EntityPlayer.PrevPosX) * (double)par2);
				double d1 = (par1EntityPlayer.Field_20065_s + (par1EntityPlayer.Field_20062_v - par1EntityPlayer.Field_20065_s) * (double)par2) - (par1EntityPlayer.PrevPosY + (par1EntityPlayer.PosY - par1EntityPlayer.PrevPosY) * (double)par2);
				double d2 = (par1EntityPlayer.Field_20064_t + (par1EntityPlayer.Field_20061_w - par1EntityPlayer.Field_20064_t) * (double)par2) - (par1EntityPlayer.PrevPosZ + (par1EntityPlayer.PosZ - par1EntityPlayer.PrevPosZ) * (double)par2);
				float f10 = par1EntityPlayer.PrevRenderYawOffset + (par1EntityPlayer.RenderYawOffset - par1EntityPlayer.PrevRenderYawOffset) * par2;
				double d3 = MathHelper2.Sin((f10 * (float)Math.PI) / 180F);
				double d4 = -MathHelper2.Cos((f10 * (float)Math.PI) / 180F);
				float f12 = (float)d1 * 10F;

				if (f12 < -6F)
				{
					f12 = -6F;
				}

				if (f12 > 32F)
				{
					f12 = 32F;
				}

				float f13 = (float)(d * d3 + d2 * d4) * 100F;
				float f14 = (float)(d * d4 - d2 * d3) * 100F;

				if (f13 < 0.0F)
				{
					f13 = 0.0F;
				}

				float f15 = par1EntityPlayer.PrevCameraYaw + (par1EntityPlayer.CameraYaw - par1EntityPlayer.PrevCameraYaw) * par2;
				f12 += MathHelper2.Sin((par1EntityPlayer.PrevDistanceWalkedModified + (par1EntityPlayer.DistanceWalkedModified - par1EntityPlayer.PrevDistanceWalkedModified) * par2) * 6F) * 32F * f15;

				if (par1EntityPlayer.IsSneaking())
				{
					f12 += 25F;
				}

				//GL.Rotate(6F + f13 / 2.0F + f12, 1.0F, 0.0F, 0.0F);
				//GL.Rotate(f14 / 2.0F, 0.0F, 0.0F, 1.0F);
				//GL.Rotate(-f14 / 2.0F, 0.0F, 1.0F, 0.0F);
				//GL.Rotate(180F, 0.0F, 1.0F, 0.0F);
				ModelBipedMain.RenderCloak(0.0625F);
				//GL.PopMatrix();
			}

			ItemStack itemstack1 = par1EntityPlayer.Inventory.GetCurrentItem();

			if (itemstack1 != null)
			{
				//GL.PushMatrix();
				ModelBipedMain.BipedRightArm.PostRender(0.0625F);
				//GL.Translate(-0.0625F, 0.4375F, 0.0625F);

				if (par1EntityPlayer.FishEntity != null)
				{
					itemstack1 = new ItemStack(Item.Stick);
				}

				EnumAction enumaction = EnumAction.None;

				if (par1EntityPlayer.GetItemInUseCount() > 0)
				{
					enumaction = itemstack1.GetItemUseAction();
				}

				if (itemstack1.ItemID < 256 && RenderBlocks.RenderItemIn3d(Block.BlocksList[itemstack1.ItemID].GetRenderType()))
				{
					float f3 = 0.5F;
					//GL.Translate(0.0F, 0.1875F, -0.3125F);
					f3 *= 0.75F;
					//GL.Rotate(20F, 1.0F, 0.0F, 0.0F);
					//GL.Rotate(45F, 0.0F, 1.0F, 0.0F);
					//GL.Scale(f3, -f3, f3);
				}
				else if (itemstack1.ItemID == Item.Bow.ShiftedIndex)
				{
					float f4 = 0.625F;
					//GL.Translate(0.0F, 0.125F, 0.3125F);
					//GL.Rotate(-20F, 0.0F, 1.0F, 0.0F);
					//GL.Scale(f4, -f4, f4);
					//GL.Rotate(-100F, 1.0F, 0.0F, 0.0F);
					//GL.Rotate(45F, 0.0F, 1.0F, 0.0F);
				}
				else if (Item.ItemsList[itemstack1.ItemID].IsFull3D())
				{
					float f5 = 0.625F;

					if (Item.ItemsList[itemstack1.ItemID].ShouldRotateAroundWhenRendering())
					{
						//GL.Rotate(180F, 0.0F, 0.0F, 1.0F);
						//GL.Translate(0.0F, -0.125F, 0.0F);
					}

					if (par1EntityPlayer.GetItemInUseCount() > 0 && enumaction == EnumAction.Block)
					{
						//GL.Translate(0.05F, 0.0F, -0.1F);
						//GL.Rotate(-50F, 0.0F, 1.0F, 0.0F);
						//GL.Rotate(-10F, 1.0F, 0.0F, 0.0F);
						//GL.Rotate(-60F, 0.0F, 0.0F, 1.0F);
					}

					//GL.Translate(0.0F, 0.1875F, 0.0F);
					//GL.Scale(f5, -f5, f5);
					//GL.Rotate(-100F, 1.0F, 0.0F, 0.0F);
					//GL.Rotate(45F, 0.0F, 1.0F, 0.0F);
				}
				else
				{
					float f6 = 0.375F;
					//GL.Translate(0.25F, 0.1875F, -0.1875F);
					//GL.Scale(f6, f6, f6);
					//GL.Rotate(60F, 0.0F, 0.0F, 1.0F);
					//GL.Rotate(-90F, 1.0F, 0.0F, 0.0F);
					//GL.Rotate(20F, 0.0F, 0.0F, 1.0F);
				}

				if (itemstack1.GetItem().Func_46058_c())
				{
					for (int j = 0; j <= 1; j++)
					{
						int k = itemstack1.GetItem().GetColorFromDamage(itemstack1.GetItemDamage(), j);
						float f8 = (float)(k >> 16 & 0xff) / 255F;
						float f9 = (float)(k >> 8 & 0xff) / 255F;
						float f11 = (float)(k & 0xff) / 255F;
						//GL.Color4(f8, f9, f11, 1.0F);
						RenderManager.ItemRenderer.RenderItem(par1EntityPlayer, itemstack1, j);
					}
				}
				else
				{
					RenderManager.ItemRenderer.RenderItem(par1EntityPlayer, itemstack1, 0);
				}

				//GL.PopMatrix();
			}
		}

		protected virtual void RenderPlayerScale(EntityPlayer par1EntityPlayer, float par2)
		{
			float f = 0.9375F;
			//GL.Scale(f, f, f);
		}

		public virtual void DrawFirstPersonHand()
		{
			ModelBipedMain.OnGround = 0.0F;
			ModelBipedMain.SetRotationAngles(0.0F, 0.0F, 0.0F, 0.0F, 0.0F, 0.0625F);
			ModelBipedMain.BipedRightArm.Render(0.0625F);
		}

		/// <summary>
		/// Renders player with sleeping offset if sleeping
		/// </summary>
		protected virtual void RenderPlayerSleep(EntityPlayer par1EntityPlayer, double par2, double par4, double par6)
		{
			if (par1EntityPlayer.IsEntityAlive() && par1EntityPlayer.IsPlayerSleeping())
			{
				base.RenderLivingAt(par1EntityPlayer, par2 + (double)par1EntityPlayer.Field_22063_x, par4 + (double)par1EntityPlayer.Field_22062_y, par6 + (double)par1EntityPlayer.Field_22061_z);
			}
			else
			{
				base.RenderLivingAt(par1EntityPlayer, par2, par4, par6);
			}
		}

		/// <summary>
		/// Rotates the player if the player is sleeping. This method is called in rotateCorpse.
		/// </summary>
		protected virtual void RotatePlayer(EntityPlayer par1EntityPlayer, float par2, float par3, float par4)
		{
			if (par1EntityPlayer.IsEntityAlive() && par1EntityPlayer.IsPlayerSleeping())
			{
				//GL.Rotate(par1EntityPlayer.GetBedOrientationInDegrees(), 0.0F, 1.0F, 0.0F);
				//GL.Rotate(GetDeathMaxRotation(par1EntityPlayer), 0.0F, 0.0F, 1.0F);
				//GL.Rotate(270F, 0.0F, 1.0F, 0.0F);
			}
			else
			{
				base.RotateCorpse(par1EntityPlayer, par2, par3, par4);
			}
		}

		/// <summary>
		/// Passes the specialRender and renders it
		/// </summary>
		protected override void PassSpecialRender(EntityLiving par1EntityLiving, double par2, double par4, double par6)
		{
			RenderName((EntityPlayer)par1EntityLiving, par2, par4, par6);
		}

		/// <summary>
		/// Allows the render to do any OpenGL state modifications necessary before the model is rendered. Args:
		/// entityLiving, partialTickTime
		/// </summary>
		protected override void PreRenderCallback(EntityLiving par1EntityLiving, float par2)
		{
			RenderPlayerScale((EntityPlayer)par1EntityLiving, par2);
		}

		/// <summary>
		/// Queries whether should render the specified pass or not.
		/// </summary>
		protected override int ShouldRenderPass(EntityLiving par1EntityLiving, int par2, float par3)
		{
			return SetArmorModel((EntityPlayer)par1EntityLiving, par2, par3);
		}

		protected override void RenderEquippedItems(EntityLiving par1EntityLiving, float par2)
		{
			RenderSpecials((EntityPlayer)par1EntityLiving, par2);
		}

		protected override void RotateCorpse(EntityLiving par1EntityLiving, float par2, float par3, float par4)
		{
			RotatePlayer((EntityPlayer)par1EntityLiving, par2, par3, par4);
		}

		/// <summary>
		/// Sets a simple glTranslate on a LivingEntity.
		/// </summary>
		protected override void RenderLivingAt(EntityLiving par1EntityLiving, double par2, double par4, double par6)
		{
			RenderPlayerSleep((EntityPlayer)par1EntityLiving, par2, par4, par6);
		}

		public override void DoRenderLiving(EntityLiving par1EntityLiving, double par2, double par4, double par6, float par8, float par9)
		{
			DoRenderPlayer((EntityPlayer)par1EntityLiving, par2, par4, par6, par8, par9);
		}

		/// <summary>
		/// Actually renders the given argument. This is a synthetic bridge method, always casting down its argument and then
		/// handing it off to a worker function which does the actual work. In all probabilty, the class Render is generic
		/// (Render<T extends Entity) and this method has signature public void doRender(T entity, double d, double d1,
		/// double d2, float f, float f1). But JAD is pre 1.5 so doesn't do that.
		/// </summary>
		public override void DoRender(Entity par1Entity, double par2, double par4, double par6, float par8, float par9)
		{
			DoRenderPlayer((EntityPlayer)par1Entity, par2, par4, par6, par8, par9);
		}
	}
}