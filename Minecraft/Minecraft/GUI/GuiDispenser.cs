namespace net.minecraft.src
{
	using net.minecraft.src;
	using Microsoft.Xna.Framework;

	public class GuiDispenser : GuiContainer
	{
		public GuiDispenser(InventoryPlayer par1InventoryPlayer, TileEntityDispenser par2TileEntityDispenser) : base(new ContainerDispenser(par1InventoryPlayer, par2TileEntityDispenser))
		{
		}

		/// <summary>
		/// Draw the foreground layer for the GuiContainer (everythin in front of the items)
		/// </summary>
		protected override void DrawGuiContainerForegroundLayer()
		{
			FontRenderer.DrawString(StatCollector.TranslateToLocal("container.dispenser"), 60, 6, 0x404040);
			FontRenderer.DrawString(StatCollector.TranslateToLocal("container.Inventory"), 8, (YSize - 96) + 2, 0x404040);
		}

		/// <summary>
		/// Draw the background layer for the GuiContainer (everything behind the items)
		/// </summary>
		protected override void DrawGuiContainerBackgroundLayer(float par1, int par2, int par3)
		{
			int i = Mc.RenderEngineOld.GetTexture("/gui/trap.png");
			//GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
			Mc.RenderEngineOld.BindTexture(i);
			int j = (Width - XSize) / 2;
			int k = (Height - YSize) / 2;
			DrawTexturedModalRect(j, k, 0, 0, XSize, YSize);
		}
	}
}