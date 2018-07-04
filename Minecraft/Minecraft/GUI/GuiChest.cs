namespace net.minecraft.src
{
    using net.minecraft.src;
    using Microsoft.Xna.Framework;

    public class GuiChest : GuiContainer
    {
        private IInventory upperChestInventory;
        private IInventory lowerChestInventory;

        /// <summary>
        /// window height is calculated with this values, the more rows, the heigher
        /// </summary>
        private int inventoryRows;

        public GuiChest(IInventory par1IInventory, IInventory par2IInventory)
            : base(new ContainerChest(par1IInventory, par2IInventory))
        {
            inventoryRows = 0;
            upperChestInventory = par1IInventory;
            lowerChestInventory = par2IInventory;
            AllowUserInput = false;
            int c = 336;
            int i = c - 108;
            inventoryRows = par2IInventory.GetSizeInventory() / 9;
            YSize = i + inventoryRows * 18;
        }

        ///<summary>
        /// Draw the foreground layer for the GuiContainer (everythin in front of the items)
        ///</summary>
        protected override void DrawGuiContainerForegroundLayer()
        {
            FontRenderer.DrawString(StatCollector.TranslateToLocal(lowerChestInventory.GetInvName()), 8, 6, 0x404040);
            FontRenderer.DrawString(StatCollector.TranslateToLocal(upperChestInventory.GetInvName()), 8, (YSize - 96) + 2, 0x404040);
        }

        ///<summary>
        /// Draw the background layer for the GuiContainer (everything behind the items)
        ///</summary>
        protected override void DrawGuiContainerBackgroundLayer(float par1, int par2, int par3)
        {
            int i = Mc.RenderEngineOld.GetTexture("/gui/container.png");
            //GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
            Mc.RenderEngineOld.BindTexture(i);
            int j = (Width - XSize) / 2;
            int k = (Height - YSize) / 2;
            DrawTexturedModalRect(j, k, 0, 0, XSize, inventoryRows * 18 + 17);
            DrawTexturedModalRect(j, k + inventoryRows * 18 + 17, 0, 126, XSize, 96);
        }
    }
}