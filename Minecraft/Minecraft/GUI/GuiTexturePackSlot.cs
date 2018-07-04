using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace net.minecraft.src
{
	class GuiTexturePackSlot : GuiSlot
	{
		readonly GuiTexturePacks ParentTexturePackGui;

        public GuiTexturePackSlot(GuiTexturePacks par1GuiTexturePacks)
            : base(GuiTexturePacks.GetMinecraft(par1GuiTexturePacks), par1GuiTexturePacks.Width, par1GuiTexturePacks.Height, 32, (par1GuiTexturePacks.Height - 55) + 4, 36)
		{
			ParentTexturePackGui = par1GuiTexturePacks;
		}

		/// <summary>
		/// Gets the size of the current slot list.
		/// </summary>
        public override int GetSize()
		{
            List<TexturePackBase> list = GuiTexturePacks.GetMinecraft(ParentTexturePackGui).TexturePackList.AvailableTexturePacks();
			return list.Count;
		}

		/// <summary>
		/// the element in the slot that was clicked, bool for wether it was double clicked or not
		/// </summary>
		protected override void ElementClicked(int par1, bool par2)
		{
            List<TexturePackBase> list = GuiTexturePacks.GetMinecraft(ParentTexturePackGui).TexturePackList.AvailableTexturePacks();

            try
            {
                GuiTexturePacks.GetMinecraft(ParentTexturePackGui).TexturePackList.SetTexturePack(list[par1]);
                GuiTexturePacks.GetMinecraft(ParentTexturePackGui).RenderEngineOld.RefreshTextures();
            }
            catch (Exception exception)
            {
                Utilities.LogException(exception);

                GuiTexturePacks.GetMinecraft(ParentTexturePackGui).TexturePackList.SetTexturePack(list[0]);
                GuiTexturePacks.GetMinecraft(ParentTexturePackGui).RenderEngineOld.RefreshTextures();
            }
		}

		/// <summary>
		/// returns true if the element passed in is currently selected
		/// </summary>
		protected override bool IsSelected(int par1)
		{
            List<TexturePackBase> list = GuiTexturePacks.GetMinecraft(ParentTexturePackGui).TexturePackList.AvailableTexturePacks();
            return GuiTexturePacks.GetMinecraft(ParentTexturePackGui).TexturePackList.SelectedTexturePack == list[par1];
		}

		/// <summary>
		/// return the height of the content being scrolled
		/// </summary>
		protected override int GetContentHeight()
		{
			return GetSize() * 36;
		}

		protected override void DrawBackground()
		{
			ParentTexturePackGui.DrawDefaultBackground();
		}

		protected override void DrawSlot(int par1, int par2, int par3, int par4, Tessellator par5Tessellator)
		{
            TexturePackBase texturepackbase = GuiTexturePacks.GetMinecraft(ParentTexturePackGui).TexturePackList.AvailableTexturePacks()[par1];
            texturepackbase.BindThumbnailTexture(GuiTexturePacks.GetMinecraft(ParentTexturePackGui));/*
			GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
			par5Tessellator.StartDrawingQuads();
			par5Tessellator.SetColorOpaque_I(0xffffff);
			par5Tessellator.AddVertexWithUV(par2,      par3 + par4, 0.0F, 0.0F, 1.0D);
			par5Tessellator.AddVertexWithUV(par2 + 32, par3 + par4, 0.0F, 1.0D, 1.0D);
			par5Tessellator.AddVertexWithUV(par2 + 32, par3,        0.0F, 1.0D, 0.0F);
			par5Tessellator.AddVertexWithUV(par2,      par3,        0.0F, 0.0F, 0.0F);
			par5Tessellator.Draw();*/
            RenderEngine.Instance.RenderSprite(new Rectangle(par2, par3, 32, par4), null);

            ParentTexturePackGui.DrawString(GuiTexturePacks.GetMinecraft(ParentTexturePackGui).FontRenderer, texturepackbase.TexturePackFileName, par2 + 32 + 2, par3 + 1, 0xffffff);
            ParentTexturePackGui.DrawString(GuiTexturePacks.GetMinecraft(ParentTexturePackGui).FontRenderer, texturepackbase.FirstDescriptionLine, par2 + 32 + 2, par3 + 12, 0x808080);
            ParentTexturePackGui.DrawString(GuiTexturePacks.GetMinecraft(ParentTexturePackGui).FontRenderer, texturepackbase.SecondDescriptionLine, par2 + 32 + 2, par3 + 12 + 10, 0x808080);
		}
	}
}