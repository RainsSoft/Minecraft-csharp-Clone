using System;
using System.Text;
using Microsoft.Xna.Framework;

namespace net.minecraft.src
{
	public class RenderItem : Render
	{
		private new RenderBlocks RenderBlocks;

		/// <summary>
		/// The RNG used in RenderItem (for bobbing itemstacks on the ground) </summary>
		private Random Random;
		public bool Field_27004_a;

		/// <summary>
		/// Defines the zLevel of rendering of item on GUI. </summary>
		public float ZLevel;

		public RenderItem()
		{
			RenderBlocks = new RenderBlocks();
			Random = new Random();
			Field_27004_a = true;
			ZLevel = 0.0F;
			ShadowSize = 0.15F;
			ShadowOpaque = 0.75F;
		}

		/// <summary>
		/// Renders the item
		/// </summary>
		public virtual void DoRenderItem(EntityItem par1EntityItem, double par2, double par4, double par6, float par8, float par9)
		{
			Random.SetSeed(187);
			ItemStack itemstack = par1EntityItem.ItemStack;
			//GL.PushMatrix();
			float f = MathHelper2.Sin(((float)par1EntityItem.Age + par9) / 10F + par1EntityItem.Field_804_d) * 0.1F + 0.1F;
			float f1 = (((float)par1EntityItem.Age + par9) / 20F + par1EntityItem.Field_804_d) * (180F / (float)Math.PI);
			sbyte byte0 = 1;

			if (par1EntityItem.ItemStack.StackSize > 1)
			{
				byte0 = 2;
			}

			if (par1EntityItem.ItemStack.StackSize > 5)
			{
				byte0 = 3;
			}

			if (par1EntityItem.ItemStack.StackSize > 20)
			{
				byte0 = 4;
			}

			//GL.Translate((float)par2, (float)par4 + f, (float)par6);
			//GL.Enable(EnableCap.RescaleNormal);

			if (itemstack.ItemID < 256 && RenderBlocks.RenderItemIn3d(Block.BlocksList[itemstack.ItemID].GetRenderType()))
			{
				//GL.Rotate(f1, 0.0F, 1.0F, 0.0F);
				LoadTexture("/terrain.png");
				float f2 = 0.25F;
				int k = Block.BlocksList[itemstack.ItemID].GetRenderType();

				if (k == 1 || k == 19 || k == 12 || k == 2)
				{
					f2 = 0.5F;
				}

				//GL.Scale(f2, f2, f2);

				for (int j1 = 0; j1 < byte0; j1++)
				{
					//GL.PushMatrix();

					if (j1 > 0)
					{
						float f5 = ((Random.NextFloat() * 2.0F - 1.0F) * 0.2F) / f2;
						float f8 = ((Random.NextFloat() * 2.0F - 1.0F) * 0.2F) / f2;
						float f11 = ((Random.NextFloat() * 2.0F - 1.0F) * 0.2F) / f2;
						//GL.Translate(f5, f8, f11);
					}

					float f6 = 1.0F;
					RenderBlocks.RenderBlockAsItem(Block.BlocksList[itemstack.ItemID], itemstack.GetItemDamage(), f6);
					//GL.PopMatrix();
				}
			}
			else if (itemstack.GetItem().Func_46058_c())
			{
				//GL.Scale(0.5F, 0.5F, 0.5F);
				LoadTexture("/gui/items.png");

				for (int i = 0; i <= 1; i++)
				{
					int l = itemstack.GetItem().Func_46057_a(itemstack.GetItemDamage(), i);
					float f3 = 1.0F;

					if (Field_27004_a)
					{
						int k1 = Item.ItemsList[itemstack.ItemID].GetColorFromDamage(itemstack.GetItemDamage(), i);
						float f9 = (float)(k1 >> 16 & 0xff) / 255F;
						float f12 = (float)(k1 >> 8 & 0xff) / 255F;
						float f14 = (float)(k1 & 0xff) / 255F;
						//GL.Color4(f9 * f3, f12 * f3, f14 * f3, 1.0F);
					}

					Func_40267_a(l, byte0);
				}
			}
			else
			{
				//GL.Scale(0.5F, 0.5F, 0.5F);
				int j = itemstack.GetIconIndex();

				if (itemstack.ItemID < 256)
				{
					LoadTexture("/terrain.png");
				}
				else
				{
					LoadTexture("/gui/items.png");
				}

				if (Field_27004_a)
				{
					int i1 = Item.ItemsList[itemstack.ItemID].GetColorFromDamage(itemstack.GetItemDamage(), 0);
					float f4 = (float)(i1 >> 16 & 0xff) / 255F;
					float f7 = (float)(i1 >> 8 & 0xff) / 255F;
					float f10 = (float)(i1 & 0xff) / 255F;
					float f13 = 1.0F;
					//GL.Color4(f4 * f13, f7 * f13, f10 * f13, 1.0F);
				}

				Func_40267_a(j, byte0);
			}

			//GL.Disable(EnableCap.RescaleNormal);
			//GL.PopMatrix();
		}

