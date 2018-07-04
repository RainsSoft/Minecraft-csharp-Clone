namespace net.minecraft.src
{
	public class ClippingHelper
	{
		public float[][] Frustum;
		public float[] ProjectionMatrix;
		public float[] ModelviewMatrix;
		public float[] ClippingMatrix;

		public ClippingHelper()
		{
			Frustum = JavaHelper.ReturnRectangularArray<float>(16, 16);
			ProjectionMatrix = new float[16];
			ModelviewMatrix = new float[16];
			ClippingMatrix = new float[16];
		}

		/// <summary>
		/// Returns true if the box is inside all 6 clipping planes, otherwise returns false.
		/// </summary>
		public virtual bool IsBoxInFrustum(double par1, double par3, double par5, double par7, double par9, double par11)
		{
			for (int i = 0; i < 6; i++)
			{
				if ((double)Frustum[i][0] * par1 + (double)Frustum[i][1] * par3 + (double)Frustum[i][2] * par5 + (double)Frustum[i][3] <= 0.0F && (double)Frustum[i][0] * par7 + (double)Frustum[i][1] * par3 + (double)Frustum[i][2] * par5 + (double)Frustum[i][3] <= 0.0F && (double)Frustum[i][0] * par1 + (double)Frustum[i][1] * par9 + (double)Frustum[i][2] * par5 + (double)Frustum[i][3] <= 0.0F && (double)Frustum[i][0] * par7 + (double)Frustum[i][1] * par9 + (double)Frustum[i][2] * par5 + (double)Frustum[i][3] <= 0.0F && (double)Frustum[i][0] * par1 + (double)Frustum[i][1] * par3 + (double)Frustum[i][2] * par11 + (double)Frustum[i][3] <= 0.0F && (double)Frustum[i][0] * par7 + (double)Frustum[i][1] * par3 + (double)Frustum[i][2] * par11 + (double)Frustum[i][3] <= 0.0F && (double)Frustum[i][0] * par1 + (double)Frustum[i][1] * par9 + (double)Frustum[i][2] * par11 + (double)Frustum[i][3] <= 0.0F && (double)Frustum[i][0] * par7 + (double)Frustum[i][1] * par9 + (double)Frustum[i][2] * par11 + (double)Frustum[i][3] <= 0.0F)
				{
					return false;
				}
			}

			return true;
		}
	}
}