
using Microsoft.Xna.Framework;

namespace net.minecraft.src
{
	public class ClippingHelperImpl : ClippingHelper
	{
		private static ClippingHelperImpl Instance = new ClippingHelperImpl();
		private Matrix ProjectionMatrixBuffer;
        private Matrix ModelviewMatrixBuffer;
		private Buffer<float> Field_1691_h;

		public ClippingHelperImpl()
		{
            ProjectionMatrixBuffer = Matrix.Identity;// GLAllocation.CreateDirectFloatBuffer(16);
            ModelviewMatrixBuffer = Matrix.Identity;// GLAllocation.CreateDirectFloatBuffer(16);
            Field_1691_h = new Buffer<float>(16);// GLAllocation.CreateDirectFloatBuffer(16);
		}

		/// <summary>
		/// Initialises the ClippingHelper object then returns an instance of it.
		/// </summary>
		public static ClippingHelper GetInstance()
		{
			Instance.Init();
			return Instance;
		}

		/// <summary>
		/// Normalize the frustum.
		/// </summary>
		private void Normalize(float[][] par1ArrayOfFloat, int par2)
		{
			float f = MathHelper2.Sqrt_float(par1ArrayOfFloat[par2][0] * par1ArrayOfFloat[par2][0] + par1ArrayOfFloat[par2][1] * par1ArrayOfFloat[par2][1] + par1ArrayOfFloat[par2][2] * par1ArrayOfFloat[par2][2]);
			par1ArrayOfFloat[par2][0] /= f;
			par1ArrayOfFloat[par2][1] /= f;
			par1ArrayOfFloat[par2][2] /= f;
			par1ArrayOfFloat[par2][3] /= f;
		}

