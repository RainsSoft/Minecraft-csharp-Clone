namespace net.minecraft.src
{
	using net.minecraft.src;
	using Microsoft.Xna.Framework;

	public class GuiCrafting : GuiContainer
	{
		public GuiCrafting(InventoryPlayer par1InventoryPlayer, World par2World, int par3, int par4, int par5) : base(new ContainerWorkbench(par1InventoryPlayer, par2World, par3, par4, par5))
		{
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
			FontRenderer.DrawString(StatCollector.TranslateToLocal("container.crafting"), 28, 6, 0x404040);
			FontRenderer.DrawString(StatCollector.TranslateToLocal("container.Inventory"), 8, (YSize - 96) + 2, 0x404040);
		}

		/// <summary>
		/// Draw the background layer for the GuiContainer (everything behind the items)
		/// </summary>
		protected override void DrawGuiContainerBackgroundLayer(float par1, int par2, int par3)
		{
			int i = Mc.RenderEngineOld.GetTexture("/gui/crafting.png");
			//GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
			Mc.RenderEngineOld.BindTexture(i);
			int j = (Width - XSize) / 2;
			int k = (Height - YSize) / 2;
			DrawTexturedModalRect(j, k, 0, 0, XSize, YSize);
		}
	}
}