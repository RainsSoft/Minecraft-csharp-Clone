namespace net.minecraft.src
{

	public interface ICamera
	{
		/// <summary>
		/// Returns true if the bounding box is inside all 6 clipping planes, otherwise returns false.
		/// </summary>
		bool IsBoundingBoxInFrustum(AxisAlignedBB axisalignedbb);

		void SetPosition(double d, double d1, double d2);
	}

}