		private void Func_40267_a(int par1, int par2)
		{
			Tessellator tessellator = Tessellator.Instance;
			float f = (float)((par1 % 16) * 16 + 0) / 256F;
			float f1 = (float)((par1 % 16) * 16 + 16) / 256F;
			float f2 = (float)((par1 / 16) * 16 + 0) / 256F;
			float f3 = (float)((par1 / 16) * 16 + 16) / 256F;
			float f4 = 1.0F;
			float f5 = 0.5F;
			float f6 = 0.25F;

			for (int i = 0; i < par2; i++)
			{
				//GL.PushMatrix();

				if (i > 0)
				{
					float f7 = (Random.NextFloat() * 2.0F - 1.0F) * 0.3F;
					float f8 = (Random.NextFloat() * 2.0F - 1.0F) * 0.3F;
					float f9 = (Random.NextFloat() * 2.0F - 1.0F) * 0.3F;
					//GL.Translate(f7, f8, f9);
				}

				//GL.Rotate(180F - RenderManager.PlayerViewY, 0.0F, 1.0F, 0.0F);
				tessellator.StartDrawingQuads();
				tessellator.SetNormal(0.0F, 1.0F, 0.0F);
				tessellator.AddVertexWithUV(0.0F - f5, 0.0F - f6, 0.0F, f, f3);
				tessellator.AddVertexWithUV(f4 - f5, 0.0F - f6, 0.0F, f1, f3);
				tessellator.AddVertexWithUV(f4 - f5, 1.0F - f6, 0.0F, f1, f2);
				tessellator.AddVertexWithUV(0.0F - f5, 1.0F - f6, 0.0F, f, f2);
				tessellator.Draw();
				//GL.PopMatrix();
			}
		}

		public virtual void DrawItemIntoGui(FontRenderer par1FontRenderer, RenderEngineOld par2RenderEngine, int par3, int par4, int par5, int par6, int par7)
		{
			if (par3 < 256 && RenderBlocks.RenderItemIn3d(Block.BlocksList[par3].GetRenderType()))
			{
				int i = par3;
				par2RenderEngine.BindTexture(par2RenderEngine.GetTexture("/terrain.png"));
				Block block = Block.BlocksList[i];
				//GL.PushMatrix();
				//GL.Translate(par6 - 2, par7 + 3, -3F + ZLevel);
				//GL.Scale(10F, 10F, 10F);
				//GL.Translate(1.0F, 0.5F, 1.0F);
				//GL.Scale(1.0F, 1.0F, -1F);
				//GL.Rotate(210F, 1.0F, 0.0F, 0.0F);
				//GL.Rotate(45F, 0.0F, 1.0F, 0.0F);
				int i1 = Item.ItemsList[par3].GetColorFromDamage(par4, 0);
				float f2 = (float)(i1 >> 16 & 0xff) / 255F;
				float f5 = (float)(i1 >> 8 & 0xff) / 255F;
				float f7 = (float)(i1 & 0xff) / 255F;

				if (Field_27004_a)
				{
					//GL.Color4(f2, f5, f7, 1.0F);
				}

				//GL.Rotate(-90F, 0.0F, 1.0F, 0.0F);
				RenderBlocks.UseInventoryTint = Field_27004_a;
				RenderBlocks.RenderBlockAsItem(block, par4, 1.0F);
				RenderBlocks.UseInventoryTint = true;
				//GL.PopMatrix();
			}
			else if (Item.ItemsList[par3].Func_46058_c())
			{
				//GL.Disable(EnableCap.Lighting);
				par2RenderEngine.BindTexture(par2RenderEngine.GetTexture("/gui/items.png"));

				for (int j = 0; j <= 1; j++)
				{
					int l = Item.ItemsList[par3].Func_46057_a(par4, j);
					int j1 = Item.ItemsList[par3].GetColorFromDamage(par4, j);
					float f3 = (float)(j1 >> 16 & 0xff) / 255F;
					float f6 = (float)(j1 >> 8 & 0xff) / 255F;
					float f8 = (float)(j1 & 0xff) / 255F;

					if (Field_27004_a)
					{
						//GL.Color4(f3, f6, f8, 1.0F);
					}

					RenderTexturedQuad(par6, par7, (l % 16) * 16, (l / 16) * 16, 16, 16);
				}

				//GL.Enable(EnableCap.Lighting);
			}
			else if (par5 >= 0)
			{
				//GL.Disable(EnableCap.Lighting);

				if (par3 < 256)
				{
					par2RenderEngine.BindTexture(par2RenderEngine.GetTexture("/terrain.png"));
				}
				else
				{
					par2RenderEngine.BindTexture(par2RenderEngine.GetTexture("/gui/items.png"));
				}

				int k = Item.ItemsList[par3].GetColorFromDamage(par4, 0);
				float f = (float)(k >> 16 & 0xff) / 255F;
				float f1 = (float)(k >> 8 & 0xff) / 255F;
				float f4 = (float)(k & 0xff) / 255F;

				if (Field_27004_a)
				{
					//GL.Color4(f, f1, f4, 1.0F);
				}

				RenderTexturedQuad(par6, par7, (par5 % 16) * 16, (par5 / 16) * 16, 16, 16);
				//GL.Enable(EnableCap.Lighting);
			}

			//GL.Enable(EnableCap.CullFace);
		}

