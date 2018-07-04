namespace net.minecraft.src
{

	public class Frustrum : ICamera
	{
		private ClippingHelper ClippingHelper;
		private double XPosition;
		private double YPosition;
		private double ZPosition;

		public Frustrum()
		{
			ClippingHelper = ClippingHelperImpl.GetInstance();
		}

		public virtual void SetPosition(double par1, double par3, double par5)
		{
			XPosition = par1;
			YPosition = par3;
			ZPosition = par5;
		}

		/// <summary>
		/// Calls the clipping helper. Returns true if the box is inside all 6 clipping planes, otherwise returns false.
		/// </summary>
		public virtual bool IsBoxInFrustum(double par1, double par3, double par5, double par7, double par9, double par11)
		{
			return ClippingHelper.IsBoxInFrustum(par1 - XPosition, par3 - YPosition, par5 - ZPosition, par7 - XPosition, par9 - YPosition, par11 - ZPosition);
		}

		/// <summary>
		/// Returns true if the bounding box is inside all 6 clipping planes, otherwise returns false.
		/// </summary>
		public virtual bool IsBoundingBoxInFrustum(AxisAlignedBB par1AxisAlignedBB)
		{
			return IsBoxInFrustum(par1AxisAlignedBB.MinX, par1AxisAlignedBB.MinY, par1AxisAlignedBB.MinZ, par1AxisAlignedBB.MaxX, par1AxisAlignedBB.MaxY, par1AxisAlignedBB.MaxZ);
		}
	}

}