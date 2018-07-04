using Microsoft.Xna.Framework;

namespace net.minecraft.src
{
	public class TileEntityRendererPiston : TileEntitySpecialRenderer
	{
		/// <summary>
		/// instance of RenderBlocks used to draw the piston base and extension. </summary>
		private RenderBlocks BlockRenderer;

		public TileEntityRendererPiston()
		{
		}

		public virtual void RenderPiston(TileEntityPiston par1TileEntityPiston, double par2, double par4, double par6, float par8)
		{
			Block block = Block.BlocksList[par1TileEntityPiston.GetStoredBlockID()];

			if (block != null && par1TileEntityPiston.GetProgress(par8) < 1.0F)
			{
				Tessellator tessellator = Tessellator.Instance;
				BindTextureByName("/terrain.png");
				RenderHelper.DisableStandardItemLighting();
				//GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
				//GL.Enable(EnableCap.Blend);
				//GL.Disable(EnableCap.CullFace);

				if (Minecraft.IsAmbientOcclusionEnabled())
				{
					//GL.ShadeModel(ShadingModel.Smooth);
				}
				else
				{
					//GL.ShadeModel(ShadingModel.Flat);
				}

				tessellator.StartDrawingQuads();
				tessellator.SetTranslation(((float)par2 - (float)par1TileEntityPiston.XCoord) + par1TileEntityPiston.GetOffsetX(par8), ((float)par4 - (float)par1TileEntityPiston.YCoord) + par1TileEntityPiston.GetOffsetY(par8), ((float)par6 - (float)par1TileEntityPiston.ZCoord) + par1TileEntityPiston.GetOffsetZ(par8));
				tessellator.SetColorOpaque(1, 1, 1);

				if (block == Block.PistonExtension && par1TileEntityPiston.GetProgress(par8) < 0.5F)
				{
					BlockRenderer.RenderPistonExtensionAllFaces(block, par1TileEntityPiston.XCoord, par1TileEntityPiston.YCoord, par1TileEntityPiston.ZCoord, false);
				}
				else if (par1TileEntityPiston.ShouldRenderHead() && !par1TileEntityPiston.IsExtending())
				{
					Block.PistonExtension.SetHeadTexture(((BlockPistonBase)block).GetPistonExtensionTexture());
					BlockRenderer.RenderPistonExtensionAllFaces(Block.PistonExtension, par1TileEntityPiston.XCoord, par1TileEntityPiston.YCoord, par1TileEntityPiston.ZCoord, par1TileEntityPiston.GetProgress(par8) < 0.5F);
					Block.PistonExtension.ClearHeadTexture();
					tessellator.SetTranslation((float)par2 - (float)par1TileEntityPiston.XCoord, (float)par4 - (float)par1TileEntityPiston.YCoord, (float)par6 - (float)par1TileEntityPiston.ZCoord);
					BlockRenderer.RenderPistonBaseAllFaces(block, par1TileEntityPiston.XCoord, par1TileEntityPiston.YCoord, par1TileEntityPiston.ZCoord);
				}
				else
				{
					BlockRenderer.RenderBlockAllFaces(block, par1TileEntityPiston.XCoord, par1TileEntityPiston.YCoord, par1TileEntityPiston.ZCoord);
				}

				tessellator.SetTranslation(0.0F, 0.0F, 0.0F);
				tessellator.Draw();
				RenderHelper.EnableStandardItemLighting();
			}
		}

		/// <summary>
		/// Called from TileEntityRenderer.cacheSpecialRenderInfo() to cache render-related references (currently world
		/// only). Used by TileEntityRendererPiston to create and store a RenderBlocks instance in the blockRenderer field.
		/// </summary>
		public override void CacheSpecialRenderInfo(World par1World)
		{
			BlockRenderer = new RenderBlocks(par1World);
		}

        public override void RenderTileEntityAt(TileEntity par1TileEntity, float par2, float par4, float par6, float par8)
		{
			RenderPiston((TileEntityPiston)par1TileEntity, par2, par4, par6, par8);
		}
	}
}