		/// <summary>
		/// Renders the item's icon or block into the UI at the specified position.
		/// </summary>
		public virtual void RenderItemIntoGUI(FontRenderer par1FontRenderer, RenderEngineOld par2RenderEngine, ItemStack par3ItemStack, int par4, int par5)
		{
			if (par3ItemStack == null)
			{
				return;
			}

			DrawItemIntoGui(par1FontRenderer, par2RenderEngine, par3ItemStack.ItemID, par3ItemStack.GetItemDamage(), par3ItemStack.GetIconIndex(), par4, par5);

			if (par3ItemStack != null && par3ItemStack.HasEffect())
			{
				//GL.DepthFunc(DepthFunction.Greater);
				//GL.Disable(EnableCap.Lighting);
				//GL.DepthMask(false);
				par2RenderEngine.BindTexture(par2RenderEngine.GetTexture("%blur%/misc/glint.png"));
				ZLevel -= 50F;
				//GL.Enable(EnableCap.Blend);
				//GL.BlendFunc(BlendingFactorSrc.DstColor, BlendingFactorDest.DstColor);
				//GL.Color4(0.5F, 0.25F, 0.8F, 1.0F);
				Func_40266_a(par4 * 0x19b4ca14 + par5 * 0x1eafff1, par4 - 2, par5 - 2, 20, 20);
				//GL.Disable(EnableCap.Blend);
				//GL.DepthMask(true);
				ZLevel += 50F;
				//GL.Enable(EnableCap.Lighting);
				//GL.DepthFunc(DepthFunction.Lequal);
			}
		}

		private void Func_40266_a(int par1, int par2, int par3, int par4, int par5)
		{
			for (int i = 0; i < 2; i++)
			{
				if (i == 0)
				{
					//GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.One);
				}

				if (i == 1)
				{
					//GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.One);
				}

				float f = 0.00390625F;
				float f1 = 0.00390625F;
				float f2 = ((float)(JavaHelper.CurrentTimeMillis() % (long)(3000 + i * 1873)) / (3000F + (float)(i * 1873))) * 256F;
				float f3 = 0.0F;
				Tessellator tessellator = Tessellator.Instance;
				float f4 = 4F;

				if (i == 1)
				{
					f4 = -1F;
				}

				tessellator.StartDrawingQuads();
				tessellator.AddVertexWithUV(par2 + 0, par3 + par5, ZLevel, (f2 + (float)par5 * f4) * f, (f3 + (float)par5) * f1);
				tessellator.AddVertexWithUV(par2 + par4, par3 + par5, ZLevel, (f2 + (float)par4 + (float)par5 * f4) * f, (f3 + (float)par5) * f1);
				tessellator.AddVertexWithUV(par2 + par4, par3 + 0, ZLevel, (f2 + (float)par4) * f, (f3 + 0.0F) * f1);
				tessellator.AddVertexWithUV(par2 + 0, par3 + 0, ZLevel, (f2 + 0.0F) * f, (f3 + 0.0F) * f1);
				tessellator.Draw();
			}
		}

