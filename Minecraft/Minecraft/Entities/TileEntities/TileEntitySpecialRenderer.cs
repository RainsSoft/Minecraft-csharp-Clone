namespace net.minecraft.src
{
	public abstract class TileEntitySpecialRenderer
	{
		/// <summary>
		/// The TileEntityRenderer instance associated with this TileEntitySpecialRenderer
		/// </summary>
		protected TileEntityRenderer TileEntityRenderer;

		public TileEntitySpecialRenderer()
		{
		}

        public abstract void RenderTileEntityAt(TileEntity tileentity, float d, float d1, float d2, float f);

		/// <summary>
		/// Binds a texture to the renderEngine given a filename from the JAR.
		/// </summary>
		protected virtual void BindTextureByName(string par1Str)
		{
			RenderEngine renderengine = TileEntityRenderer.RenderEngine;

			if (renderengine != null)
			{
				renderengine.BindTexture(par1Str);
			}
		}

		/// <summary>
		/// Associate a TileEntityRenderer with this TileEntitySpecialRenderer
		/// </summary>
		public virtual void SetTileEntityRenderer(TileEntityRenderer par1TileEntityRenderer)
		{
			TileEntityRenderer = par1TileEntityRenderer;
		}

		/// <summary>
		/// Called from TileEntityRenderer.cacheSpecialRenderInfo() to cache render-related references (currently world
		/// only). Used by TileEntityRendererPiston to create and store a RenderBlocks instance in the blockRenderer field.
		/// </summary>
		public virtual void CacheSpecialRenderInfo(World world)
		{
		}

		public virtual FontRenderer GetFontRenderer()
		{
			return TileEntityRenderer.GetFontRenderer();
		}
	}
}