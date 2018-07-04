namespace net.minecraft.src
{

	public class ModelBox
	{
		private PositionTextureVertex[] VertexPositions;
		private TexturedQuad[] QuadList;

		/// <summary>
		/// X vertex coordinate of lower box corner </summary>
		public readonly float PosX1;

		/// <summary>
		/// Y vertex coordinate of lower box corner </summary>
		public readonly float PosY1;

		/// <summary>
		/// Z vertex coordinate of lower box corner </summary>
		public readonly float PosZ1;

		/// <summary>
		/// X vertex coordinate of upper box corner </summary>
		public readonly float PosX2;

		/// <summary>
		/// Y vertex coordinate of upper box corner </summary>
		public readonly float PosY2;

		/// <summary>
		/// Z vertex coordinate of upper box corner </summary>
		public readonly float PosZ2;
		public string Field_40673_g;

		public ModelBox(ModelRenderer par1ModelRenderer, int par2, int par3, float par4, float par5, float par6, int par7, int par8, int par9, float par10)
		{
			PosX1 = par4;
			PosY1 = par5;
			PosZ1 = par6;
			PosX2 = par4 + (float)par7;
			PosY2 = par5 + (float)par8;
			PosZ2 = par6 + (float)par9;
			VertexPositions = new PositionTextureVertex[8];
			QuadList = new TexturedQuad[6];
			float f = par4 + (float)par7;
			float f1 = par5 + (float)par8;
			float f2 = par6 + (float)par9;
			par4 -= par10;
			par5 -= par10;
			par6 -= par10;
			f += par10;
			f1 += par10;
			f2 += par10;

			if (par1ModelRenderer.Mirror)
			{
				float f3 = f;
				f = par4;
				par4 = f3;
			}

			PositionTextureVertex positiontexturevertex = new PositionTextureVertex(par4, par5, par6, 0.0F, 0.0F);
			PositionTextureVertex positiontexturevertex1 = new PositionTextureVertex(f, par5, par6, 0.0F, 8F);
			PositionTextureVertex positiontexturevertex2 = new PositionTextureVertex(f, f1, par6, 8F, 8F);
			PositionTextureVertex positiontexturevertex3 = new PositionTextureVertex(par4, f1, par6, 8F, 0.0F);
			PositionTextureVertex positiontexturevertex4 = new PositionTextureVertex(par4, par5, f2, 0.0F, 0.0F);
			PositionTextureVertex positiontexturevertex5 = new PositionTextureVertex(f, par5, f2, 0.0F, 8F);
			PositionTextureVertex positiontexturevertex6 = new PositionTextureVertex(f, f1, f2, 8F, 8F);
			PositionTextureVertex positiontexturevertex7 = new PositionTextureVertex(par4, f1, f2, 8F, 0.0F);
			VertexPositions[0] = positiontexturevertex;
			VertexPositions[1] = positiontexturevertex1;
			VertexPositions[2] = positiontexturevertex2;
			VertexPositions[3] = positiontexturevertex3;
			VertexPositions[4] = positiontexturevertex4;
			VertexPositions[5] = positiontexturevertex5;
			VertexPositions[6] = positiontexturevertex6;
			VertexPositions[7] = positiontexturevertex7;
			QuadList[0] = new TexturedQuad(new PositionTextureVertex[] { positiontexturevertex5, positiontexturevertex1, positiontexturevertex2, positiontexturevertex6 }, par2 + par9 + par7, par3 + par9, par2 + par9 + par7 + par9, par3 + par9 + par8, par1ModelRenderer.TextureWidth, par1ModelRenderer.TextureHeight);
			QuadList[1] = new TexturedQuad(new PositionTextureVertex[] { positiontexturevertex, positiontexturevertex4, positiontexturevertex7, positiontexturevertex3 }, par2, par3 + par9, par2 + par9, par3 + par9 + par8, par1ModelRenderer.TextureWidth, par1ModelRenderer.TextureHeight);
			QuadList[2] = new TexturedQuad(new PositionTextureVertex[] { positiontexturevertex5, positiontexturevertex4, positiontexturevertex, positiontexturevertex1 }, par2 + par9, par3, par2 + par9 + par7, par3 + par9, par1ModelRenderer.TextureWidth, par1ModelRenderer.TextureHeight);
			QuadList[3] = new TexturedQuad(new PositionTextureVertex[] { positiontexturevertex2, positiontexturevertex3, positiontexturevertex7, positiontexturevertex6 }, par2 + par9 + par7, par3 + par9, par2 + par9 + par7 + par7, par3, par1ModelRenderer.TextureWidth, par1ModelRenderer.TextureHeight);
			QuadList[4] = new TexturedQuad(new PositionTextureVertex[] { positiontexturevertex1, positiontexturevertex, positiontexturevertex3, positiontexturevertex2 }, par2 + par9, par3 + par9, par2 + par9 + par7, par3 + par9 + par8, par1ModelRenderer.TextureWidth, par1ModelRenderer.TextureHeight);
			QuadList[5] = new TexturedQuad(new PositionTextureVertex[] { positiontexturevertex4, positiontexturevertex5, positiontexturevertex6, positiontexturevertex7 }, par2 + par9 + par7 + par9, par3 + par9, par2 + par9 + par7 + par9 + par7, par3 + par9 + par8, par1ModelRenderer.TextureWidth, par1ModelRenderer.TextureHeight);

			if (par1ModelRenderer.Mirror)
			{
				for (int i = 0; i < QuadList.Length; i++)
				{
					QuadList[i].FlipFace();
				}
			}
		}

		/// <summary>
		/// Draw the six sided box defined by this ModelBox
		/// </summary>
		public virtual void Render(Tessellator par1Tessellator, float par2)
		{
			for (int i = 0; i < QuadList.Length; i++)
			{
				QuadList[i].Draw(par1Tessellator, par2);
			}
		}

		public virtual ModelBox Func_40671_a(string par1Str)
		{
			Field_40673_g = par1Str;
			return this;
		}
	}

}