		/// <summary>
		/// Renders the item's overlay information. Examples being stack count or damage on top of the item's image at the
		/// specified position.
		/// </summary>
		public virtual void RenderItemOverlayIntoGUI(FontRenderer par1FontRenderer, RenderEngineOld par2RenderEngine, ItemStack par3ItemStack, int par4, int par5)
		{
			if (par3ItemStack == null)
			{
				return;
			}

			if (par3ItemStack.StackSize > 1)
			{
				string s = (new StringBuilder()).Append("").Append(par3ItemStack.StackSize).ToString();
				//GL.Disable(EnableCap.Lighting);
				//GL.Disable(EnableCap.DepthTest);
                par1FontRenderer.DrawStringWithShadow(s, (par4 + 19) - 2 - (int)par1FontRenderer.GetStringWidth(s), par5 + 6 + 3, 0xffffff);
				//GL.Enable(EnableCap.Lighting);
				//GL.Enable(EnableCap.DepthTest);
			}

			if (par3ItemStack.IsItemDamaged())
			{
				int i = (int)Math.Round(13D - ((double)par3ItemStack.GetItemDamageForDisplay() * 13D) / (double)par3ItemStack.GetMaxDamage());
				int j = (int)Math.Round(255D - ((double)par3ItemStack.GetItemDamageForDisplay() * 255D) / (double)par3ItemStack.GetMaxDamage());
				//GL.Disable(EnableCap.Lighting);
				//GL.Disable(EnableCap.DepthTest);
				//GL.Disable(EnableCap.Texture2D);
				Tessellator tessellator = Tessellator.Instance;
				int k = 255 - j << 16 | j << 8;
				int l = (255 - j) / 4 << 16 | 0x3f00;
				RenderQuad(tessellator, par4 + 2, par5 + 13, 13, 2, 0);
				RenderQuad(tessellator, par4 + 2, par5 + 13, 12, 1, l);
				RenderQuad(tessellator, par4 + 2, par5 + 13, i, 1, k);
				//GL.Enable(EnableCap.Texture2D);
				//GL.Enable(EnableCap.Lighting);
				//GL.Enable(EnableCap.DepthTest);
				//GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
			}
		}

		/// <summary>
		/// Adds a quad to the tesselator at the specified position with the set width and height and color.  Args:
		/// tessellator, x, y, width, height, color
		/// </summary>
		private void RenderQuad(Tessellator par1Tessellator, int par2, int par3, int par4, int par5, int par6)
		{
			par1Tessellator.StartDrawingQuads();
			par1Tessellator.SetColorOpaque_I(par6);
			par1Tessellator.AddVertex(par2 + 0, par3 + 0, 0.0F);
			par1Tessellator.AddVertex(par2 + 0, par3 + par5, 0.0F);
			par1Tessellator.AddVertex(par2 + par4, par3 + par5, 0.0F);
			par1Tessellator.AddVertex(par2 + par4, par3 + 0, 0.0F);
			par1Tessellator.Draw();
		}

		/// <summary>
		/// Adds a textured quad to the tesselator at the specified position with the specified texture coords, width and
		/// height.  Args: x, y, u, v, width, height
		/// </summary>
		public virtual void RenderTexturedQuad(int par1, int par2, int par3, int par4, int par5, int par6)
		{
			float f = 0.00390625F;
			float f1 = 0.00390625F;
			Tessellator tessellator = Tessellator.Instance;
			tessellator.StartDrawingQuads();
			tessellator.AddVertexWithUV(par1 + 0, par2 + par6, ZLevel, (float)(par3 + 0) * f, (float)(par4 + par6) * f1);
			tessellator.AddVertexWithUV(par1 + par5, par2 + par6, ZLevel, (float)(par3 + par5) * f, (float)(par4 + par6) * f1);
			tessellator.AddVertexWithUV(par1 + par5, par2 + 0, ZLevel, (float)(par3 + par5) * f, (float)(par4 + 0) * f1);
			tessellator.AddVertexWithUV(par1 + 0, par2 + 0, ZLevel, (float)(par3 + 0) * f, (float)(par4 + 0) * f1);
			tessellator.Draw();
		}

		/// <summary>
		/// Actually renders the given argument. This is a synthetic bridge method, always casting down its argument and then
		/// handing it off to a worker function which does the actual work. In all probabilty, the class Render is generic
		/// (Render<T extends Entity) and this method has signature public void doRender(T entity, double d, double d1,
		/// double d2, float f, float f1). But JAD is pre 1.5 so doesn't do that.
		/// </summary>
		public override void DoRender(Entity par1Entity, double par2, double par4, double par6, float par8, float par9)
		{
			DoRenderItem((EntityItem)par1Entity, par2, par4, par6, par8, par9);
		}
	}
}