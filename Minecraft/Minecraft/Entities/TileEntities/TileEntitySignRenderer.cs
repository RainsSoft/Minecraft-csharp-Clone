using System.Text;
using Microsoft.Xna.Framework;

namespace net.minecraft.src
{
	public class TileEntitySignRenderer : TileEntitySpecialRenderer
	{
		/// <summary>
		/// The ModelSign instance used by the TileEntitySignRenderer </summary>
		private ModelSign ModelSign;

		public TileEntitySignRenderer()
		{
			ModelSign = new ModelSign();
		}

		public virtual void RenderTileEntitySignAt(TileEntitySign par1TileEntitySign, double par2, double par4, double par6, float par8)
		{
			Block block = par1TileEntitySign.GetBlockType();
			//GL.PushMatrix();
			float f = 0.6666667F;

			if (block == Block.SignPost)
			{
				//GL.Translate((float)par2 + 0.5F, (float)par4 + 0.75F * f, (float)par6 + 0.5F);
				float f1 = (float)(par1TileEntitySign.GetBlockMetadata() * 360) / 16F;
				//GL.Rotate(-f1, 0.0F, 1.0F, 0.0F);
				ModelSign.SignStick.ShowModel = true;
			}
			else
			{
				int i = par1TileEntitySign.GetBlockMetadata();
				float f2 = 0.0F;

				if (i == 2)
				{
					f2 = 180F;
				}

				if (i == 4)
				{
					f2 = 90F;
				}

				if (i == 5)
				{
					f2 = -90F;
				}

				//GL.Translate((float)par2 + 0.5F, (float)par4 + 0.75F * f, (float)par6 + 0.5F);
				//GL.Rotate(-f2, 0.0F, 1.0F, 0.0F);
				//GL.Translate(0.0F, -0.3125F, -0.4375F);
				ModelSign.SignStick.ShowModel = false;
			}

			BindTextureByName("/item/sign.png");
			//GL.PushMatrix();
			//GL.Scale(f, -f, -f);
			ModelSign.RenderSign();
			//GL.PopMatrix();
			FontRenderer fontrenderer = GetFontRenderer();
			float f3 = 0.01666667F * f;
			//GL.Translate(0.0F, 0.5F * f, 0.07F * f);
			//GL.Scale(f3, -f3, f3);
			//GL.Normal3(0.0F, 0.0F, -1F * f3);
			//GL.DepthMask(false);
			int j = 0;

			for (int k = 0; k < par1TileEntitySign.SignText.Length; k++)
			{
				string s = par1TileEntitySign.SignText[k];

				if (k == par1TileEntitySign.LineBeingEdited)
				{
					s = (new StringBuilder()).Append("> ").Append(s).Append(" <").ToString();
					fontrenderer.DrawString(s, -fontrenderer.GetStringWidth(s) / 2, k * 10 - par1TileEntitySign.SignText.Length * 5, j);
				}
				else
				{
					fontrenderer.DrawString(s, -fontrenderer.GetStringWidth(s) / 2, k * 10 - par1TileEntitySign.SignText.Length * 5, j);
				}
			}

			//GL.DepthMask(true);
			//GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
			//GL.PopMatrix();
		}

        public override void RenderTileEntityAt(TileEntity par1TileEntity, float par2, float par4, float par6, float par8)
		{
			RenderTileEntitySignAt((TileEntitySign)par1TileEntity, par2, par4, par6, par8);
		}
	}
}