		private void Init()
		{/*
			ProjectionMatrixBuffer.Clear();
			ModelviewMatrixBuffer.Clear();
			Field_1691_h.Clear();
			//GL.GetFloat(GetPName.ProjectionMatrix, out ProjectionMatrixBuffer);
			//GL.GetFloat(GetPName.ModelviewMatrix, out ModelviewMatrixBuffer);
			ProjectionMatrixBuffer.Flip().limit(16);
			ProjectionMatrixBuffer.Get(ProjectionMatrix);
			ModelviewMatrixBuffer.Flip().limit(16);
			ModelviewMatrixBuffer.Get(ModelviewMatrix);*/
			ClippingMatrix[0] = ModelviewMatrix[0] * ProjectionMatrix[0] + ModelviewMatrix[1] * ProjectionMatrix[4] + ModelviewMatrix[2] * ProjectionMatrix[8] + ModelviewMatrix[3] * ProjectionMatrix[12];
			ClippingMatrix[1] = ModelviewMatrix[0] * ProjectionMatrix[1] + ModelviewMatrix[1] * ProjectionMatrix[5] + ModelviewMatrix[2] * ProjectionMatrix[9] + ModelviewMatrix[3] * ProjectionMatrix[13];
			ClippingMatrix[2] = ModelviewMatrix[0] * ProjectionMatrix[2] + ModelviewMatrix[1] * ProjectionMatrix[6] + ModelviewMatrix[2] * ProjectionMatrix[10] + ModelviewMatrix[3] * ProjectionMatrix[14];
			ClippingMatrix[3] = ModelviewMatrix[0] * ProjectionMatrix[3] + ModelviewMatrix[1] * ProjectionMatrix[7] + ModelviewMatrix[2] * ProjectionMatrix[11] + ModelviewMatrix[3] * ProjectionMatrix[15];
			ClippingMatrix[4] = ModelviewMatrix[4] * ProjectionMatrix[0] + ModelviewMatrix[5] * ProjectionMatrix[4] + ModelviewMatrix[6] * ProjectionMatrix[8] + ModelviewMatrix[7] * ProjectionMatrix[12];
			ClippingMatrix[5] = ModelviewMatrix[4] * ProjectionMatrix[1] + ModelviewMatrix[5] * ProjectionMatrix[5] + ModelviewMatrix[6] * ProjectionMatrix[9] + ModelviewMatrix[7] * ProjectionMatrix[13];
			ClippingMatrix[6] = ModelviewMatrix[4] * ProjectionMatrix[2] + ModelviewMatrix[5] * ProjectionMatrix[6] + ModelviewMatrix[6] * ProjectionMatrix[10] + ModelviewMatrix[7] * ProjectionMatrix[14];
			ClippingMatrix[7] = ModelviewMatrix[4] * ProjectionMatrix[3] + ModelviewMatrix[5] * ProjectionMatrix[7] + ModelviewMatrix[6] * ProjectionMatrix[11] + ModelviewMatrix[7] * ProjectionMatrix[15];
			ClippingMatrix[8] = ModelviewMatrix[8] * ProjectionMatrix[0] + ModelviewMatrix[9] * ProjectionMatrix[4] + ModelviewMatrix[10] * ProjectionMatrix[8] + ModelviewMatrix[11] * ProjectionMatrix[12];
			ClippingMatrix[9] = ModelviewMatrix[8] * ProjectionMatrix[1] + ModelviewMatrix[9] * ProjectionMatrix[5] + ModelviewMatrix[10] * ProjectionMatrix[9] + ModelviewMatrix[11] * ProjectionMatrix[13];
			ClippingMatrix[10] = ModelviewMatrix[8] * ProjectionMatrix[2] + ModelviewMatrix[9] * ProjectionMatrix[6] + ModelviewMatrix[10] * ProjectionMatrix[10] + ModelviewMatrix[11] * ProjectionMatrix[14];
			ClippingMatrix[11] = ModelviewMatrix[8] * ProjectionMatrix[3] + ModelviewMatrix[9] * ProjectionMatrix[7] + ModelviewMatrix[10] * ProjectionMatrix[11] + ModelviewMatrix[11] * ProjectionMatrix[15];
			ClippingMatrix[12] = ModelviewMatrix[12] * ProjectionMatrix[0] + ModelviewMatrix[13] * ProjectionMatrix[4] + ModelviewMatrix[14] * ProjectionMatrix[8] + ModelviewMatrix[15] * ProjectionMatrix[12];
			ClippingMatrix[13] = ModelviewMatrix[12] * ProjectionMatrix[1] + ModelviewMatrix[13] * ProjectionMatrix[5] + ModelviewMatrix[14] * ProjectionMatrix[9] + ModelviewMatrix[15] * ProjectionMatrix[13];
			ClippingMatrix[14] = ModelviewMatrix[12] * ProjectionMatrix[2] + ModelviewMatrix[13] * ProjectionMatrix[6] + ModelviewMatrix[14] * ProjectionMatrix[10] + ModelviewMatrix[15] * ProjectionMatrix[14];
			ClippingMatrix[15] = ModelviewMatrix[12] * ProjectionMatrix[3] + ModelviewMatrix[13] * ProjectionMatrix[7] + ModelviewMatrix[14] * ProjectionMatrix[11] + ModelviewMatrix[15] * ProjectionMatrix[15];
			Frustum[0][0] = ClippingMatrix[3] - ClippingMatrix[0];
			Frustum[0][1] = ClippingMatrix[7] - ClippingMatrix[4];
			Frustum[0][2] = ClippingMatrix[11] - ClippingMatrix[8];
			Frustum[0][3] = ClippingMatrix[15] - ClippingMatrix[12];
			Normalize(Frustum, 0);
			Frustum[1][0] = ClippingMatrix[3] + ClippingMatrix[0];
			Frustum[1][1] = ClippingMatrix[7] + ClippingMatrix[4];
			Frustum[1][2] = ClippingMatrix[11] + ClippingMatrix[8];
			Frustum[1][3] = ClippingMatrix[15] + ClippingMatrix[12];
			Normalize(Frustum, 1);
			Frustum[2][0] = ClippingMatrix[3] + ClippingMatrix[1];
			Frustum[2][1] = ClippingMatrix[7] + ClippingMatrix[5];
			Frustum[2][2] = ClippingMatrix[11] + ClippingMatrix[9];
			Frustum[2][3] = ClippingMatrix[15] + ClippingMatrix[13];
			Normalize(Frustum, 2);
			Frustum[3][0] = ClippingMatrix[3] - ClippingMatrix[1];
			Frustum[3][1] = ClippingMatrix[7] - ClippingMatrix[5];
			Frustum[3][2] = ClippingMatrix[11] - ClippingMatrix[9];
			Frustum[3][3] = ClippingMatrix[15] - ClippingMatrix[13];
			Normalize(Frustum, 3);
			Frustum[4][0] = ClippingMatrix[3] - ClippingMatrix[2];
			Frustum[4][1] = ClippingMatrix[7] - ClippingMatrix[6];
			Frustum[4][2] = ClippingMatrix[11] - ClippingMatrix[10];
			Frustum[4][3] = ClippingMatrix[15] - ClippingMatrix[14];
			Normalize(Frustum, 4);
			Frustum[5][0] = ClippingMatrix[3] + ClippingMatrix[2];
			Frustum[5][1] = ClippingMatrix[7] + ClippingMatrix[6];
			Frustum[5][2] = ClippingMatrix[11] + ClippingMatrix[10];
			Frustum[5][3] = ClippingMatrix[15] + ClippingMatrix[14];
			Normalize(Frustum, 5);
		}
	}
}