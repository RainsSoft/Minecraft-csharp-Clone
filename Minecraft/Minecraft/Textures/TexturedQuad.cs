namespace net.minecraft.src
{
	public class TexturedQuad
	{
		public PositionTextureVertex[] VertexPositions;
		public int NVertices;
		private bool InvertNormal;

		public TexturedQuad(PositionTextureVertex[] par1ArrayOfPositionTextureVertex)
		{
			NVertices = 0;
			InvertNormal = false;
			VertexPositions = par1ArrayOfPositionTextureVertex;
			NVertices = par1ArrayOfPositionTextureVertex.Length;
		}

		public TexturedQuad(PositionTextureVertex[] par1ArrayOfPositionTextureVertex, int par2, int par3, int par4, int par5, float par6, float par7) : this(par1ArrayOfPositionTextureVertex)
		{
			float f = 0.0F / par6;
			float f1 = 0.0F / par7;
			par1ArrayOfPositionTextureVertex[0] = par1ArrayOfPositionTextureVertex[0].SetTexturePosition((float)par4 / par6 - f, (float)par3 / par7 + f1);
			par1ArrayOfPositionTextureVertex[1] = par1ArrayOfPositionTextureVertex[1].SetTexturePosition((float)par2 / par6 + f, (float)par3 / par7 + f1);
			par1ArrayOfPositionTextureVertex[2] = par1ArrayOfPositionTextureVertex[2].SetTexturePosition((float)par2 / par6 + f, (float)par5 / par7 - f1);
			par1ArrayOfPositionTextureVertex[3] = par1ArrayOfPositionTextureVertex[3].SetTexturePosition((float)par4 / par6 - f, (float)par5 / par7 - f1);
		}

		public virtual void FlipFace()
		{
			PositionTextureVertex[] apositiontexturevertex = new PositionTextureVertex[VertexPositions.Length];

			for (int i = 0; i < VertexPositions.Length; i++)
			{
				apositiontexturevertex[i] = VertexPositions[VertexPositions.Length - i - 1];
			}

			VertexPositions = apositiontexturevertex;
		}

		public virtual void Draw(Tessellator par1Tessellator, float par2)
		{
			Vec3D vec3d = VertexPositions[1].Vector3D.Subtract(VertexPositions[0].Vector3D);
			Vec3D vec3d1 = VertexPositions[1].Vector3D.Subtract(VertexPositions[2].Vector3D);
			Vec3D vec3d2 = vec3d1.CrossProduct(vec3d).Normalize();
			par1Tessellator.StartDrawingQuads();

			if (InvertNormal)
			{
				par1Tessellator.SetNormal(-(float)vec3d2.XCoord, -(float)vec3d2.YCoord, -(float)vec3d2.ZCoord);
			}
			else
			{
				par1Tessellator.SetNormal((float)vec3d2.XCoord, (float)vec3d2.YCoord, (float)vec3d2.ZCoord);
			}

			for (int i = 0; i < 4; i++)
			{
				PositionTextureVertex positiontexturevertex = VertexPositions[i];
				par1Tessellator.AddVertexWithUV((float)positiontexturevertex.Vector3D.XCoord * par2, (float)positiontexturevertex.Vector3D.YCoord * par2, (float)positiontexturevertex.Vector3D.ZCoord * par2, positiontexturevertex.TexturePositionX, positiontexturevertex.TexturePositionY);
			}

			par1Tessellator.Draw();
		}
	}
}