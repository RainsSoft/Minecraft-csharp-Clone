namespace net.minecraft.src
{
	using net.minecraft.src;
	using Microsoft.Xna.Framework;

	public class GuiBrewingStand : GuiContainer
	{
		private TileEntityBrewingStand Field_40217_h;

		public GuiBrewingStand(InventoryPlayer par1InventoryPlayer, TileEntityBrewingStand par2TileEntityBrewingStand) : base(new ContainerBrewingStand(par1InventoryPlayer, par2TileEntityBrewingStand))
		{
			Field_40217_h = par2TileEntityBrewingStand;
		}

		/// <summary>
		/// Draw the foreground layer for the GuiContainer (everythin in front of the items)
		/// </summary>
		protected override void DrawGuiContainerForegroundLayer()
		{
			FontRenderer.DrawString(StatCollector.TranslateToLocal("container.brewing"), 56, 6, 0x404040);
			FontRenderer.DrawString(StatCollector.TranslateToLocal("container.Inventory"), 8, (YSize - 96) + 2, 0x404040);
		}

		/// <summary>
		/// Draw the background layer for the GuiContainer (everything behind the items)
		/// </summary>
		protected override void DrawGuiContainerBackgroundLayer(float par1, int par2, int par3)
		{
			int i = Mc.RenderEngineOld.GetTexture("/gui/alchemy.png");
			//GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
			Mc.RenderEngineOld.BindTexture(i);
			int j = (Width - XSize) / 2;
			int k = (Height - YSize) / 2;
			DrawTexturedModalRect(j, k, 0, 0, XSize, YSize);
			int l = Field_40217_h.GetBrewTime();

			if (l > 0)
			{
				int i1 = (int)(28F * (1.0F - (float)l / 400F));

				if (i1 > 0)
				{
					DrawTexturedModalRect(j + 97, k + 16, 176, 0, 9, i1);
				}

				int j1 = (l / 2) % 7;

				switch (j1)
				{
					case 6:
						i1 = 0;
						break;

					case 5:
						i1 = 6;
						break;

					case 4:
						i1 = 11;
						break;

					case 3:
						i1 = 16;
						break;

					case 2:
						i1 = 20;
						break;

					case 1:
						i1 = 24;
						break;

					case 0:
						i1 = 29;
						break;
				}

				if (i1 > 0)
				{
					DrawTexturedModalRect(j + 65, (k + 14 + 29) - i1, 185, 29 - i1, 12, i1);
				}
			}
		}
	}
}