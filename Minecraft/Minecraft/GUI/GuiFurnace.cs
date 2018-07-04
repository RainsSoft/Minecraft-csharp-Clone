namespace net.minecraft.src
{
	using net.minecraft.src;
	using Microsoft.Xna.Framework;

	public class GuiFurnace : GuiContainer
	{
		private TileEntityFurnace FurnaceInventory;

		public GuiFurnace(InventoryPlayer par1InventoryPlayer, TileEntityFurnace par2TileEntityFurnace) : base(new ContainerFurnace(par1InventoryPlayer, par2TileEntityFurnace))
		{
			FurnaceInventory = par2TileEntityFurnace;
		}

		/// <summary>
		/// Draw the foreground layer for the GuiContainer (everythin in front of the items)
		/// </summary>
		protected override void DrawGuiContainerForegroundLayer()
		{
			FontRenderer.DrawString(StatCollector.TranslateToLocal("container.furnace"), 60, 6, 0x404040);
			FontRenderer.DrawString(StatCollector.TranslateToLocal("container.Inventory"), 8, (YSize - 96) + 2, 0x404040);
		}

		/// <summary>
		/// Draw the background layer for the GuiContainer (everything behind the items)
		/// </summary>
		protected override void DrawGuiContainerBackgroundLayer(float par1, int par2, int par3)
		{
			int i = Mc.RenderEngineOld.GetTexture("/gui/furnace.png");
			//GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
			Mc.RenderEngineOld.BindTexture(i);
			int j = (Width - XSize) / 2;
			int k = (Height - YSize) / 2;
			DrawTexturedModalRect(j, k, 0, 0, XSize, YSize);

			if (FurnaceInventory.IsBurning())
			{
				int l = FurnaceInventory.GetBurnTimeRemainingScaled(12);
				DrawTexturedModalRect(j + 56, (k + 36 + 12) - l, 176, 12 - l, 14, l + 2);
			}

			int i1 = FurnaceInventory.GetCookProgressScaled(24);
			DrawTexturedModalRect(j + 79, k + 34, 176, 14, i1 + 1, 16);
		}
	}
}