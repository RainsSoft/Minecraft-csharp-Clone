using System;
using System.Text;
using Microsoft.Xna.Framework;
using net.minecraft.src;

namespace net.minecraft.src
{
	public class GuiEnchantment : GuiContainer
	{
		/// <summary>
		/// The book model used on the GUI. </summary>
		private static ModelBook BookModel = new ModelBook();
		private Random Field_40230_x;

		/// <summary>
		/// ContainerEnchantment object associated with this gui </summary>
		private ContainerEnchantment ContainerEnchantment;
		public int Field_40227_h;
		public float Field_40229_i;
		public float Field_40225_j;
		public float Field_40226_k;
		public float Field_40223_l;
		public float Field_40224_m;
		public float Field_40221_n;
		ItemStack Field_40222_o;

		public GuiEnchantment(InventoryPlayer par1InventoryPlayer, World par2World, int par3, int par4, int par5) : base(new ContainerEnchantment(par1InventoryPlayer, par2World, par3, par4, par5))
		{
			Field_40230_x = new Random();
			ContainerEnchantment = (ContainerEnchantment)InventorySlots;
		}

		/// <summary>
		/// Called when the screen is unloaded. Used to disable keyboard repeat events
		/// </summary>
		public override void OnGuiClosed()
		{
			base.OnGuiClosed();
		}

		/// <summary>
		/// Draw the foreground layer for the GuiContainer (everythin in front of the items)
		/// </summary>
		protected override void DrawGuiContainerForegroundLayer()
		{
			FontRenderer.DrawString(StatCollector.TranslateToLocal("container.enchant"), 12, 6, 0x404040);
			FontRenderer.DrawString(StatCollector.TranslateToLocal("container.Inventory"), 8, (YSize - 96) + 2, 0x404040);
		}

		/// <summary>
		/// Called from the main game loop to update the screen.
		/// </summary>
		public override void UpdateScreen()
		{
			base.UpdateScreen();
			Func_40219_x_();
		}

		/// <summary>
		/// Called when the mouse is clicked.
		/// </summary>
		protected override void MouseClicked(int par1, int par2, int par3)
		{
			base.MouseClicked(par1, par2, par3);
			int i = (Width - XSize) / 2;
			int j = (Height - YSize) / 2;

			for (int k = 0; k < 3; k++)
			{
				int l = par1 - (i + 60);
				int i1 = par2 - (j + 14 + 19 * k);

				if (l >= 0 && i1 >= 0 && l < 108 && i1 < 19 && ContainerEnchantment.EnchantItem(Mc.ThePlayer, k))
				{
					Mc.PlayerController.Func_40593_a(ContainerEnchantment.WindowId, k);
				}
			}
		}

