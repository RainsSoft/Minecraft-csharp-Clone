namespace net.minecraft.src
{
	public class ChestItemRenderHelper
	{
		/// <summary>
		/// The static instance of ChestItemRenderHelper. </summary>
		public static ChestItemRenderHelper Instance = new ChestItemRenderHelper();
		private TileEntityChest Field_35610_b;

		public ChestItemRenderHelper()
		{
			Field_35610_b = new TileEntityChest();
		}

		public virtual void Func_35609_a(Block par1Block, int par2, float par3)
		{
			TileEntityRenderer.Instance.RenderTileEntityAt(Field_35610_b, 0.0F, 0.0F, 0.0F, 0.0F);
		}
	}
}