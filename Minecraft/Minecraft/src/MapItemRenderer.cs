using System.Collections.Generic;
using System.Drawing;

namespace net.minecraft.src
{
    using Microsoft.Xna.Framework;

    public class MapItemRenderer
    {
        private int[] intArray;
        private int bufferedImage;
        private GameSettings gameSettings;
        private FontRendererOld fontRenderer;

        public MapItemRenderer(FontRendererOld par1FontRenderer, GameSettings par2GameSettings, RenderEngineOld par3RenderEngine)
        {
            intArray = new int[16384];
            gameSettings = par2GameSettings;
            fontRenderer = par1FontRenderer;
            bufferedImage = par3RenderEngine.AllocateAndSetupTexture(new Bitmap(128, 128));

            for (int i = 0; i < 16384; i++)
            {
                intArray[i] = 0;
            }
        }

        public virtual void RenderMap(EntityPlayer par1EntityPlayer, RenderEngineOld par2RenderEngine, MapData par3MapData)
        {
            for (int i = 0; i < 16384; i++)
            {
                byte byte0 = par3MapData.Colors[i];

                if (byte0 / 4 == 0)
                {
                    intArray[i] = (i + i / 128 & 1) * 8 + 16 << 24;
                    continue;
                }

                int l = MapColor.MapColorArray[byte0 / 4].ColorValue;
                int i1 = byte0 & 3;
                int c = 334;

                if (i1 == 2)
                {
                    c = 377;
                }

                if (i1 == 0)
                {
                    c = 264;
                }

                int j1 = ((l >> 16 & 0xff) * c) / 255;
                int k1 = ((l >> 8 & 0xff) * c) / 255;
                int l1 = ((l & 0xff) * c) / 255;

                if (gameSettings.Anaglyph)
                {
                    int i2 = (j1 * 30 + k1 * 59 + l1 * 11) / 100;
                    int j2 = (j1 * 30 + k1 * 70) / 100;
                    int k2 = (j1 * 30 + l1 * 70) / 100;
                    j1 = i2;
                    k1 = j2;
                    l1 = k2;
                }

                intArray[i] = 0xff00000 | j1 << 16 | k1 << 8 | l1;
            }

            par2RenderEngine.CreateTextureFromBytes(intArray, 128, 128, bufferedImage);
            int j = 0;
            int k = 0;
            Tessellator tessellator = Tessellator.Instance;
            float f = 0.0F;
            //GL.BindTexture(TextureTarget.Texture2D, bufferedImage);
            //GL.Enable(EnableCap.Blend);
            //GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.OneMinusSrcAlpha);
            //GL.Disable(EnableCap.AlphaTest);
            tessellator.StartDrawingQuads();
            tessellator.AddVertexWithUV((float)(j + 0) + f, (float)(k + 128) - f, -0.0099999997764825821D, 0.0F, 1.0D);
            tessellator.AddVertexWithUV((float)(j + 128) - f, (float)(k + 128) - f, -0.0099999997764825821D, 1.0D, 1.0D);
            tessellator.AddVertexWithUV((float)(j + 128) - f, (float)(k + 0) + f, -0.0099999997764825821D, 1.0D, 0.0F);
            tessellator.AddVertexWithUV((float)(j + 0) + f, (float)(k + 0) + f, -0.0099999997764825821D, 0.0F, 0.0F);
            tessellator.Draw();
            //GL.Enable(EnableCap.AlphaTest);
            //GL.Disable(EnableCap.Blend);
            par2RenderEngine.BindTexture(par2RenderEngine.GetTexture("/misc/mapicons.png"));
            /*
            for (IEnumerator<MapCoord> iterator = par3MapData.PlayersVisibleOnMap.GetEnumerator(); iterator.MoveNext(); GL.PopMatrix())
            {
                MapCoord mapcoord = iterator.Current;
                GL.PushMatrix();
                GL.Translate((float)j + (float)mapcoord.CenterX / 2.0F + 64F, (float)k + (float)mapcoord.CenterZ / 2.0F + 64F, -0.02F);
                GL.Rotate((float)(mapcoord.IconRotation * 360) / 16F, 0.0F, 0.0F, 1.0F);
                GL.Scale(4F, 4F, 3F);
                GL.Translate(-0.125F, 0.125F, 0.0F);
                float f1 = (float)(mapcoord.Field_28217_a % 4 + 0) / 4F;
                float f2 = (float)(mapcoord.Field_28217_a / 4 + 0) / 4F;
                float f3 = (float)(mapcoord.Field_28217_a % 4 + 1) / 4F;
                float f4 = (float)(mapcoord.Field_28217_a / 4 + 1) / 4F;
                tessellator.StartDrawingQuads();
                tessellator.AddVertexWithUV(-1D, 1.0D, 0.0F, f1, f2);
                tessellator.AddVertexWithUV(1.0D, 1.0D, 0.0F, f3, f2);
                tessellator.AddVertexWithUV(1.0D, -1D, 0.0F, f3, f4);
                tessellator.AddVertexWithUV(-1D, -1D, 0.0F, f1, f4);
                tessellator.Draw();
            }
            */
            //GL.PushMatrix();
            //GL.Translate(0.0F, 0.0F, -0.04F);
            //GL.Scale(1.0F, 1.0F, 1.0F);
            fontRenderer.DrawString(par3MapData.MapName, j, k, 0xff00000);
            //GL.PopMatrix();
        }
    }
}