		/// <summary>
		/// Draw the background layer for the GuiContainer (everything behind the items)
		/// </summary>
		protected override void DrawGuiContainerBackgroundLayer(float par1, int par2, int par3)
		{
			int i = Mc.RenderEngineOld.GetTexture("/gui/enchant.png");
			//GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
			Mc.RenderEngineOld.BindTexture(i);
			int j = (Width - XSize) / 2;
			int k = (Height - YSize) / 2;
			DrawTexturedModalRect(j, k, 0, 0, XSize, YSize);
			//GL.PushMatrix();
			//GL.MatrixMode(MatrixMode.Projection);
			//GL.PushMatrix();
			//GL.LoadIdentity();
			ScaledResolution scaledresolution = new ScaledResolution(Mc.GameSettings, Mc.DisplayWidth, Mc.DisplayHeight);
			//GL.Viewport(((scaledresolution.GetScaledWidth() - 320) / 2) * scaledresolution.ScaleFactor, ((scaledresolution.GetScaledHeight() - 240) / 2) * scaledresolution.ScaleFactor, 320 * scaledresolution.ScaleFactor, 240 * scaledresolution.ScaleFactor);
			//GL.Translate(-0.34F, 0.23F, 0.0F);
			Matrix.CreatePerspectiveFieldOfView(90F, 1.333333F, 9F, 80F);
			float f = 1.0F;
			//GL.MatrixMode(MatrixMode.Modelview);
			//GL.LoadIdentity();
			RenderHelper.EnableStandardItemLighting();
			//GL.Translate(0.0F, 3.3F, -16F);
			//GL.Scale(f, f, f);
			float f1 = 5F;
			//GL.Scale(f1, f1, f1);
			//GL.Rotate(180F, 0.0F, 0.0F, 1.0F);
			Mc.RenderEngineOld.BindTexture(Mc.RenderEngineOld.GetTexture("/item/book.png"));
			//GL.Rotate(20F, 1.0F, 0.0F, 0.0F);
			float f2 = Field_40221_n + (Field_40224_m - Field_40221_n) * par1;
			//GL.Translate((1.0F - f2) * 0.2F, (1.0F - f2) * 0.1F, (1.0F - f2) * 0.25F);
			//GL.Rotate(-(1.0F - f2) * 90F - 90F, 0.0F, 1.0F, 0.0F);
			//GL.Rotate(180F, 1.0F, 0.0F, 0.0F);
			float f3 = Field_40225_j + (Field_40229_i - Field_40225_j) * par1 + 0.25F;
			float f4 = Field_40225_j + (Field_40229_i - Field_40225_j) * par1 + 0.75F;
			f3 = (f3 - (float)MathHelper2.Func_40346_b(f3)) * 1.6F - 0.3F;
			f4 = (f4 - (float)MathHelper2.Func_40346_b(f4)) * 1.6F - 0.3F;

			if (f3 < 0.0F)
			{
				f3 = 0.0F;
			}

			if (f4 < 0.0F)
			{
				f4 = 0.0F;
			}

			if (f3 > 1.0F)
			{
				f3 = 1.0F;
			}

			if (f4 > 1.0F)
			{
				f4 = 1.0F;
			}

			//GL.Enable(EnableCap.RescaleNormal);
			BookModel.Render(null, 0.0F, f3, f4, f2, 0.0F, 0.0625F);
			//GL.Disable(EnableCap.RescaleNormal);
			RenderHelper.DisableStandardItemLighting();
			//GL.MatrixMode(MatrixMode.Projection);
			//GL.Viewport(0, 0, Mc.DisplayWidth, Mc.DisplayHeight);
			//GL.PopMatrix();
			//GL.MatrixMode(MatrixMode.Modelview);
			//GL.PopMatrix();
			RenderHelper.DisableStandardItemLighting();
			//GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
			Mc.RenderEngineOld.BindTexture(i);
			EnchantmentNameParts.Instance.SetRandSeed(ContainerEnchantment.NameSeed);

			for (int l = 0; l < 3; l++)
			{
				string s = EnchantmentNameParts.Instance.GenerateRandomEnchantName();
				ZLevel = 0.0F;
				Mc.RenderEngineOld.BindTexture(i);
				int i1 = ContainerEnchantment.EnchantLevels[l];
				//GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);

				if (i1 == 0)
				{
					DrawTexturedModalRect(j + 60, k + 14 + 19 * l, 0, 185, 108, 19);
					continue;
				}

				string s1 = (new StringBuilder()).Append("").Append(i1).ToString();
				FontRendererOld fontrenderer = Mc.StandardGalacticFontRenderer;
				int j1 = 0x685e4a;

				if (Mc.ThePlayer.ExperienceLevel < i1 && !Mc.ThePlayer.Capabilities.IsCreativeMode)
				{
					DrawTexturedModalRect(j + 60, k + 14 + 19 * l, 0, 185, 108, 19);
					fontrenderer.DrawSplitString(s, j + 62, k + 16 + 19 * l, 104, (j1 & 0xfefefe) >> 1);
					fontrenderer = Mc.FontRendererOld;
					j1 = 0x407f10;
					fontrenderer.DrawStringWithShadow(s1, (j + 62 + 104) - fontrenderer.GetStringWidth(s1), k + 16 + 19 * l + 7, j1);
					continue;
				}

				int k1 = par2 - (j + 60);
				int l1 = par3 - (k + 14 + 19 * l);

				if (k1 >= 0 && l1 >= 0 && k1 < 108 && l1 < 19)
				{
					DrawTexturedModalRect(j + 60, k + 14 + 19 * l, 0, 204, 108, 19);
					j1 = 0xffff80;
				}
				else
				{
					DrawTexturedModalRect(j + 60, k + 14 + 19 * l, 0, 166, 108, 19);
				}

				fontrenderer.DrawSplitString(s, j + 62, k + 16 + 19 * l, 104, j1);
				fontrenderer = Mc.FontRendererOld;
				j1 = 0x80ff20;
				fontrenderer.DrawStringWithShadow(s1, (j + 62 + 104) - fontrenderer.GetStringWidth(s1), k + 16 + 19 * l + 7, j1);
			}
		}

		public virtual void Func_40219_x_()
		{
			ItemStack itemstack = InventorySlots.GetSlot(0).GetStack();

			if (!ItemStack.AreItemStacksEqual(itemstack, Field_40222_o))
			{
				Field_40222_o = itemstack;

				do
				{
					Field_40226_k += Field_40230_x.Next(4) - Field_40230_x.Next(4);
				}
				while (Field_40229_i <= Field_40226_k + 1.0F && Field_40229_i >= Field_40226_k - 1.0F);
			}

			Field_40227_h++;
			Field_40225_j = Field_40229_i;
			Field_40221_n = Field_40224_m;
			bool flag = false;

			for (int i = 0; i < 3; i++)
			{
				if (ContainerEnchantment.EnchantLevels[i] != 0)
				{
					flag = true;
				}
			}

			if (flag)
			{
				Field_40224_m += 0.2F;
			}
			else
			{
				Field_40224_m -= 0.2F;
			}

			if (Field_40224_m < 0.0F)
			{
				Field_40224_m = 0.0F;
			}

			if (Field_40224_m > 1.0F)
			{
				Field_40224_m = 1.0F;
			}

			float f = (Field_40226_k - Field_40229_i) * 0.4F;
			float f1 = 0.2F;

			if (f < -f1)
			{
				f = -f1;
			}

			if (f > f1)
			{
				f = f1;
			}

			Field_40223_l += (f - Field_40223_l) * 0.9F;
			Field_40229_i = Field_40229_i + Field_40223_l;
		}
	}
}